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
    public class FlourTransferDataService : DataServiceBase, IFlourTransferDataService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(FlourTransferDataService));

        public FlourTransferDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        public IEnumerable<FlourTransfer> GetAllFlourTransfers()
        {
            return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                .Where(e => e.Deleted == false);
        }

        public IEnumerable<FlourTransfer> GetAllFlourTransfersForAParticularStore(long storeId)
        {
            return this.UnitOfWork.Get<FlourTransfer>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId || e.ToReceiverStoreId == storeId).OrderByDescending(e => e.CreatedOn).Take(20);
        }


        
        public FlourTransfer GetFlourTransfer(long flourTransferId)
        {
            return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.FlourTransferId == flourTransferId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new FlourTransfer or updates an already existing FlourTransfer.
        /// </summary>
        /// <param name="FlourTransferDTO">FlourTransfer to be saved or updated.</param>
        /// <param name="userId">userId of the FlourTransfer creating or updating</param>
        /// <returns>FlourTransferId</returns>
        public long SaveFlourTransfer(FlourTransferDTO flourTransferDTO, string userId)
        {
            long flourTransferId = 0;

            if (flourTransferDTO.FlourTransferId == 0)
            {
                var flourTransfer = new FlourTransfer()
                {
                   
                    StoreId = flourTransferDTO.StoreId,
                    FromSupplierStoreId = flourTransferDTO.FromSupplierStoreId,
                    ToReceiverStoreId = flourTransferDTO.ToReceiverStoreId,
                    BranchId = flourTransferDTO.BranchId,                   
                    TotalQuantity = flourTransferDTO.TotalQuantity,   
                    Accept = flourTransferDTO.Accept,
                    Reject = flourTransferDTO.Reject,
                   
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                };

                this.UnitOfWork.Get<FlourTransfer>().AddNew(flourTransfer);
                this.UnitOfWork.SaveChanges();
                flourTransferId = flourTransfer.FlourTransferId;
                

                
                return flourTransferId;
            }

            else
            {
                var result = this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                    .FirstOrDefault(e => e.FlourTransferId == flourTransferDTO.FlourTransferId);
                if (result != null)
                {
                    result.FlourTransferId = flourTransferDTO.FlourTransferId;
                    
                    result.FromSupplierStoreId = flourTransferDTO.FromSupplierStoreId;
                    result.ToReceiverStoreId = flourTransferDTO.ToReceiverStoreId;
                    result.TotalQuantity = flourTransferDTO.TotalQuantity;               
                    result.BranchId = flourTransferDTO.BranchId;
                    result.StoreId = flourTransferDTO.StoreId;
                   
                    result.Accept = flourTransferDTO.Accept;
                    result.Reject = flourTransferDTO.Reject;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<FlourTransfer>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return flourTransferDTO.FlourTransferId;
            }
        }


        //public void SaveFlourTransferBatch(FlourTransferBatchDTO flourTransferBatchDTO)
        //{
           
           
        //        var flourTransferBatch = new FlourTransferBatch()
        //        {

        //           FlourTransferId = flourTransferBatchDTO.FlourTransferId,
        //           BatchId = flourTransferBatchDTO.BatchId,
        //            CreatedOn = DateTime.Now,
        //            TimeStamp = DateTime.Now,
                   
        //        };

        //        this.UnitOfWork.Get<FlourTransferBatch>().AddNew(flourTransferBatch);
        //        this.UnitOfWork.SaveChanges();
       
        //}

        public void MarkAsDeleted(long FlourTransferId, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }


        public IEnumerable<StoreFlourTransferGradeSize> GetStoreFlourTransferStock(long storeId)
        {
            return this.UnitOfWork.Get<StoreFlourTransferGradeSize>().AsQueryable().Where(e => e.StoreId == storeId);

        }

        public IEnumerable<FlourTransferBatch> GetAllBatchesForAFlourTransfer(long flourTransferId)
        {
            return this.UnitOfWork.Get<FlourTransferBatch>().AsQueryable().Where(e => e.FlourTransferId == flourTransferId);
        }


        private void SaveStoreFlourTransferGradeSizes(StoreFlourTransferGradeSizeDTO storeFlourTransferGradeSizeDTO, bool inOrOut)
        {
            
            var result = this.UnitOfWork.Get<StoreFlourTransferGradeSize>().AsQueryable()
           .FirstOrDefault(e => e.StoreId == storeFlourTransferGradeSizeDTO.StoreId && e.GradeId == storeFlourTransferGradeSizeDTO.GradeId && e.SizeId == storeFlourTransferGradeSizeDTO.SizeId);


            if (result == null)
            {
                var storeFlourTransferGradeSize = new StoreFlourTransferGradeSize()
                {

                    GradeId = storeFlourTransferGradeSizeDTO.GradeId,
                    SizeId = storeFlourTransferGradeSizeDTO.SizeId,
                    StoreId = storeFlourTransferGradeSizeDTO.StoreId,
                    Quantity = storeFlourTransferGradeSizeDTO.Quantity,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<StoreFlourTransferGradeSize>().AddNew(storeFlourTransferGradeSize);
                this.UnitOfWork.SaveChanges();
            

            }
        }
        public void SaveStoreFlourTransferGradeSize(StoreFlourTransferGradeSizeDTO storeFlourTransferGradeSizeDTO, bool inOrOut)
        {
            double sizeQuantity = 0;
            var result = this.UnitOfWork.Get<StoreGradeSize>().AsQueryable()
           .FirstOrDefault(e => e.StoreId == storeFlourTransferGradeSizeDTO.StoreId && e.GradeId == storeFlourTransferGradeSizeDTO.GradeId && e.SizeId == storeFlourTransferGradeSizeDTO.SizeId);


            if (result == null)
            {
                var storeFlourTransferGradeSize = new StoreGradeSize()
                {

                    GradeId = storeFlourTransferGradeSizeDTO.GradeId,
                    SizeId = storeFlourTransferGradeSizeDTO.SizeId,
                    StoreId = storeFlourTransferGradeSizeDTO.StoreId,
                    Quantity = storeFlourTransferGradeSizeDTO.Quantity,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<StoreGradeSize>().AddNew(storeFlourTransferGradeSize);
                this.UnitOfWork.SaveChanges();

                SaveStoreFlourTransferGradeSizes(storeFlourTransferGradeSizeDTO, inOrOut);
               

            }

            else
            {
                if (inOrOut)
                {
                    sizeQuantity = result.Quantity + storeFlourTransferGradeSizeDTO.Quantity;

                    result.StoreId = storeFlourTransferGradeSizeDTO.StoreId;
                    result.SizeId = storeFlourTransferGradeSizeDTO.SizeId;
                    result.GradeId = storeFlourTransferGradeSizeDTO.GradeId;
                    result.Quantity = sizeQuantity;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<StoreGradeSize>().Update(result);
                    this.UnitOfWork.SaveChanges();
                   

                }
                else
                {

                    sizeQuantity = result.Quantity - storeFlourTransferGradeSizeDTO.Quantity;

                    result.StoreId = storeFlourTransferGradeSizeDTO.StoreId;
                    result.SizeId = storeFlourTransferGradeSizeDTO.SizeId;
                    result.GradeId = storeFlourTransferGradeSizeDTO.GradeId;
                    result.Quantity = sizeQuantity;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<StoreGradeSize>().Update(result);
                    this.UnitOfWork.SaveChanges();
                   
                }


            }
        }

     
        public void SaveFlourTransferGradeSize(FlourTransferGradeSizeDTO FlourTransferGradeSizeDTO)
        {
            var FlourTransferGradeSize = new FlourTransferGradeSize()
            {
                FlourTransferId = FlourTransferGradeSizeDTO.FlourTransferId,
                GradeId = FlourTransferGradeSizeDTO.GradeId,
                StoreId = FlourTransferGradeSizeDTO.StoreId,
                SizeId = FlourTransferGradeSizeDTO.SizeId,
                Quantity = FlourTransferGradeSizeDTO.Quantity,
                Rate = FlourTransferGradeSizeDTO.Rate,
                Amount = FlourTransferGradeSizeDTO.Amount,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<FlourTransferGradeSize>().AddNew(FlourTransferGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeFlourTransferGradeSize(long FlourTransferId)
        {
            this.UnitOfWork.Get<FlourTransferGradeSize>().AsQueryable()
                .Where(m => m.FlourTransferId == FlourTransferId)
                .Delete();
        }


        public void SaveBatchFlourTransferGradeSize(List<BatchTransferGradeSizeDTO> batchTransferGradeSizeDTOS)
        {
            foreach (var batchTransferGradeSizeDTO in batchTransferGradeSizeDTOS)
            {
                var batchTransferGradeSize = new BatchTransferGradeSize()
                {
                    FlourTransferId = batchTransferGradeSizeDTO.FlourTransferId,
                    GradeId = batchTransferGradeSizeDTO.GradeId,
                    SizeId = batchTransferGradeSizeDTO.SizeId,
                   
                    Quantity = batchTransferGradeSizeDTO.Quantity,
                    BatchId = batchTransferGradeSizeDTO.BatchId,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<BatchTransferGradeSize>().AddNew(batchTransferGradeSize);
                this.UnitOfWork.SaveChanges();
            }

        }

        public void PurgeBatchTransferGradeSize(long flourTransferId)
        {
            this.UnitOfWork.Get<BatchTransferGradeSize>().AsQueryable()
                .Where(m => m.FlourTransferId == flourTransferId)
                .Delete();
        }

        public void SaveFlourTransferBatch(FlourTransferBatchDTO flourTransferBatchDTO)
        {
            var flourTransferBatch = new FlourTransferBatch()
            {
                BatchId = flourTransferBatchDTO.BatchId,
                FlourTransferId = flourTransferBatchDTO.FlourTransferId,
                BatchQuantity = flourTransferBatchDTO.BatchQuantity,
               
                CreatedOn = DateTime.Now,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<FlourTransferBatch>().AddNew(flourTransferBatch);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeTransferBatch(long flourTransferId)
        {
            this.UnitOfWork.Get<FlourTransferBatch>().AsQueryable()
                .Where(m => m.FlourTransferId == flourTransferId).Delete();
        }

    }
}
