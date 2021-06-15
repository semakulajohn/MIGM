using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class AssetApiController : ApiController
    {
            private IAssetService _assetService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(AssetApiController));
            private string userId = string.Empty;

            public AssetApiController()
            {
            }

            public AssetApiController(IAssetService assetService, IUserService userService)
            {
                this._assetService = assetService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetAsset")]
            public Asset GetAsset(long assetId)
            {
                return _assetService.GetAsset(assetId);
            }

            [HttpGet]
            [ActionName("GetAllAssets")]
            public IEnumerable<Asset> GetAllAssets()
            {
                return _assetService.GetAllAssets();
            }

            [HttpGet]
            [ActionName("GetAllAssetsForAParticularCategoryForAParticularBranch")]
            public IEnumerable<Asset> GetAllAssetsForAParticularCategoryForAParticularBranch(long branchId,long assetCategoryId)
            {
                return _assetService.GetAllAssetsForAParticularCategoryForAParticularBranch(branchId,assetCategoryId);
            }
            [HttpGet]
            [ActionName("GetAllAssetsForAParticularBranch")]
            public IEnumerable<Asset> GetAllAssetsForAParticularBranch(long branchId)
            {
                return _assetService.GetAllAssetsForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetAllAssetsForAParticularCategory")]
            public IEnumerable<Asset> GetAllAssetsForAParticularCategory(long assetCategoryId)
            {
                return _assetService.GetAllAssetsForAParticularCategory(assetCategoryId);
            }
            [HttpGet]
            [ActionName("Delete")]
            public void DeleteAsset(long assetId)
            {
                _assetService.MarkAsDeleted(assetId, userId);
            }



            [HttpPost]
            [ActionName("Save")]
            public long Save(Asset model)
            {
                var assetId = _assetService.SaveAsset(model, userId);
                return assetId;
            }

        
    }
}
