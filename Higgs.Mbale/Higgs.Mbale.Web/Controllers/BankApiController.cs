using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class BankApiController : ApiController
    {
           private IBankService _bankService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(BankApiController));
            private string userId = string.Empty;

            public BankApiController()
            {
            }

            public BankApiController(IBankService bankService,IUserService userService)
            {
                this._bankService = bankService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetBank")]
            public Bank GetBank(long bankId)
            {
                return _bankService.GetBank(bankId);
            }

            [HttpGet]
            [ActionName("GetAllBanks")]
            public IEnumerable<Bank> GetAllBanks()
            {
                return _bankService.GetAllBanks();
            }
          

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteBank(long bankId)
            {
                _bankService.MarkAsDeleted(bankId, userId);
            }

         

            [HttpPost]
            [ActionName("Save")]
            public long Save(Bank model)
            {
                var bankId = _bankService.SaveBank(model, userId);
                return bankId;
            }
    }
}
