using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class WeightLoss
    {
        public long WeightLossId { get; set; }
        public long DeliveryId { get; set; }
        public double Quantity { get; set; }
        public string CustomerId { get; set; }
        public double Price { get; set; }
        public long BranchId { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<bool> Approved { get; set; }


        public string CustomerName { get; set; }
        public string BranchName { get; set; }
       
    }
}
