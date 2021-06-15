using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.DTO
{
 public   class UserBranchDTO
    {

        public string UserId { get; set; }
        public long BranchId { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    
    }
}
