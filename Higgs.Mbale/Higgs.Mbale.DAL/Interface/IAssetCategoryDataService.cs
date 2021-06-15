using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;


namespace Higgs.Mbale.DAL.Interface
{
  public  interface IAssetCategoryDataService
    {
        IEnumerable<AssetCategory> GetAllAssetCategories();
        AssetCategory GetAssetCategory(long assetCategoryId);
        long SaveAssetCategory(AssetCategoryDTO assetCategoryDTO, string userId);
        void MarkAsDeleted(long assetCategoryId, string userId);
    }
}
