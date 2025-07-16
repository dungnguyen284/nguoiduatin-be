using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessModels.Entities
{
    public class Advertisement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; } // e.g., "Banner", "Sidebar", "Popup"
    }
}
