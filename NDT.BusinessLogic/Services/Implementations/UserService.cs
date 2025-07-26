using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessLogic.Services.Interfaces;
using NDT.BusinessModels.Entities;
using NDT.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessLogic.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public Task<UserResponseDTO> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserResponseDTO>> GetUsers()
        {
            var users = await _unitOfWork.AppUser.GetAllAsync(includes: [u => u.News]);

            var result = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "NoRole";

                var dto = _mapper.Map<UserResponseDTO>(user);
                dto.Role = role;
                dto.NewsCount = user.News.Count;
                result.Add(dto);
            }

            return result;
        }

        public async Task<UserResponseDTO> UpdateUserAsync(string userId, UserUpdateRequestDTO userUpdateDto)
        {
            var user = await _unitOfWork.AppUser.FirstOrDefaultAsync(u => u.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);
            var removeRoles = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!removeRoles.Succeeded)
            {
                throw new Exception("Failed to remove roles from user");
            }
            var updateRoles = await _userManager.AddToRoleAsync(user, userUpdateDto.Role);
            if (!updateRoles.Succeeded)
            {
                throw new Exception("Failed to update user role");
            }
            var newUser = await _unitOfWork.AppUser.FirstOrDefaultAsync(u => u.Id == userId);
            return _mapper.Map<UserResponseDTO>(newUser);
        }
    }
    
}
