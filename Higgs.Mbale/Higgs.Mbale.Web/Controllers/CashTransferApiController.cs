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
    public class CashTransferApiController : ApiController
    {
         private ICashTransferService _cashTransferService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(CashTransferApiController));
            private string userId = string.Empty;

            public CashTransferApiController()
            {
            }

            public CashTransferApiController(ICashTransferService cashTransferService,IUserService userService)
            {
                this._cashTransferService = cashTransferService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetCashTransfer")]
            public CashTransfer GetCashTransfer(long cashTransferId)
            {
                return _cashTransferService.GetCashTransfer(cashTransferId);
            }

            [HttpGet]
            [ActionName("GetAllCashTransfers")]
            public IEnumerable<CashTransfer> GetAllCashTransfers()
            {
                return _cashTransferService.GetAllCashTransfers();
            }

            [HttpGet]
            [ActionName("GetAllCashTransfersForAparticularBranch")]
            public IEnumerable<CashTransfer> GetAllCashTransfersForAparticularBranch(long branchId)
            {
                return _cashTransferService.GetLatestTenCashTransfersForParticularBranch(branchId);
            }
            
           [HttpGet]
           [ActionName("GetAllUnApprovedCashTransfers")]
           public IEnumerable<CashTransfer> GetAllUnApprovedCashTransfers()
            {
                return _cashTransferService.GetAllUnApprovedCashTransfers();
            }



            [HttpGet]
            [ActionName("Delete")]
            public void DeleteCashTransfer(long cashTransferId)
            {
                _cashTransferService.MarkAsDeleted(cashTransferId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(CashTransfer model)
            {
                var cashTransferId = _cashTransferService.SaveCashTransfer(model, userId);
                return cashTransferId;
            }

            [HttpPost]
            [ActionName("Accept")]
            public long Accept(CashTransfer model)
            {
                var cashTransferId = _cashTransferService.AcceptCashTransfer(model, userId);
                return cashTransferId;
            }

            [HttpPost]
            [ActionName("Reject")]
            public long Reject(CashTransfer model)
            {
                var cashTransferId = _cashTransferService.RejectCashTransfer(model, userId);
                return cashTransferId;
            }


    }
}
