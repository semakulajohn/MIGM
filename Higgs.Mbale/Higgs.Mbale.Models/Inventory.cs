using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class Inventory
    {
        public long InventoryId { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }

        public long InventoryCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
       
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }

       
    }
}
