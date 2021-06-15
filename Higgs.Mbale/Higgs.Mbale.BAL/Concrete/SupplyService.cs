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
using System.Configuration;

namespace Higgs.Mbale.BAL.Concrete
{
  public  class SupplyService : ISupplyService
    {
      private string supplyTransactionSubTypeId = ConfigurationManager.AppSettings["SupplyTransactionSubTypeId"];
      private string offLoadingTransactionSubTypeId = ConfigurationManager.AppSettings["OffLoadingTransactionSubTypeId"];
      private string yellowTransactionSubTypeId = ConfigurationManager.AppSettings["YellowTransactionSubTypeId"];
      private string offloadingRate = ConfigurationManager.AppSettings["OffloadingRate"];
      private string stoneRate = ConfigurationManager.AppSettings["StoneRate"];
      private string yellowRate = ConfigurationManager.AppSettings["YellowRate"];
      private string sectorId = ConfigurationManager.AppSettings["SectorId"];

         ILog logger = log4net.LogManager.GetLogger(typeof(SupplyService));
        private ISupplyDataService _dataService;
        private IUserService _userService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private ICreditorService _creditorService;
        private ITransactionService _transactionService;
        private ICashService _cashService;
        private IWeightNoteNumberService _weightNoteNumberService;
        

        public SupplyService(ISupplyDataService dataService,IUserService userService,
            IAccountTransactionActivityService accountTransactionActivityService,
            ICreditorService creditorService, ITransactionService transactionService,
            ICashService cashService,IWeightNoteNumberService weightNoteNumberService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._accountTransactionActivityService = accountTransactionActivityService;
            this._creditorService = creditorService;
            this._transactionService = transactionService;
            this._cashService = cashService;
            this._weightNoteNumberService = weightNoteNumberService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SupplyId"></param>
        /// <returns></returns>
        public Supply GetSupply(long supplyId)
        {
            var result = this._dataService.GetSupply(supplyId);
            return MapEFToModel(result);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Supply> GetAllSupplies()
        {
            var results = this._dataService.GetAllSupplies();
            return MapEFToModel(results);
        }

        public IEnumerable<Supply> GetAllUnApprovedSupplies()
        {
            var results = this._dataService.GetAllUnApprovedSupplies();
            return MapEFToModel(results);
        }

        public IEnumerable<Supply> GetAllUnApprovedSuppliesForABranch(long branchId)
        {
            var results = this._dataService.GetAllUnApprovedSuppliesForABranch(branchId);
            return MapEFToModel(results);
        }
        public long MakeSupplyPayment(MultipleSupplies model, AccountTransactionActivity accountActivity, string userId)
        {
            long accountActivityId = 0;
            //long defaultBranchId = 2;
            string supplierId = "";
            if (model.Supplies.Any())
            {
                foreach (var supply in model.Supplies)
                {
                    supplierId = supply.SupplierId;
                    var supplyObject = new SupplyDTO()
                    {
                        SupplyId = supply.SupplyId,
                        SupplierId = supply.SupplierId,
                        //SupplyNumber = supply.SupplyNumber,
                        Price = supply.Price,
                        Amount = supply.Amount,
                        AmountToPay = supply.AmountToPay,
                        StoreId = supply.StoreId,
                        BranchId = supply.BranchId,
                        SupplyDate = supply.SupplyDate,
                        TruckNumber = supply.TruckNumber,
                        WeightNoteNumber = supply.WeightNoteNumber,
                        Used = supply.Used,
                        IsPaid = true,
                        StatusId = supply.StatusId,
                        Deleted = supply.Deleted,
                        MoistureContent = supply.MoistureContent,
                        BagsOfStones = supply.BagsOfStones,
                        NormalBags = supply.NormalBags,
                        Quantity = supply.Quantity,
                        Offloading = supply.Offloading,
                        PartialAmount = supply.PartialAmount,
                        PartiallyPaid = supply.PartiallyPaid,
                        YellowBags = supply.YellowBags,
                        Approved = supply.Approved,
                    };
                   _dataService.SaveSupply(supplyObject, userId);
                }

        
                //UpdateCreditorRecords(supplierId, accountActivity.Amount, userId);


                var accountActivityObject = new AccountTransactionActivity()
                {
                    AspNetUserId = accountActivity.AspNetUserId,
                    CasualWorkerId = accountActivity.CasualWorkerId,
                    Amount = accountActivity.Amount,
                    Notes = accountActivity.Notes,
                    AccountTransactionActivityId = accountActivity.AccountTransactionActivityId,
                    Action = accountActivity.Action,
                    BranchId = accountActivity.BranchId,
                    TransactionSubTypeId = accountActivity.TransactionSubTypeId,
                    SectorId = accountActivity.SectorId,
                    Deleted = accountActivity.Deleted,
                    CreatedBy = accountActivity.CreatedBy,
                    CreatedOn = accountActivity.CreatedOn
                };
                accountActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountActivityObject, userId);

                if (accountActivity.PaymentModeId == 2)
                {
                    if (accountActivity.BranchId == 0 || accountActivity.BranchId == null )
                    {

                        Cash cash = new Cash()
                        {
                            Amount = accountActivity.Amount,
                            Notes = accountActivity.Notes,
                            Action = accountActivity.Action,
                            BranchId = Convert.ToInt64(accountActivity.BranchId),
                            TransactionSubTypeId = accountActivity.TransactionSubTypeId,
                            SectorId = accountActivity.SectorId,

                        }; 
                        _cashService.SaveApplicationCash(cash,userId);
                    }
                    else
                    {
                        Cash cash = new Cash()
                        {
                            Amount = accountActivity.Amount,
                            Notes = accountActivity.Notes,
                            Action = accountActivity.Action,
                            BranchId = Convert.ToInt64(accountActivity.BranchId),
                            TransactionSubTypeId = accountActivity.TransactionSubTypeId,
                            SectorId = accountActivity.SectorId,

                        };
                        _cashService.SaveCash(cash,userId);
                        
                    }
                }
            }
            return accountActivityId;
        }

      
        private void UpdateCreditorRecords(string aspNetUserId,double amount,string userId)
        {
            long? branchId = null;
            double creditBalance=0,creditTotal=0;
            long casualWorkerId=0 ;
            var creditorRecords = _creditorService.GetAllCreditorRecordsForParticularAccount(aspNetUserId,casualWorkerId);
            if (creditorRecords.Any())
            {
                foreach (var creditorRecord in creditorRecords)
                {
                    branchId = Convert.ToInt64(creditorRecord.BranchId);
                    creditTotal = creditorRecord.Amount + creditTotal;
                    var creditor = new Creditor()
                    {
                        CreditorId = creditorRecord.CreditorId,
                        AspNetUserId = creditorRecord.AspNetUserId,
                        Action = true,
                        Amount = creditorRecord.Amount,
                        BranchId = creditorRecord.BranchId,
                        SectorId = creditorRecord.SectorId,
                        Deleted = creditorRecord.Deleted,
                        CreatedBy = creditorRecord.CreatedBy,
                        CreatedOn = creditorRecord.CreatedOn,
                        TimeStamp = creditorRecord.TimeStamp,


                    };
                    var creditorId = _creditorService.SaveCreditor(creditor, userId);
                }
                creditBalance =  creditTotal -amount ;
                if (creditBalance > 0)
                {
                    var creditor = new Creditor()
                    {
                        AspNetUserId = aspNetUserId,
                        Action = false,
                        Amount = creditBalance,
                        BranchId = branchId,
                        SectorId = Convert.ToInt64(sectorId),
                        Deleted = false,
                        CreatedBy = userId,
                       

                    };
                    var creditorId = _creditorService.SaveCreditor(creditor, userId);
                }

            }
           
        }     
        public IEnumerable<Supply> GetAllSuppliesToBeUsed()
        {
            var results = this._dataService.GetAllSuppliesToBeUsed();
            return MapEFToModel(results);
        }

        public IEnumerable<Supply> GetAllUnPaidSupplies()
        {
            var results = this._dataService.GetAllUnPaidSupplies();
            return MapEFToModel(results);
        }
        public IEnumerable<Supply> GetAllSuppliesForAParticularSupplier(string supplierId)
        {
            var results = this._dataService.GetAllSuppliesForAParticularSupplier(supplierId);
            return MapEFToModel(results);
        }

        public IEnumerable<Supply> GetAllUnPaidSuppliesForAParticularSupplier(string supplierId)
        {
            var results = this._dataService.GetAllUnPaidSuppliesForAParticularSupplier(supplierId);
            return MapEFToModel(results);
        }
        public IEnumerable<Supply> GetAllPaidSuppliesForAParticularSupplier(string supplierId)
        {
            var results = this._dataService.GetAllPaidSuppliesForAParticularSupplier(supplierId);
            return MapEFToModel(results);
        }

        public IEnumerable<Supply> GetAllSuppliesToBeUsedForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllSuppliesToBeUsedForAParticularBranch(branchId);
            return MapEFToModel(results);
        }
        public IEnumerable<Supply> GetAllSuppliesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllSuppliesForAParticularBranch(branchId);
            return MapEFToModel(results);
        }
        private bool CheckIfSupplyAlreadyExists(long supplyId)
        {
            bool transactionExists, accountTransactionExists,supplyExists=false;
            transactionExists = _transactionService.checkIfSupplyRelatesToAnyTransaction(supplyId);
            accountTransactionExists = _accountTransactionActivityService.checkIfSupplyRelatesToAnyAccountTransaction(supplyId);
            if (transactionExists || accountTransactionExists)
            {
                supplyExists = true;
            }

           return supplyExists;

        }

