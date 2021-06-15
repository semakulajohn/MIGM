using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.DTO
{
  public  class WeightNoteSupplyDTO
    {
        public long WeightNoteNumberId { get; set; }
        public long SupplyId { get; set; }
        public System.DateTime CreatedOn { get; set; }
    }
}
