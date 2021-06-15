using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
public    class BuveraTransferReportViewModel
    {
        public IEnumerable<BuveraTransfer> BuveraTransfers { get; set; }
        public double TotalQuantity { get; set; }

    }
}
