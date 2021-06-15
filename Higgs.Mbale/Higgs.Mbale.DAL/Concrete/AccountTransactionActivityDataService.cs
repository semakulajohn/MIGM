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
 public   class AccountTransactionActivityDataService : DataServiceBase,IAccountTransactionActivityDataService
    {
        
       ILog logger = log4net.LogManager.GetLogger(typeof(AccountTransactionActivityDataService));

       public AccountTransactionActivityDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

     

       public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivities()
        {
            return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.Deleted == false); 
        }

       public AccountTransactionActivity GetAccountTransactionActivity(long accountTransactionActivityId)
        {
            return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.AccountTransactionActivityId == accountTransactionActivityId &&
                    c.Deleted == false
                );
        }

       public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularAspNetUser(string accountId)
       {
           
            return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.Deleted == false && e.AspNetUserId == accountId   );
        }
       public IEnumerable<AccountTransactionActivity> GetLatestFortyAccountTransactionActivitiesForAParticularAspNetUser(string accountId)
       {

           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.Deleted == false && e.AspNetUserId == accountId).OrderByDescending(e => e.CreatedOn).Take(40);
       }

       public   IEnumerable<AccountTransactionActivity> GetAllAdvancedPaymentsForAParticularAspNetUser(string accountId,long transactionSubTypeId)
       {

           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.Deleted == false && e.AspNetUserId == accountId && e.TransactionSubTypeId == transactionSubTypeId).OrderByDescending(e => e.CreatedOn).Take(20);
       }
     
       public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularCasualWorker(long accountId)
       {

           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.Deleted == false && e.CasualWorkerId == accountId);
       }
       public IEnumerable<AccountTransactionActivity> GetLatestFortyAccountTransactionActivitiesForAParticularCasualWorker(long accountId)
       {

           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.Deleted == false && e.CasualWorkerId == accountId).OrderByDescending(e => e.CreatedOn).Take(40);
       }


       public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularSupply(long supplyId)
       {

           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.Deleted == false && e.SupplyId == supplyId);
       }

       public AccountTransactionActivity GetLatestAccountTransactionActivityForAParticularAspNetUser(string accountId)
       {
           AccountTransactionActivity accountTransactionActivity = new AccountTransactionActivity();
           var accountTransactionActivities = this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.AspNetUserId == accountId && e.Deleted == false);
           if (accountTransactionActivities.Any())
           {
               accountTransactionActivity = accountTransactionActivities.AsQueryable().OrderByDescending(e => e.AccountTransactionActivityId).First();

                //accountTransactionActivity = accountTransactionActivities.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
                return accountTransactionActivity;
           }
           else
           {
               return accountTransactionActivity;
           }
          
       }

        public AccountTransactionActivity GetLatestAccountTransactionActivityForAParticularAspNetUserForAParticularDate(string accountId,DateTime dateTime)
        {
            AccountTransactionActivity accountTransactionActivity = new AccountTransactionActivity();
            var accountTransactionActivities = this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.AspNetUserId == accountId && e.Deleted == false && e.CreatedOn <= dateTime);
            if (accountTransactionActivities.Any())
            {
                accountTransactionActivity = accountTransactionActivities.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
                return accountTransactionActivity;
            }
            else
            {
                return accountTransactionActivity;
            }

        }
        public AccountTransactionActivity GetLatestAccountTransactionActivitiesForAParticularCasualWorker(long casualWorkerId)
       {

           AccountTransactionActivity accountTransactionActivity = new AccountTransactionActivity();
           var accountTransactionActivities = this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable().Where(e => e.CasualWorkerId == casualWorkerId && e.Deleted == false);
           if (accountTransactionActivities.Any())
           {
              accountTransactionActivity = accountTransactionActivities.AsQueryable().OrderByDescending(e => e.AccountTransactionActivityId).First();
               return accountTransactionActivity;
           }
           else
           {
               return accountTransactionActivity;
           }
       }
                

       public long SaveAccountTransactionActivity(AccountTransactionActivityDTO accountTransactionActivityDTO, string userId)
        {
            long accountTransactionActivityId = 0;

            if (accountTransactionActivityDTO.AccountTransactionActivityId == 0)
            {

                var accountTransactionActivity = new AccountTransactionActivity()
                {
                    AspNetUserId = accountTransactionActivityDTO.AspNetUserId,
                    CasualWorkerId = accountTransactionActivityDTO.CasualWorkerId,
                    Amount = accountTransactionActivityDTO.Amount,
                    StartAmount = accountTransactionActivityDTO.StartAmount,
                    Notes  = accountTransactionActivityDTO.Notes,
                    Action = accountTransactionActivityDTO.Action,
                    Balance = accountTransactionActivityDTO.Balance,
                    SupplyId = accountTransactionActivityDTO.SupplyId,
                    TransactionSubTypeId = accountTransactionActivityDTO.TransactionSubTypeId,
                    BranchId = accountTransactionActivityDTO.BranchId,
                    SectorId = accountTransactionActivityDTO.SectorId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                    WeightNote = accountTransactionActivityDTO.WeightNote,
                    Bags = accountTransactionActivityDTO.Bags,
                    Price = accountTransactionActivityDTO.Price,
                    Quantity = accountTransactionActivityDTO.Quantity,
                };

                this.UnitOfWork.Get<AccountTransactionActivity>().AddNew(accountTransactionActivity);
                this.UnitOfWork.SaveChanges();
                accountTransactionActivityId = accountTransactionActivity.AccountTransactionActivityId;
                return accountTransactionActivityId;
            }

            else
            {
                var result = this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                    .FirstOrDefault(e => e.AccountTransactionActivityId == accountTransactionActivityDTO.AccountTransactionActivityId);
                if (result != null)
                {
                    result.Action = accountTransactionActivityDTO.Action;
                    result.Amount = accountTransactionActivityDTO.Amount;
                    result.AspNetUserId = accountTransactionActivityDTO.AspNetUserId;
                    result.CasualWorkerId = accountTransactionActivityDTO.CasualWorkerId;
                    result.StartAmount = accountTransactionActivityDTO.StartAmount;
                    result.Balance = accountTransactionActivityDTO.Balance;
                    result.Notes = accountTransactionActivityDTO.Notes;
                    result.SupplyId = accountTransactionActivityDTO.SupplyId;
                    result.TransactionSubTypeId = accountTransactionActivityDTO.TransactionSubTypeId;
                    result.BranchId = accountTransactionActivityDTO.BranchId;
                    result.SectorId = accountTransactionActivityDTO.SectorId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = accountTransactionActivityDTO.Deleted;
                    result.DeletedBy = accountTransactionActivityDTO.DeletedBy;
                    result.DeletedOn = accountTransactionActivityDTO.DeletedOn;
                    result.WeightNote = accountTransactionActivityDTO.WeightNote;
                    result.Quantity = accountTransactionActivityDTO.Quantity;
                    result.Price = accountTransactionActivityDTO.Price;
                    result.Bags = accountTransactionActivityDTO.Bags;

                    this.UnitOfWork.Get<AccountTransactionActivity>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return accountTransactionActivityDTO.AccountTransactionActivityId;
            }            
        }

       public void MarkAsDeleted(long accountTransactionActivityId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
                dbContext.Mark_AccountTransactionActivity_AsDeleted(accountTransactionActivityId, userId);
            }

        }

       public IEnumerable<PaymentMode> GetAllPaymentModes()
       {
           return this.UnitOfWork.Get<PaymentMode>().AsQueryable().Where(e => e.Deleted == false);
       }

       public PaymentMode GetPaymentMode(long paymentModeId)
       {
           return this.UnitOfWork.Get<PaymentMode>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.PaymentModeId == paymentModeId &&
                    c.Deleted == false
                );
       }
    }
}
