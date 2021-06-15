using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.WebViewModel
{
  public  class WeightNoteRangeViewModel
    {
        public long WeightNoteRangeId { get; set; }
        public double StartNumber { get; set; }
        public double EndNumber { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public bool Printed { get; set; }
    }
}
