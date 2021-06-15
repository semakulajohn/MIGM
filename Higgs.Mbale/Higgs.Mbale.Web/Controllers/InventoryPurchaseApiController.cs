using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class InventoryPurchaseApiController : ApiController
    {
       
        private IInventoryPurchaseService _inventoryPurchaseService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(InventoryApiController));
        private string userId = string.Empty;

        public InventoryPurchaseApiController()
        {
        }

        public InventoryPurchaseApiController( IUserService userService, IInventoryPurchaseService inventoryPurchaseService)
        {
            
            this._inventoryPurchaseService = inventoryPurchaseService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetInventoryPurchase")]
        public InventoryPurchase GetInventoryPurchase(long inventoryPurchaseId)
        {
            return _inventoryPurchaseService.GetInventoryPurchase(inventoryPurchaseId);
        }

        [HttpGet]
        [ActionName("GetAllInventoryPurchases")]
        public IEnumerable<InventoryPurchase> GetAllInventoryPurchases()
        {
            return _inventoryPurchaseService.GetAllInventoryPurchases();
        }



        [HttpGet]
        [ActionName("GetAllInventoryPurchasesForParticularStore")]
        public IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForParticularStore(long storeId)
        {
            return _inventoryPurchaseService.GetAllInventoryPurchasesForAParticularStore(storeId);
        }

       
        [HttpGet]
        [ActionName("Delete")]
        public void DeleteInventoryPurchase(long inventoryId)
        {
            _inventoryPurchaseService.MarkAsDeleted(inventoryId, userId);
        }

        [HttpPost]
        [ActionName("Save")]
        public long Save(InventoryPurchase model)
        {
            var inventoryPurchaseId = _inventoryPurchaseService.SaveInventoryPurchase(model, userId);
            return inventoryPurchaseId;
        }
    }
}