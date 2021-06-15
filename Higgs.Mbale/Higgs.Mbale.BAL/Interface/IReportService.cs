using System;
using System.Collections.Generic;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.ViewModel;
using Higgs.Mbale.Models.ViewModel.consolidated;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IReportService
 {
     #region transactions
     IEnumerable<Transaction> GetAllTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate);

        IEnumerable<Transaction> GenerateTransactionCurrentMonthReport();

        IEnumerable<Transaction> GenerateTransactionTodaysReport();

        IEnumerable<Transaction> GenerateTransactionCurrentWeekReport();

#endregion

     #region supplies
        #region web
        SupplyReportViewModel GenerateSupplyCurrentMonthReport();

        SupplyReportViewModel GenerateSupplyCurrentWeekReport();

         SupplyReportViewModel GenerateSupplyTodaysReport();
         SupplyReportViewModel CalculateDifferentSupplySums(List<Supply> supplyList);
         SupplyReportViewModel GetAllSuppliesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);
        //ConsolidatedSupplyViewModel GetConsolidatedSuppliesForAParticularDate(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);
        #endregion

        #region branch
        SupplyReportViewModel GenerateSupplyCurrentMonthReportForBranch(long branchId);

        SupplyReportViewModel GenerateSupplyCurrentWeekReportForBranch(long branchId);

         SupplyReportViewModel GenerateSupplyTodaysReportForBranch(long branchId);

         SupplyReportViewModel GetAllSuppliesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);

