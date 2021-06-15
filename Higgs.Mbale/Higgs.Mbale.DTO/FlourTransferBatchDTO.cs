using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.DTO
{
 public   class FlourTransferBatchDTO
    {
        public long BatchId { get; set; }
        public long FlourTransferId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public double BatchQuantity { get; set; }

    }
}
