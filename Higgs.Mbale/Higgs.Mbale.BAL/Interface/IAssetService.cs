using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IAssetService
    {
        IEnumerable<Asset> GetAllAssets();
        Asset GetAsset(long assetId);
        long SaveAsset(Asset asset, string userId);
        void MarkAsDeleted(long assetId, string userId);
        IEnumerable<Asset> GetAllAssetsForAParticularCategory(long assetCategoryId);
        IEnumerable<Asset> GetAllAssetsForAParticularCategoryForAParticularBranch(long assetCategoryId,long branchId);
        IEnumerable<Asset> GetAllAssetsForAParticularBranch(long branchId);
    }
}
