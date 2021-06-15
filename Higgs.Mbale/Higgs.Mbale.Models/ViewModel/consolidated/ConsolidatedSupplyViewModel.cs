
using System.Collections.Generic;


namespace Higgs.Mbale.Models.ViewModel.consolidated
{
  public  class ConsolidatedSupplyViewModel
    {
        public IEnumerable<Supply> SupplyKawempe { get; set; }
        public decimal TotalAmountKawempe { get; set; }
        public double TotalQuantityKawempe { get; set; }
        public IEnumerable<Supply> SupplyWakasanke { get; set; }
        public decimal TotalAmountWakasanke { get; set; }
        public double TotalQuantityWakasanke { get; set; }
        public IEnumerable<Supply> SupplyNatete { get; set; }
        public decimal TotalAmountNatete { get; set; }
        public double TotalQuantityNatete { get; set; }
        public IEnumerable<Supply> SupplyBira { get; set; }
        public decimal TotalAmountBira { get; set; }
        public double TotalQuantityBira { get; set; }
        public IEnumerable<Supply> SupplyWakasankeMwase { get; set; }
        public decimal TotalAmountWakasankeMwase { get; set; }
        public double TotalQuantityWakasankeMwase { get; set; }
        public IEnumerable<Supply> SupplyKisenyiMwase { get; set; }
        public decimal TotalAmountKisenyiMwase { get; set; }
        public double TotalQuantityKisenyiMwase { get; set; }
    }
}
