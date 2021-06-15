using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Branch.Controllers
{
    public class MechanicApiController : ApiController
    {
        private IUserService _userService;

        ILog logger = log4net.LogManager.GetLogger(typeof(MechanicApiController));
        private string userId = string.Empty;

        public MechanicApiController()
        {
        }

        public MechanicApiController(IUserService userService)
        {
            this._userService = userService;

            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetAllMechanics")]
        public IEnumerable<AspNetUserViewModel> GetAllMechanics()
        {
            return _userService.GetAllMechanics();
        }
    }
}
