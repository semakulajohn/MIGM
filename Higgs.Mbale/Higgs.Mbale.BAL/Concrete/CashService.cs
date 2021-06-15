using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Helpers;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class CashService : ICashService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(CashService));
        private ICashDataService _dataService;
        private IUserService _userService;
        private ITransactionSubTypeService _transactionSubTypeService;
        private IDocumentService _documentService;
        private ITransactionDataService _transactionDataService;
       


        public CashService(ICashDataService dataService, IUserService userService,IDocumentService documentService,
            ITransactionSubTypeService transactionSubTypeService,
            ITransactionDataService transactionDataService)
            
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionSubTypeService = transactionSubTypeService;
             this._transactionDataService = transactionDataService;
             this._documentService = documentService;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CashId"></param>
        /// <returns></returns>
        public Cash GetCash(long cashId)
        {
            var result = this._dataService.GetCash(cashId);
            return MapEFToModel(result);
        }

      public  IEnumerable<Cash> GetThirtyLatestCashForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetThirtyLatestCashForAParticularBranch(branchId);
            return MapEFToModel(results);
        }
        public IEnumerable<Cash> GetAllCashForAParticularBranch(long branchId)
        {
           
                var results = this._dataService.GetAllCashForAParticularBranch(branchId);
                return MapEFToModel(results);
            
            
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cash> GetAllCash()
        {
            var results = this._dataService.GetAllCash();
            return MapEFToModel(results);
        }

        private double GetBalanceForLastCash(long branchId)
        {
            double balance = 0;
           
                var result = this._dataService.GetLatestCashForAParticularBranch(branchId);
                if (result.BranchId > 0)
                {
                    balance = result.Balance;
                }
               
                return balance;
            
           
        }

        public long CheckIfBranchHasEnoughCash(long branchId,double amount,string action)
        {
            long cashId = 0;
            double OldBalance = 0;
           
                OldBalance = GetBalanceForLastCash(branchId);
               
                if (action == "-")
                {
                    if (OldBalance < amount)
                    {
                        cashId = -1;
                        return cashId;
                    }
                    cashId = 1;
                    return cashId;
                }
                else
                {
                    cashId = 1;
                    return cashId;
                }

           
        }

            public long SaveCash(Cash cash, string userId)
        {
            long cashId = 0;
            double startAmount =0;
            double OldBalance = 0;
            double NewBalance = 0;

            if (cash.BranchId != null)
            {

             OldBalance = GetBalanceForLastCash(Convert.ToInt64(cash.BranchId));
               startAmount = OldBalance;
           

                if (cash.Action == "-")
                {
                    if (OldBalance < cash.Amount)
                    {
                        cashId = -1;
                        return cashId;
                    }
                    NewBalance = OldBalance - cash.Amount;
                }
                else
                {
                    NewBalance = OldBalance + cash.Amount;
                }

                var cashDTO = new DTO.CashDTO()
                {
                   
                    Amount = cash.Amount,
                    StartAmount = startAmount,
                    Balance = NewBalance,
                    Notes = cash.Notes,
                    CashId = cash.CashId,
                    CustomerId = cash.CustomerId,
                    Action = cash.Action,
                    BranchId = cash.BranchId,
                    TransactionSubTypeId = cash.TransactionSubTypeId,
                    SectorId = cash.SectorId,
                    Deleted = cash.Deleted,
                    CreatedBy = cash.CreatedBy,
                    CreatedOn = cash.CreatedOn,
                    RequistionCategoryId = cash.RequistionCategoryId,

                };

                 cashId = this._dataService.SaveCash(cashDTO, userId);
            }
              
                 SaveApplicationCash(cash,userId);

               //var document =new  Document()
               // {
                   
               //     Name = document.Name,
               //     UserId = document.UserId,
               //     DocumentCategoryId = document.DocumentCategoryId,
               //     Amount = document.Amount,
               //     BranchId = document.BranchId,
               //     Description = document.Description,
               //     Quantity = document.Quantity,
               //     DocumentNumber = documentNumber,
                    
               //};
               //var documentId = _documentService.SaveDocument(document, userId);

                
            return cashId;
                 }

        public void SaveApplicationCash(Cash cash, string userId)
        {
            var application = _dataService.GetApplicationDetails();
            double startAmount = 0;
            double OldBalance = 0;
            double newBalance = 0;

            if (application != null)
            {
                OldBalance = application.TotalCash;
                startAmount = OldBalance;

                if (cash.Action == "-")
                {
                    newBalance = OldBalance - cash.Amount;
                }
                else
                {
                    newBalance = OldBalance + cash.Amount;
                }

                var applicatinDTO = new DTO.ApplicationDTO()
                {
                    ApplicationId = application.ApplicationId,
                    TotalCash = newBalance,
                    Name = application.Name,
                    TimeStamp = application.TimeStamp,

                };

                this._dataService.UpdateApplicationCash(applicatinDTO);

                long transactionTypeId = 0;

                var transactionSubType = _transactionSubTypeService.GetTransactionSubType(cash.TransactionSubTypeId);

                if (transactionSubType != null)
                {
                    transactionTypeId = transactionSubType.TransactionTypeId;

                    var transaction = new TransactionDTO()
                    {
                        BranchId = Convert.ToInt64(cash.BranchId),
                        SectorId = cash.SectorId,
                        Amount = cash.Amount,
                        TransactionSubTypeId = cash.TransactionSubTypeId,
                        TransactionTypeId = transactionTypeId,
                        CreatedOn = DateTime.Now,
                        TimeStamp = DateTime.Now,
                        CreatedBy = userId,
                        Deleted = false,

                    };
                    var transactionId = _transactionDataService.SaveTransaction(transaction, userId);


                }

            }

                

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cashId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long cashId, string userId,long branchId)
        {
            _dataService.MarkAsDeleted(cashId, userId,branchId);
        }

      
        #region Mapping Methods

        public IEnumerable<Cash> MapEFToModel(IEnumerable<EF.Models.Cash> data)
        {
            var list = new List<Cash>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public Cash MapEFToModel(EF.Models.Cash data)
        {
            var accountName = string.Empty;

            if (data != null)
            {

                var cash = new Cash()
                {

                    Action = data.Action,
                    StartAmount = data.StartAmount,
                    Balance = data.Balance,
                    Amount = data.Amount,
                    Notes = data.Notes,
                    CashId = data.CashId,
                    CustomerId = data.CustomerId,
                    BranchId = data.BranchId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorId = data.SectorId,
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    TransactionSubTypeId = data.TransactionSubTypeId,
                    TransactionSubTypeName = data.TransactionSubType != null ? data.TransactionSubType.Name : "",
                    RequistionCategoryId = data.RequistionCategoryId,
                    RequistionCategoryName = data.RequistionCategory != null ? data.RequistionCategory.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    CustomerName = _userService.GetUserFullName(data.AspNetUser11),



                };
                return cash;
            }
            return null;
        }

     
       #endregion

       
    }
}
