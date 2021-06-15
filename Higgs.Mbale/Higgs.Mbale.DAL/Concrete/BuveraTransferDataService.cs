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
 public   class BuveraTransferDataService : DataServiceBase, IBuveraTransferDataService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(BuveraTransferDataService));

        public BuveraTransferDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        public IEnumerable<BuveraTransfer> GetAllBuveraTransfers()
        {
            return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                .Where(e => e.Deleted == false);
        }

        public IEnumerable<BuveraTransfer> GetAllBuveraTransfersForAParticularStore(long storeId)
        {
            return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId || e.ToReceiverStoreId == storeId).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        
        public BuveraTransfer GetBuveraTransfer(long buveraTransferId)
        {
            return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.BuveraTransferId == buveraTransferId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new BuveraTransfer or updates an already existing BuveraTransfer.
        /// </summary>
        /// <param name="BuveraTransferDTO">BuveraTransfer to be saved or updated.</param>
        /// <param name="userId">userId of the BuveraTransfer creating or updating</param>
        /// <returns>BuveraTransferId</returns>
        public long SaveBuveraTransfer(BuveraTransferDTO buveraTransferDTO, string userId)
        {
            long buveraTransferId = 0;

            if (buveraTransferDTO.BuveraTransferId == 0)
            {
                var buveraTransfer = new BuveraTransfer()
                {
                   
                    StoreId = buveraTransferDTO.StoreId,
                    FromSupplierStoreId = buveraTransferDTO.FromSupplierStoreId,
                    ToReceiverStoreId = buveraTransferDTO.ToReceiverStoreId,
                    BranchId = buveraTransferDTO.BranchId,                   
                    TotalQuantity = buveraTransferDTO.TotalQuantity,   
                    Accept = buveraTransferDTO.Accept,
                    Reject = buveraTransferDTO.Reject,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                    ProductId = buveraTransferDTO.ProductId,
                };

                this.UnitOfWork.Get<BuveraTransfer>().AddNew(buveraTransfer);
                this.UnitOfWork.SaveChanges();
                buveraTransferId = buveraTransfer.BuveraTransferId;
                

                
                return buveraTransferId;
            }

            else
            {
                var result = this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                    .FirstOrDefault(e => e.BuveraTransferId == buveraTransferDTO.BuveraTransferId);
                if (result != null)
                {
                    result.BuveraTransferId = buveraTransferDTO.BuveraTransferId;
                    
                    result.FromSupplierStoreId = buveraTransferDTO.FromSupplierStoreId;
                    result.ToReceiverStoreId = buveraTransferDTO.ToReceiverStoreId;
                    result.TotalQuantity = buveraTransferDTO.TotalQuantity;
                    result.ProductId = buveraTransferDTO.ProductId;
                    result.BranchId = buveraTransferDTO.BranchId;
                    result.StoreId = buveraTransferDTO.StoreId;
                    result.Accept = buveraTransferDTO.Accept;
                    result.Reject = buveraTransferDTO.Reject;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<BuveraTransfer>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return buveraTransferDTO.BuveraTransferId;
            }
        }

        public void MarkAsDeleted(long buveraTransferId, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }
        
        public IEnumerable<StoreBuveraTransferGradeSize> GetStoreBuveraTransferStock(long storeId)
        {
            return this.UnitOfWork.Get<StoreBuveraTransferGradeSize>().AsQueryable().Where(e => e.StoreId == storeId);

        }
        private void SaveStoreBuveraTransferGradeSizes(StoreBuveraTransferGradeSizeDTO storeBuveraTransferGradeSizeDTO)
        {

            var result = this.UnitOfWork.Get<StoreBuveraTransferGradeSize>().AsQueryable()
           .FirstOrDefault(e => e.StoreId == storeBuveraTransferGradeSizeDTO.StoreId && e.GradeId == storeBuveraTransferGradeSizeDTO.GradeId && e.SizeId == storeBuveraTransferGradeSizeDTO.SizeId);


            if (result == null)
            {
                var storeBuveraTransferGradeSize = new StoreBuveraTransferGradeSize()
                {

                    GradeId = storeBuveraTransferGradeSizeDTO.GradeId,
                    SizeId = storeBuveraTransferGradeSizeDTO.SizeId,
                    StoreId = storeBuveraTransferGradeSizeDTO.StoreId,
                    Quantity = storeBuveraTransferGradeSizeDTO.Quantity,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<StoreBuveraTransferGradeSize>().AddNew(storeBuveraTransferGradeSize);
                this.UnitOfWork.SaveChanges();


            }
        }

        public void SaveStoreBuveraTransferGradeSize(StoreBuveraTransferGradeSizeDTO storeBuveraTransferGradeSizeDTO, bool inOrOut)
        {
            double sizeQuantity = 0;
            var result = this.UnitOfWork.Get<StoreBuveraGradeSize>().AsQueryable()
           .FirstOrDefault(e => e.StoreId == storeBuveraTransferGradeSizeDTO.StoreId && e.GradeId == storeBuveraTransferGradeSizeDTO.GradeId && e.SizeId == storeBuveraTransferGradeSizeDTO.SizeId);


            if (result == null)
            {
                var storeBuveraTransferGradeSize = new StoreBuveraGradeSize()
                {

                    GradeId = storeBuveraTransferGradeSizeDTO.GradeId,
                    SizeId = storeBuveraTransferGradeSizeDTO.SizeId,
                    StoreId = storeBuveraTransferGradeSizeDTO.StoreId,
                    Quantity = storeBuveraTransferGradeSizeDTO.Quantity,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<StoreBuveraGradeSize>().AddNew(storeBuveraTransferGradeSize);
                this.UnitOfWork.SaveChanges();
               
                SaveStoreBuveraTransferGradeSizes(storeBuveraTransferGradeSizeDTO);
               


            }

            else
            {
                if (inOrOut)
                {
                    sizeQuantity = result.Quantity + storeBuveraTransferGradeSizeDTO.Quantity;

                    result.StoreId = storeBuveraTransferGradeSizeDTO.StoreId;
                    result.SizeId = storeBuveraTransferGradeSizeDTO.SizeId;
                    result.GradeId = storeBuveraTransferGradeSizeDTO.GradeId;
                    result.Quantity = sizeQuantity;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<StoreBuveraGradeSize>().Update(result);
                    this.UnitOfWork.SaveChanges();
                    

                }
                else
                {
                    
                    sizeQuantity = result.Quantity - storeBuveraTransferGradeSizeDTO.Quantity;

                    result.StoreId = storeBuveraTransferGradeSizeDTO.StoreId;
                    result.SizeId = storeBuveraTransferGradeSizeDTO.SizeId;
                    result.GradeId = storeBuveraTransferGradeSizeDTO.GradeId;
                    result.Quantity = sizeQuantity;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<StoreBuveraGradeSize>().Update(result);
                    this.UnitOfWork.SaveChanges();
                    
                }


            }
        }

     
        public void SaveBuveraTransferGradeSize(BuveraTransferGradeSizeDTO buveraTransferGradeSizeDTO)
        {
            var buveraTransferGradeSize = new BuveraTransferGradeSize()
            {
                BuveraTransferId = buveraTransferGradeSizeDTO.BuveraTransferId,
                GradeId = buveraTransferGradeSizeDTO.GradeId,
                StoreId = buveraTransferGradeSizeDTO.StoreId,
                SizeId = buveraTransferGradeSizeDTO.SizeId,
                Quantity = buveraTransferGradeSizeDTO.Quantity,
                Rate = buveraTransferGradeSizeDTO.Rate,
                Amount = buveraTransferGradeSizeDTO.Amount,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<BuveraTransferGradeSize>().AddNew(buveraTransferGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeBuveraTransferGradeSize(long buveraTransferId)
        {
            this.UnitOfWork.Get<BuveraTransferGradeSize>().AsQueryable()
                .Where(m => m.BuveraTransferId == buveraTransferId)
                .Delete();
        }

    
    }
}
