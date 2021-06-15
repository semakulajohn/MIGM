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
  public  class OutSourcerOutPutDataService : DataServiceBase,IOutSourcerOutPutDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(OutSourcerOutPutDataService));

        public OutSourcerOutPutDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
        {

        }


        public IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPuts()
        {
            return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable().Where(e => e.Deleted == false);
        }

        public IEnumerable<OutSourcerOutPut> GetAllUnApprovedOutSourcerOutPuts()
        {
            return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable().Where(e => e.Deleted == false && e.Approved == null).OrderByDescending(e => e.CreatedOn).Take(20); 
        }
        public IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPuts()
        {
            return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable().Where(e => e.Deleted == false && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20); 
        }
        public IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPutsForAParticularOutSourcerStore(long storeId)
        {
            return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId);
        }

        public IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore(long storeId)
        {
            return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20); 
        }

        public long UpdateOutPutOnApprovalOrRejection(long outSourcerOutPutId, bool approved, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                dbContext.Update_OutPut_WithApprovedOrRejected(outSourcerOutPutId, approved, userId);
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }
            return outSourcerOutPutId;

        }

        public OutSourcerOutPut GetOutSourcerOutPut(long outSourcerOutPutId)
        {
            return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.OutSourcerOutPutId == outSourcerOutPutId &&
                    c.Deleted == false
                );
        }

      
        public long SaveOutSourcerOutPut(OutSourcerOutPutDTO outSourcerOutPutDTO, string userId)
        {
            long outSourcerOutPutId = 0;
            
            if (outSourcerOutPutDTO.OutSourcerOutPutId == 0)
            {

                var outSourcerOutPut = new OutSourcerOutPut()
                {

                    TotalAmount = outSourcerOutPutDTO.TotalAmount,
                    TotalQuantity = outSourcerOutPutDTO.TotalQuantity,
                    StoreId = outSourcerOutPutDTO.StoreId,
                    Approved = outSourcerOutPutDTO.Approved,
                    Price = outSourcerOutPutDTO.Price,
                    PersonLoaded = outSourcerOutPutDTO.PersonLoaded,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,



                };

                this.UnitOfWork.Get<OutSourcerOutPut>().AddNew(outSourcerOutPut);
                this.UnitOfWork.SaveChanges();
                outSourcerOutPutId = outSourcerOutPut.OutSourcerOutPutId;


                return outSourcerOutPutId;
            }

            else
            {
                var result = this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable()
                    .FirstOrDefault(e => e.OutSourcerOutPutId == outSourcerOutPutDTO.OutSourcerOutPutId);

                if (result != null)
                {
                    result.TotalQuantity = outSourcerOutPutDTO.TotalQuantity;
                    result.TotalAmount = outSourcerOutPutDTO.TotalAmount;
                    result.StoreId = outSourcerOutPutDTO.StoreId;
                    result.OutSourcerOutPutId = outSourcerOutPutDTO.OutSourcerOutPutId;
                    result.Approved = outSourcerOutPutDTO.Approved;
                    result.UpdatedBy = userId;
                    result.Price = outSourcerOutPutDTO.Price;
                    result.PersonLoaded = outSourcerOutPutDTO.PersonLoaded;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = outSourcerOutPutDTO.Deleted;
                    result.DeletedBy = outSourcerOutPutDTO.DeletedBy;
                    result.DeletedOn = outSourcerOutPutDTO.DeletedOn;

                    this.UnitOfWork.Get<OutSourcerOutPut>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return outSourcerOutPutDTO.OutSourcerOutPutId;
            }
        }

      

        public void MarkAsDeleted(long outSourcerOutPutId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
                // dbContext.Mark_Estate_And_RelatedData_AsDeleted(SectorId, userId);
            }

        }

        public void SaveOutSourcerOutPutGradeSize(OutSourcerOutPutGradeSizeDTO outSourcerOutPutGradeSizeDTO)
        {
            var outSourcerOutPutGradeSize = new OutSourcerOutPutGradeSize()
            {
                OutSourcerOutPutId = outSourcerOutPutGradeSizeDTO.OutSourcerOutPutId,
                GradeId = outSourcerOutPutGradeSizeDTO.GradeId,
                SizeId = outSourcerOutPutGradeSizeDTO.SizeId,
                Price = outSourcerOutPutGradeSizeDTO.Price,
                Quantity = outSourcerOutPutGradeSizeDTO.Quantity,
                Amount = outSourcerOutPutGradeSizeDTO.Amount,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<OutSourcerOutPutGradeSize>().AddNew(outSourcerOutPutGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeOutSourcerOutPutGradeSize(long outSourcerOutPutId)
        {
            this.UnitOfWork.Get<OutSourcerOutPutGradeSize>().AsQueryable()
                .Where(m => m.OutSourcerOutPutId == outSourcerOutPutId)
                .Delete();
        }


       
    }
}

