using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using MD =Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class BranchApiController : ApiController
    {
        private IBranchService _branchService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(BranchApiController));
        private string userId = string.Empty;

        public BranchApiController()
        {
        }

        public BranchApiController(IBranchService branchService, IUserService userService)
        {
            this._branchService = branchService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetBranch")]
        public MD.Branch GetBranch(long branchId)
        {
            return _branchService.GetBranch(branchId);
        }

        [HttpGet]
        [ActionName("GetAllBranches")]
        public IEnumerable<MD.Branch> GetAllBranches()
        {
            return _branchService.GetAllBranches();
        }



        [HttpGet]
        [ActionName("Delete")]
        public void DeleteBranch(long branchId)
        {
            _branchService.MarkAsDeleted(branchId, userId);
        }



        [HttpPost]
        [ActionName("Save")]
        public long Save(MD.Branch model)
        {

            var branchId = _branchService.SaveBranch(model, userId);
            return branchId;
        }

    }
}
