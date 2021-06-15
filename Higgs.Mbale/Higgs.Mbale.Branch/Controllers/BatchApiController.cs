using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Branch.Controllers
{
    public class BatchApiController : ApiController
    {
         private IBatchService _batchService;
            private IUserService _userService;
            private IStoreService _storeService;
            ILog logger = log4net.LogManager.GetLogger(typeof(BatchApiController));
            private string userId = string.Empty;
            long branchId = 0,storeId = 0;


            public BatchApiController()
            {
            }

            public BatchApiController(IBatchService batchService,IUserService userService,IStoreService storeService)
            {
                this._batchService = batchService;
                this._userService = userService;
                this._storeService = storeService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
                storeId = _storeService.GetAStoreForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetBatch")]
            public Batch GetBatch(long batchId)
            {
                return _batchService.GetBatch(batchId);
            }

            [HttpGet]
            [ActionName("GetAllBatches")]
            public IEnumerable<Batch> GetAllBatches()
            {
                return _batchService.GetAllBatches();
            }

            [HttpGet]
            [ActionName("GetLatestBatchesForAParticularBranch")]
            public IEnumerable<BatchViewModel> GetLatestBatchesForAParticularBranch()
            {
                return _batchService.GetTenBatchesForAParticularBranch(branchId);
            }
            [HttpGet]
            [ActionName("GetAllBatchesForAParticularBranch")]
            public IEnumerable<BatchViewModel> GetAllBatchesForAParticularBranch()
            {
                return _batchService.GetTenBatchesForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetAllBatchesForAParticularBranchToTransfer")]
            public IEnumerable<Batch> GetAllBatchesForAParticularBranchToTransfer(long productId)
            {
                return _batchService.GetBatchesForAParticularBranchToTransfer(branchId,productId);
            }
            [HttpGet]
            [ActionName("GetAllBatchesForBrandDelivery")]
            public IEnumerable<BatchViewModel> GetAllBatchesForBrandDelivery()
            {
                return _batchService.GetAllBatchesForBrandDelivery(branchId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteBatch(long batchId)
            {
                _batchService.MarkAsDeleted(batchId, userId);
            }

         

            [HttpPost]
            [ActionName("Save")]
            public long Save(Batch model)
            {
                model.BranchId = branchId;
                model.StoreId = storeId;
                var batchId = _batchService.SaveBatch(model, userId);
                return batchId;
            }
    }
}
