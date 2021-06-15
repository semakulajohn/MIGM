using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models.ViewModel;

namespace Higgs.Mbale.Models
{
   public  class DailyReport
    {
      public  DeliveryReportViewModel FlourDeliveries { get; set; }
        public DeliveryReportViewModel BrandDeliveries { get; set; }
        public CashSaleReportViewModel FlourCashSales { get; set; }
        public CashSaleReportViewModel BrandCashSales { get; set; }
        public DepositsReportViewModel Deposits { get; set; }
        public RecoveriesReportViewModel Recoveries { get; set; }
        public CashTransferReportViewModel TransferedCashTransfers { get; set; }
        public CashTransferReportViewModel ReceivedCashTransfers { get; set; }

        public MachineRepairReportViewModel MachineRepairs { get; set; }
        public LabourCostReportViewModel LabourCosts { get; set; }
        public FactoryExpenseReportViewModel FactoryExpenses { get; set; }
        public float CashBalance { get; set; }
        public FlourTransferReportViewModel ReceivedAcceptedFlourTransfers { get; set; }
        public FlourTransferReportViewModel TransferedAcceptedFlourTransfers { get; set; }
        public SupplyReportViewModel Supplies { get; set; }
        public CashReportViewModel Expenses { get; set; }
        public CashReportViewModel Income { get; set; }
        public float BrandOutPut { get; set; }
        public float FlourOutPut { get; set; }
    }
}
