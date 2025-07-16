using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessModels.Entities
{
    public class FrontPageNews
    {
        [Key]
        [Range(1, 4)]
        public int SlotNumber { get; set; }

        [Required]
        public int NewsId { get; set; }

        [ForeignKey(nameof(NewsId))]
        public News News { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
