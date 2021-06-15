using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class UtilityAccountApiController : ApiController
    {
        private IUtilityAccountService _utilityAccountService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(UtilityAccountApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public UtilityAccountApiController()
            {
            }

            public UtilityAccountApiController(IUtilityAccountService utilityAccountService, IUserService userService)
            {
                this._utilityAccountService = utilityAccountService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }


            
            [HttpGet]
            [ActionName("GetUtilityAccount")]
            public UtilityAccount GetUtilityAccount(long utilityAccountId)
            {
                return _utilityAccountService.GetUtilityAccount(utilityAccountId);
            }

            [HttpGet]
            [ActionName("GetAllUtilityAccounts")]
            public IEnumerable<UtilityAccount> GetAllUtilityAccount()
            {
                return _utilityAccountService.GetAllUtilityAccounts();
            }

            [HttpGet]
            [ActionName("GetAllUtilityCategories")]
            public IEnumerable<UtilityCategory> GetAllUtilityCategories()
            {
                return _utilityAccountService.GetAllUtilityCategories();
            }

            [HttpGet]
            [ActionName("GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory")]
            public IEnumerable<UtilityAccount> GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory(long categoryId)
            {
                return _utilityAccountService.GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory(branchId, categoryId);
            }


            [HttpGet]
            [ActionName("GetBalanceForUtilityAccountForABranch")]
            public double GetBalanceForUtilityAccountForABranch(long categoryId)
            {
                double balance = 0;
                balance = _utilityAccountService.GetBalanceForUtilityAccountForABranch(branchId,categoryId);
                return balance;
            }

            //[HttpGet]
            //[ActionName("GetAllUtilityAccountForAParticularBranch")]
            //public IEnumerable<UtilityAccount> GetAllUtilityAccountForAParticularBranch(long branchId)
            //{
            //    return _utilityAccountService.GetAllUtilityAccountForAParticularBranch(branchId);
            //}
          
            [HttpGet]
            [ActionName("Delete")]
            public void DeleteUtilityAccount(long utilityAccountId)
            {
                _utilityAccountService.MarkAsDeleted(utilityAccountId, userId);
            }


            [HttpPost]
            [ActionName("Save")]
            public long Save(UtilityAccount model)
            {
                model.BranchId = branchId; 
                var utilityAccountId = _utilityAccountService.SaveUtilityAccount(model, userId);
                return utilityAccountId;
            }

    }
}
