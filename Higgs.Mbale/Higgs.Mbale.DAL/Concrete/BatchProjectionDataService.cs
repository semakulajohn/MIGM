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
 public   class BatchProjectionDataService :DataServiceBase,IBatchProjectionDataService
    {
           ILog logger = log4net.LogManager.GetLogger(typeof(BatchProjectionDataService));

       public BatchProjectionDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

      
        public IEnumerable<BatchProjection> GetAllBatchProjections()
        {
            return this.UnitOfWork.Get<BatchProjection>().AsQueryable().Where(e => e.Deleted == false); 
        }

        public IEnumerable<BatchProjection> GetAllBatchProjectionsForAParticularBatch(long batchId)
        {
            return this.UnitOfWork.Get<BatchProjection>().AsQueryable().Where(e => e.Deleted == false && e.BatchId == batchId);
        }


        public BatchProjection GetBatchProjection(long batchProjectionId)
        {
            return this.UnitOfWork.Get<BatchProjection>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.BatchProjectionId == batchProjectionId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new BatchProjection or updates an already existing BatchProjection.
        /// </summary>
        /// <param name="Batch">BatchProjection to be saved or updated.</param>
        /// <param name="BatchId">BatchProjectionId of the BatchProjection creating or updating</param>
        /// <returns>BatchProjectionId</returns>
        public long SaveBatchProjection(BatchProjectionDTO batchProjectionDTO, string userId)
        {
            long batchProjectionId = 0;
            var resultBatch = this.UnitOfWork.Get<BatchProjection>().AsQueryable()
                    .FirstOrDefault(e => e.BatchProjectionId == batchProjectionDTO.BatchProjectionId);
           
                if (batchProjectionDTO.BatchProjectionId == 0)
                {

                    var batchProjection = new BatchProjection()
                    {
                        FlourPrice = batchProjectionDTO.FlourPrice,
                        FlourSales = batchProjectionDTO.FlourSales,
                        FlourOutPut = batchProjectionDTO.FlourOutPut,
                        BrandOutPut = batchProjectionDTO.BrandOutPut,
                        BatchId = batchProjectionDTO.BatchId,
                        BrandPrice = batchProjectionDTO.BrandPrice,
                        BrandSales = batchProjectionDTO.BrandSales,
                        BrandPercentage = batchProjectionDTO.BrandPercentage,
                        FlourPercentage = batchProjectionDTO.FlourPercentage,
                        BranchId = batchProjectionDTO.BranchId,
                       BatchProjectionId = batchProjectionDTO.BatchProjectionId,
                        CreatedOn = DateTime.Now,
                        TimeStamp = DateTime.Now,
                        CreatedBy = userId,
                        Deleted = false,
                        UnitCost = batchProjectionDTO.UnitCost,
                        ProductionCost = batchProjectionDTO.ProductionCost,
                        ExpectedContribution = batchProjectionDTO.ExpectedContribution,     

                    };

                    this.UnitOfWork.Get<BatchProjection>().AddNew(batchProjection);
                    this.UnitOfWork.SaveChanges();
                    batchProjectionId = batchProjection.BatchProjectionId;


                    return batchProjectionId;
                }

                else
                {
                    var result = this.UnitOfWork.Get<BatchProjection>().AsQueryable()
                        .FirstOrDefault(e => e.BatchProjectionId == batchProjectionDTO.BatchProjectionId);
                    if (result != null)
                    {
                        result.FlourPrice = batchProjectionDTO.FlourPrice;
                        result.FlourOutPut = batchProjectionDTO.FlourOutPut;
                        result.BrandOutPut = batchProjectionDTO.BrandOutPut;
                        result.BatchId = batchProjectionDTO.BatchId;
                        result.FlourSales = batchProjectionDTO.FlourSales;
                        result.BrandPercentage = batchProjectionDTO.BrandPercentage;
                        result.FlourPercentage = batchProjectionDTO.FlourPercentage;
                        result.BrandPrice = batchProjectionDTO.BrandPrice;
                        result.BranchId = batchProjectionDTO.BranchId;
                        result.UpdatedBy = userId;
                        result.TimeStamp = DateTime.Now;
                        result.Deleted = batchProjectionDTO.Deleted;
                        result.DeletedBy = batchProjectionDTO.DeletedBy;
                        result.DeletedOn = batchProjectionDTO.DeletedOn;
                        result.UnitCost = batchProjectionDTO.UnitCost;
                        result.BatchProjectionId = batchProjectionDTO.BatchProjectionId;
                        result.ProductionCost = batchProjectionDTO.ProductionCost;
                        result.ExpectedContribution = batchProjectionDTO.ExpectedContribution;

                        this.UnitOfWork.Get<BatchProjection>().Update(result);
                        this.UnitOfWork.SaveChanges();
                    }
                    return batchProjectionDTO.BatchProjectionId;
                }    
            }
                  
   

        public void MarkAsDeleted(long BatchProjectionId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
               // dbContext.Mark_Estate_And_RelatedData_AsDeleted(SectorId, userId);
            }

        }

       
    

     }
}
