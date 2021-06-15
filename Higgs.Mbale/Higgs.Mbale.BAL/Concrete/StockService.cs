using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Helpers;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class StockService : IStockService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(StockService));
        private IStockDataService _dataService;
        private IUserService _userService;
        private IGradeService _gradeService;
        

        public StockService(IStockDataService dataService,IUserService userService, IGradeService gradeService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._gradeService = gradeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StockId"></param>
        /// <returns></returns>
        public Stock GetStock(long stockId)
        {
            var result = this._dataService.GetStock(stockId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stock> GetAllStocks()
        {
            var results = this._dataService.GetAllStocks();
            return MapEFToModel(results);
        }

        public Stock GetStockForAParticularBatchAndProduct(long batchId, long productId, long storeId)
        {
            var results = this._dataService.GetStockForAParticularBatchAndProduct(batchId, productId, storeId);
            return MapEFToModel(results);
        }
        public IEnumerable<StoreGradeSize> GetStoreGradeSizeForParticularGradeAtAStore(long gradeId, long storeId)
        {
            var results = this._dataService.GetStoreGradeSizeForParticularGradeAtAStore(gradeId, storeId);
            return MapEFToModel(results);
        }
        public IEnumerable<Stock> GetAllStocksForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllStocksForAParticularBranch(branchId);
            return MapEFToModel(results);
        }

        private double GetStockBalanceForLastStockTransaction(long storeId,long productId)
        {
            double balance = 0;
           
                var result = this._dataService.GetLatestStockForAParticularStore(storeId,productId);
                if (result.StoreStockId > 0)
                {
                    balance = result.StockBalance;
                }

                return balance;
           
        }

     public StoreStock  GetStockForAParticularStoreForDelivery(long storeId, long productId,long batchId)
     {
         var result = this._dataService.GetStockForAParticularStoreForDelivery(storeId, productId,batchId);
         return MapEFToModel(result);

     }
     
  
     
        public IEnumerable<StoreStock> GetStocksForAParticularStore(long storeId)
        {
            var results = this._dataService.GetStocksForAParticularStore(storeId);
            return MapEFToModel(results);
        }

        public IEnumerable<Stock> GetStockForAParticularBranchForTransfer(long branchId, long productId)
        {
            var results = this._dataService.GetStockForAParticularBranchForTransfer(branchId, productId);
            var storeStocks = MapEFToModel(results);
            List<Stock> stocks = new List<Stock>();
            foreach (var storeStock in storeStocks)
            {
                var stockId = storeStock.StockId;
                var stock = GetStock(stockId);
                stocks.Add(stock);
            }
            return stocks;
        }
        public long SaveStoreStockFlourTransfer(StoreStock storeStock)
        {
            var storeStockDTO = new DTO.StoreStockDTO()
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
                Balance = storeStock.Balance,
                CreatedOn = storeStock.CreatedOn,
                SoldOut = storeStock.SoldOut,
                SoldAmount = storeStock.SoldAmount,

            };

         var storeStockId =   this._dataService.SaveStoreStock(storeStockDTO);
         return storeStockId;
        }

        public void SaveStoreGradeSize(StoreGradeSize storeGradeSize, bool inOrOut)
        {
            var storeGradeSizeDTO = new StoreGradeSizeDTO()
            {

                GradeId = storeGradeSize.GradeId,
                SizeId = storeGradeSize.SizeId,
                StoreId = storeGradeSize.StoreId,
                Quantity = storeGradeSize.Quantity,
                TimeStamp = DateTime.Now
            };
            this._dataService.SaveStoreGradeSize(storeGradeSizeDTO,inOrOut);
        }

        public void SaveStoreStock(StoreStock storeStock, bool inOrOut)
        {
         
            double startStock = 0;
            double OldStockBalance = 0;
            double NewStockBalance = 0;


                OldStockBalance = GetStockBalanceForLastStockTransaction(storeStock.StoreId, storeStock.ProductId);
                startStock = OldStockBalance;
           

            if (inOrOut == true)
            {
                NewStockBalance = OldStockBalance + storeStock.Quantity;
            }
            else
            {
                NewStockBalance = OldStockBalance - storeStock.Quantity;
            }

            var storeStockDTO = new DTO.StoreStockDTO()
            {
                StoreStockId = storeStock.StoreStockId,
                StoreId = storeStock.StoreId,
                StartStock = startStock,
                StockId = storeStock.StockId,
                ProductId = storeStock.ProductId,
                StockBalance = NewStockBalance,
                BranchId = storeStock.BranchId,
               Quantity = storeStock.Quantity,
                SectorId = storeStock.SectorId,
                TimeStamp = storeStock.TimeStamp,
                InOrOut = inOrOut,
                Balance = storeStock.Balance,
                CreatedOn = storeStock.CreatedOn,
                SoldOut = storeStock.SoldOut,
                SoldAmount = storeStock.SoldAmount,

            };

          this._dataService.SaveStockStore(storeStockDTO);
        }

        public long SaveStock(Stock stock, string userId)
        {
            //saves stock object into stock table
            var stockDTO = new DTO.StockDTO()
            {
                InOrOut = stock.InOrOut,
                ProductId = stock.ProductId,
                BatchId = stock.BatchId,
                BranchId = stock.BranchId,
                SectorId = stock.SectorId,  
                SoldOut = stock.SoldOut,
                Deleted = stock.Deleted,
                StoreId = stock.StoreId,
                CreatedBy = stock.CreatedBy,
                CreatedOn = stock.CreatedOn,
                ProductOutPut = stock.ProductOutPut,
            };

           var stockId = this._dataService.SaveStock(stockDTO, userId);
          

           if (stock.StockGradeSize != null)
           {
               List<StockGradeSize> stockGradeSizeList = new List<StockGradeSize>();
              
               foreach (var stockGradeSize in stock.StockGradeSize)
               {
                   var stockGrade_Size = new StockGradeSize(){
                       StockId = stockId,
                     GradeId = stockGradeSize.GradeId,
                     SizeId = stockGradeSize.SizeId,
                    Quantity = stockGradeSize.Quantity,
                   };

                   stockGradeSizeList.Add(stockGrade_Size);  

        //Method that adds flour output into storeGradeSize table(store flour stock)
                   var storeGradeSize = new StoreGradeSizeDTO(){
                     StoreId = stock.StoreId,
                     GradeId = stockGradeSize.GradeId,
                     SizeId = stockGradeSize.SizeId,
                    Quantity = stockGradeSize.Quantity,
                   };

                   this._dataService.SaveStoreGradeSize(storeGradeSize,stock.InOrOut);
               }

               this._dataService.PurgeStockGradeSize(stockId);
               this.SaveStockGradeSizeList(stockGradeSizeList);
           }
            //save stock and productId into stockproduct table
            var stockProductDTO = new StockProductDTO(){
                StockId = stockId,
                ProductId = stockDTO.ProductId,
                Quantity = stockDTO.ProductOutPut,
                
            };
           this._dataService.PurgeStockProduct(stockId);
           this._dataService.SaveStockProduct(stockProductDTO);

             var storeStock = new StoreStock()
            {
                
                StoreId = stock.StoreId,
                ProductId = stock.ProductId,
                BranchId = stock.BranchId,
                StockId= stockId,
               Quantity = stock.ProductOutPut,
                SectorId = stock.SectorId,
                InOrOut = stock.InOrOut,
               

            };
             SaveStoreStock(storeStock, stock.InOrOut);
           return stockId;
                      
        }

        public StoreStock GetStoreStockForParticularStock(long stockId, long productId, long storeId)
        {
            var results = this._dataService.GetStoreStockForParticularStock(stockId, productId, storeId);
            return MapEFToModel(results);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long stockId, string userId)
        {
            _dataService.MarkAsDeleted(stockId, userId);
        }

        void SaveStockGradeSizeList(List<StockGradeSize> stockGradeSizeList)
        {
            if (stockGradeSizeList != null)
            {
                if (stockGradeSizeList.Any())
                {
                    foreach (var stockGradeSize in stockGradeSizeList)
                    {
                        var stockGradeSizeDTO = new DTO.StockGradeSizeDTO()
                        {
                            StockId = stockGradeSize.StockId,
                            GradeId = stockGradeSize.GradeId,
                            Quantity = stockGradeSize.Quantity,
                            SizeId = stockGradeSize.SizeId,
                            TimeStamp = stockGradeSize.TimeStamp
                        };
                        this.SaveStockGradeSize(stockGradeSizeDTO);
                    }
                }
            }
        }
        void SaveStockGradeSize(StockGradeSizeDTO stockGradeSizeDTO)
        {
            _dataService.SaveStockGradeSize(stockGradeSizeDTO);
        }

        //public StoreGradeSize GetStoreFlourGradeSize(long gradeId, long sizeId, long storeId)
        //{
        //    var result = this._dataService.GetStoreFlourGradeSize(gradeId, sizeId, storeId);
        //    return MapEFToModel(result);
        //}  

        public StoreGrade GetStoreFlourStock(long storeId)
        {
            var result = this._dataService.GetStoreFlourStock(storeId);
            var storeGrade = GetStoreFlourStockForView(MapEFToModel(result));
           return storeGrade;
        }
        public StoreGrade GetStoreFlourStockForView(IEnumerable<Models.StoreGradeSize> list)
        {
            var storeGrade = new StoreGrade()
            {
                StoreSizeGrades = list,
            };

            
            return storeGrade;
        }
        public void UpdateStoreStockAndStockDetails(long stockId, long productId, bool soldOut, string userId)
        {
            _dataService.UpdateStoreStockAndStockDetails(stockId, productId, soldOut, userId);
        }

        public StoreGradeSize GetStoreGradeSize(long gradeId, long sizeId, long storeId)
        {
            var result = this._dataService.GetStoreGradeSize(gradeId, sizeId, storeId);
            return MapEFToModel(result);
        }
        #region Mapping Methods

        private IEnumerable<Stock> MapEFToModel(IEnumerable<EF.Models.Stock> data)
        {
            var list = new List<Stock>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Stock EF object to Stock Model Object and
        /// returns the Stock model object.
        /// </summary>
        /// <param name="result">EF Stock object to be mapped.</param>
        /// <returns>Stock Model Object.</returns>
        public Stock MapEFToModel(EF.Models.Stock data)
        {
            if (data != null)
            {

                var stock = new Stock()
                {
                    BatchId = data.BatchId,
                    SectorId = data.SectorId,
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    ProductId = data.ProductId,
                    ProductName = data.Product != null ? data.Product.Name : "",
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    BranchId = data.BranchId,
                    BatchNumber = data.Batch != null ? data.Batch.Name : "",
                    StockId = data.StockId,
                    InOrOut = data.InOrOut,
                    StoreId = data.StoreId,
                    StoreName = data.Store != null ? data.Store.Name : "",
                    StockInOrOut = (data.InOrOut == true) ? "Stock In" : "Stock Out",
                    SoldOut = data.SoldOut,
                    SoldOutOrNot = (data.SoldOut == true) ? "Sold Out" : "Not yet Done",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1)
                };

                if (data.StockGradeSizes != null)
                {
                    if (data.StockGradeSizes.Any())
                    {
                        List<Grade> grades = new List<Grade>();
                        var distinctGrades = data.StockGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        foreach (var stockGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = stockGradeSize.Grade.GradeId,
                                Value = stockGradeSize.Grade.Value,
                                CreatedOn = stockGradeSize.Grade.CreatedOn,
                                TimeStamp = stockGradeSize.Grade.TimeStamp,
                                Deleted = stockGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(stockGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(stockGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (stockGradeSize.Grade.StockGradeSizes != null)
                            {
                                if (stockGradeSize.Grade.StockGradeSizes.Any())
                                {
                                    var distinctSizes = stockGradeSize.Grade.StockGradeSizes.GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity
                                        };
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                        }
                        stock.Grades = grades;
                    }
                }

                var stockProduct = _dataService.GetStockProductForAStock(data.StockId);


                if (stockProduct != null)
                {
                    var stock_Product = new StockProduct()
                    {
                        StockId = stockProduct.StockId,
                        Quantity = stockProduct.Quantity,

                    };
                    stock.ProductOutPut = stock_Product.Quantity;

                }


                return stock;
            }
            return null;
        }


        private IEnumerable<StoreStock> MapEFToModel(IEnumerable<EF.Models.StoreStock> data)
        {
            var list = new List<StoreStock>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps StoreStock EF object to StoreStock Model Object and
        /// returns the StoreStock model object.
        /// </summary>
        /// <param name="result">EF StoreStock object to be mapped.</param>
        /// <returns>StoreStock Model Object.</returns>
        public StoreStock MapEFToModel(EF.Models.StoreStock data)
        {
            if (data != null)
            {
                var stock = GetStock(data.StockId);

                var storeStock = new StoreStock()
                {

                    SectorId = data.SectorId,
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    ProductId = data.ProductId,
                    ProductName = data.Product != null ? data.Product.Name : "",
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    BranchId = data.BranchId,
                    StockId = data.StockId,
                    InOrOut = data.InOrOut,
                    Quantity = data.Quantity,
                    StockBalance = data.StockBalance,
                    StartStock = data.StartStock,
                    Balance = (data.Balance != null) ? data.Balance : 0,
                    StoreStockId = data.StoreStockId,
                    BatchNumber = stock.BatchNumber,
                    StockInOrOut = (data.InOrOut == true) ? "Stock In" : "Stock Out",
                    SoldOut = data.SoldOut,
                    SoldOutOrNot = (data.SoldOut == true) ? "Stock Sold Out" : "Not Yet Done",
                    SoldAmount = (data.SoldAmount != null) ? data.SoldAmount : 0,
                    StoreId = data.StoreId,
                    StoreName = data.Store != null ? data.Store.Name : "",
                    TimeStamp = data.TimeStamp,
                    Stock = stock,

                };

                return storeStock;
            }
            return null;
        }


        private IEnumerable<StoreGradeSize> MapEFToModel(IEnumerable<EF.Models.StoreGradeSize> data)
        {
            var list = new List<StoreGradeSize>();
            
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
           
            return list;
        }

        /// <summary>
        /// Maps StoreGradeSize EF object to StoreGradeSize Model Object and
        /// returns the StoreGradeSize model object.
        /// </summary>
        /// <param name="result">EF StoreGradeSize object to be mapped.</param>
        /// <returns>StoreGradeSize Model Object.</returns>
        public StoreGradeSize MapEFToModel(EF.Models.StoreGradeSize data)
        {
            if (data != null)
            {
                var storeGradeSize = new StoreGradeSize()
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

                return storeGradeSize;
            }
            return null;
        }


       #endregion
    }
}
