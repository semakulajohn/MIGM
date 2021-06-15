using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;

namespace Higgs.Mbale.DAL.Concrete
{
  public  class WeightNoteNumberDataService : DataServiceBase, IWeightNoteNumberDataService
    {

        ILog logger = log4net.LogManager.GetLogger(typeof(WeightNoteNumberDataService));

        public WeightNoteNumberDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
        {

        }

        public IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbers()
        {
            return this.UnitOfWork.Get<WeightNoteNumber>().AsQueryable().Where(e => e.Deleted == false).OrderByDescending(e => e.CreatedOn).Take(50);
        }

        public WeightNoteNumber GetWeightNoteNumber(long weightNoteNumberId)
        {
            return this.UnitOfWork.Get<WeightNoteNumber>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.WeightNoteNumberId == weightNoteNumberId &&
                    c.Deleted == false
                );
        }
        public IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId)
        {
            return this.UnitOfWork.Get<WeightNoteNumber>().AsQueryable().Where(e => e.Deleted == false && e.WeightNoteRangeId == weightNoteRangeId);
        }

        public IEnumerable<WeightNoteNumber> GetAllNotUsedWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId)
        {
            return this.UnitOfWork.Get<WeightNoteNumber>().AsQueryable().Where(e => e.Deleted == false && e.WeightNoteRangeId == weightNoteRangeId && e.Used == false && e.NotUsed ==false);
        }


        public IEnumerable<WeightNoteNumber> GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<WeightNoteNumber>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Used == false && e.NotUsed == false).OrderByDescending(e => e.CreatedOn).Take(200);
        }
        public long SaveWeightNoteNumber(WeightNoteNumberDTO weightNoteNumberDTO, string userId)
        {
            long weightNoteNumberId = 0;

            if (weightNoteNumberDTO.WeightNoteNumberId == 0)
            {

                var weightNoteNumber = new WeightNoteNumber()
                {
                    WeightNoteNumberId = weightNoteNumberDTO.WeightNoteNumberId,
                    WeightNoteValue = weightNoteNumberDTO.WeightNoteValue,
                    WeightNoteRangeId = weightNoteNumberDTO.WeightNoteRangeId,
                    Used = weightNoteNumberDTO.Used,
                    BranchId = weightNoteNumberDTO.BranchId,
                    Notes = weightNoteNumberDTO.Notes,
                    NotUsed = weightNoteNumberDTO.NotUsed,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,

                };

                this.UnitOfWork.Get<WeightNoteNumber>().AddNew(weightNoteNumber);
                this.UnitOfWork.SaveChanges();
                weightNoteNumberId = weightNoteNumber.WeightNoteNumberId;
                return weightNoteNumberId;
            }

            else
            {
                var result = this.UnitOfWork.Get<WeightNoteNumber>().AsQueryable()
                    .FirstOrDefault(e => e.WeightNoteNumberId == weightNoteNumberDTO.WeightNoteNumberId);
                if (result != null)
                {
                    
                    result.WeightNoteValue = weightNoteNumberDTO.WeightNoteValue;
                    result.WeightNoteRangeId = weightNoteNumberDTO.WeightNoteRangeId;
                    result.Used = weightNoteNumberDTO.Used;
                    result.BranchId = weightNoteNumberDTO.BranchId;
                    result.Notes = weightNoteNumberDTO.Notes;
                    result.NotUsed = weightNoteNumberDTO.NotUsed;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = weightNoteNumberDTO.Deleted;
                    result.DeletedBy = weightNoteNumberDTO.DeletedBy;
                    result.DeletedOn = weightNoteNumberDTO.DeletedOn;


                    this.UnitOfWork.Get<WeightNoteNumber>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return weightNoteNumberDTO.WeightNoteNumberId;
            }
        }

        public void SaveWeightNoteSupply(WeightNoteSupplyDTO weightNoteSupplyDTO)
        {
                           

                var weightNoteSupply = new WeightNoteSupply()
                {
                    SupplyId = weightNoteSupplyDTO.SupplyId,
                    WeightNoteNumberId = weightNoteSupplyDTO.WeightNoteNumberId,
                   
                    CreatedOn = DateTime.Now,
                   

                };

                this.UnitOfWork.Get<WeightNoteSupply>().AddNew(weightNoteSupply);
                this.UnitOfWork.SaveChanges();
               
                        
        }
        public void MarkAsDeleted(long weightNoteNumberId, string userId)
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
