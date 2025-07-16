using System;

namespace NDT.BusinessLogic.DTOs.RequestDTOs
{
    public class UserRequestDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
} 