using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
public    class BatchProjection
    {
        public long BatchProjectionId { get; set; }
        public long BatchId { get; set; }
        public double FlourOutPut { get; set; }
        public double FlourSales { get; set; }
        public double BrandSales { get; set; }
        public double FlourPercentage { get; set; }
        public double BrandPercentage { get; set; }
        public double FlourPrice { get; set; }
        public double BrandPrice { get; set; }
        public double BrandOutPut { get; set; }
        public long BranchId { get; set; }
        public double UnitCost { get; set; }
        public double ProductionCost { get; set; }
        public Nullable<double> ExpectedContribution { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }

        public string BranchName { get; set; }
       
        public string BatchNumber { get; set; }

    
        public double TotalSupplyAmount { get; set; }
      
        public string SupplierNames { get; set; }
      
        public double MaizeQuantity { get; set; }
        
        public double TotalProductionCost { get; set; }
        public double TotalExpectedSales { get; set; }
        public List<Supply> Supplies { get; set; }
       

     
    }
}
