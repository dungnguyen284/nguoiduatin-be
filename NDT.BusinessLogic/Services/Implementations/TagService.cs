using AutoMapper;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessLogic.Services.Interfaces;
using NDT.BusinessModels.Entities;
using NDT.DataAccess.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDT.BusinessLogic.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagResponseDTO>> GetAllTagsAsync()
        {
            var tags = await _unitOfWork.Tags.GetAllAsync();
            return _mapper.Map<IEnumerable<TagResponseDTO>>(tags);
        }

        public async Task<TagResponseDTO> GetTagByIdAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
                return null;

            return _mapper.Map<TagResponseDTO>(tag);
        }

        public async Task<TagResponseDTO> CreateTagAsync(TagRequestDTO tagDto)
        {
            var tag = _mapper.Map<Tags>(tagDto);
            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.SaveChangesAsync();

            return await GetTagByIdAsync(tag.Id);
        }

        public async Task<TagResponseDTO> UpdateTagAsync(int id, TagRequestDTO tagDto)
        {
            var existingTag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (existingTag == null)
                return null;

            _mapper.Map(tagDto, existingTag);
            await _unitOfWork.Tags.UpdateAsync(existingTag);
            await _unitOfWork.SaveChangesAsync();

            return await GetTagByIdAsync(id);
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
                return false;

            await _unitOfWork.Tags.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
} 