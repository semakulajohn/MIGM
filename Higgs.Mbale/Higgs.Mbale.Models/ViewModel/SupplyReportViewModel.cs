using System;
using System.Collections.Generic;


namespace Higgs.Mbale.Models.ViewModel
{
   public class SupplyReportViewModel
    {
        public IEnumerable<Supply> Supplies { get; set; }
        public double TotalMaize { get; set; }
        public decimal TotalAmount { get; set; }
        public double TotalStoneBags { get; set; }
        public double TotalNormalBags { get; set; }
        public double TotalYellowBags { get; set; }
    }
}
