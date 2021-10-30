using Higgs.Mbale.Models;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;

namespace Higgs.Mbale.Branch.Controllers
{
    public class BranchDashboardNotificationApiController : ApiController
    {
        private IDashBoardNotificationService _dashBoardNotificationService;
        private IUserService _userService;
        private string userId = string.Empty;
        long branchId = 0;


        public BranchDashboardNotificationApiController()
        {
        }

        public BranchDashboardNotificationApiController(IDashBoardNotificationService dashBoardNotificationService, IUserService userService)
        {
            this._dashBoardNotificationService = dashBoardNotificationService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            branchId = _userService.GetLoggedUserBranchId(userId);
        }




        [HttpGet]
        [ActionName("GetBranchDashBoardNotification")]
        public DashBoardNotification GetBranchDashBoardNotification()
        {
            return _dashBoardNotificationService.GetBranchDashBoardNotification(branchId);
        }
    }
}
