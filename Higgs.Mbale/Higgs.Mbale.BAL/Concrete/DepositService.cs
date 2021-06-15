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
 public   class DepositService : IDepositService
    {
       

        ILog logger = log4net.LogManager.GetLogger(typeof(DepositService));
        private IDepositDataService _dataService;
        private IUserService _userService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private IDocumentService _documentService;




        public DepositService(
            IDepositDataService dataService, IUserService userService,
            ITransactionSubTypeService transactionSubTypeService, 
             IDocumentService documentService,
            IAccountTransactionActivityService accountTransactionActivityService
            
            )

        {
            this._dataService = dataService;
            this._userService = userService;
         
            this._documentService = documentService;
            this._accountTransactionActivityService = accountTransactionActivityService;

        }

        
        public Deposit GetDeposit(long depositId)
        {
            var result = this._dataService.GetDeposit(depositId);
            return MapEFToModel(result);
        }

       

      public  IEnumerable<Deposit> GetAllDepositsForAParticularAccount(string accountId)
        {
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var results = this._dataService.GetAllDepositsForAParticularAspNetUser(accountId);
                return MapEFToModel(results);
            }
            else
            {
                var casualWorkerId = Convert.ToInt64(accountId);
                var results = this._dataService.GetAllDepositsForAParticularCasualWorker(casualWorkerId);
                return MapEFToModel(results);
            }
        }

       public  IEnumerable<Deposit> GetAllUnApprovedDepositsForAParticularAccount(string accountId)
        {
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var results = this._dataService.GetAllUnApprovedDepositsForAParticularAspNetUser(accountId);
                return MapEFToModel(results);
            }
            else
            {
                var casualWorkerId = Convert.ToInt64(accountId);
                var results = this._dataService.GetAllUnpprovedDepositsForAParticularCasualWorker(casualWorkerId);
                return MapEFToModel(results);
            }
        }

       public IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForAParticularAccount(string accountId)
        {
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var results = this._dataService.GetLatestTwentyApprovedDepositsForAParticularAspNetUser(accountId);
                return MapEFToModel(results);
            }
            else
            {
                var casualWorkerId = Convert.ToInt64(accountId);
                var results = this._dataService.GetAllLatestTwentyApprovedDepositsForAParticularCasualWorker(casualWorkerId);
                return MapEFToModel(results);
            }
        }

       public IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForAParticularAccount(string accountId)
        {
            var isAspNetUser = checkIfUserIsAspNetUser(accountId);
            if (isAspNetUser)
            {
                var results = this._dataService.GetLatestTwentyRejectedDepositsForAParticularAspNetUser(accountId);
                return MapEFToModel(results);
            }
            else
            {
                var casualWorkerId = Convert.ToInt64(accountId);
                var results = this._dataService.GetAllLatestTwentyRejectedDepositsForAParticularCasualWorker(casualWorkerId);
                return MapEFToModel(results);
            }
        }

        public long ApproveOrRejectDeposit(Deposit deposit,bool status,string userId)
        {
            // var deposit = GetDeposit(depositId);
            long depositId = 0;
            long  accountActivityId = 0;
            if (deposit.CreatedBy == userId)
            {
                depositId = -1;
                return depositId;
            }
            else
            {
                if (status)
                {
                    var depositObject = new Deposit()
                    {
                        AspNetUserId = deposit.AspNetUserId,
                        CasualWorkerId = deposit.CasualWorkerId,
                        Amount = deposit.Amount,
                        StartAmount = deposit.StartAmount,
                        Balance = deposit.Balance,
                        Notes = deposit.Notes,
                        DepositId = deposit.DepositId,
                        Action = deposit.Action,
                        BranchId = deposit.BranchId,
                        TransactionSubTypeId = deposit.TransactionSubTypeId,
                        SectorId = deposit.SectorId,
                        Deleted = deposit.Deleted,
                        CreatedBy = deposit.CreatedBy,
                        CreatedOn = deposit.CreatedOn,
                        SupplyId = deposit.SupplyId,
                        WeightNote = deposit.WeightNote,
                        Bags = deposit.Bags,
                        Price = deposit.Price,
                        Quantity = deposit.Quantity,
                        Approved = status,

                    };

                    depositId = SaveDeposit(depositObject, userId);
                    if (depositId != 0)
                    {
                        var accountTransactionActivityObject = new AccountTransactionActivity()
                        {
                            AspNetUserId = deposit.AspNetUserId,
                            CasualWorkerId = deposit.CasualWorkerId,
                            Amount = deposit.Amount,
                            StartAmount = deposit.StartAmount,
                            Balance = deposit.Balance,
                            Notes = deposit.Notes,
                            AccountTransactionActivityId = 0,
                            Action = deposit.Action,
                            BranchId = deposit.BranchId,
                            TransactionSubTypeId = deposit.TransactionSubTypeId,
                            SectorId = deposit.SectorId,
                            Deleted = deposit.Deleted,
                            CreatedBy = deposit.CreatedBy,
                            CreatedOn = deposit.CreatedOn,
                            SupplyId = deposit.SupplyId,
                            WeightNote = deposit.WeightNote,
                            Bags = deposit.Bags,
                            Price = deposit.Price,
                            Quantity = deposit.Quantity,

                        };

                        accountActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivityObject, userId);
                    }
                    return accountActivityId;

                }
                else
                {
                    var depositObject = new Deposit()
                    {
                        AspNetUserId = deposit.AspNetUserId,
                        CasualWorkerId = deposit.CasualWorkerId,
                        Amount = deposit.Amount,
                        StartAmount = deposit.StartAmount,
                        Balance = deposit.Balance,
                        Notes = deposit.Notes,
                        DepositId = deposit.DepositId,
                        Action = deposit.Action,
                        BranchId = deposit.BranchId,
                        TransactionSubTypeId = deposit.TransactionSubTypeId,
                        SectorId = deposit.SectorId,
                        Deleted = deposit.Deleted,
                        CreatedBy = deposit.CreatedBy,
                        CreatedOn = deposit.CreatedOn,
                        SupplyId = deposit.SupplyId,
                        WeightNote = deposit.WeightNote,
                        Bags = deposit.Bags,
                        Price = deposit.Price,
                        Quantity = deposit.Quantity,
                        Approved = status,

                    };

                    depositId = SaveDeposit(depositObject, userId);
                    return depositId;
                }
               
            }


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
      
      
        public IEnumerable<Deposit> GetAllDeposits()
        {
            var results = this._dataService.GetAllDeposits();
            return MapEFToModel(results);
        }

        public IEnumerable<Deposit> GetLatestTwentyRejectedDeposits()
        {

            var results = this._dataService.GetLatestTwentyRejectedDeposits();
            return MapEFToModel(results);
        }

        public IEnumerable<Deposit> GetLatestTwentyApprovedDeposits()
        {

            var results = this._dataService.GetLatestTwentyApprovedDeposits();
            return MapEFToModel(results);
        }
        public IEnumerable<Deposit> GetLatestTwentyUnApprovedDeposits()
        {

            var results = this._dataService.GetLatestTwentyUnApprovedDeposits();
            return MapEFToModel(results);
        }


        public IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForABranch(long branchId)
        {
            var results = this._dataService.GetLatestTwentyRejectedDepositsForABranch(branchId);
            return MapEFToModel(results);
        }

        public IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForABranch(long branchId)
        {
            var results = this._dataService.GetLatestTwentyApprovedDepositsForABranch(branchId);
            return MapEFToModel(results);
        }

        public IEnumerable<Deposit> GetLatestTwentyUnApprovedDepositsForABranch(long branchId)
        {
            var results = this._dataService.GetLatestTwentyUnApprovedDepositsForABranch(branchId);
            return MapEFToModel(results);
        }
        public long SaveDeposit(Deposit deposit, string userId)
        {
            long depositId = 0;
           

         var depositDTO = new DTO.DepositDTO()
            {
                AspNetUserId = deposit.AspNetUserId,
                CasualWorkerId = deposit.CasualWorkerId,
                Amount = deposit.Amount,
                StartAmount = deposit.StartAmount,
                Balance = deposit.Balance,
                Notes = deposit.Notes,
                DepositId = deposit.DepositId,
                Action = deposit.Action,
                BranchId = deposit.BranchId,
                TransactionSubTypeId = deposit.TransactionSubTypeId,
                SectorId = deposit.SectorId,
                Deleted = deposit.Deleted,
                CreatedBy = deposit.CreatedBy,
                CreatedOn = deposit.CreatedOn,
                SupplyId = deposit.SupplyId,
                WeightNote = deposit.WeightNote,
                Bags = deposit.Bags,
                Price = deposit.Price,
                Quantity = deposit.Quantity,
                Approved = deposit.Approved,

            };

            depositId = this._dataService.SaveDeposit(depositDTO, userId);

          
          
            return depositId;
        }

      
        public void MarkAsDeleted(long depositId, string userId)
        {
            _dataService.MarkAsDeleted(depositId, userId);
        }


        #region Mapping Methods

        public IEnumerable<Deposit> MapEFToModel(IEnumerable<EF.Models.Deposit> data)
        {
            var list = new List<Deposit>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public Deposit MapEFToModel(EF.Models.Deposit data)
        {
            var accountName = string.Empty;
            long documentId = 0;
            if (data != null)
            {
                var document = _documentService.GetDocumentForAParticularItem(data.DepositId);
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

                var deposit = new Deposit()
                {
                    AspNetUserId = data.AspNetUserId,
                    CasualWorkerId = data.CasualWorkerId,
                    Action = data.Action,
                    StartAmount = data.StartAmount,
                    Balance = data.Balance,
                    Amount = data.Amount,
                    SupplyId = data.SupplyId,
                    Notes = data.Notes,
                    DepositId = data.DepositId,
                    BranchId = data.BranchId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorId = data.SectorId,
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    TransactionSubTypeId = data.TransactionSubTypeId,
                    TransactionSubTypeName = data.TransactionSubType != null ? data.TransactionSubType.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedByName = _userService.GetUserFullName(data.AspNetUser1),
                    CreatedBy = data.CreatedBy,
                    AccountName = accountName,
                    WeightNote = data.WeightNote,
                    Price = data.Price,
                    Quantity = data.Quantity,
                    Bags = data.Bags,
                    DocumentId = documentId,
                    Approved = data.Approved,


                };
                return deposit;
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
