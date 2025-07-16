using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessModels.Entities
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CompanyDescription { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
        public ICollection<WatchList> WatchLists { get; set; } = new List<WatchList>();
    }
}
