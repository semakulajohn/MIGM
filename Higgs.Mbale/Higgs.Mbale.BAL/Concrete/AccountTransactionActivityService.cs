using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;
using System.Configuration;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class AccountTransactionActivityService : IAccountTransactionActivityService
    {

      private long receiptId = Convert.ToInt64(ConfigurationManager.AppSettings["Receipt"]);
     
        ILog logger = log4net.LogManager.GetLogger(typeof(AccountTransactionActivityService));
        private IAccountTransactionActivityDataService _dataService;
        private IUserService _userService;
        private ITransactionSubTypeService _transactionSubTypeService;
        private ICreditorDataService _creditorDataService;
        private IDebtorDataService _debtorDataService;
        private ITransactionDataService _transactionDataService;
      private IDocumentService _documentService;
      
       


        public AccountTransactionActivityService(IAccountTransactionActivityDataService dataService, IUserService userService,
            ITransactionSubTypeService transactionSubTypeService,IDebtorDataService debtorDataService,ICreditorDataService creditorDataService,
            ITransactionDataService transactionDataService,IDocumentService documentService
           
            )
            
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionSubTypeService = transactionSubTypeService;
            this._creditorDataService = creditorDataService;
            this._debtorDataService = debtorDataService;
            this._transactionDataService = transactionDataService;
            this._documentService = documentService;
           
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountTransactionActivityId"></param>
        /// <returns></returns>
        public AccountTransactionActivity GetAccountTransactionActivity(long accountTransactionActivityId)
        {
            var result = this._dataService.GetAccountTransactionActivity(accountTransactionActivityId);
            return MapEFToModel(result);
        }

     private bool checkIfUserIsAspNetUser(string accountId)
     {
         var isAspNetUser = false;
         var user = _userService.GetAspNetUser(accountId);
         if (user != null)
         {
             isAspNetUser = true;
             
         }
         return isAspNetUser;
     }
        public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularAccount(string accountId)
        {
            
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var results = this._dataService.GetAllAccountTransactionActivitiesForAParticularAspNetUser(accountId);
                return MapEFToModel(results);
            }
            else
            {
                var casualWorkerId = Convert.ToInt64(accountId);
                var results = this._dataService.GetAllAccountTransactionActivitiesForAParticularCasualWorker(casualWorkerId);
                return MapEFToModel(results);
            }
            
            
        }

        public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularSupply(long supplyId)
        {
            var results = this._dataService.GetAllAccountTransactionActivitiesForAParticularSupply(supplyId);
            return MapEFToModel(results);
        }

        public bool checkIfSupplyRelatesToAnyAccountTransaction(long supplyId)
        {
            bool transactionExists = false;
            var results = GetAllAccountTransactionActivitiesForAParticularSupply(supplyId);
            if (results.Any())
            {
                transactionExists = true;
            }
            return transactionExists;
        }

        public IEnumerable<AccountTransactionActivity> GetAllAdvancedPaymentsForAParticularAspNetUser(string accountId,long transactionSubTypeId)
        {
            
                var results = this._dataService.GetAllAdvancedPaymentsForAParticularAspNetUser(accountId,transactionSubTypeId);
                return MapEFToModel(results);
            

        }
        public IEnumerable<AccountTransactionActivity> GetLatestFortyAccountTransactionActivitiesForAParticularAccount(string accountId)
        {
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var results = this._dataService.GetLatestFortyAccountTransactionActivitiesForAParticularAspNetUser(accountId);
                return MapEFToModel(results);
            }
            else
            {
                var casualWorkerId = Convert.ToInt64(accountId);
                var results = this._dataService.GetLatestFortyAccountTransactionActivitiesForAParticularCasualWorker(casualWorkerId);
                return MapEFToModel(results);
            }
            

        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivities()
        {
            var results = this._dataService.GetAllAccountTransactionActivities();
            return MapEFToModel(results);
        }

        private double GetBalanceForLastAccountAccountTransactionActivity(string accountId)
        {
            double balance = 0;
          
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var result = this._dataService.GetLatestAccountTransactionActivityForAParticularAspNetUser(accountId);
                if (result.AccountTransactionActivityId > 0)
                {
                    balance = result.Balance;
                }
               
                return balance;
            }
            else
            {
                var casualWorkerId = Convert.ToInt64(accountId);
                var result = this._dataService.GetLatestAccountTransactionActivitiesForAParticularCasualWorker(casualWorkerId);
                if(result.AccountTransactionActivityId > 0){
                    balance = result.Balance;
                }
                return balance;
            }
        }

        public double GetBalanceForLastAccountAccountTransactionActivityForSupplier(string accountId)
        {
            double balance = 0;
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var result = this._dataService.GetLatestAccountTransactionActivityForAParticularAspNetUser(accountId);
                if (result.AccountTransactionActivityId > 0)
                {
                    balance = result.Balance;
                }

                return balance;
            }
            return balance;
        }

        public double GetBalanceForLastAccountAccountTransactionActivityForSupplierForAParticularDate(string accountId,DateTime dateTime)
        {
            double balance = 0;
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var result = this._dataService.GetLatestAccountTransactionActivityForAParticularAspNetUserForAParticularDate(accountId,dateTime);
                if (result.AccountTransactionActivityId > 0)
                {
                    balance = result.Balance;
                }

                return balance;
            }
            return balance;
        }

        public double GetBalanceForLastAccountAccountTransactionActivityForCasualWorker(long accountId)
        {
                double balance = 0;
           
                var result = this._dataService.GetLatestAccountTransactionActivitiesForAParticularCasualWorker(accountId);
                if (result.AccountTransactionActivityId > 0)
                {
                    balance = result.Balance;
                }

                return balance;
        }

       
        public long SaveAccountTransactionActivity(AccountTransactionActivity accountTransactionActivity, string userId)
        {
            long accountTransactionActivityId = 0;
            double startAmount =0;
            double OldBalance = 0;
            double NewBalance = 0;
            
                if(accountTransactionActivity.AspNetUserId != null)
                {
                    OldBalance = GetBalanceForLastAccountAccountTransactionActivity(accountTransactionActivity.AspNetUserId);
                    startAmount = OldBalance;
                }
                else{
                    
                    OldBalance = GetBalanceForLastAccountAccountTransactionActivity(Convert.ToString(accountTransactionActivity.CasualWorkerId));
                    startAmount = OldBalance;
                }

                if (accountTransactionActivity.Action == "-")
                {
                    NewBalance = OldBalance - accountTransactionActivity.Amount;
                }
                else
                {
                    NewBalance = OldBalance + accountTransactionActivity.Amount;
                }

                var accountTransactionActivityDTO = new DTO.AccountTransactionActivityDTO()
                {
                    AspNetUserId = accountTransactionActivity.AspNetUserId,
                    CasualWorkerId = accountTransactionActivity.CasualWorkerId,
                    Amount = accountTransactionActivity.Amount,
                    StartAmount = startAmount,
                    Balance = NewBalance,
                    Notes = accountTransactionActivity.Notes,
                    AccountTransactionActivityId = accountTransactionActivity.AccountTransactionActivityId,
                    Action = accountTransactionActivity.Action,
                    BranchId = accountTransactionActivity.BranchId,
                    TransactionSubTypeId = accountTransactionActivity.TransactionSubTypeId,
                    SectorId = accountTransactionActivity.SectorId,
                    Deleted = accountTransactionActivity.Deleted,
                    CreatedBy = accountTransactionActivity.CreatedBy,
                    CreatedOn = accountTransactionActivity.CreatedOn,
                    SupplyId = accountTransactionActivity.SupplyId,
                    WeightNote = accountTransactionActivity.WeightNote,
                    Bags = accountTransactionActivity.Bags,
                    Price = accountTransactionActivity.Price,
                    Quantity = accountTransactionActivity.Quantity,

                };

                 accountTransactionActivityId = this._dataService.SaveAccountTransactionActivity(accountTransactionActivityDTO, userId);

                 long transactionTypeId = 0;
                 List<string> transactionSubTypeNames = new List<string>();
                 var transactionSubType = _transactionSubTypeService.GetTransactionSubType(accountTransactionActivity.TransactionSubTypeId);

                 if (transactionSubType != null)
                 {
                     transactionTypeId = transactionSubType.TransactionTypeId;
                    
                     if (transactionSubType.Name == "Deposit")
                     {
                         #region generate documents
                      
                             //generate receipt
                            
                             var username = string.Empty;
                             var user = _userService.GetAspNetUser(accountTransactionActivityDTO.AspNetUserId);
                             if (user != null)
                             {
                                 username = user.FirstName + " " + user.LastName;
                             }
                             var document = new Document()
                             {
                                 DocumentId = 0,

                                 UserId = username,
                                 DocumentCategoryId = receiptId,
                                 Amount = accountTransactionActivityDTO.Amount,
                                 BranchId = Convert.ToInt64(accountTransactionActivityDTO.BranchId),
                                 ItemId = accountTransactionActivityId,
                                 Description =accountTransactionActivityDTO.Notes ,



                             };

                             var documentId = _documentService.SaveDocument(document, userId);
                         
                       
                         }
                         #endregion


                     }
                   
                     var transaction = new TransactionDTO()
                     {
                         BranchId = Convert.ToInt64(accountTransactionActivity.BranchId),
                         SectorId = accountTransactionActivity.SectorId,
                         Amount = accountTransactionActivity.Amount,
                         TransactionSubTypeId = accountTransactionActivity.TransactionSubTypeId,
                         TransactionTypeId = transactionTypeId,
                         CreatedOn = DateTime.Now,
                         TimeStamp = DateTime.Now,
                         CreatedBy = userId,
                         Deleted = false,

                     };
                     var transactionId = _transactionDataService.SaveTransaction(transaction, userId);
                 //}
            return accountTransactionActivityId;
                 }

        public IEnumerable<PaymentMode> GetAllPaymentModes()
        {
            var results = this._dataService.GetAllPaymentModes();
            return MapEFToModel(results);
        }


        public PaymentMode GetPaymentMode(long paymentModeId)
        {
            var result = this._dataService.GetPaymentMode(paymentModeId);
            return MapEFToModel(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountTransactionActivityId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long accountTransactionActivityId, string userId)
        {
            _dataService.MarkAsDeleted(accountTransactionActivityId, userId);
        }

      
        #region Mapping Methods

        public IEnumerable<AccountTransactionActivity> MapEFToModel(IEnumerable<EF.Models.AccountTransactionActivity> data)
        {
            var list = new List<AccountTransactionActivity>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public AccountTransactionActivity MapEFToModel(EF.Models.AccountTransactionActivity data)
        {
            var accountName = string.Empty;
            long documentId = 0;
            if (data != null)
            {
                var document = _documentService.GetDocumentForAParticularItem(data.AccountTransactionActivityId);
                if (document != null)
                {
                    documentId = document.DocumentId;
                }

                if (data.AspNetUser != null)
                {

                    accountName = _userService.GetUserFullName(data.AspNetUser);

                }
                else
                {
                    accountName = data.CasualWorker.FirstName + ' ' + data.CasualWorker.LastName;

                }

                var accountTransactionActivity = new AccountTransactionActivity()
                {
                    AspNetUserId = data.AspNetUserId,
                    CasualWorkerId = data.CasualWorkerId,
                    Action = data.Action,
                    StartAmount = data.StartAmount,
                    Balance = data.Balance,
                    Amount = data.Amount,
                    SupplyId = data.SupplyId,
                    Notes = data.Notes,
                    AccountTransactionActivityId = data.AccountTransactionActivityId,
                    BranchId = data.BranchId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorId = data.SectorId,
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    TransactionSubTypeId = data.TransactionSubTypeId,
                    TransactionSubTypeName = data.TransactionSubType != null ? data.TransactionSubType.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser1),
                    CreatedById = data.CreatedBy,
                    AccountName = accountName,
                    WeightNote = data.WeightNote,
                    Price = data.Price,
                    Quantity = data.Quantity,
                    Bags = data.Bags,
                    DocumentId = documentId,


                };
                return accountTransactionActivity;
            }
            return null;
        }

     
       #endregion

        #region paymentModes
        private IEnumerable<PaymentMode> MapEFToModel(IEnumerable<EF.Models.PaymentMode> data)
        {
            var list = new List<PaymentMode>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        private PaymentMode MapEFToModel(EF.Models.PaymentMode data)
        {
            if (data != null)
            {
                var paymentMode = new PaymentMode()
                {
                    Name = data.Name,
                    PaymentModeId = data.PaymentModeId,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),

                };
                return paymentMode;
            }
            return null;
        }
        #endregion
    }
}
