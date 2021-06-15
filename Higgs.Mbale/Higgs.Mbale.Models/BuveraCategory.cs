using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class BuveraCategory
    {
        public long BuveraCategoryId { get; set; }
        public string Name { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public bool Deleted { get; set; }
    }
}
