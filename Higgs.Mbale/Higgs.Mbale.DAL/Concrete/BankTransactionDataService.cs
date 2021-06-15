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
        public  class BankTransactionDataService : DataServiceBase,IBankTransactionDataService
        {
         ILog logger = log4net.LogManager.GetLogger(typeof(BankTransactionDataService));

       public BankTransactionDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }



       public IEnumerable<BankTransaction> GetAllBankTransactions()
        {
            return this.UnitOfWork.Get<BankTransaction>().AsQueryable().Where(e => e.Deleted == false); 
        }

       public BankTransaction GetBankTransaction(long bankTransactionId)
        {
            return this.UnitOfWork.Get<BankTransaction>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.BankTransactionId == bankTransactionId &&
                    c.Deleted == false
                );
        }

      public IEnumerable<BankTransaction>  GetLatestTwentyBankTransactionsForAParticularBranchAndBank(long branchId, long bankId)
      {
          return this.UnitOfWork.Get<BankTransaction>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId && e.BankId == bankId).OrderByDescending(e => e.CreatedOn).Take(20);
       
      }
       public BankTransaction GetLatestBankTransactionForAParticularBranchAndBank(long branchId, long bankId)
       {

           BankTransaction bankTransaction = new BankTransaction();
           var bankTransactions = this.UnitOfWork.Get<BankTransaction>().AsQueryable().Where(e => e.BranchId == branchId && e.BankId == bankId && e.Deleted == false);
           if (bankTransactions.Any())
           {
               bankTransaction = bankTransactions.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
               return bankTransaction;
           }
           else
           {
               return bankTransaction;
           }
       }  

       public long SaveBankTransaction(BankTransactionDTO bankTransactionDTO, string userId)
        {
            long bankTransactionId = 0;

            if (bankTransactionDTO.BankTransactionId == 0)
            {

                var bankTransaction = new BankTransaction()
                {
                   
                    Amount = bankTransactionDTO.Amount,
                    StartAmount = bankTransactionDTO.StartAmount,
                    Notes  = bankTransactionDTO.Notes,
                    Action = bankTransactionDTO.Action,
                    Balance = bankTransactionDTO.Balance,
                  
                    TransactionSubTypeId = bankTransactionDTO.TransactionSubTypeId,
                    BranchId = bankTransactionDTO.BranchId,
                    SectorId = bankTransactionDTO.SectorId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                   BankId = bankTransactionDTO.BankId,
                };

                this.UnitOfWork.Get<BankTransaction>().AddNew(bankTransaction);
                this.UnitOfWork.SaveChanges();
                bankTransactionId = bankTransaction.BankTransactionId;
                return bankTransactionId;
            }

            else
            {
                var result = this.UnitOfWork.Get<BankTransaction>().AsQueryable()
                    .FirstOrDefault(e => e.BankTransactionId == bankTransactionDTO.BankTransactionId);
                if (result != null)
                {
                    result.Action = bankTransactionDTO.Action;
                    result.Amount = bankTransactionDTO.Amount;
                    result.BankId = bankTransactionDTO.BankId;
                    result.StartAmount = bankTransactionDTO.StartAmount;
                    result.Balance = bankTransactionDTO.Balance;
                    result.Notes = bankTransactionDTO.Notes;
                    
                    result.TransactionSubTypeId = bankTransactionDTO.TransactionSubTypeId;
                    result.BranchId = bankTransactionDTO.BranchId;
                    result.SectorId = bankTransactionDTO.SectorId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = bankTransactionDTO.Deleted;
                    result.DeletedBy = bankTransactionDTO.DeletedBy;
                    result.DeletedOn = bankTransactionDTO.DeletedOn;
                   

                    this.UnitOfWork.Get<BankTransaction>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return bankTransactionDTO.BankTransactionId;
            }            
        }

       public void MarkAsDeleted(long bankTransactionId, string userId)
       {


           using (var dbContext = new MbaleEntities())
           {
               // dbContext.Mark_Estate_And_RelatedData_AsDeleted(SectorId, userId);
           }

       }


     
    }
}
