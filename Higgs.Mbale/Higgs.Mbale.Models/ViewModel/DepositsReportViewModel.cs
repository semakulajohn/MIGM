using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
  public  class DepositsReportViewModel
    {
        public IEnumerable<AccountTransactionActivity> Deposits { get; set; }
        public decimal TotalAmount { get; set; }
        public double TotalQuantity { get; set; }
    }
}
