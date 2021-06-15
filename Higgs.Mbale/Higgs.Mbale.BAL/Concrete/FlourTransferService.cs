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

namespace Higgs.Mbale.BAL.Concrete
{
 public   class FlourTransferService : IFlourTransferService
    {
      ILog logger = log4net.LogManager.GetLogger(typeof(FlourTransferService));
      private long flourId = Convert.ToInt64(ConfigurationManager.AppSettings["FlourId"]);
      private double balance = 0;
        private IFlourTransferDataService _dataService;
        private IUserService _userService;
        private IGradeService _gradeService;
        private IStoreService _storeService;
        private IBatchService _batchService;
        private IStockService _stockService;


        public FlourTransferService(IFlourTransferDataService dataService, IUserService userService, IStockService stockService, IGradeService gradeService, IStoreService storeService,IBatchService batchService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._gradeService = gradeService;
            this._storeService = storeService;
            this._batchService = batchService;
            this._stockService = stockService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FlourTransferId"></param>
        /// <returns></returns>
        public FlourTransfer GetFlourTransfer(long FlourTransferId)
        {
            var result = this._dataService.GetFlourTransfer(FlourTransferId);
            return MapEFToModel(result);
        }


        private double GetRateOfAParticularSize(long sizeId)
        {
            double rate = 0;
            var size = this._gradeService.GetSize(sizeId);
            if (size != null)
            {
                rate = Convert.ToDouble(size.Rate);
                //return rate;
            }
            return rate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FlourTransfer> GetAllFlourTransfers()
        {
            var results = this._dataService.GetAllFlourTransfers();
            return MapEFToModel(results);
        }

        public IEnumerable<FlourTransfer> GetAllFlourTransfersForAParticularStore(long storeId)
        {
            var results = this._dataService.GetAllFlourTransfersForAParticularStore(storeId);
            return MapEFToModel(results);
        }

        public long RejectFlour(FlourTransfer flourTransfer, string userId)
        {
            bool inOrOut = false;

            var flourTransferDTO = new DTO.FlourTransferDTO()
            {
                FlourTransferId = flourTransfer.FlourTransferId,
                TotalQuantity = flourTransfer.TotalQuantity,
                BranchId = flourTransfer.BranchId,
                FromSupplierStoreId = flourTransfer.FromSupplierStoreId,
                ToReceiverStoreId = flourTransfer.ToReceiverStoreId,
                Accept = flourTransfer.Accept,
                Reject = true,
               
                StoreId = flourTransfer.StoreId,
                Deleted = flourTransfer.Deleted,
                CreatedBy = flourTransfer.CreatedBy,
                CreatedOn = flourTransfer.CreatedOn
            };

            var flourTransferId = this._dataService.SaveFlourTransfer(flourTransferDTO, userId);
            if (flourTransfer.Grades != null)
            {
                if (flourTransfer.Grades.Any())
                {
                    List<FlourTransferGradeSize> flourTransferGradeSizeList = new List<FlourTransferGradeSize>();

                    foreach (var grade in flourTransfer.Grades)
                    {
                        var gradeId = grade.GradeId;
                        if (grade.Denominations != null)
                        {
                            if (grade.Denominations.Any())
                            {
                                foreach (var denomination in grade.Denominations)
                                {
                                    inOrOut = true;
                                    //Method that adds FlourTransfer into receiver storeFlourTransferGradeSize table(storeFlourTransfer stock)
                                    var storeFlourTransferGradeSize = new StoreFlourTransferGradeSizeDTO()
                                    {
                                        StoreId = flourTransfer.FromSupplierStoreId,
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        Quantity = denomination.Quantity,
                                    };

                                    //storeFlourTransfer = this._dataService.SaveStoreGradeSize(storeFlourTransferGradeSize, inOrOut);
                                    this._dataService.SaveStoreFlourTransferGradeSize(storeFlourTransferGradeSize, inOrOut);


                                }
                            }
                        }
                    }

                }
            }
            _dataService.PurgeBatchTransferGradeSize(flourTransferDTO.FlourTransferId);
            _dataService.PurgeTransferBatch(flourTransferDTO.FlourTransferId);
            return flourTransferId;

        }


        public long AcceptFlour(FlourTransfer flourTransfer, string userId)
        {
            bool inOrOut = false;
            var soldOut = false;
           
          
            var flourTransferDTO = new DTO.FlourTransferDTO()
            {
                FlourTransferId = flourTransfer.FlourTransferId,
                TotalQuantity = flourTransfer.TotalQuantity,
                BranchId = flourTransfer.BranchId,
                FromSupplierStoreId = flourTransfer.FromSupplierStoreId,
                ToReceiverStoreId = flourTransfer.ToReceiverStoreId,
                Accept = true,
                Reject = flourTransfer.Reject,
               
                StoreId = flourTransfer.StoreId,
                Deleted = flourTransfer.Deleted,
                CreatedBy = flourTransfer.CreatedBy,
                CreatedOn = flourTransfer.CreatedOn
            };

            var flourTransferId = this._dataService.SaveFlourTransfer(flourTransferDTO, userId);

            if (flourTransfer.Grades != null)
            {
                if (flourTransfer.Grades.Any())
                {
                    List<FlourTransferGradeSize> flourTransferGradeSizeList = new List<FlourTransferGradeSize>();

                    foreach (var grade in flourTransfer.Grades)
                    {
                        var gradeId = grade.GradeId;
                        if (grade.Denominations != null)
                        {
                            if (grade.Denominations.Any())
                            {
                                foreach (var denomination in grade.Denominations)
                                {
                                    inOrOut = true;
                                    //Method that adds FlourTransfer into receiver storeFlourTransferGradeSize table
                                    var storeFlourTransferGradeSize = new StoreFlourTransferGradeSizeDTO()
                                    {
                                        StoreId = flourTransfer.ToReceiverStoreId,
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        Quantity = denomination.Quantity,
                                    };

                                   
                                    this._dataService.SaveStoreFlourTransferGradeSize(storeFlourTransferGradeSize, inOrOut);


                                }
                            }
                        }
                    }

                }
            }
            List<Batch> batchesList =new  List<Batch>();
            foreach (var flourTransferBatch in flourTransfer.FlourTransferBatches)
	            {
                    var batch = _batchService.GetBatch(flourTransferBatch.BatchId);
                    batchesList.Add(batch);
	            }
            
            List<Batch> SortedBatchList = batchesList.OrderBy(o => o.CreatedOn).ToList();
            foreach (var batch in SortedBatchList)
            {
                soldOut = ReduceBatchStock(batch.BatchId, flourId, flourTransfer.FromSupplierStoreId, flourTransfer.TotalQuantity,userId);
                if (soldOut && balance == 0)
                {
                    return flourTransferId;
                }
                else if(!soldOut && balance == 0 )
                {
                    return flourTransferId;
                }
                else
                {
                    if (balance > 0)
                    {
                    soldOut = ReduceBatchStock(batch.BatchId, flourId, flourTransfer.FromSupplierStoreId,balance, userId);
                    }
                }
            }
           
            return flourTransferId;

        }

        private bool ReduceBatchStock(long batchId, long productId, long storeId, double totalQuantity,string userId)
        {
            var soldOut = false;
            
            var stockToTransfer = _stockService.GetStockForAParticularBatchAndProduct(batchId, productId, storeId);
            var storeStock = _stockService.GetStoreStockForParticularStock(stockToTransfer.StockId, productId, storeId);
            if (storeStock != null)
            {
                if (storeStock.Balance == totalQuantity)
                {
                    soldOut = true;
                    var storeStockUpdate = new StoreStock()
                    {
                        StoreStockId = storeStock.StoreStockId,
                        StoreId = storeStock.StoreId,
                        StartStock = storeStock.StartStock,
                        StockId = storeStock.StockId,
                        ProductId = storeStock.ProductId,
                        StockBalance = storeStock.StockBalance,
                        BranchId = storeStock.BranchId,
                        Quantity = storeStock.Quantity,
                        SectorId = storeStock.SectorId,
                        TimeStamp = storeStock.TimeStamp,
                        InOrOut = storeStock.InOrOut,
                        Balance = 0,
                        CreatedOn = storeStock.CreatedOn,
                        SoldOut = soldOut,
                        SoldAmount = storeStock.Balance,

                    };

                  var storeStockId =  UpdateStoreStockDetailsOnTransfer(storeStockUpdate);
                    return soldOut;
                }
                else if (storeStock.Balance < totalQuantity)
                {
                    soldOut = true;
                    var storeStockUpdate = new StoreStock()
                    {
                        StoreStockId = storeStock.StoreStockId,
                        StoreId = storeStock.StoreId,
                        StartStock = storeStock.StartStock,
                        StockId = storeStock.StockId,
                        ProductId = storeStock.ProductId,
                        StockBalance = storeStock.StockBalance,
                        BranchId = storeStock.BranchId,
                        Quantity = storeStock.Quantity,
                        SectorId = storeStock.SectorId,
                        TimeStamp = storeStock.TimeStamp,
                        InOrOut = storeStock.InOrOut,
                        Balance = 0,
                        CreatedOn = storeStock.CreatedOn,
                        SoldOut = soldOut,
                        SoldAmount =storeStock.Balance,

                    };

                  var storeStockId =   UpdateStoreStockDetailsOnTransfer(storeStockUpdate);
                    balance = totalQuantity - Convert.ToDouble(storeStock.Balance);
                    if (balance > 0)
                    {
                        soldOut = false;
                        return soldOut;
                    }
                   
                }
                else if (storeStock.Balance > totalQuantity)
                {
                    soldOut = false;
                   var stockbalance = Convert.ToDouble(storeStock.Balance) - totalQuantity ;
                   balance = 0;
                    var storeStockUpdate = new StoreStock()
                    {
                        StoreStockId = storeStock.StoreStockId,
                        StoreId = storeStock.StoreId,
                        StartStock = storeStock.StartStock,
                        StockId = storeStock.StockId,
                        ProductId = storeStock.ProductId,
                        StockBalance = storeStock.StockBalance,
                        BranchId = storeStock.BranchId,
                        Quantity = storeStock.Quantity,
                        SectorId = storeStock.SectorId,
                        TimeStamp = storeStock.TimeStamp,
                        InOrOut = storeStock.InOrOut,
                        Balance = stockbalance,
                        CreatedOn = storeStock.CreatedOn,
                        SoldOut = soldOut,
                        SoldAmount = totalQuantity,

                    };

                    var storeStockId = UpdateStoreStockDetailsOnTransfer(storeStockUpdate);
                  
                   
                    if (balance > 0)
                    {
                        soldOut = false;
                        return soldOut;
                    }
                   
                }
               
            }
            return soldOut;
        }
       
        public long IssueFlourTransfer(FlourTransfer flourTransfer, string userId)
        {
            //GradeSizeTotalsViewModel batchFlourGradesValues = GetTotalGradeSizeForBatchTransferQuantities(flourTransfer.Batches);

            long flourTransferId = 0;
            if(flourTransfer.FromSupplierStoreId == flourTransfer.ToReceiverStoreId)
            {
                flourTransferId = -3;
                return flourTransferId;
            }
            else
            {
                var gradeStore = CheckForFlourInStore(flourTransfer.Grades, flourTransfer.StoreId);
                if (gradeStore == -2)
                {
                    flourTransferId = gradeStore;
                    return flourTransferId;
                }
                else if (gradeStore == -1)
                {

                    flourTransferId = gradeStore;
                    return flourTransferId;
                }
                else
                {
                    var flourTransferDTO = new DTO.FlourTransferDTO()
                    {
                        FlourTransferId = flourTransfer.FlourTransferId,
                        TotalQuantity = flourTransfer.TotalQuantity,
                        BranchId = flourTransfer.BranchId,
                        FromSupplierStoreId = flourTransfer.FromSupplierStoreId,
                        ToReceiverStoreId = flourTransfer.ToReceiverStoreId,
                        Accept = flourTransfer.Accept,
                        Reject = flourTransfer.Reject,

                        StoreId = flourTransfer.StoreId,
                        Deleted = flourTransfer.Deleted,
                        CreatedBy = flourTransfer.CreatedBy,
                        CreatedOn = flourTransfer.CreatedOn
                    };

                    flourTransferId = this._dataService.SaveFlourTransfer(flourTransferDTO, userId);
                    if (flourTransfer.Grades != null)
                    {
                        if (flourTransfer.Grades.Any())
                        {
                            List<FlourTransferGradeSize> flourTransferGradeSizeList = new List<FlourTransferGradeSize>();

                            foreach (var grade in flourTransfer.Grades)
                            {
                                var gradeId = grade.GradeId;
                                if (grade.Denominations != null)
                                {
                                    if (grade.Denominations.Any())
                                    {
                                        foreach (var denomination in grade.Denominations)
                                        {
                                            var sizeRate = GetRateOfAParticularSize(denomination.DenominationId);

                                            var flourTransferGradeSize = new FlourTransferGradeSize()
                                            {
                                                GradeId = gradeId,
                                                SizeId = denomination.DenominationId,
                                                FlourTransferId = flourTransferId,
                                                StoreId = flourTransfer.ToReceiverStoreId,
                                                //Amount = Convert.ToDouble(denomination.Quantity * sizeRate),
                                                Rate = sizeRate,
                                                Quantity = denomination.Quantity,
                                                TimeStamp = DateTime.Now
                                            };
                                            flourTransferGradeSizeList.Add(flourTransferGradeSize);

                                            //Method that updates From store Flour into storeGradeSize table(storeFlourTransfer flourTransfer)
                                            var fromStoreFlourTransferGradeSize = new StoreFlourTransferGradeSizeDTO()
                                            {
                                                StoreId = flourTransfer.FromSupplierStoreId,
                                                GradeId = flourTransferGradeSize.GradeId,
                                                SizeId = flourTransferGradeSize.SizeId,
                                                Quantity = flourTransferGradeSize.Quantity,
                                            };

                                            this._dataService.SaveStoreFlourTransferGradeSize(fromStoreFlourTransferGradeSize, false);
                                        }
                                    }
                                }
                            }
                            this._dataService.PurgeFlourTransferGradeSize(flourTransferId);
                            this.SaveFlourTransferGradeSizeList(flourTransferGradeSizeList);
                        }
                    }
                    return flourTransferId;
                }
            }
           

      

        }

        public GradeSizeTotalsViewModel GetTotalGradeSizeForBatchTransferQuantities(List<Batch> batchList)
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

            List<long> gradesAddedToBatchTransfer = new List<long>();

            GradeSizeTotalsViewModel gradeSizeTotalsViewModel = new GradeSizeTotalsViewModel();
            List<Grade> grades = new List<Grade>();
            if (batchList.Count() == 0)
            {
                return gradeSizeTotalsViewModel;
            }
            else
            {
                int i = 0;
                for (i = 0; i <= batchList.Count();)
                {


                    var item = batchList.ElementAt(i);
                    var name = item.BatchId;

                    grades = item.AvailabeBatchGrades != null ? item.AvailabeBatchGrades : grades;
                    if (grades.Any())
                    {
                        //  foreach (var grade in grades)
                        for (int j = 0; j < grades.Count(); j++)
                        {
                            var grade = grades.ElementAt(j);
                            if (gradesAddedToBatchTransfer.Contains(grade.GradeId))
                            {
                                gradesAddedToBatchTransfer.Add(grade.GradeId);
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
                                                totalSuperTenQuantity = totalSuperTenQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 25:
                                                totalSuperTwoFiveQuantity = totalSuperTwoFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                
                                                break;
                                            case 50:
                                                totalSuperFiveZeroQuantity = totalSuperFiveZeroQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 100:
                                                totalSuperHundredQuantity = totalSuperHundredQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 5:
                                                totalSuperFiveQuantity = totalSuperFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                          

                                                break;
                                            case 1:
                                                totalSuperOneQuantity = totalSuperOneQuantity + Convert.ToInt32(denom.QuantityToRemove);
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
                                                totalNumberOneTenQuantity = totalNumberOneTenQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 25:
                                                totalNumberOneTwoFiveQuantity = totalNumberOneTwoFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                

                                                break;
                                            case 50:
                                                totalNumberOneFiveZeroQuantity = totalNumberOneFiveZeroQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 100:
                                                totalNumberOneHundredQuantity = totalNumberOneHundredQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                
                                                break;
                                            case 5:
                                                totalNumberOneFiveQuantity = totalNumberOneFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                      
                                                break;
                                            case 1:
                                                totalNumberOneOneQuantity = totalNumberOneOneQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
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
                                                totalNumberOneHalfTenQuantity = totalNumberOneHalfTenQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                
                                                break;
                                            case 25:
                                                totalNumberOneHalfTwoFiveQuantity = totalNumberOneHalfTwoFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                
                                                break;
                                            case 50:
                                                totalNumberOneHalfFiveZeroQuantity = totalNumberOneHalfFiveZeroQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                
                                                break;
                                            case 100:
                                                totalNumberOneHalfHundredQuantity = totalNumberOneHalfHundredQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 5:
                                                totalNumberOneHalfFiveQuantity = totalNumberOneHalfFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 1:
                                                totalNumberOneHalfOneQuantity = totalNumberOneHalfOneQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
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
                                                totalKabaleTenQuantity = totalKabaleTenQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 25:
                                                totalKabaleTwoFiveQuantity = totalKabaleTwoFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
                                                break;
                                            case 50:
                                                totalKabaleFiveZeroQuantity = totalKabaleFiveZeroQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                break;
                                            case 100:
                                                totalKabaleHundredQuantity = totalKabaleHundredQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                break;
                                            case 5:
                                                totalKabaleFiveQuantity = totalKabaleFiveQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                                break;
                                            case 1:
                                                totalKabaleOneQuantity = totalKabaleOneQuantity + Convert.ToInt32(denom.QuantityToRemove);
                                               
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
                        batchList.RemoveAll(r => r.BatchId == item.BatchId);

                    }

                    else
                    {

                        batchList.RemoveAll(r => r.BatchId == item.BatchId);

                    }

                    i = 0;
                    if (batchList.Count() == 0)
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

        public long SaveFlourTransfer(FlourTransfer flourTransfer, string userId)
        {
            
             long issueId = 0;
            if (flourTransfer.Issuing == "YES")
            {
                var flourTransferObject = new FlourTransfer()
                {

                    FlourTransferId = flourTransfer.FlourTransferId,
                    FromSupplierStoreId = flourTransfer.FromSupplierStoreId,
                    TotalQuantity = flourTransfer.TotalQuantity,
                    BranchId = flourTransfer.BranchId,
                    ToReceiverStoreId = flourTransfer.ToReceiverStoreId,
                    StoreId = flourTransfer.StoreId,
                    Accept = flourTransfer.Accept,
                    Reject = flourTransfer.Reject,
                   Batches = flourTransfer.Batches,
                    Deleted = flourTransfer.Deleted,
                    CreatedBy = flourTransfer.CreatedBy,
                    CreatedOn = flourTransfer.CreatedOn,
                    Grades = flourTransfer.Grades,
                };

                 issueId = IssueFlourTransfer(flourTransferObject, userId);
                if(issueId > 0)
                {
                    if (flourTransferObject.Batches.Any())
                    {


                        foreach (var batch in flourTransferObject.Batches)
                        {
                            #region NewestCode

                            double batchQuantity = 0;
                            //Grade foundGrade = new Grade();


                            if (batch.AvailabeBatchGrades.Any())
                            {

                                //bool hasGrade = false;

                                List<BatchTransferGradeSizeDTO> batchTransferGradeSizes = new List<BatchTransferGradeSizeDTO>();

                                #region foreach
                                foreach (var grade in batch.AvailabeBatchGrades)
                                {
                                    int denom = 0;
                                    foreach (var gradeDenom in grade.Denominations)
                                    {
                                        //making a value to use to get which number in the denomination list is the loop on ie 0 for first or 1 for the second

                                        batchQuantity = batchQuantity + (gradeDenom.QuantityToRemove * gradeDenom.Value);

                                        var batchTransferGradeSize = new BatchTransferGradeSizeDTO()
                                        {
                                            BatchId = batch.BatchId,
                                            GradeId = grade.GradeId,
                                            SizeId = gradeDenom.DenominationId,
                                            Quantity = gradeDenom.QuantityToRemove,
                                            FlourTransferId = issueId,

                                        };

                                        batchTransferGradeSizes.Add(batchTransferGradeSize);

                                        denom = denom + 1;
                                    }
                                    #endregion
                                    _dataService.SaveBatchFlourTransferGradeSize(batchTransferGradeSizes);
                                    batchTransferGradeSizes.Clear();


                                }


                            }



                            #endregion
                            var flourTransferBatchDTO = new FlourTransferBatchDTO()
                            {
                                BatchId = batch.BatchId,
                                FlourTransferId = issueId,
                                BatchQuantity = batchQuantity,

                            };
                            this._dataService.SaveFlourTransferBatch(flourTransferBatchDTO);
                        }

                    }
                }
                
                return issueId;
            }
            return issueId;
        }


        public StoreGrade GetStoreFlourTransferStock(long storeId)
        {
            var result = this._dataService.GetStoreFlourTransferStock(storeId);
            var storeGrade = GetStoreFlourTransferStockForView(MapEFToModel(result));
            return storeGrade;
        }
        public StoreGrade GetStoreFlourTransferStockForView(IEnumerable<Models.StoreFlourTransferGradeSize> list)
        {
            var storeGrade = new StoreGrade()
            {
                StoreFlourTransferGradeSizes = list,
            };


            return storeGrade;
        }

        private long UpdateStoreStockDetailsOnTransfer(StoreStock storeStock)
        {
         var storeStockId=   _stockService.SaveStoreStockFlourTransfer(storeStock);
         return storeStockId;

        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FlourTransferId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long FlourTransferId, string userId)
        {
            _dataService.MarkAsDeleted(FlourTransferId, userId);
        }

        void SaveFlourTransferGradeSizeList(List<FlourTransferGradeSize> flourTransferGradeSizeList)
        {
            if (flourTransferGradeSizeList != null)
            {
                if (flourTransferGradeSizeList.Any())
                {
                    foreach (var flourTransferGradeSize in flourTransferGradeSizeList)
                    {
                        var flourTransferGradeSizeDTO = new DTO.FlourTransferGradeSizeDTO()
                        {
                            FlourTransferId = flourTransferGradeSize.FlourTransferId,
                            GradeId = flourTransferGradeSize.GradeId,
                            StoreId = flourTransferGradeSize.StoreId,
                            Quantity = flourTransferGradeSize.Quantity,
                            SizeId = flourTransferGradeSize.SizeId,
                            Rate = flourTransferGradeSize.Rate,
                            Amount = flourTransferGradeSize.Amount,
                            TimeStamp = flourTransferGradeSize.TimeStamp
                        };
                        this.SaveFlourTransferGradeSize(flourTransferGradeSizeDTO);

                    }
                }
            }
        }

        public long CheckForFlourInStore(List<Grade> grades, long storeId)
        {

            foreach (var grade in grades)
            {
                
                if (grade.Denominations != null)
                {
                    foreach (var denomination in grade.Denominations)
                    {
                        var result = _stockService.GetStoreGradeSize(grade.GradeId, denomination.DenominationId, storeId);
                        if (result == null)
                        {
                            //grade doesn't exist in store
                            return -2;
                        }
                        else
                        {
                            if (result.Quantity < denomination.Quantity)
                            {
                                //quantity is greater than what you have in store
                                return -1;
                            }
                        }
                    }
                }


            }
            return 1;
        }

        void SaveFlourTransferGradeSize(FlourTransferGradeSizeDTO FlourTransferGradeSizeDTO)
        {
            _dataService.SaveFlourTransferGradeSize(FlourTransferGradeSizeDTO);
        }

        private IEnumerable<FlourTransferBatch> GetAllBatchesForAFlourTransfer(long flourTransferId)
        {
            var results = _dataService.GetAllBatchesForAFlourTransfer(flourTransferId);
            return MapEFToModel(results);
        }

        #region Mapping Methods

        public IEnumerable<FlourTransfer> MapEFToModel(IEnumerable<EF.Models.FlourTransfer> data)
        {
            var list = new List<FlourTransfer>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps FlourTransfer EF object to FlourTransfer Model Object and
        /// returns the FlourTransfer model object.
        /// </summary>
        /// <param name="result">EF FlourTransfer object to be mapped.</param>
        /// <returns>FlourTransfer Model Object.</returns>
        public FlourTransfer MapEFToModel(EF.Models.FlourTransfer data)
        {
            if (data != null)
            {
                var flourTransfer = new FlourTransfer()
                {


                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    BranchId = data.BranchId,
                    TotalQuantity = data.TotalQuantity,
                    StoreId = data.StoreId,
                    FlourTransferId = data.FlourTransferId,
                    FromSupplierStoreId = data.FromSupplierStoreId,
                    Accept = data.Accept,
                    Reject = data.Reject,

                    ToReceiverStoreId = data.ToReceiverStoreId,
                    ReceiverStoreName = data.Store2 != null ? data.Store2.Name : "",
                    SupplierStoreName = data.Store1 != null ? data.Store1.Name : "",
                    StoreName = data.Store != null ? data.Store.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser2)
                };

                var batches = GetAllBatchesForAFlourTransfer(data.FlourTransferId);
                List<FlourTransferBatch> flourTransferBatchList = new List<FlourTransferBatch>();
                if (batches.Any())
                {
                    foreach (var batch in batches)
                    {
                        var flourbatch = new FlourTransferBatch()
                        {
                            BatchId = batch.BatchId,
                            BatchNumber = batch.BatchNumber,
                            FlourTransferId = batch.FlourTransferId,

                        };
                        flourTransferBatchList.Add(flourbatch);

                    }
                    flourTransfer.FlourTransferBatches = flourTransferBatchList;
                }
                if (data.FlourTransferGradeSizes != null)
                {
                    if (data.FlourTransferGradeSizes.Any())
                    {
                        List<Grade> grades = new List<Grade>();
                        //old query
                        //var distinctGrades = data.FlourTransferGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        //new query as at 8/7/2019
                        var distinctGrades = data.FlourTransferGradeSizes.Where(a => a.FlourTransferId == data.FlourTransferId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        
                        foreach (var flourTransferGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = flourTransferGradeSize.Grade.GradeId,
                                Value = flourTransferGradeSize.Grade.Value,
                                CreatedOn = flourTransferGradeSize.Grade.CreatedOn,
                                TimeStamp = flourTransferGradeSize.Grade.TimeStamp,
                                Deleted = flourTransferGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(flourTransferGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(flourTransferGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (flourTransferGradeSize.Grade.FlourTransferGradeSizes != null)
                            {
                                if (flourTransferGradeSize.Grade.FlourTransferGradeSizes.Any())
                                {
                                    //old query
                                   // var distinctSizes = flourTransferGradeSize.Grade.FlourTransferGradeSizes.GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                    
                                    //new query as at 8/7/2019
                                    var distinctSizes = flourTransferGradeSize.Grade.FlourTransferGradeSizes.Where(a => a.FlourTransferId == data.FlourTransferId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                   
                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity
                                        };
                                        // FlourTransfer.TotalQuantity += (ogs.Quantity * ogs.Size.Value);
                                        
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                        }
                        flourTransfer.Grades = grades;
                    }
                }

                return flourTransfer;
            }
            return null;
        }


        public StoreFlourTransferGradeSize MapEFToModel(EF.Models.StoreFlourTransferGradeSize data)
        {
            var storeFlourTransferGradeSize = new StoreFlourTransferGradeSize()
            {

                GradeId = data.GradeId,
                Quantity = data.Quantity,
                SizeId = data.SizeId,
                SizeValue = data.Size.Value,
                GradeValue = data.Grade.Value,
                StoreId = data.StoreId,
                StoreName = data.Store != null ? data.Store.Name : "",
                TimeStamp = data.TimeStamp,

            };
            return storeFlourTransferGradeSize;

        }


        private IEnumerable<StoreFlourTransferGradeSize> MapEFToModel(IEnumerable<EF.Models.StoreFlourTransferGradeSize> data)
        {
            var list = new List<StoreFlourTransferGradeSize>();

            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }

            return list;
        }


        public FlourTransferBatch MapEFToModel(EF.Models.FlourTransferBatch data)
        {
            var flourTransferBatch = new FlourTransferBatch()
            {

                BatchId = data.BatchId,
                FlourTransferId = data.FlourTransferId,
               CreatedOn = data.CreatedOn,
               BatchNumber = data.Batch != null?data.Batch.Name:"",
                TimeStamp = data.TimeStamp,

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
       #endregion
    }
}
