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
 public   class WeightNoteRangeDataService : DataServiceBase,IWeightNoteRangeDataService
    {

      
            ILog logger = log4net.LogManager.GetLogger(typeof(WeightNoteRangeDataService));

            public WeightNoteRangeDataService(IUnitOfWork<MbaleEntities> unitOfWork)
                 : base(unitOfWork)
            {

            }

            public IEnumerable<WeightNoteRange> GetAllWeightNoteRanges()
            {
                return this.UnitOfWork.Get<WeightNoteRange>().AsQueryable().Where(e => e.Deleted == false).OrderByDescending(e => e.CreatedOn).Take(20);
            }
           
            public WeightNoteRange GetWeightNoteRange(long weightNoteRangeId)
            {
                return this.UnitOfWork.Get<WeightNoteRange>().AsQueryable()
                     .FirstOrDefault(c =>
                        c.WeightNoteRangeId == weightNoteRangeId &&
                        c.Deleted == false
                    );
            }
            public IEnumerable<WeightNoteRange> GetAllWeightNoteRangesForAParticularBranch(long branchId)
            {
                return this.UnitOfWork.Get<WeightNoteRange>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
            }


            public IEnumerable<WeightNoteRange> GetAllPrintedWeightNoteRangesForAParticularBranch(long branchId)
            {
                return this.UnitOfWork.Get<WeightNoteRange>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Printed == true);
            }

        public IEnumerable<WeightNoteRange> GetLatestTenPrintedWeightNoteRangeForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<WeightNoteRange>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Printed == true).OrderByDescending(e => e.CreatedOn).Take(10);
        }

        public WeightNoteRange GetLatestWeightNoteRange()
        {

            WeightNoteRange weightNoteRange = new WeightNoteRange();
            var weightNoteRanges = this.UnitOfWork.Get<WeightNoteRange>().AsQueryable();
            if (weightNoteRanges.Any())
            {
                weightNoteRange = weightNoteRanges.AsQueryable().OrderByDescending(e => e.WeightNoteRangeId).First();
                return weightNoteRange;
            }
            else
            {
                return weightNoteRange;
            }
        }
        public IEnumerable<WeightNoteRange> GetAllPrintedWeightNoteRanges()
        {
            return this.UnitOfWork.Get<WeightNoteRange>().AsQueryable().Where(e => e.Deleted == false && e.Printed == true);
        }
        public long SaveWeightNoteRange(WeightNoteRangeDTO weightNoteRangeDTO, string userId)
            {
                long weightNoteRangeId = 0;

                if (weightNoteRangeDTO.WeightNoteRangeId == 0)
                {

                    var weightNoteRange = new WeightNoteRange()
                    {
                        WeightNoteRangeId = weightNoteRangeDTO.WeightNoteRangeId,
                        Printed = weightNoteRangeDTO.Printed,
                        StartNumber  = weightNoteRangeDTO.StartNumber,
                        EndNumber = weightNoteRangeDTO.EndNumber,
                        BranchId = weightNoteRangeDTO.BranchId,
                     
                        CreatedOn = DateTime.Now,
                        TimeStamp = DateTime.Now,
                        CreatedBy = userId,
                        Deleted = false,
                    
                    };

                    this.UnitOfWork.Get<WeightNoteRange>().AddNew(weightNoteRange);
                    this.UnitOfWork.SaveChanges();
                    weightNoteRangeId = weightNoteRange.WeightNoteRangeId;
                    return weightNoteRangeId;
                }

                else
                {
                    var result = this.UnitOfWork.Get<WeightNoteRange>().AsQueryable()
                        .FirstOrDefault(e => e.WeightNoteRangeId == weightNoteRangeDTO.WeightNoteRangeId);
                    if (result != null)
                    {
                    //result.WeightNoteRangeId = weightNoteRangeDTO.WeightNoteRangeId;
                    result.Printed = weightNoteRangeDTO.Printed;
                    result.StartNumber = weightNoteRangeDTO.StartNumber;
                    result.EndNumber = weightNoteRangeDTO.EndNumber;
                    result.BranchId = weightNoteRangeDTO.BranchId;
                     
                        result.UpdatedBy = userId;
                        result.TimeStamp = DateTime.Now;
                        result.Deleted = weightNoteRangeDTO.Deleted;
                        result.DeletedBy = weightNoteRangeDTO.DeletedBy;
                        result.DeletedOn = weightNoteRangeDTO.DeletedOn;
                       

                        this.UnitOfWork.Get<WeightNoteRange>().Update(result);
                        this.UnitOfWork.SaveChanges();
                    }
                    return weightNoteRangeDTO.WeightNoteRangeId;
                }
            }

            public void MarkAsDeleted(long weightNoteRangeId, string userId)
            {


                using (var dbContext = new MbaleEntities())
                {
                    //dbContext.Mark_Delivery_AsDeleted(deliveryId, userId);
                    //TODO: THROW NOT IMPLEMENTED EXCEPTION
                }

            }

            
            //public void PurgeWeightNoteRangeWeightNoteNumbers(long weightNoteRangeId)
            //{
            //    this.UnitOfWork.Get<WeightNoteNumber>().AsQueryable()
            //        .Where(m => m.WeightNoteRangeId == weightNoteRangeId)
            //        .Delete();
            //}

          

    }
}
