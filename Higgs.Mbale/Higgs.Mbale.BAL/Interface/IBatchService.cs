using System;
using System.Collections.Generic;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface IBatchService
    {
        IEnumerable<Batch> GetAllBatches();
        Batch GetBatch(long batchId);
        long SaveBatch(Batch batch, string userId);
        void MarkAsDeleted(long batchId, string userId);
        IEnumerable<BatchViewModel> GetAllBatchesForAParticularBranch(long branchId);
        IEnumerable<Batch> MapEFToModel(IEnumerable<EF.Models.Batch> data);
        IEnumerable<LabourCost> GenerateLabourCosts(long batchId,string userId);
        IEnumerable<Batch> GetBatchesForAParticularBranchToTransfer(long branchId,long productId);
        void UpdateBatchBrandBalance(long batchId, double quantity, string userId);
        IEnumerable<BatchViewModel> GetAllBatchesForBrandDelivery(long branchId);
        void UpdateBatchGradeSizes(List<BatchGradeSize> batchGradeSizeList);
        IEnumerable<BatchViewModel> GetTenBatchesForAParticularBranch(long branchId);
        IEnumerable<FlourTransferBatch> GetAllBatchFlourTransfers(long batchId);
        IEnumerable<DeliveryBatch> GetAllBatchesForADelivery(long deliveryId);
        //IEnumerable<CashSaleBatch> GetBatchCashSales(long batchId);
        IEnumerable<BatchViewModel> MapEFBatchToBatchViewModel(IEnumerable<EF.Models.Batch> data);

    }
}
