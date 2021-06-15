using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
 public   class CashTransferReportViewModel
    {
        public IEnumerable<CashTransfer> CashTransfers { get; set; }
       
        public decimal TotalAmount { get; set; }
    }
}
