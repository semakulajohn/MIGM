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
public    class MillingChargeDataService : DataServiceBase,IMillingChargeDataService
    {
    ILog logger = log4net.LogManager.GetLogger(typeof(MillingChargeDataService));

       public MillingChargeDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }



       public IEnumerable<MillingCharge> GetAllMillingCharges()
        {
            return this.UnitOfWork.Get<MillingCharge>().AsQueryable().Where(e => e.Deleted == false); 
        }

       public MillingCharge GetMillingCharge(long millingChargeId)
        {
            return this.UnitOfWork.Get<MillingCharge>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.MillingChargeId == millingChargeId &&
                    c.Deleted == false
                );
        }

       public MillingCharge GetLatestMillingChargeForAParticularBranch(long branchId)
       {

           MillingCharge millingCharge = new MillingCharge();
           var millingCharges = this.UnitOfWork.Get<MillingCharge>().AsQueryable().Where(e => e.BranchId == branchId && e.Deleted == false);
           if (millingCharges.Any())
           {
               millingCharge = millingCharges.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
               return millingCharge;
           }
           else
           {
               return millingCharge;
           }
       }  

       public long SaveMillingCharge(MillingChargeDTO millingChargeDTO, string userId)
        {
            long millingChargeId = 0;

            if (millingChargeDTO.MillingChargeId == 0)
            {

                var millingCharge = new MillingCharge()
                {
                   
                    Amount = millingChargeDTO.Amount,
                    StartAmount = millingChargeDTO.StartAmount,
                    Notes  = millingChargeDTO.Notes,
                    Action = millingChargeDTO.Action,
                    Balance = millingChargeDTO.Balance,
                  
                    TransactionSubTypeId = millingChargeDTO.TransactionSubTypeId,
                    BranchId = millingChargeDTO.BranchId,
                    SectorId = millingChargeDTO.SectorId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                    BatchId= millingChargeDTO.BatchId,
                };

                this.UnitOfWork.Get<MillingCharge>().AddNew(millingCharge);
                this.UnitOfWork.SaveChanges();
                millingChargeId = millingCharge.MillingChargeId;
                return millingChargeId;
            }

            else
            {
                var result = this.UnitOfWork.Get<MillingCharge>().AsQueryable()
                    .FirstOrDefault(e => e.MillingChargeId == millingChargeDTO.MillingChargeId);
                if (result != null)
                {
                    result.Action = millingChargeDTO.Action;
                    result.Amount = millingChargeDTO.Amount;
                    result.BatchId = millingChargeDTO.BatchId;
                    result.StartAmount = millingChargeDTO.StartAmount;
                    result.Balance = millingChargeDTO.Balance;
                    result.Notes = millingChargeDTO.Notes;
                    
                    result.TransactionSubTypeId = millingChargeDTO.TransactionSubTypeId;
                    result.BranchId = millingChargeDTO.BranchId;
                    result.SectorId = millingChargeDTO.SectorId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = millingChargeDTO.Deleted;
                    result.DeletedBy = millingChargeDTO.DeletedBy;
                    result.DeletedOn = millingChargeDTO.DeletedOn;
                   

                    this.UnitOfWork.Get<MillingCharge>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return millingChargeDTO.MillingChargeId;
            }            
        }

       public void MarkAsDeleted(long millingChargeId, string userId)
       {


           using (var dbContext = new MbaleEntities())
           {
               // dbContext.Mark_Estate_And_RelatedData_AsDeleted(branchId, userId);
           }

       }
    }
}
