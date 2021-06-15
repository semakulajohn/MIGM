using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class CashSale
    {
        public long CashSaleId { get; set; }
        public Nullable<double> Price { get; set; }
        public long ProductId { get; set; }
        public long PaymentModeId { get; set; }
        public long TransactionSubTypeId { get; set; }
        public long BranchId { get; set; }
        public long SectorId { get; set; }
        public Nullable<double> Amount { get; set; }
        public long StoreId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<double> Quantity { get; set; }
        public bool Cancelled { get; set; }
        public Nullable<int> ReceiptLimit { get; set; }
        //public Nullable<long> DocumentCategoryId { get; set; }
    
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }

        public List<Grade> Grades { get; set; }
        public List<Batch> Batches { get; set; }
        public List<CashSaleBatch> CashSaleBatches { get; set; }

        public string BranchName { get; set; }
        public string StoreName { get; set; }

        public List<Grade> SelectedGrades { get; set; }
     
        public string TransactionSubTypeName { get; set; }
        public string ProductName { get; set; }
        public string SectorName { get; set; }
        public string PaymentModeName { get; set; }
        public long DocumentId { get; set; }
        public long DocumentNumber { get; set; }
        
    }
}
