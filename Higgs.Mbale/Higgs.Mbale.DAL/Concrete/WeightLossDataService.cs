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
 public   class WeightLossDataService : DataServiceBase, IWeightLossDataService
    {
        
            ILog logger = log4net.LogManager.GetLogger(typeof(WeightLossDataService));

            public WeightLossDataService(IUnitOfWork<MbaleEntities> unitOfWork)
                 : base(unitOfWork)
            {

            }

           
            public WeightLoss GetWeightLoss(long weightLossId)
            {
                return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                     .FirstOrDefault(c =>
                        c.WeightLossId == weightLossId &&
                        c.Deleted == false
                    );
            }

        public WeightLoss GetWeightLossForDelivery(long deliveryId)
        {
            return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.DeliveryId == deliveryId &&
                    c.Deleted == false
                );
        }
        public IEnumerable<WeightLoss> GetAllWeightLossesForAParticularBranch(long branchId)
            {
                return this.UnitOfWork.Get<WeightLoss>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Quantity > 0);
            }

            public IEnumerable<WeightLoss> GetAllWeightLossesForAParticularDelivery(long deliveryId)
            {
                return this.UnitOfWork.Get<WeightLoss>().AsQueryable().Where(e => e.Deleted == false && e.DeliveryId == deliveryId && e.Quantity > 0);
            }

        public IEnumerable<WeightLoss> GetAllWeightLossesForAParticularCustomer(string customerId)
        {
            return this.UnitOfWork.Get<WeightLoss>().AsQueryable().Where(e => e.Deleted == false && e.CustomerId == customerId && e.Quantity > 0);
        }
        /// <summary>
        /// Saves a new WeightLoss or updates an already existing WeightLoss.
        /// </summary>
        /// <param name="WeightLoss">WeightLoss to be saved or updated.</param>
        /// <param name="WeightLossId">WeightLossId of the WeightLoss creating or updating</param>
        /// <returns>WeightLossId</returns>
        public long SaveWeightLoss(WeightLossDTO weightLossDTO, string userId)
            {
                long weightLossId = 0;

                if (weightLossDTO.WeightLossId == 0)
                {

                    var weightLoss = new WeightLoss()
                    {
                        CustomerId = weightLossDTO.CustomerId,
                      DeliveryId = weightLossDTO.DeliveryId,
                         Price = weightLossDTO.Price,
                         DeliveryDate = weightLossDTO.DeliveryDate,
                        Quantity = weightLossDTO.Quantity,
                         BranchId = weightLossDTO.BranchId,
                         Approved = weightLossDTO.Approved,
                         CreatedOn = DateTime.Now,
                        TimeStamp = DateTime.Now,
                        CreatedBy = userId,
                        Deleted = false,
                        
                    };

                    this.UnitOfWork.Get<WeightLoss>().AddNew(weightLoss);
                    this.UnitOfWork.SaveChanges();
                    weightLossId = weightLoss.WeightLossId;
                    return weightLossId;
                }

                else
                {
                    var result = this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                        .FirstOrDefault(e => e.WeightLossId == weightLossDTO.WeightLossId);
                    if (result != null)
                    {
                       
                        result.CustomerId = weightLossDTO.CustomerId;
                      
                        result.DeliveryId = weightLossDTO.DeliveryId;
                       
                        result.DeliveryDate = weightLossDTO.DeliveryDate;
                        
                        result.Price = weightLossDTO.Price;
                        
                        result.Quantity = weightLossDTO.Quantity;
                       
                        result.BranchId = weightLossDTO.BranchId;
                     
                        result.UpdatedBy = userId;
                        result.TimeStamp = DateTime.Now;
                        result.Deleted = weightLossDTO.Deleted;
                        result.DeletedBy = weightLossDTO.DeletedBy;
                        result.DeletedOn = weightLossDTO.DeletedOn;
                        result.Approved = weightLossDTO.Approved;

                        this.UnitOfWork.Get<WeightLoss>().Update(result);
                        this.UnitOfWork.SaveChanges();
                    }
                    return weightLossDTO.WeightLossId;
                }
            }

        public void MarkAsDeleted(long weightLossId, string userId)
            {


                using (var dbContext = new MbaleEntities())
                {
                dbContext.Mark_WeightLoss_AsDeleted(weightLossId, userId);
            }

            }

        public void PurgeWeightLoss(long deliveryId, string userId)
        {
            this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                .Where(m => m.DeliveryId == deliveryId).Delete();
        }






    }
}

