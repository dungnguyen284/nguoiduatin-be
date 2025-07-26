using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessModels.Entities;

namespace NDT.BusinessLogic.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsResponseDTO>> GetAllNewsAsync();
        Task<IEnumerable<News>> GetNewsByAuthorId(string aId);
        Task<NewsResponseDTO> GetNewsByIdAsync(int id, string ipAddress);
        Task<NewsResponseDTO> CreateNewsAsync(NewsRequestDTO newsDto);
        Task<NewsResponseDTO> UpdateNewsAsync(int id, NewsRequestDTO newsDto);
        Task<bool> UpdateFPNews(IEnumerable<FPNewsDTO> fPNews);
        Task<bool> DeleteNewsAsync(int id);
    }
}
