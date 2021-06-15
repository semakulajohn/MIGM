using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface ICashTransferDataService
    {
            IEnumerable<CashTransfer> GetAllCashTransfers();
       
            IEnumerable<CashTransfer> GetAllCashTransfersForParticularBranch(long branchId);
       
            CashTransfer GetCashTransfer(long CashTransferId);
          
            long SaveCashTransfer(CashTransferDTO cashTransferDTO, string userId);
            void MarkAsDeleted(long CashTransferId, string userId);
            IEnumerable<CashTransfer> GetLatestTenCashTransfersForParticularBranch(long branchId);
            IEnumerable<CashTransfer> GetAllUnApprovedCashTransfers();



    }
}
