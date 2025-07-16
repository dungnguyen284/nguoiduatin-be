using System.ComponentModel.DataAnnotations;

namespace NDT.BusinessLogic.DTOs.RequestDTOs
{
    public class StockRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string CompanyDescription { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
    }
} 