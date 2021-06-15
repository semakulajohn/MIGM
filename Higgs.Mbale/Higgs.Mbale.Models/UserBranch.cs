using System;
using System.Collections.Generic;

namespace Higgs.Mbale.Models
{
 public   class UserBranch
    {

        public string UserId { get; set; }
        public long BranchId { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public List<long> SelectedBranches { get; set; }
    
        public string BranchName { get; set; }
    }
}
