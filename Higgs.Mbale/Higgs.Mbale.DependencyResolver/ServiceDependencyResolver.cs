
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.BAL.Concrete;
using Higgs.Mbale.DAL.Concrete;
using Higgs.Mbale.DAL.Interface;
using Ninject.Modules;

namespace Higgs.Mbale.DependencyResolver
{
 public   class ServiceDependencyResolver : NinjectModule
    {
       
     public override void Load()
     {
         //BAL
         Bind(typeof(IUserService)).To(typeof(UserService));
         Bind(typeof(IBranchService)).To(typeof(BranchService));
         Bind(typeof(ISectorService)).To(typeof(SectorService));
         Bind(typeof(ICasualWorkerService)).To(typeof(CasualWorkerService));
         Bind(typeof(IBatchService)).To(typeof(BatchService));
         Bind(typeof(IProductService)).To(typeof(ProductService));
         Bind(typeof(IDeliveryService)).To(typeof(DeliveryService));
         Bind(typeof(IOrderService)).To(typeof(OrderService));
         Bind(typeof(IStatusService)).To(typeof(StatusService));
         Bind(typeof(IMachineRepairService)).To(typeof(MachineRepairService));
         Bind(typeof(ITransactionSubTypeService)).To(typeof(TransactionSubTypeService));
         Bind(typeof(IRequistionService)).To(typeof(RequistionService));
         Bind(typeof(ITransactionService)).To(typeof(TransactionService));
         Bind(typeof(ISupplyService)).To(typeof(SupplyService));
         Bind(typeof(IInventoryService)).To(typeof(InventoryService));
         Bind(typeof(IStoreService)).To(typeof(StoreService));
         Bind(typeof(IGradeService)).To(typeof(GradeService));
         Bind(typeof(IAccountTransactionActivityService)).To(typeof(AccountTransactionActivityService));
         Bind(typeof(IActivityService)).To(typeof(ActivityService));
         Bind(typeof(IReportService)).To(typeof(ReportService));
         Bind(typeof(ICreditorService)).To(typeof(CreditorService));
         Bind(typeof(IDebtorService)).To(typeof(DebtorService));
         Bind(typeof(IFactoryExpenseService)).To(typeof(FactoryExpenseService));
         Bind(typeof(ILabourCostService)).To(typeof(LabourCostService));
         Bind(typeof(IStockService)).To(typeof(StockService));
         Bind(typeof(ICasualActivityService)).To(typeof(CasualActivityService));
         Bind(typeof(IOtherExpenseService)).To(typeof(OtherExpenseService));
         Bind(typeof(IBatchOutPutService)).To(typeof(BatchOutPutService));
         Bind(typeof(IBuveraService)).To(typeof(BuveraService));
         Bind(typeof(IUtilityService)).To(typeof(UtilityService));
         Bind(typeof(ICashService)).To(typeof(CashService));
            Bind(typeof(IInventoryPurchaseService)).To(typeof(InventoryPurchaseService));
         Bind(typeof(IDocumentService)).To(typeof(DocumentService));
         Bind(typeof(IFlourTransferService)).To(typeof(FlourTransferService));
         Bind(typeof(IBuveraTransferService)).To(typeof(BuveraTransferService));
         Bind(typeof(ICashTransferService)).To(typeof(CashTransferService));
         Bind(typeof(ICashSaleService)).To(typeof(CashSaleService));
         Bind(typeof(IMaizeBrandStoreService)).To(typeof(MaizeBrandStoreService));
         Bind(typeof(IUtilityAccountService)).To(typeof(UtilityAccountService));
         Bind(typeof(IBankService)).To(typeof(BankService));
         Bind(typeof(IBankTransactionService)).To(typeof(BankTransactionService));
         Bind(typeof(IMaizeOffloadingService)).To(typeof(MaizeOffloadingService));
         Bind(typeof(IMillingChargeService)).To(typeof(MillingChargeService));
         Bind(typeof(IBatchProjectionService)).To(typeof(BatchProjectionService));
         Bind(typeof(IDepositService)).To(typeof(DepositService));
        Bind(typeof(IAssetCategoryService)).To(typeof(AssetCategoryService));
        Bind(typeof(IAssetService)).To(typeof(AssetService));
            Bind(typeof(IWeightLossService)).To(typeof(WeightLossService));
            Bind(typeof(IFinancialAccountTransactionService)).To(typeof(FinancialAccountTransactionService));
            Bind(typeof(IFinancialAccountService)).To(typeof(FinancialAccountService));
            Bind(typeof(IWeightNoteRangeService)).To(typeof(WeightNoteRangeService));
            Bind(typeof(IWeightNoteNumberService)).To(typeof(WeightNoteNumberService));
            Bind(typeof(IOutSourcerOutPutService)).To(typeof(OutSourcerOutPutService));
            Bind(typeof(IDashBoardNotificationService)).To(typeof(DashBoardNotificationService));
            Bind(typeof(IRiceInputService)).To(typeof(RiceInputService));
            //DAL
            Bind(typeof(IUserDataService)).To(typeof(UserDataService));
         Bind(typeof(IBranchDataService)).To(typeof(BranchDataService));
         Bind(typeof(ISectorDataService)).To(typeof(SectorDataService));
         Bind(typeof(ICasualWorkerDataService)).To(typeof(CasualWorkerDataService));
         Bind(typeof(ISupplyDataService)).To(typeof(SupplyDataService));
         Bind(typeof(IBatchDataService)).To(typeof(BatchDataService));
         Bind(typeof(IProductDataService)).To(typeof(ProductDataService));
         Bind(typeof(IDeliveryDataService)).To(typeof(DeliveryDataService));
         Bind(typeof(IOrderDataService)).To(typeof(OrderDataService));
         Bind(typeof(IStatusDataService)).To(typeof(StatusDataService));
         Bind(typeof(IMachineRepairDataService)).To(typeof(MachineRepairDataService));
         Bind(typeof(ITransactionDataService)).To(typeof(TransactionDataService));
         Bind(typeof(ITransactionSubTypeDataService)).To(typeof(TransactionSubTypeDataService));
         Bind(typeof(IRequistionDataService)).To(typeof(RequistionDataService));
         Bind(typeof(IInventoryDataService)).To(typeof(InventoryDataService));
         Bind(typeof(IStoreDataService)).To(typeof(StoreDataService));
         Bind(typeof(IGradeDataService)).To(typeof(GradeDataService));
         Bind(typeof(IAccountTransactionActivityDataService)).To(typeof(AccountTransactionActivityDataService));
         Bind(typeof(IActivityDataService)).To(typeof(ActivityDataService));
         Bind(typeof(IReportDataService)).To(typeof(ReportDataService));
         Bind(typeof(ICreditorDataService)).To(typeof(CreditorDataService));
         Bind(typeof(IDebtorDataService)).To(typeof(DebtorDataService));
         Bind(typeof(IFactoryExpenseDataService)).To(typeof(FactoryExpenseDataService));
         Bind(typeof(ILabourCostDataService)).To(typeof(LabourCostDataService));
         Bind(typeof(IStockDataService)).To(typeof(StockDataService));
         Bind(typeof(ICasualActivityDataService)).To(typeof(CasualActivityDataService));
         Bind(typeof(IOtherExpenseDataService)).To(typeof(OtherExpenseDataService));
         Bind(typeof(IBatchOutPutDataService)).To(typeof(BatchOutPutDataService));
         Bind(typeof(IBuveraDataService)).To(typeof(BuveraDataService));
         Bind(typeof(IUtilityDataService)).To(typeof(UtilityDataService));
         Bind(typeof(ICashDataService)).To(typeof(CashDataService));
         Bind(typeof(IDocumentDataService)).To(typeof(DocumentDataService));
         Bind(typeof(IFlourTransferDataService)).To(typeof(FlourTransferDataService));
         Bind(typeof(IBuveraTransferDataService)).To(typeof(BuveraTransferDataService));
         Bind(typeof(ICashTransferDataService)).To(typeof(CashTransferDataService));
         Bind(typeof(ICashSaleDataService)).To(typeof(CashSaleDataService));
         Bind(typeof(IMaizeBrandStoreDataService)).To(typeof(MaizeBrandStoreDataService));
         Bind(typeof(IUtilityAccountDataService)).To(typeof(UtilityAccountDataService));
         Bind(typeof(IBankDataService)).To(typeof(BankDataService));
         Bind(typeof(IBankTransactionDataService)).To(typeof(BankTransactionDataService));
         Bind(typeof(IMaizeOffloadingDataService)).To(typeof(MaizeOffloadingDataService));
         Bind(typeof(IBatchProjectionDataService)).To(typeof(BatchProjectionDataService));
         Bind(typeof(IMillingChargeDataService)).To(typeof(MillingChargeDataService));
         Bind(typeof(IDepositDataService)).To(typeof(DepositDataService));
        Bind(typeof(IAssetCategoryDataService)).To(typeof(AssetCategoryDataService));
        Bind(typeof(IAssetDataService)).To(typeof(AssetDataService));
            Bind(typeof(InventoryPurchaseDataService)).To(typeof(InventoryPurchaseDataService));
            Bind(typeof(IFinancialAccountDataService)).To(typeof(FinancialAccountDataService));
            Bind(typeof(IFinancialAccountTransactionDataService)).To(typeof(FinancialAccountTransactionDataService));
            Bind(typeof(IWeightLossDataService)).To(typeof(WeightLossDataService));
            Bind(typeof(IWeightNoteRangeDataService)).To(typeof(WeightNoteRangeDataService));
            Bind(typeof(IWeightNoteNumberDataService)).To(typeof(WeightNoteNumberDataService));
            Bind(typeof(IOutSourcerOutPutDataService)).To(typeof(OutSourcerOutPutDataService));
            Bind(typeof(IDashBoardDataService)).To(typeof(DashBoardNotificationDataService));
            Bind(typeof(IRiceInputDataService)).To(typeof(RiceInputDataService)); 
        } 
        
          
    }
}
