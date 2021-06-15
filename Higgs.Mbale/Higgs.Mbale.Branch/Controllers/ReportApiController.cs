using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.ViewModel;

namespace Higgs.Mbale.Branch.Controllers
{
    public class ReportApiController : ApiController
    {
          private IReportService _reportService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(ReportApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public ReportApiController()
            {
            }

            public ReportApiController(IReportService reportService,IUserService userService)
            {
                this._reportService = reportService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }


          
        #region supplies
            [HttpPost]
            [ActionName("GetAllSuppliesBetweenTheSpecifiedDatesForBranch")]
            public SupplyReportViewModel GetAllSuppliesBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllSuppliesBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate,branchId,searchDates.SupplierId);
            }

            [HttpGet]
            [ActionName("GenerateSupplyCurrentMonthReportForBranch")]
            public SupplyReportViewModel GenerateSupplyCurrentMonthReportForBranch()
            {
                return _reportService.GenerateSupplyCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateSupplyTodaysReportForBranch")]
            public SupplyReportViewModel GenerateSupplyTodaysReportForBranch()
            {
                return _reportService.GenerateSupplyTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateSupplyCurrentWeekReportForBranch")]
            public SupplyReportViewModel GenerateSupplyCurrentWeekReportForBranch()
            {
                return _reportService.GenerateSupplyCurrentWeekReportForBranch(branchId);
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
            [ActionName("GetAllBatchesBetweenTheSpecifiedDatesForBranch")]
            public BatchReportViewModel GetAllBatchesBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllBatchesBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchCurrentMonthReportForBranch")]
            public BatchReportViewModel GenerateBatchCurrentMonthReportForBranch()
            {
                return _reportService.GenerateBatchCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchTodaysReportForBranch")]
            public BatchReportViewModel GenerateBatchTodaysReportForBranch()
            {
                return _reportService.GenerateBatchTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchCurrentWeekReportForBranch")]
            public BatchReportViewModel GenerateBatchCurrentWeekReportForBranch()
            {
                return _reportService.GenerateBatchCurrentWeekReportForBranch(branchId);
            }
            #endregion

         #region deliveries
            [HttpPost]
            [ActionName("GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch")]
            public DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.CustomerId,searchDates.ProductId);
            }

            [HttpGet]
            [ActionName("GenerateDeliveryCurrentMonthReportForBranch")]
            public DeliveryReportViewModel GenerateDeliveryCurrentMonthReportForBranch()
            {
                return _reportService.GenerateDeliveryCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateDeliveryTodaysReportForBranch")]
            public DeliveryReportViewModel GenerateDeliveryTodaysReportForBranch()
            {
                return _reportService.GenerateDeliveryTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateDeliveryCurrentWeekReportForBranch")]
            public DeliveryReportViewModel GenerateDeliveryCurrentWeekReportForBranch()
            {
                return _reportService.GenerateDeliveryCurrentWeekReportForBranch(branchId);
            }

            [HttpPost]
            [ActionName("GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct")]
            public GradeSizeTotalsViewModel GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct(ReportSearch searchDates)
            {
                return _reportService.GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.CustomerId, searchDates.ProductId);
            }
        #endregion

        #region FactoryExpenses
        [HttpPost]
            [ActionName("GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch")]
            public FactoryExpenseReportViewModel GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateFactoryExpenseCurrentMonthReportForBranch")]
            public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentMonthReportForBranch()
            {
                return _reportService.GenerateFactoryExpenseCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateFactoryExpenseTodaysReportForBranch")]
            public FactoryExpenseReportViewModel GenerateFactoryExpenseTodaysReportForBranch()
            {
                return _reportService.GenerateFactoryExpenseTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateFactoryExpenseCurrentWeekReportForBranch")]
            public FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentWeekReportForBranch()
            {
                return _reportService.GenerateFactoryExpenseCurrentWeekReportForBranch(branchId);
            }
            #endregion

            #region orders
            [HttpPost]
            [ActionName("GetAllOrdersBetweenTheSpecifiedDatesForBranch")]
            public IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllOrdersBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.CustomerId);
            }

            [HttpGet]
            [ActionName("GenerateOrderCurrentMonthReportForBranch")]
            public IEnumerable<Order> GenerateOrderCurrentMonthReportForBranch()
            {
                return _reportService.GenerateOrderCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateOrderTodaysReportForBranch")]
            public IEnumerable<Order> GenerateOrderTodaysReportForBranch()
            {
                return _reportService.GenerateOrderTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateOrderCurrentWeekReportForBranch")]
            public IEnumerable<Order> GenerateOrderCurrentWeekReportForBranch()
            {
                return _reportService.GenerateOrderCurrentWeekReportForBranch(branchId);
            }
            #endregion

            #region Cash      

            #region branch
            #region expenses
            [HttpPost]
            [ActionName("GetAllExpensesBetweenTheSpecifiedDatesForBranch")]
            public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllExpensesBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateExpensesCurrentMonthReportForBranch")]
            public IEnumerable<Cash> GenerateExpensesCurrentMonthReportForBranch()
            {
                return _reportService.GenerateExpensesCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateExpensesTodaysReportForBranch")]
            public IEnumerable<Cash> GenerateExpensesTodaysReportForBranch()
            {
                return _reportService.GenerateExpensesTodaysReportForBranch(branchId);
            }
            [HttpGet]
            [ActionName("GenerateExpensesCurrentWeekReportForBranch")]
            public IEnumerable<Cash> GenerateExpensesCurrentWeekReportForBranch()
            {

                return _reportService.GenerateExpensesCurrentWeekReportForBranch(branchId);
            }

            [HttpPost]
            [ActionName("GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory")]
            public CashReportViewModel GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory(ReportSearch searchDates)
            {
                return _reportService.GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory(searchDates.FromDate, searchDates.ToDate, branchId,searchDates.RequistionCategoryId);
            }

        #endregion

        #region incomes
        [HttpPost]
            [ActionName("GetAllIncomesBetweenTheSpecifiedDatesForBranch")]
            public CashReportViewModel GetAllIncomesBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {

                return _reportService.GetAllIncomesBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateIncomesCurrentMonthReportForBranch")]
            public IEnumerable<Cash> GenerateIncomesCurrentMonthReportForBranch()
            {
                return _reportService.GenerateIncomesCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateIncomesTodaysReportForBranch")]
            public IEnumerable<Cash> GenerateIncomesTodaysReportForBranch()
            {
                return _reportService.GenerateIncomesTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateIncomesCurrentWeekReportForBranch")]
            public IEnumerable<Cash> GenerateIncomesCurrentWeekReportForBranch()
            {
                return _reportService.GenerateIncomesCurrentWeekReportForBranch(branchId);
            }

            #endregion

            #region cash
            [HttpPost]
            [ActionName("GetAllCashBetweenTheSpecifiedDatesForBranch")]
            public IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllCashBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }
            #endregion
            #endregion
            #endregion

            #region OtherExpenses
            [HttpPost]
            [ActionName("GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch")]
            public IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateOtherExpenseCurrentMonthReportForBranch")]
            public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReportForBranch()
            {
                return _reportService.GenerateOtherExpenseCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateOtherExpenseTodaysReportForBranch")]
            public IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReportForBranch()
            {
                return _reportService.GenerateOtherExpenseTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateOtherExpenseCurrentWeekReportForBranch")]
            public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReportForBranch()
            {
                return _reportService.GenerateOtherExpenseCurrentWeekReportForBranch(branchId);
            }
            #endregion

            #region LabourCosts
            [HttpPost]
            [ActionName("GetAllLabourCostsBetweenTheSpecifiedDatesForBranch")]
            public LabourCostReportViewModel GetAllLabourCostsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllLabourCostsBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateLabourCostCurrentMonthReportForBranch")]
            public LabourCostReportViewModel GenerateLabourCostCurrentMonthReportForBranch()
            {
                return _reportService.GenerateLabourCostCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateLabourCostTodaysReportForBranch")]
            public LabourCostReportViewModel GenerateLabourCostTodaysReportForBranch()
            {
                return _reportService.GenerateLabourCostTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateLabourCostCurrentWeekReportForBranch")]
            public LabourCostReportViewModel GenerateLabourCostCurrentWeekReportForBranch()
            {
                return _reportService.GenerateLabourCostCurrentWeekReportForBranch(branchId);
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
            [ActionName("GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch")]
            public FlourTransferReportViewModel GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId,searchDates.Status,searchDates.Position);
            }

            [HttpGet]
            [ActionName("GenerateFlourTransferCurrentMonthReportForBranch")]
            public FlourTransferReportViewModel GenerateFlourTransferCurrentMonthReportForBranch()
            {
                return _reportService.GenerateFlourTransferCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateFlourTransferTodaysReportForBranch")]
            public FlourTransferReportViewModel GenerateFlourTransferTodaysReportForBranch()
            {
                return _reportService.GenerateFlourTransferTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateFlourTransferCurrentWeekReportForBranch")]
            public FlourTransferReportViewModel GenerateFlourTransferCurrentWeekReportForBranch()
            {
                return _reportService.GenerateFlourTransferCurrentWeekReportForBranch(branchId);
            }

            [HttpPost]
            [ActionName("GetAllFlourTransferTotalsBetweenTheSpecifiedDatesForBranch")]
            public FlourTransferReportTotalModel GetAllFlourTransferTotalsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
           
            return _reportService.GetAllFlourTransferTotalsBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate,branchId, searchDates.Status,searchDates.Position);
                   }
        #endregion

        #region MachineRepairs
        [HttpPost]
            [ActionName("GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch")]
            public IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateMachineRepairCurrentMonthReportForBranch")]
            public IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReportForBranch()
            {
                return _reportService.GenerateMachineRepairCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateMachineRepairTodaysReportForBranch")]
            public IEnumerable<MachineRepair> GenerateMachineRepairTodaysReportForBranch()
            {
                return _reportService.GenerateMachineRepairTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateMachineRepairCurrentWeekReportForBranch")]
            public IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReportForBranch()
            {
                return _reportService.GenerateMachineRepairCurrentWeekReportForBranch(branchId);
            }
            #endregion

            #region BatchOutPuts
            [HttpPost]
            [ActionName("GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch")]
            public IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchOutPutCurrentMonthReportForBranch")]
            public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReportForBranch()
            {
                return _reportService.GenerateBatchOutPutCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchOutPutTodaysReportForBranch")]
            public IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReportForBranch()
            {
                return _reportService.GenerateBatchOutPutTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateBatchOutPutCurrentWeekReportForBranch")]
            public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReportForBranch()
            {
                return _reportService.GenerateBatchOutPutCurrentWeekReportForBranch(branchId);
            }

            [HttpPost]
            [ActionName("GetAllFlourTotalsBetweenTheSpecifiedDatesForABranch")]
            public GradeSizeTotalsViewModel GetAllFlourTotalsBetweenTheSpecifiedDatesForABranch(ReportSearch searchDates)
            {
            return _reportService.GetAllFlourTotalsBetweenTheSpecifiedDatesForABranch(searchDates.FromDate,searchDates.ToDate,branchId);
            }
        #endregion

        #region casual accountTransactions



        [HttpPost]
            [ActionName("GenerateCasualAccountTransactionsBetweenTheSpecifiedDates")]
            public IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllCasualAccountTransactionsBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId, Convert.ToInt64(searchDates.SupplierId));

            }
            #endregion


            #region CashSale
            [HttpPost]
            [ActionName("GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch")]
            public CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch(searchDates.FromDate, searchDates.ToDate, branchId,searchDates.ProductId);
            }

            [HttpGet]
            [ActionName("GenerateCashSaleCurrentMonthReportForBranch")]
            public CashSaleReportViewModel GenerateCashSaleCurrentMonthReportForBranch()
            {
                return _reportService.GenerateCashSaleCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateCashSaleTodaysReportForBranch")]
            public CashSaleReportViewModel GenerateCashSaleTodaysReportForBranch()
            {
                return _reportService.GenerateCashSaleTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateCashSaleCurrentWeekReportForBranch")]
            public CashSaleReportViewModel GenerateCashSaleCurrentWeekReportForBranch()
            {
                return _reportService.GenerateCashSaleCurrentWeekReportForBranch(branchId);
            }

        [HttpPost]
        [ActionName("GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct")]
        public GradeSizeTotalsViewModel GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct(ReportSearch searchDates)
        {
            return _reportService.GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.ProductId);
        }
        #endregion

        #region CashTransfer
        [HttpPost]
            [ActionName("GetAllCashTransfersBetweenTheSpecifiedDatesForBranch")]
            public CashTransferReportViewModel GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId,searchDates.Status,searchDates.Position);
            }

            [HttpGet]
            [ActionName("GenerateCashTransferCurrentMonthReportForBranch")]
            public CashTransferReportViewModel GenerateCashTransferCurrentMonthReportForBranch()
            {
                return _reportService.GenerateCashTransferCurrentMonthReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateCashTransferTodaysReportForBranch")]
            public CashTransferReportViewModel GenerateCashTransferTodaysReportForBranch()
            {
                return _reportService.GenerateCashTransferTodaysReportForBranch(branchId);
            }

            [HttpGet]
            [ActionName("GenerateCashTransferCurrentWeekReportForBranch")]
            public CashTransferReportViewModel GenerateCashTransferCurrentWeekReportForBranch()
            {
                return _reportService.GenerateCashTransferCurrentWeekReportForBranch(branchId);
            }
            #endregion

            #region BuveraTransfer
            [HttpPost]
            [ActionName("GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch")]
            public BuveraTransferReportViewModel GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
            {
                return _reportService.GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId,searchDates.Status,searchDates.Position);
            }

        [HttpPost]
        [ActionName("GetAllBuveraTransferTotalsBetweenTheSpecifiedDatesForBranch")]
        public GradeSizeTotalsViewModel GetAllBuveraTransferTotalsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
        {
            return _reportService.GetAllBuveraTransferTotalsBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.Status, searchDates.Position);
        }


        #endregion

        #region Buvera
        [HttpPost]
        [ActionName("GetAllBuverasBetweenTheSpecifiedDatesForBranch")]
        public BuveraReportViewModel GetAllBuverasBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
        {
            return _reportService.GetAllBuverasBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);
        }

        [HttpPost]
        [ActionName("GetAllBuveraTotalsBetweenTheSpecifiedDatesForBranch")]
        public GradeSizeTotalsViewModel GetAllBuveraTotalsBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
        {
            return _reportService.GetAllBuveraTotalsBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId);

        }



        #endregion


        #region deposits,recoveries,discounts
        [HttpPost]
            [ActionName("GenerateDepositsReport")]
            public DepositsReportViewModel GenerateDepositsReport(ReportSearch searchDates)
            {
                return _reportService.GenerateDepositsReport(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.SupplierId);
            }

        [HttpPost]
        [ActionName("GenerateDiscountsReport")]
        public DepositsReportViewModel GenerateDiscountsReport(ReportSearch searchDates)
        {
            return _reportService.GenerateDiscountsReport(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.SupplierId);
        }
        [HttpPost]
        [ActionName("GenerateRecoveriesReport")]
        public DepositsReportViewModel GenerateRecoveriesReport(ReportSearch searchDates)
        {
            return _reportService.GenerateRecoveriesReport(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.SupplierId);
        }
        #endregion


        #region weightLosses
        [HttpPost]
        [ActionName("GetAllWeightLossesBetweenTheSpecifiedDatesForBranch")]
        public WeightLossesReportViewModel GetAllWeightLossesBetweenTheSpecifiedDatesForBranch(ReportSearch searchDates)
        {
            return _reportService.GetAllWeightLossesBetweenTheSpecifiedDatesForBranch(searchDates.FromDate, searchDates.ToDate, branchId, searchDates.CustomerId);
        }
        //[HttpPost]
        //[ActionName("GetAllWeightLossesBetweenTheSpecifiedDates")]
        //public WeightLossesReportViewModel GetAllWeightLossesBetweenTheSpecifiedDates(ReportSearch searchDates)
        //{
        //    return _reportService.GetAllWeightLossesBetweenTheSpecifiedDates(searchDates.FromDate, searchDates.ToDate, searchDates.BranchId, searchDates.SupplierId);
        //}
        #endregion

        #region daily
        [HttpPost]
        [ActionName("GetAllActivitiesForAParticularBranchForASpecificPeriod")]
        public DailyReport GetAllActivitiesForAParticularBranchForASpecificPeriod(ReportSearch searchDates)
        {
            return _reportService.GetAllActivitiesForAParticularBranchForASpecificPeriod(searchDates.FromDate, searchDates.ToDate, branchId);
        }

        #endregion


    }
}
