using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.ViewModel;

namespace Higgs.Mbale.Web.Controllers
{
    public class ReportApiController : ApiController
    {
          private IReportService _reportService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(ReportApiController));
            private string userId = string.Empty;

            public ReportApiController()
            {
            }

            public ReportApiController(IReportService reportService,IUserService userService)
            {
                this._reportService = reportService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }


            #region transactions
            [HttpGet]
            [ActionName("GetAllTransactionsBetweenTheSpecifiedDates")]
            public IEnumerable<Transaction> GetAllTransactionsBetweenTheSpecifiedDates(ReportSearch search)
            {
                return _reportService.GetAllTransactionsBetweenTheSpecifiedDates(search.FromDate,search.ToDate);
            }

            [HttpGet]
            [ActionName("GenerateTransactionCurrentMonthReport")]
            public IEnumerable<Transaction> GenerateTransactionCurrentMonthReport()
            {
                return _reportService.GenerateTransactionCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateTransactionTodaysReport")]
            public IEnumerable<Transaction> GenerateTransactionTodaysReport()
            {
                return _reportService.GenerateTransactionTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateTransactionCurrentWeekReport")]
            public IEnumerable<Transaction> GenerateTransactionCurrentWeekReport()
            {
                return _reportService.GenerateTransactionCurrentWeekReport();
            }

           
      
            #endregion

        #region supplies
            [HttpPost]
            [ActionName("GetAllSuppliesBetweenTheSpecifiedDates")]
            public SupplyReportViewModel GetAllSuppliesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllSuppliesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate,searchDates.BranchId,searchDates.SupplierId);
            }

