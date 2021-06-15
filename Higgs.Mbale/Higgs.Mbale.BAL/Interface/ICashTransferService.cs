using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
public    interface ICashTransferService
    {
        IEnumerable<CashTransfer> GetAllCashTransfers();

        IEnumerable<CashTransfer> GetAllCashTransfersForAParticularBranch(long branchId);

        CashTransfer GetCashTransfer(long CashTransferId);

        long SaveCashTransfer(CashTransfer cashTransfer, string userId);
        void MarkAsDeleted(long CashTransferId, string userId);

        long RejectCashTransfer(CashTransfer cashTransfer, string userId);
      
        long AcceptCashTransfer(CashTransfer cashTransfer, string userId);
        IEnumerable<CashTransfer> MapEFToModel(IEnumerable<EF.Models.CashTransfer> data);
        IEnumerable<CashTransfer> GetLatestTenCashTransfersForParticularBranch(long branchId);
        IEnumerable<CashTransfer> GetAllUnApprovedCashTransfers();


    }
}
