using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDT.BusinessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync();
        Task<CategoryResponseDTO> GetCategoryByIdAsync(int id);
        Task<CategoryResponseDTO> CreateCategoryAsync(CategoryRequestDTO categoryDto);
        Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CategoryRequestDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
} 