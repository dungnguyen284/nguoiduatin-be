using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDT.BusinessLogic.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagResponseDTO>> GetAllTagsAsync();
        Task<IEnumerable<AdminTagResponseDTO>> GetAllTagsForAdminAsync();

        Task<TagResponseDTO> GetTagByIdAsync(int id);
        Task<TagResponseDTO> CreateTagAsync(TagRequestDTO tagDto);
        Task<TagResponseDTO> UpdateTagAsync(int id, TagRequestDTO tagDto);
        Task<bool> DeleteTagAsync(int id);
    }
} 