using System;
using System.Collections.Generic;


namespace Higgs.Mbale.Models.ViewModel
{
  public class WeightLossesReportViewModel
    {
        public IEnumerable<WeightLoss> WeightLosses { get; set; }
      
        public double TotalQuantity { get; set; }
    }
}
