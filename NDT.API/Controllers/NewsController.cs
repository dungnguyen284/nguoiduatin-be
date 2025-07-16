using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NDT.API.CustomedResponses;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessLogic.Services.Interfaces;

namespace NDT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsResponseDTO>>> GetAllNews()
        {
            try
            {
                var news = await _newsService.GetAllNewsAsync();
                return Ok(news);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<NewsResponseDTO>>> GetNewsById(int id)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var news = await _newsService.GetNewsByIdAsync(id, ip);
                if (news == null)
                    return NotFound(new ApiResponse<NewsResponseDTO>(404, $"News with ID {id} not found"));

                return Ok(new ApiResponse<NewsResponseDTO>(200, "News retrieved successfully", news));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NewsResponseDTO>(400, ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<NewsResponseDTO>>> CreateNews(NewsRequestDTO newsDto)
        {
            try
            {
                var createdNews = await _newsService.CreateNewsAsync(newsDto);
                return CreatedAtAction(
                    nameof(GetNewsById),
                    new { id = createdNews.Id },
                    new ApiResponse<NewsResponseDTO>(201, "News created successfully", createdNews)
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NewsResponseDTO>(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<NewsResponseDTO>>> UpdateNews(int id, NewsRequestDTO newsDto)
        {
            try
            {
                var updatedNews = await _newsService.UpdateNewsAsync(id, newsDto);
                if (updatedNews == null)
                    return NotFound(new ApiResponse<NewsResponseDTO>(404, $"News with ID {id} not found"));

                return Ok(new ApiResponse<NewsResponseDTO>(200, "News updated successfully", updatedNews));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NewsResponseDTO>(400, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteNews(int id)
        {
            try
            {
                var result = await _newsService.DeleteNewsAsync(id);
                if (!result)
                    return NotFound(new ApiResponse<bool>(404, $"News with ID {id} not found"));

                return Ok(new ApiResponse<bool>(200, "News deleted successfully", true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool>(400, ex.Message));
            }
        }
    }
} 