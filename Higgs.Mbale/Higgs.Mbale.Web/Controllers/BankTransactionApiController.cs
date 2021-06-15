using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class BankTransactionApiController : ApiController
    {
        
         private IBankTransactionService _bankTransactionService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(BankTransactionApiController));
            private string userId = string.Empty;

            public BankTransactionApiController()
            {
            }

            public BankTransactionApiController(IBankTransactionService bankTransactionService,IUserService userService)
            {
                this._bankTransactionService = bankTransactionService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetBankTransaction")]
            public BankTransaction GetBankTransaction(long bankTransactionId)
            {
                return _bankTransactionService.GetBankTransaction(bankTransactionId);
            }

            [HttpGet]
            [ActionName("GetAllBankTransactions")]
            public IEnumerable<BankTransaction> GetAllBankTransactions()
            {
                return _bankTransactionService.GetAllBankTransactions();
            }

          

            [HttpGet]
            [ActionName("GetLatestTwentyBankTransactionsForAParticularBranchAndBank")]
            public IEnumerable<BankTransaction> GetLatestTwentyBankTransactionsForAParticularBranchAndBank(long branchId, long bankId)
            {
                return _bankTransactionService.GetLatestTwentyBankTransactionsForAParticularBranchAndBank(branchId, bankId);
            }


            [HttpGet]
            [ActionName("Delete")]
            public void DeleteBankTransaction(long bankTransactionId)
            {
                _bankTransactionService.MarkAsDeleted(bankTransactionId, userId);
            }

         

            [HttpPost]
            [ActionName("Save")]
            public long Save(BankTransaction model)
            {

                var bankTransactionId = _bankTransactionService.SaveBankTransaction(model, userId);
                return bankTransactionId;
            }
    }
}
