using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.BusinessModels.Entities
{
    public class Tags
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<News> News { get; set; } = new List<News>();
    }
}
