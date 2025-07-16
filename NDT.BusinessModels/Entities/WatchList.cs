using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessModels.Entities
{
    public class WatchList
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
