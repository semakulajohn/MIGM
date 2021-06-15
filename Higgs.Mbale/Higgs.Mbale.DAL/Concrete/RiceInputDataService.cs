using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DAL.Interface;
using log4net;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using EntityFramework.Extensions;

namespace Higgs.Mbale.DAL.Concrete
{
  public  class RiceInputDataService : DataServiceBase,IRiceInputDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(RiceInputDataService));

        public RiceInputDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
        {

        }


        public IEnumerable<RiceInput> GetAllRiceInputs()
        {
            return this.UnitOfWork.Get<RiceInput>().AsQueryable().Where(e => e.Deleted == false).OrderByDescending(e => e.CreatedOn).Take(20); 
        }

        public IEnumerable<RiceInput> GetAllUnApprovedRiceInputs()
        {
            return this.UnitOfWork.Get<RiceInput>().AsQueryable().Where(e => e.Deleted == false && e.Approved == null).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public IEnumerable<RiceInput> GetAllApprovedRiceInputs()
        {
            return this.UnitOfWork.Get<RiceInput>().AsQueryable().Where(e => e.Deleted == false && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public IEnumerable<RiceInput> GetAllRiceInputsForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<RiceInput>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
        }

        public IEnumerable<RiceInput> GetAllApprovedRiceInputsForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<RiceInput>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public long UpdateRiceInputOnApprovalOrRejection(long riceInputId, bool approved, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                dbContext.Update_RiceInPut_WithApprovedOrRejected(riceInputId, approved, userId);
                
            }
            return riceInputId;

        }

        public RiceInput GetRiceInput(long riceInputId)
        {
            return this.UnitOfWork.Get<RiceInput>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.RiceInputId == riceInputId &&
                    c.Deleted == false
                );
        }


        public long SaveRiceInput(RiceInputDTO riceInputDTO, string userId)
        {
            long riceInputId = 0;

            if (riceInputDTO.RiceInputId == 0)
            {

                var riceInput = new RiceInput()
                {

                    TotalAmount = riceInputDTO.TotalAmount,
                    TotalQuantity = riceInputDTO.TotalQuantity,
                    StoreId = riceInputDTO.StoreId,
                    Approved = riceInputDTO.Approved,
                    Price = riceInputDTO.Price,
                    BranchId = riceInputDTO.BranchId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,



                };

                this.UnitOfWork.Get<RiceInput>().AddNew(riceInput);
                this.UnitOfWork.SaveChanges();
                riceInputId = riceInput.RiceInputId;


                return riceInputId;
            }

            else
            {
                var result = this.UnitOfWork.Get<RiceInput>().AsQueryable()
                    .FirstOrDefault(e => e.RiceInputId == riceInputDTO.RiceInputId);

                if (result != null)
                {
                    result.TotalQuantity = riceInputDTO.TotalQuantity;
                    result.TotalAmount = riceInputDTO.TotalAmount;
                    result.StoreId = riceInputDTO.StoreId;
                    result.RiceInputId = riceInputDTO.RiceInputId;
                    result.Approved = riceInputDTO.Approved;
                    result.UpdatedBy = userId;
                    result.Price = riceInputDTO.Price;
                    result.BranchId = riceInputDTO.BranchId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = riceInputDTO.Deleted;
                    result.DeletedBy = riceInputDTO.DeletedBy;
                    result.DeletedOn = riceInputDTO.DeletedOn;

                    this.UnitOfWork.Get<RiceInput>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return riceInputDTO.RiceInputId;
            }
        }



        public void MarkAsDeleted(long riceInputId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
                
            }

        }

        public void SaveRiceInputGradeSize(RiceInputGradeSizeDTO riceInputGradeSizeDTO)
        {
            var riceInputGradeSize = new RiceInputGradeSize()
            {
                RiceInputId = riceInputGradeSizeDTO.RiceInputId,
                GradeId = riceInputGradeSizeDTO.GradeId,
                SizeId = riceInputGradeSizeDTO.SizeId,
                Price = riceInputGradeSizeDTO.Price,
                Quantity = riceInputGradeSizeDTO.Quantity,
                Amount = riceInputGradeSizeDTO.Amount,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<RiceInputGradeSize>().AddNew(riceInputGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeRiceInputGradeSize(long riceInputId)
        {
            this.UnitOfWork.Get<RiceInputGradeSize>().AsQueryable()
                .Where(m => m.RiceInputId == riceInputId)
                .Delete();
        }


    }
}
