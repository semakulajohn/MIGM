using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class MakeDelivery
    {
     public bool StockReduced { get; set; }
     public long StockId { get; set; }
     public double OrderQuantityBalance { get; set; }
     public double BatchBrandBalance { get; set; }
     public List<BatchDeliveryDetails> BatchList { get; set; }
     public List<CashSaleBatch> CashSaleBatchList { get; set; }
    
    }
}
