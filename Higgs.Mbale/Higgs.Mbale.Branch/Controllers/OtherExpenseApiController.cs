using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class OtherExpenseApiController : ApiController
    {
         private IOtherExpenseService _otherExpenseService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(OtherExpenseApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public OtherExpenseApiController()
            {
            }

            public OtherExpenseApiController(IOtherExpenseService otherExpenseService,IUserService userService)
            {
                this._otherExpenseService = otherExpenseService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }

            [HttpGet]
            [ActionName("GetOtherExpense")]
            public OtherExpense GetOtherExpense(long otherExpenseId)
            {
                return _otherExpenseService.GetOtherExpense(otherExpenseId);
            }

          

            [HttpGet]
            [ActionName("GetAllOtherExpensesForAParticularBatch")]
            public IEnumerable<OtherExpense> GetAllOtherExpensesForAParticularBatch(long batchId)
            {
                return _otherExpenseService.GetAllOtherExpensesForAParticularBatch(batchId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteOtherExpense(long otherExpenseId)
            {
                _otherExpenseService.MarkAsDeleted(otherExpenseId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(OtherExpense model)
            {
                model.BranchId = branchId;
                var otherExpenseId = _otherExpenseService.SaveOtherExpense(model, userId);
                return otherExpenseId;
            }
    }
}
