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
 public   class FinancialAccountTransactionDataService : DataServiceBase,IFinancialAccountTransactionDataService
    {
            ILog logger = log4net.LogManager.GetLogger(typeof(FinancialAccountTransactionDataService));

            public FinancialAccountTransactionDataService(IUnitOfWork<MbaleEntities> unitOfWork)
                 : base(unitOfWork)
            {

            }



            public IEnumerable<FinancialAccountTransaction> GetAllFinancialAccountTransactions()
            {
                return this.UnitOfWork.Get<FinancialAccountTransaction>().AsQueryable().Where(e => e.Deleted == false);
            }

            public FinancialAccountTransaction GetFinancialAccountTransaction(long financialAccountTransactionId)
            {
                return this.UnitOfWork.Get<FinancialAccountTransaction>().AsQueryable()
                     .FirstOrDefault(c =>
                        c.FinancialAccountTransactionId == financialAccountTransactionId &&
                        c.Deleted == false
                    );
            }

            public IEnumerable<FinancialAccountTransaction> GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount( long financialAccountId)
            {
                return this.UnitOfWork.Get<FinancialAccountTransaction>().AsQueryable().Where(e => e.Deleted == false &&  e.FinancialAccountId == financialAccountId).OrderByDescending(e => e.CreatedOn).Take(20);

            }
            public FinancialAccountTransaction GetLatestFinancialAccountTransactionForAParticularFinancialAccount( long financialAccountId)
            {

                FinancialAccountTransaction financialAccountTransaction = new FinancialAccountTransaction();
                var financialAccountTransactions = this.UnitOfWork.Get<FinancialAccountTransaction>().AsQueryable().Where(e => e.FinancialAccountId == financialAccountId && e.Deleted == false);
                if (financialAccountTransactions.Any())
                {
                    financialAccountTransaction = financialAccountTransactions.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
                    return financialAccountTransaction;
                }
                else
                {
                    return financialAccountTransaction;
                }
            }

            public long SaveFinancialAccountTransaction(FinancialAccountTransactionDTO financialAccountTransactionDTO, string userId)
            {
                long financialAccountTransactionId = 0;

                if (financialAccountTransactionDTO.FinancialAccountTransactionId == 0)
                {

                    var financialAccountTransaction = new FinancialAccountTransaction()
                    {

                        Amount = financialAccountTransactionDTO.Amount,
                        StartAmount = financialAccountTransactionDTO.StartAmount,
                        Notes = financialAccountTransactionDTO.Notes,
                        Action = financialAccountTransactionDTO.Action,
                        Balance = financialAccountTransactionDTO.Balance,

                        BranchId = financialAccountTransactionDTO.BranchId,
                        
                        CreatedOn = DateTime.Now,
                        TimeStamp = DateTime.Now,
                        CreatedBy = userId,
                        Deleted = false,
                        FinancialAccountId = financialAccountTransactionDTO.FinancialAccountId,
                    };

                    this.UnitOfWork.Get<FinancialAccountTransaction>().AddNew(financialAccountTransaction);
                    this.UnitOfWork.SaveChanges();
                    financialAccountTransactionId = financialAccountTransaction.FinancialAccountTransactionId;
                    return financialAccountTransactionId;
                }

                else
                {
                    var result = this.UnitOfWork.Get<FinancialAccountTransaction>().AsQueryable()
                        .FirstOrDefault(e => e.FinancialAccountTransactionId == financialAccountTransactionDTO.FinancialAccountTransactionId);
                    if (result != null)
                    {
                        result.Action = financialAccountTransactionDTO.Action;
                        result.Amount = financialAccountTransactionDTO.Amount;
                        result.FinancialAccountId = financialAccountTransactionDTO.FinancialAccountId;
                        result.StartAmount = financialAccountTransactionDTO.StartAmount;
                        result.Balance = financialAccountTransactionDTO.Balance;
                        result.Notes = financialAccountTransactionDTO.Notes;
                        result.BranchId = financialAccountTransactionDTO.BranchId;
                        result.TimeStamp = DateTime.Now;
                        result.Deleted = financialAccountTransactionDTO.Deleted;
                        result.DeletedBy = financialAccountTransactionDTO.DeletedBy;
                        result.DeletedOn = financialAccountTransactionDTO.DeletedOn;


                        this.UnitOfWork.Get<FinancialAccountTransaction>().Update(result);
                        this.UnitOfWork.SaveChanges();
                    }
                    return financialAccountTransactionDTO.FinancialAccountTransactionId;
                }
            }

            public void MarkAsDeleted(long financialAccountId,long financialAccountTransactionId, string userId)
            {


                using (var dbContext = new MbaleEntities())
                {
                     dbContext.Mark_FinancialAccountTransaction_AsDeleted(financialAccountId, userId, financialAccountTransactionId);
                }

            }
        }
}
