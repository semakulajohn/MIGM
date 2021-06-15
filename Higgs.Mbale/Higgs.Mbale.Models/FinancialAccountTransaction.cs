using System;


namespace Higgs.Mbale.Models
{
 public   class FinancialAccountTransaction
    {
        public long FinancialAccountTransactionId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public double StartAmount { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public long FinancialAccountId { get; set; }

        public string BranchName { get; set; }
        public string FinancialAccountName { get; set; }
    }
}
