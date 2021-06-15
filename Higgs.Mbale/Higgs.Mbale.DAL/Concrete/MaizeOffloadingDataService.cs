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
        public  class MaizeOffloadingDataService : DataServiceBase,IMaizeOffloadingDataService
        {
         ILog logger = log4net.LogManager.GetLogger(typeof(MaizeOffloadingDataService));

       public MaizeOffloadingDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }



       public IEnumerable<MaizeOffloading> GetAllMaizeOffloadings()
        {
            return this.UnitOfWork.Get<MaizeOffloading>().AsQueryable().Where(e => e.Deleted == false); 
        }

       public MaizeOffloading GetMaizeOffloading(long maizeOffloadingId)
        {
            return this.UnitOfWork.Get<MaizeOffloading>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.MaizeOffloadingId == maizeOffloadingId &&
                    c.Deleted == false
                );
        }

       public MaizeOffloading GetLatestMaizeOffloadingForAParticularBranch(long branchId)
       {

           MaizeOffloading maizeOffloading = new MaizeOffloading();
           var maizeOffloadings = this.UnitOfWork.Get<MaizeOffloading>().AsQueryable().Where(e => e.BranchId == branchId  && e.Deleted == false);
           if (maizeOffloadings.Any())
           {
               maizeOffloading = maizeOffloadings.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
               return maizeOffloading;
           }
           else
           {
               return maizeOffloading;
           }
       }  

       public long SaveMaizeOffloading(MaizeOffloadingDTO maizeOffloadingDTO, string userId)
        {
            long maizeOffloadingId = 0;

            if (maizeOffloadingDTO.MaizeOffloadingId == 0)
            {

                var maizeOffloading = new MaizeOffloading()
                {
                   
                    Amount = maizeOffloadingDTO.Amount,
                    StartAmount = maizeOffloadingDTO.StartAmount,
                    Notes  = maizeOffloadingDTO.Notes,
                    Action = maizeOffloadingDTO.Action,
                    Balance = maizeOffloadingDTO.Balance,
                   SupplyId = maizeOffloadingDTO.SupplyId,
                   WeightNoteNumber = maizeOffloadingDTO.WeightNoteNumber,
                    TransactionSubTypeId = maizeOffloadingDTO.TransactionSubTypeId,
                    BranchId = maizeOffloadingDTO.BranchId,
                    SectorId = maizeOffloadingDTO.SectorId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                   
                };

                this.UnitOfWork.Get<MaizeOffloading>().AddNew(maizeOffloading);
                this.UnitOfWork.SaveChanges();
                maizeOffloadingId = maizeOffloading.MaizeOffloadingId;
                return maizeOffloadingId;
            }

            else
            {
                var result = this.UnitOfWork.Get<MaizeOffloading>().AsQueryable()
                    .FirstOrDefault(e => e.MaizeOffloadingId == maizeOffloadingDTO.MaizeOffloadingId);
                if (result != null)
                {
                    result.Action = maizeOffloadingDTO.Action;
                    result.Amount = maizeOffloadingDTO.Amount;
                    result.SupplyId = maizeOffloadingDTO.SupplyId;
                    result.StartAmount = maizeOffloadingDTO.StartAmount;
                    result.Balance = maizeOffloadingDTO.Balance;
                    result.Notes = maizeOffloadingDTO.Notes;
                    result.WeightNoteNumber = maizeOffloadingDTO.WeightNoteNumber;
                    result.TransactionSubTypeId = maizeOffloadingDTO.TransactionSubTypeId;
                    result.BranchId = maizeOffloadingDTO.BranchId;
                    result.SectorId = maizeOffloadingDTO.SectorId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = maizeOffloadingDTO.Deleted;
                    result.DeletedBy = maizeOffloadingDTO.DeletedBy;
                    result.DeletedOn = maizeOffloadingDTO.DeletedOn;
                   

                    this.UnitOfWork.Get<MaizeOffloading>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return maizeOffloadingDTO.MaizeOffloadingId;
            }            
        }

       public void MarkAsDeleted(long maizeOffloadingId, string userId)
       {


           using (var dbContext = new MbaleEntities())
           {
               // dbContext.Mark_Estate_And_RelatedData_AsDeleted(branchId, userId);
           }

       }
     
    }
}