#endregion
     #endregion

         #region supplies for supplier
         IEnumerable<Supply> GenerateSupplyCurrentMonthReportForAParticularSupplier(string supplierId);

         IEnumerable<Supply> GenerateSupplyCurrentWeekReportForAParticularSupplier(string supplierId);

         IEnumerable<Supply> GenerateSupplyTodaysReportForAParticularSupplier(string supplierId);

         IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate,string supplierId);

         #endregion

         #region accountTransactions
          IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentMonthReport();
        
          IEnumerable<AccountTransactionActivity> GenerateAccountTransactionTodaysReport();
        
          IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentWeekReport();

          IEnumerable<AccountTransactionActivity> GetAllAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);
       
         #endregion

          #region batches
          #region web
          BatchReportViewModel GenerateBatchCurrentMonthReport();

          BatchReportViewModel GenerateBatchCurrentWeekReport();

          BatchReportViewModel GenerateBatchTodaysReport();

          BatchReportViewModel GetAllBatchesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
          #endregion
          #region branch
          BatchReportViewModel GenerateBatchCurrentMonthReportForBranch(long branchId);

          BatchReportViewModel GenerateBatchCurrentWeekReportForBranch(long branchId);

          BatchReportViewModel GenerateBatchTodaysReportForBranch(long branchId);

          BatchReportViewModel GetAllBatchesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
         
          #endregion
          #endregion

          #region deliveries
          #region web
          DeliveryReportViewModel GenerateDeliveryCurrentMonthReport();

          DeliveryReportViewModel GenerateDeliveryCurrentWeekReport();

          DeliveryReportViewModel GenerateDeliveryTodaysReport();

          DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);

          DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId);

          GradeSizeTotalsViewModel GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId);

        #endregion
        #region branch
        DeliveryReportViewModel GenerateDeliveryCurrentMonthReportForBranch(long branchId);

          DeliveryReportViewModel GenerateDeliveryCurrentWeekReportForBranch(long branchId);

          DeliveryReportViewModel GenerateDeliveryTodaysReportForBranch(long branchId);

          DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);

          DeliveryReportViewModel GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId);

        #endregion
        #endregion
        #region rice inputs
        RiceInputReportViewModel GetAllRiceInputsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
        #endregion

        #region cash

        IEnumerable<Cash> GenerateCashCurrentMonthReport();

          IEnumerable<Cash> GenerateCashCurrentWeekReport();

          IEnumerable<Cash> GenerateCashTodaysReport();

          IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

          #region  expenses and incomes
          #region expenses
          IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);


          CashReportViewModel GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long requistionCategoryId);

        #endregion

        #region incomes
        
          IEnumerable<Cash> GenerateIncomesCurrentMonthReport();

          IEnumerable<Cash> GenerateIncomesTodaysReport();

          IEnumerable<Cash> GenerateIncomesCurrentWeekReport();
        CashReportViewModel GetAllIncomesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

        #endregion
        #endregion

        #region branch expenses and incomes
        #region expenses
        IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

          IEnumerable<Cash> GenerateExpensesCurrentMonthReportForBranch(long branchId);

          IEnumerable<Cash> GenerateExpensesTodaysReportForBranch(long branchId);

          IEnumerable<Cash> GenerateExpensesCurrentWeekReportForBranch(long branchId);

         CashReportViewModel GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,long requistionCategoryId);

        #endregion

        #region incomes
         CashReportViewModel GetAllIncomesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

          IEnumerable<Cash> GenerateIncomesCurrentMonthReportForBranch(long branchId);

          IEnumerable<Cash> GenerateIncomesTodaysReportForBranch(long branchId);

          IEnumerable<Cash> GenerateIncomesCurrentWeekReportForBranch(long branchId);
          #endregion
         
        #region cash
          IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
          #endregion
          #endregion
          #endregion

          #region orders
          #region web
          IEnumerable<Order> GenerateOrderCurrentMonthReport();

          IEnumerable<Order> GenerateOrderCurrentWeekReport();

          IEnumerable<Order> GenerateOrderTodaysReport();

          IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);
          #endregion
          #region branch
          IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);
       
            IEnumerable<Order> GenerateOrderCurrentMonthReportForBranch(long branchId);
        
        IEnumerable<Order> GenerateOrderTodaysReportForBranch(long branchId);
        
        IEnumerable<Order> GenerateOrderCurrentWeekReportForBranch(long branchId);
          #endregion
          #endregion

          #region  Factory Expenses
        #region web
        FactoryExpenseReportViewModel GetAllFactoryExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
        FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentMonthReport();
        FactoryExpenseReportViewModel GenerateFactoryExpenseTodaysReport();
        FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentWeekReport();
        #endregion
          #region branch
        FactoryExpenseReportViewModel GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
        FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentMonthReportForBranch(long branchId);
        FactoryExpenseReportViewModel GenerateFactoryExpenseTodaysReportForBranch(long branchId);
        FactoryExpenseReportViewModel GenerateFactoryExpenseCurrentWeekReportForBranch(long branchId);
          #endregion

          #endregion

          #region  Other Expenses
          #region web
          IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
           IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReport();
          
           IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReport();
          

           IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReport();
          #endregion
           #region branch
           IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
           IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReportForBranch(long branchId);

           IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReportForBranch(long branchId);
     
           IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReportForBranch(long branchId);

           #endregion

          #endregion

           #region  batchoutputs
           #region web
           IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
           IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReport();
        
           IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReport();
         
           IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReport();

            GradeSizeTotalsViewModel GetAllFlourTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

        #endregion
        #region branch
            IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
            IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReportForBranch(long branchId);
     
            IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReportForBranch(long branchId);
     
            IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReportForBranch(long branchId);
            //GradeSizeTotalsViewModel GetAllFlourTotalsBetweenTheSpecifiedDatesForABranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
        GradeSizeTotalsViewModel GetAllFlourTotalsBetweenTheSpecifiedDatesForABranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

           #endregion

        #endregion

        #region  LabourCosts
        #region web
        LabourCostReportViewModel GetAllLabourCostsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);  

           LabourCostReportViewModel GenerateLabourCostCurrentMonthReport();
        LabourCostReportViewModel GenerateLabourCostTodaysReport();
        LabourCostReportViewModel GenerateLabourCostCurrentWeekReport();
        #endregion
        #region branch
        LabourCostReportViewModel GetAllLabourCostsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

        LabourCostReportViewModel GenerateLabourCostCurrentMonthReportForBranch(long branchId);
        LabourCostReportViewModel GenerateLabourCostTodaysReportForBranch(long branchId);
        LabourCostReportViewModel GenerateLabourCostCurrentWeekReportForBranch(long branchId);
         

          #endregion

           #endregion

          #region  MachineRepair
          #region web
          IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
          
           IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReport();
         
           IEnumerable<MachineRepair> GenerateMachineRepairTodaysReport();
         
           IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReport();
          #endregion
           #region branch
           IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

           IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReportForBranch(long branchId);

           IEnumerable<MachineRepair> GenerateMachineRepairTodaysReportForBranch(long branchId);

           IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReportForBranch(long branchId);
         
           #endregion

          #endregion

           #region  Utility
           IEnumerable<Utility> GetAllUtilitiesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
           IEnumerable<Utility> GenerateUtilityCurrentMonthReport();
          
           IEnumerable<Utility> GenerateUtilityTodaysReport();
         
           IEnumerable<Utility> GenerateUtilityCurrentWeekReport();
          

          #endregion
              

           #region  FlourTransfer
           #region web
           FlourTransferReportViewModel GetAllFlourTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status);

           FlourTransferReportViewModel GenerateFlourTransferCurrentMonthReport();

           FlourTransferReportViewModel GenerateFlourTransferTodaysReport();

           FlourTransferReportViewModel GenerateFlourTransferCurrentWeekReport();

           FlourTransferReportTotalModel GetAllFlourTransferTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status);
           #endregion
           #region branch
           FlourTransferReportViewModel GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status,string position);

           FlourTransferReportViewModel GenerateFlourTransferCurrentMonthReportForBranch(long branchId);

           FlourTransferReportViewModel GenerateFlourTransferTodaysReportForBranch(long branchId);

           FlourTransferReportViewModel GenerateFlourTransferCurrentWeekReportForBranch(long branchId);

        FlourTransferReportTotalModel GetAllFlourTransferTotalsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position);

       
           #endregion

        #endregion

        #region casualaccounttransactions

        IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId);
           IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId);
     #endregion


           #region  CashSale
           CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

           CashSaleReportViewModel GenerateCashSaleCurrentMonthReport();


           CashSaleReportViewModel GenerateCashSaleTodaysReport();

           CashSaleReportViewModel GenerateCashSaleCurrentWeekReport();

           CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId);

        GradeSizeTotalsViewModel GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId);

        #region branch
        CashSaleReportViewModel GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId);


           CashSaleReportViewModel GenerateCashSaleCurrentMonthReportForBranch(long branchId);


           CashSaleReportViewModel GenerateCashSaleTodaysReportForBranch(long branchId);

           CashSaleReportViewModel GenerateCashSaleCurrentWeekReportForBranch(long branchId);

           #endregion
           #endregion

           #region cashtransfer
           CashTransferReportViewModel GetAllCashTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long fromBranchId, long toBranchId, string status);
           CashTransferReportViewModel GenerateCashTransferCurrentMonthReport();
           CashTransferReportViewModel GenerateCashTransferTodaysReport();
           CashTransferReportViewModel GenerateCashTransferCurrentWeekReport();
     #region branch
           CashTransferReportViewModel GenerateCashTransferCurrentMonthReportForBranch(long branchId);
           CashTransferReportViewModel GenerateCashTransferTodaysReportForBranch(long branchId);
           CashTransferReportViewModel GenerateCashTransferCurrentWeekReportForBranch(long branchId);
           CashTransferReportViewModel GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status,string position);
    
     #endregion
     #endregion


      #region  BuveraTransfer
      #region web
      BuveraTransferReportViewModel GetAllBuveraTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status);
        GradeSizeTotalsViewModel GetAllBuveraTransferTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status);


        #endregion
        #region branch
        BuveraTransferReportViewModel GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position);
        GradeSizeTotalsViewModel GetAllBuveraTransferTotalsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status,string position);
       

    #endregion

    #endregion

    #region buvera
    #region web
    BuveraReportViewModel GetAllBuverasBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

        GradeSizeTotalsViewModel GetAllBuveraTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

        #endregion
        #region branch
        BuveraReportViewModel GetAllBuverasBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

        GradeSizeTotalsViewModel GetAllBuveraTotalsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

        #endregion
        #endregion
        #region utilityaccounts
        #region web

        IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long categoryId);

      #endregion
      #region branch

      IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategoryForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long categoryId);

      #endregion
      #endregion

     #region advance,creditors,debtors
      DebtorReportViewModel GenerateAdvancePaymentReport();
      CreditorReportViewModel GenerateCreditorReport();
      DebtorReportViewModel GenerateDebtorReport();

        CreditorReportViewModel GenerateCreditorReportForAParticularDate(DateTime dateTime);
        DebtorReportViewModel GenerateDebtorReportForAParticularDate(DateTime dateTime);
        DebtorReportViewModel GenerateAdvancePaymentReportForAParticularDate(DateTime dateTime);
        #endregion

        #region deposits,recoveries and discounts

        DepositsReportViewModel GenerateDepositsReport(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);

        DepositsReportViewModel GenerateRecoveriesReport(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);

        DepositsReportViewModel GenerateDiscountsReport(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);
        #endregion

        #region weightLoss
        #region web
        WeightLossesReportViewModel GetAllWeightLossesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);


        #endregion
        #region branch
        WeightLossesReportViewModel GetAllWeightLossesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);
        #endregion
        #endregion

        #region daily
        DailyReport GetAllActivitiesForAParticularBranchForASpecificPeriod(DateTime fromDate, DateTime toDate, long branchId);
        #endregion

        #region outsourcer
        OutSourcerOutPutReportViewModel GetAllOutPutsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId);

        GradeSizeTotalsViewModel GetAllOutPutTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId);

        OutSourcerOutPutReportViewModel CalculateDifferentOutSourcerOutPutSums(List<OutSourcerOutPut> outPutList);

        GradeSizeTotalsViewModel GetTotalGradeSizeOutPutQuantities(List<OutSourcerOutPut> outPutList);

      DeliveryReportViewModel GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId, string customerId, long productId);
        GradeSizeTotalsViewModel GetAllDeliveryTotalsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId, string customerId, long productId);
        #endregion
    }
}
