using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;


namespace Higgs.Mbale.BAL.Interface
{
  public  interface IMaizeBrandStoreService
    {
      double GetBalanceForLastMaizeBrandStore(long branchId);
      MaizeBrandStore GetMaizeBrandStore(long maizeBrandStoreId);
      IEnumerable<MaizeBrandStore> GetAllMaizeBrandStore();

      IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranch(long branchId);
      IEnumerable<MaizeBrandStore> MapEFToModel(IEnumerable<EF.Models.MaizeBrandStore> data);
      long SaveMaizeBrandStore(MaizeBrandStore maizeBrandStore, string userId);
      double GetBalanceForMaizeBrandStoreForABranch(long branchId);
      void MarkAsDeleted(long maizeBrandStoreId, string userId);
      void UpdateMaizeBrandBatchBalance(long batchId, double quantity, string userId);
      IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBatch(long batchId);
      MaizeBrandStore GetLatestMaizeBrandStoreForAParticularBranch(long branchId);
      IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranchToDeliver(long branchId);
      long UpdateBrandStore(long branchId, string action,string userId, double quantity);
    }
}
