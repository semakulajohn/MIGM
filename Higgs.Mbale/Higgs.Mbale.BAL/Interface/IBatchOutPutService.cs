using System;
using System.Collections.Generic;

using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IBatchOutPutService
    {
        IEnumerable<BatchOutPut> GetAllBatchOutPuts();
        BatchOutPut GetBatchOutPut(long batchOutPutId);
        long SaveBatchOutPut(BatchOutPut batchOutPut, string userId);
        void MarkAsDeleted(long batchOutPutId, string userId);
        IEnumerable<BatchOutPut> GetAllBatchOutPutsForAParticularBatch(long batchId);
        IEnumerable<BatchOutPut> MapEFToModel(IEnumerable<EF.Models.BatchOutPut> data);
        void UpdateBatchBrandBalance(long batchId, double quantity, string userId);
        void UpdateBatchGradeSizes(List<BatchGradeSize> batchGradeSizeList);
         double ComputeBatchLoss(double brandOutPut, double flourOutPut, double quantity);
         double ComputePercentageBatchLoss(double brandOutPut, double flourOutPut, double quantity);
      
         double ComputePercentageBatchFlourOutPut(double flourOutput, double quantity);
       
         double ComputePercentageBatchBrandOutPut(double brandOutPut, double quantity);
       
        
    }
}
