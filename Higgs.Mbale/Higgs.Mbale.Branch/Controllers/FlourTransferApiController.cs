using System;
using System.Collections.Generic;

using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;


namespace Higgs.Mbale.Branch.Controllers
{
    public class FlourTransferApiController : ApiController
    {
           private IFlourTransferService _flourTransferService;
            private IUserService _userService;
            private IStoreService _storeService;
            ILog logger = log4net.LogManager.GetLogger(typeof(FlourTransferApiController));
            private string userId = string.Empty;
            long branchId = 0,storeId = 0;

            public FlourTransferApiController()
            {
            }

            public FlourTransferApiController(IFlourTransferService flourTransferService,IUserService userService,IStoreService storeService)
            {
                this._flourTransferService = flourTransferService;
                this._userService = userService;
                this._storeService = storeService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
                storeId = _storeService.GetAStoreForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetFlourTransfer")]
            public FlourTransfer GetFlourTransfer(long flourTransferId)
            {
                return _flourTransferService.GetFlourTransfer(flourTransferId);
            }

          

            [HttpGet]
            [ActionName("GetAllFlourTransfersForAparticularStore")]
            public IEnumerable<FlourTransfer> GetAllFlourTransfersForAparticularStore()
            {
                return _flourTransferService.GetAllFlourTransfersForAParticularStore(storeId);
            }

            [HttpGet]
            [ActionName("GetStoreFlourTransferStock")]
            public StoreGrade GetStoreFlourTransferStock()
            {
                return _flourTransferService.GetStoreFlourTransferStock(storeId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteFlourTransfer(long FlourTransferId)
            {
                _flourTransferService.MarkAsDeleted(FlourTransferId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(FlourTransfer model)
            {
              
                model.StoreId = storeId;
                model.FromSupplierStoreId = storeId;
                var FlourTransferId = _flourTransferService.SaveFlourTransfer(model, userId);
                return FlourTransferId;
            }

            [HttpPost]
            [ActionName("Accept")]
            public long Accept(FlourTransfer model)
            {
                var FlourTransferId = _flourTransferService.AcceptFlour(model, userId);
                return FlourTransferId;
            }

            [HttpPost]
            [ActionName("Reject")]
            public long Reject(FlourTransfer model)
            {
                var FlourTransferId = _flourTransferService.RejectFlour(model, userId);
                return FlourTransferId;
            }
    }
}
