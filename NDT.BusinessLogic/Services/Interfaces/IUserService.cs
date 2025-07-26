using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetUsers();
        Task<UserResponseDTO> GetUserByIdAsync(string userId);
        Task<UserResponseDTO> UpdateUserAsync(string userId, UserUpdateRequestDTO userUpdateDto);
    }
}
