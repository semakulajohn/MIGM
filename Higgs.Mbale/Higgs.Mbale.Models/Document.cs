using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class Document
    {
        public long DocumentId { get; set; }
        
        public double Amount { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public long ItemId { get; set; }
        public long DocumentNumber { get; set; }
        public long DocumentCategoryId { get; set; }
        public Nullable<double> Quantity { get; set; }
        public long BranchId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string AmountInWords { get; set; }
        public List<Grade> Grades { get; set; }

        public string UserName { get; set; }
        public string BranchName { get; set; }
        public string DocumentCategoryName { get; set; }
    }
}
