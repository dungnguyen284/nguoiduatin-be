using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessModels.Entities
{
    public enum NewsStatus
    {
        ACTIVE,
        INACTIVE,
        DRAFT
    }

    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        public string Link { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Source { get; set; } = "Người Đưa Tin";
        public NewsStatus Status { get; set; } = NewsStatus.ACTIVE;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; } = 0;

        public string AuthorId { get; set; }
        public AppUser Author { get; set; } = null!;

        public ICollection<Tags> Tags { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<FrontPageNews> FrontPageNews { get; set; }

    }
}
