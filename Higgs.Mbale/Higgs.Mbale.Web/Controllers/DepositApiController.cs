using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using System;

namespace Higgs.Mbale.Web.Controllers
{
    public class DepositApiController : ApiController
    {
      
            private IDepositService _depositService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(DepositApiController));
            private string userId = string.Empty;

            public DepositApiController()
            {
            }

            public DepositApiController(IDepositService depositService, IUserService userService)
            {
                this._depositService = depositService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

           
            [HttpGet]
            [ActionName("GetDeposit")]
            public Deposit GetDeposit(long depositId)
            {
                return _depositService.GetDeposit(depositId);
            }

            [HttpGet]
            [ActionName("GetAllDeposits")]
            public IEnumerable<Deposit> GetAllDeposits()
            {
                return _depositService.GetAllDeposits();
            }

        [HttpGet]
        [ActionName("GetLatestTwentyRejectedDeposits")]
        public IEnumerable<Deposit> GetLatestTwentyRejectedDeposits()
        {
            return _depositService.GetLatestTwentyRejectedDeposits();
        }
        [HttpGet]
        [ActionName("GetLatestTwentyApprovedDeposits")]
        public IEnumerable<Deposit> GetLatestTwentyApprovedDeposits()
        {
            return _depositService.GetLatestTwentyApprovedDeposits();
        }
        [HttpGet]
        [ActionName("GetLatestTwentyUnApprovedDeposits")]
        public IEnumerable<Deposit> GetLatestTwentyUnApprovedDeposits()
        {
            return _depositService.GetLatestTwentyUnApprovedDeposits();
        }

        [HttpGet]
            [ActionName("GetAllDepositsForAParticularAccount")]
            public IEnumerable<Deposit> GetAllDepositsForAParticularAccount(string accountId)
            {
                return _depositService.GetAllDepositsForAParticularAccount(accountId);
            }



            [HttpGet]
            [ActionName("Delete")]
            public void MarkAsDeleted(long depositId)
            {
                _depositService.MarkAsDeleted(depositId, userId);
            }

           
            [HttpGet]
            [ActionName("GetAllUnApprovedDepositsForAParticularAccount")]
            public IEnumerable<Deposit> GetAllUnApprovedDepositsForAParticularAccount(string accountId)
            {
                return _depositService.GetAllUnApprovedDepositsForAParticularAccount(accountId);
            }

            [HttpGet]
            [ActionName("GetLatestTwentyApprovedDepositsForAParticularAccount")]
            public IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForAParticularAccount(string accountId)
            {
                return _depositService.GetLatestTwentyApprovedDepositsForAParticularAccount(accountId);
            }

            [HttpGet]
            [ActionName("GetLatestTwentyRejectedDepositsForAParticularAccount")]
            public IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForAParticularAccount(string accountId)
            {
                return _depositService.GetLatestTwentyRejectedDepositsForAParticularAccount(accountId);
            }

            [HttpPost]
            [ActionName("ApproveOrRejectDeposit")]
            public long ApproveOrRejectDeposit(Deposit deposit)
            {
               var status = Convert.ToBoolean(deposit.Approved);
                return _depositService.ApproveOrRejectDeposit(deposit,status, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(Deposit model)
            {

                var depositId = _depositService.SaveDeposit(model, userId);
                return depositId;
            }
        
        
    }
}
