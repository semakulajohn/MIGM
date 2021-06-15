using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel.consolidated
{
 public   class ConsolidatedCashTransferViewModel
    {
        public IEnumerable<CashTransfer> CashTransferKawempe { get; set; }
        public decimal TotalAmountKawempe { get; set; }
       
        public IEnumerable<CashTransfer> CashTransferWakasanke { get; set; }
        public decimal TotalAmountWakasanke { get; set; }
       
        public IEnumerable<CashTransfer> CashTransferNatete { get; set; }
        public decimal TotalAmountNatete { get; set; }
       
        public IEnumerable<CashTransfer> CashTransferBira { get; set; }
        public decimal TotalAmountBira { get; set; }
        
        public IEnumerable<CashTransfer> CashTransferWakasankeMwase { get; set; }
        public decimal TotalAmountWakasankeMwase { get; set; }
     
        public IEnumerable<CashTransfer> CashTransferKisenyiMwase { get; set; }
        public decimal TotalAmountKisenyiMwase { get; set; }

        public IEnumerable<CashTransfer> CashTransferFAM { get; set; }
        public decimal TotalAmountFAM { get; set; }

        public IEnumerable<CashTransfer> CashTransferHeadOffice { get; set; }
        public decimal TotalAmountHeadOffice{ get; set; }


    }
}
