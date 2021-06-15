using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel.consolidated
{
  public  class PeriodicConsolidatedReport
    {
        public IEnumerable<Supply> Supplies { get; set; }
        public double TotalMaizeReceived { get; set; }
        public double TotalMaizeMilled { get; set; }
        public double TotalMaizeInProgress { get; set; }

        public IEnumerable<CashTransfer> CashTransfersFromBranch { get; set; }
        public IEnumerable<CashTransfer> CashTransfersToBranch { get; set; }
        public decimal TotalAmountTransfered { get; set; }
        public decimal TotalAmountReceived { get; set; }
        public IEnumerable<FlourTransfer> FlourTransfersFromBranch { get; set; }
        public IEnumerable<FlourTransfer> FlourTransfersToBranch { get; set; }
        public IEnumerable<Delivery> Deliveries { get; set; }
        public double TotalFlourDelivered { get; set; }
        public double TotalBrandDelivered { get; set; }
        public double TotalFlourMilled { get; set; }
        public IEnumerable<Batch> Batches { get; set; }
        public IEnumerable<Cash> Incomes { get; set; }
        public IEnumerable<Cash> Expenditures { get; set; }
        public double TotalFlourOutPut { get; set; }

        public double TotalBrandOutPut { get; set; }

        public double TotalIncomes { get; set; }
        public double TotalExpenses { get; set; }
    }
}
