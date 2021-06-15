using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class InventoryPurchaseService
    {

        ILog logger = log4net.LogManager.GetLogger(typeof(InventoryPurchaseService));
        private IInventoryPurchaseDataService _dataService;
        private IUserService _userService;
        private ITransactionSubTypeService _transactionSubTypeService;


        public InventoryPurchaseService(IInventoryPurchaseDataService dataService, IUserService userService, ITransactionSubTypeService transactionSubTypeService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionSubTypeService = transactionSubTypeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryPurchaseId"></param>
        /// <returns></returns>
        public InventoryPurchase GetInventoryPurchase(long inventoryPurchaseId)
        {
            var result = this._dataService.GetInventoryPurchase(inventoryPurchaseId);
            return MapEFToModel(result);
        }
        public IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllInventoryPurchasesForAParticularBranch(branchId);
            return MapEFToModel(results);
        }

        //public IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularInventoryCategory(long categoryId)
        //{
        //    var results = this._dataService.GetAllInventoriesForAParticularInventoryCategory(categoryId);
        //    return MapEFToModel(results);
        //}

        public IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularStore(long storeId)
        {
            var results = this._dataService.GetAllInventoryPurchasesForAParticularStore(storeId);
            return MapEFToModel(results);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InventoryPurchase> GetAllInventoryPurchases()
        {
            var results = this._dataService.GetAllInventoryPurchases();
            return MapEFToModel(results);
        }


        public long SaveInventoryPurchase(InventoryPurchase inventoryPurchase, string userId)
        {
            var inventoryPurchaseDTO = new DTO.InventoryPurchaseDTO()
            {
                ItemName = inventoryPurchase.ItemName,
                Description = inventoryPurchase.Description,
                PurchaseDate = inventoryPurchase.PurchaseDate,
                Price = inventoryPurchase.Price,
                Quantity = inventoryPurchase.Quantity,
                InventoryPurchaseId = inventoryPurchase.InventoryPurchaseId,
                Amount = inventoryPurchase.Amount,
                BranchId = inventoryPurchase.BranchId,
                SectorId = inventoryPurchase.SectorId,
                StoreId = inventoryPurchase.StoreId,
                TransactionSubTypeId = inventoryPurchase.TransactionSubTypeId,
                InventoryId = inventoryPurchase.InventoryId,
                Deleted = inventoryPurchase.Deleted,
                CreatedBy = inventoryPurchase.CreatedBy,
                CreatedOn = inventoryPurchase.CreatedOn

            };

            var InventoryPurchaseId = this._dataService.SaveInventoryPurchase(inventoryPurchaseDTO, userId);

            
            return InventoryPurchaseId;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryPurchaseId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long inventoryPurchaseId, string userId)
        {
            _dataService.MarkAsDeleted(inventoryPurchaseId, userId);
        }

      
        #region Mapping Methods

        private IEnumerable<InventoryPurchase> MapEFToModel(IEnumerable<EF.Models.InventoryPurchase> data)
        {
            var list = new List<InventoryPurchase>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Inventory Purchase EF object to Inventory Model Object and
        /// returns the Inventory Purchase model object.
        /// </summary>
        /// <param name="result">EF Inventory Purchase object to be mapped.</param>
        /// <returns>Inventory Purchase Model Object.</returns>
        public InventoryPurchase MapEFToModel(EF.Models.InventoryPurchase data)
        {
            if (data != null)
            {



                var inventoryPurchase = new InventoryPurchase()
                {
                    ItemName = data.ItemName,
                    Amount = data.Amount,
                    Description = data.Description,
                    PurchaseDate = data.PurchaseDate,
                    Price = data.Price,
                    Quantity = data.Quantity,
                   
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    TransactionSubTypeId = data.TransactionSubTypeId,
                    TransactionSubTypeName = data.TransactionSubType != null ? data.TransactionSubType.Name : "",
                    StoreName = data.Store != null ? data.Store.Name : "",
                    BranchId = data.BranchId,
                    StoreId = data.StoreId,
                    SectorId = data.SectorId,
                    InventoryId = data.InventoryId,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };
                return inventoryPurchase;
            }
            return null;
        }


    

        #endregion
    }
}
