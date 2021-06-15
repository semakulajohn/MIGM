using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Higgs.Mbale.Helpers;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.Models;
using Rotativa;
using Higgs.Mbale.Models.ViewModel;

namespace Higgs.Mbale.Web.Controllers
{
    public class ExcelController : Controller
    {
        
        private ITransactionService _transactionService;
        private IReportService _reportService;
        private ISupplyService _supplyService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private IBatchService _batchService;
        private IDeliveryService _deliveryService;
        private IOrderService _orderService;
        private ICashService _cashService;
        private ILabourCostService _labourCostService;
        private IOtherExpenseService _otherExpenseService;
        private IFactoryExpenseService _factoryExpenseService;
        private IBatchOutPutService _batchOutPutService;
        private IFlourTransferService _flourTransferService;
        private IMachineRepairService _machineRepairService;
        private IUtilityService _utilityService;
        private IRequistionService _requistionService;
        private IDocumentService _documentService;
        private ICreditorService _creditorService;
        private ICashSaleService _cashSaleService;

        public ExcelController()
        {

        }

        public ExcelController(ITransactionService transactionService, IReportService reportService,
            ISupplyService supplyService,IAccountTransactionActivityService accountTransactionActivityService,
            IBatchService batchService,IDeliveryService deliveryService,ICashService cashService,IOrderService orderService,
            ILabourCostService labourCostService, IOtherExpenseService otherExpenseService, IFactoryExpenseService factoryExpenseService,
            IBatchOutPutService batchOutPutService, IFlourTransferService flourTransferService, IMachineRepairService machineRepairService,
            ICashSaleService cashSaleService,
            IUtilityService utilityService,IRequistionService requistionService,IDocumentService documentService,ICreditorService creditorService)
        {
            this._transactionService = transactionService;
            this._reportService = reportService;
            this._supplyService = supplyService;
            this._accountTransactionActivityService = accountTransactionActivityService;
            this._batchService = batchService;
            this._deliveryService = deliveryService;
            this._orderService = orderService;
            this._cashService = cashService;
            this._labourCostService = labourCostService;
            this._otherExpenseService = otherExpenseService;
            this._factoryExpenseService = factoryExpenseService;
            this._batchOutPutService = batchOutPutService;
            this._flourTransferService = flourTransferService;
            this._machineRepairService = machineRepairService;
            this._utilityService = utilityService;
            this._requistionService = requistionService;
            this._documentService = documentService;
            this._creditorService = creditorService;
            this._cashSaleService = cashSaleService;
        }
        // GET: Excel
        public ActionResult Index(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("TransactionId ");
            headers.Add("Amount");
            headers.Add("TransactionSubTypeName");
            headers.Add("CreatedOn");
            headers.Add("TransactionType");
            headers.Add("Branch");

            IEnumerable<Transaction> transactionList;
            switch (reportType)
            {

                case 1://all todays transactions
                    nameOfReport = "TodaysTransactions";
                    transactionList = _reportService.GenerateTransactionTodaysReport();
                    break;
                case 2://all this months transactions
                    nameOfReport = "CurrentMonthsTransactions";
                    transactionList = _reportService.GenerateTransactionCurrentMonthReport();
                    break;
               
                case 3://transactions for this week
                    nameOfReport = "CurrentWeeksTransactions";
                    transactionList = _reportService.GenerateTransactionCurrentWeekReport();
                   break;
               
                default://Todo:: need to decide which one is the default report data
                    transactionList = _transactionService.GetAllTransactions();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in transactionList)
            {
               
                var sxr = new List<string>();
                sxr.Add(w.TransactionId.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.TransactionSubTypeName);
                sxr.Add(w.CreatedOn.ToString());
                sxr.Add(w.TransactionTypeName);
                sxr.Add(w.BranchName);

                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //public ActionResult Supply(int id)
        //{
        //    int reportType = id;
        //    string nameOfReport = string.Empty;
        //    List<string> headers = new List<string>();
        //    headers.Add("WeightNoteNumber ");
        //    headers.Add("Quantity");
        //    headers.Add("Price");
        //    headers.Add("Amount");
        //    headers.Add("BranchName");
        //    headers.Add("SupplyDate");
        //    headers.Add("TruckerNumber");
        //    headers.Add("SupplierName");
        //    headers.Add("SupplierNumber");
        //    headers.Add("NormalBags");
        //    headers.Add("StoneBags");
          

        //    SupplyReportViewModel supplyList;
        //    IEnumerable<Supply> supplyDefaultList;
        //    switch (reportType)
        //    {

        //        case 1://all todays Supplies
        //            nameOfReport = "TodaysSupplies";
        //            supplyList = _reportService.GenerateSupplyTodaysReport();
        //            break;
        //        case 2://all this months Supplies
        //            nameOfReport = "CurrentMonthsSupplies";
        //            supplyList = _reportService.GenerateSupplyCurrentMonthReport();
        //            break;
               
        //        case 3://Supplies for this week
        //            nameOfReport = "CurrentWeeksSupplies";
        //            supplyList = _reportService.GenerateSupplyCurrentWeekReport();
        //            break;
        //        //case 4: //supplies for a specified date
        //        //    nameOfReport ="SuppliesForSpecifiedDate"
        //        //        supplyList = _reportService.GetAllAccountTransactionsBetweenTheSpecifiedDates(DateTime lowerSpecifiedDate, DateTime upperSpecifiedDate, long branchId, string supplierId);
        //        default://Todo:: need to decide which one is the default report data
        //            supplyDefaultList = _supplyService.GetAllSupplies();
        //            break;
        //    }
        //    List<List<string>> cellValues = new List<List<string>>();
        //    foreach (var w in supplyList.Supplies)
        //    {
               
        //        var sxr = new List<string>();
        //        sxr.Add(w.WeightNoteNumber.ToString());
        //        sxr.Add(w.Quantity.ToString());
        //        sxr.Add(w.Price.ToString());
        //        sxr.Add(w.Amount.ToString());
        //        sxr.Add(w.BranchName);
        //        sxr.Add(w.SupplyDate.ToString());
        //        sxr.Add(w.TruckNumber);
        //        sxr.Add(w.SupplierName);
        //        sxr.Add(w.SupplierNumber);
        //        sxr.Add(w.NormalBags.ToString());
        //        sxr.Add(w.BagsOfStones.ToString());
                

        //        cellValues.Add(sxr);
        //    }
        //    var data = new ExcelData();
        //    data.Headers = headers;
        //    data.DataRows = cellValues;

        //    var file = new ExcelWriter();
        //    var excelFileContentInBytes = file.GenerateExcelFile(data);
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
        //    return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //}

        public ActionResult SupplyReport(int id)
        {

            ViewBag.supplier = "Sentongo";
            ViewBag.wwn = "334";

            return View();
        }

        public ActionResult ExportSupplyAsPDF(int id)
        {
            return new ActionAsPdf("SupplyReport", new {id = id })
            {
                FileName = "FileName.pdf"
            };
        }
     
        public ActionResult SupplierSupply(int reportTypeId,string  supplierId)
        {
            int reportType = reportTypeId;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("WeightNoteNumber ");
            headers.Add("Quantity");
            headers.Add("Price");
            headers.Add("Amount");
            headers.Add("BranchName");
            headers.Add("SupplyDate");
            headers.Add("TruckerNumber");
            headers.Add("SupplierName");
            headers.Add("SupplierNumber");
            headers.Add("NormalBags");
            headers.Add("StoneBags");


            IEnumerable<Supply> supplyList;
            switch (reportType)
            {

                case 1://all todays Supplies
                    nameOfReport = "TodaysSupplierSupplies";
                    supplyList = _reportService.GenerateSupplyTodaysReportForAParticularSupplier(supplierId);
                    break;
                case 2://all this months Supplies
                    nameOfReport = "CurrentMonthsSupplierSupplies";
                    supplyList = _reportService.GenerateSupplyCurrentMonthReportForAParticularSupplier(supplierId);
                    break;

                case 3://Supplies for this week
                    nameOfReport = "CurrentWeeksSupplierSupplies";
                    supplyList = _reportService.GenerateSupplyCurrentWeekReportForAParticularSupplier(supplierId);
                    break;

                default://Todo:: need to decide which one is the default report data
                    supplyList = _supplyService.GetAllSuppliesForAParticularSupplier(supplierId);
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in supplyList)
            {

                var sxr = new List<string>();
                sxr.Add(w.WeightNoteNumber.ToString());
                sxr.Add(w.Quantity.ToString());
                sxr.Add(w.Price.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.BranchName);
                sxr.Add(w.SupplyDate.ToString());
                sxr.Add(w.TruckNumber);
                sxr.Add(w.SupplierName);
                sxr.Add(w.SupplierNumber);
                sxr.Add(w.NormalBags.ToString());
                sxr.Add(w.BagsOfStones.ToString());


                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }

        public ActionResult AccountTransaction(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("StartAmount ");
            headers.Add("Action");
            headers.Add("Amount");
            headers.Add("Balance");
            headers.Add("Branch");
            headers.Add("Notes");
            headers.Add("CreatedOn");
            
           

            IEnumerable<AccountTransactionActivity> accountTransactionList;
            switch (reportType)
            {

                case 1://all todays account transactions
                    nameOfReport = "TodaysTransactions";
                    accountTransactionList = _reportService.GenerateAccountTransactionTodaysReport();
                    break;
                case 2://all this months account transactions
                    nameOfReport = "CurrentMonthsTransactions";
                    accountTransactionList = _reportService.GenerateAccountTransactionCurrentMonthReport();
                    break;

                case 3:// account transactions for this week
                    nameOfReport = "CurrentWeeksTransactions";
                   accountTransactionList = _reportService.GenerateAccountTransactionCurrentWeekReport();
                    break;

                default://Todo:: need to decide which one is the default report data
                    accountTransactionList = _accountTransactionActivityService.GetAllAccountTransactionActivities();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in accountTransactionList)
            {

                var sxr = new List<string>();
                sxr.Add(w.StartAmount.ToString());
                sxr.Add(w.Action.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.Balance.ToString());
                sxr.Add(w.BranchName);
                sxr.Add(w.Notes);
                sxr.Add(w.CreatedOn.ToString());
                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //public ActionResult Batch(int id)
        //{
        //    int reportType = id;
        //    string nameOfReport = string.Empty;
        //    List<string> headers = new List<string>();
        //    headers.Add("BatchNumber ");
        //    headers.Add("Quantity");
           
        //    headers.Add("BranchName");
        //    headers.Add("CreatedOn");
            


        //    IEnumerable<Batch> batchList;
        //    switch (reportType)
        //    {

        //        case 1://all todays batches
        //            nameOfReport = "TodaysBatches";
        //            batchList = _reportService.GenerateBatchTodaysReport();
        //            break;
        //        case 2://all this months batches
        //            nameOfReport = "CurrentMonthsBatches";
        //            batchList = _reportService.GenerateBatchCurrentMonthReport();
        //            break;

        //        case 3://Batches for this week
        //            nameOfReport = "CurrentWeeksBatches";
        //            batchList = _reportService.GenerateBatchCurrentWeekReport();
        //            break;
             
        //        default://Todo:: need to decide which one is the default report data
        //            batchList = _batchService.GetAllBatches();
        //            break;
        //    }
        //    List<List<string>> cellValues = new List<List<string>>();
        //    foreach (var w in batchList)
        //    {

        //        var sxr = new List<string>();
        //        sxr.Add(w.Name.ToString());
        //        sxr.Add(w.Quantity.ToString());
        //        sxr.Add(w.BranchName);
        //        sxr.Add(w.CreatedOn.ToString());
    
        //        cellValues.Add(sxr);
        //    }
        //    var data = new ExcelData();
        //    data.Headers = headers;
        //    data.DataRows = cellValues;

        //    var file = new ExcelWriter();
        //    var excelFileContentInBytes = file.GenerateExcelFile(data);
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
        //    return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //}

        //public ActionResult Delivery(int id)
        //{
        //    int reportType = id;
        //    string nameOfReport = string.Empty;
        //    List<string> headers = new List<string>();
        //    headers.Add("Location ");
        //    headers.Add("OrderNummber");
        //    headers.Add("DriverName");
        //    headers.Add("DriverNIN");
        //    headers.Add("Quantity");
        //    headers.Add("BranchName");
           
        //    headers.Add("VehicleNumber");
        //    headers.Add("DeliveryCost");
        //    headers.Add("CustomerName");
        //    headers.Add("CreatedOn");
             

        //    IEnumerable<Delivery>  deliveryList;
        //    switch (reportType)
        //    {

        //        case 1://all todays Deliveries
        //            nameOfReport = "TodaysDeliveries";
        //            deliveryList = _reportService.GenerateDeliveryTodaysReport();
        //            break;
        //        case 2://all this months Deliveries
        //            nameOfReport = "CurrentMonthsDeliveries";
        //            deliveryList = _reportService.GenerateDeliveryCurrentMonthReport();
        //            break;

        //        case 3://Deliveries for this week
        //            nameOfReport = "CurrentWeeksDeliveries";
        //            deliveryList = _reportService.GenerateDeliveryCurrentWeekReport();
        //            break;
               
        //        default://Todo:: need to decide which one is the default report data
        //            deliveryList = _deliveryService.GetAllDeliveries();
        //            break;
        //    }
        //    List<List<string>> cellValues = new List<List<string>>();
        //    foreach (var w in deliveryList)
        //    {

        //        var sxr = new List<string>();
        //        sxr.Add(w.Location.ToString());
        //        sxr.Add(w.OrderId.ToString());
        //        sxr.Add(w.DriverName.ToString());
        //        sxr.Add(w.DriverNIN.ToString());
        //        sxr.Add(w.Quantity.ToString());
        //        sxr.Add(w.BranchName.ToString());
                
        //        sxr.Add(w.VehicleNumber);
        //        sxr.Add(w.DeliveryCost.ToString());
        //        sxr.Add(w.CustomerName.ToString());
        //        sxr.Add(w.CreatedOn.ToString());


        //        cellValues.Add(sxr);
        //    }
        //    var data = new ExcelData();
        //    data.Headers = headers;
        //    data.DataRows = cellValues;

        //    var file = new ExcelWriter();
        //    var excelFileContentInBytes = file.GenerateExcelFile(data);
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
        //    return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //}

        public ActionResult Cash(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("BranchName ");
            headers.Add("TransactionSubType");
            headers.Add("Notes");
            headers.Add("StartAmount");
            headers.Add("Action");
            headers.Add("Amount");
            headers.Add("Balance");
            headers.Add("Department");
            headers.Add("CreatedOn");



            IEnumerable<Cash> cashList;
            switch (reportType)
            {

                case 1://all todays batches
                    nameOfReport = "TodaysCash";
                    cashList = _reportService.GenerateCashTodaysReport();
                    break;
                case 2://all this months batches
                    nameOfReport = "CurrentMonthsCash";
                    cashList = _reportService.GenerateCashCurrentMonthReport();
                    break;

                case 3://Batches for this week
                    nameOfReport = "CurrentWeeksCash";
                    cashList = _reportService.GenerateCashCurrentWeekReport();
                    break;

                default://Todo:: need to decide which one is the default report data
                    cashList = _cashService.GetAllCash();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in cashList)
            {

                var sxr = new List<string>();
                sxr.Add(w.BranchName);
                sxr.Add(w.TransactionSubTypeName.ToString());
                sxr.Add(w.Notes.ToString());
                sxr.Add(w.StartAmount.ToString());
                sxr.Add(w.Action.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.Balance.ToString());
                sxr.Add(w.SectorName.ToString());
                sxr.Add(w.CreatedOn.ToString());

                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult Order(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("ProductName ");
            headers.Add("CustomerName");
            headers.Add("OrderNummber");
            headers.Add("Status");
            headers.Add("Quantity");
            headers.Add("BranchName");        
            headers.Add("CreatedOn");


            IEnumerable<Order> orderList;
            switch (reportType)
            {

                case 1://all todays Orders
                    nameOfReport = "TodaysOrders";
                    orderList = _reportService.GenerateOrderTodaysReport();
                    break;
                case 2://all this months Orders
                    nameOfReport = "CurrentMonthsOrders";
                    orderList = _reportService.GenerateOrderCurrentMonthReport();
                    break;

                case 3://Orders for this week
                    nameOfReport = "CurrentWeeksOrders";
                    orderList = _reportService.GenerateOrderCurrentWeekReport();
                    break;

                default://Todo:: need to decide which one is the default report data
                    orderList = _orderService.GetAllOrders();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in orderList)
            {

                var sxr = new List<string>();
                sxr.Add(w.ProductName.ToString());
                sxr.Add(w.CustomerName.ToString());
                sxr.Add(w.OrderId.ToString());
                sxr.Add(w.StatusName.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.BranchName.ToString()); 
                sxr.Add(w.CreatedOn.ToString());


                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //public ActionResult FactoryExpense(int id)
        //{
        //    int reportType = id;
        //    string nameOfReport = string.Empty;
        //    List<string> headers = new List<string>();
        //    headers.Add("Description ");
        //    headers.Add("Amount");
        //    headers.Add("Department");
        //    headers.Add("BranchName");
        //    headers.Add("CreatedOn");


        //    IEnumerable<FactoryExpense> factoryExpenseList;
        //    switch (reportType)
        //    {

        //        case 1://all todays FactoryExpenses
        //            nameOfReport = "TodaysFactoryExpenses";
        //            factoryExpenseList = _reportService.GenerateFactoryExpenseTodaysReport();
        //            break;
        //        case 2://all this months Orders
        //            nameOfReport = "CurrentMonthsFactoryExpenses";
        //            factoryExpenseList = _reportService.GenerateFactoryExpenseCurrentMonthReport();
        //            break;

        //        case 3://FactoryExpenses for this week
        //            nameOfReport = "CurrentWeeksFactoryExpenses";
        //            factoryExpenseList = _reportService.GenerateFactoryExpenseCurrentWeekReport();
        //            break;

        //        default://Todo:: need to decide which one is the default report data
        //            factoryExpenseList = _factoryExpenseService.GetAllFactoryExpenses();
        //            break;
        //    }
        //    List<List<string>> cellValues = new List<List<string>>();
        //    foreach (var w in factoryExpenseList)
        //    {

        //        var sxr = new List<string>();
        //        sxr.Add(w.Description.ToString());
        //        sxr.Add(w.Amount.ToString());
        //        sxr.Add(w.SectorName.ToString());
        //        sxr.Add(w.BranchName.ToString());
        //        sxr.Add(w.CreatedOn.ToString());


        //        cellValues.Add(sxr);
        //    }
        //    var data = new ExcelData();
        //    data.Headers = headers;
        //    data.DataRows = cellValues;

        //    var file = new ExcelWriter();
        //    var excelFileContentInBytes = file.GenerateExcelFile(data);
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
        //    return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //}

        public ActionResult OtherExpense(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("Description ");
            headers.Add("Amount");
            headers.Add("Department");
            headers.Add("BranchName");
            headers.Add("CreatedOn");


            IEnumerable<OtherExpense> otherExpenseList;
            switch (reportType)
            {

                case 1://all todays OtherExpenses
                    nameOfReport = "TodaysOtherExpenses";
                    otherExpenseList = _reportService.GenerateOtherExpenseTodaysReport();
                    break;
                case 2://all this months otherExpenses
                    nameOfReport = "CurrentMonthsOtherExpenses";
                    otherExpenseList = _reportService.GenerateOtherExpenseCurrentMonthReport();
                    break;

                case 3://Orders for this week
                    nameOfReport = "CurrentWeeksOtherExpenses";
                    otherExpenseList = _reportService.GenerateOtherExpenseCurrentWeekReport();
                    break;

                default://Todo:: need to decide which one is the default report data
                    otherExpenseList = _otherExpenseService.GetAllOtherExpenses();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in otherExpenseList)
            {

                var sxr = new List<string>();
                sxr.Add(w.Description.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.SectorName.ToString());
                sxr.Add(w.BranchName.ToString());
                sxr.Add(w.CreatedOn.ToString());


                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult MachineRepair(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("Repair's Name ");
            headers.Add("Amount");
            headers.Add("Description");
            headers.Add("DateRepaired");
          
            headers.Add("BranchName");
            headers.Add("CreatedOn");


            IEnumerable<MachineRepair> machineRepairList;
            switch (reportType)
            {

                case 1://all todays machineRepairs
                    nameOfReport = "TodaysMachineRepairs";
                    machineRepairList = _reportService.GenerateMachineRepairTodaysReport();
                    break;
                case 2://all this months Orders
                    nameOfReport = "CurrentMonthsMachineRepairs";
                    machineRepairList = _reportService.GenerateMachineRepairCurrentMonthReport();
                    break;

                case 3://MachineRepairs for this week
                    nameOfReport = "CurrentWeeksMachineRepairs";
                    machineRepairList = _reportService.GenerateMachineRepairCurrentWeekReport();
                    break;

                default://Todo:: need to decide which one is the default report data
                    machineRepairList = _machineRepairService.GetAllMachineRepairs();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in machineRepairList)
            {

                var sxr = new List<string>();
                sxr.Add(w.NameOfRepair.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.Description.ToString());
                sxr.Add(w.DateRepaired.ToString());
                
                sxr.Add(w.BranchName.ToString());
                sxr.Add(w.CreatedOn.ToString());


                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult Utility(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("Description ");
            headers.Add("Amount");
            headers.Add("Department");
            headers.Add("BranchName");
            headers.Add("CreatedOn");


            IEnumerable<Utility> utilityList;
            switch (reportType)
            {

                case 1://all todays Utilities
                    nameOfReport = "TodaysUtilities";
                    utilityList = _reportService.GenerateUtilityTodaysReport();
                    break;
                case 2://all this months Utilities
                    nameOfReport = "CurrentMonthsUtilities";
                    utilityList = _reportService.GenerateUtilityCurrentMonthReport();
                    break;

                case 3://Utilties for this week
                    nameOfReport = "CurrentWeeksUtilities";
                    utilityList = _reportService.GenerateUtilityCurrentWeekReport();
                    break;

                default://Todo:: need to decide which one is the default report data
                    utilityList = _utilityService.GetAllUtilities();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in utilityList)
            {

                var sxr = new List<string>();
                sxr.Add(w.Description.ToString());
                sxr.Add(w.Amount.ToString());
                sxr.Add(w.SectorName.ToString());
                sxr.Add(w.BranchName.ToString());
                sxr.Add(w.CreatedOn.ToString());


                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult BatchOutPut(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("Batch");
            headers.Add("Loss ");
            headers.Add("Loss(%)");
            headers.Add("Flour(kgs)");
            headers.Add("Flour(%)");
            headers.Add("Brand(kgs)");
            headers.Add("Brand(%)");
            headers.Add("BranchName");
            headers.Add("CreatedOn");


            IEnumerable<BatchOutPut> batchOutPutList;
            switch (reportType)
            {

                case 1://all todays BatchOutPuts
                    nameOfReport = "TodaysBatchOutPuts";
                    batchOutPutList = _reportService.GenerateBatchOutPutTodaysReport();
                    break;
                case 2://all this months BatchOutPuts
                    nameOfReport = "CurrentMonthsBatchOutPuts";
                    batchOutPutList = _reportService.GenerateBatchOutPutCurrentMonthReport();
                    break;

                case 3://Orders for this week
                    nameOfReport = "CurrentWeeksBatchOutPuts";
                    batchOutPutList = _reportService.GenerateBatchOutPutCurrentWeekReport();
                    break;

                default://Todo:: need to decide which one is the default report data
                    batchOutPutList = _batchOutPutService.GetAllBatchOutPuts();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in batchOutPutList)
            {

                var sxr = new List<string>();
                sxr.Add(w.BatchName.ToString());
                sxr.Add(w.Loss.ToString());
                sxr.Add(w.LossPercentage.ToString());
                sxr.Add(w.FlourOutPut.ToString());
                sxr.Add(w.FlourPercentage.ToString());
                sxr.Add(w.BrandOutPut.ToString());
                sxr.Add(w.BrandPercentage.ToString());
                sxr.Add(w.BranchName.ToString());
                sxr.Add(w.CreatedOn.ToString());


                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //public ActionResult FlourTransfer(int id)
        //{
        //    int reportType = id;
        //    string nameOfReport = string.Empty;
        //    List<string> headers = new List<string>();
        //    headers.Add("Quantity(kgs) ");
        //    headers.Add("From Store");
        //    headers.Add("Receiver Store");
        //    headers.Add("Accepted");
        //    headers.Add("Rejected");
        //    headers.Add("BranchName");
        //    headers.Add("CreatedOn");


        //    IEnumerable<FlourTransfer> flourTransferList;
        //    switch (reportType)
        //    {

        //        case 1://all todays flourTransfers
        //            nameOfReport = "TodaysFlourTransfers";
        //            flourTransferList = _reportService.GenerateFlourTransferTodaysReport();
        //            break;
        //        case 2://all this months Orders
        //            nameOfReport = "CurrentMonthsFlourTransfers";
        //            flourTransferList = _reportService.GenerateFlourTransferCurrentMonthReport();
        //            break;

        //        case 3://Orders for this week
        //            nameOfReport = "CurrentWeeksFlourTransfers";
        //            flourTransferList = _reportService.GenerateFlourTransferCurrentWeekReport();
        //            break;

        //        default://Todo:: need to decide which one is the default report data
        //            flourTransferList = _flourTransferService.GetAllFlourTransfers();
        //            break;
        //    }
        //    List<List<string>> cellValues = new List<List<string>>();
        //    foreach (var w in flourTransferList)
        //    {

        //        var sxr = new List<string>();
        //        sxr.Add(w.TotalQuantity.ToString());
        //        sxr.Add(w.StoreName.ToString());
        //        sxr.Add(w.ReceiverStoreName.ToString());
        //        sxr.Add(w.Accept.ToString());
        //        sxr.Add(w.Reject.ToString());
        //        sxr.Add(w.BranchName.ToString());
        //        sxr.Add(w.CreatedOn.ToString());


        //        cellValues.Add(sxr);
        //    }
        //    var data = new ExcelData();
        //    data.Headers = headers;
        //    data.DataRows = cellValues;

        //    var file = new ExcelWriter();
        //    var excelFileContentInBytes = file.GenerateExcelFile(data);
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
        //    return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //}

        //public ActionResult LabourCost(int id)
        //{
        //    int reportType = id;
        //    string nameOfReport = string.Empty;
        //    List<string> headers = new List<string>();
        //    headers.Add("ActivityName ");
        //    headers.Add("Quantity");
        //    headers.Add("Rate");
        //    headers.Add("Quantity");
        //    headers.Add("BranchName");
        //    headers.Add("CreatedOn");


        //    IEnumerable<LabourCost> labourCostList;
        //    switch (reportType)
        //    {

        //        case 1://all todays LabourCosts
        //            nameOfReport = "TodaysLabourCosts";
        //            labourCostList = _reportService.GenerateLabourCostTodaysReport();
        //            break;
        //        case 2://all this months LabourCosts
        //            nameOfReport = "CurrentMonthsLabourCosts";
        //            labourCostList = _reportService.GenerateLabourCostCurrentMonthReport();
        //            break;

        //        case 3://Orders for this week
        //            nameOfReport = "CurrentWeeksLabourCosts";
        //            labourCostList = _reportService.GenerateLabourCostCurrentWeekReport();
        //            break;

        //        default://Todo:: need to decide which one is the default report data
        //            labourCostList = _labourCostService.GetAllLabourCosts();
        //            break;
        //    }
        //    List<List<string>> cellValues = new List<List<string>>();
        //    foreach (var w in labourCostList)
        //    {

        //        var sxr = new List<string>();
        //        sxr.Add(w.ActivityName.ToString());
        //        sxr.Add(w.Quantity.ToString());
        //        sxr.Add(w.Rate.ToString());
        //        sxr.Add(w.Amount.ToString());
        //        sxr.Add(w.BranchName.ToString());
        //        sxr.Add(w.CreatedOn.ToString());


        //        cellValues.Add(sxr);
        //    }
        //    var data = new ExcelData();
        //    data.Headers = headers;
        //    data.DataRows = cellValues;

        //    var file = new ExcelWriter();
        //    var excelFileContentInBytes = file.GenerateExcelFile(data);
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
        //    return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //}

        public ActionResult Creditor(int id)
        {
            int reportType = id;
            string nameOfReport = string.Empty;
            List<string> headers = new List<string>();
            headers.Add("Number ");
            headers.Add("Creditor Name");
            headers.Add("Amount");
            


            IEnumerable<CreditorView> creditorList;
            switch (reportType)
            {

                case 1://all creditors
                    nameOfReport = "Creditors";
                    creditorList = _creditorService.GetCreditorView();
                    break;
               

                default://Todo:: need to decide which one is the default report data
                    creditorList = _creditorService.GetCreditorView();
                    break;
            }
            List<List<string>> cellValues = new List<List<string>>();
            foreach (var w in creditorList)
            {

                var sxr = new List<string>();
                sxr.Add(w.CreditorNumber.ToString());
                sxr.Add(w.CreditorName.ToString());
              
                sxr.Add(w.Amount.ToString());
               


                cellValues.Add(sxr);
            }
            var data = new ExcelData();
            data.Headers = headers;
            data.DataRows = cellValues;

            var file = new ExcelWriter();
            var excelFileContentInBytes = file.GenerateExcelFile(data);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nameOfReport + "_Report_" + DateTime.Now.ToString("yyyy-MM-dd-mm-ss") + ".xlsx");
            return new FileContentResult(excelFileContentInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


       

        #region generating pdf
        #region Requistion
        // GET: Report
        public ActionResult Requistion(long requistionId)
        {
            var requistion = _requistionService.GetRequistion(requistionId);
            if (requistion != null)
            {
                var requistionPdf = new Requistion()
                {
                    RequistionId = requistion.RequistionId,

                    Amount = requistion.Amount,

                    Description = requistion.Description,

                    RequistionNumber = requistion.RequistionNumber,

                    CreatedBy = requistion.CreatedBy,
                    AmountInWords = requistion.AmountInWords,

                    CreatedOn = requistion.CreatedOn,

                    BranchName = requistion.BranchName,
                    StatusName = requistion.StatusName,
                    ApprovedByName = requistion.ApprovedByName,

                };
                ViewBag.requistionPdf = requistionPdf;
            }
           

            return View();
        }

        public ActionResult ExportRequistionAsPDF(long requistionId)
        {
            return new ActionAsPdf("Requistion", new { requistionId = requistionId })
            {
                FileName = "Requistion.pdf"
            };
        }
        #endregion

        #region document
        // GET: Report
        public ActionResult Document(long documentId)
        {
            var document = _documentService.GetDocument(documentId);
            if (document != null)
            {
                var documentPdf = new Document()
                {
                    DocumentId = document.DocumentId,

                    Amount = document.Amount,

                    Description = document.Description,

                    DocumentNumber = document.DocumentNumber,

                    CreatedBy = document.CreatedBy,
                    UserId = document.UserId,
                    AmountInWords = document.AmountInWords,
                    Grades = document.Grades,
                    CreatedOn = document.CreatedOn,

                    BranchName = document.BranchName,

                    DocumentCategoryName = document.DocumentCategoryName,

                };
                ViewBag.documentPdf = documentPdf;
            }
           

            return View();
        }


        public ActionResult CashReceipt(long documentId)
        {
            var document = _documentService.GetDocument(documentId);
            if (document != null)
            {
                var documentPdf = new Document()
                {
                    DocumentId = document.DocumentId,

                    Amount = document.Amount,

                    Description = document.Description,

                    DocumentNumber = document.DocumentNumber,

                    CreatedBy = document.CreatedBy,
                    UserId = document.UserId,
                    Grades = document.Grades,
                    BranchId = document.BranchId,
                    CreatedOn = document.CreatedOn,

                    BranchName = document.BranchName,

                    DocumentCategoryName = document.DocumentCategoryName,

                };
                ViewBag.documentPdf = documentPdf;
            }


            return View();
        }

        public ActionResult ExportDocumentAsPDF(long documentId)
        {
            return new ActionAsPdf("Document", new { documentId = documentId })
            {
                FileName = "Document.pdf"
            };
        }

        public ActionResult ExportCashReceiptAsPDF(long documentId)
        {
            return new ActionAsPdf("CashReceipt", new { documentId = documentId })
            {
                FileName = "cashreceipt.pdf"
            };
        }
        #endregion
        #endregion
      
    }
}