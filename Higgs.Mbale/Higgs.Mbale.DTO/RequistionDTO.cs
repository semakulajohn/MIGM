using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.DTO
{
 public   class RequistionDTO
    {
        public long RequistionId { get; set; }
        public long StatusId { get; set; }
        public long BranchId { get; set; }
        public double Amount { get; set; }
        public string ApprovedById { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public bool Approved { get; set; }
        public bool Rejected { get; set; }
        public string RequistionNumber { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string AmountInWords { get; set; }
        public string OutSourcerId { get; set; }
        public Nullable<long> CasualWorkerId { get; set; }
        public Nullable<long> BatchId { get; set; }
        public Nullable<long> SupplyId { get; set; }
        public Nullable<long> ActivityId { get; set; }
        public Nullable<bool> PartPayment { get; set; }
        public long RequistionCategoryId { get; set; }
        public Nullable<double> Quantity { get; set; }
        public string RepairerName { get; set; }
        public System.DateTime RepairDate { get; set; }
        public Nullable<long> BankId { get; set; }
        public Nullable<long> UtilityCategoryId { get; set; }
        public Nullable<long> FinancialAccountId { get; set; }





    }
}
