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
    public class BuveraTransferApiController : ApiController
    {
           private IBuveraTransferService _buveraTransferService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(BuveraTransferApiController));
            private string userId = string.Empty;

            public BuveraTransferApiController()
            {
            }

            public BuveraTransferApiController(IBuveraTransferService buveraTransferService,IUserService userService)
            {
                this._buveraTransferService = buveraTransferService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
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
            public IEnumerable<BuveraTransfer> GetAllBuveraTransfersForAparticularStore(long storeId)
            {
                return _buveraTransferService.GetAllBuveraTransfersForAParticularStore(storeId);
            }

            [HttpGet]
            [ActionName("GetStoreBuveraTransferStock")]
            public StoreGrade GetStoreBuveraTransferStock(long storeId)
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

    

