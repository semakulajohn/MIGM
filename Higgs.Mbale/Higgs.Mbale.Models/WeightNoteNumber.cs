using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class WeightNoteNumber
    {
        public long WeightNoteNumberId { get; set; }
        public double WeightNoteValue { get; set; }
        public long WeightNoteRangeId { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public bool Used { get; set; }
        public string Notes { get; set; }
        public bool NotUsed { get; set; }

        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    }
}
