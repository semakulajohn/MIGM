using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;


namespace Higgs.Mbale.Web.Controllers
{
    public class AssetCategoryApiController : ApiController
    {
        
            private IAssetCategoryService _assetCategoryService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(AssetCategoryApiController));
            private string userId = string.Empty;

            public AssetCategoryApiController()
            {
            }

        public AssetCategoryApiController(IAssetCategoryService assetCategoryService, IUserService userService)
            {
                this._assetCategoryService = assetCategoryService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetAssetCategory")]
            public AssetCategory GetAssetCategory(long assetCategoryId)
            {
                return _assetCategoryService.GetAssetCategory(assetCategoryId);
            }

            [HttpGet]
            [ActionName("GetAllAssetCategories")]
            public IEnumerable<AssetCategory> GetAllAssetCategories()
            {
                return _assetCategoryService.GetAllAssetCategories();
            }


            [HttpGet]
            [ActionName("Delete")]
            public void DeleteAssetCategory(long assetCategoryId)
            {
            _assetCategoryService.MarkAsDeleted(assetCategoryId, userId);
            }



            [HttpPost]
            [ActionName("Save")]
            public long Save(AssetCategory model)
            {
                var assetCategoryId = _assetCategoryService.SaveAssetCategory(model, userId);
                return assetCategoryId;
            }
        
    }
}
