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
using System.Configuration;

namespace Higgs.Mbale.DAL.Concrete
{
  public  class SupplyDataService : DataServiceBase,ISupplyDataService
    {
      private string supplyStatusId = ConfigurationManager.AppSettings["SupplyStatusId"];
        ILog logger = log4net.LogManager.GetLogger(typeof(SupplyDataService));

       public SupplyDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }
              
        public IEnumerable<Supply> GetAllSupplies()
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false); 
        }

        public IEnumerable<Supply> GetAllApprovedSupplies()
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.Approved == true);
        }
        public IEnumerable<Supply> GetAllUnApprovedSupplies()
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.Approved == null);
        }

        public IEnumerable<Supply> GetAllUnApprovedSuppliesForABranch(long branchId)
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.Approved == null && e.BranchId == branchId);
        }
        public IEnumerable<Supply> GetAllSuppliesToBeUsed()
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.Used == false && e.Approved == true);
        }
        public   IEnumerable<Supply> GetAllSuppliesForAParticularSupplier(string supplierId)
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.SupplierId == supplierId).OrderByDescending(e => e.CreatedOn).Take(20);
         }

        public IEnumerable<Supply> GetAllUnPaidSuppliesForAParticularSupplier(string supplierId)
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.SupplierId == supplierId && e.IsPaid == false && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Supply> GetAllUnPaidSupplies()
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => (e.Deleted == false || e.Deleted == null) && e.IsPaid == false && e.Approved == true).OrderByDescending(e => e.CreatedOn);
        }
        public IEnumerable<Supply> GetAllPaidSuppliesForAParticularSupplier(string supplierId)
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.SupplierId == supplierId && e.IsPaid == true).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public IEnumerable<Supply> GetAllSuppliesForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
        }
        public IEnumerable<Supply> GetAllSuppliesToBeUsedForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<Supply>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Used == false && e.Approved == true);
        }

        public Supply GetSupply(long supplyId)
        {
           var supply = this.UnitOfWork.Get<Supply>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.SupplyId == supplyId &&
                    (c.Deleted == false || c.Deleted == null)
                );
           
            return supply;
        }

        /// <summary>
        /// Saves a new Supply or updates an already existing Supply.
        /// </summary>
        /// <param name="Supply">Supply to be saved or updated.</param>
        /// <param name="SupplyId">SupplyId of the Supply creating or updating</param>
        /// <returns>SupplyId</returns>
        public long SaveSupply(SupplyDTO supplyDTO, string userId)
        {
            long supplyId = 0;
            
            if (supplyDTO.SupplyId == 0)
            {

                var supply = new Supply()
                {
                     
                    Quantity = supplyDTO.Quantity,
                    SupplyDate =  supplyDTO.SupplyDate,
                    //SupplyNumber = supplyDTO.SupplyNumber,
                    BranchId = supplyDTO.BranchId,
                    SupplierId = supplyDTO.SupplierId,
                    Amount = supplyDTO.Amount,
                    TruckNumber = supplyDTO.TruckNumber,
                    Used = supplyDTO.Used,
                    MoistureContent = supplyDTO.MoistureContent,
                    WeightNoteNumber = supplyDTO.WeightNoteNumber,
                    NormalBags = supplyDTO.NormalBags,
                    BagsOfStones = supplyDTO.BagsOfStones,
                    Price = supplyDTO.Price,
                    IsPaid = supplyDTO.IsPaid,
                    StatusId = Convert.ToInt64(supplyStatusId),
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                    AmountToPay = supplyDTO.AmountToPay,
                    StoreId = supplyDTO.StoreId,
                    Offloading = supplyDTO.Offloading,
                    PartialAmount = supplyDTO.PartialAmount,
                    PartiallyPaid = supplyDTO.PartiallyPaid,
                    YellowBags = supplyDTO.YellowBags,
                    Approved = supplyDTO.Approved,
                };

                this.UnitOfWork.Get<Supply>().AddNew(supply);
                this.UnitOfWork.SaveChanges();
                supplyId = supply.SupplyId;
                return supplyId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Supply>().AsQueryable()
                    .SingleOrDefault(e => e.SupplyId == supplyDTO.SupplyId);
                            
                if (result != null)
                {
                    result.Quantity = supplyDTO.Quantity;
                    result.SupplyDate = Convert.ToDateTime(supplyDTO.SupplyDate);
                   
                    result.BranchId = supplyDTO.BranchId;
                    result.SupplierId = supplyDTO.SupplierId;
                    result.Amount = supplyDTO.Amount;
                    result.IsPaid = supplyDTO.IsPaid;
                    result.TruckNumber = supplyDTO.TruckNumber;
                    result.Price = supplyDTO.Price;
                    result.AmountToPay = supplyDTO.AmountToPay;
                    result.Used = supplyDTO.Used;
                    result.WeightNoteNumber = supplyDTO.WeightNoteNumber;
                    result.BagsOfStones = supplyDTO.BagsOfStones;
                    result.NormalBags = supplyDTO.NormalBags;
                    result.StatusId = supplyDTO.StatusId;
                    result.MoistureContent = supplyDTO.MoistureContent;
                   
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = supplyDTO.Deleted;
                    result.DeletedBy = supplyDTO.DeletedBy;
                    result.DeletedOn = supplyDTO.DeletedOn;
                    result.StoreId = supplyDTO.StoreId;
                    result.Offloading = supplyDTO.Offloading;
                    result.YellowBags = supplyDTO.YellowBags;
                    result.PartiallyPaid = supplyDTO.PartiallyPaid;
                    result.PartialAmount = supplyDTO.PartialAmount;
                    result.Approved = supplyDTO.Approved;
                   

                    this.UnitOfWork.Get<Supply>().Update(result);
                    
                    this.UnitOfWork.SaveChanges();
                    
                }
                return supplyDTO.SupplyId;
            }            
        }

        public long UpdateSupplyOnRequistionAprroval(SupplyDTO supplyDTO, string userId)
        {
            var supplyId = 0;
            if(supplyDTO != null)
            {
               
                using (var dbContext = new MbaleEntities())
                {
                    supplyId = dbContext.UpdateSupplyOnRequistionApproval
                          (
                              supplyDTO.SupplyId,
                              supplyDTO.IsPaid,   
                              supplyDTO.AmountToPay,
                              supplyDTO.PartialAmount,
                              supplyDTO.PartiallyPaid,
                             
                              userId
                              );
                }
            }
            return supplyId;
            
        }

        public void MarkAsDeleted(long supplyId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
              //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }
        public  long UpdateBatchSupplyWithCompletedStatus(long supplyId, long statusId, string userId)
        {
            var resultId = 0;
            using (var dbContext = new MbaleEntities())
            {
            resultId =  dbContext.UpdateSupplyWithCompletedStatus(supplyId, statusId, userId);
            }
            return resultId;
        }

        public long UpdateSupplyWithInProgressStatus(long supplyId, long statusId, string userId)
        {
            var resultId = 0;
            using (var dbContext = new MbaleEntities())
            {
                resultId = dbContext.UpdateSupplyWithInProgressStatus(supplyId, statusId, userId);
            }
            return resultId;
        }
        public void SaveStoreMaizeStock(StoreMaizeStockDTO storeMaizeStockDTO)
    {

        var storeMaizeStock = new StoreMaizeStock()
        {
            StoreMaizeStockId = storeMaizeStockDTO.StoreMaizeStockId,
            StockBalance = storeMaizeStockDTO.StockBalance,
            SupplyId = storeMaizeStockDTO.SupplyId,
            StoreId = storeMaizeStockDTO.StoreId,
            BranchId = storeMaizeStockDTO.BranchId,
            StartStock = storeMaizeStockDTO.StartStock,
            SectorId = storeMaizeStockDTO.SectorId,
            Quantity = storeMaizeStockDTO.Quantity,
            InOrOut = storeMaizeStockDTO.InOrOut,
            TimeStamp = DateTime.Now
        };
        this.UnitOfWork.Get<StoreMaizeStock>().AddNew(storeMaizeStock);
        this.UnitOfWork.SaveChanges();
    }

    public StoreMaizeStock GetLatestMaizeStockForAParticularStore(long storeId)
    {
        StoreMaizeStock storeMaizeStock = new StoreMaizeStock();

        var storeMaizeStocks = this.UnitOfWork.Get<StoreMaizeStock>().AsQueryable().Where(e => e.StoreId == storeId);
        if (storeMaizeStocks.Any())
        {
            storeMaizeStock = storeMaizeStocks.AsQueryable().OrderByDescending(e => e.TimeStamp).First();
            return storeMaizeStock;
        }
        else
        {
            return storeMaizeStock;
        }
            
   }

    public IEnumerable<StoreMaizeStock> GetMaizeStocksForAParticularStore(long storeId)
    {
        return this.UnitOfWork.Get<StoreMaizeStock>().AsQueryable().Where(e => e.StoreId == storeId);

    }

     public int CheckIfWeightNoteExists(string weightNoteNumber)
        {
            int exists = 0;
            using (var dbContext = new MbaleEntities())
            {
            var    result = dbContext.CheckIfWeightNoteExists(weightNoteNumber);
                
                exists =Convert.ToInt32(result.FirstOrDefault());
            }
        
            return exists;
        }

    }
}
