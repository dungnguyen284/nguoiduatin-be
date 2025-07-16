using System.ComponentModel.DataAnnotations;

namespace NDT.BusinessLogic.DTOs.RequestDTOs
{
    public class ForgotPasswordRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
} 