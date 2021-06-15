using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class BuveraTransfer
    {
        public long BuveraTransferId { get; set; }
        public long StoreId { get; set; }
        public long BranchId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public long ToReceiverStoreId { get; set; }
        public bool Accept { get; set; }
        public bool Reject { get; set; }
        public long FromSupplierStoreId { get; set; }
        public double TotalQuantity { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public long ProductId { get; set; }

        public string ProductName { get; set; }
        public string Issuing { get; set; }

        public List<Grade> Grades { get; set; }

        public string BranchName { get; set; }
        public string StoreName { get; set; }
        public string ReceiverStoreName { get; set; }
        public string SupplierStoreName { get; set; }
    }
}
