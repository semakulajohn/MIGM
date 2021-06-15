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
 public   class InventoryPurchaseDataService : DataServiceBase,IInventoryPurchaseDataService
    {

        ILog logger = log4net.LogManager.GetLogger(typeof(InventoryPurchaseDataService));

        public InventoryPurchaseDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
        {

        }

        public IEnumerable<InventoryPurchase> GetAllInventoryPurchases()
        {
            return this.UnitOfWork.Get<InventoryPurchase>().AsQueryable().Where(e => e.Deleted == false);
        }

        public InventoryPurchase GetInventoryPurchase(long InventoryPurchaseId)
        {
            return this.UnitOfWork.Get<InventoryPurchase>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.InventoryPurchaseId == InventoryPurchaseId &&
                    c.Deleted == false
                );
        }
        public IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<InventoryPurchase>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
        }

        public IEnumerable<InventoryPurchase> GetAllInventoryPurchasesForAParticularStore(long storeId)
        {
            return this.UnitOfWork.Get<InventoryPurchase>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId ).OrderByDescending(e => e.CreatedOn).Take(10);
        }

       
        /// <summary>
        /// Saves a new Inventory Purchase or updates an already existing Inventory.
        /// </summary>
        /// <param name="InventoryPurchase">InventoryPurchase to be saved or updated.</param>
        /// <param name="InventoryPurchaseId">InventoryPurchaseId of the Inventory creating or updating</param>
        /// <returns>InventoryPurchaseId</returns>
        public long SaveInventoryPurchase(InventoryPurchaseDTO inventoryPurchaseDTO, string userId)
        {
            long inventoryPurchaseId = 0;

            if (inventoryPurchaseDTO.InventoryPurchaseId == 0)
            {

                var inventoryPurchase = new InventoryPurchase()
                {
                    ItemName = inventoryPurchaseDTO.ItemName,
                    Description = inventoryPurchaseDTO.Description,
                    Price = inventoryPurchaseDTO.Price,
                    Quantity = inventoryPurchaseDTO.Quantity,
                    
                    StoreId = inventoryPurchaseDTO.StoreId,
                    BranchId = inventoryPurchaseDTO.BranchId,
                    PurchaseDate = inventoryPurchaseDTO.PurchaseDate,
                    SectorId = inventoryPurchaseDTO.SectorId,
                    Amount = inventoryPurchaseDTO.Amount,
                    TransactionSubTypeId = inventoryPurchaseDTO.TransactionSubTypeId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                };

                this.UnitOfWork.Get<InventoryPurchase>().AddNew(inventoryPurchase);
                this.UnitOfWork.SaveChanges();
                inventoryPurchaseId = inventoryPurchase.InventoryPurchaseId;
                return inventoryPurchaseId;
            }

            else
            {
                var result = this.UnitOfWork.Get<InventoryPurchase>().AsQueryable()
                    .FirstOrDefault(e => e.InventoryPurchaseId == inventoryPurchaseDTO.InventoryPurchaseId);
                if (result != null)
                {
                    result.Description = inventoryPurchaseDTO.Description;
                    result.SectorId = inventoryPurchaseDTO.SectorId;
                    result.Price = inventoryPurchaseDTO.Price;
                    result.Quantity = inventoryPurchaseDTO.Quantity;
                    
                    result.StoreId = inventoryPurchaseDTO.StoreId;
                    result.PurchaseDate = inventoryPurchaseDTO.PurchaseDate;
                    result.ItemName = inventoryPurchaseDTO.ItemName;
                    result.TransactionSubTypeId = inventoryPurchaseDTO.TransactionSubTypeId;
                    result.BranchId = inventoryPurchaseDTO.BranchId;
                    result.Amount = inventoryPurchaseDTO.Amount;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = inventoryPurchaseDTO.Deleted;
                    result.DeletedBy = inventoryPurchaseDTO.DeletedBy;
                    result.DeletedOn = inventoryPurchaseDTO.DeletedOn;

                    this.UnitOfWork.Get<InventoryPurchase>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return inventoryPurchaseDTO.InventoryPurchaseId;
            }
        }

        public void MarkAsDeleted(long inventoryPurchaseId, string userId)
        {

            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }


        //#region inventory category
        //public IEnumerable<InventoryCategory> GetAllInventoryCategories()
        //{
        //    return this.UnitOfWork.Get<InventoryCategory>().AsQueryable();
        //}
        //#endregion
    }
}