        public long UpdateSupply(Supply supply, string userId)
        {
            //double amount = 0, totalBags = 0, offloadingFee = 0, stoneBagsFee = 0, yellowBagsFee = 0;
            //amount = (supply.Price * supply.Quantity);

          

            //if (supply.Offloading == "NO")
            //{

            //    totalBags = supply.NormalBags + supply.BagsOfStones + Convert.ToDouble(supply.YellowBags);
            //    offloadingFee = totalBags * (Convert.ToDouble(offloadingRate));
            //    stoneBagsFee = Convert.ToDouble(supply.BagsOfStones) * Convert.ToDouble(stoneRate);
            //    yellowBagsFee = Convert.ToDouble(supply.YellowBags) * Convert.ToDouble(yellowRate);
                
            //}
            //else
            //{
            //    stoneBagsFee = Convert.ToDouble(supply.BagsOfStones) * Convert.ToDouble(stoneRate);
            //    yellowBagsFee = Convert.ToDouble(supply.YellowBags) * Convert.ToDouble(yellowRate);

               
            //}
            var supplyDTO = new DTO.SupplyDTO()
            {
                Quantity = supply.Quantity,
                SupplyDate = supply.SupplyDate,
               
                BranchId = supply.BranchId,
                SupplierId = supply.SupplierId,
                Amount = supply.Amount,
                TruckNumber = supply.TruckNumber,
                AmountToPay = supply.AmountToPay,
                Used = supply.Used,
                SupplyId = supply.SupplyId,
                WeightNoteNumber = supply.WeightNoteNumber,
                MoistureContent = supply.MoistureContent,
                NormalBags = supply.NormalBags,
                BagsOfStones = supply.BagsOfStones,
                Price = supply.Price,
                IsPaid = supply.IsPaid,
                StatusId = supply.StatusId,
                CreatedOn = supply.CreatedOn,
                TimeStamp = supply.TimeStamp,
                CreatedBy = supply.CreatedBy,
                Deleted = supply.Deleted,
                StoreId = supply.StoreId,
                Offloading = supply.Offloading,
                YellowBags = supply.YellowBags,
                PartiallyPaid = supply.PartiallyPaid,
                PartialAmount = supply.PartialAmount,
                Approved = supply.Approved,

            };

       var supplyId =      this._dataService.UpdateSupplyOnRequistionAprroval(supplyDTO, userId);
          

            return supplyId;

        }

