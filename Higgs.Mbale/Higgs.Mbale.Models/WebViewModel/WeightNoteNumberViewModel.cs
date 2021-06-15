using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.WebViewModel
{
  public  class WeightNoteNumberViewModel
    {
        public long WeightNoteNumberId { get; set; }
        public double WeightNoteValue { get; set; }
        public long WeightNoteRangeId { get; set; }
        public long BranchId { get; set; }
        public bool Used { get; set; }
    }
}
