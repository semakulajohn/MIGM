using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using System.Configuration;

namespace Higgs.Mbale.Web.Controllers
{
    public class FlourTransferApiController : ApiController
    {
           private IFlourTransferService _flourTransferService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(FlourTransferApiController));
            private string userId = string.Empty;

            public FlourTransferApiController()
            {
            }

            public FlourTransferApiController(IFlourTransferService flourTransferService,IUserService userService)
            {
                this._flourTransferService = flourTransferService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetFlourTransfer")]
            public FlourTransfer GetFlourTransfer(long flourTransferId)
            {
                return _flourTransferService.GetFlourTransfer(flourTransferId);
            }

            [HttpGet]
            [ActionName("GetAllFlourTransfers")]
            public IEnumerable<FlourTransfer> GetAllFlourTransfers()
            {
                return _flourTransferService.GetAllFlourTransfers();
            }

            [HttpGet]
            [ActionName("GetAllFlourTransfersForAparticularStore")]
            public IEnumerable<FlourTransfer> GetAllFlourTransfersForAparticularStore(long storeId)
            {
                return _flourTransferService.GetAllFlourTransfersForAParticularStore(storeId);
            }

            [HttpGet]
            [ActionName("GetStoreFlourTransferStock")]
            public StoreGrade GetStoreFlourTransferStock(long storeId)
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
