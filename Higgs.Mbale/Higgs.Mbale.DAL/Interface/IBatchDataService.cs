using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IBatchDataService
    {
        IEnumerable<Batch> GetAllBatches();
        Batch GetBatch(long batchId);
        long SaveBatch(BatchDTO batch, string userId);
        void MarkAsDeleted(long batchId, string userId);
        IEnumerable<Batch> GetAllBatchesForAParticularBranch(long branchId);
       // void SaveBatchGradeSize(BatchGradeSizeDTO batchGradeSizeDTO);
       // void PurgeBatchGradeSize(long batchId);
        void SaveBatchSupply(BatchSupplyDTO batchSupplyDTO);
        void PurgeBatchSupply(long batchId,long supplyId);
        void UpdateBatchBrandBalance(long batchId, double quantity, string userId);
        IEnumerable<Batch> GetAllBatchesForBrandDelivery(long branchId);
        IEnumerable<Batch> GetTenBatchesForAParticularBranch(long branchId);
        IEnumerable<DeliveryBatch> GetBatchDeliveries(long batchId);
        IEnumerable<CashSaleBatch> GetBatchCashSales(long batchId);
        IEnumerable<DeliveryBatch> GetAllBatchesForADelivery(long batchId);
        IEnumerable<FlourTransferBatch> GetBatchFlourTransfers(long batchId);
    }
}
