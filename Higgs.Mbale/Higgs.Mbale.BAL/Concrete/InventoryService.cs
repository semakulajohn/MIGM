using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class InventoryService : IInventoryService
    {
     ILog logger = log4net.LogManager.GetLogger(typeof(InventoryService));
        private IInventoryDataService _dataService;
        private IUserService _userService;
        private ITransactionDataService _transactionDataService;
        private ITransactionSubTypeService _transactionSubTypeService;
        

        public InventoryService(IInventoryDataService dataService,IUserService userService,ITransactionDataService transactionDataService,ITransactionSubTypeService transactionSubTypeService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionDataService = transactionDataService;
            this._transactionSubTypeService = transactionSubTypeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryId"></param>
        /// <returns></returns>
        public Inventory GetInventory(long inventoryId)
        {
            var result = this._dataService.GetInventory(inventoryId);
            return MapEFToModel(result);
        }
     

        public IEnumerable<Inventory> GetAllInventoriesForAParticularInventoryCategory( long categoryId)
        {
            var results = this._dataService.GetAllInventoriesForAParticularInventoryCategory( categoryId);
            return MapEFToModel(results);
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Inventory> GetAllInventories()
        {
            var results = this._dataService.GetAllInventories();
            return MapEFToModel(results);
        } 

       
        public long SaveInventory(Inventory inventory, string userId)
        {
            var inventoryDTO = new DTO.InventoryDTO()
            {
                ItemName = inventory.ItemName,
                Description = inventory.Description,
             
                Price = inventory.Price,
                
                InventoryCategoryId = inventory.InventoryCategoryId,
               
                InventoryId = inventory.InventoryId,
                Deleted = inventory.Deleted,
                CreatedBy = inventory.CreatedBy,
                CreatedOn = inventory.CreatedOn

            };

           var InventoryId = this._dataService.SaveInventory(inventoryDTO, userId);

          
           
           return InventoryId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long inventoryId, string userId)
        {
            _dataService.MarkAsDeleted(inventoryId, userId);
        }

        #region inventorycategory
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InventoryCategory> GetAllInventoryCategories()
        {
            var results = this._dataService.GetAllInventoryCategories();
            return MapEFToModel(results);
        } 
        #endregion

        #region Mapping Methods

        private IEnumerable<Inventory> MapEFToModel(IEnumerable<EF.Models.Inventory> data)
        {
            var list = new List<Inventory>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Inventory EF object to Inventory Model Object and
        /// returns the Inventory model object.
        /// </summary>
        /// <param name="result">EF Inventory object to be mapped.</param>
        /// <returns>Inventory Model Object.</returns>
        public Inventory MapEFToModel(EF.Models.Inventory data)
        {
            if (data != null)
            {

                var inventory = new Inventory()
                {
                    ItemName = data.ItemName,
                   
                    Description = data.Description,
                  
                    Price = data.Price,
                   CategoryName = data.InventoryCategory != null ? data.InventoryCategory.Name : "",
                    InventoryCategoryId = data.InventoryCategoryId,
                   
                    InventoryId = data.InventoryId,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };
                return inventory;
            }
            return null;
        }


        private IEnumerable<InventoryCategory> MapEFToModel(IEnumerable<EF.Models.InventoryCategory> data)
        {
            var list = new List<InventoryCategory>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Inventory Category EF object to Inventory Category Model Object and
        /// returns the Inventory Category model object.
        /// </summary>
        /// <param name="result">EF Inventory Category object to be mapped.</param>
        /// <returns>Inventory Category Model Object.</returns>
        public InventoryCategory MapEFToModel(EF.Models.InventoryCategory data)
        {
            if (data != null)
            {
                var inventoryCategory = new InventoryCategory()
                {
                    InventoryCategoryId = data.InventoryCategoryId,
                    Name = data.Name,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,


                };
                return inventoryCategory;
            }
            return null;
        }


       #endregion
    }
}
