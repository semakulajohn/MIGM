
using System.Collections.Generic;

using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;

using log4net;


namespace Higgs.Mbale.BAL.Concrete
{
public    class BankTransactionService : IBankTransactionService
    {
     ILog logger = log4net.LogManager.GetLogger(typeof(BankTransactionService));
        private IBankTransactionDataService _dataService;
        private IUserService _userService;
        

        public BankTransactionService(IBankTransactionDataService dataService,IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BankTransactionId"></param>
        /// <returns></returns>
        public BankTransaction GetBankTransaction(long bankTransactionId)
        {
            var result = this._dataService.GetBankTransaction(bankTransactionId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BankTransaction> GetAllBankTransactions()
        {
            var results = this._dataService.GetAllBankTransactions();
            return MapEFToModel(results);
        }

        public IEnumerable<BankTransaction> GetLatestTwentyBankTransactionsForAParticularBranchAndBank(long branchId, long bankId)
        {
            var results = this._dataService.GetLatestTwentyBankTransactionsForAParticularBranchAndBank(branchId,bankId);
            return MapEFToModel(results);
        }
       
        public long SaveBankTransaction(BankTransaction bankTransaction, string userId)
        {
            var bankTransactionDTO = new DTO.BankTransactionDTO()
            {
                BankTransactionId = bankTransaction.BankTransactionId,
                Amount = bankTransaction.Amount,
                StartAmount = bankTransaction.StartAmount,
                Notes = bankTransaction.Notes,
                Action = bankTransaction.Action,
                Balance = bankTransaction.Balance,

                TransactionSubTypeId = bankTransaction.TransactionSubTypeId,
                BranchId = bankTransaction.BranchId,
                SectorId = bankTransaction.SectorId,
                CreatedOn = bankTransaction.CreatedOn,
                TimeStamp = bankTransaction.TimeStamp,
              
                Deleted = bankTransaction.Deleted,
                CreatedBy = bankTransaction.CreatedBy,
                BankId = bankTransaction.BankId,
                DeletedOn = bankTransaction.DeletedOn,
                
              
            };

           var bankTransactionId = this._dataService.SaveBankTransaction(bankTransactionDTO, userId);

           return bankTransactionId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BankTransactionId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long bankTransactionId, string userId)
        {
            _dataService.MarkAsDeleted(bankTransactionId, userId);
        }

      
        #region Mapping Methods

        private IEnumerable<BankTransaction> MapEFToModel(IEnumerable<EF.Models.BankTransaction> data)
        {
            var list = new List<BankTransaction>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps BankTransaction EF object to BankTransaction Model Object and
        /// returns the BankTransaction model object.
        /// </summary>
        /// <param name="result">EF BankTransaction object to be mapped.</param>
        /// <returns>BankTransaction Model Object.</returns>
        public BankTransaction MapEFToModel(EF.Models.BankTransaction data)
        {
            if (data != null)
            {
                var BankTransaction = new BankTransaction()
                {
                    BankTransactionId = data.BankTransactionId,
                    Amount = data.Amount,
                    StartAmount = data.StartAmount,
                    Notes = data.Notes,
                    Action = data.Action,
                    Balance = data.Balance,

                    TransactionSubTypeId = data.TransactionSubTypeId,
                    BranchId = data.BranchId,
                    SectorId = data.SectorId,
                  
                    BankId = data.BankId,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                   
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),

                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    BankName = data.Bank != null? data.Bank.Name : "",

                };
                return BankTransaction;
            }
            return null;
        }



       #endregion
    }
}
