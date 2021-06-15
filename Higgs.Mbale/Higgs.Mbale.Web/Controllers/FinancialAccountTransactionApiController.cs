using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class FinancialAccountTransactionApiController : ApiController
    {
        private IFinancialAccountTransactionService _financialAccountTransactionService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(FinancialAccountTransactionApiController));
        private string userId = string.Empty;

        public FinancialAccountTransactionApiController()
        {
        }

        public FinancialAccountTransactionApiController(IFinancialAccountTransactionService financialAccountTransactionService, IUserService userService)
        {
            this._financialAccountTransactionService = financialAccountTransactionService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetFinancialAccountTransaction")]
        public FinancialAccountTransaction GetFinancialAccountTransaction(long financialAccountTransactionId)
        {
            return _financialAccountTransactionService.GetFinancialAccountTransaction(financialAccountTransactionId);
        }

        [HttpGet]
        [ActionName("GetAllFinancialAccountTransactions")]
        public IEnumerable<FinancialAccountTransaction> GetAllFinancialAccountTransactions()
        {
            return _financialAccountTransactionService.GetAllFinancialAccountTransactions();
        }



        [HttpGet]
        [ActionName("GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount")]
        public IEnumerable<FinancialAccountTransaction> GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount(long financialAccountId)
        {
            return _financialAccountTransactionService.GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount( financialAccountId);
        }


        [HttpGet]
        [ActionName("Delete")]
        public void DeleteFinancialAccountTransaction(long financialAccountId,long financialAccountTransactionId)
        {
            _financialAccountTransactionService.MarkAsDeleted(financialAccountId,financialAccountTransactionId, userId);
        }



        [HttpPost]
        [ActionName("Save")]
        public long Save(FinancialAccountTransaction model)
        {

            var financialAccountTransactionId = _financialAccountTransactionService.SaveFinancialAccountTransaction(model, userId);
            return financialAccountTransactionId;
        }
    }
}
