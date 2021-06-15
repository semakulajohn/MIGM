using System;
using System.Collections.Generic;

namespace Higgs.Mbale.Models.ViewModel
{
 public   class RecoveriesReportViewModel
    {
        public IEnumerable<AccountTransactionActivity> Recoveries { get; set; }
        public decimal TotalAmount { get; set; }
        public double TotalQuantity { get; set; }
    }
}
