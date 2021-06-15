using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
  public  class MachineRepairReportViewModel
    {
        public IEnumerable<MachineRepair> MachineRepairs { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
