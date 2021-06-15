using System;
using System.Collections.Generic;

using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class LabourCostApiController : ApiController
    {
         private ILabourCostService _labourCostService;
            private IUserService _userService;
            private IBatchService _batchService;
            ILog logger = log4net.LogManager.GetLogger(typeof(LabourCostApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public LabourCostApiController()
            {
            }

            public LabourCostApiController(ILabourCostService labourCostService,IUserService userService,IBatchService batchService)
            {
                this._labourCostService = labourCostService;
                this._userService = userService;
                this._batchService = batchService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }

            [HttpGet]
            [ActionName("GetLabourCost")]
            public LabourCost GetLabourCost(long labourCostId)
            {
                return _labourCostService.GetLabourCost(labourCostId);
            }

          

            [HttpGet]
            [ActionName("GetAllLabourCostsForAParticularBatch")]
            public IEnumerable<LabourCost> GetAllLabourCostsForAParticularBatch(long batchId)
            {
                return _labourCostService.GetAllLabourCostsForAParticularBatch(batchId);
            }

            [HttpGet]
            [ActionName("GenerateLabourCosts")]
            public IEnumerable<LabourCost> GenerateLabourCosts(long batchId)
            {
                return _batchService.GenerateLabourCosts(batchId,userId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteLabourCost(long labourCostId)
            {
                _labourCostService.MarkAsDeleted(labourCostId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(LabourCost model)
            {
                model.BranchId = branchId;
                var labourCostId = _labourCostService.SaveLabourCost(model, userId);
                return labourCostId;
            }
    }
}
