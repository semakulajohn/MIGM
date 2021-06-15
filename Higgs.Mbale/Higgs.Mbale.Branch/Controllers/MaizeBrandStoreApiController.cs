using System;
using System.Collections.Generic;

using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class MaizeBrandStoreApiController : ApiController
    {
        private IMaizeBrandStoreService _maizeBrandStoreService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(MaizeBrandStoreApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public MaizeBrandStoreApiController()
            {
            }

            public MaizeBrandStoreApiController(IMaizeBrandStoreService maizeBrandStoreService, IUserService userService)
            {
                this._maizeBrandStoreService = maizeBrandStoreService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }


            
            [HttpGet]
            [ActionName("GetMaizeBrandStore")]
            public MaizeBrandStore GetMaizeBrandStore(long maizeBrandStoreId)
            {
                return _maizeBrandStoreService.GetMaizeBrandStore(maizeBrandStoreId);
            }

            

            [HttpGet]
            [ActionName("GetBalanceForMaizeBrandStoreForABranch")]
            public double GetBalanceForMaizeBrandStoreForABranch()
            {
                double brandBalance = 0;
                brandBalance = _maizeBrandStoreService.GetBalanceForMaizeBrandStoreForABranch(branchId);
                return brandBalance;
            }

           
        [HttpGet]
        [ActionName("GetAllMaizeBrandStoreForAParticularBranchToDeliver")]
            public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranchToDeliver()
            {
                return _maizeBrandStoreService.GetAllMaizeBrandStoreForAParticularBranchToDeliver(branchId);
            }
            [HttpGet]
            [ActionName("GetLatestMaizeBrandForAParticularBranch")]
            public double GetBalanceForLastMaizeBrandStore()
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
                model.BranchId = branchId;
                var maizeBrandStoreId = _maizeBrandStoreService.SaveMaizeBrandStore(model, userId);
                return maizeBrandStoreId;
            }

    }
}
