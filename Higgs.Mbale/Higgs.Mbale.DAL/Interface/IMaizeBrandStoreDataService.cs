using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IMaizeBrandStoreDataService
    {
        MaizeBrandStore GetMaizeBrandStore(long maizeBrandStoreId);
        IEnumerable<MaizeBrandStore> GetAllMaizeBrandStore();

        IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranch(long branchId);

        long SaveMaizeBrandStore(MaizeBrandStoreDTO maizeBrandStoreDTO, string userId);

        void MarkAsDeleted(long maizeBrandStoreId, string userId);
        MaizeBrandStore GetLatestMaizeBrandStoreForAParticularBranch(long branchId);

        void UpdateMaizeBrandBatchBalance(long batchId, double quantity, string userId);

        IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBatch(long batchId);

    }
}
