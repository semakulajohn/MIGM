using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.DTO
{
  public  class BuveraDTO
    {
        public long BuveraId { get; set; }
        public Nullable<double> TotalCost { get; set; }
        public long StoreId { get; set; }
        public long BranchId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public double TotalQuantity { get; set; }
        public string FromSupplier { get; set; }
        public string ToReceiver { get; set; }
        public string InvoiceNumber { get; set; }
        public long BuveraCategoryId { get; set; }
        public bool Approved { get; set; }
        public bool Rejected { get; set; }
        public long ProductId { get; set; }



    }
}
