using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.WebViewModel
{
 public  class BatchViewModel
    {
        public long BatchId { get; set; }
        
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double TotalQuantity { get; set; }
        public double BrandBalance { get; set; }

        public double QuantityToRemove { get; set; }

        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
             
    }
}
