using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class CasualWorkerApiController : ApiController
    {
        private ICasualWorkerService _casualWorkerService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(CasualWorkerApiController));
        private string userId = string.Empty;
        long branchId = 0;

        public CasualWorkerApiController()
        {
        }

        public CasualWorkerApiController(ICasualWorkerService casualWorkerService, IUserService userService)
        {
            this._casualWorkerService = casualWorkerService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            branchId = _userService.GetLoggedUserBranchId(userId);
        }

        [HttpGet]
        [ActionName("GetCasualWorker")]
        public CasualWorker GetCasualWorker(long casualWorkerId)
        {
            return _casualWorkerService.GetCasualWorker(casualWorkerId);
        }

      

        [HttpGet]
        [ActionName("GetAllCasualWorkersForAParticularBranch")]
        public IEnumerable<CasualWorker> GetAllCasualWorkersForAParticularBranch()
        {
            return _casualWorkerService.GetAllCasualWorkersForAParticularBranch(branchId);
        }

        [HttpGet]
        [ActionName("Delete")]
        public void DeleteCasualWorker(long casualWorkerId)
        {
            _casualWorkerService.MarkAsDeleted(casualWorkerId, userId);
        }



        [HttpPost]
        [ActionName("Save")]
        public long Save(CasualWorker model)
        {
            model.BranchId = branchId;
            var casualWorkerId = _casualWorkerService.SaveCasualWorker(model, userId);
            return casualWorkerId;
        }
    }
}
