using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
 public   class CreditorReportViewModel
    {
        public IEnumerable<CreditorView> Creditors { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
