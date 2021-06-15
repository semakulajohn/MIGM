using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
public    interface IBatchProjectionDataService
    {
            IEnumerable<BatchProjection> GetAllBatchProjections();
            BatchProjection GetBatchProjection(long batchProjectionId);
            long SaveBatchProjection(BatchProjectionDTO batchProjection, string userId);
            void MarkAsDeleted(long batchProjectionId, string userId);

            IEnumerable<BatchProjection> GetAllBatchProjectionsForAParticularBatch(long batchId);

    }
}
