using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using System.Configuration;

namespace Higgs.Mbale.Branch.Controllers
{
    public class BuveraTransferApiController : ApiController
    {
           private IBuveraTransferService _buveraTransferService;
            private IUserService _userService;
            private IStoreService _storeService;
            ILog logger = log4net.LogManager.GetLogger(typeof(BuveraTransferApiController));
            private string userId = string.Empty;
            long branchId = 0,storeId = 0;

            public BuveraTransferApiController()
            {
            }

            public BuveraTransferApiController(IBuveraTransferService buveraTransferService,IUserService userService,IStoreService storeService)
            {
                this._buveraTransferService = buveraTransferService;
                this._userService = userService;
                this._storeService = storeService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
                storeId = _storeService.GetAStoreForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetBuveraTransfer")]
            public BuveraTransfer GetBuveraTransfer(long buveraTransferId)
            {
                return _buveraTransferService.GetBuveraTransfer(buveraTransferId);
            }

            [HttpGet]
            [ActionName("GetAllBuveraTransfers")]
            public IEnumerable<BuveraTransfer> GetAllBuveraTransfers()
            {
                return _buveraTransferService.GetAllBuveraTransfers();
            }

            [HttpGet]
            [ActionName("GetAllBuveraTransfersForAparticularStore")]
            public IEnumerable<BuveraTransfer> GetAllBuveraTransfersForAparticularStore()
            {
                return _buveraTransferService.GetAllBuveraTransfersForAParticularStore(storeId);
            }

            [HttpGet]
            [ActionName("GetStoreBuveraTransferStock")]
            public StoreGrade GetStoreBuveraTransferStock()
            {
                return _buveraTransferService.GetStoreBuveraTransferStock(storeId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteBuveraTransfer(long BuveraTransferId)
            {
                _buveraTransferService.MarkAsDeleted(BuveraTransferId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(BuveraTransfer model)
            {
                model.BranchId = branchId;
                model.StoreId = storeId;
                var buveraTransferId = _buveraTransferService.SaveBuveraTransfer(model, userId);
                return buveraTransferId;
            }

            [HttpPost]
            [ActionName("Accept")]
            public long Accept(BuveraTransfer model)
            {
                var buveraTransferId = _buveraTransferService.AcceptBuvera(model, userId);
                return buveraTransferId;
            }

            [HttpPost]
            [ActionName("Reject")]
            public long Reject(BuveraTransfer model)
            {
                var buveraTransferId = _buveraTransferService.RejectBuvera(model, userId);
                return buveraTransferId;
            }
    }
}

    

