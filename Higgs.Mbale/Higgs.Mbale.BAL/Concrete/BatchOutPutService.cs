using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;
using System.Configuration;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class BatchOutPutService : IBatchOutPutService
    {
     private long supplyStatusIdComplete = Convert.ToInt64(ConfigurationManager.AppSettings["StatusIdComplete"]);
     private long flourId = Convert.ToInt64(ConfigurationManager.AppSettings["FlourId"]);
     private long brandId = Convert.ToInt64(ConfigurationManager.AppSettings["BrandId"]);
        private long buveraCategoryId = Convert.ToInt64(ConfigurationManager.AppSettings["UsedBuveraCategoryId"]);
        ILog logger = log4net.LogManager.GetLogger(typeof(BatchOutPutService));
        private IBatchOutPutDataService _dataService;
        private IUserService _userService;
      private ISupplyDataService _supplyDataService;
      private IStockService _stockService;
      private IBuveraService _buveraService;
      private IMaizeBrandStoreService _maizeBrandStoreService;
                

        public BatchOutPutService(IBatchOutPutDataService dataService,IUserService userService,ISupplyDataService supplyDataService,
            IStockService stockService,IBuveraService buveraService,IMaizeBrandStoreService maizeBrandStoreService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._supplyDataService = supplyDataService;
            this._stockService = stockService;
            this._buveraService = buveraService;
            this._maizeBrandStoreService = maizeBrandStoreService;
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchOutPutId"></param>
        /// <returns></returns>
        public BatchOutPut GetBatchOutPut(long batchOutPutId)
        {
            var result = this._dataService.GetBatchOutPut(batchOutPutId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BatchOutPut> GetAllBatchOutPuts()
        {
            var results = this._dataService.GetAllBatchOutPuts();
            return MapEFToModel(results);
        }


        public IEnumerable<BatchOutPut> GetAllBatchOutPutsForAParticularBatch(long batchId)
        {
            var results = this._dataService.GetAllBatchOutPutsForAParticularBatch(batchId);
            return MapEFToModel(results);
        }

        #region computations
        public double ComputeBatchLoss(double brandOutPut, double flourOutPut, double quantity)
        {
            double loss;
            loss = quantity - (brandOutPut + flourOutPut);
            return loss;
        }
        public double ComputePercentageBatchLoss(double brandOutPut, double flourOutPut, double quantity)
        {
            double percentageLoss;
            var loss = ComputeBatchLoss(brandOutPut, flourOutPut, quantity);
            percentageLoss = 100 * (loss / quantity);
            return percentageLoss;

        }
        public double ComputePercentageBatchFlourOutPut(double flourOutput, double quantity)
        {
            double percentageFlour;

            percentageFlour = 100 * (flourOutput / quantity);
            return percentageFlour;

        }
        public double ComputePercentageBatchBrandOutPut(double brandOutPut, double quantity)
        {
            double percentageBrand;

            percentageBrand = 100 * (brandOutPut / quantity);
            return percentageBrand;

        }

        public double ComputeTotalProductionCost(double millingCharge, double labourCost, double buveraCost)
        {
            double totalProductonCost = 0;
            totalProductonCost = millingCharge + labourCost + buveraCost;
            return totalProductonCost;
        }

       
        #endregion

        private double BatchQuantity(long batchId)
        {
            double totalQuantity = 0;
            var batchSupplies = _dataService.GetBatchSupplies(batchId);
            foreach (var batchSupply in batchSupplies)
            {
                totalQuantity = batchSupply.Quantity + totalQuantity;
            }
            return totalQuantity;
        }

        public long CheckForBuveraInStore(List<Grade> grades,long storeId)
        {
            
            foreach (var grade in grades)
            {
                //var result = _buveraService.GetStoreBuveraGradeSize(grade.GradeId, storeId);
                if (grade.Denominations != null)
                {
                    foreach (var denomination in grade.Denominations)
                    {
                        var result = _buveraService.GetStoreBuveraGradeSize(grade.GradeId, denomination.DenominationId, storeId);
                        if (result == null)
                        {
                            return -2;
                        }
                        else
                        {
                            if (result.Quantity < denomination.Quantity)
                            {
                                return -1;
                            }
                        }
                    }
                }
               
                
            }
            return 1; 
        }
        public long SaveBatchOutPut(BatchOutPut batchOutPut, string userId)
        {
            long batchOutPutId = 0;
            
            List<StockGradeSize> stockGradeSizeList = new List<StockGradeSize>();

            if (batchOutPut.Grades != null)
            {
              var gradeStore =  CheckForBuveraInStore(batchOutPut.Grades,batchOutPut.StoreId);
              if (gradeStore == -2)
              {
                  batchOutPutId = gradeStore;
                  return batchOutPutId;
              }
              else if (gradeStore == -1)
              {
                  batchOutPutId = gradeStore;
                  return batchOutPutId;
              }
              else
              {

                double batchQuantity = BatchQuantity(batchOutPut.BatchId);
                var batchOutPutDTO = new DTO.BatchOutPutDTO()
                {
                    BatchId = batchOutPut.BatchId,
                    BrandOutPut = batchOutPut.BrandOutPut,
                    FlourOutPut = batchOutPut.FlourOutPut,
                    BranchId = batchOutPut.BranchId,
                    SectorId = batchOutPut.SectorId,
                    Loss = ComputeBatchLoss(batchOutPut.BrandOutPut, batchOutPut.FlourOutPut, batchQuantity),
                    FlourPercentage = ComputePercentageBatchFlourOutPut(batchOutPut.FlourOutPut, batchQuantity),
                    BrandPercentage = ComputePercentageBatchBrandOutPut(batchOutPut.BrandOutPut, batchQuantity),
                    LossPercentage = ComputePercentageBatchLoss(batchOutPut.BrandOutPut, batchOutPut.FlourOutPut, batchQuantity),
                    Deleted = batchOutPut.Deleted,
                    CreatedBy = batchOutPut.CreatedBy,
                    CreatedOn = batchOutPut.CreatedOn
                };

                batchOutPutId = this._dataService.SaveBatchOutPut(batchOutPutDTO, userId);

                if (batchOutPutId == 0)
                {
                    batchOutPutId = -3;
                    return batchOutPutId;
                }
                else
                {
                    double totalBuveraQuantity = 0;
                    if (batchOutPut.Grades.Any())
                    {
                        List<BatchGradeSize> batchGradeSizeList = new List<BatchGradeSize>();

                        foreach (var grade in batchOutPut.Grades)
                        {
                            var gradeId = grade.GradeId;
                            if (grade.Denominations != null)
                            {
                                if (grade.Denominations.Any())
                                {
                                    foreach (var denomination in grade.Denominations)
                                    {
                                        var batchGradeSize = new BatchGradeSize()
                                        {
                                            GradeId = gradeId,
                                            SizeId = denomination.DenominationId,
                                            BatchOutPutId = batchOutPutId,
                                            Quantity = denomination.Quantity,
                                            Balance = denomination.Quantity,
                                            TimeStamp = DateTime.Now
                                        };
                                        batchGradeSizeList.Add(batchGradeSize);

                                        var stockGradeSize = new StockGradeSize()
                                        {
                                            GradeId = gradeId,
                                            SizeId = denomination.DenominationId,
                                            Quantity = denomination.Quantity,
                                            TimeStamp = DateTime.Now
                                        };
                                        stockGradeSizeList.Add(stockGradeSize);



                                        var inOrOut = false;
                                        //Method that updates buvera into storeBuveraGradeSize table(storeBuvera stock)
                                        var storeBuveraGradeSize = new StoreBuveraGradeSize()
                                        {
                                            StoreId = batchOutPut.StoreId,
                                            GradeId = batchGradeSize.GradeId,
                                            SizeId = batchGradeSize.SizeId,
                                            Quantity = batchGradeSize.Quantity,
                                        };

                                        this._buveraService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, inOrOut);
                                        totalBuveraQuantity = denomination.Quantity + totalBuveraQuantity;
                                    }
                                }
                            }
                        }

                        var buvera = new Buvera()
                        {

                            TotalQuantity = totalBuveraQuantity,
                            BranchId = batchOutPut.BranchId,
                            FromSupplier = Convert.ToString(batchOutPut.StoreId),
                            ToReceiver = Convert.ToString(batchOutPut.BatchId),
                            StoreId = batchOutPut.StoreId,
                            BuveraCategoryId = buveraCategoryId,

                        };

                        var buveraId = this._buveraService.SaveBuveraOnBatchUsage(buvera, userId);
                        this._dataService.PurgeBatchGradeSize(batchOutPutId);
                        this.SaveBatchGradeSizeList(batchGradeSizeList);

                    }

                    UpdateBatchSupply(batchOutPut.BatchId, supplyStatusIdComplete, userId);


                    var maizeBrandStore = new MaizeBrandStore()
                    {

                        BranchId = batchOutPut.BranchId,
                        BatchId = batchOutPut.BatchId,
                        Action = "+",
                        StoreId = batchOutPut.StoreId,
                        Quantity = batchOutPut.BrandOutPut,


                    };
                    var maizeBrandstoreId = _maizeBrandStoreService.SaveMaizeBrandStore(maizeBrandStore, userId);

                    if (batchOutPut.BrandOutPut != 0 && batchOutPut.FlourOutPut == 0)
                    {
                        var stock = new Stock()
                        {
                            SectorId = batchOutPut.SectorId,
                            BranchId = batchOutPut.BranchId,
                            BatchId = batchOutPut.BatchId,
                            InOrOut = true,
                            StoreId = batchOutPut.StoreId,
                            ProductId = brandId,
                            ProductOutPut = batchOutPut.BrandOutPut,

                        };
                        UpdateBatchBrandBalance(batchOutPut.BatchId, batchOutPut.BrandOutPut, userId);
                         _stockService.SaveStock(stock, userId);
                    }
                    else if (batchOutPut.FlourOutPut != 0 && batchOutPut.BrandOutPut == 0)
                    {

                        var flourStock = new Stock()
                        {
                            SectorId = batchOutPut.SectorId,
                            BranchId = batchOutPut.BranchId,
                            BatchId = batchOutPut.BatchId,
                            InOrOut = true,
                            StoreId = batchOutPut.StoreId,
                            ProductId = flourId,
                            ProductOutPut = batchOutPut.FlourOutPut,
                            StockGradeSize = stockGradeSizeList

                        };
                        _stockService.SaveStock(flourStock, userId);
                    }
                    else if (batchOutPut.FlourOutPut != 0 && batchOutPut.BrandOutPut != 0)
                    {
                        var stock = new Stock()
                        {
                            SectorId = batchOutPut.SectorId,
                            BranchId = batchOutPut.BranchId,
                            BatchId = batchOutPut.BatchId,
                            InOrOut = true,
                            StoreId = batchOutPut.StoreId,
                            ProductId = brandId,
                            ProductOutPut = batchOutPut.BrandOutPut,

                        };
                        UpdateBatchBrandBalance(batchOutPut.BatchId, batchOutPut.BrandOutPut, userId);
                        _stockService.SaveStock(stock, userId);            

                        var flourStock = new Stock()
                        {
                            SectorId = batchOutPut.SectorId,
                            BranchId = batchOutPut.BranchId,
                            BatchId = batchOutPut.BatchId,
                            InOrOut = true,
                            StoreId = batchOutPut.StoreId,
                            ProductId = flourId,
                            ProductOutPut = batchOutPut.FlourOutPut,
                            StockGradeSize = stockGradeSizeList

                        };
                        _stockService.SaveStock(flourStock, userId);
                    }
                        //}
                    
                    return batchOutPutId;
                }
            }
        }
            else
            {
                return batchOutPutId;
            }
                      
        }

        private   void UpdateBatchSupply(long batchId,long statusId,string userId)
        {
            var batchSupplies = _dataService.GetBatchSupplies(batchId);
            if (batchSupplies.Any())
            {
               
                foreach (var batchSupply in batchSupplies)
                {
                   
                    _supplyDataService.UpdateBatchSupplyWithCompletedStatus(batchSupply.SupplyId,supplyStatusIdComplete, userId);
                }
            }
               
        }
        void SaveBatchGradeSizeList(List<BatchGradeSize> batchGradeSizeList)
        {
            if (batchGradeSizeList != null)
            {
                if (batchGradeSizeList.Any())
                {
                    foreach (var batchGradeSize in batchGradeSizeList)
                    {
                        var batchGradeSizeDTO = new DTO.BatchGradeSizeDTO()
                        {
                            BatchOutPutId = batchGradeSize.BatchOutPutId,
                            GradeId = batchGradeSize.GradeId,
                            Quantity = batchGradeSize.Quantity,
                            SizeId = batchGradeSize.SizeId,
                            Balance = batchGradeSize.Balance,
                            TimeStamp = batchGradeSize.TimeStamp
                        };
                        this.SaveBatchGradeSize(batchGradeSizeDTO);
                    }
                }
            }
        }

        void SaveBatchGradeSize(BatchGradeSizeDTO batchGradeSizeDTO)
        {
            _dataService.SaveBatchGradeSize(batchGradeSizeDTO);
        }

        //public IEnumerable<BatchOutPut> GetCombinedBatchOutPuts()
        //{
        //    List<BatchOutPut> newBatchOutPutList = new List<BatchOutPut>();
        //    var batchOutPuts = GetAllBatchOutPuts();
        //    foreach (var batchOutPut in batchOutPuts)
        //    {
                
        //    }

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchOutPutId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long batchOutPutId, string userId)
        {
            _dataService.MarkAsDeleted(batchOutPutId, userId);
        }

        public void UpdateBatchBrandBalance(long batchId, double quantity, string userId)
        {
            
            _dataService.UpdateBatchBrandBalance(batchId, quantity, userId);
        }

        public void UpdateBatchGradeSizes(List<BatchGradeSize> batchGradeSizeList)
        {
            if (batchGradeSizeList != null)
            {
                if (batchGradeSizeList.Any())
                {
                    foreach (var batchGradeSize in batchGradeSizeList)
                    {
                        var batchGradeSizeDTO = new DTO.BatchGradeSizeDTO()
                        {
                            BatchOutPutId = batchGradeSize.BatchOutPutId,
                            GradeId = batchGradeSize.GradeId,
                            Quantity = batchGradeSize.Quantity,
                            SizeId = batchGradeSize.SizeId,
                            Balance = batchGradeSize.Balance,
                            TimeStamp = batchGradeSize.TimeStamp
                        };
                        this.UpdateBatchGradeSize(batchGradeSizeDTO);
                    }
                }
            }
        }

        public void UpdateBatchGradeSize(BatchGradeSizeDTO batchGradeSizeDTO)
        {
            _dataService.UpdateBatchGradeSizes(batchGradeSizeDTO);
        }

        #region Mapping Methods

        public IEnumerable<BatchOutPut> MapEFToModel(IEnumerable<EF.Models.BatchOutPut> data)
        {
            var list = new List<BatchOutPut>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps BatchOutPut EF object to Batch Model Object and
        /// returns the BatchOutPut model object.
        /// </summary>
        /// <param name="result">EF BatchOutPut object to be mapped.</param>
        /// <returns>BatchOutPut Model Object.</returns>
        public BatchOutPut MapEFToModel(EF.Models.BatchOutPut data)
        {
            if (data != null)
            {
                var batchOutPut = new BatchOutPut()
                {
                    BatchOutPutId = data.BatchOutPutId,
                    BatchId = data.BatchId,
                    FlourOutPut = data.FlourOutPut,
                    BrandOutPut = data.BrandOutPut,
                    BrandPercentage = data.BrandPercentage,
                    FlourPercentage = data.FlourPercentage,
                    Loss = data.Loss,
                   
                    LossPercentage = data.LossPercentage,
                    BranchId = data.BranchId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorId = data.SectorId,
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),
                    BatchName = data.Batch != null ? data.Batch.Name : "",
                };

                List<Grade> grades = new List<Grade>();
                List<Grade> availableGrades = new List<Grade>();
                if (data.BatchGradeSizes != null)
                {
                    if (data.BatchGradeSizes.Any())
                    {
                        
                       // var distinctGrades = data.BatchGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        var distinctGrades = data.BatchGradeSizes.Where(a => a.BatchOutPutId == data.BatchOutPutId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        foreach (var batchGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = batchGradeSize.Grade.GradeId,
                                Value = batchGradeSize.Grade.Value,
                                CreatedOn = batchGradeSize.Grade.CreatedOn,
                                TimeStamp = batchGradeSize.Grade.TimeStamp,
                                Deleted = batchGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(batchGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(batchGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (batchGradeSize.Grade.BatchGradeSizes != null)
                            {
                                if (batchGradeSize.Grade.BatchGradeSizes.Any())
                                {
                                    
                                    
                                    var distinctSizes = batchGradeSize.Grade.BatchGradeSizes.Where(a => a.BatchOutPutId == data.BatchOutPutId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                   

                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity,
                                            Rate = ogs.Size.Rate,
                                            Amount = ogs.Quantity * (Convert.ToDouble(ogs.Size.Rate)),
                                            Balance = ogs.Balance,
                                        };
                                        batchOutPut.TotalQuantity += (ogs.Quantity * ogs.Size.Value);
                                       
                                        batchOutPut.TotalBuveraCost += denomination.Amount;
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                            availableGrades.Add(grade);  
                        }
                       
                    }

                }
                batchOutPut.Grades = grades;

                batchOutPut.AvailableGrades = availableGrades;

                return batchOutPut;
            }
            return null;
           
        }



       #endregion
    }
}
