using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
public    class FlourTransferReportViewModel
    {
        public IEnumerable<FlourTransfer> FlourTransfers { get; set; }
        public double TotalQuantity { get; set; }
      
    }
}
