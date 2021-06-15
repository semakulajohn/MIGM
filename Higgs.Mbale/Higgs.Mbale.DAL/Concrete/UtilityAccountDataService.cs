using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Concrete;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;

namespace Higgs.Mbale.DAL.Concrete
{
  public  class UtilityAccountDataService : DataServiceBase,IUtilityAccountDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(UtilityAccountDataService));

       public UtilityAccountDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

       public IEnumerable<UtilityAccount> GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory(long branchId, long categoryId)
       {
           return this.UnitOfWork.Get<UtilityAccount>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId&& e.UtilityCategoryId == categoryId).OrderByDescending(e => e.CreatedOn).Take(20);
           
       }


       public IEnumerable<UtilityAccount> GetAllUtilityAccounts()
        {
            return this.UnitOfWork.Get<UtilityAccount>().AsQueryable().Where(e => e.Deleted == false); 
        }

       public UtilityAccount GetUtilityAccount(long utilityAccountId)
        {
            return this.UnitOfWork.Get<UtilityAccount>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.UtilityAccountId == utilityAccountId &&
                    c.Deleted == false
                );
        }

       public IEnumerable<UtilityAccount> GetAllUtilityAccountForAParticularBranch(long branchId)
       {
           
            return this.UnitOfWork.Get<UtilityAccount>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId   );
        }
      
     
       public UtilityAccount GetLatestUtilityAccountForAParticularBranchAndCategory(long branchId,long categoryId)
       {

           UtilityAccount utilityAccount = new UtilityAccount();
           var utilityAccounts = this.UnitOfWork.Get<UtilityAccount>().AsQueryable().Where(e => e.BranchId == branchId && e.UtilityCategoryId == categoryId && e.Deleted == false);
           if (utilityAccounts.Any())
           {
              utilityAccount = utilityAccounts.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
               return utilityAccount;
           }
           else
           {
               return utilityAccount;
           }
       }


        public UtilityAccount GetLatestUtilityAccountForAParticularBranchAndCategoryForAParticularDate(long branchId, long categoryId,DateTime dateTime)
        {

            UtilityAccount utilityAccount = new UtilityAccount();
            var utilityAccounts = this.UnitOfWork.Get<UtilityAccount>().AsQueryable().Where(e => e.BranchId == branchId && e.UtilityCategoryId == categoryId && e.Deleted == false && e.CreatedOn <= dateTime);
            if (utilityAccounts.Any())
            {
                utilityAccount = utilityAccounts.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
                return utilityAccount;
            }
            else
            {
                return utilityAccount;
            }
        }

        public long SaveUtilityAccount(UtilityAccountDTO utilityAccountDTO, string userId)
       {
           long utilityAccountId = 0;

           if (utilityAccountDTO.UtilityAccountId == 0)
           {

               var utilityAccount = new UtilityAccount()
               {

                   UtilityAccountId = utilityAccountDTO.UtilityAccountId,
                   Amount = utilityAccountDTO.Amount,
                   StartAmount = utilityAccountDTO.StartAmount,
                   Description = utilityAccountDTO.Description,
                   InvoiceNumber = utilityAccountDTO.InvoiceNumber,
                  
                   Action = utilityAccountDTO.Action,
                   Balance = utilityAccountDTO.Balance,

                   BranchId = utilityAccountDTO.BranchId,
                 UtilityCategoryId = utilityAccountDTO.UtilityCategoryId,
                   CreatedOn = DateTime.Now,
                   TimeStamp = DateTime.Now,
                   CreatedBy = userId,
                   Deleted = false,
               };

               this.UnitOfWork.Get<UtilityAccount>().AddNew(utilityAccount);
               this.UnitOfWork.SaveChanges();
               utilityAccountId = utilityAccount.UtilityAccountId;
               return utilityAccountId;
           }

           else
           {
               var result = this.UnitOfWork.Get<UtilityAccount>().AsQueryable()
                   .FirstOrDefault(e => e.UtilityAccountId == utilityAccountDTO.UtilityAccountId);
               if (result != null)
               {
                   result.Action = utilityAccountDTO.Action;
                   result.Amount = utilityAccountDTO.Amount;
                   result.InvoiceNumber = utilityAccountDTO.InvoiceNumber;
                   result.Description = utilityAccountDTO.Description;
                   result.StartAmount = utilityAccountDTO.StartAmount;
                   result.Balance = utilityAccountDTO.Balance;
                   result.UtilityCategoryId = utilityAccountDTO.UtilityCategoryId;
                   result.BranchId = utilityAccountDTO.BranchId;
                   
                   result.TimeStamp = DateTime.Now;
                   result.Deleted = utilityAccountDTO.Deleted;
                   result.DeletedBy = utilityAccountDTO.DeletedBy;
                   result.DeletedOn = utilityAccountDTO.DeletedOn;

                   this.UnitOfWork.Get<UtilityAccount>().Update(result);
                   this.UnitOfWork.SaveChanges();
               }
               return utilityAccountDTO.UtilityAccountId;
           }
       }

       public void MarkAsDeleted(long utilityAccountId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
              //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }

       public IEnumerable<UtilityCategory> GetAllUtilityCategories()
       {
           return this.UnitOfWork.Get<UtilityCategory>().AsQueryable();
       }

       public UtilityCategory GetUtilityCategory(long utilityCategoryId)
       {
           return this.UnitOfWork.Get<UtilityCategory>().AsQueryable()
                .FirstOrDefault(c =>
                   c.UtilityCategoryId == utilityCategoryId 
               );
       }
    }
}
