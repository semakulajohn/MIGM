using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.WebViewModel
{
 public   class CashSaleViewModel
    {
        public long CashSaleId { get; set; }
        public Nullable<double> Price { get; set; }
              
        public Nullable<double> Amount { get; set; }
      
        public Nullable<double> Quantity { get; set; }
       
        public Nullable<System.DateTime> CreatedOn { get; set; }
         
        public string ProductName { get; set; }
        
        public long DocumentId { get; set; }
      
    }
}
