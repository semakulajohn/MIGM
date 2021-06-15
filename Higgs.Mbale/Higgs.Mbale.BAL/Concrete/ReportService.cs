using System;
using System.Collections.Generic;
using System.Linq;

using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using System.Configuration;
using log4net;
using Higgs.Mbale.Models.ViewModel;
using Higgs.Mbale.Models.ViewModel.consolidated;

namespace Higgs.Mbale.BAL.Concrete
{
    public class ReportService : IReportService
    {
        private long depositTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["Deposit"]);
        private long discountTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["Discount"]);
        private long recoveryTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["Recovery"]);

        ILog logger = log4net.LogManager.GetLogger(typeof(ReportService));
        private IReportDataService _dataService;
        private IUserService _userService;
        private ITransactionService _transactionService;
        private ISupplyService _supplyService;
        private IRiceInputService _riceInputService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private IBatchService _batchService;
        private IDeliveryService _deliveryService;
        private ICashService _cashService;
        private IOrderService _orderService;
        private ILabourCostService _labourCostService;
        private IOtherExpenseService _otherExpenseService;
        private IFactoryExpenseService _factoryExpenseService;
        private IBatchOutPutService _batchOutPutService;
        private IFlourTransferService _flourTransferService;
        private IMachineRepairService _machineRepairService;
        private IUtilityService _utilityService;
        private ICreditorService _creditorService;
        private ICashTransferService _cashTransferService;
        private ICashSaleService _cashSaleService;
        private IBuveraTransferService _buveraTransferService;
        private IUtilityAccountService _utilityAccountService;
        private IDebtorService _debtorService;
        private IStoreService _storeService;
        private IBuveraService _buveraService;
        private IBranchService _branchService;
        private IWeightLossService _weightLossService;
        private IOutSourcerOutPutService _outSourcerOutPutService;



        public ReportService(IReportDataService dataService, IUserService userService, ITransactionService transactionService,
            ISupplyService supplyService, IAccountTransactionActivityService accountTransactionActivityService,
            IBatchService batchService, IDeliveryService deliveryService, ICashService cashService, IOrderService orderService,
            ILabourCostService labourCostService, IOtherExpenseService otherExpenseService, IFactoryExpenseService factoryExpenseService,
            IBatchOutPutService batchOutPutService, IFlourTransferService flourTransferService, IMachineRepairService machineRepairService,
            IUtilityService utilityService, ICreditorService creditorService, ICashTransferService cashTransferService,
            ICashSaleService cashSaleService, IBuveraTransferService buveraTransferService, IUtilityAccountService utilityAccountService,
            IWeightLossService weightLossService,IOutSourcerOutPutService outSourcerOutPutService,
            IDebtorService debtorService, IStoreService storeService, IBuveraService buveraService, IBranchService branchService,
            IRiceInputService riceInputService
            )
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionService = transactionService;
            this._supplyService = supplyService;
            this._accountTransactionActivityService = accountTransactionActivityService;
            this._batchService = batchService;
            this._deliveryService = deliveryService;
            this._cashService = cashService;
            this._orderService = orderService;
            this._labourCostService = labourCostService;
            this._otherExpenseService = otherExpenseService;
            this._factoryExpenseService = factoryExpenseService;
            this._batchOutPutService = batchOutPutService;
            this._flourTransferService = flourTransferService;
            this._machineRepairService = machineRepairService;
            this._utilityService = utilityService;
            this._creditorService = creditorService;
            this._cashTransferService = cashTransferService;
            this._cashSaleService = cashSaleService;
            this._buveraTransferService = buveraTransferService;
            this._utilityAccountService = utilityAccountService;
            this._debtorService = debtorService;
            this._storeService = storeService;
            this._buveraService = buveraService;
            this._weightLossService = weightLossService;
            this._branchService = branchService;
            this._outSourcerOutPutService = outSourcerOutPutService;
            this._riceInputService = riceInputService;
        }

        #region transactions
        public IEnumerable<Transaction> GenerateTransactionCurrentMonthReport()
        {
            var results = this._dataService.GenerateTransactionCurrentMonthReport();
            var transactionList = _transactionService.MapEFToModel(results.ToList());
            return transactionList;
        }

        public IEnumerable<Transaction> GenerateTransactionCurrentWeekReport()
        {
            var results = this._dataService.GenerateTransactionCurrentWeekReport();
            var transactionList = _transactionService.MapEFToModel(results.ToList());
            return transactionList;
        }


        public IEnumerable<Transaction> GenerateTransactionTodaysReport()
        {
            var results = this._dataService.GenerateTransactionTodaysReport();
            var transactionList = _transactionService.MapEFToModel(results.ToList());
            return transactionList;
        }

        public IEnumerable<Transaction> GetAllTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate)
        {
            var results = this._dataService.GetAllTransactionsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate);
            var transactionList = _transactionService.MapEFToModel(results.ToList());
            return transactionList;
        }


        #endregion

        #region accountTransactions
        public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentMonthReport()
        {
            var results = this._dataService.GenerateAccountTransactionCurrentMonthReport();
            var accountTransactionList = _accountTransactionActivityService.MapEFToModel(results.ToList());
            return accountTransactionList;
        }

        public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionTodaysReport()
        {
            var results = this._dataService.GenerateAccountTransactionTodaysReport();
            var accountTransactionList = _accountTransactionActivityService.MapEFToModel(results.ToList());
            return accountTransactionList;
        }

        public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentWeekReport()
        {

            var results = this._dataService.GenerateAccountTransactionCurrentWeekReport();
            var accountTransactionList = _accountTransactionActivityService.MapEFToModel(results.ToList());
            return accountTransactionList;
        }


        public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
        {
            var results = this._dataService.GetAllAccountTransactionsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId);
            var accountTransactionActivityList = _accountTransactionActivityService.MapEFToModel(results.ToList());
            return accountTransactionActivityList;
        }

        #endregion


        #region supplies
        #region web
        public SupplyReportViewModel GenerateSupplyCurrentMonthReport()
        {
            var results = this._dataService.GenerateSupplyCurrentMonthReport();
            var supplyList = _supplyService.MapEFToModel(results.ToList());

            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }

        public SupplyReportViewModel GenerateSupplyCurrentWeekReport()
        {
            var results = this._dataService.GenerateSupplyCurrentWeekReport();
            var supplyList = _supplyService.MapEFToModel(results.ToList());

            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }


        public SupplyReportViewModel GenerateSupplyTodaysReport()
        {
            var results = this._dataService.GenerateSupplyTodaysReport();
            var supplyList = _supplyService.MapEFToModel(results.ToList());

            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }

        public SupplyReportViewModel GetAllSuppliesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
        {
            var results = this._dataService.GetAllSuppliesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());


            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }

        //public ConsolidatedSupplyViewModel GetConsolidatedSuppliesForAParticularDate(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
        //{
        //    var results = this._dataService.GetAllSuppliesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId);
        //    var supplyList = _supplyService.MapEFToModel(results.ToList());

        //    var branches = _branchService.GetAllBranches();
        //    var distinctSupplies = results.GroupBy(g => g.BranchId).Select(o => o.First()).ToList();

        //    List<Supply> supplies = new List<Supply>();
        //    var consolidatedSupplies = new ConsolidatedSupplyViewModel()
        //    {
        //       if()

        //    };

        // }

        public SupplyReportViewModel CalculateDifferentSupplySums(List<Supply> supplyList)
        {
            var totalAmount = Convert.ToDecimal(supplyList.Sum(d => d.Amount));
            var totalMaize = supplyList.Sum(d => d.Quantity);
            var totalStoneBags = supplyList.Sum(d => d.BagsOfStones);
            var totalNormalBags = supplyList.Sum(d => d.NormalBags);
            var totalYellowBags = Convert.ToDouble(supplyList.Sum(d => d.YellowBags));

            var supplyReport = new SupplyReportViewModel()
            {
                Supplies = supplyList,
                TotalAmount = totalAmount,
                TotalMaize = totalMaize,
                TotalNormalBags = totalNormalBags,
                TotalStoneBags = totalStoneBags,
                TotalYellowBags = totalYellowBags,
            };
            return supplyReport;
        }
        #endregion
        #region branch
        public SupplyReportViewModel GenerateSupplyCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateSupplyCurrentMonthReportForBranch(branchId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());

            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }

        public SupplyReportViewModel GenerateSupplyCurrentWeekReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateSupplyCurrentWeekReportForBranch(branchId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());

            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }

        public SupplyReportViewModel GenerateSupplyTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateSupplyTodaysReportForBranch(branchId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());

            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }

        public SupplyReportViewModel GetAllSuppliesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
        {
            var results = this._dataService.GetAllSuppliesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());

            var supplyReport = CalculateDifferentSupplySums(supplyList.ToList());
            return supplyReport;
        }

        #endregion
        #endregion

        #region supplies for a supplier
        public IEnumerable<Supply> GenerateSupplyCurrentMonthReportForAParticularSupplier(string supplierId)
        {
            var results = this._dataService.GenerateSupplyCurrentMonthReportForAParticularSupplier(supplierId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());
            return supplyList;
        }

        public IEnumerable<Supply> GenerateSupplyCurrentWeekReportForAParticularSupplier(string supplierId)
        {
            var results = this._dataService.GenerateSupplyCurrentWeekReportForAParticularSupplier(supplierId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());
            return supplyList;
        }


        public IEnumerable<Supply> GenerateSupplyTodaysReportForAParticularSupplier(string supplierId)
        {
            var results = this._dataService.GenerateSupplyTodaysReportForAParticularSupplier(supplierId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());
            return supplyList;
        }

        public IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, string supplierId)
        {
            var results = this._dataService.GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier(lowerSpecifiedDate, upperSpecifiedDate, supplierId);
            var supplyList = _supplyService.MapEFToModel(results.ToList());
            return supplyList;
        }
        #endregion

        #region batches
        #region web
        public BatchReportViewModel GenerateBatchCurrentMonthReport()
        {
            var results = this._dataService.GenerateBatchCurrentMonthReport();
            var batchList = _batchService.MapEFToModel(results.ToList());

            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }

        public BatchReportViewModel GenerateBatchCurrentWeekReport()
        {
            var results = this._dataService.GenerateBatchCurrentWeekReport();
            var batchList = _batchService.MapEFToModel(results.ToList());
            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }

        public BatchReportViewModel GenerateBatchTodaysReport()
        {
            var results = this._dataService.GenerateBatchTodaysReport();
            var batchList = _batchService.MapEFToModel(results.ToList());
            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }

        public BatchReportViewModel GetAllBatchesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBatchesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var batchList = _batchService.MapEFToModel(results.ToList());
            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }
        #endregion
        #region branch
        public BatchReportViewModel GenerateBatchCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateBatchCurrentMonthReportForBranch(branchId);
            var batchList = _batchService.MapEFToModel(results.ToList());
            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }

        public BatchReportViewModel GenerateBatchCurrentWeekReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateBatchCurrentWeekReportForBranch(branchId);
            var batchList = _batchService.MapEFToModel(results.ToList());
            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }

        public BatchReportViewModel GenerateBatchTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateBatchTodaysReportForBranch(branchId);
            var batchList = _batchService.MapEFToModel(results.ToList());
            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }

        public BatchReportViewModel GetAllBatchesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBatchesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var batchList = _batchService.MapEFToModel(results.ToList());
            var batchReport = CalculateDifferentBatchSums(batchList.ToList());
            return batchReport;
        }

        #endregion

        public BatchReportViewModel CalculateDifferentBatchSums(List<Batch> batchList)
        {
            var totalBrandKgs = batchList.Sum(d => d.BrandOutPut);
            var totalMaize = batchList.Sum(d => d.Quantity);
            var totalFactoryExpenses = Convert.ToDecimal(batchList.Sum(d => d.TotalFactoryExpenseCost));
            var totalFlourKgs = batchList.Sum(d => d.FlourOutPut);
            var totalLabourCosts = Convert.ToDecimal(batchList.Sum(d => d.TotalLabourCosts));
            var totalMillingBalance = Convert.ToDecimal(batchList.Sum(d => d.MillingChargeBalance));
            var totalMillingCharge = Convert.ToDecimal(batchList.Sum(d => d.MillingCharge));
            var totalBuveraCosts = Convert.ToDecimal(batchList.Sum(d => d.TotalBuveraCost));
            var totalOtherExpenses = Convert.ToDecimal(batchList.Sum(d => d.TotalOtherExpenseCost));
            var totalUtilityCosts = Convert.ToDecimal(batchList.Sum(d => d.TotalUtilityCost));
            var totalProductionCosts = Convert.ToDecimal(batchList.Sum(d => d.TotalProductionCost));
            var totalMachineCosts = Convert.ToDecimal(batchList.Sum(d => d.TotalMachineCost));



            var batchReport = new BatchReportViewModel()
            {
                Batches = batchList,
                TotalBrandKgs = totalBrandKgs,
                TotalMachineCosts = totalMachineCosts,
                TotalBuveraCosts = totalBuveraCosts,
                TotalFactoryExpenses = totalFactoryExpenses,
                TotalFlourKgs = totalFlourKgs,
                TotalMaize = totalMaize,
                TotalLabourCosts = totalLabourCosts,
                TotalMillingBalance = totalMillingBalance,
                TotalMillingCharge = totalMillingCharge,
                TotalOtherExpenseCosts = totalOtherExpenses,
                TotalProductionCosts = totalProductionCosts,
                TotalUtilityCosts = totalUtilityCosts,
            };
            return batchReport;
        }
        #endregion

        #region deliveries
        #region web
        public DeliveryReportViewModel GenerateDeliveryCurrentMonthReport()
        {
            var results = this._dataService.GenerateDeliveryCurrentMonthReport();
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        public DeliveryReportViewModel GenerateDeliveryCurrentWeekReport()
        {
            var results = this._dataService.GenerateDeliveryCurrentWeekReport();
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }


        public DeliveryReportViewModel GenerateDeliveryTodaysReport()
        {
            var results = this._dataService.GenerateDeliveryTodaysReport();
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        public DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            var results = this._dataService.GetAllDeliveriesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        public DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId)
        {
            var results = this._dataService.GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId, productId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }
        public GradeSizeTotalsViewModel GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId)
        {
            var results = this._dataService.GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId, productId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveries = GetTotalGradeSizeDeliveryQuantities(deliveryList.ToList());
            return deliveries;
        }
        #endregion
        #region branch
        public DeliveryReportViewModel GenerateDeliveryCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateDeliveryCurrentMonthReportForBranch(branchId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        public DeliveryReportViewModel GenerateDeliveryCurrentWeekReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateDeliveryCurrentWeekReportForBranch(branchId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        public DeliveryReportViewModel GenerateDeliveryTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateDeliveryTodaysReportForBranch(branchId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        public DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            var results = this._dataService.GetAllDeliveriesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        public DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId)
        {
            var results = this._dataService.GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId, productId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        #endregion

        public DeliveryReportViewModel CalculateDifferentDeliverySums(List<Delivery> deliveryList)
        {
            var totalAmount = Convert.ToDecimal(deliveryList.Sum(d => d.Amount));
            var totalQuantity = deliveryList.Sum(d => d.Quantity);

            var deliveryReport = new DeliveryReportViewModel()
            {
                Deliveries = deliveryList,
                TotalAmount = totalAmount,
                TotalQuantity = totalQuantity
            };
            return deliveryReport;
        }

        public GradeSizeTotalsViewModel GetTotalGradeSizeDeliveryQuantities(List<Delivery> deliveryList)
        {
            //quantity
            int totalSuperTenQuantity = 0, totalSuperTwoFiveQuantity = 0, totalSuperFiveZeroQuantity = 0, totalSuperHundredQuantity = 0, totalSuperFiveQuantity = 0, totalSuperOneQuantity = 0;
            int totalNumberOneTenQuantity = 0, totalNumberOneTwoFiveQuantity = 0, totalNumberOneFiveZeroQuantity = 0, totalNumberOneHundredQuantity = 0, totalNumberOneFiveQuantity = 0, totalNumberOneOneQuantity = 0;
            int totalNumberOneHalfTenQuantity = 0, totalNumberOneHalfTwoFiveQuantity = 0, totalNumberOneHalfFiveZeroQuantity = 0, totalNumberOneHalfHundredQuantity = 0, totalNumberOneHalfFiveQuantity = 0, totalNumberOneHalfOneQuantity = 0;
            int totalKabaleTenQuantity = 0, totalKabaleTwoFiveQuantity = 0, totalKabaleFiveZeroQuantity = 0, totalKabaleHundredQuantity = 0, totalKabaleFiveQuantity = 0, totalKabaleOneQuantity = 0;

            //amount
            double totalSuperTenAmount = 0, totalSuperTwoFiveAmount = 0, totalSuperFiveZeroAmount = 0, totalSuperHundredAmount = 0, totalSuperFiveAmount = 0, totalSuperOneAmount = 0;
            double totalNumberOneTenAmount = 0, totalNumberOneTwoFiveAmount = 0, totalNumberOneFiveZeroAmount = 0, totalNumberOneHundredAmount = 0, totalNumberOneFiveAmount = 0, totalNumberOneOneAmount = 0;
            double totalNumberOneHalfTenAmount = 0, totalNumberOneHalfTwoFiveAmount = 0, totalNumberOneHalfFiveZeroAmount = 0, totalNumberOneHalfHundredAmount = 0, totalNumberOneHalfFiveAmount = 0, totalNumberOneHalfOneAmount = 0;
            double totalKabaleTenAmount = 0, totalKabaleTwoFiveAmount = 0, totalKabaleFiveZeroAmount = 0, totalKabaleHundredAmount = 0, totalKabaleFiveAmount = 0, totalKabaleOneAmount = 0;

            List<long> gradesAddedToTransferReport = new List<long>();

            GradeSizeTotalsViewModel gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel();
            List<Grade> grades = new List<Grade>();
            if (deliveryList.Count() == 0)
            {
                return gradeSizeTotalsViewModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= deliveryList.Count();)
                {


                    var item = deliveryList.ElementAt(i);
                    var name = item.DeliveryId;

                    grades = item.Grades != null ? item.Grades : grades;
                    if (grades.Any())
                    {
                        //  foreach (var grade in grades)
                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToTransferReport.Contains(grade.GradeId))
                            {
                                gradesAddedToTransferReport.Add(grade.GradeId);
                            }

                            switch (grade.Value)
                            {
                                case "Super":
                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperTenAmount = totalSuperTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperTwoFiveAmount = totalSuperTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperFiveZeroAmount = totalSuperFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperHundredAmount = totalSuperHundredAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperFiveAmount = totalSuperFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;
                                case "Number 1":
                                    foreach (var denom in grade.Denominations)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneTenAmount = totalNumberOneTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneTwoFiveAmount = totalNumberOneTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneFiveZeroAmount = totalNumberOneFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHundredAmount = totalNumberOneHundredAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneFiveAmount = totalNumberOneFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneOneAmount = totalNumberOneOneAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    break;
                                case "Number  1.5":

                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfTenAmount = totalNumberOneHalfTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfTwoFiveAmount = totalNumberOneHalfTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfFiveZeroAmount = totalNumberOneHalfFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfHundredAmount = totalNumberOneHalfHundredAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfFiveAmount = totalNumberOneHalfFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfOneAmount = totalNumberOneHalfOneAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                case "NO. 1 Kabale":

                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleTenAmount = totalKabaleTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleTwoFiveAmount = totalKabaleTwoFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleFiveZeroAmount = totalKabaleFiveZeroAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleHundredAmount = totalKabaleHundredAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleFiveAmount = totalKabaleFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleOneAmount = totalKabaleOneAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                            //grades.RemoveAt(0);
                        }

                        item.Grades.Clear();
                        deliveryList.RemoveAll(r => r.DeliveryId == item.DeliveryId);

                    }

                    else
                    {

                        deliveryList.RemoveAll(r => r.DeliveryId == item.DeliveryId);

                    }

                    i = 0;
                    if (deliveryList.Count() == 0)
                    {
                        break;
                    }
                }
            }
            gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel()
            {
                //Grades = gradesAddedToTransferReport.ToList();
                TotalKabaleFiveQuantity = totalKabaleFiveQuantity,
                TotalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity,
                TotalKabaleHundredQuantity = totalKabaleHundredQuantity,
                TotalKabaleOneQuantity = totalKabaleOneQuantity,
                TotalKabaleTenQuantity = totalKabaleTenQuantity,
                TotalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity,
                TotalNumberOneFiveQuantity = totalNumberOneFiveQuantity,
                TotalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity,
                TotalNumberOneTenQuantity = totalNumberOneTenQuantity,
                TotalNumberOneOneQuantity = totalNumberOneOneQuantity,
                TotalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity,
                TotalNumberOneHundredQuantity = totalNumberOneHundredQuantity,
                TotalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity,
                TotalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity,
                TotalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity,
                TotalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity,
                TotalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity,
                TotalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity,
                TotalSuperFiveQuantity = totalSuperFiveQuantity,
                TotalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity,
                TotalSuperHundredQuantity = totalSuperHundredQuantity,
                TotalSuperOneQuantity = totalSuperOneQuantity,
                TotalSuperTenQuantity = totalSuperTenQuantity,
                TotalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity,

                TotalKabaleFiveAmount = totalKabaleFiveAmount,
                TotalKabaleFiveZeroAmount = totalKabaleFiveZeroAmount,
                TotalKabaleHundredAmount = totalKabaleHundredAmount,
                TotalKabaleOneAmount = totalKabaleOneAmount,
                TotalKabaleTenAmount = totalKabaleTenAmount,
                TotalKabaleTwoFiveAmount = totalKabaleTwoFiveAmount,
                TotalNumberOneFiveAmount = totalNumberOneFiveAmount,
                TotalNumberOneFiveZeroAmount = totalNumberOneFiveZeroAmount,
                TotalNumberOneTenAmount = totalNumberOneTenAmount,
                TotalNumberOneOneAmount = totalNumberOneOneAmount,
                TotalNumberOneTwoFiveAmount = totalNumberOneTwoFiveAmount,
                TotalNumberOneHundredAmount = totalNumberOneHundredAmount,
                TotalNumberOneHalfFiveAmount = totalNumberOneHalfFiveAmount,
                TotalNumberOneHalfFiveZeroAmount = totalNumberOneHalfFiveZeroAmount,
                TotalNumberOneHalfHundredAmount = totalNumberOneHalfHundredAmount,
                TotalNumberOneHalfOneAmount = totalNumberOneHalfOneAmount,
                TotalNumberOneHalfTenAmount = totalNumberOneHalfTenAmount,
                TotalNumberOneHalfTwoFiveAmount = totalNumberOneHalfTwoFiveAmount,
                TotalSuperFiveAmount = totalSuperFiveAmount,
                TotalSuperFiveZeroAmount = totalSuperFiveZeroAmount,
                TotalSuperHundredAmount = totalSuperHundredAmount,
                TotalSuperOneAmount = totalSuperOneAmount,
                TotalSuperTenAmount = totalSuperTenAmount,
                TotalSuperTwoFiveAmount = totalSuperTwoFiveAmount,

            };
            return gradeSizeTotalsViewModel;
        }
        #endregion
        #region rice inputs
        public RiceInputReportViewModel GetAllRiceInputsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllRiceInputsBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var riceInputList = _riceInputService.MapEFToModel(results.ToList());

            var riceInputReport = CalculateDifferentRiceInputSums(riceInputList.ToList());
            return riceInputReport;
        }

        public RiceInputReportViewModel CalculateDifferentRiceInputSums(List<RiceInput> riceInputList)
        {
            var totalAmount = Convert.ToDecimal(riceInputList.Sum(d => d.TotalAmount));
            var totalQuantity = riceInputList.Sum(d => d.TotalQuantity);

            var riceInputReport = new RiceInputReportViewModel()
            {
                RiceInputs = riceInputList,
                TotalAmount = totalAmount,
                TotalQuantity = totalQuantity
            };
            return riceInputReport;
        }

        #endregion
        #region Cash

        public IEnumerable<Cash> GenerateCashCurrentMonthReport()
        {
            var results = this._dataService.GenerateCashCurrentMonthReport();
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateCashCurrentWeekReport()
        {
            var results = this._dataService.GenerateCashCurrentWeekReport();
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }


        public IEnumerable<Cash> GenerateCashTodaysReport()
        {
            var results = this._dataService.GenerateCashTodaysReport();
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllCashBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }


        #region expenses
        public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllExpensesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public CashReportViewModel GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long requistionCategoryId)
        {
            var results = this._dataService.GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory(lowerSpecifiedDate, upperSpecifiedDate, branchId,requistionCategoryId);
            var cashList = _cashService.MapEFToModel(results.ToList());

            var expenseReport = CalculateDifferentCashSums(cashList.ToList());
            return expenseReport;
          
        }

        #endregion

        #region incomes
        public CashReportViewModel GetAllIncomesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllIncomesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());

            var incomeReport = CalculateDifferentCashSums(cashList.ToList());
            return incomeReport;
          
        }

        public IEnumerable<Cash> GenerateIncomesCurrentMonthReport()
        {
            var results = this._dataService.GenerateIncomesCurrentMonthReport();
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateIncomesTodaysReport()
        {
            var results = this._dataService.GenerateIncomesTodaysReport();
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateIncomesCurrentWeekReport()
        {

            var results = this._dataService.GenerateIncomesCurrentWeekReport();
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }
        #endregion

        #region branch
        #region expenses
        public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllExpensesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateExpensesCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateExpensesCurrentMonthReportForBranch(branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateExpensesTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateExpensesTodaysReportForBranch(branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateExpensesCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateExpensesCurrentWeekReportForBranch(branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public CashReportViewModel GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,long requistionCategoryId)
        {
            var results = this._dataService.GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory(lowerSpecifiedDate, upperSpecifiedDate, branchId,requistionCategoryId);
            var cashList = _cashService.MapEFToModel(results.ToList());

            var expenseReport = CalculateDifferentCashSums(cashList.ToList());
            return expenseReport;
        }
        #endregion

        #region incomes
        public CashReportViewModel GetAllIncomesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {

            var results = this._dataService.GetAllIncomesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());

            var incomeReport = CalculateDifferentCashSums(cashList.ToList());
            return incomeReport;
        }

        public IEnumerable<Cash> GenerateIncomesCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateIncomesCurrentMonthReportForBranch(branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateIncomesTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateIncomesTodaysReportForBranch(branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        public IEnumerable<Cash> GenerateIncomesCurrentWeekReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateIncomesCurrentWeekReportForBranch(branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        #endregion

        #region cash
        public IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllCashBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var cashList = _cashService.MapEFToModel(results.ToList());
            return cashList;
        }

        #endregion
        #endregion

        public CashReportViewModel CalculateDifferentCashSums(List<Cash> cashList)
        {
            var totalAmount = Convert.ToDecimal(cashList.Sum(d => d.Amount));
           
            var cashReport = new CashReportViewModel()
            {
                Cashs = cashList,
                TotalAmount = totalAmount,
                
            };
            return cashReport;
        }
        #endregion

        #region  Factory Expenses
        #region web
        public FactoryExpenseReportViewModel GetAllFactoryExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllFactoryExpensesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }

        public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentMonthReport()
        {
            var results = this._dataService.GenerateFactoryExpenseCurrentMonthReport();
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }

        public FactoryExpenseReportViewModel GenerateFactoryExpenseTodaysReport()
        {
            var results = this._dataService.GenerateFactoryExpenseTodaysReport();
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }

        public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentWeekReport()
        {

            var results = this._dataService.GenerateFactoryExpenseCurrentWeekReport();
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }
        #endregion
        #region branch
        public FactoryExpenseReportViewModel GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }

        public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateFactoryExpenseCurrentMonthReportForBranch(branchId);
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }

        public FactoryExpenseReportViewModel GenerateFactoryExpenseTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateFactoryExpenseTodaysReportForBranch(branchId);
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }

        public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateFactoryExpenseCurrentWeekReportForBranch(branchId);
            var factoryExpenseList = _factoryExpenseService.MapEFToModel(results.ToList());

            var factoryExpenseReport = CalculateDifferentFactoryExpenseSums(factoryExpenseList.ToList());
            return factoryExpenseReport;
        }
        #endregion
        public FactoryExpenseReportViewModel CalculateDifferentFactoryExpenseSums(List<FactoryExpense> factoryExpenseList)
        {
            var totalAmount = Convert.ToDecimal(factoryExpenseList.Sum(d => d.Amount));

            var factoryExpenseReport = new FactoryExpenseReportViewModel()
            {
                FactoryExpenses = factoryExpenseList,
                TotalAmount = totalAmount,

            };
            return factoryExpenseReport;
        }
        #endregion

        #region  Other Expenses
        #region web
        public IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllOtherExpensesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }

        public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReport()
        {
            var results = this._dataService.GenerateOtherExpenseCurrentMonthReport();
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }

        public IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReport()
        {
            var results = this._dataService.GenerateOtherExpenseTodaysReport();
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }

        public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReport()
        {
            var results = this._dataService.GenerateOtherExpenseCurrentWeekReport();
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }
        #endregion
        #region branch
        public IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }

        public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateOtherExpenseCurrentMonthReportForBranch(branchId);
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }

        public IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateOtherExpenseTodaysReportForBranch(branchId);
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }

        public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateOtherExpenseCurrentWeekReportForBranch(branchId);
            var otherExpenseList = _otherExpenseService.MapEFToModel(results.ToList());
            return otherExpenseList;
        }
        #endregion
        #endregion

        #region  batchoutputs
        #region web
        public IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBatchOutPutsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());
            return batchOutPutList;
        }

        public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReport()
        {
            var results = this._dataService.GenerateBatchOutPutCurrentMonthReport();
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());
            return batchOutPutList;
        }

        public IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReport()
        {
            var results = this._dataService.GenerateBatchOutPutTodaysReport();
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());
            return batchOutPutList;
        }

        public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReport()
        {

            var results = this._dataService.GenerateBatchOutPutCurrentWeekReport();
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());
            return batchOutPutList;
        }
        public GradeSizeTotalsViewModel GetAllFlourTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBatchOutPutsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());


            var batchOutPuts = GetTotalGradeSizeFlourOutPutQuantities(batchOutPutList.ToList());
            return batchOutPuts;

        }
        #endregion
        #region branch
        public IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());

            //get distinct batchoutputs according to batchId
            //var distinctBatchOutPuts = batchOutPutList.Where(a => a.BatchId == data.DeliveryId).GroupBy(g => g.BatchId).Select(o => o.First()).ToList();

            var distinctBatchOutPuts = batchOutPutList.GroupBy(g => g.BatchId).Select(o => o.First()).ToList();

            return batchOutPutList;
        }

        public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateBatchOutPutCurrentMonthReportForBranch(branchId);
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());
            return batchOutPutList;
        }

        public IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateBatchOutPutTodaysReportForBranch(branchId);
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());
            return batchOutPutList;
        }

        public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateBatchOutPutCurrentWeekReportForBranch(branchId);
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());
            return batchOutPutList;
        }

        public GradeSizeTotalsViewModel GetAllFlourTotalsBetweenTheSpecifiedDatesForABranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var batchOutPutList = _batchOutPutService.MapEFToModel(results.ToList());

            var batchOutPuts = GetTotalGradeSizeFlourOutPutQuantities(batchOutPutList.ToList());
            return batchOutPuts;
        }
        #endregion

        public GradeSizeTotalsViewModel GetTotalGradeSizeFlourOutPutQuantities(List<BatchOutPut> batchOutPutList)
        {
            int totalSuperTenQuantity = 0, totalSuperTwoFiveQuantity = 0, totalSuperFiveZeroQuantity = 0, totalSuperHundredQuantity = 0, totalSuperFiveQuantity = 0, totalSuperOneQuantity = 0;
            int totalNumberOneTenQuantity = 0, totalNumberOneTwoFiveQuantity = 0, totalNumberOneFiveZeroQuantity = 0, totalNumberOneHundredQuantity = 0, totalNumberOneFiveQuantity = 0, totalNumberOneOneQuantity = 0;
            int totalNumberOneHalfTenQuantity = 0, totalNumberOneHalfTwoFiveQuantity = 0, totalNumberOneHalfFiveZeroQuantity = 0, totalNumberOneHalfHundredQuantity = 0, totalNumberOneHalfFiveQuantity = 0, totalNumberOneHalfOneQuantity = 0;
            int totalKabaleTenQuantity = 0, totalKabaleTwoFiveQuantity = 0, totalKabaleFiveZeroQuantity = 0, totalKabaleHundredQuantity = 0, totalKabaleFiveQuantity = 0, totalKabaleOneQuantity = 0;

            List<long> gradesAddedToTransferReport = new List<long>();

            GradeSizeTotalsViewModel gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel();
            List<Grade> grades = new List<Grade>();

            if (batchOutPutList.Count() == 0)
            {
                return gradeSizeTotalsViewModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= batchOutPutList.Count();)
                {


                    var item = batchOutPutList.ElementAt(i);
                    var name = item.BatchOutPutId;

                    //var grades = item.Grades;
                    // grades = item.Grades;
                    grades = item.Grades != null ? item.Grades : grades;
                    if (grades.Any() || grades.Count() != 0)
                    {

                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToTransferReport.Contains(grade.GradeId))
                            {
                                gradesAddedToTransferReport.Add(grade.GradeId);
                            }

                            switch (grade.Value)
                            {
                                case "Super":
                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;
                                case "Number 1":
                                    foreach (var denom in grade.Denominations)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    break;
                                case "Number  1.5":

                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                case "NO. 1 Kabale":

                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }

                        }
                        item.Grades.Clear();
                        batchOutPutList.RemoveAll(r => r.BatchOutPutId == item.BatchOutPutId);
                    }
                    else
                    {
                        //item.Grades.Clear();
                        batchOutPutList.RemoveAll(r => r.BatchOutPutId == item.BatchOutPutId);
                    }



                    i = 0;
                    if (batchOutPutList.Count() == 0)
                    {
                        break;
                    }
                }
            }
            gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel()
            {
                //Grades = gradesAddedToTransferReport.ToList();
                TotalKabaleFiveQuantity = totalKabaleFiveQuantity,
                TotalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity,
                TotalKabaleHundredQuantity = totalKabaleHundredQuantity,
                TotalKabaleOneQuantity = totalKabaleOneQuantity,
                TotalKabaleTenQuantity = totalKabaleTenQuantity,
                TotalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity,
                TotalNumberOneFiveQuantity = totalNumberOneFiveQuantity,
                TotalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity,
                TotalNumberOneTenQuantity = totalNumberOneTenQuantity,
                TotalNumberOneOneQuantity = totalNumberOneOneQuantity,
                TotalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity,
                TotalNumberOneHundredQuantity = totalNumberOneHundredQuantity,
                TotalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity,
                TotalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity,
                TotalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity,
                TotalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity,
                TotalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity,
                TotalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity,
                TotalSuperFiveQuantity = totalSuperFiveQuantity,
                TotalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity,
                TotalSuperHundredQuantity = totalSuperHundredQuantity,
                TotalSuperOneQuantity = totalSuperOneQuantity,
                TotalSuperTenQuantity = totalSuperTenQuantity,
                TotalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity,
            };
            return gradeSizeTotalsViewModel;
        }

        #endregion

        #region  LabourCosts
        #region web
        public LabourCostReportViewModel GetAllLabourCostsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllLabourCostsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());
            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }

        public LabourCostReportViewModel GenerateLabourCostCurrentMonthReport()
        {
            var results = this._dataService.GenerateLabourCostCurrentMonthReport();
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());
            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }

        public LabourCostReportViewModel GenerateLabourCostTodaysReport()
        {
            var results = this._dataService.GenerateLabourCostTodaysReport();
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());
            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }

        public LabourCostReportViewModel GenerateLabourCostCurrentWeekReport()
        {

            var results = this._dataService.GenerateLabourCostCurrentWeekReport();
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());

            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }
        #endregion
        #region branch
        public LabourCostReportViewModel GetAllLabourCostsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllLabourCostsBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());
            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }

        public LabourCostReportViewModel GenerateLabourCostCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateLabourCostCurrentMonthReportForBranch(branchId);
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());

            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }

        public LabourCostReportViewModel GenerateLabourCostTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateLabourCostTodaysReportForBranch(branchId);
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());
            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }

        public LabourCostReportViewModel GenerateLabourCostCurrentWeekReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateLabourCostCurrentWeekReportForBranch(branchId);
            var labourCostList = _labourCostService.MapEFToModel(results.ToList());
            var labourCostReport = CalculateDifferentLabourCostSums(labourCostList.ToList());

            return labourCostReport;
        }

        #endregion

        public LabourCostReportViewModel CalculateDifferentLabourCostSums(List<LabourCost> labourCostList)
        {
            var totalAmount = Convert.ToDecimal(labourCostList.Sum(d => d.Amount));


            var labourCostReport = new LabourCostReportViewModel()
            {
                LabourCosts = labourCostList,
                TotalAmount = totalAmount,

            };
            return labourCostReport;
        }

        #endregion

        #region  MachineRepair
        #region web
        public IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllMachineRepairsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }

        public IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReport()
        {
            var results = this._dataService.GenerateMachineRepairCurrentMonthReport();
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }

        public IEnumerable<MachineRepair> GenerateMachineRepairTodaysReport()
        {
            var results = this._dataService.GenerateMachineRepairTodaysReport();
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }

        public IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReport()
        {

            var results = this._dataService.GenerateMachineRepairCurrentWeekReport();
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }
        #endregion
        #region branch
        public IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }

        public IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateMachineRepairCurrentMonthReportForBranch(branchId);
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }

        public IEnumerable<MachineRepair> GenerateMachineRepairTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateMachineRepairTodaysReportForBranch(branchId);
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }

        public IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateMachineRepairCurrentWeekReportForBranch(branchId);
            var machineRepairList = _machineRepairService.MapEFToModel(results.ToList());
            return machineRepairList;
        }

        #endregion
        #endregion

        #region  Utility
        public IEnumerable<Utility> GetAllUtilitiesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllUtilitiesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var utilityList = _utilityService.MapEFToModel(results.ToList());
            return utilityList;
        }

        public IEnumerable<Utility> GenerateUtilityCurrentMonthReport()
        {
            var results = this._dataService.GenerateUtilityCurrentMonthReport();
            var utilityList = _utilityService.MapEFToModel(results.ToList());
            return utilityList;
        }

        public IEnumerable<Utility> GenerateUtilityTodaysReport()
        {
            var results = this._dataService.GenerateUtilityTodaysReport();
            var utilityList = _utilityService.MapEFToModel(results.ToList());
            return utilityList;
        }

        public IEnumerable<Utility> GenerateUtilityCurrentWeekReport()
        {

            var results = this._dataService.GenerateUtilityCurrentWeekReport();
            var utilityList = _utilityService.MapEFToModel(results.ToList());
            return utilityList;
        }

        #endregion


        #region  FlourTransfer
        #region web
        public FlourTransferReportViewModel GetAllFlourTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status)
        {
            var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, status);
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
            var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
            return flourTransferReport;
        }

        public FlourTransferReportTotalModel GetAllFlourTransferTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status)
        {
            var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, status);
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());

            var flourTransers = GetTotalGradeSizeQuantities(flourTransferList.ToList());
            return flourTransers;
        }
        public FlourTransferReportViewModel GenerateFlourTransferCurrentMonthReport()
        {
            var results = this._dataService.GenerateFlourTransferCurrentMonthReport();
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
            var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
            return flourTransferReport;
        }

        public FlourTransferReportViewModel GenerateFlourTransferTodaysReport()
        {
            var results = this._dataService.GenerateFlourTransferTodaysReport();
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
            var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
            return flourTransferReport;
        }

        public FlourTransferReportViewModel GenerateFlourTransferCurrentWeekReport()
        {

            var results = this._dataService.GenerateFlourTransferCurrentWeekReport();
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());

            var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
            return flourTransferReport;
        }
        #endregion
        #region branch
        public FlourTransferReportViewModel GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position)
        {
            if ((position != string.Empty || position != null) && position == "Transfered")
            {
                var storeId = _storeService.GetAStoreForAParticularBranch(branchId);
                if (storeId != 0)
                {
                    var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, storeId, status, position);
                    var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
                    var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
                    return flourTransferReport;
                }
            }
            else if ((position != string.Empty || position != null) && position == "Received")
            {
                var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
                var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
                return flourTransferReport;
            }
            else
            {
                var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
                var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
                return flourTransferReport;
            }
            return null;
        }

        public FlourTransferReportViewModel GenerateFlourTransferCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateFlourTransferCurrentMonthReportForBranch(branchId);
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
            var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
            return flourTransferReport;
        }

        public FlourTransferReportViewModel GenerateFlourTransferTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateFlourTransferTodaysReportForBranch(branchId);
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
            var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
            return flourTransferReport;
        }

        public FlourTransferReportViewModel GenerateFlourTransferCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateFlourTransferCurrentWeekReportForBranch(branchId);
            var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
            var flourTransferReport = CalculateDifferentFlourTransferSums(flourTransferList.ToList());
            return flourTransferReport;
        }

        public FlourTransferReportTotalModel GetAllFlourTransferTotalsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position)
        {
            if ((position != string.Empty || position != null) && position == "Transfered")
            {
                var storeId = _storeService.GetAStoreForAParticularBranch(branchId);
                if (storeId != 0)
                {
                    var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, storeId, status, position);
                    var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
                    var flourTransferReport = GetTotalGradeSizeQuantities(flourTransferList.ToList());
                    return flourTransferReport;
                }
            }
            else if ((position != string.Empty || position != null) && position == "Received")
            {
                var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
                var flourTransferReport = GetTotalGradeSizeQuantities(flourTransferList.ToList());
                return flourTransferReport;
            }
            else
            {
                var results = this._dataService.GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var flourTransferList = _flourTransferService.MapEFToModel(results.ToList());
                var flourTransferReport = GetTotalGradeSizeQuantities(flourTransferList.ToList());
                return flourTransferReport;
            }
            return null;
        }


        #endregion

        public FlourTransferReportViewModel CalculateDifferentFlourTransferSums(List<FlourTransfer> flourTransferList)
        {
            var totalQuantity = flourTransferList.Sum(d => d.TotalQuantity);

            var flourTransferReport = new FlourTransferReportViewModel()
            {
                FlourTransfers = flourTransferList,
                TotalQuantity = totalQuantity,

            };



            return flourTransferReport;
        }



        public FlourTransferReportTotalModel GetTotalGradeSizeQuantities(List<FlourTransfer> flourList)
        {
            int totalSuperTenQuantity = 0, totalSuperTwoFiveQuantity = 0, totalSuperFiveZeroQuantity = 0, totalSuperHundredQuantity = 0, totalSuperFiveQuantity = 0, totalSuperOneQuantity = 0;
            int totalNumberOneTenQuantity = 0, totalNumberOneTwoFiveQuantity = 0, totalNumberOneFiveZeroQuantity = 0, totalNumberOneHundredQuantity = 0, totalNumberOneFiveQuantity = 0, totalNumberOneOneQuantity = 0;
            int totalNumberOneHalfTenQuantity = 0, totalNumberOneHalfTwoFiveQuantity = 0, totalNumberOneHalfFiveZeroQuantity = 0, totalNumberOneHalfHundredQuantity = 0, totalNumberOneHalfFiveQuantity = 0, totalNumberOneHalfOneQuantity = 0;
            int totalKabaleTenQuantity = 0, totalKabaleTwoFiveQuantity = 0, totalKabaleFiveZeroQuantity = 0, totalKabaleHundredQuantity = 0, totalKabaleFiveQuantity = 0, totalKabaleOneQuantity = 0;

            List<long> gradesAddedToTransferReport = new List<long>();

            FlourTransferReportTotalModel flourTransferReportTotalModel = new FlourTransferReportTotalModel();
            List<Grade> grades = new List<Grade>();
            if (flourList.Count() == 0)
            {
                return flourTransferReportTotalModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= flourList.Count();)
                {

                    //}
                    //foreach (var item in flourTransferList)
                    //{
                    var item = flourList.ElementAt(i);
                    var name = item.FlourTransferId;

                    grades = item.Grades != null ? item.Grades : grades;
                    if (grades.Any())
                    {
                        //  foreach (var grade in grades)
                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToTransferReport.Contains(grade.GradeId))
                            {
                                gradesAddedToTransferReport.Add(grade.GradeId);
                            }

                            switch (grade.Value)
                            {
                                case "Super":
                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;
                                case "Number 1":
                                    foreach (var denom in grade.Denominations)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    break;
                                case "Number  1.5":

                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                case "NO. 1 Kabale":

                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                            //grades.RemoveAt(0);
                        }
                        item.Grades.Clear();
                        flourList.RemoveAll(r => r.FlourTransferId == item.FlourTransferId);
                    }

                    else
                    {
                        flourList.RemoveAll(r => r.FlourTransferId == item.FlourTransferId);
                    }

                    i = 0;
                    if (flourList.Count() == 0)
                    {
                        break;
                    }
                }
            }
            flourTransferReportTotalModel = new FlourTransferReportTotalModel()
            {
                //Grades = gradesAddedToTransferReport.ToList();
                TotalKabaleFiveQuantity = totalKabaleFiveQuantity,
                TotalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity,
                TotalKabaleHundredQuantity = totalKabaleHundredQuantity,
                TotalKabaleOneQuantity = totalKabaleOneQuantity,
                TotalKabaleTenQuantity = totalKabaleTenQuantity,
                TotalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity,
                TotalNumberOneFiveQuantity = totalNumberOneFiveQuantity,
                TotalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity,
                TotalNumberOneTenQuantity = totalNumberOneTenQuantity,
                TotalNumberOneOneQuantity = totalNumberOneOneQuantity,
                TotalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity,
                TotalNumberOneHundredQuantity = totalNumberOneHundredQuantity,
                TotalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity,
                TotalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity,
                TotalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity,
                TotalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity,
                TotalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity,
                TotalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity,
                TotalSuperFiveQuantity = totalSuperFiveQuantity,
                TotalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity,
                TotalSuperHundredQuantity = totalSuperHundredQuantity,
                TotalSuperOneQuantity = totalSuperOneQuantity,
                TotalSuperTenQuantity = totalSuperTenQuantity,
                TotalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity,
            };
            return flourTransferReportTotalModel;
        }
        #endregion

        #region orders
        #region web
        public IEnumerable<Order> GenerateOrderCurrentMonthReport()
        {
            var results = this._dataService.GenerateOrderCurrentMonthReport();
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }

        public IEnumerable<Order> GenerateOrderCurrentWeekReport()
        {
            var results = this._dataService.GenerateOrderCurrentWeekReport();
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }


        public IEnumerable<Order> GenerateOrderTodaysReport()
        {
            var results = this._dataService.GenerateOrderTodaysReport();
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }

        public IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            var results = this._dataService.GetAllOrdersBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId);
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }
        #endregion
        #region branch
        public IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            var results = this._dataService.GetAllOrdersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId);
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }

        public IEnumerable<Order> GenerateOrderCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateOrderCurrentMonthReportForBranch(branchId);
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }

        public IEnumerable<Order> GenerateOrderTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateOrderTodaysReportForBranch(branchId);
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }

        public IEnumerable<Order> GenerateOrderCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateOrderCurrentWeekReportForBranch(branchId);
            var orderList = _orderService.MapEFToModel(results.ToList());
            return orderList;
        }
        #endregion
        #endregion

        #region casual accounttransactions


        public IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId)
        {
            var results = this._dataService.GetAllCasualAccountTransactionsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId);
            var accountTransactionActivityList = _accountTransactionActivityService.MapEFToModel(results.ToList());
            return accountTransactionActivityList;
        }

        #region branch
        public IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId)
        {
            var results = this._dataService.GetAllCasualAccountTransactionsBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId);
            var accountTransactionActivityList = _accountTransactionActivityService.MapEFToModel(results.ToList());
            return accountTransactionActivityList;
        }

        #endregion

        #endregion

        #region  CashSale
        public CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllCashSalesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());

            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public CashSaleReportViewModel GenerateCashSaleCurrentMonthReport()
        {
            var results = this._dataService.GenerateCashSaleCurrentMonthReport();
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());
            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public CashSaleReportViewModel GenerateCashSaleTodaysReport()
        {
            var results = this._dataService.GenerateCashSaleTodaysReport();
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());
            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public CashSaleReportViewModel GenerateCashSaleCurrentWeekReport()
        {

            var results = this._dataService.GenerateCashSaleCurrentWeekReport();
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());
            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId)
        {
            var results = this._dataService.GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(lowerSpecifiedDate, upperSpecifiedDate, branchId, productId);
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());
            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public GradeSizeTotalsViewModel GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId)
        {
            var results = this._dataService.GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(lowerSpecifiedDate, upperSpecifiedDate, branchId, productId);
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());

            var cashSales = GetTotalGradeSizeCashSaleQuantities(cashSaleList.ToList());
            return cashSales;
        }

        #region branch
        public CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId)
        {
            var results = this._dataService.GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, productId);
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());

            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public CashSaleReportViewModel GenerateCashSaleCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateCashSaleCurrentMonthReportForBranch(branchId);
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());
            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public CashSaleReportViewModel GenerateCashSaleTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateCashSaleTodaysReportForBranch(branchId);
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());
            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        public CashSaleReportViewModel GenerateCashSaleCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateCashSaleCurrentWeekReportForBranch(branchId);
            var cashSaleList = _cashSaleService.MapEFToModel(results.ToList());
            var cashSaleReport = CalculateDifferentCashSaleSums(cashSaleList.ToList());
            return cashSaleReport;
        }

        #endregion

        public CashSaleReportViewModel CalculateDifferentCashSaleSums(List<CashSale> cashSaleList)
        {
            var totalAmount = Convert.ToDecimal(cashSaleList.Sum(d => d.Amount));
            var totalQuantity = cashSaleList.Sum(d => d.Quantity);


            var cashSaleReport = new CashSaleReportViewModel()
            {
                CashSales = cashSaleList,
                TotalAmount = totalAmount,
                TotalQuantity = Convert.ToDouble(totalQuantity),

            };
            return cashSaleReport;
        }

        public GradeSizeTotalsViewModel GetTotalGradeSizeCashSaleQuantities(List<CashSale> cashSaleList)
        {
            int totalSuperTenQuantity = 0, totalSuperTwoFiveQuantity = 0, totalSuperFiveZeroQuantity = 0, totalSuperHundredQuantity = 0, totalSuperFiveQuantity = 0, totalSuperOneQuantity = 0;
            int totalNumberOneTenQuantity = 0, totalNumberOneTwoFiveQuantity = 0, totalNumberOneFiveZeroQuantity = 0, totalNumberOneHundredQuantity = 0, totalNumberOneFiveQuantity = 0, totalNumberOneOneQuantity = 0;
            int totalNumberOneHalfTenQuantity = 0, totalNumberOneHalfTwoFiveQuantity = 0, totalNumberOneHalfFiveZeroQuantity = 0, totalNumberOneHalfHundredQuantity = 0, totalNumberOneHalfFiveQuantity = 0, totalNumberOneHalfOneQuantity = 0;
            int totalKabaleTenQuantity = 0, totalKabaleTwoFiveQuantity = 0, totalKabaleFiveZeroQuantity = 0, totalKabaleHundredQuantity = 0, totalKabaleFiveQuantity = 0, totalKabaleOneQuantity = 0;
            
            //amount
            double totalSuperTenAmount = 0, totalSuperTwoFiveAmount = 0, totalSuperFiveZeroAmount = 0, totalSuperHundredAmount = 0, totalSuperFiveAmount = 0, totalSuperOneAmount = 0;
            double totalNumberOneTenAmount = 0, totalNumberOneTwoFiveAmount = 0, totalNumberOneFiveZeroAmount = 0, totalNumberOneHundredAmount = 0, totalNumberOneFiveAmount = 0, totalNumberOneOneAmount = 0;
            double totalNumberOneHalfTenAmount = 0, totalNumberOneHalfTwoFiveAmount = 0, totalNumberOneHalfFiveZeroAmount = 0, totalNumberOneHalfHundredAmount = 0, totalNumberOneHalfFiveAmount = 0, totalNumberOneHalfOneAmount = 0;
            double totalKabaleTenAmount = 0, totalKabaleTwoFiveAmount = 0, totalKabaleFiveZeroAmount = 0, totalKabaleHundredAmount = 0, totalKabaleFiveAmount = 0, totalKabaleOneAmount = 0;


            List<long> gradesAddedToTransferReport = new List<long>();

            GradeSizeTotalsViewModel gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel();
            List<Grade> grades = new List<Grade>();
            if (cashSaleList.Count() == 0)
            {
                return gradeSizeTotalsViewModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= cashSaleList.Count();)
                {

                    //}
                    //foreach (var item in flourTransferList)
                    //{
                    var item = cashSaleList.ElementAt(i);
                    var name = item.CashSaleId;

                    grades = item.Grades != null ? item.Grades : grades;
                    if (grades.Any())
                    {
                        //  foreach (var grade in grades)
                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToTransferReport.Contains(grade.GradeId))
                            {
                                gradesAddedToTransferReport.Add(grade.GradeId);
                            }

                            switch (grade.Value)
                            {
                                case "Super":
                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperTenAmount = totalSuperTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperTwoFiveAmount = totalSuperTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperFiveZeroAmount = totalSuperFiveZeroAmount + Convert.ToInt32(denom.Amount);

                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperHundredAmount = totalSuperHundredAmount + Convert.ToInt32(denom.Amount);
                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperFiveAmount = totalSuperFiveAmount + Convert.ToInt32(denom.Amount);
                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperOneAmount = totalSuperOneAmount + Convert.ToInt32(denom.Amount);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;
                                case "Number 1":
                                    foreach (var denom in grade.Denominations)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneTenAmount = totalNumberOneTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneTwoFiveAmount = totalNumberOneTwoFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneFiveZeroAmount  = totalNumberOneFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHundredAmount = totalNumberOneHundredAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneFiveAmount = totalNumberOneFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneOneAmount = totalNumberOneOneAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    break;
                                case "Number  1.5":

                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfTenAmount = totalNumberOneHalfTenAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfTwoFiveAmount= totalNumberOneHalfTwoFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfFiveZeroAmount = totalNumberOneHalfFiveZeroAmount + Convert.ToInt32(denom.Amount);
                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfHundredAmount = totalNumberOneHalfHundredAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfFiveAmount = totalNumberOneHalfFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfOneAmount = totalNumberOneHalfOneAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                case "NO. 1 Kabale":

                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleTenAmount = totalKabaleTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleTwoFiveAmount = totalKabaleTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleFiveZeroAmount = totalKabaleFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleHundredAmount = totalKabaleHundredAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleFiveAmount = totalKabaleFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleOneAmount = totalKabaleOneAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                            //grades.RemoveAt(0);
                        }
                        item.Grades.Clear();
                        cashSaleList.RemoveAll(r => r.CashSaleId == item.CashSaleId);

                    }

                    else
                    {

                        cashSaleList.RemoveAll(r => r.CashSaleId == item.CashSaleId);

                    }

                    i = 0;
                    if (cashSaleList.Count() == 0)
                    {
                        break;
                    }
                }
            }
            gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel()
            {
                //Grades = gradesAddedToTransferReport.ToList();
                TotalKabaleFiveQuantity = totalKabaleFiveQuantity,
                TotalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity,
                TotalKabaleHundredQuantity = totalKabaleHundredQuantity,
                TotalKabaleOneQuantity = totalKabaleOneQuantity,
                TotalKabaleTenQuantity = totalKabaleTenQuantity,
                TotalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity,
                TotalNumberOneFiveQuantity = totalNumberOneFiveQuantity,
                TotalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity,
                TotalNumberOneTenQuantity = totalNumberOneTenQuantity,
                TotalNumberOneOneQuantity = totalNumberOneOneQuantity,
                TotalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity,
                TotalNumberOneHundredQuantity = totalNumberOneHundredQuantity,
                TotalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity,
                TotalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity,
                TotalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity,
                TotalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity,
                TotalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity,
                TotalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity,
                TotalSuperFiveQuantity = totalSuperFiveQuantity,
                TotalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity,
                TotalSuperHundredQuantity = totalSuperHundredQuantity,
                TotalSuperOneQuantity = totalSuperOneQuantity,
                TotalSuperTenQuantity = totalSuperTenQuantity,
                TotalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity,

                TotalKabaleFiveAmount = totalKabaleFiveAmount,
                TotalKabaleFiveZeroAmount = totalKabaleFiveZeroAmount,
                TotalKabaleHundredAmount = totalKabaleHundredAmount,
                TotalKabaleOneAmount = totalKabaleOneAmount,
                TotalKabaleTenAmount = totalKabaleTenAmount,
                TotalKabaleTwoFiveAmount = totalKabaleTwoFiveAmount,
                TotalNumberOneFiveAmount = totalNumberOneFiveAmount,
                TotalNumberOneFiveZeroAmount = totalNumberOneFiveZeroAmount,
                TotalNumberOneTenAmount = totalNumberOneTenAmount,
                TotalNumberOneOneAmount = totalNumberOneOneAmount,
                TotalNumberOneTwoFiveAmount = totalNumberOneTwoFiveAmount,
                TotalNumberOneHundredAmount = totalNumberOneHundredAmount,
                TotalNumberOneHalfFiveAmount = totalNumberOneHalfFiveAmount,
                TotalNumberOneHalfFiveZeroAmount = totalNumberOneHalfFiveZeroAmount,
                TotalNumberOneHalfHundredAmount = totalNumberOneHalfHundredAmount,
                TotalNumberOneHalfOneAmount = totalNumberOneHalfOneAmount,
                TotalNumberOneHalfTenAmount = totalNumberOneHalfTenAmount,
                TotalNumberOneHalfTwoFiveAmount = totalNumberOneHalfTwoFiveAmount,
                TotalSuperFiveAmount = totalSuperFiveAmount,
                TotalSuperFiveZeroAmount = totalSuperFiveZeroAmount,
                TotalSuperHundredAmount = totalSuperHundredAmount,
                TotalSuperOneAmount = totalSuperOneAmount,
                TotalSuperTenAmount = totalSuperTenAmount,
                TotalSuperTwoFiveAmount = totalSuperTwoFiveAmount,
            };
            return gradeSizeTotalsViewModel;
        }

        #endregion

        #region  CashTransfer
        public CashTransferReportViewModel GetAllCashTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long fromBranchId, long toBranchId, string status)
        {
            var results = this._dataService.GetAllCashTransfersBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, fromBranchId, toBranchId, status);
            var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());

            var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
            return cashTransferReport;
        }

        public CashTransferReportViewModel GenerateCashTransferCurrentMonthReport()
        {
            var results = this._dataService.GenerateCashTransferCurrentMonthReport();
            var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
            var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
            return cashTransferReport;
        }

        public CashTransferReportViewModel GenerateCashTransferTodaysReport()
        {
            var results = this._dataService.GenerateCashTransferTodaysReport();
            var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
            var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
            return cashTransferReport;
        }

        public CashTransferReportViewModel GenerateCashTransferCurrentWeekReport()
        {

            var results = this._dataService.GenerateCashTransferCurrentWeekReport();
            var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
            var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
            return cashTransferReport;
        }

        #region branch
        public CashTransferReportViewModel GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position)
        {
            if ((position != string.Empty || position != null) && position == "Transfered")
            {
                var results = this._dataService.GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId,status,position);
                var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
                var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
                return cashTransferReport;
            }
            else if ((position != string.Empty || position != null) && position == "Received")
            {
                var results = this._dataService.GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId,status,position);
                var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
                var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
                return cashTransferReport;
            }
            else
            {
                var results = this._dataService.GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId,status,position);
                var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
                var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
                return cashTransferReport;
            }
            
        }
        public CashTransferReportViewModel GenerateCashTransferCurrentMonthReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateCashTransferCurrentMonthReportForBranch(branchId);
            var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
            var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
            return cashTransferReport;
        }

        public CashTransferReportViewModel GenerateCashTransferTodaysReportForBranch(long branchId)
        {
            var results = this._dataService.GenerateCashTransferTodaysReportForBranch(branchId);
            var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
            var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
            return cashTransferReport;
        }

        public CashTransferReportViewModel GenerateCashTransferCurrentWeekReportForBranch(long branchId)
        {

            var results = this._dataService.GenerateCashTransferCurrentWeekReportForBranch(branchId);
            var cashTransferList = _cashTransferService.MapEFToModel(results.ToList());
            var cashTransferReport = CalculateDifferentCashTransferSums(cashTransferList.ToList());
            return cashTransferReport;
        }

       
        #endregion

        public CashTransferReportViewModel CalculateDifferentCashTransferSums(List<CashTransfer> cashTransferList)
        {
            var totalAmount = Convert.ToDecimal(cashTransferList.Sum(d => d.Amount));


            var cashTransferReport = new CashTransferReportViewModel()
            {
                CashTransfers = cashTransferList,
                TotalAmount = totalAmount,

            };
            return cashTransferReport;
        }

        #region consolidated

        #endregion
        #endregion

        #region  BuveraTransfer
        #region web
        public BuveraTransferReportViewModel GetAllBuveraTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status)
        {
            var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, status);
            var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());
            var buveraTransferReport = CalculateDifferentBuveraTransferSums(buveraTransferList.ToList());
            return buveraTransferReport;
        }

        public GradeSizeTotalsViewModel GetAllBuveraTransferTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status)
        {
            var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, status);
            var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());

            var buveraTransfers = GetTotalGradeSizeBuveraTransferQuantities(buveraTransferList.ToList());
            return buveraTransfers;
        }


        #endregion
        #region branch
        public BuveraTransferReportViewModel GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position)
        {
            if ((position != string.Empty || position != null) && position == "Transfered")
            {
                var storeId = _storeService.GetAStoreForAParticularBranch(branchId);
                if (storeId != 0)
                {

                    var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, storeId, status, position);
                    var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());
                    var buveraTransferReport = CalculateDifferentBuveraTransferSums(buveraTransferList.ToList());
                    return buveraTransferReport;
                }
            }
            else if ((position != string.Empty || position != null) && position == "Received")
            {
                var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());
                var buveraTransferReport = CalculateDifferentBuveraTransferSums(buveraTransferList.ToList());
                return buveraTransferReport;
            }
            else
            {
                var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());
                var buveraTransferReport = CalculateDifferentBuveraTransferSums(buveraTransferList.ToList());
                return buveraTransferReport;
            }
            return null;

        }

        public GradeSizeTotalsViewModel GetAllBuveraTransferTotalsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position)
        {
            if ((position != string.Empty || position != null) && position == "Transfered")
            {
                var storeId = _storeService.GetAStoreForAParticularBranch(branchId);
                if (storeId != 0)
                {

                    var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, storeId, status, position);
                    var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());
                    var buveraTransferReport = GetTotalGradeSizeBuveraTransferQuantities(buveraTransferList.ToList());
                    return buveraTransferReport;
                }
            }
            else if ((position != string.Empty || position != null) && position == "Received")
            {
                var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());
                var buveraTransferReport = GetTotalGradeSizeBuveraTransferQuantities(buveraTransferList.ToList());
                return buveraTransferReport;
            }
            else
            {
                var results = this._dataService.GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, status, position);
                var buveraTransferList = _buveraTransferService.MapEFToModel(results.ToList());
                var buveraTransferReport = GetTotalGradeSizeBuveraTransferQuantities(buveraTransferList.ToList());
                return buveraTransferReport;
            }
            return null;
        }

        #endregion

        public BuveraTransferReportViewModel CalculateDifferentBuveraTransferSums(List<BuveraTransfer> buveraTransferList)
        {
            var totalQuantity = buveraTransferList.Sum(d => d.TotalQuantity);

            var buveraTransferReport = new BuveraTransferReportViewModel()
            {
                BuveraTransfers = buveraTransferList,
                TotalQuantity = totalQuantity,

            };



            return buveraTransferReport;
        }



        public GradeSizeTotalsViewModel GetTotalGradeSizeBuveraTransferQuantities(List<BuveraTransfer> buveraTransferList)
        {
            int totalSuperTenQuantity = 0, totalSuperTwoFiveQuantity = 0, totalSuperFiveZeroQuantity = 0, totalSuperHundredQuantity = 0, totalSuperFiveQuantity = 0, totalSuperOneQuantity = 0;
            int totalNumberOneTenQuantity = 0, totalNumberOneTwoFiveQuantity = 0, totalNumberOneFiveZeroQuantity = 0, totalNumberOneHundredQuantity = 0, totalNumberOneFiveQuantity = 0, totalNumberOneOneQuantity = 0;
            int totalNumberOneHalfTenQuantity = 0, totalNumberOneHalfTwoFiveQuantity = 0, totalNumberOneHalfFiveZeroQuantity = 0, totalNumberOneHalfHundredQuantity = 0, totalNumberOneHalfFiveQuantity = 0, totalNumberOneHalfOneQuantity = 0;
            int totalKabaleTenQuantity = 0, totalKabaleTwoFiveQuantity = 0, totalKabaleFiveZeroQuantity = 0, totalKabaleHundredQuantity = 0, totalKabaleFiveQuantity = 0, totalKabaleOneQuantity = 0;

            List<long> gradesAddedToTransferReport = new List<long>();

            GradeSizeTotalsViewModel gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel();
            List<Grade> grades = new List<Grade>();


            if (buveraTransferList.Count() == 0)
            {
                return gradeSizeTotalsViewModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= buveraTransferList.Count();)
                {


                    var item = buveraTransferList.ElementAt(i);
                    var name = item.BuveraTransferId;


                    grades = item.Grades != null ? item.Grades : grades;
                    if (grades.Any())
                    {
                        //  foreach (var grade in grades)
                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToTransferReport.Contains(grade.GradeId))
                            {
                                gradesAddedToTransferReport.Add(grade.GradeId);
                            }

                            switch (grade.Value)
                            {
                                case "Super":
                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;
                                case "Number 1":
                                    foreach (var denom in grade.Denominations)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    break;
                                case "Number  1.5":

                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                case "NO. 1 Kabale":

                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                            //grades.RemoveAt(0);
                        }

                        item.Grades.Clear();
                        buveraTransferList.RemoveAll(r => r.BuveraTransferId == item.BuveraTransferId);

                    }

                    else
                    {

                        buveraTransferList.RemoveAll(r => r.BuveraTransferId == item.BuveraTransferId);
                    }



                    i = 0;
                    if (buveraTransferList.Count() == 0)
                    {
                        break;
                    }
                }
            }
            gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel()
            {
                //Grades = gradesAddedToTransferReport.ToList();
                TotalKabaleFiveQuantity = totalKabaleFiveQuantity,
                TotalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity,
                TotalKabaleHundredQuantity = totalKabaleHundredQuantity,
                TotalKabaleOneQuantity = totalKabaleOneQuantity,
                TotalKabaleTenQuantity = totalKabaleTenQuantity,
                TotalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity,
                TotalNumberOneFiveQuantity = totalNumberOneFiveQuantity,
                TotalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity,
                TotalNumberOneTenQuantity = totalNumberOneTenQuantity,
                TotalNumberOneOneQuantity = totalNumberOneOneQuantity,
                TotalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity,
                TotalNumberOneHundredQuantity = totalNumberOneHundredQuantity,
                TotalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity,
                TotalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity,
                TotalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity,
                TotalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity,
                TotalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity,
                TotalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity,
                TotalSuperFiveQuantity = totalSuperFiveQuantity,
                TotalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity,
                TotalSuperHundredQuantity = totalSuperHundredQuantity,
                TotalSuperOneQuantity = totalSuperOneQuantity,
                TotalSuperTenQuantity = totalSuperTenQuantity,
                TotalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity,
            };
            return gradeSizeTotalsViewModel;
        }
        #endregion

        #region buvera
        #region web
        public BuveraReportViewModel GetAllBuverasBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBuverasBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var buveraList = _buveraService.MapEFToModel(results.ToList());
            var buveraReport = CalculateDifferentBuveraSums(buveraList.ToList());

            return buveraReport;
        }

        public GradeSizeTotalsViewModel GetAllBuveraTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBuverasBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var buveraList = _buveraService.MapEFToModel(results.ToList());

            var buveras = GetTotalGradeSizeBuveraQuantities(buveraList.ToList());
            return buveras;
        }


        #endregion
        #region branch
        public BuveraReportViewModel GetAllBuverasBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBuverasBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var buveraList = _buveraService.MapEFToModel(results.ToList());
            var buveraReport = CalculateDifferentBuveraSums(buveraList.ToList());

            return buveraReport;
        }

        public GradeSizeTotalsViewModel GetAllBuveraTotalsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            var results = this._dataService.GetAllBuverasBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId);
            var buveraList = _buveraService.MapEFToModel(results.ToList());

            var buveras = GetTotalGradeSizeBuveraQuantities(buveraList.ToList());
            return buveras;
        }

        #endregion

        public BuveraReportViewModel CalculateDifferentBuveraSums(List<Buvera> buveraList)
        {
            var totalQuantity = buveraList.Sum(d => d.TotalQuantity);

            var buveraReport = new BuveraReportViewModel()
            {
                Buveras = buveraList,
                TotalQuantity = totalQuantity,

            };



            return buveraReport;
        }



        public GradeSizeTotalsViewModel GetTotalGradeSizeBuveraQuantities(List<Buvera> buveraList)
        {
            int totalSuperTenQuantity = 0, totalSuperTwoFiveQuantity = 0, totalSuperFiveZeroQuantity = 0, totalSuperHundredQuantity = 0, totalSuperFiveQuantity = 0, totalSuperOneQuantity = 0;
            int totalNumberOneTenQuantity = 0, totalNumberOneTwoFiveQuantity = 0, totalNumberOneFiveZeroQuantity = 0, totalNumberOneHundredQuantity = 0, totalNumberOneFiveQuantity = 0, totalNumberOneOneQuantity = 0;
            int totalNumberOneHalfTenQuantity = 0, totalNumberOneHalfTwoFiveQuantity = 0, totalNumberOneHalfFiveZeroQuantity = 0, totalNumberOneHalfHundredQuantity = 0, totalNumberOneHalfFiveQuantity = 0, totalNumberOneHalfOneQuantity = 0;
            int totalKabaleTenQuantity = 0, totalKabaleTwoFiveQuantity = 0, totalKabaleFiveZeroQuantity = 0, totalKabaleHundredQuantity = 0, totalKabaleFiveQuantity = 0, totalKabaleOneQuantity = 0;

            List<long> gradesAddedToTransferReport = new List<long>();

            GradeSizeTotalsViewModel gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel();
            List<Grade> grades = new List<Grade>();

            if (buveraList.Count() == 0)
            {
                return gradeSizeTotalsViewModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= buveraList.Count();)
                {

                    //}
                    //foreach (var item in flourTransferList)
                    //{
                    var item = buveraList.ElementAt(i);
                    var name = item.BuveraId;

                    grades = item.Grades != null ? item.Grades : grades;
                    if (grades.Any())
                    {
                        //  foreach (var grade in grades)
                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToTransferReport.Contains(grade.GradeId))
                            {
                                gradesAddedToTransferReport.Add(grade.GradeId);
                            }

                            switch (grade.Value)
                            {
                                case "Super":
                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;
                                case "Number 1":
                                    foreach (var denom in grade.Denominations)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    break;
                                case "Number  1.5":

                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                case "NO. 1 Kabale":

                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                            //grades.RemoveAt(0);
                        }
                        item.Grades.Clear();
                        buveraList.RemoveAll(r => r.BuveraId == item.BuveraId);
                    }

                    else
                    {
                        buveraList.RemoveAll(r => r.BuveraId == item.BuveraId);
                    }

                    i = 0;
                    if (buveraList.Count() == 0)
                    {
                        break;
                    }
                }
            }
            gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel()
            {
                //Grades = gradesAddedToTransferReport.ToList();
                TotalKabaleFiveQuantity = totalKabaleFiveQuantity,
                TotalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity,
                TotalKabaleHundredQuantity = totalKabaleHundredQuantity,
                TotalKabaleOneQuantity = totalKabaleOneQuantity,
                TotalKabaleTenQuantity = totalKabaleTenQuantity,
                TotalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity,
                TotalNumberOneFiveQuantity = totalNumberOneFiveQuantity,
                TotalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity,
                TotalNumberOneTenQuantity = totalNumberOneTenQuantity,
                TotalNumberOneOneQuantity = totalNumberOneOneQuantity,
                TotalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity,
                TotalNumberOneHundredQuantity = totalNumberOneHundredQuantity,
                TotalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity,
                TotalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity,
                TotalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity,
                TotalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity,
                TotalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity,
                TotalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity,
                TotalSuperFiveQuantity = totalSuperFiveQuantity,
                TotalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity,
                TotalSuperHundredQuantity = totalSuperHundredQuantity,
                TotalSuperOneQuantity = totalSuperOneQuantity,
                TotalSuperTenQuantity = totalSuperTenQuantity,
                TotalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity,
            };
            return gradeSizeTotalsViewModel;
        }
        #endregion
        #region utilityAccounts
        #region web

        public IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long categoryId)
        {
            var results = this._dataService.GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory(lowerSpecifiedDate, upperSpecifiedDate, branchId, categoryId);
            var utilityAccountList = _utilityAccountService.MapEFToModel(results.ToList());

            return utilityAccountList;
        }
        #endregion
        #region branch

        public IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategoryForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long categoryId)
        {
            var results = this._dataService.GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategoryForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, categoryId);
            var utilityAccountList = _utilityAccountService.MapEFToModel(results.ToList());

            return utilityAccountList;
        }

        #endregion


        #endregion

        #region creditors

        public CreditorReportViewModel GenerateCreditorReport()
        {

            var results = this._creditorService.GetCreditorView();

            var creditorReport = ListOfCreditorsAndTotalAmount(results.ToList());
            return creditorReport;
        }

        public CreditorReportViewModel GenerateCreditorReportForAParticularDate(DateTime dateTime)
        {

            var results = this._creditorService.GetCreditorViewForAParticularDate(dateTime);

            var creditorReport = ListOfCreditorsAndTotalAmount(results.ToList());
            return creditorReport;
        }
        public CreditorReportViewModel ListOfCreditorsAndTotalAmount(List<CreditorView> creditorList)
        {
            var totalAmount = Convert.ToDecimal(creditorList.Sum(d => d.Amount));


            var creditorReport = new CreditorReportViewModel()
            {
                Creditors = creditorList,
                TotalAmount = totalAmount,

            };
            return creditorReport;
        }
        #endregion

        #region debtors
        public DebtorReportViewModel GenerateDebtorReport()
        {

            var results = this._debtorService.GetDebtorView();

            var debtorReport = ListOfDebtorsAndTotalAmount(results.ToList());
            return debtorReport;
        }

        public DebtorReportViewModel GenerateDebtorReportForAParticularDate(DateTime dateTime)
        {

            var results = this._debtorService.GetDebtorViewForAParticularDate(dateTime);

            var debtorReport = ListOfDebtorsAndTotalAmount(results.ToList());
            return debtorReport;
        }

        public DebtorReportViewModel ListOfDebtorsAndTotalAmount(List<DebtorView> debtorList)
        {
            var totalAmount = Convert.ToDecimal(debtorList.Sum(d => d.Amount));


            var debtorReport = new DebtorReportViewModel()
            {
                Debtors = debtorList,
                TotalAmount = totalAmount,

            };
            return debtorReport;
        }
        #endregion

        #region advance payments
        public DebtorReportViewModel GenerateAdvancePaymentReport()
        {

            var results = this._debtorService.GetAdvancePaymentView();

            var advancePaymentReport = ListOfAdvancePaymentsAndTotalAmount(results.ToList());
            return advancePaymentReport;
        }
        public DebtorReportViewModel GenerateAdvancePaymentReportForAParticularDate(DateTime dateTime)
        {

            var results = this._debtorService.GetAdvancePaymentViewForAParticularDate(dateTime);

            var advancePaymentReport = ListOfAdvancePaymentsAndTotalAmount(results.ToList());
            return advancePaymentReport;
        }
        public DebtorReportViewModel ListOfAdvancePaymentsAndTotalAmount(List<DebtorView> advancePaymentList)
        {
            var totalAmount = Convert.ToDecimal(advancePaymentList.Sum(d => d.Amount));


            var advancePaymentReport = new DebtorReportViewModel()
            {
                Debtors = advancePaymentList,
                TotalAmount = totalAmount,

            };
            return advancePaymentReport;
        }
        #endregion

        #region deposits,recoveries,discounts
        public DepositsReportViewModel GenerateDepositsReport(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
        {

            var results = this._dataService.GetAllDepositsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId, depositTransactionSubTypeId);
            var accountTransactionActivityList = _accountTransactionActivityService.MapEFToModel(results.ToList());

            var depositReport = ListOfDepositsAndTotalAmount(accountTransactionActivityList.ToList());
            return depositReport;
        }

        public DepositsReportViewModel GenerateRecoveriesReport(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
        {

            var results = this._dataService.GetAllRecoveriesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId, recoveryTransactionSubTypeId);
            var accountTransactionActivityList = _accountTransactionActivityService.MapEFToModel(results.ToList());

            var recoveryReport = ListOfDepositsAndTotalAmount(accountTransactionActivityList.ToList());
            return recoveryReport;
        }

        public DepositsReportViewModel GenerateDiscountsReport(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
        {

            var results = this._dataService.GetAllDiscountsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, supplierId,discountTransactionSubTypeId);
            var accountTransactionActivityList = _accountTransactionActivityService.MapEFToModel(results.ToList());

            var discountReport = ListOfDepositsAndTotalAmount(accountTransactionActivityList.ToList());
            return discountReport;
        }
        public DepositsReportViewModel ListOfDepositsAndTotalAmount(List<AccountTransactionActivity> depositList)
        {


            var totalAmount = Convert.ToDecimal(depositList.Sum(d => d.Amount));
            var totalQuantity = Convert.ToDouble(depositList.Sum(d => d.Quantity));

            var depositReport = new DepositsReportViewModel()
            {
                Deposits = depositList,
                TotalAmount = totalAmount,
                TotalQuantity = totalQuantity,

            };
            return depositReport;
        }
        #endregion

        #region WeightLosses
        #region web
        public WeightLossesReportViewModel GetAllWeightLossesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            
            var results = this._dataService.GetAllWeightLossesBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId);
            var weightLossList = _weightLossService.MapEFToModel(results.ToList());

            var weightLossReport = ListOfWeightLossesAndTotalQuantity(weightLossList.ToList());
            return weightLossReport;

        }
        #endregion
        #region branch
        public WeightLossesReportViewModel GetAllWeightLossesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            var results = this._dataService.GetAllWeightLossesBetweenTheSpecifiedDatesForBranch(lowerSpecifiedDate, upperSpecifiedDate, branchId, customerId);
            var weightLossList = _weightLossService.MapEFToModel(results.ToList());
            var weightLossReport = ListOfWeightLossesAndTotalQuantity(weightLossList.ToList());
            return weightLossReport;
        }


        #endregion

        public WeightLossesReportViewModel ListOfWeightLossesAndTotalQuantity(List<WeightLoss> weightLossList)
        {


            var totalQuantity = Convert.ToDouble(weightLossList.Sum(d => d.Quantity));

            var weightLossReport = new WeightLossesReportViewModel()
            {
                WeightLosses = weightLossList,
                
                TotalQuantity = totalQuantity,

            };
            return weightLossReport;
        }
        #endregion

        #region daily
        public DailyReport GetAllActivitiesForAParticularBranchForASpecificPeriod(DateTime fromDate,DateTime toDate, long branchId)
        {
            //  var status = ["Accepted", "Rejected", "OutStanding"];
            // $scope.position = ["Transfered", "Received"];
            var customerId = string.Empty;
            var acceptedTransferedFlourTransfers = GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(fromDate, toDate, branchId, "Accepted", "Transfered");
            var acceptedReceivedFlourTransfers = GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(fromDate, toDate, branchId, "Accepted", "Received");
            var factoryExpenses = GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(fromDate, toDate, branchId);
            var acceptedTransferedCashTransfers = GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(fromDate, toDate, branchId, "Accepted", "Transfered");
            var acceptedReceivedCashTransfers = GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(fromDate, toDate, branchId, "Accepted", "Received");

            var flourCashSales = GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(fromDate, toDate, branchId,1);
            var brandCashSales = GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(fromDate, toDate, branchId, 2);
            var labourCosts = GetAllLabourCostsBetweenTheSpecifiedDatesForBranch(fromDate, toDate, branchId);
            var flourDeliveries = GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(fromDate, toDate, branchId,customerId, 1);
            var brandDeliveries = GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(fromDate, toDate, branchId, customerId, 2);

            var supplies = GetAllSuppliesBetweenTheSpecifiedDatesForBranch(fromDate, toDate, branchId, null);
            var expenses = labourCosts.TotalAmount + factoryExpenses.TotalAmount;

            var dailyReport = new DailyReport()
            {
                BrandDeliveries = brandDeliveries,
                FlourDeliveries = flourDeliveries,
                ReceivedCashTransfers = acceptedReceivedCashTransfers,
                TransferedCashTransfers = acceptedTransferedCashTransfers,
                Supplies = supplies,
                //Expenses = expenses,
               FactoryExpenses = factoryExpenses,
               LabourCosts = labourCosts,
               FlourCashSales = flourCashSales,
               BrandCashSales = brandCashSales,
               TransferedAcceptedFlourTransfers = acceptedTransferedFlourTransfers,
               ReceivedAcceptedFlourTransfers = acceptedReceivedFlourTransfers,
            };
            return dailyReport;
        }
        #endregion

        #region out sourcers
             

        public OutSourcerOutPutReportViewModel GetAllOutPutsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId)
        {
            var results = this._dataService.GetAllOutPutsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate,storeId);
            var outPutList = _outSourcerOutPutService.MapEFToModel(results.ToList());

            var outPutReport = CalculateDifferentOutSourcerOutPutSums(outPutList.ToList());
            return outPutReport;
        }
        public GradeSizeTotalsViewModel GetAllOutPutTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId)
        {
            var results = this._dataService.GetAllOutPutsBetweenTheSpecifiedDates(lowerSpecifiedDate, upperSpecifiedDate, storeId);
            var outPutList = _outSourcerOutPutService.MapEFToModel(results.ToList());

            var outPuts = GetTotalGradeSizeOutPutQuantities(outPutList.ToList());
            return outPuts;
        }

        public GradeSizeTotalsViewModel GetAllDeliveryTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId,string customerId,long productId)
        {
            var results = this._dataService.GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(lowerSpecifiedDate, upperSpecifiedDate, storeId,customerId,productId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveries = GetTotalGradeSizeDeliveryQuantities(deliveryList.ToList());
            return deliveries;
        }


        public OutSourcerOutPutReportViewModel CalculateDifferentOutSourcerOutPutSums(List<OutSourcerOutPut> outPutList)
        {
            var totalAmount = Convert.ToDecimal(outPutList.Sum(d => d.TotalAmount));
            var totalQuantity = outPutList.Sum(d => d.TotalQuantity);

            var outPutReport = new OutSourcerOutPutReportViewModel()
            {
                OutSourcerOutPuts = outPutList,
                TotalAmount = totalAmount,
                TotalQuantity = totalQuantity
            };
            return outPutReport;
        }

        public GradeSizeTotalsViewModel GetTotalGradeSizeOutPutQuantities(List<OutSourcerOutPut> outPutList)
        {
            //quantity
            int totalSuperTenQuantity = 0, totalSuperTwoFiveQuantity = 0, totalSuperFiveZeroQuantity = 0, totalSuperHundredQuantity = 0, totalSuperFiveQuantity = 0, totalSuperOneQuantity = 0;
            int totalNumberOneTenQuantity = 0, totalNumberOneTwoFiveQuantity = 0, totalNumberOneFiveZeroQuantity = 0, totalNumberOneHundredQuantity = 0, totalNumberOneFiveQuantity = 0, totalNumberOneOneQuantity = 0;
            int totalNumberOneHalfTenQuantity = 0, totalNumberOneHalfTwoFiveQuantity = 0, totalNumberOneHalfFiveZeroQuantity = 0, totalNumberOneHalfHundredQuantity = 0, totalNumberOneHalfFiveQuantity = 0, totalNumberOneHalfOneQuantity = 0;
            int totalKabaleTenQuantity = 0, totalKabaleTwoFiveQuantity = 0, totalKabaleFiveZeroQuantity = 0, totalKabaleHundredQuantity = 0, totalKabaleFiveQuantity = 0, totalKabaleOneQuantity = 0;

            //amount
            double totalSuperTenAmount = 0, totalSuperTwoFiveAmount = 0, totalSuperFiveZeroAmount = 0, totalSuperHundredAmount = 0, totalSuperFiveAmount = 0, totalSuperOneAmount = 0;
            double totalNumberOneTenAmount = 0, totalNumberOneTwoFiveAmount = 0, totalNumberOneFiveZeroAmount = 0, totalNumberOneHundredAmount = 0, totalNumberOneFiveAmount = 0, totalNumberOneOneAmount = 0;
            double totalNumberOneHalfTenAmount = 0, totalNumberOneHalfTwoFiveAmount = 0, totalNumberOneHalfFiveZeroAmount = 0, totalNumberOneHalfHundredAmount = 0, totalNumberOneHalfFiveAmount = 0, totalNumberOneHalfOneAmount = 0;
            double totalKabaleTenAmount = 0, totalKabaleTwoFiveAmount = 0, totalKabaleFiveZeroAmount = 0, totalKabaleHundredAmount = 0, totalKabaleFiveAmount = 0, totalKabaleOneAmount = 0;

            List<long> gradesAddedToTransferReport = new List<long>();

            GradeSizeTotalsViewModel gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel();
            List<Grade> grades = new List<Grade>();
            if (outPutList.Count() == 0)
            {
                return gradeSizeTotalsViewModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= outPutList.Count();)
                {


                    var item = outPutList.ElementAt(i);
                    var name = item.OutSourcerOutPutId;

                    grades = item.Grades != null ? item.Grades : grades;
                    if (grades.Any())
                    {
                        //  foreach (var grade in grades)
                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToTransferReport.Contains(grade.GradeId))
                            {
                                gradesAddedToTransferReport.Add(grade.GradeId);
                            }

                            switch (grade.Value)
                            {
                                case "Super":
                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperTenAmount = totalSuperTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperTwoFiveAmount = totalSuperTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperFiveZeroAmount = totalSuperFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperHundredAmount = totalSuperHundredAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalSuperFiveAmount = totalSuperFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.Quantity);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;
                                case "Number 1":
                                    foreach (var denom in grade.Denominations)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneTenAmount = totalNumberOneTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneTwoFiveAmount = totalNumberOneTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneFiveZeroAmount = totalNumberOneFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHundredAmount = totalNumberOneHundredAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneFiveAmount = totalNumberOneFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneOneAmount = totalNumberOneOneAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    break;
                                case "Number  1.5":

                                    foreach (var denom in grade.Denominations)
                                    //  for (int j = 0; j < grade.Denominations.Count(); j++)
                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfTenAmount = totalNumberOneHalfTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfTwoFiveAmount = totalNumberOneHalfTwoFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfFiveZeroAmount = totalNumberOneHalfFiveZeroAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfHundredAmount = totalNumberOneHalfHundredAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfFiveAmount = totalNumberOneHalfFiveAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalNumberOneHalfOneAmount = totalNumberOneHalfOneAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                case "NO. 1 Kabale":

                                    foreach (var denom in grade.Denominations)

                                    {
                                        switch (denom.Value)
                                        {
                                            case 10:
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleTenAmount = totalKabaleTenAmount + Convert.ToDouble(denom.Amount);

                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleTwoFiveAmount = totalKabaleTwoFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleFiveZeroAmount = totalKabaleFiveZeroAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleHundredAmount = totalKabaleHundredAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleFiveAmount = totalKabaleFiveAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.Quantity);
                                                totalKabaleOneAmount = totalKabaleOneAmount + Convert.ToDouble(denom.Amount);
                                                break;
                                            default:

                                                continue;
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                            //grades.RemoveAt(0);
                        }

                        item.Grades.Clear();
                        outPutList.RemoveAll(r => r.OutSourcerOutPutId== item.OutSourcerOutPutId);

                    }

                    else
                    {

                        outPutList.RemoveAll(r => r.OutSourcerOutPutId == item.OutSourcerOutPutId);

                    }

                    i = 0;
                    if (outPutList.Count() == 0)
                    {
                        break;
                    }
                }
            }
            gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel()
            {
                //Grades = gradesAddedToTransferReport.ToList();
                TotalKabaleFiveQuantity = totalKabaleFiveQuantity,
                TotalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity,
                TotalKabaleHundredQuantity = totalKabaleHundredQuantity,
                TotalKabaleOneQuantity = totalKabaleOneQuantity,
                TotalKabaleTenQuantity = totalKabaleTenQuantity,
                TotalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity,
                TotalNumberOneFiveQuantity = totalNumberOneFiveQuantity,
                TotalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity,
                TotalNumberOneTenQuantity = totalNumberOneTenQuantity,
                TotalNumberOneOneQuantity = totalNumberOneOneQuantity,
                TotalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity,
                TotalNumberOneHundredQuantity = totalNumberOneHundredQuantity,
                TotalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity,
                TotalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity,
                TotalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity,
                TotalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity,
                TotalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity,
                TotalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity,
                TotalSuperFiveQuantity = totalSuperFiveQuantity,
                TotalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity,
                TotalSuperHundredQuantity = totalSuperHundredQuantity,
                TotalSuperOneQuantity = totalSuperOneQuantity,
                TotalSuperTenQuantity = totalSuperTenQuantity,
                TotalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity,

                TotalKabaleFiveAmount = totalKabaleFiveAmount,
                TotalKabaleFiveZeroAmount = totalKabaleFiveZeroAmount,
                TotalKabaleHundredAmount = totalKabaleHundredAmount,
                TotalKabaleOneAmount = totalKabaleOneAmount,
                TotalKabaleTenAmount = totalKabaleTenAmount,
                TotalKabaleTwoFiveAmount = totalKabaleTwoFiveAmount,
                TotalNumberOneFiveAmount = totalNumberOneFiveAmount,
                TotalNumberOneFiveZeroAmount = totalNumberOneFiveZeroAmount,
                TotalNumberOneTenAmount = totalNumberOneTenAmount,
                TotalNumberOneOneAmount = totalNumberOneOneAmount,
                TotalNumberOneTwoFiveAmount = totalNumberOneTwoFiveAmount,
                TotalNumberOneHundredAmount = totalNumberOneHundredAmount,
                TotalNumberOneHalfFiveAmount = totalNumberOneHalfFiveAmount,
                TotalNumberOneHalfFiveZeroAmount = totalNumberOneHalfFiveZeroAmount,
                TotalNumberOneHalfHundredAmount = totalNumberOneHalfHundredAmount,
                TotalNumberOneHalfOneAmount = totalNumberOneHalfOneAmount,
                TotalNumberOneHalfTenAmount = totalNumberOneHalfTenAmount,
                TotalNumberOneHalfTwoFiveAmount = totalNumberOneHalfTwoFiveAmount,
                TotalSuperFiveAmount = totalSuperFiveAmount,
                TotalSuperFiveZeroAmount = totalSuperFiveZeroAmount,
                TotalSuperHundredAmount = totalSuperHundredAmount,
                TotalSuperOneAmount = totalSuperOneAmount,
                TotalSuperTenAmount = totalSuperTenAmount,
                TotalSuperTwoFiveAmount = totalSuperTwoFiveAmount,

            };
            return gradeSizeTotalsViewModel;
        }

        public DeliveryReportViewModel GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId, string customerId, long productId)
        {
            var results = this._dataService.GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(lowerSpecifiedDate, upperSpecifiedDate, storeId, customerId, productId);
            var deliveryList = _deliveryService.MapEFToModel(results.ToList());

            var deliveryReport = CalculateDifferentDeliverySums(deliveryList.ToList());
            return deliveryReport;
        }

        #endregion

    }
}

