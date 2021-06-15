using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IAssetDataService
    {
        IEnumerable<Asset> GetAllAssets();
        Asset GetAsset(long assetId);
        long SaveAsset(AssetDTO assetDTO, string userId);
        void MarkAsDeleted(long assetId, string userId);
        IEnumerable<Asset> GetAllAssetsForAParticularCategory(long assertCategoryId);
        IEnumerable<Asset> GetAllAssetsForAParticularCategoryForAParticularBranch(long assetCategoryId, long branchId);
        IEnumerable<Asset> GetAllAssetsForAParticularBranch(long branchId);

    }
}
