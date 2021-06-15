using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
  public  class BuveraReportViewModel
    {
        public IEnumerable<Buvera> Buveras { get; set; }
        public double TotalQuantity { get; set; }

    }
}
