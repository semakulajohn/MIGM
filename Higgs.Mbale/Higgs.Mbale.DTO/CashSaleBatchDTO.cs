using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.DTO
{
 public   class CashSaleBatchDTO
    {
        public long BatchId { get; set; }
        public long CashSaleId { get; set; }
        public Nullable<double> BatchQuantity { get; set; }
        public Nullable<double> Price { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<long> ProductId { get; set; }

    }
}
