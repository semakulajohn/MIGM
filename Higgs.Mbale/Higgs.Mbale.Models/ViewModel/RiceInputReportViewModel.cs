using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
 public   class RiceInputReportViewModel
    {
        public IEnumerable<RiceInput> RiceInputs { get; set; }
        public double TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
