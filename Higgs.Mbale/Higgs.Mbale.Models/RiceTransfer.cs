using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class RiceTransfer
    {
        public long RiceTransferId { get; set; }
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
    }
}
