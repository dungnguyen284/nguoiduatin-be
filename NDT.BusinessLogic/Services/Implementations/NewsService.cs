using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessLogic.Services.Interfaces;
using NDT.BusinessModels.Entities;
using NDT.DataAccess.UnitOfWork;

namespace NDT.BusinessLogic.Services.Implementations
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // newsId -> (ip -> (date, count, lastViewTime))
        private static readonly Dictionary<int, Dictionary<string, (DateTime date, int count, DateTime lastViewTime)>> _newsViewIpCache = new();

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsResponseDTO>> GetAllNewsAsync()
        {
            var news = await _unitOfWork.News.GetAllAsync(includes: [
                    n => n.Author, 
                    n => n.Category, 
                    n => n.Tags,
                    n => n.FrontPageNews
                ]);
            
            var newsDto = _mapper.Map<IEnumerable<NewsResponseDTO>>(news);
            foreach (var item in newsDto)
            {
                item.IsFrontPage = news.FirstOrDefault(n => n.Id == item.Id)?.FrontPageNews.Count > 0;
                item.SlotNumber = news.FirstOrDefault(n => n.Id == item.Id)?.FrontPageNews.FirstOrDefault()?.SlotNumber ?? 0;
            }
            return newsDto;
        }

        public async Task<NewsResponseDTO> GetNewsByIdAsync(int id, string ipAddress)
        {
            var news = await _unitOfWork.News.FirstOrDefaultAsync(n => n.Id == id, [
                    n => n.Author,
                    n => n.Category,
                    n => n.Tags
                ]);

            if (news == null)
                return null;

            var now = DateTime.UtcNow;
            var today = now.Date;
            if (!_newsViewIpCache.ContainsKey(id))
                _newsViewIpCache[id] = new Dictionary<string, (DateTime, int, DateTime)>();
            if (!_newsViewIpCache[id].ContainsKey(ipAddress) || _newsViewIpCache[id][ipAddress].date != today)
                _newsViewIpCache[id][ipAddress] = (today, 0, DateTime.MinValue);

            var (date, count, lastViewTime) = _newsViewIpCache[id][ipAddress];
            if (count < 10 && (now - lastViewTime).TotalSeconds >= 5)
            {
                news.ViewCount++;
                _newsViewIpCache[id][ipAddress] = (today, count + 1, now);
                await _unitOfWork.News.UpdateAsync(news);
                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<NewsResponseDTO>(news);
        }

        public async Task<NewsResponseDTO> CreateNewsAsync(NewsRequestDTO newsDto)
        {
            var news = _mapper.Map<News>(newsDto);
            news.PublicationDate = DateTime.Now;

            // Handle tags
            if (newsDto.TagIds != null && newsDto.TagIds.Any())
            {
                var tags = await _unitOfWork.Tags.GetAllAsync(t => newsDto.TagIds.Contains(t.Id));
                news.Tags = tags.ToList();
            }

            await _unitOfWork.News.AddAsync(news);
            await _unitOfWork.SaveChangesAsync();

            return await GetNewsByIdAsync(news.Id, "0.0.0.0"); // Placeholder IP for creation
        }

        public async Task<NewsResponseDTO> UpdateNewsAsync(int id, NewsRequestDTO newsDto)
        {
            var existingNews = await _unitOfWork.News.FirstOrDefaultAsync(n => n.Id == id, [
                    n => n.Author,
                    n => n.Category,
                    n => n.Tags
                ]);

            if (existingNews == null)
                return null;

            _mapper.Map(newsDto, existingNews);

            // Update tags
            if (newsDto.TagIds != null)
            {
                var tags = await _unitOfWork.Tags.GetAllAsync(t => newsDto.TagIds.Contains(t.Id));
                existingNews.Tags = tags.ToList();
            }

            await _unitOfWork.SaveChangesAsync();

            return await GetNewsByIdAsync(id, "0.0.0.0"); // Placeholder IP for update
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _unitOfWork.News.GetByIdAsync(id);
            if (news == null)
                return false;

            await _unitOfWork.News.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