            [HttpGet]
            [ActionName("GenerateSupplyCurrentMonthReport")]
            public SupplyReportViewModel GenerateSupplyCurrentMonthReport()
            {
                return _reportService.GenerateSupplyCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateSupplyTodaysReport")]
            public SupplyReportViewModel GenerateSupplyTodaysReport()
            {
                return _reportService.GenerateSupplyTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateSupplyCurrentWeekReport")]
            public SupplyReportViewModel GenerateSupplyCurrentWeekReport()
            {
                return _reportService.GenerateSupplyCurrentWeekReport();
            }
        #endregion

            #region supplies for supplier
            [HttpPost]
            [ActionName("GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier")]
            public IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier(ReportSearch searchDates,string supplierId)
            {
                return _reportService.GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier(searchDates.FromDate, searchDates.ToDate,supplierId);
            }

            [HttpGet]
            [ActionName("GenerateSupplyCurrentMonthReportForAParticularSupplier")]
            public IEnumerable<Supply> GenerateSupplyCurrentMonthReportforAParticularSupplier(string supplierId)
            {
                return _reportService.GenerateSupplyCurrentMonthReportForAParticularSupplier(supplierId);
            }

            [HttpGet]
            [ActionName("GenerateSupplyTodaysReportForAParticularSupplier")]
            public IEnumerable<Supply> GenerateSupplyTodaysReportForAParticularSupplier(string supplierId)
            {
                return _reportService.GenerateSupplyTodaysReportForAParticularSupplier(supplierId);
            }

            [HttpGet]
            [ActionName("GenerateSupplyCurrentWeekReportForAParticularSupplier")]
            public IEnumerable<Supply> GenerateSupplyCurrentWeekReportForAParticularSupplier(string supplierId)
            {
                return _reportService.GenerateSupplyCurrentWeekReportForAParticularSupplier(supplierId);
            }
            #endregion

        #region accountTransactions

           

            [HttpGet]
            [ActionName("GenerateAccountTransactionCurrentMonthReport")]
            public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentMonthReport()
            {
                return _reportService.GenerateAccountTransactionCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateAccountTransactionTodaysReport")]
            public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionTodaysReport()
            {
                return _reportService.GenerateAccountTransactionTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateAccountTransactionCurrentWeekReport")]
            public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentWeekReport()
            {
                return _reportService.GenerateAccountTransactionCurrentWeekReport();
            }

            [HttpPost]
            [ActionName("GenerateAccountTransactionsBetweenTheSpecifiedDates")]
            public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionsBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllAccountTransactionsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.SupplierId);

            }
        #endregion

         #region batches
            [HttpPost]
            [ActionName("GetAllBatchesBetweenTheSpecifiedDates")]
            public BatchReportViewModel GetAllBatchesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllBatchesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchCurrentMonthReport")]
            public BatchReportViewModel GenerateBatchCurrentMonthReport()
            {
                return _reportService.GenerateBatchCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateBatchTodaysReport")]
            public BatchReportViewModel GenerateBatchTodaysReport()
            {
                return _reportService.GenerateBatchTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateBatchCurrentWeekReport")]
            public BatchReportViewModel GenerateBatchCurrentWeekReport()
            {
                return _reportService.GenerateBatchCurrentWeekReport();
            }
            #endregion

         #region deliveries
            [HttpPost]
            [ActionName("GetAllDeliveriesBetweenTheSpecifiedDates")]
            public DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllDeliveriesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.CustomerId);
            }

            [HttpGet]
            [ActionName("GenerateDeliveryCurrentMonthReport")]
            public DeliveryReportViewModel GenerateDeliveryCurrentMonthReport()
            {
                return _reportService.GenerateDeliveryCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateDeliveryTodaysReport")]
            public DeliveryReportViewModel GenerateDeliveryTodaysReport()
            {
                return _reportService.GenerateDeliveryTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateDeliveryCurrentWeekReport")]
            public DeliveryReportViewModel GenerateDeliveryCurrentWeekReport()
            {
                return _reportService.GenerateDeliveryCurrentWeekReport();
            }

            [HttpPost]
            [ActionName("GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct")]
            public DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(ReportSearch searchDates)
            {
                return _reportService.GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.CustomerId,searchDates.ProductId);
            }

        [HttpPost]
        [ActionName("GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct")]
        public GradeSizeTotalsViewModel GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct(ReportSearch searchDates)
        {
            return _reportService.GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.CustomerId, searchDates.ProductId);
        }
        #endregion

        #region FactoryExpenses
        [HttpPost]
            [ActionName("GetAllFactoryExpensesBetweenTheSpecifiedDates")]
            public FactoryExpenseReportViewModel GetAllFactoryExpensesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllFactoryExpensesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateFactoryExpenseCurrentMonthReport")]
            public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentMonthReport()
            {
                return _reportService.GenerateFactoryExpenseCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateFactoryExpenseTodaysReport")]
            public FactoryExpenseReportViewModel GenerateFactoryExpenseTodaysReport()
            {
                return _reportService.GenerateFactoryExpenseTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateFactoryExpenseCurrentWeekReport")]
            public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentWeekReport()
            {
                return _reportService.GenerateFactoryExpenseCurrentWeekReport();
            }
            #endregion

            #region orders
            [HttpPost]
            [ActionName("GetAllOrdersBetweenTheSpecifiedDates")]
            public IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllOrdersBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.CustomerId);
            }

            [HttpGet]
            [ActionName("GenerateOrderCurrentMonthReport")]
            public IEnumerable<Order> GenerateOrderCurrentMonthReport()
            {
                return _reportService.GenerateOrderCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateOrderTodaysReport")]
            public IEnumerable<Order> GenerateOrderTodaysReport()
            {
                return _reportService.GenerateOrderTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateOrderCurrentWeekReport")]
            public IEnumerable<Order> GenerateOrderCurrentWeekReport()
            {
                return _reportService.GenerateOrderCurrentWeekReport();
            }
            #endregion

            #region Cash
            [HttpPost]
            [ActionName("GetAllCashBetweenTheSpecifiedDates")]
            public IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllCashBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateCashCurrentMonthReport")]
            public IEnumerable<Cash> GenerateCashCurrentMonthReport()
            {
                return _reportService.GenerateCashCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateCashTodaysReport")]
            public IEnumerable<Cash> GenerateCashTodaysReport()
            {
                return _reportService.GenerateCashTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateCashCurrentWeekReport")]
            public IEnumerable<Cash> GenerateCashCurrentWeekReport()
            {
                return _reportService.GenerateCashCurrentWeekReport();
            }

            #region expenses
           
            [HttpPost]
            [ActionName("GetAllExpensesBetweenTheSpecifiedDates")]
            public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllExpensesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }
            [HttpPost]
            [ActionName("GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory")]
            public CashReportViewModel GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory(ReportSearch searchDates)
            {
            return _reportService.GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.RequistionCategoryId);
            }

           
            #endregion

            #region incomes
           

            [HttpPost]
            [ActionName("GetAllIncomesBetweenTheSpecifiedDates")]
            public CashReportViewModel GetAllIncomesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllIncomesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateIncomesCurrentMonthReport")]
            public IEnumerable<Cash> GenerateIncomesCurrentMonthReport()
            {
                return _reportService.GenerateIncomesCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateIncomesTodaysReport")]
            public IEnumerable<Cash> GenerateIncomesTodaysReport()
            {
                return _reportService.GenerateIncomesTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateIncomesCurrentWeekReport")]
            public IEnumerable<Cash> GenerateIncomesCurrentWeekReport()
            {

                return _reportService.GenerateIncomesCurrentWeekReport();
            }
            #endregion
            #endregion

            #region OtherExpenses
            [HttpPost]
            [ActionName("GetAllOtherExpensesBetweenTheSpecifiedDates")]
            public IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllOtherExpensesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateOtherExpenseCurrentMonthReport")]
            public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReport()
            {
                return _reportService.GenerateOtherExpenseCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateOtherExpenseTodaysReport")]
            public IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReport()
            {
                return _reportService.GenerateOtherExpenseTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateOtherExpenseCurrentWeekReport")]
            public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReport()
            {
                return _reportService.GenerateOtherExpenseCurrentWeekReport();
            }
            #endregion

            #region LabourCosts
            [HttpPost]
            [ActionName("GetAllLabourCostsBetweenTheSpecifiedDates")]
            public LabourCostReportViewModel GetAllLabourCostsBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllLabourCostsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateLabourCostCurrentMonthReport")]
            public LabourCostReportViewModel GenerateLabourCostCurrentMonthReport()
            {
                return _reportService.GenerateLabourCostCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateLabourCostTodaysReport")]
            public LabourCostReportViewModel GenerateLabourCostTodaysReport()
            {
                return _reportService.GenerateLabourCostTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateLabourCostCurrentWeekReport")]
            public LabourCostReportViewModel GenerateLabourCostCurrentWeekReport()
            {
                return _reportService.GenerateLabourCostCurrentWeekReport();
            }
            #endregion

            #region Utility
            [HttpPost]
            [ActionName("GetAllUtilityBetweenTheSpecifiedDates")]
            public IEnumerable<Utility> GetAllUtilitiesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllUtilitiesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateUtilityCurrentMonthReport")]
            public IEnumerable<Utility> GenerateUtilityCurrentMonthReport()
            {
                return _reportService.GenerateUtilityCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateUtilityTodaysReport")]
            public IEnumerable<Utility> GenerateUtilityTodaysReport()
            {
                return _reportService.GenerateUtilityTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateUtilityCurrentWeekReport")]
            public IEnumerable<Utility> GenerateUtilityCurrentWeekReport()
            {
                return _reportService.GenerateUtilityCurrentWeekReport();
            }
            #endregion

            #region FlourTransfer
            [HttpPost]
            [ActionName("GetAllFlourTransfersBetweenTheSpecifiedDates")]
            public FlourTransferReportViewModel GetAllFlourTransfersBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllFlourTransfersBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId,searchDates.Status);
            }

            [HttpGet]
            [ActionName("GenerateFlourTransferCurrentMonthReport")]
            public FlourTransferReportViewModel GenerateFlourTransferCurrentMonthReport()
            {
                return _reportService.GenerateFlourTransferCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateFlourTransferTodaysReport")]
            public FlourTransferReportViewModel GenerateFlourTransferTodaysReport()
            {
                return _reportService.GenerateFlourTransferTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateFlourTransferCurrentWeekReport")]
            public FlourTransferReportViewModel GenerateFlourTransferCurrentWeekReport()
            {
                return _reportService.GenerateFlourTransferCurrentWeekReport();
            }

            [HttpPost]
            [ActionName("GetAllFlourTransferTotalsBetweenTheSpecifiedDates")]
            public FlourTransferReportTotalModel GetAllFlourTransferTotalsBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllFlourTransferTotalsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.Status);
            }
            #endregion

            #region MachineRepairs
            [HttpPost]
            [ActionName("GetAllMachineRepairsBetweenTheSpecifiedDates")]
            public IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllMachineRepairsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateMachineRepairCurrentMonthReport")]
            public IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReport()
            {
                return _reportService.GenerateMachineRepairCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateMachineRepairTodaysReport")]
            public IEnumerable<MachineRepair> GenerateMachineRepairTodaysReport()
            {
                return _reportService.GenerateMachineRepairTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateMachineRepairCurrentWeekReport")]
            public IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReport()
            {
                return _reportService.GenerateMachineRepairCurrentWeekReport();
            }
            #endregion

            #region BatchOutPuts
            [HttpPost]
            [ActionName("GetAllBatchOutPutsBetweenTheSpecifiedDates")]
            public IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllBatchOutPutsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchOutPutCurrentMonthReport")]
            public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReport()
            {
                return _reportService.GenerateBatchOutPutCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateBatchOutPutTodaysReport")]
            public IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReport()
            {
                return _reportService.GenerateBatchOutPutTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateBatchOutPutCurrentWeekReport")]
            public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReport()
            {
                return _reportService.GenerateBatchOutPutCurrentWeekReport();
            }

            [HttpPost]
            [ActionName("GetAllFlourTotalsBetweenTheSpecifiedDates")]
            public GradeSizeTotalsViewModel GetAllFlourTotalsBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllFlourTotalsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }
        #endregion

        #region casual accountTransactions



        [HttpPost]
            [ActionName("GenerateCasualAccountTransactionsBetweenTheSpecifiedDates")]
            public IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllCasualAccountTransactionsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, Convert.ToInt64(searchDates.SupplierId));

            }
            #endregion


            #region CashSale
            [HttpPost]
            [ActionName("GetAllCashSalesBetweenTheSpecifiedDates")]
            public CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllCashSalesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
            }

            [HttpGet]
            [ActionName("GenerateCashSaleCurrentMonthReport")]
            public CashSaleReportViewModel GenerateCashSaleCurrentMonthReport()
            {
                return _reportService.GenerateCashSaleCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateCashSaleTodaysReport")]
            public CashSaleReportViewModel GenerateCashSaleTodaysReport()
            {
                return _reportService.GenerateCashSaleTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateCashSaleCurrentWeekReport")]
            public CashSaleReportViewModel GenerateCashSaleCurrentWeekReport()
            {
                return _reportService.GenerateCashSaleCurrentWeekReport();
            }

            [HttpPost]
            [ActionName("GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct")]
            public CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(ReportSearch searchDates)
            {
                return _reportService.GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId,searchDates.ProductId);
            }

        [HttpPost]
        [ActionName("GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct")]
        public GradeSizeTotalsViewModel GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct(ReportSearch searchDates)
        {
            return _reportService.GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.ProductId);
        }
        #endregion

        #region CashTransfer
        [HttpPost]
            [ActionName("GetAllCashTransfersBetweenTheSpecifiedDates")]
            public CashTransferReportViewModel GetAllCashTransfersBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllCashTransfersBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.FromBranchId,searchDates.ToBranchId,searchDates.Status);
            }

            [HttpGet]
            [ActionName("GenerateCashTransferCurrentMonthReport")]
            public CashTransferReportViewModel GenerateCashTransferCurrentMonthReport()
            {
                return _reportService.GenerateCashTransferCurrentMonthReport();
            }

            [HttpGet]
            [ActionName("GenerateCashTransferTodaysReport")]
            public CashTransferReportViewModel GenerateCashTransferTodaysReport()
            {
                return _reportService.GenerateCashTransferTodaysReport();
            }

            [HttpGet]
            [ActionName("GenerateCashTransferCurrentWeekReport")]
            public CashTransferReportViewModel GenerateCashTransferCurrentWeekReport()
            {
                return _reportService.GenerateCashTransferCurrentWeekReport();
            }
            #endregion

            #region BuveraTransfer
            [HttpPost]
            [ActionName("GetAllBuveraTransfersBetweenTheSpecifiedDates")]
            public BuveraTransferReportViewModel GetAllBuveraTransfersBetweenTheSpecifiedDates(ReportSearch searchDates)
            {
                return _reportService.GetAllBuveraTransfersBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId,searchDates.Status);
            }

       
        [HttpPost]
        [ActionName("GetAllBuveraTransferTotalsBetweenTheSpecifiedDates")]
        public GradeSizeTotalsViewModel GetAllBuveraTransferTotalsBetweenTheSpecifiedDates(ReportSearch searchDates)
        {
            return _reportService.GetAllBuveraTransferTotalsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.Status);
        }
        #endregion

        #region buveras

        [HttpPost]
        [ActionName("GetAllBuverasBetweenTheSpecifiedDates")]
        public BuveraReportViewModel GetAllBuverasBetweenTheSpecifiedDates(ReportSearch searchDates)
        {
            return _reportService.GetAllBuverasBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
        }


        [HttpPost]
        [ActionName("GetAllBuveraTotalsBetweenTheSpecifiedDates")]
        public GradeSizeTotalsViewModel GetAllBuveraTotalsBetweenTheSpecifiedDates(ReportSearch searchDates)
        {
            return _reportService.GetAllBuveraTotalsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
        }
        #endregion



        #region utility accounts         

        [HttpPost]
            [ActionName("GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory")]
            public IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory(ReportSearch searchDates)
            {
                return _reportService.GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.CategoryId);
            }
            #endregion

            #region advance,creditors,debtors

            [HttpGet]
            [ActionName("GenerateAdvancePaymentReport")]
            public DebtorReportViewModel GenerateAdvancePaymentReport()
            {
                return _reportService.GenerateAdvancePaymentReport();
            }

            [HttpGet]
            [ActionName("GenerateCreditorReport")]
            public CreditorReportViewModel GenerateCreditorReport()
            {
                return _reportService.GenerateCreditorReport();
            }

            [HttpGet]
            [ActionName("GenerateDebtorReport")]
            public DebtorReportViewModel GenerateDebtorReport()
            {
                return _reportService.GenerateDebtorReport();
            }

        [HttpPost]
        [ActionName("GenerateCreditorReportForAParticularDate")]
        public CreditorReportViewModel GenerateCreditorReportForAParticularDate(ReportSearch dateTime)
        {
            return _reportService.GenerateCreditorReportForAParticularDate(dateTime.ToDate);
        }

        [HttpPost]
        [ActionName("GenerateDebtorReportForAParticularDate")]
        public DebtorReportViewModel GenerateDebtorReportForAParticularDate(ReportSearch dateTime)
        {
            return _reportService.GenerateDebtorReportForAParticularDate(dateTime.ToDate);
        }

        [HttpPost]
        [ActionName("GenerateAdvancePaymentReportForAParticularDate")]
        public DebtorReportViewModel GenerateAdvancePaymentReportForAParticularDate(ReportSearch dateTime)
        {
            return _reportService.GenerateAdvancePaymentReportForAParticularDate(dateTime.ToDate);
        }
        #endregion

        #region deposits,recoveries and discounts
        [HttpPost]
            [ActionName("GenerateDepositsReport")]
            public DepositsReportViewModel GenerateDepositsReport(ReportSearch searchDates)
            {
                return _reportService.GenerateDepositsReport(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.SupplierId);
            }
        [HttpPost]
        [ActionName("GenerateRecoveriesReport")]
        public DepositsReportViewModel GenerateRecoveriesReport(ReportSearch searchDates)
        {
            return _reportService.GenerateRecoveriesReport(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.SupplierId);
        }
        [HttpPost]
        [ActionName("GenerateDiscountsReport")]
        public DepositsReportViewModel GenerateDiscountsReport(ReportSearch searchDates)
        {
            return _reportService.GenerateDiscountsReport(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.SupplierId);
        }
        #endregion

        #region weightloss
        [HttpPost]
        [ActionName("GetAllWeightLossesBetweenTheSpecifiedDates")]
        public WeightLossesReportViewModel GetAllWeightLossesBetweenTheSpecifiedDates(ReportSearch searchDates)
        {
            return _reportService.GetAllWeightLossesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.SupplierId);
        }

        #endregion

        #region outsourcers
        [HttpPost]
        [ActionName("GetAllOutSourcerOutPutsBetweenTheSpecifiedDates")]
        public OutSourcerOutPutReportViewModel GetAllOutSourcerOutPutsBetweenTheSpecifiedDates(ReportSearch searchDates)
        {
            return _reportService.GetAllOutPutsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
        }
        [HttpPost]
        [ActionName("GetAllOutPutTotalsBetweenTheSpecifiedDates")]
        public GradeSizeTotalsViewModel GetAllOutPutTotalsBetweenTheSpecifiedDates(ReportSearch searchDates)
        {
            return _reportService.GetAllOutPutTotalsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
        }

        [HttpPost]
        [ActionName("GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct")]
        public DeliveryReportViewModel GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(ReportSearch searchDates)
        {
            return _reportService.GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId,searchDates.CustomerId,searchDates.ProductId);
        }
        [HttpPost]
        [ActionName("GetAllDeliveryTotalsBetweenTheSpecifiedDates")]
        public GradeSizeTotalsViewModel GetAllDeliveryTotalsBetweenTheSpecifiedDates(ReportSearch searchDates)
        {
            return _reportService.GetAllDeliveryTotalsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId,searchDates.CustomerId,searchDates.ProductId);
        }

        #endregion

        #region rice inputs
        [HttpPost]
        [ActionName("GetAllRiceInputsBetweenTheSpecifiedDatesForBranch")]
        public RiceInputReportViewModel GetAllRiceInputsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
        {
            return _reportService.GetAllRiceInputsBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId);
        }
        #endregion

    }
}
