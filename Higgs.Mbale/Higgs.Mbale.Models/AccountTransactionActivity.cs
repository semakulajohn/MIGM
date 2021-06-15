using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class AccountTransactionActivity
    {
        public long AccountTransactionActivityId { get; set; }
        public string AspNetUserId { get; set; }
        public Nullable<long> CasualWorkerId { get; set; }
        public long TransactionSubTypeId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public long SectorId { get; set; }
        public double StartAmount { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<long> SupplyId { get; set; }
        public long PaymentModeId { get; set; }

        public string WeightNote { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Bags { get; set; }
        public Nullable<double> Price { get; set; }
        public long DocumentId { get; set; }
        public string PaymentMode { get; set; }

        public string AccountName { get; set; }
        public string TransactionSubTypeName { get; set; }
        public string SectorName { get; set; }
        public string BranchName { get; set; }
        public double AccountBalance { get; set; }
    }
}
