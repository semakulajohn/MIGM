using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IAssetCategoryService
    {
        IEnumerable<AssetCategory> GetAllAssetCategories();
        AssetCategory GetAssetCategory(long assetCategoryId);
        long SaveAssetCategory(AssetCategory assetCategory, string userId);
        void MarkAsDeleted(long assetCategoryId, string userId);
    }
}
