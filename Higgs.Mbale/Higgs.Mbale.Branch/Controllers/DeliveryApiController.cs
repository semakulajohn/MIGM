using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class DeliveryApiController : ApiController
    {
            private IDeliveryService _deliveryService;
            private IUserService _userService;
            private IStoreService _storeService;
            ILog logger = log4net.LogManager.GetLogger(typeof(DeliveryApiController));
            private string userId = string.Empty;
            long branchId = 0 , storeId=0;


            public DeliveryApiController()
            {
            }

            public DeliveryApiController(IDeliveryService deliveryService,IUserService userService,IStoreService storeService)
            {
                this._deliveryService = deliveryService;
                this._userService = userService;
                this._storeService = storeService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
                storeId = _storeService.GetAStoreForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetDelivery")]
            public Delivery GetDelivery(long deliveryId)
            {
                return _deliveryService.GetDelivery(deliveryId);
            }

           
            [HttpGet]
            [ActionName("GetAllBranchDeliveries")]
            public IEnumerable<Delivery> GetAllBranchDeliveries()
            {
                return _deliveryService.GetAllDeliveriesForAParticularBranch(branchId);
            }
            [HttpGet]
            [ActionName("GetAllDeliveriesForAParticularOrder")]
            public IEnumerable<Delivery> GetAllDeliveriesForAParticularOrder(long orderId)
            {
                return _deliveryService.GetAllDeliveriesForAParticularOrder(orderId);
            }
             [HttpGet]
             [ActionName("GetAllBranchUnApprovedDeliveries")]
             public IEnumerable<Delivery> GetAllBranchUnApprovedDeliveries()
            {
              
                return _deliveryService.GetAllBranchUnApprovedDeliveries(Convert.ToInt64(branchId));
            }

        [HttpGet]
        [ActionName("GetAllBranchApprovedDeliveries")]
        public IEnumerable<Delivery> GetAllBranchApprovedDeliveries()
        {

            return _deliveryService.GetAllBranchApprovedDeliveries(branchId);
        }

        [HttpGet]
        [ActionName("GetAllBranchRejectedDeliveries")]
        public IEnumerable<Delivery> GetAllBranchRejectedDeliveries()
        {

            return _deliveryService.GetAllBranchRejectedDeliveries(branchId);
        }

        [HttpGet]
            [ActionName("Delete")]
            public void DeleteDelivery(long deliveryId)
            {
                _deliveryService.MarkAsDeleted(deliveryId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(Delivery model)
            {
                model.BranchId = branchId;
                model.StoreId = storeId;
                var deliveryId = _deliveryService.SaveDelivery(model, userId);
                return deliveryId;
            }
    }
}
