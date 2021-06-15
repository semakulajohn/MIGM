using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;
using System.Configuration;
using Higgs.Mbale.Models.ViewModel;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Concrete
{
  public  class BatchService : IBatchService
    {
        private long supplyStatusIdComplete = Convert.ToInt64(ConfigurationManager.AppSettings["StatusIdComplete"]);
        private string supplyStatusIdInProgress = ConfigurationManager.AppSettings["SupplyStatusIdInProgress"];
      private long flourId = Convert.ToInt64(ConfigurationManager.AppSettings["FlourId"]);
     //// private double totalQuantity = 0;
         ILog logger = log4net.LogManager.GetLogger(typeof(BatchService));
        private IBatchDataService _dataService;
        private IUserService _userService;
        private ISupplyDataService _supplyDataService;
        private IFactoryExpenseService _factoryExpenseService;
        private ILabourCostService _labourCostService;
        private IOtherExpenseService _otherExpenseService;
        private IMachineRepairService _machineRepairService;
        private IBatchOutPutService _batchOutPutService;
        private ISupplyService _supplyService;
        private IUtilityService _utilityService;
        private IActivityService _activityService;
        private IStockService _stockService;
        private IBranchService _branchService;
       
        

        public BatchService(IBatchDataService dataService,IUserService userService,ISupplyDataService supplyDataService,
            IFactoryExpenseService factoryExpenseService,ILabourCostService labourCostService,
            IMachineRepairService machineRepairService,IOtherExpenseService otherExpenseService,
            IBatchOutPutService batchOutPutService,ISupplyService supplyService,IUtilityService utilityService,
            IActivityService activityService,IStockService stockService,IBranchService branchService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._supplyDataService = supplyDataService;
            this._factoryExpenseService = factoryExpenseService;
            this._labourCostService = labourCostService;
            this._machineRepairService = machineRepairService;
            this._otherExpenseService = otherExpenseService;
            this._batchOutPutService = batchOutPutService;
            this._supplyService = supplyService;
            this._utilityService = utilityService;
            this._activityService = activityService;
            this._stockService = stockService;
            this._branchService = branchService;
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchId"></param>
        /// <returns></returns>
        public Batch GetBatch(long batchId)
        {
            var result = this._dataService.GetBatch(batchId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Batch> GetAllBatches()
        {
            var results = this._dataService.GetAllBatches();
            return MapEFToModel(results);
        }

        public IEnumerable<BatchViewModel> GetAllBatchesForBrandDelivery(long branchId)
        {
            var results = this._dataService.GetAllBatchesForBrandDelivery(branchId);
            return MapEFBatchToBatchViewModel(results);
        }

        public IEnumerable<BatchViewModel> GetTenBatchesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetTenBatchesForAParticularBranch(branchId);
            return MapEFBatchToBatchViewModel(results);
        }
        public IEnumerable<BatchViewModel> GetAllBatchesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllBatchesForAParticularBranch(branchId);
            return MapEFBatchToBatchViewModel(results);
        }

        #region computations
     
        private double ComputeTotalProductionCost(double millingCharge, double labourCost, double buveraCost)
        {
            double totalProductonCost = 0;
            totalProductonCost = millingCharge + labourCost + buveraCost;
            return totalProductonCost;
        }

        private double ComputeMillingChargeBalance(double millingCharge, double factoryExpenses)
        {
            double millingChargeBalance = 0;
            millingChargeBalance = millingCharge - factoryExpenses;
            return millingChargeBalance;
        }
        #endregion
        private IEnumerable<FactoryExpense> GetAllFactoryExpensesForABatch(long batchId)
        {
            var results = _factoryExpenseService.GetAllFactoryExpensesForAParticularBatch(batchId);
            return results;
        }

        private IEnumerable<DeliveryBatch> GetAllDeliveriesForABatch(long batchId)
        {
            var results = GetAllBatchesForADelivery(batchId);
            return results;
        }

        public IEnumerable<FlourTransferBatch> GetAllBatchFlourTransfers(long batchId)
        {
            var results = this._dataService.GetBatchFlourTransfers(batchId);
            return MapEFToModel(results);
        }
        private IEnumerable<Utility> GetAllUtilitiesForABatch(long batchId)
        {
            var results = _utilityService.GetAllUtilitiesForAParticularBatch(batchId);
            return results;
        }

        private IEnumerable<OtherExpense> GetAllOtherExpensesForABatch(long batchId)
        {
            var results = _otherExpenseService.GetAllOtherExpensesForAParticularBatch(batchId);
            return results;
        }
        private IEnumerable<LabourCost> GetAllLabourCostsForABatch(long batchId)
        {
            var results = _labourCostService.GetAllLabourCostsForAParticularBatch(batchId);
            return results;
        }
        private IEnumerable<BatchOutPut> GetAllBatchOutPutsForABatch(long batchId)
        {
            var results = _batchOutPutService.GetAllBatchOutPutsForAParticularBatch(batchId);
            return results;
        }

        private IEnumerable<MachineRepair> GetAllMachineRepairsForABatch(long batchId)
        {
            var results = _machineRepairService.GetAllMachineRepairsForAParticularBatch(batchId);
            return results;
        }
        public long SaveBatch(Batch batch, string userId)
        {
            if(batch.BatchId != 0)
            {
                var updatedBatchId = UpdateBatchQuantityAndSupplies(batch, userId);
                return updatedBatchId;
            }
            else
            {

            
            var batchDTO = new DTO.BatchDTO()
            {
                BatchId = batch.BatchId,
                Name = batch.Name,
                SectorId = batch.SectorId,
                Quantity = batch.Quantity,
                BranchId = batch.BranchId,
                Deleted = batch.Deleted,
                CreatedBy = batch.CreatedBy,
                CreatedOn = batch.CreatedOn
            };

           var batchId = this._dataService.SaveBatch(batchDTO, userId);

          
           if (batch.Supplies.Any())
           {
              // EF.Models.Supply supplyObject = new EF.Models.Supply();
               
               foreach (var batchSupply in batch.Supplies) 
               {
                   var batchSupplyDTO = new BatchSupplyDTO()
                   {
                       BatchId = batchId,
                       SupplyId = batchSupply.SupplyId,
                       Quantity = batchSupply.Quantity,
                       NormalBags = batchSupply.NormalBags,
                       BagsOfStones = batchSupply.BagsOfStones,
                   };
                   this._dataService.PurgeBatchSupply(batchId,batchSupply.SupplyId);
                   this._dataService.SaveBatchSupply(batchSupplyDTO);
 
                    _supplyDataService.UpdateSupplyWithInProgressStatus(batchSupply.SupplyId,Convert.ToInt64(supplyStatusIdInProgress), userId);
                
               }

            
           }
           return batchId;
            }
        }

        public long UpdateBatchQuantityAndSupplies(Batch batch, string userId)
        {
            double newQuantity = 0;
            var batchToBeUpdated = GetBatch(batch.BatchId);
            newQuantity = batch.Quantity + batchToBeUpdated.Quantity;
            var batchDTO = new DTO.BatchDTO()
            {
                BatchId = batchToBeUpdated.BatchId,
                Name = batchToBeUpdated.Name,
                SectorId = batchToBeUpdated.SectorId,
                Quantity = newQuantity,
                BranchId = batchToBeUpdated.BranchId,
                Deleted = batchToBeUpdated.Deleted,
               // CreatedBy = batch.CreatedBy,
                CreatedOn = batch.CreatedOn
            };

            var batchId = this._dataService.SaveBatch(batchDTO, userId);


            if (batch.Supplies.Any())
            {
               // EF.Models.Supply supplyObject = new EF.Models.Supply();

                foreach (var batchSupply in batch.Supplies)
                {
                    var batchSupplyDTO = new BatchSupplyDTO()
                    {
                        BatchId = batchId,
                        SupplyId = batchSupply.SupplyId,
                        Quantity = batchSupply.Quantity,
                        NormalBags = batchSupply.NormalBags,
                        BagsOfStones = batchSupply.BagsOfStones,
                    };
                    this._dataService.PurgeBatchSupply(batchId, batchSupply.SupplyId);
                    this._dataService.SaveBatchSupply(batchSupplyDTO);

                    _supplyDataService.UpdateSupplyWithInProgressStatus(batchSupply.SupplyId, Convert.ToInt64(supplyStatusIdComplete), userId);

                }


            }
            return batchId;

        }

        private SupplyBatch GetSupplyDetailsInABatch(Batch batch)
        {
            double numberOfStoneBags = 0;
            double totalQuantities = 0;
             var supplyBatch = new SupplyBatch();
            var supplies = batch.Supplies;
            if (supplies.Any())
            {
                foreach (var supply in supplies)
                {
                    numberOfStoneBags = supply.BagsOfStones + numberOfStoneBags;
                    totalQuantities = supply.Quantity + totalQuantities;
                }
                   supplyBatch. NoOfStoneBags = numberOfStoneBags;
                   supplyBatch.TotalQuantity = totalQuantities;
                
                return supplyBatch;
            }
            return supplyBatch;
        }

        private bool CheckToSeeIfBatchLabourCostExists(long activityId,long batchId)
        {
            bool exists = false;
            var labourCost = _labourCostService.GetBatchLabourCost(activityId, batchId);
            if (labourCost != null)
            {
                exists = true;
            }
            return exists;
        }

      public IEnumerable<LabourCost> GenerateLabourCosts(long batchId,string userId){

          bool labourCostExists = false;
          Batch batch = GetBatch(batchId);
        
          var supplyBatch = GetSupplyDetailsInABatch(batch);
          List<LabourCost> labourCostList = new List<LabourCost>();
          var activities = _activityService.GetAllActivities();
          foreach (var activity in activities)
          {
              labourCostExists = CheckToSeeIfBatchLabourCostExists(activity.ActivityId, batchId);
              if (!labourCostExists)
              {
                  switch (activity.Name)
                  {
                      case "Stone Sorting":
                          LabourCost sortingLabourCost = new LabourCost()
                          {
                              LabourCostId = 0,
                              Quantity = supplyBatch.NoOfStoneBags,
                              Amount = (supplyBatch.NoOfStoneBags * activity.Charge),
                              Rate = activity.Charge,
                              BatchId = batch.BatchId,
                              ActivityId = activity.ActivityId,
                              SectorId = batch.SectorId,
                              BranchId = batch.BranchId,

                          };
                          labourCostList.Add(sortingLabourCost);
                          break;
                      case "Brand packaging":
                          LabourCost packagingLabourCost = new LabourCost()
                          {
                              LabourCostId = 0,
                              Quantity = batch.BrandOutPut,
                              Amount = (batch.BrandOutPut * (activity.Charge / 100)),
                              Rate = activity.Charge,
                              BatchId = batch.BatchId,
                              ActivityId = activity.ActivityId,
                              SectorId = batch.SectorId,
                              BranchId = batch.BranchId,

                          };
                          labourCostList.Add(packagingLabourCost);
                          break;
                      case "kase sorting":
                          LabourCost kaseLabourCost = new LabourCost()
                          {
                              LabourCostId = 0,
                              Quantity = supplyBatch.TotalQuantity,
                              Amount = (supplyBatch.TotalQuantity * activity.Charge),
                              Rate = activity.Charge,
                              BatchId = batch.BatchId,
                              ActivityId = activity.ActivityId,
                              SectorId = batch.SectorId,
                              BranchId = batch.BranchId,

                          };
                          labourCostList.Add(kaseLabourCost);
                          break;
                      case "Machine Operator Mill":
                          LabourCost machineLabourCost = new LabourCost()
                          {
                              LabourCostId = 0,
                              Quantity = batch.FlourOutPut,
                              Amount = batch.FlourOutPut * (activity.Charge / 50),
                              Rate = activity.Charge,
                              BatchId = batch.BatchId,
                              ActivityId = activity.ActivityId,
                              SectorId = batch.SectorId,
                              BranchId = batch.BranchId,

                          };
                          labourCostList.Add(machineLabourCost);
                          break;
                      default:
                          break;
                        }
              
              }
              
          }
          foreach (var labourCost in labourCostList)
          {
              _labourCostService.SaveLabourCost(labourCost,userId);
          }
          return labourCostList;
      }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long batchId, string userId)
        {
            _dataService.MarkAsDeleted(batchId, userId);
        }

        public IEnumerable<Batch> GetBatchesForAParticularBranchToTransfer(long branchId,long productId)
        {
            List<Batch> batches = new List<Batch>();
            var stocks = _stockService.GetStockForAParticularBranchForTransfer(branchId, productId);
            if (stocks.Any())
            {
                List<long> batchIds = new List<long>();
               
                foreach (var stock in stocks)
                {
                    long batchId = stock.BatchId;
                    if (!batchIds.Contains(batchId))
                    {
                        batchIds.Add(batchId);
                    }
                    
                }

                foreach (var batchid in batchIds)
                {
                    var batch = GetBatch(batchid);
                    if(batch != null)
                    {
                        batches.Add(batch);
                    }
                   
                }
            }
            return batches;
        }

        public void UpdateBatchGradeSizes(List<BatchGradeSize> batchGradeSizeList)
        {
            _batchOutPutService.UpdateBatchGradeSizes(batchGradeSizeList);
        }


        #region computations
        private double ComputeBatchLoss(double brandOutPut, double flourOutPut, double quantity)
        {
            double loss;
            loss = quantity - (brandOutPut + flourOutPut);
            return loss;
        }
        private double ComputePercentageBatchLoss(double loss,double quantity)
        {
            double percentageLoss;
            percentageLoss = 100 * (loss / quantity);
            return percentageLoss;

        }
        private double ComputePercentageBatchFlourOutPut(double flourOutput, double quantity)
        {
            double percentageFlour;

            percentageFlour = 100 * (flourOutput / quantity);
            return percentageFlour;

        }
        private double ComputePercentageBatchBrandOutPut(double brandOutPut, double quantity)
        {
            double percentageBrand;

            percentageBrand = 100 * (brandOutPut / quantity);
            return percentageBrand;

        }


        #endregion

        public IEnumerable<DeliveryBatch> GetAllBatchesForADelivery(long deliveryId)
        {
            var results = _dataService.GetAllBatchesForADelivery(deliveryId);
            return MapEFToModel(results);
        }

        public IEnumerable<CashSaleBatch> GetBatchCashSales(long batchId)
        {
            var results = _dataService.GetBatchCashSales(batchId);
            return MapEFToModel(results);
        }
        #region Mapping Methods

        public IEnumerable<Batch> MapEFToModel(IEnumerable<EF.Models.Batch> data)
        {
            var list = new List<Batch>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        /// <summary>
        /// Maps Batch EF object to Batch Model Object and
        /// returns the Batch model object.
        /// </summary>
        /// <param name="result">EF Batch object to be mapped.</param>
        /// <returns>Batch Model Object.</returns>
        public Batch MapEFToModel(EF.Models.Batch data)
        {
            double totalBuvera = 0, totalFlour = 0, totalBrand = 0,totalLoss=0, totalQuantity = 0; ;
            if (data != null)
            {
                var batch = new Batch()
                {
                    BatchId = data.BatchId,
                    Name = data.Name,
                    Quantity = data.Quantity,
                    BranchId = data.BranchId,

                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    SectorId = data.SectorId,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    BranchMillingChargeRate = data.Branch != null ? data.Branch.MillingChargeRate : 0,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),
                    BrandBalance = data.BrandBalance,
                };


             
                var otherExpenses = GetAllOtherExpensesForABatch(data.BatchId);
                double otherExpenseCost = 0;
                List<OtherExpense> otherExpenseList = new List<OtherExpense>();
                if (otherExpenses.Any())
                {
                    foreach (var other in otherExpenses)
                    {
                        var otherExpense = new OtherExpense()
                        {
                            Amount = other.Amount,
                            Description = other.Description,

                        };
                        otherExpenseList.Add(otherExpense);
                        otherExpenseCost = otherExpenseCost + other.Amount;
                    }
                    batch.TotalOtherExpenseCost = otherExpenseCost;
                    batch.OtherExpenses = otherExpenseList;
                }

                var utilities = GetAllUtilitiesForABatch(data.BatchId);
                double utilityCost = 0;
                List<Utility> utilityList = new List<Utility>();
                if (utilities.Any())
                {
                    foreach (var utility in utilities)
                    {
                        var utilityObject = new Utility()
                        {
                            Amount = utility.Amount,
                            Description = utility.Description,

                        };
                        utilityList.Add(utility);
                        utilityCost = utilityCost + utility.Amount;
                    }
                    batch.TotalUtilityCost = utilityCost;
                    batch.Utilities = utilityList;
                }

                var batchDeliveries = GetAllDeliveriesForABatch(data.BatchId);
                double totalFlourkgsDelivered = 0,totalFlourDeliveryAmount = 0,totalBrandDelivered=0,totalBrandDeliveryAmount = 0, 
                    totalFlourkgsTransfered = 0, totalFlourCashSaleAmount = 0, totalBrandCashSale = 0, totalBrandCashSaleAmount = 0, totalFlourCashSale = 0;
                GradeSizeTotalsViewModel flourDeliveryGradeSizeTotals = new GradeSizeTotalsViewModel();
                List<DeliveryBatch> deliveryBrandBatchList = new List<DeliveryBatch>();
                List<DeliveryBatch> deliveryFlourBatchList = new List<DeliveryBatch>();
                List<CashSaleBatch> cashSaleBrandBatchList = new List<CashSaleBatch>();
                List<CashSaleBatch> cashSaleFlourBatchList = new List<CashSaleBatch>();
                List<FlourTransferBatch> flourTransferBatchList = new List<FlourTransferBatch>();
                if (batchDeliveries.Any())
                {
                    
                    foreach (var batchDelivery in batchDeliveries)
                    {
                        if(batchDelivery.ProductId == 1)
                        {
                            var deliveryBatchObject = new DeliveryBatch()
                            {
                                Amount = batchDelivery.Amount,
                                BatchQuantity = batchDelivery.BatchQuantity,
                                Price = batchDelivery.Price,
                                

                            };
                            deliveryFlourBatchList.Add(batchDelivery);
                            totalFlourDeliveryAmount = Convert.ToDouble(totalFlourDeliveryAmount + batchDelivery.Amount);
                            totalFlourkgsDelivered = totalFlourkgsDelivered + batchDelivery.BatchQuantity;
                        }
                        else if(batchDelivery.ProductId == 2)
                        {
                            var deliveryBatchObject = new DeliveryBatch()
                            {
                                Amount = batchDelivery.Amount,
                                BatchQuantity = batchDelivery.BatchQuantity,
                                Price = batchDelivery.Price

                            };
                            deliveryBrandBatchList.Add(batchDelivery);
                            totalBrandDeliveryAmount = Convert.ToDouble(totalBrandDeliveryAmount + batchDelivery.Amount);
                            totalBrandDelivered = totalBrandDelivered + batchDelivery.BatchQuantity;
                        }
                       
                    }
                    batch.TotalBrandDeliveryAmount = totalBrandDeliveryAmount;
                    batch.TotalBrandDelivered = totalBrandDelivered;
                    batch.TotalFlourKgsDelivered = totalFlourkgsDelivered;
                    batch.TotalFlourDeliveryAmount = totalFlourDeliveryAmount;
                    batch.BatchBrandDeliveries = deliveryBrandBatchList;
                    batch.BatchFlourDeliveries = deliveryFlourBatchList;
                }

                var batchCashSales = GetBatchCashSales(data.BatchId);
                if (batchCashSales.Any())
                {

                    foreach (var batchCashSale in batchCashSales)
                    {
                        if (batchCashSale.ProductId == 1)
                        {
                            var cashSaleBatchObject = new CashSaleBatch()
                            {
                                Amount = batchCashSale.Amount,
                                BatchQuantity = batchCashSale.BatchQuantity,
                                
                            };
                            cashSaleFlourBatchList.Add(batchCashSale);
                            totalFlourCashSaleAmount = Convert.ToDouble(totalFlourCashSaleAmount + batchCashSale.Amount);
                            totalFlourCashSale = totalFlourCashSale + Convert.ToDouble(batchCashSale.BatchQuantity);
                        }
                        else if (batchCashSale.ProductId == 2)
                        {
                            var cashSaleBatchObject = new CashSaleBatch()
                            {
                                Amount = batchCashSale.Amount,
                                BatchQuantity = batchCashSale.BatchQuantity,
                                Price = batchCashSale.Price

                            };
                            cashSaleBrandBatchList.Add(batchCashSale);
                            totalBrandCashSaleAmount = Convert.ToDouble(totalBrandCashSaleAmount + batchCashSale.Amount);
                            totalBrandCashSale = totalBrandCashSale + Convert.ToDouble(batchCashSale.BatchQuantity);
                        }

                    }
                    batch.TotalBrandDeliveryAmount = totalBrandDeliveryAmount;
                    batch.TotalBrandDelivered = totalBrandDelivered;
                    batch.TotalFlourKgsDelivered = totalFlourkgsDelivered;
                    batch.TotalFlourDeliveryAmount = totalFlourDeliveryAmount;
                    batch.BatchBrandDeliveries = deliveryBrandBatchList;
                    batch.BatchFlourDeliveries = deliveryFlourBatchList;
                    batch.BatchBrandCashSales = cashSaleBrandBatchList;
                    batch.BatchFlourCashSales = cashSaleFlourBatchList;
                    batch.TotalCashSaleBrand = totalBrandCashSale;
                    batch.TotalCashSaleFlour = totalFlourCashSale;
                    batch.TotalCashSaleBrandAmount = totalBrandCashSaleAmount;
                    batch.TotalCashSaleFlourAmount = totalFlourCashSaleAmount;
                }

                var batchFlourTransfers = GetAllBatchFlourTransfers(data.BatchId);

                if (batchFlourTransfers.Any())
                {

                    foreach (var batchFlourTransfer in batchFlourTransfers)
                    {
                        flourTransferBatchList.Add(batchFlourTransfer);
                        totalFlourkgsTransfered = totalFlourkgsTransfered + batchFlourTransfer.BatchQuantity;

                    }

                    batch.TotalFlourKgsTransfered = totalFlourkgsTransfered;
                    batch.BatchFlourTransfers = flourTransferBatchList;

                }

                //var allBatchDeliveries = GetAllBatchDeliveries(data.BatchId);
                //flourDeliveryGradeSizeTotals = GetTotalGradeSizeDeliveryQuantities(allBatchDeliveries);
                var factoryExpenses = GetAllFactoryExpensesForABatch(data.BatchId);
                double totalFactoryExpense = 0;
                double factoryExpenseCost = 0;
                List<FactoryExpense> factoryExpenseList = new List<FactoryExpense>();
                if (factoryExpenses.Any())
                {
                    foreach (var item in factoryExpenses)
                    {
                        var factoryExpense = new FactoryExpense()
                        {
                            Amount = item.Amount,
                            Description = item.Description,

                        };
                        factoryExpenseList.Add(factoryExpense);
                        factoryExpenseCost = factoryExpenseCost + item.Amount;
                    }
                    batch.FactoryExpenseCost = factoryExpenseCost;
                    batch.FactoryExpenses = factoryExpenseList;
                }

                var machineRepairs = GetAllMachineRepairsForABatch(data.BatchId);
                double machineCosts = 0;
                List<MachineRepair> machineRepairList = new List<MachineRepair>();
                if (machineRepairs.Any())
                {
                    foreach (var repair in machineRepairs)
                    {
                        var machineRepair = new MachineRepair()
                        {
                            Amount = repair.Amount,
                            Description = repair.Description,
                        };
                        machineRepairList.Add(machineRepair);
                        machineCosts = machineRepair.Amount + machineCosts;
                    }
                    batch.MachineRepairs = machineRepairList;
                    batch.TotalMachineCost = machineCosts;
                }
                totalFactoryExpense = batch.TotalMachineCost + batch.FactoryExpenseCost;
                batch.TotalFactoryExpenseCost = totalFactoryExpense;
               
                var labourCosts = GetAllLabourCostsForABatch(data.BatchId);
                double totalLabourCosts = 0;
                List<LabourCost> labourCostList = new List<LabourCost>();
                //labourCostList.AddRange(AddBatchLabourCostsAutomatically(data));
                if (labourCosts.Any())
                {
                    foreach (var labour in labourCosts)
                    {
                        var labourCost = new LabourCost()
                        {
                            ActivityName = labour.ActivityName,
                            Amount = labour.Amount,
                            Quantity = labour.Quantity,
                            Rate = labour.Rate,
                        };
                        labourCostList.Add(labourCost);
                        totalLabourCosts = totalLabourCosts + labour.Amount;
                    }
                    batch.TotalLabourCosts = totalLabourCosts;
                    batch.LabourCosts = labourCostList;
                }

                if (data.BatchSupplies != null)
                {
                    if (data.BatchSupplies.Any())
                    {
                        double totalSupplyAmount = 0;
                        List<Supply> supplies = new List<Supply>();
                        var batchSupplies = data.BatchSupplies.AsQueryable().Where(m => m.BatchId == data.BatchId);
                        foreach (var batchSupply in batchSupplies)
                        {
                            var supply = new Supply()
                            {
                                SupplyId = batchSupply.Supply.SupplyId,
                                Quantity = batchSupply.Supply.Quantity,
                                SupplierId = batchSupply.Supply.SupplierId,
                                Price = batchSupply.Supply.Price,
                                WeightNoteNumber = batchSupply.Supply.WeightNoteNumber,
                                NormalBags = batchSupply.Supply.NormalBags,
                                BagsOfStones = batchSupply.Supply.BagsOfStones,
                                Amount = batchSupply.Supply.Amount,
                                SupplierName = _userService.GetUserFullName(batchSupply.Supply.AspNetUser2),
                            };
                            supplies.Add(supply);
                            totalSupplyAmount = totalSupplyAmount + supply.Amount;
                            totalQuantity = totalQuantity + supply.Quantity;
                        }
                        batch.Supplies = supplies;
                        batch.TotalSupplyAmount = totalSupplyAmount;
                    }


              }

                GradeSizeTotalsViewModel batchFlourGradesValues = new GradeSizeTotalsViewModel();


                List<BatchOutPut> batchOutPuts = GetAllBatchOutPutsForABatch(data.BatchId).ToList();
                List<BatchOutPut> batchOutPutList = new List<BatchOutPut>();
                if (batchOutPuts.Any())
                {
                    foreach (var outPut in batchOutPuts)
                    {
                        var batchOutPut = new BatchOutPut()
                        {
                            BatchOutPutId = outPut.BatchOutPutId,
                            Grades = outPut.Grades,
                            TotalBuveraCost = outPut.TotalBuveraCost,
                            TotalQuantity = outPut.TotalQuantity,
                            BrandOutPut = outPut.BrandOutPut,
                            FlourPercentage = outPut.FlourPercentage,
                            BrandPercentage = outPut.BrandPercentage,
                            FlourOutPut = outPut.FlourOutPut,
                            LossPercentage = outPut.LossPercentage,
                            Loss = outPut.Loss,

                        };
                        batchOutPutList.Add(batchOutPut);
                        totalBuvera = totalBuvera + batchOutPut.TotalBuveraCost;
                        batch.TotalQuantity = batchOutPut.TotalQuantity;
                        totalFlour = totalFlour + batchOutPut.FlourOutPut;
                        totalBrand = totalBrand + batchOutPut.BrandOutPut;
                        batch.Grades = batchOutPut.Grades;
                    }
                    batchFlourGradesValues = GetTotalGradeSizeBatchOutPutQuantities(batchOutPutList);
                    batch.TotalBuveraCost = totalBuvera;
                    batch.FlourOutPut = totalFlour;
                    batch.BrandOutPut = totalBrand;
                    totalLoss =_batchOutPutService.ComputeBatchLoss(totalBrand, totalFlour, totalQuantity);
                    batch.Loss = totalLoss;
                    batch.FlourPercentage =_batchOutPutService.ComputePercentageBatchFlourOutPut(totalFlour, totalQuantity);
                    batch.BrandPercentage =_batchOutPutService.ComputePercentageBatchBrandOutPut(totalBrand, totalQuantity);
                    batch.LossPercentage = _batchOutPutService.ComputePercentageBatchLoss(totalBrand, totalFlour, totalQuantity);
                    batch.BatchOutPuts = batchOutPutList;
                }
                var gradeList = new List<Grade>();
                var gradeIds = new List<long>();
                if (batchOutPuts.Any())
                {
                    foreach (var batchOutPut in batchOutPuts)
                    {

                        //var grades = batchOutPut.Grades;
                        var grades = batchOutPut.AvailableGrades;
                        foreach (var grade in grades)
                        {
                           
                              
                                if(gradeList.Any(g => g.Value == grade.Value))
                                {

                                    continue;
                                         
               
                                }
                                else
                                {
                                    gradeList.Add(grade);
                                }

                         

                        }
                       
                    }
                    gradeIds.Clear();
                }
                batch.AvailabeBatchGrades = gradeList;
                batch.MillingCharge = batch.BranchMillingChargeRate * batch.FlourOutPut;
                batch.BatchFlourGradesValues = batchFlourGradesValues;
                batch.MillingChargeBalance = ComputeMillingChargeBalance(batch.MillingCharge, batch.TotalFactoryExpenseCost);

                batch.TotalProductionCost =ComputeTotalProductionCost(batch.MillingCharge, batch.TotalLabourCosts, batch.TotalBuveraCost);
         
                return batch;

            }
            return null;
           
        }

        public IEnumerable<BatchViewModel> MapEFBatchToBatchViewModel(IEnumerable<EF.Models.Batch> data)
        {
            var list = new List<BatchViewModel>();
            foreach (var result in data)
            {
                list.Add(MapEFBatchToBatchViewModel(result));
            }
            return list;
        }
        public BatchViewModel MapEFBatchToBatchViewModel(EF.Models.Batch data)
        {
            
            if (data != null)
            {
                var batch = new BatchViewModel()
                {
                    BatchId = data.BatchId,
                    Name = data.Name,
                    Quantity = data.Quantity,
                     BrandBalance = data.BrandBalance,          
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    
                   
                    
                };

             
                return batch;

            }
            return null;

        }
        public void UpdateBatchBrandBalance(long batchId, double quantity, string userId)
        {

            _dataService.UpdateBatchBrandBalance(batchId, quantity, userId);
        }

        public DeliveryBatch MapEFToModel(EF.Models.DeliveryBatch data)
        {
            string customerName = string.Empty;
            if(data.Delivery != null)
            {
                var customer = _userService.GetAspNetUser(data.Delivery.CustomerId);
                if(customer != null)
                {
                    customerName = customer.FirstName + " " + customer.LastName;
                }
            }
            var deliveryBatch = new DeliveryBatch()
            {

                BatchId = data.BatchId,
                DeliveryId = data.DeliveryId,
                CreatedOn = data.CreatedOn,
                CustomerName = customerName,
                BatchNumber = data.Batch != null ? data.Batch.Name : "",
                TimeStamp = data.TimeStamp,
                ProductId = data.ProductId,
                ProductName = data.Product != null ? data.Product.Name : "",
                Price = data.Price,
                BatchQuantity = data.BatchQuantity,
                Amount = data.Amount,

            };
            return deliveryBatch;

        }

        public IEnumerable<DeliveryBatch> MapEFToModel(IEnumerable<EF.Models.DeliveryBatch> data)
        {
            var list = new List<DeliveryBatch>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public FlourTransferBatch MapEFToModel(EF.Models.FlourTransferBatch data)
        {
            string branchName = string.Empty;
            if (data.FlourTransfer != null)
            {
                var branch = _branchService.GetBranch(data.FlourTransfer.BranchId);
                if (branch != null)
                {
                    branchName = branch.Name;
                }
            }

            var flourTransferBatch = new FlourTransferBatch()
            {

                BatchId = data.BatchId,
                FlourTransferId = data.FlourTransferId,
                CreatedOn = data.CreatedOn,
                BranchName = branchName,
                BatchNumber = data.Batch != null ? data.Batch.Name : "",
                TimeStamp = data.TimeStamp,

                BatchQuantity = data.BatchQuantity,

            };
            return flourTransferBatch;

        }

        public IEnumerable<FlourTransferBatch> MapEFToModel(IEnumerable<EF.Models.FlourTransferBatch> data)
        {
            var list = new List<FlourTransferBatch>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        public CashSaleBatch MapEFToModel(EF.Models.CashSaleBatch data)
        {
           
            var cashSaleBatch = new CashSaleBatch()
            {

                BatchId = data.BatchId,
                CashSaleId = data.CashSaleId,
                CreatedOn = data.CreatedOn,
                
                BatchNumber = data.Batch != null ? data.Batch.Name : "",
                TimeStamp = data.TimeStamp,
                ProductId = data.ProductId,
                ProductName = data.Product != null ? data.Product.Name : "",
                Price = data.Price,
                BatchQuantity = data.BatchQuantity,
                Amount = data.Amount,

            };
            return cashSaleBatch;

        }

        public IEnumerable<CashSaleBatch> MapEFToModel(IEnumerable<EF.Models.CashSaleBatch> data)
        {
            var list = new List<CashSaleBatch>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public GradeSizeTotalsViewModel GetTotalGradeSizeBatchOutPutQuantities(List<BatchOutPut> batchOutPutList)
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
                        batchOutPutList.RemoveAll(r => r.BatchOutPutId == item.BatchOutPutId);

                    }

                    else
                    {

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
    }
}
