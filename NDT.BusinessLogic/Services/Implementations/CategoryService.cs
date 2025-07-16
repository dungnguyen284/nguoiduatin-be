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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
        }

        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return null;

            return _mapper.Map<CategoryResponseDTO>(category);
        }

        public async Task<CategoryResponseDTO> CreateCategoryAsync(CategoryRequestDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return await GetCategoryByIdAsync(category.Id);
        }

        public async Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CategoryRequestDTO categoryDto)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
                return null;

            _mapper.Map(categoryDto, existingCategory);
            await _unitOfWork.Categories.UpdateAsync(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            return await GetCategoryByIdAsync(id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return false;

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
} 