        public long SaveSupply(Supply supply, string userId)
        {
            double amount = 0, totalBags = 0,offloadingFee = 0,amountToPay = 0,stoneBagsFee = 0,amountafterOffloading = 0,yellowBagsFee= 0;
            amount = (supply.Price * supply.Quantity);
            System.Text.StringBuilder weightNoteNumber = new System.Text.StringBuilder(" ");

           
                 if (supply.WeightNoteNumbers != null)
                {
                if(supply.WeightNoteNumbers.Count() == 1)
                {
                    var weightN = supply.WeightNoteNumbers.FirstOrDefault();
                    weightNoteNumber.Append(Convert.ToString(weightN.WeightNoteValue));

                    var weightNumber = new WeightNoteNumber()
                    {
                        WeightNoteNumberId = weightN.WeightNoteNumberId,
                        WeightNoteRangeId = weightN.WeightNoteRangeId,
                        Used = true,
                        BranchId = weightN.BranchId,
                        WeightNoteValue = weightN.WeightNoteValue,

                    };
                    _weightNoteNumberService.SaveWeightNoteNumber(weightNumber, userId);


                }
                else
                {
                    foreach (var weightNote in supply.WeightNoteNumbers)
                    {

                        weightNoteNumber.Append(Convert.ToString(weightNote.WeightNoteValue));
                        weightNoteNumber.Append(" ");

                        var weightN = new WeightNoteNumber()
                        {
                            WeightNoteNumberId = weightNote.WeightNoteNumberId,
                            WeightNoteRangeId = weightNote.WeightNoteRangeId,
                            Used = true,
                            BranchId = weightNote.BranchId,
                            WeightNoteValue = weightNote.WeightNoteValue,

                        };
                      _weightNoteNumberService.SaveWeightNoteNumber(weightN, userId);

                    }
                }

                    
                }

           
              
       
            if (supply.SupplyId == 0)
            {
                // int exists = _dataService.CheckIfWeightNoteExists(supply.WeightNoteNumber);
                 int exists = _dataService.CheckIfWeightNoteExists(weightNoteNumber.ToString());
                if (exists > 0)
                {
                    return -1;
                }

            }

            if (supply.Offloading == "NO")
            {

                totalBags = supply.NormalBags + supply.BagsOfStones + Convert.ToDouble(supply.YellowBags);
                offloadingFee = totalBags * (Convert.ToDouble(offloadingRate));
               amountafterOffloading = amount - offloadingFee;
               stoneBagsFee = Convert.ToDouble(supply.BagsOfStones) * Convert.ToDouble(stoneRate);
               yellowBagsFee = Convert.ToDouble(supply.YellowBags) * Convert.ToDouble(yellowRate);
               amountToPay = amountafterOffloading - (stoneBagsFee + yellowBagsFee);

            }
            else
            {
                stoneBagsFee = Convert.ToDouble(supply.BagsOfStones) * Convert.ToDouble(stoneRate);
                yellowBagsFee = Convert.ToDouble(supply.YellowBags) * Convert.ToDouble(yellowRate);
                totalBags = supply.NormalBags + supply.BagsOfStones + Convert.ToDouble(supply.YellowBags);
                amountToPay = amount - (stoneBagsFee + yellowBagsFee);
            }
            var supplyDTO = new DTO.SupplyDTO()
            {
                Quantity = supply.Quantity,
                SupplyDate = supply.SupplyDate,
               
                BranchId = supply.BranchId,
                SupplierId = supply.SupplierId,
                Amount = amount,
                TruckNumber = supply.TruckNumber,
                AmountToPay = amountToPay,
                Used = supply.Used,
                SupplyId = supply.SupplyId,
                //WeightNoteNumber = supply.WeightNoteNumber,
                WeightNoteNumber =  supply.SupplyId == 0 ? weightNoteNumber.ToString() : supply.WeightNoteNumber,
                MoistureContent = supply.MoistureContent,
                NormalBags = supply.NormalBags,
                BagsOfStones = supply.BagsOfStones,
                Price = supply.Price,
                IsPaid = supply.IsPaid,
                StatusId = supply.StatusId,
                CreatedOn = supply.CreatedOn,
                TimeStamp = supply.TimeStamp,
                CreatedBy = supply.CreatedBy,
                Deleted = supply.Deleted,
                StoreId = supply.StoreId,
                Offloading = supply.Offloading,
                YellowBags = supply.YellowBags,
                PartiallyPaid = supply.PartiallyPaid,
                PartialAmount = supply.PartialAmount,
                Approved = supply.Approved,
              
            };

           var supplyId = this._dataService.SaveSupply(supplyDTO, userId);
            if(supply.Approved == true)
            {
                
                var notes = "Maize";
                var accountActivity = new AccountTransactionActivity()
                {

                    AspNetUserId = supply.SupplierId,

                    Amount = amount,
                    Notes = notes,
                    Action = "+",
                    BranchId = supply.BranchId,
                    TransactionSubTypeId = Convert.ToInt64(supplyTransactionSubTypeId),
                    SectorId = Convert.ToInt64(sectorId),
                    Deleted = supply.Deleted,
                    CreatedBy = userId,
                    SupplyId = supplyId,
                    Price = supply.Price,
                    Quantity = supply.Quantity,
                   // WeightNote = supply.WeightNoteNumber,
                   WeightNote = supply.SupplyId == 0 ? weightNoteNumber.ToString() : supply.WeightNoteNumber,
                    Bags = totalBags

                };
                var accountActivityId = this._accountTransactionActivityService.SaveAccountTransactionActivity(accountActivity, userId);

                if (supply.BagsOfStones > 0)
                {
                    var stoneBagNotes = "stones";

                    var accountActivityBagStones = new AccountTransactionActivity()
                    {

                        AspNetUserId = supply.SupplierId,

                        Amount = stoneBagsFee,
                        Notes = stoneBagNotes,
                        Action = "-",
                        BranchId = supply.BranchId,
                        TransactionSubTypeId = Convert.ToInt64(offLoadingTransactionSubTypeId),
                        SectorId = Convert.ToInt64(sectorId),
                        Deleted = supply.Deleted,
                        CreatedBy = userId,
                        SupplyId = supplyId,
                        Price = Convert.ToDouble(stoneRate),

                       // WeightNote = supply.WeightNoteNumber,
                       WeightNote = supply.SupplyId == 0 ? weightNoteNumber.ToString() : supply.WeightNoteNumber,
                        Bags = supply.BagsOfStones,

                    };
                    var accountActivityBagsOfStoneId = this._accountTransactionActivityService.SaveAccountTransactionActivity(accountActivityBagStones, userId);


                }
                if (supply.Offloading == "NO")
                {
                    var offLoadingNotes = "Offloading ";

                    var accountActivityOffloading = new AccountTransactionActivity()
                    {

                        AspNetUserId = supply.SupplierId,

                        Amount = offloadingFee,
                        Notes = offLoadingNotes,
                        Action = "-",
                        BranchId = supply.BranchId,
                        TransactionSubTypeId = Convert.ToInt64(offLoadingTransactionSubTypeId),
                        SectorId = Convert.ToInt64(sectorId),
                        Deleted = supply.Deleted,
                        CreatedBy = userId,
                        SupplyId = supplyId,
                        Bags = totalBags,
                        Price = Convert.ToDouble(offloadingRate),
                        //WeightNote = supply.WeightNoteNumber,
                        WeightNote = supply.SupplyId == 0 ? weightNoteNumber.ToString() : supply.WeightNoteNumber,

                    };
                    var accountActivityOffloadingId = this._accountTransactionActivityService.SaveAccountTransactionActivity(accountActivityOffloading, userId);

                }

                if (supply.YellowBags > 0)
                {
                    var yellowBagsNotes = "yellow Bags ";

                    var accountActivityYellowBags = new AccountTransactionActivity()
                    {

                        AspNetUserId = supply.SupplierId,

                        Amount = yellowBagsFee,
                        Notes = yellowBagsNotes,
                        Action = "-",
                        BranchId = supply.BranchId,
                        TransactionSubTypeId = Convert.ToInt64(yellowTransactionSubTypeId),
                        SectorId = Convert.ToInt64(sectorId),
                        Deleted = supply.Deleted,
                        CreatedBy = userId,
                        SupplyId = supplyId,
                        Bags = supply.YellowBags,
                        Price = Convert.ToDouble(yellowRate),
                        //WeightNote = supply.WeightNoteNumber,
                        WeightNote  = supply.SupplyId == 0 ? weightNoteNumber.ToString() : supply.WeightNoteNumber,

                    };
                    var accountActivityYellowId = this._accountTransactionActivityService.SaveAccountTransactionActivity(accountActivityYellowBags, userId);

                }

            }


            return supplyId;
                      
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SupplyId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long SupplyId, string userId)
        {
            _dataService.MarkAsDeleted(SupplyId, userId);
        }

