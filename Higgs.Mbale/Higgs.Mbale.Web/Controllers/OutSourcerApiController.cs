using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Web.Controllers
{
    public class OutSourcerApiController : ApiController
    {
        private IUserService _userService;

        ILog logger = log4net.LogManager.GetLogger(typeof(OutSourcerApiController));
        private string userId = string.Empty;

        public OutSourcerApiController()
        {
        }

        public OutSourcerApiController(IUserService userService)
        {
            this._userService = userService;

            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetAllOutSourcers")]
        public IEnumerable<AspNetUserViewModel> GetAllOutSourcers()
        {
            return _userService.GetAllOutSourcers();
        }
    }
}
