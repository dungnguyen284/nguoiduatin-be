using System;

namespace NDT.BusinessLogic.DTOs.ResponseDTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
} 