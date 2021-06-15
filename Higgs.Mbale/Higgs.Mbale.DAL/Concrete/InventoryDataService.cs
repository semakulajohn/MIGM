using System;
using System.Collections.Generic;
using System.Linq;

using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Concrete;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;

namespace Higgs.Mbale.DAL.Concrete
{
 public   class InventoryDataService : DataServiceBase,IInventoryDataService
    {
        
       ILog logger = log4net.LogManager.GetLogger(typeof(InventoryDataService));

       public InventoryDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }
              
        public IEnumerable<Inventory> GetAllInventories()
        {
            return this.UnitOfWork.Get<Inventory>().AsQueryable().Where(e => e.Deleted == false).OrderByDescending(e => e.CreatedOn).Take(10); 
        }

        public Inventory GetInventory(long InventoryId)
        {
            return this.UnitOfWork.Get<Inventory>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.InventoryId == InventoryId &&
                    c.Deleted == false
                );
        }
      
        public IEnumerable<Inventory> GetAllInventoriesForAParticularInventoryCategory(long categoryId)
        {
            return this.UnitOfWork.Get<Inventory>().AsQueryable().Where(e => e.Deleted == false && e.InventoryCategoryId == categoryId);
        }

       
        /// <summary>
        /// Saves a new Inventory or updates an already existing Inventory.
        /// </summary>
        /// <param name="Inventory">Inventory to be saved or updated.</param>
        /// <param name="InventoryId">InventoryId of the Inventory creating or updating</param>
        /// <returns>InventoryId</returns>
        public long SaveInventory(InventoryDTO inventoryDTO, string userId)
        {
            long inventoryId = 0;
            
            if (inventoryDTO.InventoryId == 0)
            {

                var inventory = new Inventory()
                {
                    ItemName = inventoryDTO.ItemName,
                    Description = inventoryDTO.Description,
                    Price = inventoryDTO.Price,
                   
                    InventoryCategoryId = inventoryDTO.InventoryCategoryId,
                    
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                };

                this.UnitOfWork.Get<Inventory>().AddNew(inventory);
                this.UnitOfWork.SaveChanges();
                inventoryId = inventory.InventoryId;
                return inventoryId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Inventory>().AsQueryable()
                    .FirstOrDefault(e => e.InventoryId == inventoryDTO.InventoryId);
                if (result != null)
                {
                    result.Description = inventoryDTO.Description;
                   
                    result.Price = inventoryDTO.Price;
                   
                    result.InventoryCategoryId = inventoryDTO.InventoryCategoryId;
                   
                    result.ItemName = inventoryDTO.ItemName;
                   
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = inventoryDTO.Deleted;
                    result.DeletedBy = inventoryDTO.DeletedBy;
                    result.DeletedOn = inventoryDTO.DeletedOn;

                    this.UnitOfWork.Get<Inventory>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return inventoryDTO.InventoryId;
            }            
        }

        public void MarkAsDeleted(long inventoryId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
              //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }


        #region inventory category
        public IEnumerable<InventoryCategory> GetAllInventoryCategories()
        {
            return this.UnitOfWork.Get<InventoryCategory>().AsQueryable();
        }
        #endregion
    }
}
