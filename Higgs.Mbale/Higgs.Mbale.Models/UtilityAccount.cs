using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class UtilityAccount
    {
        public long UtilityAccountId { get; set; }
        public long UtilityCategoryId { get; set; }
        public double Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public long BranchId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public double Balance { get; set; }
        public double StartAmount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }

        public string BranchName { get; set; }
        public string UtilityCategoryName { get; set; }
    }
}
