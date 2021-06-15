using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class FinancialAccountApiController : ApiController
    {
        private IFinancialAccountService _financialAccountService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(FinancialAccountApiController));
        private string userId = string.Empty;

        public FinancialAccountApiController()
        {
        }

        public FinancialAccountApiController(IFinancialAccountService financialAccountService, IUserService userService)
        {
            this._financialAccountService = financialAccountService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetFinancialAccount")]
        public FinancialAccount GetFinancialAccount(long financialAccountId)
        {
            return _financialAccountService.GetFinancialAccount(financialAccountId);
        }

        [HttpGet]
        [ActionName("GetAllFinancialAccounts")]
        public IEnumerable<FinancialAccount> GetAllFinancialAccounts()
        {
            return _financialAccountService.GetAllFinancialAccounts();
        }

        [HttpPost]
        [ActionName("Save")]
        public long Save(FinancialAccount model)
        {
            var financialAccountId = _financialAccountService.SaveFinancialAccount(model, userId);
            return financialAccountId;
        }
    }
}
