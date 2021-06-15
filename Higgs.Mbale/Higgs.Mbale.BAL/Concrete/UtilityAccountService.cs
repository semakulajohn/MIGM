using System;
using System.Collections.Generic;

using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;

using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class UtilityAccountService : IUtilityAccountService
    {
     ILog logger = log4net.LogManager.GetLogger(typeof(UtilityAccountService));
        private IUtilityAccountDataService _dataService;
        private IUserService _userService;
        private ITransactionSubTypeService _transactionSubTypeService;
        private ITransactionDataService _transactionDataService;



        public UtilityAccountService(IUtilityAccountDataService dataService, IUserService userService,ITransactionSubTypeService transactionSubTypeService,
            ITransactionDataService transactionDataService)
            
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionSubTypeService = transactionSubTypeService;
             this._transactionDataService = transactionDataService;
            
        }


        public UtilityAccount GetUtilityAccount(long utilityAccountId)
        {
            var result = this._dataService.GetUtilityAccount(utilityAccountId);
            return MapEFToModel(result);
        }


        public IEnumerable<UtilityAccount> GetAllUtilityAccountForAParticularBranch(long branchId)
        {

            var results = this._dataService.GetAllUtilityAccountForAParticularBranch(branchId);
                return MapEFToModel(results);
            
            
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UtilityAccount> GetAllUtilityAccounts()
        {
            var results = this._dataService.GetAllUtilityAccounts();
            return MapEFToModel(results);
        }

        public IEnumerable<UtilityAccount> GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory(long branchId, long categoryId)
        {
            var results = this._dataService.GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory(branchId,categoryId);
            return MapEFToModel(results);
        }
        public IEnumerable<UtilityCategory> GetAllUtilityCategories()
        {
            var results = this._dataService.GetAllUtilityCategories();
            return MapEFToModel(results);
        }

        public UtilityCategory GetUtilityCategory(long utilityCategoryId)
        {
            var result = this._dataService.GetUtilityCategory(utilityCategoryId);
            return MapEFToModel(result);
        }


        public double GetBalanceForLastUtilityAccount(long branchId,long categoryId)
        {
            double balance = 0;

            var result = this._dataService.GetLatestUtilityAccountForAParticularBranchAndCategory(branchId,categoryId);
                if (result != null)
                {
                    balance = result.Balance;
                }
               
                return balance;
            
           
        }

        public double GetBalanceForUtilityAccountForABranch(long branchId,long categoryId)
        {
            double balance = 0;

            var results = this._dataService.GetAllUtilityAccountForAParticularBranch(branchId);
            foreach (var result in results)
            {
                balance = Convert.ToDouble(result.Amount) + balance;
            }
            

            return balance;


        }

        public double GetBalanceForLastUtilityAccountForAParticularDate(long branchId, long categoryId,DateTime dateTime)
        {
            double balance = 0;

            var result = this._dataService.GetLatestUtilityAccountForAParticularBranchAndCategoryForAParticularDate(branchId, categoryId,dateTime);
            if (result != null)
            {
                balance = result.Balance;
            }

            return balance;


        }


        public long SaveUtilityAccount(UtilityAccount utilityAccount, string userId)
        {
            long  utilityAccountId = 0;
            double startAmount =0;
            double OldBalance = 0;
            double NewBalance = 0;

                OldBalance = GetBalanceForLastUtilityAccount(Convert.ToInt64(utilityAccount.BranchId),Convert.ToInt64(utilityAccount.UtilityCategoryId));
               startAmount = OldBalance;


               if (utilityAccount.Action == "-")
                {
                    //if (OldBalance < utilityAccount.Amount)
                    //{
                    //    utilityAccountId = -1;
                    //    return utilityAccountId;
                    //}
                    NewBalance = OldBalance - Convert.ToDouble(utilityAccount.Amount);
                }
                else
                {
                    NewBalance = OldBalance + Convert.ToDouble(utilityAccount.Amount);
                }

                var utilityAccountDTO = new DTO.UtilityAccountDTO()
                {
                   
                    Amount = utilityAccount.Amount,
                    StartAmount = startAmount,
                    Balance = NewBalance,
                    UtilityAccountId = utilityAccount.UtilityAccountId,
                    Action = utilityAccount.Action,
                    BranchId = utilityAccount.BranchId,
                    InvoiceNumber = utilityAccount.InvoiceNumber,
                    Description = utilityAccount.Description,
                    TimeStamp = utilityAccount.TimeStamp,
                    Deleted = utilityAccount.Deleted,
                    UtilityCategoryId = utilityAccount.UtilityCategoryId,
                    CreatedBy = utilityAccount.CreatedBy,
                    CreatedOn = utilityAccount.CreatedOn

                };

                 utilityAccountId = this._dataService.SaveUtilityAccount(utilityAccountDTO, userId);
         
              
                
            return utilityAccountId;
                 }

      
        public void MarkAsDeleted(long utilityAccountId, string userId)
        {
            _dataService.MarkAsDeleted(utilityAccountId, userId);
        }

      
        #region Mapping Methods

        public IEnumerable<UtilityAccount> MapEFToModel(IEnumerable<EF.Models.UtilityAccount> data)
        {
            var list = new List<UtilityAccount>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public UtilityAccount MapEFToModel(EF.Models.UtilityAccount data)
        {
            
            if (data != null)
            {

                var utilityAccount = new UtilityAccount()
                {
                    UtilityCategoryName = data.UtilityCategory != null ? data.UtilityCategory.Name : "",
                    Action = data.Action,
                    StartAmount = data.StartAmount,
                    Balance = data.Balance,
                    UtilityAccountId = data.UtilityAccountId,
                    BranchId = data.BranchId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                   Amount = data.Amount,
                   InvoiceNumber = data.InvoiceNumber,
                   Description = data.Description,
                     CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),

                };
                return utilityAccount;
            }
            return null;
        }

        public IEnumerable<UtilityCategory> MapEFToModel(IEnumerable<EF.Models.UtilityCategory> data)
        {
            var list = new List<UtilityCategory>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public UtilityCategory MapEFToModel(EF.Models.UtilityCategory data)
        {

            if (data != null)
            {

                var utilityCategory = new UtilityCategory()
                {

                    Name = data.Name,
                    UtilityCategoryId = data.UtilityCategoryId,
                    TimeStamp = data.TimeStamp,
                   

                };
                return utilityCategory;
            }
            return null;
        }

     
       #endregion

    }
}
