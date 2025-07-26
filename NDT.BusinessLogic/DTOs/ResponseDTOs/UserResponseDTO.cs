using System;

namespace NDT.BusinessLogic.DTOs.ResponseDTOs
{
    public class UserResponseDTO
    {

        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int NewsCount { get; set; }
    }
} 