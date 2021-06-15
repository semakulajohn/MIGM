using System;

namespace Higgs.Mbale.DTO
{
 public   class DepositDTO
    {
        public long DepositId { get; set; }
        public string AspNetUserId { get; set; }
        public Nullable<long> CasualWorkerId { get; set; }
        public long TransactionSubTypeId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public long SectorId { get; set; }
        public double StartAmount { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public Nullable<long> SupplyId { get; set; }
        public double Balance { get; set; }
        public string WeightNote { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Bags { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<bool> Approved { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }

      
    }
}
