using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDT.API.CustomedResponses;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponseDTO>>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(new ApiResponse<IEnumerable<CategoryResponseDTO>>(200, "Categories retrieved successfully", categories));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<CategoryResponseDTO>>(400, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDTO>>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound(new ApiResponse<CategoryResponseDTO>(404, $"Category with ID {id} not found"));

                return Ok(new ApiResponse<CategoryResponseDTO>(200, "Category retrieved successfully", category));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CategoryResponseDTO>(400, ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponseDTO>>> CreateCategory(CategoryRequestDTO categoryDto)
        {
            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto);
                return CreatedAtAction(
                    nameof(GetCategoryById),
                    new { id = createdCategory.Id },
                    new ApiResponse<CategoryResponseDTO>(201, "Category created successfully", createdCategory)
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CategoryResponseDTO>(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CategoryResponseDTO>>> UpdateCategory(int id, CategoryRequestDTO categoryDto)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryDto);
                if (updatedCategory == null)
                    return NotFound(new ApiResponse<CategoryResponseDTO>(404, $"Category with ID {id} not found"));

                return Ok(new ApiResponse<CategoryResponseDTO>(200, "Category updated successfully", updatedCategory));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CategoryResponseDTO>(400, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                    return NotFound(new ApiResponse<bool>(404, $"Category with ID {id} not found"));

                return Ok(new ApiResponse<bool>(200, "Category deleted successfully", true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool>(400, ex.Message));
            }
        }
    }
} 