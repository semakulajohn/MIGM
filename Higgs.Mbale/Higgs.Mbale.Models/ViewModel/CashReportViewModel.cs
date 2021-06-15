using System;
using System.Collections.Generic;


namespace Higgs.Mbale.Models.ViewModel
{
 public   class CashReportViewModel
    {
        public IEnumerable<Cash> Cashs { get; set; }
      
        public decimal TotalAmount { get; set; }
    }
}