        private double GetStockMaizeBalanceForLastSupplyTransaction(long storeId)
        {
            double balance = 0;

            var result = this._dataService.GetLatestMaizeStockForAParticularStore(storeId);
            if (result.StoreMaizeStockId > 0)
            {
                balance = result.StockBalance;
            }

            return balance;

        }

        public IEnumerable<StoreMaizeStock> GetMaizeStocksForAParticularStore(long storeId)
        {
            var results = this._dataService.GetMaizeStocksForAParticularStore(storeId);
            return MapEFToModel(results);
        }

        public void SaveStoreMaizeStock(StoreMaizeStock storeMaizeStock, bool inOrOut)
        {

            double startStock = 0;
            double OldMaizeStockBalance = 0;
            double NewMaizeStockBalance = 0;


            OldMaizeStockBalance = GetStockMaizeBalanceForLastSupplyTransaction(storeMaizeStock.StoreId);
            startStock = OldMaizeStockBalance;


            if (inOrOut == true)
            {
                NewMaizeStockBalance = OldMaizeStockBalance + storeMaizeStock.Quantity;
            }
            else
            {
                NewMaizeStockBalance = OldMaizeStockBalance - storeMaizeStock.Quantity;
            }

            var storeMaizeStockDTO = new DTO.StoreMaizeStockDTO()
            {
                StoreMaizeStockId = storeMaizeStock.StoreMaizeStockId,
                StoreId = storeMaizeStock.StoreId,
                StartStock = startStock,
                SupplyId = storeMaizeStock.SupplyId,
                StockBalance = NewMaizeStockBalance,
                BranchId = storeMaizeStock.BranchId,
                Quantity = storeMaizeStock.Quantity,
                SectorId = storeMaizeStock.SectorId,
                TimeStamp = storeMaizeStock.TimeStamp,
                InOrOut = inOrOut,

            };

            this._dataService.SaveStoreMaizeStock(storeMaizeStockDTO);
        }
            
