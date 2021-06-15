using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
public    class BatchReportViewModel
    {
        public IEnumerable<Batch> Batches{get; set;}
        public double TotalMaize {get; set;}
        public decimal TotalFactoryExpenses {get; set;}
        public double TotalBrandKgs{get; set;}
        public double TotalFlourKgs{get; set;}
        public decimal TotalLabourCosts{get; set;}
        public decimal TotalMillingBalance{get; set;}
        public decimal TotalMillingCharge {get; set;}
        public decimal TotalBuveraCosts{get; set;}
        public decimal TotalMachineCosts { get; set; }
        public decimal TotalProductionCosts { get; set; }
        public decimal TotalUtilityCosts { get; set; }
        public decimal TotalOtherExpenseCosts { get; set; }
               
    }
}
