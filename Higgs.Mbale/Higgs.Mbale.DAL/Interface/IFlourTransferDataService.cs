using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;
namespace Higgs.Mbale.DAL.Interface
{
 public   interface IFlourTransferDataService
    {
            IEnumerable<FlourTransfer> GetAllFlourTransfers();
            FlourTransfer GetFlourTransfer(long flourTransferId);
            long SaveFlourTransfer(FlourTransferDTO flourTransfer, string userId);
            void MarkAsDeleted(long flourTransferId, string userId);
            IEnumerable<FlourTransfer> GetAllFlourTransfersForAParticularStore(long storeId);
            void SaveFlourTransferGradeSize(FlourTransferGradeSizeDTO flourTransferGradeSizeDTO);
            void PurgeFlourTransferGradeSize(long flourTransferId);
            void SaveStoreFlourTransferGradeSize(StoreFlourTransferGradeSizeDTO storeGradeSizeDTO, bool inOrOut);
            IEnumerable<StoreFlourTransferGradeSize> GetStoreFlourTransferStock(long storeId);
            void SaveFlourTransferBatch(FlourTransferBatchDTO flourTransferBatch);
            IEnumerable<FlourTransferBatch> GetAllBatchesForAFlourTransfer(long flourTransferId);
        // void SaveBatchTransferGradeSize(List<BatchTransferGradeSizeDTO> batchTransferGradeSizeDTOS);
            void SaveBatchFlourTransferGradeSize(List<BatchTransferGradeSizeDTO> batchTransferGradeSizeDTOS);
            void PurgeBatchTransferGradeSize(long flourTransferId);

            void PurgeTransferBatch(long flourTransferId);
        
    }
}
