using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class BatchProjectionApiController : ApiController
    {


        private IBatchProjectionService _batchProjectionService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(BatchProjectionApiController));
        private string userId = string.Empty;
        long branchId = 0;

        public BatchProjectionApiController()
        {
        }

        public BatchProjectionApiController(IBatchProjectionService batchProjectionService, IUserService userService)
        {
            this._batchProjectionService = batchProjectionService;
            this._userService = userService;
           
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
       
            branchId = _userService.GetLoggedUserBranchId(userId);
            
        }

        [HttpGet]
        [ActionName("GetBatchProjection")]
        public BatchProjection GetBatchProjection(long batchProjectionId)
        {
            return _batchProjectionService.GetBatchProjection(batchProjectionId);
        }

        [HttpGet]
        [ActionName("GetAllBatchProjections")]
        public IEnumerable<BatchProjection> GetAllBatchProjections()
        {
            return _batchProjectionService.GetAllBatchProjections();
        }

        [HttpGet]
        [ActionName("GetAllBatchProjectionsForAParticularBatch")]
        public IEnumerable<BatchProjection> GetAllBatchProjectionsForAParticularBatch(long batchId)
        {
            return _batchProjectionService.GetAllBatchProjectionsForAParticularBatch(batchId);
        }


        [HttpGet]
        [ActionName("Delete")]
        public void DeleteBatchProjection(long batchProjectionId)
        {
            _batchProjectionService.MarkAsDeleted(batchProjectionId, userId);
        }



        [HttpPost]
        [ActionName("Save")]
        public long Save(BatchProjection model)
        {
            model.BranchId = branchId;
        
            var batchProjectionId = _batchProjectionService.SaveBatchProjection(model, userId);
            return batchProjectionId;
        }
    }
}
