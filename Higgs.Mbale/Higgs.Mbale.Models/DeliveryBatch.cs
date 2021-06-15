using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class DeliveryBatch
    {
        public long BatchId { get; set; }
        public long DeliveryId { get; set; }
        public double BatchQuantity { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<double> Amount { get; set; }

        public string BatchNumber { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        
    }
}
