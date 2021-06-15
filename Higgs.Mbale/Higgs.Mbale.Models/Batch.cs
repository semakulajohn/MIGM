using System;
using System.Collections.Generic;
using Higgs.Mbale.Models.ViewModel;

namespace Higgs.Mbale.Models
{
    public class Batch
    {
        public long BatchId { get; set; }
        public long SectorId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double TotalQuantity { get; set; }
        public long BranchId { get; set; }
        public long StoreId { get; set; }

        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public double BrandBalance { get; set; }

        public double QuantityToRemove { get; set; }

        public double Loss { get; set; }
        public double BrandOutPut { get; set; }
        public double FlourOutPut { get; set; }
        public Nullable<double> LossPercentage { get; set; }
        public Nullable<double> FlourPercentage { get; set; }
        public Nullable<double> BrandPercentage { get; set; }


        public double BranchMillingChargeRate { get; set; }
        public double MillingCharge { get; set; }
        public double TotalSupplyAmount { get; set; }
        public string BranchName { get; set; }
        public string SupplierNames { get; set; }
        public string SectorName { get; set; }
        public double TotalBuveraCost { get; set; }
        public double TotalFactoryExpenseCost { get; set; }
        public double FactoryExpenseCost { get; set; }
        public double TotalMachineCost { get; set; }
        public double TotalLabourCosts { get; set; }
        public double TotalUtilityCost { get; set; }
        public double TotalOtherExpenseCost { get; set; }
        public double MillingChargeBalance { get; set; }
        public double TotalProductionCost { get; set; }

        public double TotalFlourKgsDelivered { get; set; }
        public double TotalBrandDelivered { get; set; }
        public double TotalFlourDeliveryAmount { get; set; }
        public double TotalBrandDeliveryAmount { get; set; }

        public double TotalFlourKgsTransfered { get; set; }
        public double TotalCashSaleBrand { get; set; }
        public double TotalCashSaleFlourAmount { get; set; }
        public double TotalCashSaleFlour { get; set; }
        public double TotalCashSaleBrandAmount { get; set; }


        public List<Supply> Supplies { get; set; }
        public List<long> SupplyIds { get; set; }
        public List<Grade> Grades { get; set; }

        public List<FactoryExpense> FactoryExpenses { get; set; }
        public List<LabourCost> LabourCosts { get; set; }
        public List<MachineRepair> MachineRepairs { get; set; }
        public List<OtherExpense> OtherExpenses { get; set; }
        public List<BatchOutPut> BatchOutPuts { get; set; }
        public List<Utility> Utilities { get; set; }
        public List<DeliveryBatch> BatchBrandDeliveries { get; set; }
        public List<DeliveryBatch> BatchFlourDeliveries { get; set; }
        public List<CashSaleBatch> BatchBrandCashSales { get; set; }
        public List<CashSaleBatch> BatchFlourCashSales { get; set; }

        public List<FlourTransferBatch> BatchFlourTransfers { get; set; }
        public GradeSizeTotalsViewModel BatchFlourGradesValues { get; set; }
        public GradeSizeTotalsViewModel BatchFlourDeliveryGradesValues { get; set; }

        public List<Grade> AvailabeBatchGrades { get; set; }
        

    }
}
