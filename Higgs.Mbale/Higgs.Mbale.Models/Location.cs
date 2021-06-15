using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class Location
    {
        public long LocationId { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string Initials { get; set; }
        public long RegionId { get; set; }
        public string RegionName { get; set; }

    }
}
