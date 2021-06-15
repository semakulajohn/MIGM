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
  public  class CashDataService : DataServiceBase,ICashDataService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(CashDataService));

       public CashDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

     

       public IEnumerable<Cash> GetAllCash()
        {
            return this.UnitOfWork.Get<Cash>().AsQueryable().Where(e => e.Deleted == false); 
        }

       public Cash GetCash(long cashId)
        {
            return this.UnitOfWork.Get<Cash>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.CashId == cashId &&
                    c.Deleted == false
                );
        }

       public IEnumerable<Cash> GetAllCashForAParticularBranch(long branchId)
       {
           
            return this.UnitOfWork.Get<Cash>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId   );
        }

       public IEnumerable<Cash> GetThirtyLatestCashForAParticularBranch(long branchId)
       {

           return this.UnitOfWork.Get<Cash>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId).OrderByDescending(e => e.CashId).Take(30);
       }

       public Cash GetLatestCashForAParticularBranch(long branchId)
       {

           Cash cash = new Cash();
           var cashActivities = this.UnitOfWork.Get<Cash>().AsQueryable().Where(e => e.BranchId == branchId);
           if (cashActivities.Any())
           {
              cash = cashActivities.AsQueryable().OrderByDescending(e => e.CashId).First();
               return cash;
           }
           else
           {
               return cash;
           }
       }
                

       public long SaveCash(CashDTO cashDTO, string userId)
        {
            long cashId = 0;

            if (cashDTO.CashId == 0)
            {

                var cash = new Cash()
                {

                    Amount = cashDTO.Amount,
                    StartAmount = cashDTO.StartAmount,
                    Notes = cashDTO.Notes,
                    Action = cashDTO.Action,
                    Balance = cashDTO.Balance,
                    TransactionSubTypeId = cashDTO.TransactionSubTypeId,
                    BranchId = cashDTO.BranchId,
                    SectorId = cashDTO.SectorId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                    RequistionCategoryId = cashDTO.RequistionCategoryId,

                };

                this.UnitOfWork.Get<Cash>().AddNew(cash);
                this.UnitOfWork.SaveChanges();
                cashId = cash.CashId;
                return cashId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Cash>().AsQueryable()
                    .FirstOrDefault(e => e.CashId == cashDTO.CashId);
                if (result != null)
                {
                    result.Action = cashDTO.Action;
                    result.Amount = cashDTO.Amount;

                    result.StartAmount = cashDTO.StartAmount;
                    result.Balance = cashDTO.Balance;
                    result.Notes = cashDTO.Notes;
                    result.TransactionSubTypeId = cashDTO.TransactionSubTypeId;
                    result.BranchId = cashDTO.BranchId;
                    result.SectorId = cashDTO.SectorId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = cashDTO.Deleted;
                    result.DeletedBy = cashDTO.DeletedBy;
                    result.DeletedOn = cashDTO.DeletedOn;
                    result.RequistionCategoryId = cashDTO.RequistionCategoryId;

                    this.UnitOfWork.Get<Cash>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return cashDTO.CashId;
            }            
        }

       public void MarkAsDeleted(long cashId, string userId,long branchId)
        {


            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
                dbContext.Mark_Cash_AsDeleted(cashId, userId,branchId);
            }

        }


#region application cash

       public Application GetApplicationDetails()
       {
           return this.UnitOfWork.Get<Application>().AsQueryable().FirstOrDefault();
       }


       public long UpdateApplicationCash(ApplicationDTO applicationDTO)
       {
           var result = this.UnitOfWork.Get<Application>().AsQueryable()
                    .FirstOrDefault(e => e.ApplicationId == applicationDTO.ApplicationId);
                if (result != null)
                {
                    result.ApplicationId = applicationDTO.ApplicationId;
                    result.Name = applicationDTO.Name;
                    result.TotalCash = applicationDTO.TotalCash;
                    result.TimeStamp = DateTime.Now;

                    this.UnitOfWork.Get<Application>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return applicationDTO.ApplicationId;
           

       }
#endregion
    }
}
