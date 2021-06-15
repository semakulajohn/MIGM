using System;
using System.Collections.Generic;
using System.Text;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;
using System.Web;
using System.Configuration;
using System.IO;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class RequistionService : IRequistionService
    {
        private long allowanceTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["AllowanceId"]);
        private long supplyTransactionSubTypeId =Convert.ToInt64( ConfigurationManager.AppSettings["SupplyTransactionSubTypeId"]);
        private long requistionStatusIdComplete = Convert.ToInt64(ConfigurationManager.AppSettings["StatusIdComplete"]);
     private long paymentVoucherId = Convert.ToInt64(ConfigurationManager.AppSettings["PaymentVoucher"]);
     private long debitId = Convert.ToInt64(ConfigurationManager.AppSettings["DebitId"]);
        private long bankTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["BankTransactionSubTypeId"]);
        private long sectorId = Convert.ToInt64(ConfigurationManager.AppSettings["SectorId"]);
       ILog logger = log4net.LogManager.GetLogger(typeof(RequistionService));
        private IRequistionDataService _dataService;
        private IUserService _userService;
        private IDocumentService _documentService;
        private ICashService _cashService;
        private IFactoryExpenseService _factoryExpenseService;
        private IMachineRepairService _machineRepairService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private ISupplyService _supplyService;
        private ILabourCostService _labourCostService;
        private IUtilityAccountService _utilityAccountService;
        private IBankTransactionService _bankTransactionService;
        private IFinancialAccountTransactionService _financialAccountTransactionService;
        

        public RequistionService(IRequistionDataService dataService,IUserService userService,IDocumentService documentService,
            ICashService cashService,IFactoryExpenseService factoryExpenseService,IMachineRepairService machineRepairService,
            IAccountTransactionActivityService accountTransactionActivityService,ISupplyService supplyService,
            ILabourCostService labourCostService,IUtilityAccountService utilityAccountService,IBankTransactionService bankTransactionService,IFinancialAccountTransactionService financialAccountTransactionService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._documentService = documentService;
            this._cashService = cashService;
            this._factoryExpenseService = factoryExpenseService;
            this._machineRepairService = machineRepairService;
            this._accountTransactionActivityService = accountTransactionActivityService;
            this._supplyService = supplyService;
            this._labourCostService = labourCostService;
            this._utilityAccountService = utilityAccountService;
            this._bankTransactionService = bankTransactionService;
            this._financialAccountTransactionService = financialAccountTransactionService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequistionId"></param>
        /// <returns></returns>
        public Requistion GetRequistion(long requistionId)
        {
            var result = this._dataService.GetRequistion(requistionId);
            return MapEFToModel(result);
        }

        public IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllRequistionsForAParticularBranch(branchId);
            return MapRequistionEFToRequistionViewModel(results);
        }

        public IEnumerable<RequistionViewModel> GetLatestSixtyRequistionsForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetLatestSixtyRequistionsForAParticularBranch(branchId);
            return MapRequistionEFToRequistionViewModel(results);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Requistion> GetAllRequistions()
        {
            var results = this._dataService.GetAllRequistions();
            return MapEFToModel(results);
        }

        public IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularStatus(long statusId)
        {
            var results = this._dataService.GetAllRequistionsForAParticularStatus(statusId);
            return MapRequistionEFToRequistionViewModel(results);
        }

        public IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularStatusForBranch(long statusId,long branchId)
        {
            var results = this._dataService.GetAllRequistionsForAParticularStatusForBranch(statusId,branchId);
            return MapRequistionEFToRequistionViewModel(results);
        }

        public IEnumerable<RequistionViewModel> GetLatestThirtyRequistionsForAParticularStatusForBranch(long statusId, long branchId)
        {
            var results = this._dataService.GetLatestThirtyRequistionsForAParticularStatusForBranch(statusId, branchId);
            return MapRequistionEFToRequistionViewModel(results);
        }

        private long GetRequistionNumber()
        {
            long requistionNumber = 0;
            var latestRequistion = _dataService.GetLatestCreatedRequistion();
            if (latestRequistion != null)
            {
                requistionNumber = Convert.ToInt64(latestRequistion.RequistionNumber) + 1;
            }
            else
            {
                requistionNumber = requistionNumber + 1;

            }
            return requistionNumber;
        }
        public long SaveRequistion(Requistion requistion, string userId)
        {
            long requistionId = 0,cashId =0;
           if (requistion.RequistionCategoryId == 2 && requistionId == 0)
            {

                requistion.Description = requistion.Description + "  " + requistion.WeightNoteNumber;
            }
            
            var requistionDTO = new DTO.RequistionDTO()
            {
                RequistionId = requistion.RequistionId,
                Response = requistion.Response,
                StatusId = requistion.StatusId,
                RequistionNumber = requistion.RequistionNumber,
                Amount = requistion.Amount,
                Approved = requistion.Approved,
                AmountInWords = requistion.AmountInWords,
                Rejected = requistion.Rejected,
                ApprovedById = requistion.ApprovedById,
                BranchId = requistion.BranchId,
                Description = requistion.Description,
                FinancialAccountId = requistion.FinancialAccountId,
                Deleted = requistion.Deleted,
                CreatedBy = requistion.CreatedBy,
                CreatedOn = requistion.CreatedOn,
                ActivityId = requistion.ActivityId,
                BatchId = requistion.BatchId,
                SupplyId = requistion.SupplyId,
                PartPayment = requistion.PartPayment,
                CasualWorkerId = requistion.CasualWorkerId,
                RequistionCategoryId = requistion.RequistionCategoryId,
                Quantity = requistion.Quantity,
                RepairDate = requistion.RepairDate,
                RepairerName = requistion.RepairerName,
                UtilityCategoryId = requistion.UtilityCategoryId,
                BankId = requistion.BankId,
                OutSourcerId = requistion.OutSourcerId,

            };

          
           if (requistion.Approved && requistion.ApprovedById != null)
           {
                if(requistion.CreatedById == requistion.ApprovedById)
                {
                    requistionId = -88;
                    return requistionId;
                }
                else
                {
                    //factoryexpense
                    if (requistion.RequistionCategoryId == 1)
                    {
                        if (requistion.BatchId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var factoryExpenseObject = new FactoryExpense()
                                {
                                    FactoryExpenseId = 0,
                                    BatchId = Convert.ToInt64(requistion.BatchId),
                                    Description = requistion.Description,
                                    BranchId = requistion.BranchId,
                                    Amount = requistion.Amount,
                                    Deleted = requistion.Deleted,
                                    CreatedBy = userId,
                                    CreatedOn = Convert.ToDateTime(requistion.CreatedOn),
                                    SectorId = 1,

                                };

                                var factoryExpenseId = _factoryExpenseService.SaveFactoryExpense(factoryExpenseObject, userId);
                                var cashFactory = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashFactory, userId);
                            }
                            else
                            {

                                return requistionId = checkedCashId;
                            }

                        }
                        else
                        {
                            requistionId = -7;
                            return requistionId;
                        }

                    }
                    //supply
                    else if (requistion.RequistionCategoryId == 2)
                    {
                        Supply supply = new Supply();
                        if (requistion.SupplyId != null)
                        {
                            supply = _supplyService.GetSupply(Convert.ToInt64(requistion.SupplyId));
                            if (supply.IsPaid == true)
                            {
                                int paid = -6;
                                //supply already paid
                                return requistionId = paid;
                            }
                            else
                            {
                                if (requistion.PartPayment == true)
                                {
                                    if (supply.AmountToPay < requistion.Amount)
                                    {
                                        int paid = -4;
                                        //paying more than the supply cost
                                        return requistionId = paid;
                                    }

                                    else
                                    {
                                        bool isPaid = false;
                                        var amountPaid = supply.AmountToPay - requistion.Amount;
                                        var partialAmount = (supply.PartialAmount == null ? 0 : supply.PartialAmount) + requistion.Amount;
                                        if (amountPaid == 0)
                                        {
                                            isPaid = true;
                                        }
                                        var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                                        if (checkedCashId > 0)
                                        {
                                            var supplyObject = new Supply()
                                            {
                                                SupplyId = supply.SupplyId,
                                                SupplierId = supply.SupplierId,

                                                Price = supply.Price,
                                                Amount = supply.Amount,
                                                AmountToPay = amountPaid,
                                                StoreId = supply.StoreId,
                                                BranchId = supply.BranchId,
                                                SupplyDate = supply.SupplyDate,
                                                TruckNumber = supply.TruckNumber,
                                                WeightNoteNumber = supply.WeightNoteNumber,
                                                Used = supply.Used,
                                                CreatedOn = supply.CreatedOn,
                                                CreatedBy = supply.CreatedBy,
                                                IsPaid = isPaid,
                                                StatusId = supply.StatusId,
                                                Deleted = supply.Deleted,
                                                MoistureContent = supply.MoistureContent,
                                                BagsOfStones = supply.BagsOfStones,
                                                NormalBags = supply.NormalBags,
                                                Quantity = supply.Quantity,
                                                Offloading = supply.Offloading,
                                                PartialAmount = partialAmount,
                                                PartiallyPaid = true,
                                                YellowBags = supply.YellowBags,
                                                Approved = supply.Approved,
                                            };
                                            _supplyService.UpdateSupply(supplyObject, userId);
                                            var accountActivityObject = new AccountTransactionActivity()
                                            {
                                                AspNetUserId = supply.SupplierId,
                                                Amount = requistion.Amount,
                                                Notes = requistion.Description,
                                                Action = "-",
                                                BranchId = requistion.BranchId,
                                                TransactionSubTypeId = supplyTransactionSubTypeId,
                                                SectorId = sectorId,
                                                CreatedOn = DateTime.Now,
                                            };
                                            var accountActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountActivityObject, userId);

                                            var cashSupply = new Cash()
                                            {

                                                Amount = requistion.Amount,
                                                Notes = requistion.Description,
                                                Action = "-",
                                                BranchId = requistion.BranchId,
                                                TransactionSubTypeId = debitId,
                                                SectorId = sectorId,
                                                RequistionCategoryId = requistion.RequistionCategoryId,
                                                CreatedBy = requistion.ApprovedById,

                                            };

                                            cashId = _cashService.SaveCash(cashSupply, userId);

                                        }
                                        else
                                        {
                                            requistionId = checkedCashId;
                                            return requistionId;
                                        }

                                    }
                                }
                                else
                                {
                                    if (supply.AmountToPay < requistion.Amount)
                                    {
                                        int paid = -4;
                                        //paying more than the supply cost
                                        return requistionId = paid;
                                    }

                                    else
                                    {
                                        bool isPaid = false;
                                        var amountPaid = supply.AmountToPay - requistion.Amount;
                                        var partialAmount = (supply.PartialAmount == null ? 0 : supply.PartialAmount) + requistion.Amount;
                                        if (amountPaid == 0)
                                        {
                                            isPaid = true;
                                        }

                                        var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                                        if (checkedCashId > 0)
                                        {
                                            var supplyObject = new Supply()
                                            {
                                                SupplyId = supply.SupplyId,
                                                SupplierId = supply.SupplierId,

                                                Price = supply.Price,
                                                Amount = supply.Amount,
                                                AmountToPay = amountPaid,
                                                StoreId = supply.StoreId,
                                                BranchId = supply.BranchId,
                                                SupplyDate = supply.SupplyDate,
                                                TruckNumber = supply.TruckNumber,
                                                WeightNoteNumber = supply.WeightNoteNumber,
                                                Used = supply.Used,
                                                IsPaid = isPaid,
                                                CreatedOn = supply.CreatedOn,
                                                CreatedBy = supply.CreatedBy,
                                                StatusId = supply.StatusId,
                                                Deleted = supply.Deleted,
                                                MoistureContent = supply.MoistureContent,
                                                BagsOfStones = supply.BagsOfStones,
                                                NormalBags = supply.NormalBags,
                                                Quantity = supply.Quantity,
                                                Offloading = supply.Offloading,
                                                PartialAmount = partialAmount,
                                                PartiallyPaid = false,
                                                YellowBags = supply.YellowBags,
                                                Approved = supply.Approved,
                                            };
                                            var supplyId = _supplyService.UpdateSupply(supplyObject, userId);
                                            var accountActivityObject = new AccountTransactionActivity()
                                            {
                                                AspNetUserId = supply.SupplierId,
                                                Amount = requistion.Amount,
                                                Notes = requistion.Description,
                                                Action = "-",
                                                BranchId = requistion.BranchId,
                                                TransactionSubTypeId = supplyTransactionSubTypeId,
                                                SectorId = sectorId,
                                                CreatedOn = DateTime.Now,
                                            };
                                            var accountActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountActivityObject, userId);
                                            var cashSupply = new Cash()
                                            {

                                                Amount = requistion.Amount,
                                                Notes = requistion.Description,
                                                Action = "-",
                                                BranchId = requistion.BranchId,
                                                TransactionSubTypeId = debitId,
                                                SectorId = sectorId,
                                                RequistionCategoryId = requistion.RequistionCategoryId,
                                                CreatedBy = requistion.ApprovedById,

                                            };

                                            cashId = _cashService.SaveCash(cashSupply, userId);


                                        }
                                        else
                                        {
                                            requistionId = checkedCashId;
                                            return requistionId;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            int checkWithAdmin = -2;
                            //supply doesn't exist.
                            return requistionId = checkWithAdmin;
                        }
                    }
                    //labourcosts
                    else if (requistion.RequistionCategoryId == 3)
                    {
                        if (requistion.Quantity != null && requistion.BatchId != null && requistion.ActivityId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                if (requistion.CasualWorkerId != null)
                                {
                                    try
                                    {
                                        var accountTransactionActivity = new AccountTransactionActivity()
                                        {

                                            CasualWorkerId = requistion.CasualWorkerId,
                                            Amount = requistion.Amount,
                                            Notes = requistion.Description,
                                            Action = "+",
                                            BranchId = requistion.BranchId,
                                            TransactionSubTypeId = allowanceTransactionSubTypeId,
                                            SectorId = sectorId,
                                            CreatedOn = DateTime.Now,

                                        };

                                        var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                    }

                                    catch (Exception e)
                                    {
                                        var accountActivityId = -22;
                                        return requistionId = accountActivityId;
                                    }
                                }

                                var labourCost = new LabourCost()
                                {

                                    BatchId = Convert.ToInt64(requistion.BatchId),

                                    Quantity = Convert.ToInt64(requistion.Quantity),
                                    ActivityId = Convert.ToInt64(requistion.ActivityId),
                                    BranchId = requistion.BranchId,
                                    Amount = requistion.Amount,

                                    SectorId = sectorId,

                                };

                                var LabourCostId = _labourCostService.SaveLabourCost(labourCost, userId);
                                var cashLabour = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashLabour, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -5;
                            return requistionId;
                        }


                    }
                    //machine repair
                    else if (requistion.RequistionCategoryId == 4)
                    {
                        if (requistion.RepairerName != null && requistion.BatchId != null && requistion.RepairDate != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var machineRepairObject = new MachineRepair()
                                {

                                    Amount = requistion.Amount,
                                    NameOfRepair = requistion.RepairerName,
                                    DateRepaired = requistion.RepairDate,
                                    BranchId = requistion.BranchId,
                                    BatchId = Convert.ToInt64(requistion.BatchId),
                                    TransactionSubTypeId = 3,
                                    Description = requistion.Description,
                                    SectorId = 1,
                                    MachineRepairId = 0,
                                    Deleted = requistion.Deleted,
                                    CreatedBy = requistion.CreatedBy,
                                    CreatedOn = Convert.ToDateTime(requistion.CreatedOn)
                                };
                                var machineRepairId = _machineRepairService.SaveMachineRepair(machineRepairObject, userId);

                                var cashMachine = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashMachine, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -8;
                            return requistionId;
                        }
                    }
                    //allowance
                    else if (requistion.RequistionCategoryId == 5)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashAllowanceCasual = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashAllowanceCasual, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }

                    //otherexpense
                    else if (requistion.RequistionCategoryId == 6)
                    {
                        long checkedCashId = 0;
                        if (requistion.CasualWorkerId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var accountTransactionActivity = new AccountTransactionActivity()
                                {

                                    CasualWorkerId = requistion.CasualWorkerId,
                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "+",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = allowanceTransactionSubTypeId,
                                    SectorId = sectorId,
                                    CreatedOn = DateTime.Now,


                                };

                                var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);
                                var cashAllowance = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashAllowance, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {

                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var cashOther = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashOther, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }


                    }

                    //protective gears

                    else if (requistion.RequistionCategoryId == 31)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashProtective = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashProtective, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }
                    //Nssf

                    else if (requistion.RequistionCategoryId == 32)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashNssf = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashNssf, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }
                    //Ura
                    else if (requistion.RequistionCategoryId == 33)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashUra = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashUra, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }

                    //newspaper
                    else if (requistion.RequistionCategoryId == 29)
                    {
                        long checkedCashId = 0;
                        if (requistion.CasualWorkerId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var accountTransactionActivity = new AccountTransactionActivity()
                                {

                                    CasualWorkerId = requistion.CasualWorkerId,
                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "+",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = allowanceTransactionSubTypeId,
                                    SectorId = sectorId,
                                    CreatedOn = DateTime.Now,


                                };

                                var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);
                                var cashPaper = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashPaper, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {

                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var cashPaper = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashPaper, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }

                    }

                    //packaging
                    else if (requistion.RequistionCategoryId == 30)
                    {
                        long checkedCashId = 0;
                        if (requistion.CasualWorkerId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var accountTransactionActivity = new AccountTransactionActivity()
                                {

                                    CasualWorkerId = requistion.CasualWorkerId,
                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "+",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = allowanceTransactionSubTypeId,
                                    SectorId = sectorId,
                                    CreatedOn = DateTime.Now,


                                };

                                var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);
                                var cashPackaging = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashPackaging, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {

                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var cashPackaging = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashPackaging, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }

                    }

                    //capital expenses
                    else if (requistion.RequistionCategoryId == 35)
                    {
                        long checkedCashId = 0;


                        checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                        if (checkedCashId > 0)
                        {
                            var cashCapitalExpense = new Cash()
                            {

                                Amount = requistion.Amount,
                                Notes = requistion.Description,
                                Action = "-",
                                BranchId = requistion.BranchId,
                                TransactionSubTypeId = debitId,
                                SectorId = sectorId,
                                RequistionCategoryId = requistion.RequistionCategoryId,
                                CreatedBy = requistion.ApprovedById,

                            };

                            cashId = _cashService.SaveCash(cashCapitalExpense, userId);
                        }
                        else
                        {
                            return requistionId = checkedCashId;
                        }

                    }

                    //computer accessories and repair
                    else if (requistion.RequistionCategoryId == 25)
                    {
                        long checkedCashId = 0;


                        checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                        if (checkedCashId > 0)
                        {
                            var cashComputer = new Cash()
                            {

                                Amount = requistion.Amount,
                                Notes = requistion.Description,
                                Action = "-",
                                BranchId = requistion.BranchId,
                                TransactionSubTypeId = debitId,
                                SectorId = sectorId,
                                RequistionCategoryId = requistion.RequistionCategoryId,
                                CreatedBy = requistion.ApprovedById,

                            };

                            cashId = _cashService.SaveCash(cashComputer, userId);
                        }
                        else
                        {
                            return requistionId = checkedCashId;
                        }

                    }

                    //mobile money charges
                    else if (requistion.RequistionCategoryId == 24)
                    {
                        long checkedCashId = 0;


                        checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                        if (checkedCashId > 0)
                        {
                            var cashMM = new Cash()
                            {

                                Amount = requistion.Amount,
                                Notes = requistion.Description,
                                Action = "-",
                                BranchId = requistion.BranchId,
                                TransactionSubTypeId = debitId,
                                SectorId = sectorId,
                                RequistionCategoryId = requistion.RequistionCategoryId,
                                CreatedBy = requistion.ApprovedById,

                            };

                            cashId = _cashService.SaveCash(cashMM, userId);
                        }
                        else
                        {
                            return requistionId = checkedCashId;
                        }

                    }

                    //bank charges
                    else if (requistion.RequistionCategoryId == 26)
                    {
                        long checkedCashId = 0;


                        checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                        if (checkedCashId > 0)
                        {
                            var cashBankCharge = new Cash()
                            {

                                Amount = requistion.Amount,
                                Notes = requistion.Description,
                                Action = "-",
                                BranchId = requistion.BranchId,
                                TransactionSubTypeId = debitId,
                                SectorId = sectorId,
                                RequistionCategoryId = requistion.RequistionCategoryId,
                                CreatedBy = requistion.ApprovedById,

                            };

                            cashId = _cashService.SaveCash(cashBankCharge, userId);
                        }
                        else
                        {
                            return requistionId = checkedCashId;
                        }

                    }

                    //airtime& data
                    else if (requistion.RequistionCategoryId == 18)
                    {


                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {

                                    var cashAirTime = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashAirTime, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }

                    }

                    //advance
                    else if (requistion.RequistionCategoryId == 16)
                    {
                        long checkedCashId = 0;
                        if (requistion.CasualWorkerId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var accountTransactionActivity = new AccountTransactionActivity()
                                {

                                    CasualWorkerId = requistion.CasualWorkerId,
                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "+",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = allowanceTransactionSubTypeId,
                                    SectorId = sectorId,
                                    CreatedOn = DateTime.Now,


                                };

                                var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);
                                var cashAdvance = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashAdvance, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            checkedCashId = -33;

                            return requistionId = checkedCashId;

                        }
                    }

                    //utility
                    else if (requistion.RequistionCategoryId == 10)
                    {
                        long checkedCashId = 0;
                        if (requistion.UtilityCategoryId != null && requistion.RepairerName != string.Empty)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {

                                var utilityAccountTransaction = new UtilityAccount()
                                {

                                    Amount = requistion.Amount,
                                    Description = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    CreatedOn = DateTime.Now,
                                    InvoiceNumber = requistion.RepairerName,
                                    UtilityCategoryId = Convert.ToInt64(requistion.UtilityCategoryId),

                                };

                                var accountTransactionActivityId = _utilityAccountService.SaveUtilityAccount(utilityAccountTransaction, userId);
                                var cashUtility = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashUtility, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -44;

                            return requistionId;

                        }


                    }

                    //financail account
                   else if (requistion.RequistionCategoryId == 34)
                   ////else if (requistion.RequistionCategoryId == 30013)
                    {
                        long checkedCashId = 0;
                        if (requistion.FinancialAccountId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {

                                var financialAccountTransaction = new FinancialAccountTransaction()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    CreatedOn = DateTime.Now,

                                    FinancialAccountId = Convert.ToInt64(requistion.FinancialAccountId),

                                };

                               
                                var accountTransactionActivityId = _financialAccountTransactionService.SaveFinancialAccountTransaction(financialAccountTransaction, userId);
                                if(accountTransactionActivityId < 0)
                                {
                                    requistionId = -55;
                                        return requistionId;
                                }
                                var cashFinancial = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashFinancial, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -44;

                            return requistionId;

                        }


                    }

                    //marketing
                    else if (requistion.RequistionCategoryId == 19)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashMarketing = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashMarketing, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }

                    }
                    //offloading 
                    else if (requistion.RequistionCategoryId == 13)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {

                                try
                                {
                                    var cashOffLoading = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashOffLoading, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }
                    //spare parts
                    else if (requistion.RequistionCategoryId == 17)
                    {
                        long checkedCashId = 0;
                        checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                        if (requistion.CasualWorkerId != null)
                        {
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                var cashSpare = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashSpare, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }

                        }
                        else
                        {

                            requistionId = -9;
                            return requistionId;
                        }




                    }
                    // bank deposits
                    else if (requistion.RequistionCategoryId == 10007)
                    {
                        long checkedCashId = 0;
                        if (requistion.BankId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                var bankTransaction = new BankTransaction()
                                {
                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "+",
                                    SectorId = sectorId,
                                    BranchId = requistion.BranchId,
                                    CreatedOn = DateTime.Now,
                                    BankId = Convert.ToInt64(requistion.BankId),
                                    TransactionSubTypeId = bankTransactionSubTypeId,

                                };

                                var bankTransactionId = _bankTransactionService.SaveBankTransaction(bankTransaction, userId);
                                var cashBank = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashBank, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -44;

                            return requistionId = checkedCashId;

                        }


                    }
                    //stationery
                    else if (requistion.RequistionCategoryId == 23)
                    {
                        long checkedCashId = 0;
                        if (requistion.CasualWorkerId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                var cashStationery = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashStationery, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }



                    }
                    // parking fee
                    else if (requistion.RequistionCategoryId == 21)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashParking = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashParking, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }

                    }
                    // commision
                    else if (requistion.RequistionCategoryId == 27)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashCommission = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashCommission, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }


                    }
                    // personal use
                    else if (requistion.RequistionCategoryId == 11)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashPersonal = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashPersonal, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }
                    // salary
                    else if (requistion.RequistionCategoryId == 28)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashSalary = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashSalary, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }
                    //foodstuff
                    else if (requistion.RequistionCategoryId == 12)
                    {

                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashFood = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashFood, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }

                    }

                    //fuel
                    else if (requistion.RequistionCategoryId == 14)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashFuel = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashFuel, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }

                    }

                    //loading
                    else if (requistion.RequistionCategoryId == 15)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashLoading = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashLoading, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }

                    // car maintanance
                    else if (requistion.RequistionCategoryId == 22)
                    {

                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashCar = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashCar, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }



                    }

                    // Lunch Allowance
                    else if (requistion.RequistionCategoryId == 36)
                    {
                        if (requistion.CasualWorkerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashLunchAllowance = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashLunchAllowance, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }

                    // out sourcer
                    else if (requistion.RequistionCategoryId == 37)
                    {
                        if (requistion.OutSourcerId != null)
                        {
                            var checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        AspNetUserId = requistion.OutSourcerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                try
                                {
                                    var cashOutSourcer = new Cash()
                                    {

                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "-",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = debitId,
                                        SectorId = sectorId,
                                        RequistionCategoryId = requistion.RequistionCategoryId,
                                        CreatedBy = requistion.ApprovedById,

                                    };

                                    cashId = _cashService.SaveCash(cashOutSourcer, userId);
                                }
                                catch (Exception e)
                                {
                                    var casual = -23;
                                    return requistionId = casual;
                                }

                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }
                    }
                    //transport
                    else if (requistion.RequistionCategoryId == 20)
                    {
                        long checkedCashId = 0;

                        if (requistion.CasualWorkerId != null)
                        {
                            checkedCashId = _cashService.CheckIfBranchHasEnoughCash(requistion.BranchId, requistion.Amount, "-");
                            if (checkedCashId > 0)
                            {
                                try
                                {
                                    var accountTransactionActivity = new AccountTransactionActivity()
                                    {

                                        CasualWorkerId = requistion.CasualWorkerId,
                                        Amount = requistion.Amount,
                                        Notes = requistion.Description,
                                        Action = "+",
                                        BranchId = requistion.BranchId,
                                        TransactionSubTypeId = allowanceTransactionSubTypeId,
                                        SectorId = sectorId,
                                        CreatedOn = DateTime.Now,

                                    };

                                    var accountTransactionActivityId = _accountTransactionActivityService.SaveAccountTransactionActivity(accountTransactionActivity, userId);

                                }

                                catch (Exception e)
                                {
                                    var accountActivityId = -22;
                                    return requistionId = accountActivityId;
                                }
                                var cashTransport = new Cash()
                                {

                                    Amount = requistion.Amount,
                                    Notes = requistion.Description,
                                    Action = "-",
                                    BranchId = requistion.BranchId,
                                    TransactionSubTypeId = debitId,
                                    SectorId = sectorId,
                                    RequistionCategoryId = requistion.RequistionCategoryId,
                                    CreatedBy = requistion.ApprovedById,

                                };

                                cashId = _cashService.SaveCash(cashTransport, userId);
                            }
                            else
                            {
                                return requistionId = checkedCashId;
                            }
                        }
                        else
                        {
                            requistionId = -9;
                            return requistionId;
                        }



                    }

                    //requistionId = this._dataService.SaveRequistion(requistionDTO, userId);
                    UpdateRequistion(requistion.RequistionId, requistionStatusIdComplete, requistion.ApprovedById);

                    var document = new Document()
                    {
                        DocumentId = 0,

                        UserId = requistion.CreatedBy,
                        DocumentCategoryId = paymentVoucherId,
                        Amount = requistion.Amount,
                        BranchId = requistion.BranchId,
                        ItemId = requistion.RequistionId,
                        Description = requistion.Description,
                        AmountInWords = requistion.AmountInWords,

                    };

                    var documentId = _documentService.SaveDocument(document, userId);


                    requistionId = requistion.RequistionId;
                }
           }
           else
           {
           //    SendEmail(requistionDTO, userId);
                requistionId = this._dataService.SaveRequistion(requistionDTO, userId);
           }
          
           return requistionId;
                      
        }

       public  IEnumerable<RequistionCategory> GetAllRequistionCategories()
        {
            var results = this._dataService.GetAllRequistionCategories();
            return MapEFToModel(results);
        }

        public void SendEmail(RequistionDTO requistion,string userId)
        {
            DateTime createdOn = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            string strNewPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["RequistionEmail"]);
            using (StreamReader sr = new StreamReader(strNewPath))
            {
                while (!sr.EndOfStream)
                {
                    sb.Append(sr.ReadLine());
                }
            }



            string body = sb.ToString().Replace("#REQUISTIONNUMBER#", requistion.RequistionNumber);
            body = body.Replace("#DESCRIPTION#", requistion.Description);
            body = body.Replace("#CREATEDBY#", userId);
            body = body.Replace("#CREATEDON#", Convert.ToString(createdOn));

            Helpers.Email email = new Helpers.Email();
            email.MailBodyHtml = body;
            email.MailToAddress = ConfigurationManager.AppSettings["administrator-email"];
            email.MailFromAddress = ConfigurationManager.AppSettings["EmailAddressFrom"];
            email.Subject = ConfigurationManager.AppSettings["requistion_email_subject"];
            email.SendMail();
            logger.Debug("Email sent");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequistionId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long requistionId, string userId)
        {
            _dataService.MarkAsDeleted(requistionId, userId);
        }

        private void UpdateRequistion(long requistionId, long statusId, string userId)
        {
            var requistion = _dataService.GetRequistion(requistionId);
            if (requistion !=null)
            {
                    _dataService.UpdateRequistionWithCompletedStatus(requistionId, statusId, userId);
                
            }

        }
      
        #region Mapping Methods

        private IEnumerable<Requistion> MapEFToModel(IEnumerable<EF.Models.Requistion> data)
        {
            var list = new List<Requistion>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        private IEnumerable<RequistionViewModel> MapRequistionEFToRequistionViewModel(IEnumerable<EF.Models.Requistion> data)
        {
            var list = new List<RequistionViewModel>();
            foreach (var result in data)
            {
                list.Add(MapRequistionEFToRequistionViewModel(result));
            }
            return list;
        }
        /// <summary>
        /// Maps Requistion EF object to Requistion Model Object and
        /// returns the Requistion model object.
        /// </summary>
        /// <param name="result">EF Requistion object to be mapped.</param>
        /// <returns>Requistion Model Object.</returns>
        public Requistion MapEFToModel(EF.Models.Requistion data)
        {
            string statusName = string.Empty;
            long documentId = 0;
            if (data != null)
            {
                //var document = _documentService.GetDocumentForAParticularItem(data.RequistionId);
                var document = _documentService.GetDocumentForAParticularItemAndCategory(data.RequistionId,paymentVoucherId);
                if (document != null)
                {
                    documentId = document.DocumentId;
                }
                if (data.Status != null)
                {
                    if (data.Status.StatusId == 2)
                    {
                        statusName = "Approved";
                    }
                    else
                    {
                        statusName = data.Status.Name;
                    }

                }
                var requistion = new Requistion()
                {
                    RequistionId = data.RequistionId,
                    ApprovedById = data.ApprovedById,
                    StatusId = data.StatusId,
                    Amount = data.Amount,
                    Response = data.Response,
                    BranchId = data.BranchId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    StatusName = statusName,
                    ApprovedByName = _userService.GetUserFullName(data.AspNetUser),
                    RequistionNumber = data.RequistionNumber,
                    Description = String.Concat(" ", data.Description, data.Supply != null ? data.Supply.WeightNoteNumber : " "),
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Approved = Convert.ToBoolean(data.Approved),
                    Rejected = Convert.ToBoolean(data.Rejected),
                    AmountInWords = data.AmountInWords,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser1),
                    CreatedById = data.CreatedBy,
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser2),
                    DocumentId = documentId,
                     CasualWorkerId = data.CasualWorkerId,
                     BatchId = data.BatchId,
                     SupplyId = data.SupplyId,
                     ActivityId = data.ActivityId,
                     PartPayment = data.PartPayment,
                     RequistionCategoryId = data.RequistionCategoryId,
                     Quantity = data.Quantity,
                     RepairerName = data.RepairerName,
                     RepairDate = data.RepairDate,
                     BankId = data.BankId,
                     UtilityCategoryId = data.UtilityCategoryId,
                     FinancialAccountId = data.FinancialAccountId,
                     FinancialAccountName = data.FinancialAccount != null ? data.FinancialAccount.Name : "",
                     BatchName = data.Batch != null ? data.Batch.Name : "",
                     WeightNoteNumber = data.Supply != null ? data.Supply.WeightNoteNumber : "",
                     ActivityName = data.Activity != null ? data.Activity.Name : "",
                     CasualWorkerName = data.CasualWorker != null ? data.CasualWorker.FirstName + " " + data.CasualWorker.LastName : "",
                     RequistionCategoryName = data.RequistionCategory != null ? data.RequistionCategory.Name: "",
                     BankName = data.Bank != null ? data.Bank.Name : "",
                     UtilityCategoryName = data.UtilityCategory != null ? data.UtilityCategory.Name : "",
                     OutSourcerId = data.OutSourcerId,
                     OutSourcerName = _userService.GetUserFullName(data.AspNetUser31),

                };
                return requistion;
            }
            return null;
        }

        public RequistionViewModel MapRequistionEFToRequistionViewModel(EF.Models.Requistion data)
        {
            string statusName = string.Empty;
            long documentId = 0;
            if (data != null)
            {                
                var document = _documentService.GetDocumentForAParticularItemAndCategory(data.RequistionId, paymentVoucherId);
                if (document != null)
                {
                    documentId = document.DocumentId;
                }
                if (data.Status != null)
                {
                    if (data.Status.StatusId == 2)
                    {
                        statusName = "Approved";
                    }
                    else
                    {
                        statusName = data.Status.Name;
                    }

                }
                var requistion = new RequistionViewModel()
                {
                    RequistionId = data.RequistionId,
                    ApprovedById = data.ApprovedById,
                   
                    Amount = data.Amount,
                    Response = data.Response,
                   
                    StatusName = statusName,
                    ApprovedByName = _userService.GetUserFullName(data.AspNetUser),
                    RequistionNumber = data.RequistionNumber,
                    Description = String.Concat(" ", data.Description, data.Supply != null ? data.Supply.WeightNoteNumber : " "),
                   
                    AmountInWords = data.AmountInWords,
                   
                   
                    DocumentId = documentId,
                   
                };
                return requistion;
            }
            return null;
        }


        private IEnumerable<RequistionCategory> MapEFToModel(IEnumerable<EF.Models.RequistionCategory> data)
        {
            var list = new List<RequistionCategory>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        
        public RequistionCategory MapEFToModel(EF.Models.RequistionCategory data)
        {
           
            if (data != null)
            {
                var requistionCategory = new RequistionCategory()
                {
                    RequistionCategoryId = data.RequistionCategoryId,
                    Name = data.Name,
                    TimeStamp = data.TimeStamp,
                   
                };
                return requistionCategory;
            }
            return null;
        }

        #endregion
    }
}
