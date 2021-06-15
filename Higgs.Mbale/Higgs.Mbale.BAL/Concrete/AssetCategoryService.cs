using System;
using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;


namespace Higgs.Mbale.BAL.Concrete
{
  public  class AssetCategoryService : IAssetCategoryService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(AssetCategoryService));
        private IAssetCategoryDataService _dataService;
        private IUserService _userService;


        public AssetCategoryService(IAssetCategoryDataService dataService, IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssetCategoryId"></param>
        /// <returns></returns>
        public AssetCategory GetAssetCategory(long assetCategoryId)
        {
            var result = this._dataService.GetAssetCategory(assetCategoryId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AssetCategory> GetAllAssetCategories()
        {
            var results = this._dataService.GetAllAssetCategories();
            return MapEFToModel(results);
        }


        public long SaveAssetCategory(AssetCategory assetCategory, string userId)
        {
            var assetCategoryDTO = new DTO.AssetCategoryDTO()
            {
                AssetCategoryId = assetCategory.AssetCategoryId,
                Name = assetCategory.Name,
                Deleted = assetCategory.Deleted,
                CreatedBy = assetCategory.CreatedBy,
                CreatedOn = assetCategory.CreatedOn,
                DeletedOn = assetCategory.DeletedOn,


            };

            var assetCategoryId = this._dataService.SaveAssetCategory(assetCategoryDTO, userId);

            return assetCategoryId;

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

        private IEnumerable<AssetCategory> MapEFToModel(IEnumerable<EF.Models.AssetCategory> data)
        {
            var list = new List<AssetCategory>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps AssetCategory EF object to Bank Model Object and
        /// returns the AssetCategory model object.
        /// </summary>
        /// <param name="result">EF AssetCategory object to be mapped.</param>
        /// <returns>AssetCategory Model Object.</returns>
        public AssetCategory MapEFToModel(EF.Models.AssetCategory data)
        {
            if (data != null)
            {
                var assetCategory = new AssetCategory()
                {
                    AssetCategoryId = data.AssetCategoryId,
                    Name = data.Name,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,

                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),


                };
                return assetCategory;
            }
            return null;
        }



        #endregion
    }
}
