using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
public    interface IBuveraTransferDataService
    {
        IEnumerable<BuveraTransfer> GetAllBuveraTransfers();
        BuveraTransfer GetBuveraTransfer(long buveraTransferId);
        long SaveBuveraTransfer(BuveraTransferDTO buveraTransfer, string userId);
        void MarkAsDeleted(long buveraTransferId, string userId);
        IEnumerable<BuveraTransfer> GetAllBuveraTransfersForAParticularStore(long storeId);
        void SaveBuveraTransferGradeSize(BuveraTransferGradeSizeDTO buveraTransferGradeSizeDTO);
        void PurgeBuveraTransferGradeSize(long buveraTransferId);
        void SaveStoreBuveraTransferGradeSize(StoreBuveraTransferGradeSizeDTO storeGradeSizeDTO, bool inOrOut);
        IEnumerable<StoreBuveraTransferGradeSize> GetStoreBuveraTransferStock(long storeId);
    }
}
