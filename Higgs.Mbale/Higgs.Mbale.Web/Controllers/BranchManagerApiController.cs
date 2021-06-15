using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class BranchManagerApiController : ApiController
    {
        private IUserService _userService;
       
        ILog logger = log4net.LogManager.GetLogger(typeof(BranchManagerApiController));
        private string userId = string.Empty;

        public BranchManagerApiController()
        {
        }

        public BranchManagerApiController(IUserService userService)
        {
            this._userService = userService;
            
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetAllBranchManagers")]
        public IEnumerable<AspNetUser> GetAllBranchManagers()
        {
            return _userService.GetAllBranchManagers();
        }
        [HttpGet]
        [ActionName("GetAllBranchManagersSuperAdminsSuperUsersAndAdmins")]
       public IEnumerable<AspNetUser> GetAllBranchManagersSuperAdminsSuperUsersAndAdmins()
        {
            return _userService.GetAllBranchManagersSuperAdminsSuperUsersAndAdmins();
        }

        [HttpGet]
        [ActionName("GetBranchManager")]
        public UserBranch GetBranchManager(string branchManagerId)
        {
            return _userService.GetBranchManager(branchManagerId);
        }

        [HttpPost]
        [ActionName("Save")]
        public void Save(UserBranch model)
        {
            foreach (var selectedBranchId in model.SelectedBranches)
            {
              _userService.SaveUserBranch(model.UserId, selectedBranchId);

            }

        }

        [HttpPost]
        [ActionName("DetachUserFromBranch")]
        public void PurgeUserBranch(string userId, long branchId)
        {
            _userService.PurgeUserBranch(userId, branchId);
        }
    }
}
