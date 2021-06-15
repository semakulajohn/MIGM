using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Concrete;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;

namespace Higgs.Mbale.DAL.Concrete
{
 public   class MaizeBrandStoreDataService : DataServiceBase,IMaizeBrandStoreDataService
    {
     ILog logger = log4net.LogManager.GetLogger(typeof(MaizeBrandStoreDataService));

       public MaizeBrandStoreDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

     

       public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStore()
        {
            return this.UnitOfWork.Get<MaizeBrandStore>().AsQueryable().Where(e => e.Deleted == false); 
        }

       public MaizeBrandStore GetMaizeBrandStore(long maizeBrandStoreId)
        {
            return this.UnitOfWork.Get<MaizeBrandStore>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.MaizeBrandStoreId == maizeBrandStoreId &&
                    c.Deleted == false
                );
        }

       public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranch(long branchId)
       {
           
            return this.UnitOfWork.Get<MaizeBrandStore>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId   );
        }


       public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBatch(long batchId)
       {
           return this.UnitOfWork.Get<MaizeBrandStore>().AsQueryable().Where(e => e.Deleted == false && e.BatchId == batchId);
     
       }

       public MaizeBrandStore GetLatestMaizeBrandStoreForAParticularBranch(long branchId)
       {

           MaizeBrandStore maizeBrandStore = new MaizeBrandStore();
           var maizeBrandStores = this.UnitOfWork.Get<MaizeBrandStore>().AsQueryable().Where(e => e.BranchId == branchId);
           if (maizeBrandStores.Any())
           {
              maizeBrandStore = maizeBrandStores.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
               return maizeBrandStore;
           }
           else
           {
               return maizeBrandStore;
           }
       }


       public long SaveMaizeBrandStore(MaizeBrandStoreDTO maizeBrandStoreDTO, string userId)
       {
           long maizeBrandStoreId = 0;

           if (maizeBrandStoreDTO.MaizeBrandStoreId == 0)
           {

               var maizeBrandStore = new MaizeBrandStore()
               {

                   MaizeBrandStoreId = maizeBrandStoreDTO.MaizeBrandStoreId,
                   Quantity = maizeBrandStoreDTO.Quantity,
                   StartQuantity = maizeBrandStoreDTO.StartQuantity,
                   BatchId = maizeBrandStoreDTO.BatchId,
                   
                  
                   Action = maizeBrandStoreDTO.Action,
                   Balance = maizeBrandStoreDTO.Balance,
                   
                   BranchId = maizeBrandStoreDTO.BranchId,
                   StoreId = maizeBrandStoreDTO.StoreId,
                   CreatedOn = DateTime.Now,
                   TimeStamp = DateTime.Now,
                   CreatedBy = userId,
                   Deleted = false,
               };

               this.UnitOfWork.Get<MaizeBrandStore>().AddNew(maizeBrandStore);
               this.UnitOfWork.SaveChanges();
               maizeBrandStoreId = maizeBrandStore.MaizeBrandStoreId;
               return maizeBrandStoreId;
           }

           else
           {
               var result = this.UnitOfWork.Get<MaizeBrandStore>().AsQueryable()
                   .FirstOrDefault(e => e.MaizeBrandStoreId == maizeBrandStoreDTO.MaizeBrandStoreId);
               if (result != null)
               {
                   result.Action = maizeBrandStoreDTO.Action;
                   result.Quantity = maizeBrandStoreDTO.Quantity;

                   result.StartQuantity = maizeBrandStoreDTO.StartQuantity;
                   result.Balance = maizeBrandStoreDTO.Balance;
                   result.StoreId = maizeBrandStoreDTO.StoreId;
                   result.BatchId = maizeBrandStoreDTO.BatchId;
                   result.BranchId = maizeBrandStoreDTO.BranchId;
                   
                   result.TimeStamp = DateTime.Now;
                   result.Deleted = maizeBrandStoreDTO.Deleted;
                   result.DeletedBy = maizeBrandStoreDTO.DeletedBy;
                   result.DeletedOn = maizeBrandStoreDTO.DeletedOn;

                   this.UnitOfWork.Get<MaizeBrandStore>().Update(result);
                   this.UnitOfWork.SaveChanges();
               }
               return maizeBrandStoreDTO.MaizeBrandStoreId;
           }
       }

       public void MarkAsDeleted(long cashId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
              //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }


       public void UpdateMaizeBrandBatchBalance(long batchId, double quantity, string userId)
       {
           using (var dbContext = new MbaleEntities())
           {
               dbContext.UpdateMaizeBrandBatchQuantity(batchId, quantity, userId);
           }
       }


    }
}
