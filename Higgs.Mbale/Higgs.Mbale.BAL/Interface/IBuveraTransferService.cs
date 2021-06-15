using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IBuveraTransferService
    {
        IEnumerable<BuveraTransfer> GetAllBuveraTransfers();
        BuveraTransfer GetBuveraTransfer(long buveraTransferId);
        long SaveBuveraTransfer(BuveraTransfer buveraTransfer, string userId);
        void MarkAsDeleted(long buveraTransferId, string userId);
        IEnumerable<BuveraTransfer> GetAllBuveraTransfersForAParticularStore(long storeId);
        StoreGrade GetStoreBuveraTransferStock(long storeId);
        long RejectBuvera(BuveraTransfer buveraTransfer, string userId);
        long AcceptBuvera(BuveraTransfer buveraTransfer, string userId);
        IEnumerable<BuveraTransfer> MapEFToModel(IEnumerable<EF.Models.BuveraTransfer> data);
    }
}
