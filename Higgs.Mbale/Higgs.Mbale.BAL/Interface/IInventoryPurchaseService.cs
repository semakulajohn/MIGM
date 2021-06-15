using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface IInventoryPurchaseService
    {
        IEnumerable<InventoryPurchase> GetAllInventoryPurchases();
        InventoryPurchase GetInventoryPurchase(long inventoryPurchaseId);
        long SaveInventoryPurchase(InventoryPurchase inventoryPurchase, string userId);
        void MarkAsDeleted(long inventoryPurchaseId, string userId);
        IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularBranch(long branchId);
        IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularStore(long storeId);

    }
}
