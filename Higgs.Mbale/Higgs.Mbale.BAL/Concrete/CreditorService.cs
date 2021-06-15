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
  public  class CreditorService : ICreditorService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(CreditorService));
        private ICreditorDataService _dataService;
        private IUserService _userService;
        private ITransactionDataService _transactionDataService;
        private ITransactionSubTypeService _transactionSubTypeService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private IUtilityAccountService _utilityAccountService;
        private IBranchService _branchService;
       

        

        public CreditorService(ICreditorDataService dataService,IUserService userService,ITransactionDataService transactionDataService,
            ITransactionSubTypeService transactionSubTypeService,
            IAccountTransactionActivityService accountTransactionActivityService,
            IUtilityAccountService utilityAccountService,IBranchService branchService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionDataService = transactionDataService;
            this._transactionSubTypeService = transactionSubTypeService;
            this._accountTransactionActivityService = accountTransactionActivityService;
            this._utilityAccountService = utilityAccountService;
            this._branchService = branchService;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CreditorId"></param>
        /// <returns></returns>
        public Creditor GetCreditor(long creditorId)
        {
            var result = this._dataService.GetCreditor(creditorId);
            return MapEFToModel(result);
        }

        public IEnumerable<Creditor> GetAllCreditorsForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllCreditorsForAParticularBranch(branchId);
            return MapEFToModel(results);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Creditor> GetAllCreditors()
        {
            var results = this._dataService.GetAllCreditors();
            return MapEFToModel(results);
        }

        public IEnumerable<Creditor> GetAllDistinctCreditorRecords()
        {
            var creditors = GetAllCreditors();
            List<Creditor> distinctCreditors = new List<Creditor>();
            List<Creditor> xdistinctCreditors = new List<Creditor>();
                var distinctAspnetCreditors = creditors.GroupBy(g => g.AspNetUserId).Select(o => o.First()).ToList();
                var distinctCasualCreditors = creditors.GroupBy(g => g.CasualWorkerId).Select(o => o.First()).ToList();


                distinctCreditors.AddRange(distinctAspnetCreditors);
                distinctCreditors.AddRange(distinctCasualCreditors);
                xdistinctCreditors = distinctCreditors.Distinct().ToList();

                foreach (var creditor in xdistinctCreditors)
                {
                    double creditorBalance = 0;
                    var creditorRecords = GetAllCreditorRecordsForParticularAccount(creditor.AspNetUserId,Convert.ToInt64(creditor.CasualWorkerId));
                    foreach (var creditorRecord in creditorRecords)
                    {
                        creditorBalance = creditorRecord.Amount + creditorBalance;
                    }
                    creditor.CreditBalance = creditorBalance;
                }

                return xdistinctCreditors;
            
        }

      public  IEnumerable<Creditor> GetAllCreditorRecordsForParticularAccount(string aspNetUserId,long casualWorkerId)
        {
            var results = this._dataService.GetAllCreditorRecordsForParticularAccount(aspNetUserId,casualWorkerId);
            return MapEFToModel(results);
        }

      public IEnumerable<CreditorView> GetCreditorView()
      {
          List<CreditorView> creditorList = new List<CreditorView>();
        var suppliers =  _userService.GetAllSuppliers();
            var outsourcers = _userService.GetAllOutSourcers();
            var utilityCategories = _utilityAccountService.GetAllUtilityCategories();
            var branches = _branchService.GetAllBranches();
            foreach (var branch in branches)
            {
                foreach (var utilityCategory in utilityCategories)
                {
                    var result = _utilityAccountService.GetBalanceForLastUtilityAccount(branch.BranchId, utilityCategory.UtilityCategoryId);
                    if (result > 0)
                    {
                        var creditorView = new CreditorView()
                        {
                            //Id = utilityCategory.UtilityCategoryId,
                            Amount = result,
                            CreditorName = utilityCategory.Name,
                             BranchName = branch.Name,
                            

                        };
                        creditorList.Add(creditorView);
                    }

                   
                }
            }
        if (suppliers != null)
        {
            if (suppliers.Any())
            {
                foreach (var supplier in suppliers)
                {
                    var balance = _accountTransactionActivityService.GetBalanceForLastAccountAccountTransactionActivityForSupplier(supplier.Id);
                  
                    if (balance > 0)
                    {
                        var creditorView = new CreditorView()
                        {
                            Id = supplier.Id,
                            Amount = balance,
                            CreditorName = supplier.FirstName + ' ' + supplier.LastName,
                            CreditorNumber = supplier.UniqueNumber,

                        };
                        creditorList.Add(creditorView);
                    }
                   
                }
            }
        }
            if (outsourcers != null)
            {
                if (outsourcers.Any())
                {
                    foreach (var outsourcer in outsourcers)
                    {
                        var balance = _accountTransactionActivityService.GetBalanceForLastAccountAccountTransactionActivityForSupplier(outsourcer.Id);

                        if (balance > 0)
                        {
                            var creditorView = new CreditorView()
                            {
                                Id = outsourcer.Id,
                                Amount = balance,
                                CreditorName = outsourcer.FirstName + ' ' + outsourcer.LastName,
                               // CreditorNumber = supplier.UniqueNumber,

                            };
                            creditorList.Add(creditorView);
                        }

                    }
                }
            }
            return creditorList;
      }

        public IEnumerable<CreditorView> GetCreditorViewForAParticularDate(DateTime dateTime)
        {
            List<CreditorView> creditorList = new List<CreditorView>();
            var suppliers = _userService.GetAllSuppliers();
            var outsourcers = _userService.GetAllOutSourcers();
            var utilityCategories = _utilityAccountService.GetAllUtilityCategories();
            var branches = _branchService.GetAllBranches();
            foreach (var branch in branches)
            {
                foreach (var utilityCategory in utilityCategories)
                {
                    var result = _utilityAccountService.GetBalanceForLastUtilityAccountForAParticularDate(branch.BranchId, utilityCategory.UtilityCategoryId,dateTime);
                    if (result > 0)
                    {
                        var creditorView = new CreditorView()
                        {
                            //Id = utilityCategory.UtilityCategoryId,
                            Amount = result,
                            CreditorName = utilityCategory.Name,
                            BranchName = branch.Name,


                        };
                        creditorList.Add(creditorView);
                    }


                }
            }
            if (suppliers != null)
            {
                if (suppliers.Any())
                {
                    foreach (var supplier in suppliers)
                    {
                        var balance = _accountTransactionActivityService.GetBalanceForLastAccountAccountTransactionActivityForSupplierForAParticularDate(supplier.Id,dateTime);

                        if (balance > 0)
                        {
                            var creditorView = new CreditorView()
                            {
                                Id = supplier.Id,
                                Amount = balance,
                                CreditorName = supplier.FirstName + ' ' + supplier.LastName,
                                CreditorNumber = supplier.UniqueNumber,

                            };
                            creditorList.Add(creditorView);
                        }

                    }
                }
            }
            if (outsourcers != null)
            {
                if (outsourcers.Any())
                {
                    foreach (var outsourcer in outsourcers)
                    {
                        var balance = _accountTransactionActivityService.GetBalanceForLastAccountAccountTransactionActivityForSupplierForAParticularDate(outsourcer.Id, dateTime);

                        if (balance > 0)
                        {
                            var creditorView = new CreditorView()
                            {
                                Id = outsourcer.Id,
                                Amount = balance,
                                CreditorName = outsourcer.FirstName + ' ' + outsourcer.LastName,
                                
                            };
                            creditorList.Add(creditorView);
                        }

                    }
                }
            }
            return creditorList;
        }

        public long SaveCreditor(Creditor creditor, string userId)
        {
            var creditorDTO = new DTO.CreditorDTO()
            {
                AspNetUserId = creditor.AspNetUserId,
                Action = creditor.Action,
                Amount = creditor.Amount,
                CasualWorkerId = creditor.CasualWorkerId,
                BranchId = creditor.BranchId,
                SectorId = creditor.SectorId,
                CreditorId = creditor.CreditorId,
                Deleted = creditor.Deleted,
                CreatedBy = creditor.CreatedBy,
                CreatedOn = creditor.CreatedOn

            };

           var creditorId = this._dataService.SaveCreditor(creditorDTO, userId);
         
           return creditorId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CreditorId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long CreditorId, string userId)
        {
            _dataService.MarkAsDeleted(CreditorId, userId);
        }

      
        #region Mapping Methods

        private IEnumerable<Creditor> MapEFToModel(IEnumerable<EF.Models.Creditor> data)
        {
            var list = new List<Creditor>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Creditor EF object to Creditor Model Object and
        /// returns the Creditor model object.
        /// </summary>
        /// <param name="result">EF Creditor object to be mapped.</param>
        /// <returns>Creditor Model Object.</returns>
        public Creditor MapEFToModel(EF.Models.Creditor data)
        {
            var accountName = string.Empty;
            var accountUniqueNumber = string.Empty;

            if (data != null)
            {

                if (data.CasualWorkerId != null)
                {
                    accountName = (data.CasualWorker.FirstName + " " + data.CasualWorker.LastName);
                }
                else if (data.AspNetUserId != null)
                {
                    accountName = _userService.GetUserFullName(data.AspNetUser);
                    accountUniqueNumber = data.AspNetUser.UniqueNumber;
                }
                var creditor = new Creditor()
                {

                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    AccountName = accountName,
                    AccountUniqueNumber = accountUniqueNumber,
                    BranchId = data.BranchId,
                    AspNetUserId = data.AspNetUserId,
                    CasualWorkerId = data.CasualWorkerId,
                    Action = data.Action,
                    SectorId = data.SectorId,
                    Amount = data.Amount,
                    CreditorId = data.CreditorId,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };
                return creditor;
            }
            return null;
        }



       #endregion
    }
}
