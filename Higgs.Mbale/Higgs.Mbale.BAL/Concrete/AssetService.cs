using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
  public  class AssetService : IAssetService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(AssetService));
        private IAssetDataService _dataService;
        private IUserService _userService;


        public AssetService(IAssetDataService dataService, IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssetId"></param>
        /// <returns></returns>
        public Asset GetAsset(long assetId)
        {
            var result = this._dataService.GetAsset(assetId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Asset> GetAllAssets()
        {
            var results = this._dataService.GetAllAssets();
            return MapEFToModel(results);
        }

        public IEnumerable<Asset> GetAllAssetsForAParticularCategory(long assetCategoryId)
        {
            var results = this._dataService.GetAllAssetsForAParticularCategory(assetCategoryId);
            return MapEFToModel(results);
        }
         public IEnumerable<Asset> GetAllAssetsForAParticularCategoryForAParticularBranch(long assetCategoryId, long branchId)
        {
            var results = this._dataService.GetAllAssetsForAParticularCategoryForAParticularBranch(assetCategoryId,branchId);
            return MapEFToModel(results);
        }

        public IEnumerable<Asset> GetAllAssetsForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllAssetsForAParticularBranch(branchId);
            return MapEFToModel(results);
        }
        public long SaveAsset(Asset asset, string userId)
        {
            var assetDTO = new DTO.AssetDTO()
            {
                AssetId = asset.AssetId,
                AssetCategoryId = asset.AssetCategoryId,
                Name = asset.Name,
                Deleted = asset.Deleted,
                CreatedBy = asset.CreatedBy,
                CreatedOn = asset.CreatedOn,
                DeletedOn = asset.DeletedOn,
                AssetCount = asset.AssetCount,
                PurchaseDate = asset.PurchaseDate,
                Amount = asset.Amount,
                Notes = asset.Notes,
                BranchId = asset.BranchId


            };

            var assetId = this._dataService.SaveAsset(assetDTO, userId);

            return assetId;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssetCategoryId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long AssetCategoryId, string userId)
        {
            _dataService.MarkAsDeleted(AssetCategoryId, userId);
        }


        #region Mapping Methods

        private IEnumerable<Asset> MapEFToModel(IEnumerable<EF.Models.Asset> data)
        {
            var list = new List<Asset>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Asset EF object to Bank Model Object and
        /// returns the Asset model object.
        /// </summary>
        /// <param name="result">EF Asset object to be mapped.</param>
        /// <returns>Asset Model Object.</returns>
        public Asset MapEFToModel(EF.Models.Asset data)
        {
            if (data != null)
            {
                var asset = new Asset()
                {
                    AssetCategoryId = data.AssetCategoryId,
                    Name = data.Name,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    AssetId = data.AssetId,
                    Deleted = data.Deleted,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    AssetCategoryName = data.AssetCategory != null ? data.AssetCategory.Name:"",
                    PurchaseDate = data.PurchaseDate,
                    Amount = data.Amount,
                    Notes = data.Notes,
                    AssetCount = data.AssetCount,
                    BranchId = data.BranchId,
         
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),


                };
                return asset;
            }
            return null;
        }



        #endregion
    }
}