        #region Mapping Methods

        public IEnumerable<Supply> MapEFToModel(IEnumerable<EF.Models.Supply> data)
        {
            var list = new List<Supply>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Supply EF object to Supply Model Object and
        /// returns the Supply model object.
        /// </summary>
        /// <param name="result">EF Supply object to be mapped.</param>
        /// <returns>Supply Model Object.</returns>
        public Supply MapEFToModel(EF.Models.Supply data)
        {
            if (data != null)
            {

                var Supply = new Supply()
                {
                    Quantity = data.Quantity,
                    SupplyDate = data.SupplyDate,
                    SupplyId = data.SupplyId,
                    BranchId = data.BranchId,
                    SupplierId = data.SupplierId,
                    Amount = data.Amount,
                    AmountToPay = data.AmountToPay,
                    TruckNumber = data.TruckNumber,
                    Price = data.Price,
                    Used = data.Used,
                    StoreId = data.StoreId,
                    StoreName = data.Store != null ? data.Store.Name : "",
                    StatusId = data.StatusId,
                    IsPaid = data.IsPaid,
                    NormalBags = data.NormalBags,
                    BagsOfStones = data.BagsOfStones,
                    MoistureContent = data.MoistureContent,
                    WeightNoteNumber = data.WeightNoteNumber,
                    Offloading = data.Offloading == "YES" ? "YES" : (data.Offloading == "NO" ? "NO" : null),
                    YellowBags = data.YellowBags,
                    PartiallyPaid = data.PartiallyPaid,
                    PartialAmount = data.PartialAmount,
                    Approved = data.Approved,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    StatusName = data.Status != null ? data.Status.Name : "",
                    SupplierName = _userService.GetUserFullName(data.AspNetUser2),
                    SupplierNumber = data.AspNetUser2.UniqueNumber,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    CreatedBy  = data.CreatedBy,
                    UpdatedBy = data.UpdatedBy,
                    CreatedByName = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedByName = _userService.GetUserFullName(data.AspNetUser1),

                };
                return Supply;
            }
            return null;
        }
      
        public IEnumerable<StoreMaizeStock> MapEFToModel(IEnumerable<EF.Models.StoreMaizeStock> data)
        {
            var list = new List<StoreMaizeStock>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps StoreMaizeStock EF object to StoreMaizeStock Model Object and
        /// returns the StoreMaizeStock model object.
        /// </summary>
        /// <param name="result">EF StoreMaizeStock object to be mapped.</param>
        /// <returns>StoreMaizeStock Model Object.</returns>
        public StoreMaizeStock MapEFToModel(EF.Models.StoreMaizeStock data)
        {
            if (data != null)
            {


                var storeMaizeStock = new StoreMaizeStock()
                {
                    Quantity = data.Quantity,
                    StockBalance = data.StockBalance,
                    StartStock = data.StartStock,
                    SupplyId = data.SupplyId,
                    //SupplyNumber = data.Supply != null ? Convert.ToString(data.Supply.SupplyNumber) :"",
                    BranchId = data.BranchId,
                    StoreId = data.StoreId,
                    StoreName = data.Store != null ? data.Store.Name : "",
                    TimeStamp = data.TimeStamp,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorId = data.SectorId,
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    StoreMaizeStockId = data.StoreMaizeStockId,
                    MaizeInOrOut = (data.InOrOut == true) ? "Maize In" : "Maize Out",
                };
                return storeMaizeStock;
            }
            return null;
        }


       #endregion
    }
}
