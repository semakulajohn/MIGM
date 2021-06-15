using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;
using EntityFramework.Extensions;


namespace Higgs.Mbale.DAL.Concrete
{
    public class DeliveryDataService: DataServiceBase,IDeliveryDataService
    {
       ILog logger = log4net.LogManager.GetLogger(typeof(DeliveryDataService));

       public DeliveryDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }
              
        public IEnumerable<Delivery> GetAllDeliveries()
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false); 
        }

        public IEnumerable<Delivery> GetAllUnApprovedDeliveries()
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.Approved == null).OrderByDescending(e => e.CreatedOn).Take(20); ;
        }

        public IEnumerable<Delivery> GetAllApprovedDeliveries()
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20); ;
        }

        public IEnumerable<Delivery> GetAllRejectedDeliveries()
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.Approved == false).OrderByDescending(e => e.CreatedOn).Take(20); ;
        }
        public Delivery GetDelivery(long deliveryId)
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.DeliveryId == deliveryId &&
                    c.Deleted == false
                );
        }
        public IEnumerable<Delivery> GetAllDeliveriesForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
        }

        public IEnumerable<Delivery> GetAllBranchUnApprovedDeliveries(long branchId)
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Approved == null).OrderByDescending(e => e.CreatedOn).Take(20); ;
        }

        public IEnumerable<Delivery> GetAllBranchApprovedDeliveries(long branchId)
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20); 
        }

        public IEnumerable<Delivery> GetAllBranchRejectedDeliveries(long branchId)
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Approved == false).OrderByDescending(e => e.CreatedOn).Take(20); 
        }

        public IEnumerable<Delivery> GetAllDeliveriesForAParticularOrder(long orderId)
        {
            return this.UnitOfWork.Get<Delivery>().AsQueryable().Where(e => e.Deleted == false && e.OrderId == orderId && (e.Approved == true || e.Approved == null));
        }
        /// <summary>
        /// Saves a new Delivery or updates an already existing Delivery.
        /// </summary>
        /// <param name="Delivery">Delivery to be saved or updated.</param>
        /// <param name="DeliveryId">DeliveryId of the Delivery creating or updating</param>
        /// <returns>DeliveryId</returns>
        public long SaveDelivery(DeliveryDTO deliveryDTO, string userId)
        {
            long deliveryId = 0;
            
            if (deliveryDTO.DeliveryId == 0)
            {

                var delivery = new Delivery()
                {
                    CustomerId = deliveryDTO.CustomerId,
                    StoreId = deliveryDTO.StoreId,
                    DeliveryCost = deliveryDTO.DeliveryCost,
                    OrderId = deliveryDTO.OrderId,
                    DeliveryDate = deliveryDTO.DeliveryDate,
                    Amount = deliveryDTO.Amount,
                    PaymentModeId = deliveryDTO.PaymentModeId,
                    Price = deliveryDTO.Price,
                    Quantity = deliveryDTO.Quantity,
                    ProductId = deliveryDTO.ProductId,
                    VehicleNumber = deliveryDTO.VehicleNumber,
                    BranchId = deliveryDTO.BranchId,
                    Location = deliveryDTO.Location,
                    SectorId = deliveryDTO.SectorId,
                    MediaId = deliveryDTO.MediaId,
                    DriverNIN = deliveryDTO.DriverNIN,
                    DriverName = deliveryDTO.DriverName,
                    TransactionSubTypeId = deliveryDTO.TransactionSubTypeId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                    Approved = deliveryDTO.Approved,
                };

                this.UnitOfWork.Get<Delivery>().AddNew(delivery);
                this.UnitOfWork.SaveChanges();
                deliveryId = delivery.DeliveryId;
                return deliveryId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Delivery>().AsQueryable()
                    .FirstOrDefault(e => e.DeliveryId == deliveryDTO.DeliveryId);
                if (result != null)
                {
                    result.DeliveryCost = deliveryDTO.DeliveryCost;
                    result.CustomerId = deliveryDTO.CustomerId;
                    result.StoreId = deliveryDTO.StoreId;
                    result.OrderId =  deliveryDTO.OrderId;
                    result.ProductId = deliveryDTO.ProductId;
                    result.DeliveryDate = deliveryDTO.DeliveryDate;
                    result.PaymentModeId = deliveryDTO.PaymentModeId;
                    result.Price = deliveryDTO.Price;
                    result.Amount = deliveryDTO.Amount;
                    result.Quantity = deliveryDTO.Quantity;
                    result.TransactionSubTypeId = deliveryDTO.TransactionSubTypeId;
                    result.VehicleNumber = deliveryDTO.VehicleNumber;
                    result.BranchId = deliveryDTO.BranchId;
                    result.Location = deliveryDTO.Location;
                    result.SectorId = deliveryDTO.SectorId;
                    result.MediaId = deliveryDTO.MediaId;
                    result.DriverName = deliveryDTO.DriverName;
                    result.DriverNIN = deliveryDTO.DriverNIN;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = deliveryDTO.Deleted;
                    result.DeletedBy = deliveryDTO.DeletedBy;
                    result.DeletedOn = deliveryDTO.DeletedOn;
                    result.Approved = deliveryDTO.Approved;

                    this.UnitOfWork.Get<Delivery>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return deliveryDTO.DeliveryId;
            }            
        }

        public void MarkAsDeleted(long deliveryId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
                dbContext.Mark_Delivery_AsDeleted(deliveryId, userId);
              //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }

        public long UpdateDeliveryOnApprovalOrRejection(long deliveryId, bool approved, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
                dbContext.Update_Delivery_WithApprovedOrRejected(deliveryId,approved, userId);
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }
            return deliveryId;

        }
        public void SaveDeliveryGradeSize(DeliveryGradeSizeDTO deliveryGradeSizeDTO)
        {
            var deliveryGradeSize = new DeliveryGradeSize()
            {
                DeliveryId = deliveryGradeSizeDTO.DeliveryId,
                GradeId = deliveryGradeSizeDTO.GradeId,
                SizeId = deliveryGradeSizeDTO.SizeId,
                Amount = deliveryGradeSizeDTO.Amount,
                Price = deliveryGradeSizeDTO.Price,
                Quantity = deliveryGradeSizeDTO.Quantity,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<DeliveryGradeSize>().AddNew(deliveryGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeDeliveryGradeSize(long deliveryId)
        {
            this.UnitOfWork.Get<DeliveryGradeSize>().AsQueryable()
                .Where(m => m.DeliveryId == deliveryId)
                .Delete();
        }

        public void SaveBatchDeliveryGradeSize(List<BatchDeliveryGradeSizeDTO> batchDeliveryGradeSizeDTOS)
        {
            foreach (var batchDeliveryGradeSizeDTO in batchDeliveryGradeSizeDTOS)
            {
                var batchDeliveryGradeSize = new BatchDeliveryGradeSize()
                {
                    DeliveryId = batchDeliveryGradeSizeDTO.DeliveryId,
                    GradeId = batchDeliveryGradeSizeDTO.GradeId,
                    SizeId = batchDeliveryGradeSizeDTO.SizeId,
                    Amount = batchDeliveryGradeSizeDTO.Amount,
                    Price = batchDeliveryGradeSizeDTO.Price,
                    Quantity = batchDeliveryGradeSizeDTO.Quantity,
                    BatchId = batchDeliveryGradeSizeDTO.BatchId,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<BatchDeliveryGradeSize>().AddNew(batchDeliveryGradeSize);
                this.UnitOfWork.SaveChanges();
            }
           
        }

        public void PurgeBatchDeliveryGradeSize(long deliveryId)
        {
            this.UnitOfWork.Get<BatchDeliveryGradeSize>().AsQueryable()
                .Where(m => m.DeliveryId == deliveryId)
                .Delete();
        }

        public void SaveDeliveryStock(DeliveryStockDTO deliveryStockDTO)
        {
            var deliveryStock = new DeliveryStock()
            {
                StockId = deliveryStockDTO.StockId,
                DeliveryId = deliveryStockDTO.DeliveryId,
                CreatedOn = DateTime.Now,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<DeliveryStock>().AddNew(deliveryStock);
            this.UnitOfWork.SaveChanges();
        }

        public void SaveDeliveryBatch(DeliveryBatchDTO deliveryBatchDTO)
        {
            var deliveryBatch= new DeliveryBatch()
            {
                BatchId = deliveryBatchDTO.BatchId,
                DeliveryId = deliveryBatchDTO.DeliveryId,
                BatchQuantity = deliveryBatchDTO.BatchQuantity,
                Amount = deliveryBatchDTO.Amount,
                Price = deliveryBatchDTO.Price,
                ProductId = deliveryBatchDTO.ProductId,
                CreatedOn = DateTime.Now,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<DeliveryBatch>().AddNew(deliveryBatch);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeDeliveryBatch(long deliveryId)
        {
            this.UnitOfWork.Get<DeliveryBatch>().AsQueryable()
                .Where(m =>m.DeliveryId == deliveryId).Delete();
        }

        public IEnumerable<DeliveryStock> GetDeliveryStocksForDelivery(long deliveryId){
          return  this.UnitOfWork.Get<DeliveryStock>().AsQueryable().
               Where(b => b.DeliveryId == deliveryId);
        }

        public void PurgeDeliveryStock(long stockId, long deliveryId)
        {
            this.UnitOfWork.Get<DeliveryStock>().AsQueryable()
                .Where(m => m.StockId == stockId && m.DeliveryId == deliveryId).Delete();
        }

       
    }
}
