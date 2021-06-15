using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IBatchProjectionService
    {
        IEnumerable<BatchProjection> GetAllBatchProjections();
        BatchProjection GetBatchProjection(long batchProjectionId);
        long SaveBatchProjection(BatchProjection batchProjection, string userId);
        void MarkAsDeleted(long batchProjectionId, string userId);

        IEnumerable<BatchProjection> GetAllBatchProjectionsForAParticularBatch(long batchId);

    }
}
