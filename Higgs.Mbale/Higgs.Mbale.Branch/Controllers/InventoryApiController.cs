using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class InventoryApiController : ApiController
    {
         private IInventoryService _inventoryService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(InventoryApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public InventoryApiController()
            {
            }

            public InventoryApiController(IInventoryService inventoryService,IUserService userService)
            {
                this._inventoryService = inventoryService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }

            [HttpGet]
            [ActionName("GetInventory")]
            public Inventory GetInventory(long inventoryId)
            {
                return _inventoryService.GetInventory(inventoryId);
            }

            [HttpGet]
            [ActionName("GetAllInventories")]
            public IEnumerable<Inventory> GetAllInventories()
            {
                return _inventoryService.GetAllInventories();
            }

            //[HttpGet]
            //[ActionName("GetAllInventoriesForParticularStore")]
            //public IEnumerable<Inventory> GetAllInventoriesForAParticularStore(long storeId)
            //{
            //    return _inventoryService.GetAllInventoriesForAParticularStore(storeId);
            //}

            //[HttpGet]
            //[ActionName("GetAllInventoriesForParticularStoreForAParticularCategory")]
            //public IEnumerable<Inventory> GetAllInventoriesForAParticularStore(long storeId,long categoryId)
            //{
            //    return _inventoryService.GetAllInventoriesForAParticularStoreInAParticularInventoryCategory(storeId,categoryId);
            //}

            [HttpGet]
            [ActionName("GetAllInventoryCategories")]
            public IEnumerable<InventoryCategory> GetAllInventoryCategories()
            {
                return _inventoryService.GetAllInventoryCategories();
            }
            [HttpGet]
            [ActionName("Delete")]
            public void DeleteInventory(long inventoryId)
            {
                _inventoryService.MarkAsDeleted(inventoryId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(Inventory model)
            {
           
                var InventoryId = _inventoryService.SaveInventory(model, userId);
                return InventoryId;
            }
    }
}
