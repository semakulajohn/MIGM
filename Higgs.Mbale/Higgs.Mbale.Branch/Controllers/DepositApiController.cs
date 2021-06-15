using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;


namespace Higgs.Mbale.Branch.Controllers
{
    public class DepositApiController : ApiController
    {

        private IDepositService _depositService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(DepositApiController));
        private string userId = string.Empty;
        long branchId = 0;

        public DepositApiController()
        {
        }

        public DepositApiController(IDepositService depositService, IUserService userService)
        {
            this._depositService = depositService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            branchId = _userService.GetLoggedUserBranchId(userId);
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

        [HttpPost]
        [ActionName("Save")]
        public long Save(Deposit model)
        {
            model.BranchId = branchId;
            var depositId = _depositService.SaveDeposit(model, userId);
            return depositId;
        }


        [HttpGet]
        [ActionName("GetLatestTwentyRejectedDepositsForABranch")]
        public IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForABranch()
        {
            return _depositService.GetLatestTwentyRejectedDepositsForABranch(branchId);
        }
        [HttpGet]
        [ActionName("GetLatestTwentyApprovedDepositsForABranch")]
        public IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForABranch()
        {
            return _depositService.GetLatestTwentyApprovedDepositsForABranch(branchId);
        }
        [HttpGet]
        [ActionName("GetLatestTwentyUnApprovedDepositsForABranch")]
        public IEnumerable<Deposit> GetLatestTwentyUnApprovedDepositsForABranch()
        {
            return _depositService.GetLatestTwentyUnApprovedDepositsForABranch(branchId);
        }
    }
}
