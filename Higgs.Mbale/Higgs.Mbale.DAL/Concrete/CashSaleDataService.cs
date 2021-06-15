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
using EntityFramework.Extensions;

namespace Higgs.Mbale.DAL.Concrete
{
 public   class CashSaleDataService : DataServiceBase, ICashSaleDataService
    {
    
         ILog logger = log4net.LogManager.GetLogger(typeof(CashSaleDataService));

         public CashSaleDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
         {

         }

         public IEnumerable<CashSale> GetAllCashSales()
         {
             return this.UnitOfWork.Get<CashSale>().AsQueryable()
                 .Where(e => e.Deleted == false);
         }

         public IEnumerable<CashSale> GetAllCashSalesForAParticularStore(long storeId)
         {
             return this.UnitOfWork.Get<CashSale>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId );
         }

         public IEnumerable<CashSale> GetTenLatestCashSalesForAParticularBranch(long branchId,int offSet,int pageSize)
         {
             
             return this.UnitOfWork.Get<CashSale>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId).OrderByDescending(e =>e.CreatedOn).Skip(offSet).Take(pageSize);
         }


         public IEnumerable<CashSale> GetAllCashSalesForAParticularBranch(long branchId)
         {
             return this.UnitOfWork.Get<CashSale>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
         }

         public CashSale GetCashSale(long cashSaleId)
         {
             return this.UnitOfWork.Get<CashSale>().AsQueryable()
                  .FirstOrDefault(c =>
                     c.CashSaleId == cashSaleId &&
                     c.Deleted == false
                 );
         }

        
         public long SaveCashSale(CashSaleDTO cashSaleDTO, string userId)
         {
             long cashSaleId = 0;

             if (cashSaleDTO.CashSaleId == 0)
             {
                 var cashSale = new CashSale()
                 {

                     StoreId = cashSaleDTO.StoreId,
                     Price = cashSaleDTO.Price,
                     Quantity = cashSaleDTO.Quantity,
                     PaymentModeId = cashSaleDTO.PaymentModeId,
                     BranchId = cashSaleDTO.BranchId,
                     Amount = cashSaleDTO.Amount,
                     ProductId = cashSaleDTO.ProductId,
                    TransactionSubTypeId = cashSaleDTO.TransactionSubTypeId,
                    SectorId = cashSaleDTO.SectorId,
                     CreatedOn = DateTime.Now,
                     TimeStamp = DateTime.Now,
                     CreatedBy = userId,
                     Deleted = false,
                     Cancelled = false,
                     ReceiptLimit = cashSaleDTO.ReceiptLimit,
                     //DocumentCategoryId = cashSaleDTO.DocumentCategoryId,
                 };

                 this.UnitOfWork.Get<CashSale>().AddNew(cashSale);
                 this.UnitOfWork.SaveChanges();
                 cashSaleId = cashSale.CashSaleId;



                 return cashSaleId;
             }

             else
             {
                 var result = this.UnitOfWork.Get<CashSale>().AsQueryable()
                     .FirstOrDefault(e => e.CashSaleId == cashSaleDTO.CashSaleId);
                 if (result != null)
                 {
                
                     result.CashSaleId = cashSaleDTO.CashSaleId;
                     result.Quantity = cashSaleDTO.Quantity;
                     result.Price = cashSaleDTO.Price;
                     result.ProductId = cashSaleDTO.ProductId;
                     result.PaymentModeId = cashSaleDTO.PaymentModeId;
                     result.BranchId = cashSaleDTO.BranchId;
                     result.StoreId = cashSaleDTO.StoreId;
                     result.SectorId = cashSaleDTO.SectorId;
                     result.Amount = cashSaleDTO.Amount;
                     result.Cancelled = cashSaleDTO.Cancelled;
                     result.ReceiptLimit = cashSaleDTO.ReceiptLimit;
                     result.TransactionSubTypeId = cashSaleDTO.TransactionSubTypeId;
                     result.UpdatedBy = userId;
                     result.TimeStamp = DateTime.Now;
                     this.UnitOfWork.Get<CashSale>().Update(result);
                     this.UnitOfWork.SaveChanges();
                 }
                 return cashSaleDTO.CashSaleId;
             }
         }


         public void SaveCashSaleBatch(CashSaleBatchDTO cashSaleBatchDTO)
         {


             var cashSaleBatch = new CashSaleBatch()
             {

                 CashSaleId = cashSaleBatchDTO.CashSaleId,
                 BatchId = cashSaleBatchDTO.BatchId,
                 BatchQuantity = cashSaleBatchDTO.BatchQuantity,
                 CreatedOn = DateTime.Now,
                 Price = cashSaleBatchDTO.Price,
                 ProductId = cashSaleBatchDTO.ProductId,
                 Amount = cashSaleBatchDTO.Amount,
                 TimeStamp = DateTime.Now,

             };

             this.UnitOfWork.Get<CashSaleBatch>().AddNew(cashSaleBatch);
             this.UnitOfWork.SaveChanges();

         }

         public void MarkAsDeleted(long CashSaleId, string userId)
         {
             using (var dbContext = new MbaleEntities())
             {
                 //TODO: THROW NOT IMPLEMENTED EXCEPTION
             }

         }


         public long Cancelled(CashSaleDTO cashSale, string userId)
         {
             var result = this.UnitOfWork.Get<CashSale>().AsQueryable()
                     .FirstOrDefault(e => e.CashSaleId == cashSale.CashSaleId);
             if (result != null)
             {

                 result.CashSaleId = cashSale.CashSaleId;
                 result.Quantity = cashSale.Quantity;
                 result.Price = cashSale.Price;
                 result.ProductId = cashSale.ProductId;
                 result.PaymentModeId = cashSale.PaymentModeId;
                 result.BranchId = cashSale.BranchId;
                 result.StoreId = cashSale.StoreId;
                 result.SectorId = cashSale.SectorId;
                 result.Amount = cashSale.Amount;
                 result.Cancelled = true;
                 result.ReceiptLimit = cashSale.ReceiptLimit;
                 result.TransactionSubTypeId = cashSale.TransactionSubTypeId;
                 result.UpdatedBy = userId;
                 result.Deleted = true;
                 result.DeletedBy = userId;
                 result.DeletedOn = DateTime.Now;
                 result.TimeStamp = DateTime.Now;
                 this.UnitOfWork.Get<CashSale>().Update(result);
                 this.UnitOfWork.SaveChanges();
             }
            // var cashSaleId = cashSale.CashSaleId;
             PurgeBatchCashSaleGradeSize(cashSale.CashSaleId);
             PurgeCashSaleBatch(cashSale.CashSaleId);
             return cashSale.CashSaleId;
         }
         public IEnumerable<CashSaleBatch> GetAllBatchesForACashSale(long cashSaleId)
         {
             return this.UnitOfWork.Get<CashSaleBatch>().AsQueryable().Where(e => e.CashSaleId == cashSaleId);
         }

         public void SaveCashSaleGradeSize(CashSaleGradeSizeDTO cashSaleGradeSizeDTO)
         {
             var cashSaleGradeSize = new CashSaleGradeSize()
             {
                 CashSaleId = cashSaleGradeSizeDTO.CashSaleId,
                 GradeId = cashSaleGradeSizeDTO.GradeId,
                 SizeId = cashSaleGradeSizeDTO.SizeId,
                 Quantity = cashSaleGradeSizeDTO.Quantity,
                 Price = cashSaleGradeSizeDTO.Price,
                 Amount = cashSaleGradeSizeDTO.Amount,
                 TimeStamp = DateTime.Now
             };
             this.UnitOfWork.Get<CashSaleGradeSize>().AddNew(cashSaleGradeSize);
             this.UnitOfWork.SaveChanges();
         }

         public void PurgeCashSaleGradeSize(long cashSaleId)
         {
             this.UnitOfWork.Get<CashSaleGradeSize>().AsQueryable()
                 .Where(m => m.CashSaleId == cashSaleId)
                 .Delete();
         }
         public void PurgeCashSaleBatch(long cashSaleId)
         {
             this.UnitOfWork.Get<CashSaleBatch>().AsQueryable()
                 .Where(m => m.CashSaleId == cashSaleId)
                 .Delete();
         }


        public void SaveBatchCashSaleGradeSize(List<CashSaleBatchGradeSizeDTO> batchCashSaleGradeSizeDTOS)
        {
            foreach (var batchCashSaleGradeSizeDTO in batchCashSaleGradeSizeDTOS)
            {
                var batchCashSaleGradeSize = new CashSaleBatchGradeSize()
                {
                    CashSaleId = batchCashSaleGradeSizeDTO.CashSaleId,
                    GradeId = batchCashSaleGradeSizeDTO.GradeId,
                    SizeId = batchCashSaleGradeSizeDTO.SizeId,
                    Price = batchCashSaleGradeSizeDTO.Price,
                    Amount = batchCashSaleGradeSizeDTO.Amount,
                    Quantity = batchCashSaleGradeSizeDTO.Quantity,
                    BatchId = batchCashSaleGradeSizeDTO.BatchId,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<CashSaleBatchGradeSize>().AddNew(batchCashSaleGradeSize);
                this.UnitOfWork.SaveChanges();
            }

        }

        public void PurgeBatchCashSaleGradeSize(long cashSaleId)
        {
            this.UnitOfWork.Get<CashSaleBatchGradeSize>().AsQueryable()
                .Where(m => m.CashSaleId == cashSaleId)
                .Delete();
        }


    }
}

