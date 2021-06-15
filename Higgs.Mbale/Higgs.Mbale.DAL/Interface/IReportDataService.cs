using System;
using System.Collections.Generic;

using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IReportDataService
  {
      #region transactions
      IEnumerable<Transaction> GetAllTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate);

         IEnumerable<Transaction> GenerateTransactionCurrentMonthReport();

         IEnumerable<Transaction> GenerateTransactionTodaysReport();

         IEnumerable<Transaction> GenerateTransactionCurrentWeekReport();
              #endregion

         #region supplies
         #region web
         IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate,long branchId,string supplierId);
        
         IEnumerable<Supply> GenerateSupplyCurrentMonthReport();
        
          IEnumerable<Supply> GenerateSupplyTodaysReport();

          IEnumerable<Supply> GenerateSupplyCurrentWeekReport();
#endregion

          #region branch
          IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);

          IEnumerable<Supply> GenerateSupplyCurrentMonthReportForBranch(long branchId);

          IEnumerable<Supply> GenerateSupplyTodaysReportForBranch(long branchId);

          IEnumerable<Supply> GenerateSupplyCurrentWeekReportForBranch(long branchId);
         
          #endregion
         #endregion
          #region supplier for particular supplier

          IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate,string supplierId);

          IEnumerable<Supply> GenerateSupplyCurrentMonthReportForAParticularSupplier(string supplierId);

          IEnumerable<Supply> GenerateSupplyTodaysReportForAParticularSupplier(string supplierId);

          IEnumerable<Supply> GenerateSupplyCurrentWeekReportForAParticularSupplier(string supplierId);
      #endregion

      #region accountTransactions
          
           IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentMonthReport();
         
           IEnumerable<AccountTransactionActivity> GenerateAccountTransactionTodaysReport();
         
          IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentWeekReport();

          IEnumerable<AccountTransactionActivity> GetAllAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);
        

      #endregion

      #region batches
          #region web
          IEnumerable<Batch> GetAllBatchesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
         
           IEnumerable<Batch> GenerateBatchCurrentMonthReport();
         
           IEnumerable<Batch> GenerateBatchTodaysReport();

           IEnumerable<Batch> GenerateBatchCurrentWeekReport();
          #endregion
           #region branch
           IEnumerable<Batch> GetAllBatchesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

           IEnumerable<Batch> GenerateBatchCurrentMonthReportForBranch(long branchId);

           IEnumerable<Batch> GenerateBatchTodaysReportForBranch(long branchId);

           IEnumerable<Batch> GenerateBatchCurrentWeekReportForBranch(long branchId); 

           #endregion
      #endregion


           #region Deliveries
           #region web
           IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);
           
            IEnumerable<Delivery> GenerateDeliveryCurrentMonthReport();
         
            IEnumerable<Delivery> GenerateDeliveryTodaysReport();
          
            IEnumerable<Delivery> GenerateDeliveryCurrentWeekReport();
            IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId);
           #endregion

            #region branch
            IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);

            IEnumerable<Delivery> GenerateDeliveryCurrentMonthReportForBranch(long branchId);

            IEnumerable<Delivery> GenerateDeliveryTodaysReportForBranch(long branchId);

            IEnumerable<Delivery> GenerateDeliveryCurrentWeekReportForBranch(long branchId);
            IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId);

        #endregion

        #endregion
        #region rice inputs
        IEnumerable<RiceInput> GetAllRiceInputsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
        #endregion


        #region Orders
        #region web
        IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);

            IEnumerable<Order> GenerateOrderCurrentMonthReport();

            IEnumerable<Order> GenerateOrderTodaysReport();

            IEnumerable<Order> GenerateOrderCurrentWeekReport();
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
            IEnumerable<FactoryExpense> GetAllFactoryExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
           
             IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentMonthReport();
           
             IEnumerable<FactoryExpense> GenerateFactoryExpenseTodaysReport();
           
             IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentWeekReport();
            #endregion
             #region branch
             IEnumerable<FactoryExpense> GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

             IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentMonthReportForBranch(long branchId);

             IEnumerable<FactoryExpense> GenerateFactoryExpenseTodaysReportForBranch(long branchId);

             IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentWeekReportForBranch(long branchId);
           

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
             #endregion
            #region branch
            IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

            IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReportForBranch(long branchId);

            IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReportForBranch(long branchId);

            IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReportForBranch(long branchId);

            #endregion
             #endregion

            #region  LabourCosts
            #region web
            IEnumerable<LabourCost> GetAllLabourCostsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);         

             IEnumerable<LabourCost> GenerateLabourCostCurrentMonthReport();

             IEnumerable<LabourCost> GenerateLabourCostTodaysReport();
           
             IEnumerable<LabourCost> GenerateLabourCostCurrentWeekReport();
            #endregion
             #region branch
             IEnumerable<LabourCost> GetAllLabourCostsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

             IEnumerable<LabourCost> GenerateLabourCostCurrentMonthReportForBranch(long branchId);

             IEnumerable<LabourCost> GenerateLabourCostTodaysReportForBranch(long branchId);

             IEnumerable<LabourCost> GenerateLabourCostCurrentWeekReportForBranch(long branchId);

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
             IEnumerable<FlourTransfer> GetAllFlourTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status);
            
             IEnumerable<FlourTransfer> GenerateFlourTransferCurrentMonthReport();
           
             IEnumerable<FlourTransfer> GenerateFlourTransferTodaysReport();

             IEnumerable<FlourTransfer> GenerateFlourTransferCurrentWeekReport();
             #endregion
             #region branch
             IEnumerable<FlourTransfer> GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status,string position);

             IEnumerable<FlourTransfer> GenerateFlourTransferCurrentMonthReportForBranch(long branchId);

              IEnumerable<FlourTransfer> GenerateFlourTransferTodaysReportForBranch(long branchId);
      
             IEnumerable<FlourTransfer> GenerateFlourTransferCurrentWeekReportForBranch(long branchId);
            
             #endregion
            #endregion

             #region casual account transactions

             IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId);
      #region branch
      IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId);
      #endregion
      #endregion

      #region  cashsale
             IEnumerable<CashSale> GetAllCashSalesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
             IEnumerable<CashSale> GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId);
       
             IEnumerable<CashSale> GenerateCashSaleCurrentMonthReport();
             IEnumerable<CashSale> GenerateCashSaleTodaysReport();

             IEnumerable<CashSale> GenerateCashSaleCurrentWeekReport();
             #region branch
             
             IEnumerable<CashSale> GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId);
             IEnumerable<CashSale> GenerateCashSaleCurrentMonthReportForBranch(long branchId);
             IEnumerable<CashSale> GenerateCashSaleTodaysReportForBranch(long branchId);

             IEnumerable<CashSale> GenerateCashSaleCurrentWeekReportForBranch(long branchId);

             #endregion

             #endregion

             #region cashtransfer
             IEnumerable<CashTransfer> GetAllCashTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long fromBranchId, long toBranchId, string status);
           IEnumerable<CashTransfer> GenerateCashTransferCurrentMonthReport();
           IEnumerable<CashTransfer> GenerateCashTransferTodaysReport();
           IEnumerable<CashTransfer> GenerateCashTransferCurrentWeekReport();

      #region branch
            IEnumerable<CashTransfer> GenerateCashTransferCurrentMonthReportForBranch(long branchId);
            IEnumerable<CashTransfer> GenerateCashTransferTodaysReportForBranch(long branchId);
            IEnumerable<CashTransfer> GenerateCashTransferCurrentWeekReportForBranch(long branchId);
            IEnumerable<CashTransfer> GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status,string position);
    
      #endregion
             #endregion

            #region cash
            IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

            IEnumerable<Cash> GenerateCashCurrentMonthReport();

            IEnumerable<Cash> GenerateCashTodaysReport();

            IEnumerable<Cash> GenerateCashCurrentWeekReport();

            #region  expenses and incomes
            #region expenses
            IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

            IEnumerable<Cash> GenerateExpensesCurrentMonthReport();

            IEnumerable<Cash> GenerateExpensesTodaysReport();

            IEnumerable<Cash> GenerateExpensesCurrentWeekReport();

            IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,long requistionCategoryId);

        #endregion

        #region incomes
        IEnumerable<Cash> GetAllIncomesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

            IEnumerable<Cash> GenerateIncomesCurrentMonthReport();

            IEnumerable<Cash> GenerateIncomesTodaysReport();

            IEnumerable<Cash> GenerateIncomesCurrentWeekReport();
            #endregion
            #endregion

      #region branch expenses and incomes
            #region expenses
            IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

            IEnumerable<Cash> GenerateExpensesCurrentMonthReportForBranch(long branchId);

            IEnumerable<Cash> GenerateExpensesTodaysReportForBranch(long branchId);

            IEnumerable<Cash> GenerateExpensesCurrentWeekReportForBranch(long branchId);
            IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long requistionCategoryId);

        #endregion

        #region incomes
        IEnumerable<Cash> GetAllIncomesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);

            IEnumerable<Cash> GenerateIncomesCurrentMonthReportForBranch(long branchId);

            IEnumerable<Cash> GenerateIncomesTodaysReportForBranch(long branchId);

            IEnumerable<Cash> GenerateIncomesCurrentWeekReportForBranch(long branchId);
      #endregion

            #region cash
            IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
        #endregion
        #endregion
        #endregion


        #region  BuveraTransfer
        #region web
        IEnumerable<BuveraTransfer> GetAllBuveraTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status);

        #endregion
        #region branch
        IEnumerable<BuveraTransfer> GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position);
      
       #endregion
        #endregion
        #region buvera
        IEnumerable<Buvera> GetAllBuverasBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);


        #endregion
        #region branch
        IEnumerable<Buvera> GetAllBuverasBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId);
        
         #endregion

                #region utilityaccounts
                #region web

                IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long categoryId);
      

            #endregion
            #region branch

            IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategoryForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long categoryId);
           

            #endregion
            #endregion

            #region deposits,recoveries and discounts

            IEnumerable<AccountTransactionActivity> GetAllDepositsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId, long transactionSubTypeId);

        IEnumerable<AccountTransactionActivity> GetAllRecoveriesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId, long transactionSubTypeId);
        IEnumerable<AccountTransactionActivity> GetAllDiscountsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId, long transactionSubTypeId);

        #endregion

        #region weightLoss
        #region web
        IEnumerable<WeightLoss> GetAllWeightLossesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);
        

        #endregion
        #region branch
        IEnumerable<WeightLoss> GetAllWeightLossesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId);

        #endregion
        #endregion

        #region outsourcer
        IEnumerable<OutSourcerOutPut> GetAllOutPutsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId);

        IEnumerable<Delivery> GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId, string customerId, long productId);
        #endregion
    }
}
