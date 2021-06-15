using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class BatchDeliveryDetails
    {
       
            public string BatchNumber { get; set; }
            public long BatchId { get; set; }
            public double BatchQuantity { get; set; }
            public double Price { get; set; }
            public double Amount { get; set; }
        
    }
}
