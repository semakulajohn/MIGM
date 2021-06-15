using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class FactoryExpenseApiController : ApiController
    {
          private IFactoryExpenseService _factoryExpenseService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(FactoryExpenseApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public FactoryExpenseApiController()
            {
            }

            public FactoryExpenseApiController(IFactoryExpenseService factoryExpenseService,IUserService userService)
            {
                this._factoryExpenseService = factoryExpenseService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }

            [HttpGet]
            [ActionName("GetFactoryExpense")]
            public FactoryExpense GetFactoryExpense(long factoryExpenseId)
            {
                return _factoryExpenseService.GetFactoryExpense(factoryExpenseId);
            }

            [HttpGet]
            [ActionName("GetAllFactoryExpenses")]
            public IEnumerable<FactoryExpense> GetAllFactoryExpenses()
            {
                return _factoryExpenseService.GetAllFactoryExpenses();
            }

            [HttpGet]
            [ActionName("GetAllFactoryExpensesForAParticularBatch")]
            public IEnumerable<FactoryExpense> GetAllFactoryExpensesForAParticularBatch(long batchId)
            {
                return _factoryExpenseService.GetAllFactoryExpensesForAParticularBatch(batchId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteFactoryExpense(long factoryExpenseId)
            {
                _factoryExpenseService.MarkAsDeleted(factoryExpenseId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(FactoryExpense model)
            {
                model.BranchId = branchId;
                var factoryExpenseId = _factoryExpenseService.SaveFactoryExpense(model, userId);
                return factoryExpenseId;
            }

            [HttpPost]
            [ActionName("SaveFactoryExpenses")]
            public long SaveFactoryExpenses(BatchFactoryExpense model)
            {
                model.BranchId = branchId;
                var factoryExpenseId = _factoryExpenseService.SaveFactoryExpense(model, userId);
                return factoryExpenseId;
            }
    }
}
