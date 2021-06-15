using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class CashApiController : ApiController
    {
          private ICashService _cashService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(CashApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public CashApiController()
            {
            }

            public CashApiController(ICashService cashService, IUserService userService)
            {
                this._cashService = cashService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }


            
            [HttpGet]
            [ActionName("GetCash")]
            public Cash GetCash(long cashId)
            {
                return _cashService.GetCash(cashId);
            }

           

            [HttpGet]
            [ActionName("GetAllCashForAParticularBranch")]
            public IEnumerable<Cash> GetAllCashForAParticularBranch()
            {
                return _cashService.GetThirtyLatestCashForAParticularBranch(branchId);
            }
         
          

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteCash(long cashId,long branchId)
            {
                _cashService.MarkAsDeleted(cashId, userId,branchId);
            }

         

            [HttpPost]
            [ActionName("Save")]
            public long Save(Cash model)
            {
               
                model.BranchId = branchId;
                var cashId = _cashService.SaveCash(model, userId);
                return cashId;
            }

          

    }
}
