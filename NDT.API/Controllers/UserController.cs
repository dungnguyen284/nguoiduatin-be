using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessLogic.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NDT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> Get()
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetbyId(string id)
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDTO>> Update( UserUpdateRequestDTO userUpdateDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(userUpdateDto.Id, userUpdateDto);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

    }
}
