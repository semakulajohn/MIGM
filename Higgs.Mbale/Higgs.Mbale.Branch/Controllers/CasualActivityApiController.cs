using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class CasualActivityApiController : ApiController
    {
         private ICasualActivityService _casualActivityService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(CasualActivityApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public CasualActivityApiController()
            {
            }

            public CasualActivityApiController(ICasualActivityService casualActivityService,IUserService userService)
            {
                this._casualActivityService = casualActivityService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }

            [HttpGet]
            [ActionName("GetCasualActivity")]
            public CasualActivity GetCasualActivity(long casualActivityId)
            {
                return _casualActivityService.GetCasualActivity(casualActivityId);
            }

            [HttpGet]
            [ActionName("GetAllCasualActivities")]
            public IEnumerable<CasualActivity> GetAllCasualActivities()
            {
                return _casualActivityService.GetAllCasualActivities();
            }

            [HttpGet]
            [ActionName("GetAllCasualActivitiesForAParticularCasualWorker")]
            public IEnumerable<CasualActivity> GetAllCasualActivitiesForAParticularCasualWorker(long casualWorkerId)
            {
                return _casualActivityService.GetAllCasualActivitiesForAParticularCasualWorker(casualWorkerId);
            }

            [HttpGet]
            [ActionName("GetAllCasualActivitiesForAParticularBatch")]
            public IEnumerable<CasualActivity> GetAllCasualActivitiesForAParticularBatch(long batchId)
            {
                return _casualActivityService.GetAllCasualActivitiesForAParticularBatch(batchId);
            }


            [HttpGet]
            [ActionName("GetAllCasualActivitiesForAParticularBranch")]
            public IEnumerable<CasualActivity> GetAllCasualActivitiesForAParticularBranch()
            {
                return _casualActivityService.GetAllCasualActivitiesForAParticularBranch(branchId);
            }
            [HttpGet]
            [ActionName("Delete")]
            public void DeleteCasualActivity(long casualActivityId)
            {
                _casualActivityService.MarkAsDeleted(casualActivityId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(CasualActivity model)
            {
                model.BranchId = branchId;
                var casualActivityId = _casualActivityService.SaveCasualActivity(model, userId);
                return casualActivityId;
            }
    }
}
