using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Branch.Controllers
{
    public class RequistionApiController : ApiController
    {
         private IRequistionService _requistionService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(RequistionApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public RequistionApiController()
            {
            }

            public RequistionApiController(IRequistionService requistionService,IUserService userService)
            {
                this._requistionService = requistionService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId); 
            }

            [HttpGet]
            [ActionName("GetRequistion")]
            public Requistion GetRequistion(long requistionId)
            {
                return _requistionService.GetRequistion(requistionId);
            }

          
            [HttpGet]
            [ActionName("GetAllRequistionsForAParticularStatusForBranch")]
            public IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularStatusForBranch(long statusId)
            {
                return _requistionService.GetLatestThirtyRequistionsForAParticularStatusForBranch(statusId,branchId);
            }

           
            [HttpGet]
            [ActionName("GetAllRequistionsForAParticularBranch")]
            public IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularBranch()
            {
                return _requistionService.GetLatestSixtyRequistionsForAParticularBranch(branchId);
            }
            [HttpGet]
            [ActionName("Delete")]
            public void DeleteRequistion(long requistionId)
            {
                _requistionService.MarkAsDeleted(requistionId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(Requistion model)
            {
                model.BranchId = branchId;
                var requistionId = _requistionService.SaveRequistion(model, userId);
                return requistionId;
            }

        [HttpGet]
        [ActionName("GetAllRequistionCategories")]
        public IEnumerable<RequistionCategory> GetAllRequistionCategories()
        {
            return _requistionService.GetAllRequistionCategories();
        }

    }
}
