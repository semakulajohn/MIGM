using System.Collections.Generic;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DTO;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IInventoryPurchaseDataService
    {
        IEnumerable<InventoryPurchase> GetAllInventoryPurchases();
        InventoryPurchase GetInventoryPurchase(long inventoryId);
        long SaveInventoryPurchase(InventoryPurchaseDTO inventoryPurchase, string userId);
        void MarkAsDeleted(long inventoryPurchaseId, string userId);

        //IEnumerable<InventoryCategory> GetAllInventoryCategories();
        IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularBranch(long branchId);

        IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularStore(long storeId);


    }
}
