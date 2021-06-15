using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
    public interface IFlourTransferService
    {
        IEnumerable<FlourTransfer> GetAllFlourTransfers();
        FlourTransfer GetFlourTransfer(long flourTransferId);
        long SaveFlourTransfer(FlourTransfer flourTransfer, string userId);
        void MarkAsDeleted(long flourTransferId, string userId);
        IEnumerable<FlourTransfer> GetAllFlourTransfersForAParticularStore(long storeId);
        StoreGrade GetStoreFlourTransferStock(long storeId);
        long RejectFlour(FlourTransfer flourTransfer, string userId);
        long AcceptFlour(FlourTransfer flourTransfer, string userId);
        IEnumerable<FlourTransfer> MapEFToModel(IEnumerable<EF.Models.FlourTransfer> data);
    }
        
    
}
