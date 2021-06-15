using System;
using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class FinancialAccountTransactionService :IFinancialAccountTransactionService
    {
        

            ILog logger = log4net.LogManager.GetLogger(typeof(FinancialAccountTransactionService));
            private IFinancialAccountTransactionDataService _dataService;
            private IUserService _userService;


            public FinancialAccountTransactionService(IFinancialAccountTransactionDataService dataService, IUserService userService)
            {
                this._dataService = dataService;
                this._userService = userService;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="FinancialAccountTransactionId"></param>
            /// <returns></returns>
            public FinancialAccountTransaction GetFinancialAccountTransaction(long financialAccountTransactionId)
            {
                var result = this._dataService.GetFinancialAccountTransaction(financialAccountTransactionId);
                return MapEFToModel(result);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IEnumerable<FinancialAccountTransaction> GetAllFinancialAccountTransactions()
            {
                var results = this._dataService.GetAllFinancialAccountTransactions();
                return MapEFToModel(results);
            }

            public IEnumerable<FinancialAccountTransaction> GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount(long financialAccountId)
            {
                var results = this._dataService.GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount(financialAccountId);
                return MapEFToModel(results);
            }

        private double GetBalanceForLastFinancialAccountTransaction(long financialAccountId)
        {
            double balance = 0;

            var result = this._dataService.GetLatestFinancialAccountTransactionForAParticularFinancialAccount(financialAccountId);
            if (result.FinancialAccountId > 0)
            {
                balance = result.Balance;
            }

            return balance;


        }
        public long SaveFinancialAccountTransaction(FinancialAccountTransaction financialAccountTransaction, string userId)
            {
                long financialAccountTransactionId = 0;
                double startAmount = 0;
                double oldBalance = 0;
                double newBalance = 0;

            oldBalance = GetBalanceForLastFinancialAccountTransaction(Convert.ToInt64(financialAccountTransaction.FinancialAccountId));
            startAmount = oldBalance;


            if (financialAccountTransaction.Action == "-")
            {
                if (oldBalance < financialAccountTransaction.Amount)
                {
                    financialAccountTransactionId = -1;
                    return financialAccountTransactionId;
                }
                newBalance = oldBalance - financialAccountTransaction.Amount;
            }
            else
            {
                newBalance = oldBalance + financialAccountTransaction.Amount;
            }


            var financialAccountTransactionDTO = new DTO.FinancialAccountTransactionDTO()
                {
                    FinancialAccountTransactionId = financialAccountTransaction.FinancialAccountTransactionId,
                    Amount = financialAccountTransaction.Amount,
                    StartAmount = startAmount,
                    Notes = financialAccountTransaction.Notes,
                    Action = financialAccountTransaction.Action,
                    Balance = newBalance,

                    BranchId = financialAccountTransaction.BranchId,

                    CreatedOn = financialAccountTransaction.CreatedOn,
                    TimeStamp = financialAccountTransaction.TimeStamp,

                    Deleted = financialAccountTransaction.Deleted,
                    CreatedBy = financialAccountTransaction.CreatedBy,
                    FinancialAccountId = financialAccountTransaction.FinancialAccountId,
                    DeletedOn = financialAccountTransaction.DeletedOn,


                };

                 financialAccountTransactionId = this._dataService.SaveFinancialAccountTransaction(financialAccountTransactionDTO, userId);

                return financialAccountTransactionId;

            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="FinancialAccountTransactionId"></param>
            ///  /// <param name="FinancialAccountId"></param>
            /// <param name="userId"></param>
            public void MarkAsDeleted(long financialAccountId, long financialAccountTransactionId, string userId)
            {
                _dataService.MarkAsDeleted(financialAccountId, financialAccountTransactionId, userId);
            }


            #region Mapping Methods

            private IEnumerable<FinancialAccountTransaction> MapEFToModel(IEnumerable<EF.Models.FinancialAccountTransaction> data)
            {
                var list = new List<FinancialAccountTransaction>();
                foreach (var result in data)
                {
                    list.Add(MapEFToModel(result));
                }
                return list;
            }

            /// <summary>
            /// Maps FinancialAccountTransaction EF object to FinancialAccountTransaction Model Object and
            /// returns the FinancialAccountTransaction model object.
            /// </summary>
            /// <param name="result">EF FinancialAccountTransaction object to be mapped.</param>
            /// <returns>FinancialAccountTransaction Model Object.</returns>
            public FinancialAccountTransaction MapEFToModel(EF.Models.FinancialAccountTransaction data)
            {
                if (data != null)
                {
                    var financialAccountTransaction = new FinancialAccountTransaction()
                    {
                        FinancialAccountTransactionId = data.FinancialAccountTransactionId,
                        Amount = data.Amount,
                        StartAmount = data.StartAmount,
                        Notes = data.Notes,
                        Action = data.Action,
                        Balance = data.Balance,

                        BranchId = data.BranchId,

                        FinancialAccountId = data.FinancialAccountId,
                        CreatedOn = data.CreatedOn,
                        TimeStamp = data.TimeStamp,

                        Deleted = data.Deleted,
                        CreatedBy = _userService.GetUserFullName(data.AspNetUser),

                        BranchName = data.Branch != null ? data.Branch.Name : "",

                        FinancialAccountName = data.FinancialAccount != null ? data.FinancialAccount.Name : "",

                    };
                    return financialAccountTransaction;
                }
                return null;
            }



            #endregion

        
    }
}
