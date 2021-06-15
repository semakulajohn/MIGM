using System;
using System.Collections.Generic;
using System.Linq;

using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;

using log4net;

namespace Higgs.Mbale.DAL.Concrete
{
  public  class ReportDataService : DataServiceBase, IReportDataService
    {
          ILog logger = log4net.LogManager.GetLogger(typeof(ReportDataService));

       public ReportDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }
       #region Transactions
       public IEnumerable<Transaction> GetAllTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate)
       {
           return this.UnitOfWork.Get<Transaction>().AsQueryable()
               .Where(m => m.Deleted ==false &&(m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<Transaction> GenerateTransactionCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Transaction>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Transaction> GenerateTransactionTodaysReport()
       {
           return this.UnitOfWork.Get<Transaction>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Transaction> GenerateTransactionCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Transaction>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }


       #endregion

       #region accountTransactions
       public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentMonthReport()
       {
           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionTodaysReport()
       {
           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<AccountTransactionActivity> GenerateAccountTransactionCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }

       public IEnumerable<AccountTransactionActivity> GetAllAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
       {
           if (branchId != 0 && supplierId == null)
           {

               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }
           else if (supplierId != null && branchId == 0)
           {

               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId);
           }
           else if (supplierId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId && m.BranchId == branchId);
           }
           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       #endregion

       #region Supplies
       #region web
       public IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate,long branchId,string supplierId)
       {
           if (branchId != 0 && supplierId == null)
           {

               return this.UnitOfWork.Get<Supply>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.SupplyDate >= lowerSpecifiedDate && m.SupplyDate <= upperSpecifiedDate) && m.BranchId == branchId && m.Approved == true);
           }
           else if (supplierId != null && branchId == 0)
           {

               return this.UnitOfWork.Get<Supply>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.SupplyDate >= lowerSpecifiedDate && m.SupplyDate <= upperSpecifiedDate)&& m.SupplierId == supplierId && m.Approved == true);
           }
           else if (supplierId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(m => m.Deleted == false && (m.SupplyDate >= lowerSpecifiedDate && m.SupplyDate <= upperSpecifiedDate) && m.SupplierId == supplierId && m.BranchId == branchId && m.Approved == true);
           }
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(m => m.Deleted == false && (m.SupplyDate >= lowerSpecifiedDate && m.SupplyDate <= upperSpecifiedDate)&& m.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted == false && p.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyTodaysReport()
       {
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted ==false && p.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Deleted ==false && p.Approved == true);
       }
       #endregion
       #region branch
       public IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId)
       {
           if (branchId != 0 && ((supplierId == null)||(supplierId == string.Empty)))
           {

               return this.UnitOfWork.Get<Supply>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.SupplyDate >= lowerSpecifiedDate && m.SupplyDate <= upperSpecifiedDate) && m.BranchId == branchId && m.Approved == true);
           }
          
           else if (supplierId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(m => m.Deleted == false && (m.SupplyDate >= lowerSpecifiedDate && m.SupplyDate <= upperSpecifiedDate) && m.SupplierId == supplierId && m.BranchId == branchId && m.Approved == true);
           }
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(m => m.Deleted == false && (m.SupplyDate >= lowerSpecifiedDate && m.SupplyDate <= upperSpecifiedDate) && m.BranchId == branchId && m.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted == false && p.BranchId == branchId && p.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted == false && p.BranchId == branchId && p.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Deleted == false && p.BranchId == branchId && p.Approved == true);
       }

       #endregion
       #endregion

       #region supplies for particular supplier and branch
       public IEnumerable<Supply> GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate,string supplierId)
       {
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate)&& m.SupplierId == supplierId && m.Deleted==false && m.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyCurrentMonthReportForAParticularSupplier(string supplierId)
       {
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.SupplierId == supplierId && p.Deleted==false && p.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyTodaysReportForAParticularSupplier(string supplierId)
       {
           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.SupplierId == supplierId && p.Deleted == false && p.Approved == true);
       }

       public IEnumerable<Supply> GenerateSupplyCurrentWeekReportForAParticularSupplier(string supplierId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Supply>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.SupplierId == supplierId && p.Deleted == false && p.Approved == true);
       }
        #endregion

        #region  batches
       #region web
       public IEnumerable<Batch> GetAllBatchesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0 )
           {

               return this.UnitOfWork.Get<Batch>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }
          
          
           return this.UnitOfWork.Get<Batch>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<Batch> GenerateBatchCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Batch>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted ==false);
       }

       public IEnumerable<Batch> GenerateBatchTodaysReport()
       {
           return this.UnitOfWork.Get<Batch>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted ==false);
       }

       public IEnumerable<Batch> GenerateBatchCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Batch>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Deleted ==false);
       }
       #endregion
       #region branch
       public IEnumerable<Batch> GetAllBatchesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<Batch>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return null;
       }

       public IEnumerable<Batch> GenerateBatchCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Batch>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted == false && p.BranchId == branchId);
       }

       public IEnumerable<Batch> GenerateBatchTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Batch>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Deleted == false && p.BranchId == branchId);
       }

       public IEnumerable<Batch> GenerateBatchCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Batch>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Deleted == false && p.BranchId == branchId);
       }
       #endregion
        #endregion

       #region  Factory Expenses
       #region web
       public IEnumerable<FactoryExpense> GetAllFactoryExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentMonthReport()
       {
           return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<FactoryExpense> GenerateFactoryExpenseTodaysReport()
       {
           return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion
       #region branch
       public IEnumerable<FactoryExpense> GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return null;
       }

       public IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<FactoryExpense> GenerateFactoryExpenseTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<FactoryExpense> GenerateFactoryExpenseCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<FactoryExpense>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }
       #endregion

       #endregion

       #region  Other Expenses
       #region web
       public IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReport()
       {
           return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReport()
       {
           return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion
       #region branch

       public IEnumerable<OtherExpense> GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return null;
       }

       public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<OtherExpense> GenerateOtherExpenseTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<OtherExpense> GenerateOtherExpenseCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<OtherExpense>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }
       #endregion
       #endregion

       #region  batchoutputs
       #region web
       public IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReport()
       {
           return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReport()
       {
           return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion

       #region branch
       public IEnumerable<BatchOutPut> GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return null;
       }

       public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<BatchOutPut> GenerateBatchOutPutTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId ==  branchId);
       }

       public IEnumerable<BatchOutPut> GenerateBatchOutPutCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<BatchOutPut>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }

       #endregion
       #endregion

       #region  LabourCosts
       #region web
       public IEnumerable<LabourCost> GetAllLabourCostsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<LabourCost>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<LabourCost>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<LabourCost> GenerateLabourCostCurrentMonthReport()
       {
           return this.UnitOfWork.Get<LabourCost>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<LabourCost> GenerateLabourCostTodaysReport()
       {
           return this.UnitOfWork.Get<LabourCost>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<LabourCost> GenerateLabourCostCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<LabourCost>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion
       #region branch
       public IEnumerable<LabourCost> GetAllLabourCostsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<LabourCost>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return null;
       }

       public IEnumerable<LabourCost> GenerateLabourCostCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<LabourCost>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<LabourCost> GenerateLabourCostTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<LabourCost>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<LabourCost> GenerateLabourCostCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<LabourCost>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }

       #endregion
       #endregion

       #region  MachineRepair
       #region web
       public IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReport()
       {
           return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<MachineRepair> GenerateMachineRepairTodaysReport()
       {
           return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }

       #endregion
       #region branch
       public IEnumerable<MachineRepair> GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return null;
       }

       public IEnumerable<MachineRepair> GenerateMachineRepairCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<MachineRepair> GenerateMachineRepairTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<MachineRepair> GenerateMachineRepairCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<MachineRepair>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }

       #endregion
       #endregion

       #region  Utility
       public IEnumerable<Utility> GetAllUtilitiesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<Utility>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<Utility>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<Utility> GenerateUtilityCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Utility>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Utility> GenerateUtilityTodaysReport()
       {
           return this.UnitOfWork.Get<Utility>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Utility> GenerateUtilityCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Utility>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }

       #endregion


       #region  FlourTransfer
       #region web
       public IEnumerable<FlourTransfer> GetAllFlourTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status)
       {
           if (branchId != 0 && (status == string.Empty || status ==null))
           {

               return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }
           else if (branchId != 0 && status == "Accepted")
           {
               return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Accept == true);
        
           }
           else if (branchId != 0 && status == "Rejected")
           {
               return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == true);
        
           }
            else if (branchId != 0 && status == "OutStanding")
            {
                return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == false && m.Accept == false);

            }
            return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<FlourTransfer> GenerateFlourTransferCurrentMonthReport()
       {
           return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<FlourTransfer> GenerateFlourTransferTodaysReport()
       {
           return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<FlourTransfer> GenerateFlourTransferCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion
       #region branch
       public IEnumerable<FlourTransfer> GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status,string position)
       {
           if (position == string.Empty || position == null)
           {
               if (branchId != 0 && (status == string.Empty || status == null))
               {

                   return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                       .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
               }
               else if (branchId != 0  && status == "Accepted")
               {
                   return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                       .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Accept == true);

               }
               else if (branchId != 0 && status == "Rejected")
               {
                   return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                       .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == true);

               }
                else if (branchId != 0 && status == "OutStanding")
                {
                    return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == false && m.Accept == false);

                }

            }
           else 
           {
               if (position == "Transfered")
               {
                   if (branchId != 0 && (status == string.Empty || status == null))
                   {

                       return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                           .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId);
                   }
                   else if (branchId != 0 && status == "Accepted")
                   {
                       return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                           .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId && m.Accept == true);

                   }
                   else if (branchId != 0 && status == "Rejected")
                   {
                       return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                           .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId && m.Reject == true);

                   }
                    else if (branchId != 0 && status == "OutStanding")
                    {
                        return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId && m.Reject == false && m.Accept == false);

                    }

                }
               else if(position == "Received")
               {
                        if (branchId != 0 && (status == string.Empty || status == null))
                        {

                            return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
                        }
                        else if (branchId != 0 && status == "Accepted")
                        {
                            return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Accept == true);

                        }
                        else if (branchId != 0 && status == "Rejected")
                        {
                            return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == true);

                        }
                    else if (branchId != 0 && status == "OutStanding")
                    {
                        return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == false && m.Accept == false);

                    }

                }
           }
            return null;
       }

       public IEnumerable<FlourTransfer> GenerateFlourTransferCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<FlourTransfer> GenerateFlourTransferTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<FlourTransfer> GenerateFlourTransferCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<FlourTransfer>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }
       #endregion
       #endregion

       #region Deliveries
       #region web
       public IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
       {
           if (branchId != 0 && customerId == null)
           {

               return this.UnitOfWork.Get<Delivery>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Approved == true);
           }
           else if (customerId != null && branchId == 0)
           {

               return this.UnitOfWork.Get<Delivery>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.Approved == true);
           }
           else if (customerId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId && m.Approved == true);
           }
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Approved == true);
       }

       public IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId,long productId)
       {
           if (branchId != 0 && customerId == null)
           {

               return this.UnitOfWork.Get<Delivery>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.ProductId == productId && m.Approved == true);
           }
           else if (customerId != null && branchId == 0)
           {

               return this.UnitOfWork.Get<Delivery>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.ProductId == productId && m.Approved == true);
           }
           else if (customerId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId && m.ProductId == productId && m.Approved == true);
           }
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ProductId == productId && m.Approved == true);
       }

       public IEnumerable<Delivery> GenerateDeliveryCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Approved == true);
       }

       public IEnumerable<Delivery> GenerateDeliveryTodaysReport()
       {
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Approved ==true);
       }

       public IEnumerable<Delivery> GenerateDeliveryCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Approved == true);
       }
       #endregion
       #region branch
       public IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
       {
           if (branchId != 0 && customerId == null)
           {

               return this.UnitOfWork.Get<Delivery>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Approved == true);
           }
           
           else if (customerId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId && m.Approved == true);
           }
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Approved == true);
       }

       public IEnumerable<Delivery> GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId, long productId)
       {
           if (branchId != 0 && customerId == null)
           {

               return this.UnitOfWork.Get<Delivery>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.ProductId == productId && m.Approved == true);
           }
          
           else if (customerId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId && m.ProductId == productId && m.Approved == true);
           }
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.ProductId == productId && m.Approved == true);
       }

       public IEnumerable<Delivery> GenerateDeliveryCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId && p.Approved == true);
       }

       public IEnumerable<Delivery> GenerateDeliveryTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId && p.Approved == true);
       }

       public IEnumerable<Delivery> GenerateDeliveryCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Delivery>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId && p.Approved == true);
       }

        #endregion
        #endregion

        #region rice inputs
        #region web
       
        public IEnumerable<RiceInput> GetAllRiceInputsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            if (branchId != 0 )
            {

                return this.UnitOfWork.Get<RiceInput>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Approved == true);
            }

              return this.UnitOfWork.Get<RiceInput>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Approved == true);
        }


        #endregion
        #endregion
        #region cash

        public IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<Cash>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<Cash> GenerateCashCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Cash> GenerateCashTodaysReport()
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Cash> GenerateCashCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }

       #region expenses
       public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<Cash>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action == "-");
           }


           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Action =="-");
       }

       public IEnumerable<Cash> GenerateExpensesCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action == "-");
       }

       public IEnumerable<Cash> GenerateExpensesTodaysReport()
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action =="-");
       }

       public IEnumerable<Cash> GenerateExpensesCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Action =="-");
       }

        public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,long requistionCategoryId)
        {
            if (branchId != 0 && requistionCategoryId != 0)
            {

                return this.UnitOfWork.Get<Cash>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action == "-" && m.RequistionCategoryId == requistionCategoryId);
            }


            else if (branchId != 0 )
            {

                return this.UnitOfWork.Get<Cash>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action == "-");
            }
            else if (branchId == 0 && requistionCategoryId !=0)
            {

                return this.UnitOfWork.Get<Cash>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Action == "-" && m.RequistionCategoryId == requistionCategoryId);
            }
            return this.UnitOfWork.Get<Cash>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Action == "-" );
        }

       
        #endregion

        #region incomes
       public IEnumerable<Cash> GetAllIncomesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<Cash>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action =="+");
           }


           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Action == "+");
       }

       public IEnumerable<Cash> GenerateIncomesCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action =="+");
       }

       public IEnumerable<Cash> GenerateIncomesTodaysReport()
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action =="+");
       }

       public IEnumerable<Cash> GenerateIncomesCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Action =="+");
       }
       #endregion

       #region branch
       #region expenses
       public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<Cash>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action == "-");
           }
           return null;

       }

       public IEnumerable<Cash> GenerateExpensesCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action == "-" && p.BranchId == branchId);
       }

       public IEnumerable<Cash> GenerateExpensesTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action == "-" && p.BranchId == branchId);
       }

       public IEnumerable<Cash> GenerateExpensesCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Action == "-" && p.BranchId == branchId);
       }

        public IEnumerable<Cash> GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long requistionCategoryId)
      {
            if (branchId != 0 && requistionCategoryId != 0)
            {

                return this.UnitOfWork.Get<Cash>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action == "-" && m.RequistionCategoryId == requistionCategoryId);
            }
                        
            else if (branchId != 0)
            {

                return this.UnitOfWork.Get<Cash>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action == "-");
            }

            return null;



        }

        #endregion

        #region incomes
        public IEnumerable<Cash> GetAllIncomesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<Cash>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Action == "+");
           }


           return null;
       }

       public IEnumerable<Cash> GenerateIncomesCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action == "+" && p.BranchId == branchId);
       }

       public IEnumerable<Cash> GenerateIncomesTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.Action == "+" && p.BranchId == branchId);
       }

       public IEnumerable<Cash> GenerateIncomesCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Cash>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.Action == "+" && p.BranchId == branchId);
       }
       #endregion

        #region cash
       public IEnumerable<Cash> GetAllCashBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
         

               return this.UnitOfWork.Get<Cash>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
          
       }

        #endregion


       #endregion
        #endregion

       #region orders
       #region web
       public IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
       {
           if (branchId != 0 && customerId == null)
           {

               return this.UnitOfWork.Get<Order>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }
           else if (customerId != null && branchId == 0)
           {

               return this.UnitOfWork.Get<Order>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId);
           }
           else if (customerId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId);
           }
           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<Order> GenerateOrderCurrentMonthReport()
       {
           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Order> GenerateOrderTodaysReport()
       {
           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<Order> GenerateOrderCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion
       #region branch
       public IEnumerable<Order> GetAllOrdersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
       {
           if (branchId != 0 && customerId == null)
           {

               return this.UnitOfWork.Get<Order>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }
           
           else if (customerId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId);
           }
           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
       }

       public IEnumerable<Order> GenerateOrderCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<Order> GenerateOrderTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<Order> GenerateOrderCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<Order>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }
       #endregion
       #endregion

       #region casual accounttransactions
       #region web
       public IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId)
       {
           if (branchId != 0 && supplierId == null)
           {

               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }
           else if (supplierId != null && branchId == 0)
           {

               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CasualWorkerId == supplierId);
           }
           else if (supplierId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CasualWorkerId == supplierId && m.BranchId == branchId);
           }
           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }
       #endregion 
       #region branch
       public IEnumerable<AccountTransactionActivity> GetAllCasualAccountTransactionsBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long supplierId)
       {
                
           if (supplierId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CasualWorkerId == supplierId && m.BranchId == branchId);
           }
           return null;
       }
       #endregion
        #endregion

       #region  CashSale
       #region web
       public IEnumerable<CashSale> GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,long productId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<CashSale>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.ProductId == productId);
           }


           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ProductId == productId);
       }

       public IEnumerable<CashSale> GetAllCashSalesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<CashSale>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
           }


           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<CashSale> GenerateCashSaleCurrentMonthReport()
       {
           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<CashSale> GenerateCashSaleTodaysReport()
       {
           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<CashSale> GenerateCashSaleCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion
       #region branch
       public IEnumerable<CashSale> GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long productId)
       {
           if (branchId != 0)
           {

               return this.UnitOfWork.Get<CashSale>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.ProductId == productId);
           }


           return null;
       }

       //public IEnumerable<CashSale> GetAllCashSalesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
       //{
       //    if (branchId != 0)
       //    {

       //        return this.UnitOfWork.Get<CashSale>().AsQueryable()
       //            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
       //    }


       //    return null;
       //}

       public IEnumerable<CashSale> GenerateCashSaleCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<CashSale> GenerateCashSaleTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.BranchId == branchId);
       }

       public IEnumerable<CashSale> GenerateCashSaleCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<CashSale>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.BranchId == branchId);
       }

       #endregion

       #endregion

       #region  CashTransfers
       #region web
       public IEnumerable<CashTransfer> GetAllCashTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long fromBranchId,long toBranchId,string status)
       {
          
           if (fromBranchId != 0 && toBranchId != 0 && status == "Accepted")
           {
               return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == fromBranchId && m.ToReceiverBranchId == toBranchId && m.Accept == true);

           }
           else if (fromBranchId != 0 && toBranchId != 0 && status == "Rejected")
           {
               return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == fromBranchId && m.ToReceiverBranchId == toBranchId && m.Reject == true);

           }
            else if (fromBranchId != 0 && toBranchId != 0 && status == "OutStanding")
            {
                return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == fromBranchId && m.ToReceiverBranchId == toBranchId && m.Reject == false && m.Accept == false);

            }
            else if (fromBranchId != 0 && toBranchId != 0)
           {

               return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == fromBranchId && m.ToReceiverBranchId == toBranchId);
           }
           return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
       }

       public IEnumerable<CashTransfer> GenerateCashTransferCurrentMonthReport()
       {
           return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<CashTransfer> GenerateCashTransferTodaysReport()
       {
           return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year);
       }

       public IEnumerable<CashTransfer> GenerateCashTransferCurrentWeekReport()
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate);
       }
       #endregion
       #region branch
       public IEnumerable<CashTransfer> GenerateCashTransferCurrentMonthReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.FromBranchId == branchId);
       }

       public IEnumerable<CashTransfer> GenerateCashTransferTodaysReportForBranch(long branchId)
       {
           return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
               .Where(p => p.CreatedOn.Day == DateTime.Now.Day && p.CreatedOn.Month == DateTime.Now.Month && p.CreatedOn.Year == DateTime.Now.Year && p.FromBranchId == branchId);
       }

       public IEnumerable<CashTransfer> GenerateCashTransferCurrentWeekReportForBranch(long branchId)
       {

           DateTime startOfWeek = DateTime.Today.AddDays((int)DateTime.Today.DayOfWeek * -1);
           DateTime endDate = DateTime.Now;

           return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
               .Where(p => p.CreatedOn >= startOfWeek && p.CreatedOn <= endDate && p.FromBranchId == branchId);
       }
       public IEnumerable<CashTransfer> GetAllCashTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,string status,string position)
       {
           //if (branchId != 0)
           //{

           //    return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
           //        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == branchId);
           //}


           //return null;

            if (position == string.Empty || position == null)
            {
                if (branchId != 0 && (status == string.Empty || status == null))
                {

                    return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId);
                }
                else if (branchId != 0 && status == "Accepted")
                {
                    return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId && m.Accept == true);

                }
                else if (branchId != 0 && status == "Rejected")
                {
                    return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId && m.Reject == true);

                }
                else if (branchId != 0 && status == "OutStanding")
                {
                    return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId && m.Reject == false && m.Accept == false);

                }

            }
            else
            {
                if (position == "Transfered")
                {
                    if (branchId != 0 && (status == string.Empty || status == null))
                    {

                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == branchId);
                    }
                    else if (branchId != 0 && status == "Accepted")
                    {
                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == branchId && m.Accept == true);

                    }
                    else if (branchId != 0 && status == "Rejected")
                    {
                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == branchId && m.Reject == true);

                    }
                    else if (branchId != 0 && status == "OutStanding")
                    {
                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromBranchId == branchId && m.Reject == false && m.Accept == false);

                    }

                }
                else if (position == "Received")
                {
                    if (branchId != 0 && (status == string.Empty || status == null))
                    {

                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId);
                    }
                    else if (branchId != 0 && status == "Accepted")
                    {
                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId && m.Accept == true);

                    }
                    else if (branchId != 0 && status == "Rejected")
                    {
                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId && m.Reject == true);

                    }
                    else if (branchId != 0 && status == "OutStanding")
                    {
                        return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ToReceiverBranchId == branchId && m.Reject == false && m.Accept == false);

                    }

                }
            }
            return null;
        }

        #endregion

        #endregion

        #region  buveraTransfer
        #region web
        public IEnumerable<BuveraTransfer> GetAllBuveraTransfersBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status)
        {
            if (branchId != 0 && (status == string.Empty || status == null))
            {

                return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
            }
            else if (branchId != 0 && status == "Accepted")
            {
                return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Accept == true);

            }
            else if (branchId != 0 && status == "Rejected")
            {
                return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == true);

            }
            else if (branchId != 0 && status == "OutStanding")
            {
                return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == false && m.Accept == false);

            }

            return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
        }

        #endregion
        #region branch
        public IEnumerable<BuveraTransfer> GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string status, string position)
        {
            if (position == string.Empty || position == null)
            {
                if (branchId != 0 && (status == string.Empty || status == null))
                {

                    return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
                }
                else if (branchId != 0 && status == "Accepted")
                {
                    return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Accept == true);

                }
                else if (branchId != 0 && status == "Rejected")
                {
                    return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == true);

                }
                else if (branchId != 0 && status == "OutStanding")
                {
                    return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                        .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == false && m.Accept == false);

                }

            }
            else
            {
                if (position == "Transfered")
                {
                    if (branchId != 0 && (status == string.Empty || status == null))
                    {

                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId);
                    }
                    else if (branchId != 0 && status == "Accepted")
                    {
                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId && m.Accept == true);

                    }
                    else if (branchId != 0 && status == "Rejected")
                    {
                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId && m.Reject == true);

                    }
                    else if (branchId != 0 && status == "OutStanding")
                    {
                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.FromSupplierStoreId == branchId && m.Reject == false && m.Accept == false);

                    }

                }
                else if (position == "Received")
                {
                    if (branchId != 0 && (status == string.Empty || status == null))
                    {

                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
                    }
                    else if (branchId != 0 && status == "Accepted")
                    {
                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Accept == true);

                    }
                    else if (branchId != 0 && status == "Rejected")
                    {
                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == true);

                    }
                    else if (branchId != 0 && status == "OutStanding")
                    {
                        return this.UnitOfWork.Get<BuveraTransfer>().AsQueryable()
                            .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Reject == false && m.Accept == false);

                    }


                }
            }
            return null;
        }


        #endregion
        #endregion
        #region  buvera

        #region web
        public IEnumerable<Buvera> GetAllBuverasBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            if (branchId != 0)
            {

                return this.UnitOfWork.Get<Buvera>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
            }


            return this.UnitOfWork.Get<Buvera>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate));
        }

        #endregion
        #region branch
        public IEnumerable<Buvera> GetAllBuverasBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId)
        {
            if (branchId != 0)
            {

                return this.UnitOfWork.Get<Buvera>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
            }


            return null;
        }

     
        #endregion
        #endregion


        #region utilityaccounts
        #region web

        public IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId,  long categoryId)
       {
         
           if (categoryId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<UtilityAccount>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.UtilityCategoryId == categoryId && m.BranchId == branchId);
           }
           return this.UnitOfWork.Get<UtilityAccount>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) );
       }

       #endregion
       #region branch

       public IEnumerable<UtilityAccount> GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategoryForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, long categoryId)
       {
          

           if (categoryId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<UtilityAccount>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.UtilityCategoryId == categoryId && m.BranchId == branchId);
           }
           return this.UnitOfWork.Get<UtilityAccount>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId);
       }


       #endregion
        #endregion

        #region deposits,recoveries and discounts
        //deposits
       public IEnumerable<AccountTransactionActivity> GetAllDepositsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId,long transactionSubTypeId)
       {
           if (branchId != 0 && supplierId == null)
           {

               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.TransactionSubTypeId== transactionSubTypeId);
           }
           else if (supplierId != null && branchId == 0)
           {

               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                   .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId && m.TransactionSubTypeId == transactionSubTypeId);
           }
           else if (supplierId != null && branchId != 0)
           {
               return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId && m.BranchId == branchId && m.TransactionSubTypeId == transactionSubTypeId);
           }
           return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
               .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.TransactionSubTypeId== transactionSubTypeId);
       }
        //recoveries
        public IEnumerable<AccountTransactionActivity> GetAllRecoveriesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId, long transactionSubTypeId)
        {
            if (branchId != 0 && supplierId == null)
            {

                return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.TransactionSubTypeId == transactionSubTypeId);
            }
            else if (supplierId != null && branchId == 0)
            {

                return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId && m.TransactionSubTypeId == transactionSubTypeId);
            }
            else if (supplierId != null && branchId != 0)
            {
                return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId && m.BranchId == branchId && m.TransactionSubTypeId == transactionSubTypeId);
            }
            return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.TransactionSubTypeId == transactionSubTypeId);
        }

        //discounts
        public IEnumerable<AccountTransactionActivity> GetAllDiscountsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId, long transactionSubTypeId)
        {
            if (branchId != 0 && supplierId == null)
            {

                return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.TransactionSubTypeId == transactionSubTypeId);
            }
            else if (supplierId != null && branchId == 0)
            {

                return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId && m.TransactionSubTypeId == transactionSubTypeId);
            }
            else if (supplierId != null && branchId != 0)
            {
                return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.AspNetUserId == supplierId && m.BranchId == branchId && m.TransactionSubTypeId == transactionSubTypeId);
            }
            return this.UnitOfWork.Get<AccountTransactionActivity>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.TransactionSubTypeId == transactionSubTypeId);
        }

        #endregion

        #region weightLoss
        #region web
        public IEnumerable<WeightLoss> GetAllWeightLossesBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            if (branchId != 0 && customerId == null)
            {

                return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Quantity > 0);
            }
            else if (customerId != null && branchId == 0)
            {

                return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.Quantity > 0);
            }
            else if (customerId != null && branchId != 0)
            {
                return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId && m.Quantity > 0);
            }
            return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Quantity > 0);
        }
              
        #endregion
        #region branch
        public IEnumerable<WeightLoss> GetAllWeightLossesBetweenTheSpecifiedDatesForBranch(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string customerId)
        {
            if (branchId != 0 && customerId == null)
            {

                return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Quantity > 0);
            }

            else if (customerId != null && branchId != 0)
            {
                return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.BranchId == branchId);
            }
            return this.UnitOfWork.Get<WeightLoss>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.BranchId == branchId && m.Quantity > 0);
        }


        #endregion
        #endregion

        #region outsourcer
       
        public IEnumerable<OutSourcerOutPut> GetAllOutPutsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId)
        {
            if (storeId != 0)
            {

                return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate)  && m.Approved == true && m.StoreId == storeId);
            }
           
          
            return this.UnitOfWork.Get<OutSourcerOutPut>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.Approved == true);
        }

        public IEnumerable<Delivery> GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long storeId, string customerId, long productId)
        {
            if (storeId != 0 && customerId == null)
            {

                return this.UnitOfWork.Get<Delivery>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.StoreId == storeId && m.ProductId == productId && m.Approved == true);
            }
            else if (customerId != null && storeId == 0)
            {

                return this.UnitOfWork.Get<Delivery>().AsQueryable()
                    .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.ProductId == productId && m.Approved == true);
            }
            else if (customerId != null && storeId != 0)
            {
                return this.UnitOfWork.Get<Delivery>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.CustomerId == customerId && m.StoreId == storeId && m.ProductId == productId && m.Approved == true);
            }
            return this.UnitOfWork.Get<Delivery>().AsQueryable()
                .Where(m => m.Deleted == false && (m.CreatedOn >= lowerSpecifiedDate && m.CreatedOn <= upperSpecifiedDate) && m.ProductId == productId && m.Approved == true);
        }

        #endregion
    }
}
