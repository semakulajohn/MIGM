using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class MaizeBrandStoreApiController : ApiController
    {
        private IMaizeBrandStoreService _maizeBrandStoreService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(MaizeBrandStoreApiController));
            private string userId = string.Empty;

            public MaizeBrandStoreApiController()
            {
            }

            public MaizeBrandStoreApiController(IMaizeBrandStoreService maizeBrandStoreService, IUserService userService)
            {
                this._maizeBrandStoreService = maizeBrandStoreService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }


            
            [HttpGet]
            [ActionName("GetMaizeBrandStore")]
            public MaizeBrandStore GetMaizeBrandStore(long maizeBrandStoreId)
            {
                return _maizeBrandStoreService.GetMaizeBrandStore(maizeBrandStoreId);
            }

            [HttpGet]
            [ActionName("GetAllMaizeBrandStores")]
            public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStore()
            {
                return _maizeBrandStoreService.GetAllMaizeBrandStore();
            }

            [HttpGet]
            [ActionName("GetBalanceForMaizeBrandStoreForABranch")]
            public double GetBalanceForMaizeBrandStoreForABranch(long branchId)
            {
                double brandBalance = 0;
                brandBalance = _maizeBrandStoreService.GetBalanceForMaizeBrandStoreForABranch(branchId);
                return brandBalance;
            }

            //[HttpGet]
            //[ActionName("GetAllMaizeBrandStoreForAParticularBranch")]
            //public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranch(long branchId)
            //{
            //    return _maizeBrandStoreService.GetAllMaizeBrandStoreForAParticularBranch(branchId);
            //}
        [HttpGet]
        [ActionName("GetAllMaizeBrandStoreForAParticularBranchToDeliver")]
            public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranchToDeliver(long branchId)
            {
                return _maizeBrandStoreService.GetAllMaizeBrandStoreForAParticularBranchToDeliver(branchId);
            }
            [HttpGet]
            [ActionName("GetLatestMaizeBrandForAParticularBranch")]
            public double GetBalanceForLastMaizeBrandStore(long branchId)
            {
                double balance = 0;
                balance = _maizeBrandStoreService.GetBalanceForLastMaizeBrandStore(branchId);
                return balance;
                
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteMaizeBrandStore(long maizeBrandStoreId)
            {
                _maizeBrandStoreService.MarkAsDeleted(maizeBrandStoreId, userId);
            }


            [HttpPost]
            [ActionName("Save")]
            public long Save(MaizeBrandStore model)
            {
                
                var maizeBrandStoreId = _maizeBrandStoreService.SaveMaizeBrandStore(model, userId);
                return maizeBrandStoreId;
            }

    }
}
