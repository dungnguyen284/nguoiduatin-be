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
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TagResponseDTO>>>> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(new ApiResponse<IEnumerable<TagResponseDTO>>(200, "Success", tags));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TagResponseDTO>>> GetTagById(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound(new ApiResponse<TagResponseDTO>(404, "Not found"));

            return Ok(new ApiResponse<TagResponseDTO>(200, "Success", tag));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<TagResponseDTO>>> CreateTag(TagRequestDTO tagDto)
        {
            var createdTag = await _tagService.CreateTagAsync(tagDto);
            return Ok(new ApiResponse<TagResponseDTO>(201, "Created", createdTag));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<TagResponseDTO>>> UpdateTag(int id, TagRequestDTO tagDto)
        {
            var updatedTag = await _tagService.UpdateTagAsync(id, tagDto);
            if (updatedTag == null)
                return NotFound(new ApiResponse<TagResponseDTO>(404, "Not found"));

            return Ok(new ApiResponse<TagResponseDTO>(200, "Updated", updatedTag));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTag(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);
            if (!result)
                return NotFound(new ApiResponse<object>(404, "Not found"));

            return Ok(new ApiResponse<object>(200, "Deleted"));
        }
    }
} 