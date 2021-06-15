using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;
using log4net;
using System.Configuration;


namespace Higgs.Mbale.BAL.Concrete
{
public   class CashSaleService : ICashSaleService
    {
        private long flourTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["FlourSaleTransactionSubTypeId"]);
        private long branTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["BranSaleTransactionSubTypeId"]);
        private long riceTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["RiceSaleTransactionSubTypeId"]);

        private long cashReceiptId = Convert.ToInt64(ConfigurationManager.AppSettings["CashReceipt"]);
        private double cashSaleBalance = 0, batchBrandBalance = 0, soldOutAmount = 0;
        ////private bool hasBalance = false;

        List<CashSaleBatch> batchCashSaleList = new List<CashSaleBatch>();

        ILog logger = log4net.LogManager.GetLogger(typeof(CashSaleService));
        private ICashSaleDataService _dataService;
        private IUserService _userService;
        private ITransactionDataService _transactionDataService;
        private ITransactionSubTypeService _transactionSubTypeService;
       private ICashService _cashService;
       private IMaizeBrandStoreDataService _maizeBrandStoreDataService;
        //private IBatchService _batchService;
       private IMaizeBrandStoreService _maizeBrandStoreService;
        private IDocumentService _documentService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private IStockDataService _stockDataService;
        private IStockService _stockService;


        public CashSaleService(ICashSaleDataService dataService, IUserService userService, ITransactionDataService transactionDataService,
            ITransactionSubTypeService transactionSubTypeService, IDocumentService documentService,
             ICashService cashService,IAccountTransactionActivityService accountTransactionActivityService,IBatchService batchService,
            IStockDataService stockDataService,IStockService stockService,IMaizeBrandStoreService maizeBrandStoreService,
            IMaizeBrandStoreDataService maizeBrandStoreDataService
            )
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionDataService = transactionDataService;
            this._transactionSubTypeService = transactionSubTypeService;
            this._documentService = documentService;
            this._cashService = cashService;
            //this._batchService = batchService;
            this._maizeBrandStoreDataService = maizeBrandStoreDataService;
            this._maizeBrandStoreService = maizeBrandStoreService;
            this._accountTransactionActivityService = accountTransactionActivityService;
            this._stockDataService = stockDataService;
            this._stockService = stockService;
        }

      
        public CashSale GetCashSale(long cashSaleId)
        {
            var result = this._dataService.GetCashSale(cashSaleId);
            return MapEFToModel(result);
        }

        public IEnumerable<CashSale> GetAllCashSalesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllCashSalesForAParticularBranch(branchId);
            return MapEFToModel(results);

        }

     public IEnumerable<CashSale> GetAllCashSalesForAParticularStore(long storeId)
        {
            var results = this._dataService.GetAllCashSalesForAParticularStore(storeId);
            return MapEFToModel(results);

        }

     public IEnumerable<CashSaleViewModel> GetTenLatestCashSalesForAParticularBranch(long branchId,int offSet,int pageSize)
     {
         var results = this._dataService.GetTenLatestCashSalesForAParticularBranch(branchId, offSet, pageSize);
         return MapEFCashSaleToModelCashSaleViewModel(results);
     }
        public IEnumerable<CashSale> GetAllCashSales()
        {
            var results = this._dataService.GetAllCashSales();
            return MapEFToModel(results);
        }

        private Grade GetGradeSameAsOrderGrade(List<Grade> deliveryGrades, long orderGradeId)
        {
            var gradeFound = new Grade();
            List<long> gradeIds = new List<long>();
            foreach (var grade in deliveryGrades)
            {
                long gradeId = grade.GradeId;
                gradeIds.Add(gradeId);
            }

            foreach (var foundGradeId in gradeIds)
            {
                if (orderGradeId == foundGradeId)
                {
                    foreach (var grade in deliveryGrades)
                    {
                        if (grade.GradeId == foundGradeId)
                        {
                            gradeFound = grade;


                        }
                    }
                }
            }
            return gradeFound;
        }


        #region makeDelivery



        #region flour

        private bool CheckIfBatchHasOrderGrade(long orderGrade, List<Grade> deliveryGrades)
        {
            bool hasGrade = false;
            List<long> gradeIds = new List<long>();
            foreach (var grade in deliveryGrades)
            {
                long gradeId = grade.GradeId;
                gradeIds.Add(gradeId);
            }

            foreach (var foundGradeId in gradeIds)
            {
                if (orderGrade == foundGradeId)
                {
                    hasGrade = true;
                    return hasGrade;
                }
            }
            return hasGrade;
        }
       
       #endregion


        #region Brand

        //private MakeDelivery ReduceBatchBrandStock(Batch batch, double totalQuantity, string userId)
        private MakeDelivery ReduceBatchBrandStock(MaizeBrandStore brandStore, double totalQuantity, string userId)
        {
            var soldOut = false;
            MakeDelivery makeDelivery = new MakeDelivery();


            if (brandStore != null)
            {

                if (totalQuantity > brandStore.Quantity)
                {
                    cashSaleBalance = totalQuantity -Convert.ToDouble(brandStore.Quantity);
                }
                else
                {
                    batchBrandBalance = Convert.ToDouble(brandStore.Quantity)- totalQuantity;
                }

                if (totalQuantity <= Convert.ToDouble(brandStore.Quantity))
                {
                    batchBrandBalance = Convert.ToDouble(brandStore.Quantity) - totalQuantity;
                    _maizeBrandStoreService.UpdateMaizeBrandBatchBalance(brandStore.MaizeBrandStoreId, batchBrandBalance, userId);
                    //_batchService.UpdateBatchBrandBalance(batch.BatchId, batchBrandBalance, userId);
                    if (batchBrandBalance > 0)
                    {

                        makeDelivery = new MakeDelivery()
                        {
                            StockReduced = soldOut,
                            OrderQuantityBalance = cashSaleBalance,
                            BatchBrandBalance = batchBrandBalance,

                        };



                        return makeDelivery;
                    }
                    else
                    {

                        makeDelivery = new MakeDelivery()
                        {
                            StockReduced = true,
                            OrderQuantityBalance = cashSaleBalance,
                            BatchBrandBalance = batchBrandBalance,

                        };
                        return makeDelivery;
                    }

                }
                else
                {
                    batchBrandBalance = 0;
                    //_batchService.UpdateBatchBrandBalance(batch.BatchId, batchBrandBalance, userId);
                    cashSaleBalance = totalQuantity - Convert.ToDouble(brandStore.Quantity);


                    makeDelivery = new MakeDelivery()
                    {
                        StockReduced = true,
                        OrderQuantityBalance = cashSaleBalance,
                        BatchBrandBalance = batchBrandBalance,

                    };
                    return makeDelivery;
                }


            }
            return makeDelivery;
        }

        private MakeDelivery MakeBatchCashSaleGradeSize(long cashSaleId, CashSale cashSale)
        {
            var makeDelivery = new MakeDelivery();
           
            foreach (var item in cashSale.Batches)
            {
                double batchAmount = 0, batchQuantity = 0;


                //new code for july 12th 2020
                #region NewestCode
                Grade foundGrade = new Grade();

                if (item.AvailabeBatchGrades.Any())
                {

                    bool hasGrade = false;

                    List<CashSaleBatchGradeSizeDTO> batchCashSaleGradeSizes = new List<CashSaleBatchGradeSizeDTO>();

                    #region foreach
                    foreach (var orderGrade in cashSale.SelectedGrades)
                    {
                        hasGrade = CheckIfBatchHasOrderGrade(orderGrade.GradeId, item.AvailabeBatchGrades);

                        if (hasGrade)
                        {
                            foundGrade = GetGradeSameAsOrderGrade(item.AvailabeBatchGrades, orderGrade.GradeId);
                            int denom = 0;
                            foreach (var foundGradeDenom in foundGrade.Denominations)
                            {
                                //making a value to use to get which number in the denomination list is the loop on ie 0 for first or 1 for the second

                                batchQuantity = batchQuantity + (foundGradeDenom.QuantityToRemove * foundGradeDenom.Value);
                                batchAmount = batchAmount + (orderGrade.Denominations[denom].Price * foundGradeDenom.QuantityToRemove);
                                var batchCashSaleGradeSize = new CashSaleBatchGradeSizeDTO()
                                {
                                    BatchId = item.BatchId,
                                    GradeId = foundGrade.GradeId,
                                    SizeId = foundGradeDenom.DenominationId,
                                    Quantity = foundGradeDenom.QuantityToRemove,
                                    Amount = Convert.ToDouble(foundGradeDenom.QuantityToRemove * orderGrade.Denominations[denom].Price),
                                    Price = orderGrade.Denominations[denom].Price,
                                    CashSaleId = cashSaleId,

                                };

                                batchCashSaleGradeSizes.Add(batchCashSaleGradeSize);

                                denom = denom + 1;
                            }
                            #endregion
                            _dataService.SaveBatchCashSaleGradeSize(batchCashSaleGradeSizes);
                            batchCashSaleGradeSizes.Clear();

                        }


                    }


                    var batchCashSale = new CashSaleBatchDTO()
                    {
                        BatchId = item.BatchId,
                        BatchQuantity = batchQuantity,
                        Price = cashSale.Price,
                        CashSaleId = cashSaleId,
                        ProductId = cashSale.ProductId,
                        Amount = batchAmount,
                    };
                    this.SaveCashSaleBatch(batchCashSale); 


                    makeDelivery = new MakeDelivery()
                    {
                        StockReduced = true,
                        OrderQuantityBalance = 0,
                    };


                    #endregion

                }


            }
            return makeDelivery;

        }

        private MakeDelivery MakeBrandCashSaleRecord(long storeId, CashSale cashSale, string userId)
        {
            var makeDelivery = new MakeDelivery();

            if (cashSale.Batches == null || !cashSale.Batches.Any())
            {
                return makeDelivery;
            }
            else
            {
                List<Batch> batchesList = new List<Batch>();
                List<MaizeBrandStore> maizeBrandStoreList = new List<MaizeBrandStore>();
               
                foreach (var cashSaleBatch in cashSale.Batches)
                {
                    var maizeBrandStores = _maizeBrandStoreService.GetAllMaizeBrandStoreForAParticularBatch(cashSaleBatch.BatchId);
                    
                    if (maizeBrandStores.Any())
                    {
                        foreach (var item in maizeBrandStores)
                        {
                            maizeBrandStoreList.Add(item);
                        }
                    }
                   
                }

                
                List<MaizeBrandStore> SortedMaizeBrandStoreList = maizeBrandStoreList.OrderBy(o => o.CreatedOn).ToList();
                foreach (var batch in SortedMaizeBrandStoreList)
                {
                    //var batch = _batchService.GetBatch(cashSaleBatch.BatchId);
                    if (batch.Quantity > 0)
                    {

                        makeDelivery = ReduceBatchBrandStock(batch, Convert.ToDouble(cashSale.Quantity), userId);
                        makeDelivery = new MakeDelivery();
                        if (makeDelivery.StockReduced)
                        {
                            soldOutAmount = Convert.ToDouble(batch.Quantity) - batchBrandBalance;
                            var latestMaizeBrandStore = _maizeBrandStoreService.GetLatestMaizeBrandStoreForAParticularBranch(cashSale.BranchId);
                            var newBalance = latestMaizeBrandStore.Balance - soldOutAmount;
                            var maizeBrandStoreDTO = new DTO.MaizeBrandStoreDTO()
                            {

                                Quantity = latestMaizeBrandStore.Quantity,
                                StartQuantity = latestMaizeBrandStore.StartQuantity,
                                Balance = newBalance,
                                BatchId = latestMaizeBrandStore.BatchId,
                                MaizeBrandStoreId = latestMaizeBrandStore.MaizeBrandStoreId,
                                Action = latestMaizeBrandStore.Action,
                                BranchId = latestMaizeBrandStore.BranchId,
                                StoreId = latestMaizeBrandStore.StoreId,

                                Deleted = latestMaizeBrandStore.Deleted,
                                CreatedBy = latestMaizeBrandStore.CreatedBy,
                                CreatedOn = latestMaizeBrandStore.CreatedOn

                            };

                          var  maizeBrandStoreId = _maizeBrandStoreDataService.SaveMaizeBrandStore(maizeBrandStoreDTO, userId);
         
              
                            CashSaleBatch batchDetails = new CashSaleBatch()
                            {
                                BatchId = batch.BatchId,
                                //BatchNumber = batch.Name,
                                Price = cashSale.Price,
                                BatchQuantity = soldOutAmount
                            };

                            batchCashSaleList.Add(batchDetails);
                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = 0,
                                CashSaleBatchList = batchCashSaleList,
                            };
                            cashSaleBalance = makeDelivery.OrderQuantityBalance;

                            return makeDelivery;
                        }

                        else  if (!makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance ==0)
                        {
                            soldOutAmount = Convert.ToDouble(batch.Quantity) - batchBrandBalance;

                            CashSaleBatch batchDetails = new CashSaleBatch()
                            {
                                BatchId = batch.BatchId,
                                //BatchNumber = batch.Name,
                                Price = cashSale.Price,
                                BatchQuantity = soldOutAmount
                            };

                            batchCashSaleList.Add(batchDetails);
                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = 0,
                                CashSaleBatchList = batchCashSaleList,
                            };
                            cashSaleBalance = makeDelivery.OrderQuantityBalance;

                            return makeDelivery;
                        }


                    }

                }
            }
            return makeDelivery;
        }
        #endregion
        #endregion

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
       
        public long SaveCashSale(CashSale cashSale, string userId)
        {
            double totalQuantity = Convert.ToDouble(cashSale.Quantity);
            long cashSaleId = 0;
            MakeDelivery makeDelivery = new MakeDelivery();

           
                #region cashsale brand
                if (cashSale.ProductId == 2)
                {

                    var cashSaleDTO = new DTO.CashSaleDTO()
                    {
                        StoreId = cashSale.StoreId,
                        BranchId = cashSale.BranchId,
                        SectorId = cashSale.SectorId,
                        PaymentModeId = cashSale.PaymentModeId,
                        Price = cashSale.Price,
                        Quantity = cashSale.Quantity,
                        ProductId = cashSale.ProductId,
                        Amount = cashSale.Amount,
                         TransactionSubTypeId = cashSale.TransactionSubTypeId,
                         CashSaleId = cashSale.CashSaleId,
                        Deleted = cashSale.Deleted,
                        CreatedBy = cashSale.CreatedBy,
                        CreatedOn = cashSale.CreatedOn,


                    };
                   
                        cashSaleId = this._dataService.SaveCashSale(cashSaleDTO, userId);
                       this._maizeBrandStoreService.UpdateBrandStore(cashSale.BranchId, "-",userId, Convert.ToDouble(cashSale.Quantity));
                       
                        foreach (var cashSaleBatch in cashSale.Batches)
                        {
                            var batchCashSale = new CashSaleBatchDTO()
                            {
                                BatchId = cashSaleBatch.BatchId,
                                BatchQuantity = cashSaleBatch.QuantityToRemove,
                                Price = cashSale.Price,
                                ProductId = cashSale.ProductId,
                                Amount = (cashSale.Price * cashSaleBatch.QuantityToRemove),
                                CashSaleId = cashSaleId,
                            };
                            this.SaveCashSaleBatch(batchCashSale);
                        }
                        #region generate documents
                        if (cashSale.PaymentModeId == 2 )
                        {
                            //generate cash receipt
                            // throw new NotImplementedException();
                            var username = string.Empty;
                            var user = _userService.GetAspNetUser(cashSale.CreatedBy);
                            if (user != null)
                            {
                                username = user.FirstName + " " + user.LastName;
                            }
                            var document = new Document()
                            {
                                DocumentId = 0,

                                UserId = username,
                                DocumentCategoryId = cashReceiptId,
                                Amount =Convert.ToDouble(cashSale.Amount),
                                BranchId = cashSale.BranchId,
                                ItemId = cashSaleId,
                                Description = "Cash Sale of maize brand of " + totalQuantity.ToString() + " kgs" + " at" + cashSale.Price + " Per Kilogram",
                              

                            };

                            var documentId = _documentService.SaveDocument(document, userId);
                        }
                      
                        #endregion



                                  
                   
                }
            #endregion

            #region deliver rice
                else if(cashSale.ProductId == 10003)
            {
               
                    if (cashSale.SelectedGrades.Any())
                    {
                        var gradeStore = CheckForFlourInStore(cashSale.SelectedGrades, cashSale.StoreId);
                        if (gradeStore == -2)
                        {
                            cashSaleId = gradeStore;
                            return cashSaleId;
                        }
                        else if (gradeStore == -1)
                        {
                            cashSaleId = gradeStore;
                            return cashSaleId;
                        }
                        else
                        {
                            var cashSaleDTO = new DTO.CashSaleDTO()
                            {
                                StoreId = cashSale.StoreId,
                                BranchId = cashSale.BranchId,
                                SectorId = cashSale.SectorId,
                                PaymentModeId = cashSale.PaymentModeId,
                                Price = cashSale.Price,
                                Quantity = cashSale.Quantity,
                                ProductId = cashSale.ProductId,
                                Amount = cashSale.Amount,
                                TransactionSubTypeId = cashSale.TransactionSubTypeId,
                                CashSaleId = cashSale.CashSaleId,
                                Deleted = cashSale.Deleted,
                                CreatedBy = cashSale.CreatedBy,
                                CreatedOn = cashSale.CreatedOn,
                                //DocumentCategoryId = cashReceiptId,



                            };

                            cashSaleId = this._dataService.SaveCashSale(cashSaleDTO, userId);

                            List<CashSaleGradeSize> cashSaleGradeSizeList = new List<CashSaleGradeSize>();
                            if (cashSale.SelectedGrades != null)
                            {
                                bool inOrOut = false;
                                if (cashSale.SelectedGrades.Any())
                                {
                                    foreach (var grade in cashSale.SelectedGrades)
                                    {
                                        long sizeId = 0;
                                        double amount = 0, price = 0, quantity = 0;

                                        foreach (var denomination in grade.Denominations)
                                        {
                                            sizeId = denomination.DenominationId;
                                            price = denomination.Price;
                                            quantity = denomination.Quantity;
                                            amount = (denomination.Quantity * denomination.Price);

                                            var cashSaleGradeSize = new CashSaleGradeSize()
                                            {
                                                CashSaleId = cashSaleId,
                                                GradeId = grade.GradeId,
                                                SizeId = sizeId,
                                                Quantity = quantity,
                                                Price = price,
                                                Amount = amount,
                                                TimeStamp = DateTime.Now,

                                            };
                                            cashSaleGradeSizeList.Add(cashSaleGradeSize);

                                            //Method that removes flour output into storeGradeSize table(store flour stock)
                                            var storeGradeSize = new StoreGradeSizeDTO()
                                            {
                                                StoreId = cashSale.StoreId,
                                                GradeId = cashSaleGradeSize.GradeId,
                                                SizeId = cashSaleGradeSize.SizeId,
                                                Quantity = cashSaleGradeSize.Quantity,
                                            };

                                            _stockDataService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                        }


                                    }
                                    this._dataService.PurgeCashSaleGradeSize(cashSaleId);
                                    this.SaveCashSaleGradeSizeList(cashSaleGradeSizeList);


                                }
                                #region generate documents
                                if (cashSale.PaymentModeId == 2)
                                {
                                    //generate receipt
                                    //throw new NotImplementedException();
                                    var username = string.Empty;
                                    var user = _userService.GetAspNetUser(userId);
                                    if (user != null)
                                    {
                                        username = user.FirstName + " " + user.LastName;
                                    }
                                    var document = new Document()
                                    {
                                        DocumentId = 0,

                                        UserId = username,
                                        DocumentCategoryId = cashReceiptId,
                                        Amount = Convert.ToDouble(cashSale.Amount),
                                        BranchId = cashSale.BranchId,
                                        ItemId = cashSaleId,
                                        Description = "Sale of Rice of " + cashSale.Quantity.ToString() + " kgs",

                                        Grades = cashSale.SelectedGrades,


                                    };

                                    var documentId = _documentService.SaveDocument(document, userId);
                                }

                                #endregion
                            }
                        }


                    }
              
            }
            #endregion
            #region deliver flour
            else
                {

                if (cashSale.Batches == null && cashSale.BranchId == 20006)
                {
                    if (cashSale.SelectedGrades.Any())
                    {
                        var gradeStore = CheckForFlourInStore(cashSale.SelectedGrades, cashSale.StoreId);
                        if (gradeStore == -2)
                        {
                            cashSaleId = gradeStore;
                            return cashSaleId;
                        }
                        else if (gradeStore == -1)
                        {
                            cashSaleId = gradeStore;
                            return cashSaleId;
                        }
                        else
                        {
                            var cashSaleDTO = new DTO.CashSaleDTO()
                            {
                                StoreId = cashSale.StoreId,
                                BranchId = cashSale.BranchId,
                                SectorId = cashSale.SectorId,
                                PaymentModeId = cashSale.PaymentModeId,
                                Price = cashSale.Price,
                                Quantity = cashSale.Quantity,
                                ProductId = cashSale.ProductId,
                                Amount = cashSale.Amount,
                                TransactionSubTypeId = cashSale.TransactionSubTypeId,
                                CashSaleId = cashSale.CashSaleId,
                                Deleted = cashSale.Deleted,
                                CreatedBy = cashSale.CreatedBy,
                                CreatedOn = cashSale.CreatedOn,
                                //DocumentCategoryId = cashReceiptId,



                            };
                           
                            cashSaleId = this._dataService.SaveCashSale(cashSaleDTO, userId);
                          
                            List<CashSaleGradeSize> cashSaleGradeSizeList = new List<CashSaleGradeSize>();
                            if (cashSale.SelectedGrades != null)
                            {
                                bool inOrOut = false;
                                if (cashSale.SelectedGrades.Any())
                                {
                                    foreach (var grade in cashSale.SelectedGrades)
                                    {
                                        long sizeId = 0;
                                        double amount = 0, price = 0, quantity = 0;

                                        foreach (var denomination in grade.Denominations)
                                        {
                                            sizeId = denomination.DenominationId;
                                            price = denomination.Price;
                                            quantity = denomination.Quantity;
                                            amount = (denomination.Quantity * denomination.Price);

                                            var cashSaleGradeSize = new CashSaleGradeSize()
                                            {
                                                CashSaleId = cashSaleId,
                                                GradeId = grade.GradeId,
                                                SizeId = sizeId,
                                                Quantity = quantity,
                                                Price = price,
                                                Amount = amount,
                                                TimeStamp = DateTime.Now,

                                            };
                                            cashSaleGradeSizeList.Add(cashSaleGradeSize);

                                            //Method that removes flour output into storeGradeSize table(store flour stock)
                                            var storeGradeSize = new StoreGradeSizeDTO()
                                            {
                                                StoreId = cashSale.StoreId,
                                                GradeId = cashSaleGradeSize.GradeId,
                                                SizeId = cashSaleGradeSize.SizeId,
                                                Quantity = cashSaleGradeSize.Quantity,
                                            };

                                            _stockDataService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                        }


                                    }
                                    this._dataService.PurgeCashSaleGradeSize(cashSaleId);
                                    this.SaveCashSaleGradeSizeList(cashSaleGradeSizeList);


                                }
                                #region generate documents
                                if (cashSale.PaymentModeId == 2)
                                {
                                    //generate receipt
                                    //throw new NotImplementedException();
                                    var username = string.Empty;
                                    var user = _userService.GetAspNetUser(userId);
                                    if (user != null)
                                    {
                                        username = user.FirstName + " " + user.LastName;
                                    }
                                    var document = new Document()
                                    {
                                        DocumentId = 0,

                                        UserId = username,
                                        DocumentCategoryId = cashReceiptId,
                                        Amount = Convert.ToDouble(cashSale.Amount),
                                        BranchId = cashSale.BranchId,
                                        ItemId = cashSaleId,
                                        Description = "Sale of maize flour of " + cashSale.Quantity.ToString() + " kgs",

                                        Grades = cashSale.SelectedGrades,


                                    };

                                    var documentId = _documentService.SaveDocument(document, userId);
                                }

                                #endregion
                            }
                        }


                    }
                }
                else
                {
                    if (cashSale.Batches == null && cashSale.BranchId != 20006)
                    {
                        //no batch selected
                         cashSaleId = -33;
                        return cashSaleId;
                    }
                    else if (cashSale.SelectedGrades.Any())
                    {
                        var gradeStore = CheckForFlourInStore(cashSale.SelectedGrades, cashSale.StoreId);
                        if (gradeStore == -2)
                        {
                            cashSaleId = gradeStore;
                            return cashSaleId;
                        }
                        else if (gradeStore == -1)
                        {
                            cashSaleId = gradeStore;
                            return cashSaleId;
                        }
                        else
                        {
                            var cashSaleDTO = new DTO.CashSaleDTO()
                            {
                                StoreId = cashSale.StoreId,
                                BranchId = cashSale.BranchId,
                                SectorId = cashSale.SectorId,
                                PaymentModeId = cashSale.PaymentModeId,
                                Price = cashSale.Price,
                                Quantity = cashSale.Quantity,
                                ProductId = cashSale.ProductId,
                                Amount = cashSale.Amount,
                                TransactionSubTypeId = cashSale.TransactionSubTypeId,
                                CashSaleId = cashSale.CashSaleId,
                                Deleted = cashSale.Deleted,
                                CreatedBy = cashSale.CreatedBy,
                                CreatedOn = cashSale.CreatedOn,
                                //DocumentCategoryId = cashReceiptId,



                            };

                            cashSaleId = this._dataService.SaveCashSale(cashSaleDTO, userId);
                            makeDelivery = MakeBatchCashSaleGradeSize(cashSaleId, cashSale);
                            if (makeDelivery.StockReduced)
                            {
                                List<CashSaleGradeSize> cashSaleGradeSizeList = new List<CashSaleGradeSize>();
                                if (cashSale.SelectedGrades != null)
                                {
                                    bool inOrOut = false;
                                    if (cashSale.SelectedGrades.Any())
                                    {
                                        foreach (var grade in cashSale.SelectedGrades)
                                        {
                                            long sizeId = 0;
                                            double amount = 0, price = 0, quantity = 0;

                                            foreach (var denomination in grade.Denominations)
                                            {
                                                sizeId = denomination.DenominationId;
                                                price = denomination.Price;
                                                quantity = denomination.Quantity;
                                                amount = (denomination.Quantity * denomination.Price);

                                                var cashSaleGradeSize = new CashSaleGradeSize()
                                                {
                                                    CashSaleId = cashSaleId,
                                                    GradeId = grade.GradeId,
                                                    SizeId = sizeId,
                                                    Quantity = quantity,
                                                    Price = price,
                                                    Amount = amount,
                                                    TimeStamp = DateTime.Now,

                                                };
                                                cashSaleGradeSizeList.Add(cashSaleGradeSize);

                                                //Method that removes flour output into storeGradeSize table(store flour stock)
                                                var storeGradeSize = new StoreGradeSizeDTO()
                                                {
                                                    StoreId = cashSale.StoreId,
                                                    GradeId = cashSaleGradeSize.GradeId,
                                                    SizeId = cashSaleGradeSize.SizeId,
                                                    Quantity = cashSaleGradeSize.Quantity,
                                                };

                                                _stockDataService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                            }


                                        }
                                        this._dataService.PurgeCashSaleGradeSize(cashSaleId);
                                        this.SaveCashSaleGradeSizeList(cashSaleGradeSizeList);


                                    }
                                    #region generate documents
                                    if (cashSale.PaymentModeId == 2)
                                    {
                                        //generate receipt
                                        //throw new NotImplementedException();
                                        var username = string.Empty;
                                        var user = _userService.GetAspNetUser(userId);
                                        if (user != null)
                                        {
                                            username = user.FirstName + " " + user.LastName;
                                        }
                                        var document = new Document()
                                        {
                                            DocumentId = 0,

                                            UserId = username,
                                            DocumentCategoryId = cashReceiptId,
                                            Amount = Convert.ToDouble(cashSale.Amount),
                                            BranchId = cashSale.BranchId,
                                            ItemId = cashSaleId,
                                            Description = "Sale of maize flour of " + cashSale.Quantity.ToString() + " kgs",

                                            Grades = cashSale.SelectedGrades,


                                        };

                                        var documentId = _documentService.SaveDocument(document, userId);
                                    }

                                    #endregion
                                }
                            }
                            else
                            {

                            }
                        }


                    }
                }
                }       
                #endregion

               

           

                #region transactions

                long transactionSubTypeId = 0;
                var notes = string.Empty;
                if (cashSale.ProductId == 1)
                {
                    transactionSubTypeId = flourTransactionSubTypeId;
                    notes = "Maize Flour Sale of " + cashSale.Quantity.ToString() + " kgs";
                }
            else if (cashSale.ProductId == 10003)
            {
                transactionSubTypeId = riceTransactionSubTypeId;
                notes = "Rice Sale of " + cashSale.Quantity.ToString() + " kgs";
            }
            else if (cashSale.ProductId == 2)
                {
                    transactionSubTypeId = branTransactionSubTypeId;
                    notes = "Brand Sale of " + totalQuantity + " kgs at " + cashSale.Price + " Per Kilogram";
                }
                var paymentMode = _accountTransactionActivityService.GetPaymentMode(cashSale.PaymentModeId);
                var paymentModeName = paymentMode.Name;
               
                if (paymentModeName == "Cash")
                {

                    var cash = new Cash()
                    {

                        Amount = Convert.ToDouble(cashSale.Amount),
                        Notes = notes,
                        Action = "+",
                        BranchId = Convert.ToInt64(cashSale.BranchId),
                        TransactionSubTypeId = transactionSubTypeId,
                        SectorId = cashSale.SectorId,

                    };
                    _cashService.SaveCash(cash, userId);


                }
                

           
                #endregion




            return cashSaleId;
        }

     
        public void MarkAsDeleted(long cashSaleId, string userId)
        {
            _dataService.MarkAsDeleted(cashSaleId, userId);
        }


        public long Cancelled(CashSale cashSale, string userId)
        {
            bool inOrOut = false;
            if (cashSale.Grades != null)
            {
                if (cashSale.Grades.Any())
                {
                    //List<CashSaleGradeSize> cashSaleGradeSizeList = new List<CashSaleGradeSize>();

                    foreach (var grade in cashSale.Grades)
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
                                    var storeFlourTransferGradeSize = new StoreGradeSizeDTO()
                                    {
                                        StoreId = cashSale.StoreId,
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        Quantity = denomination.Quantity,
                                    };

                                    //storeFlourTransfer = this._dataService.SaveStoreGradeSize(storeFlourTransferGradeSize, inOrOut);
                                    _stockDataService.SaveStoreGradeSize(storeFlourTransferGradeSize, inOrOut);

                                }
                            }
                        }
                    }

                }
            }
            _dataService.PurgeCashSaleGradeSize(cashSale.CashSaleId);
            if(cashSale.ProductId == 2)
            {
                this._maizeBrandStoreService.UpdateBrandStore(cashSale.BranchId, "+", userId, Convert.ToDouble(cashSale.Quantity));

            }
            var cashSaleDTO = new CashSaleDTO()
            {
                CashSaleId = cashSale.CashSaleId,
                StoreId = cashSale.StoreId,
                Price = cashSale.Price,
                Quantity = cashSale.Quantity,
                PaymentModeId = cashSale.PaymentModeId,
                BranchId = cashSale.BranchId,
                Amount = cashSale.Amount,
                ProductId = cashSale.ProductId,
                TransactionSubTypeId = cashSale.TransactionSubTypeId,
                SectorId = cashSale.SectorId,
                CreatedOn = cashSale.CreatedOn,
                TimeStamp = DateTime.Now,
                CreatedBy = cashSale.CreatedBy,
                Deleted = cashSale.Deleted,
                Cancelled = cashSale.Cancelled,
                ReceiptLimit = cashSale.ReceiptLimit,
            };
        var cashSaleId =   _dataService.Cancelled(cashSaleDTO, userId);
        var document = _documentService.GetDocument(cashSale.DocumentId);
        if (document != null)
        {
            var documentId = _documentService.Cancelled(document, userId);
        }
     
        #region transactions

        long transactionSubTypeId = 0;
        var notes = string.Empty;
        if (cashSale.ProductId == 1)
        {
            transactionSubTypeId = flourTransactionSubTypeId;
            notes = "Cancelled Maize Flour Sale of " + cashSale.Quantity.ToString() + " kgs";
        }
        else if (cashSale.ProductId == 10003)
        {
                transactionSubTypeId = riceTransactionSubTypeId;
                notes = "Cancelled Rice Sale of " + cashSale.Quantity.ToString() + " kgs";
            }
            else if (cashSale.ProductId == 2)
        {
            transactionSubTypeId = branTransactionSubTypeId;
            notes = "Cancelled Brand Sale of " + cashSale.Quantity.ToString() + " kgs at " + cashSale.Price + " Per Kilogram";
        }
       

            var cash = new Cash()
            {

                Amount = Convert.ToDouble(cashSale.Amount),
                Notes = notes,
                Action = "-",
                BranchId = Convert.ToInt64(cashSale.BranchId),
                TransactionSubTypeId = transactionSubTypeId,
                SectorId = cashSale.SectorId,

            };
            _cashService.SaveCash(cash, userId);


        



        #endregion


        return cashSaleId;
        }

        public void SaveCashSaleBatchList(List<CashSaleBatch> cashSaleBatchList)
        {
            if (cashSaleBatchList != null)
            {
                if (cashSaleBatchList.Any())
                {
                    foreach (var cashSaleBatch in cashSaleBatchList)
                    {
                        var cashSaleBatchDTO = new DTO.CashSaleBatchDTO()
                        {
                            CashSaleId = cashSaleBatch.CashSaleId,
                            BatchQuantity = cashSaleBatch.BatchQuantity,
                            BatchId = cashSaleBatch.BatchId,
                            Price = cashSaleBatch.Price,
                            CreatedOn = cashSaleBatch.CreatedOn,
                            TimeStamp = cashSaleBatch.TimeStamp
                        };
                        this.SaveCashSaleBatch(cashSaleBatchDTO);
                    }
                }
            }
        }

        public void SaveCashSaleBatch(CashSaleBatchDTO cashSaleBatchDTO)
        {
            _dataService.SaveCashSaleBatch(cashSaleBatchDTO);
        }

        public void SaveCashSaleGradeSizeList(List<CashSaleGradeSize> cashSaleGradeSizeList)
        {
            if (cashSaleGradeSizeList != null)
            {
                if (cashSaleGradeSizeList.Any())
                {
                    foreach (var cashSaleGradeSize in cashSaleGradeSizeList)
                    {
                        var cashSaleGradeSizeDTO = new DTO.CashSaleGradeSizeDTO()
                        {
                            CashSaleId = cashSaleGradeSize.CashSaleId,
                            GradeId = cashSaleGradeSize.GradeId,
                            Quantity = cashSaleGradeSize.Quantity,
                            SizeId = cashSaleGradeSize.SizeId,
                            Price = cashSaleGradeSize.Price,
                            Amount = cashSaleGradeSize.Amount,
                            TimeStamp = cashSaleGradeSize.TimeStamp
                        };
                        this.SaveCashSaleGradeSize(cashSaleGradeSizeDTO);
                    }
                }
            }
        }
       
        public void SaveCashSaleGradeSize(CashSaleGradeSizeDTO cashSaleGradeSizeDTO)
        {
            _dataService.SaveCashSaleGradeSize(cashSaleGradeSizeDTO);
        }

        private IEnumerable<CashSaleBatch> GetAllBatchesForACashSale(long cashSaleId)
        {
            var results = _dataService.GetAllBatchesForACashSale(cashSaleId);
            return MapEFToModel(results);
        }

      
        #region Mapping Methods

        
        public IEnumerable<CashSaleViewModel> MapEFCashSaleToModelCashSaleViewModel(IEnumerable<EF.Models.CashSale> data)
        {
            var list = new List<CashSaleViewModel>();
            foreach (var result in data)
            {
                list.Add(MapEFCashSaleToModelCashSaleViewModel(result));
            }
            return list;
        }

        public CashSaleViewModel MapEFCashSaleToModelCashSaleViewModel(EF.Models.CashSale data)
        {
            var cashSale = new CashSaleViewModel();
            long documentId = 0;
            long cashSaleDocumentCategoryId = 0;
            cashSaleDocumentCategoryId = GetCashSaleDocumentCategoryId();

            if (data != null)
            {
                var document = _documentService.GetDocumentForAParticularItemAndCategory(data.CashSaleId, cashSaleDocumentCategoryId);
                if (document != null)
                {
                    documentId = document.DocumentId;
                    
                }


                cashSale = new CashSaleViewModel()
                {
                   
                    CashSaleId = data.CashSaleId,
                    Quantity = data.Quantity,
                  
                    CreatedOn = Convert.ToDateTime(data.CreatedOn),
                   
                    ProductName = data.Product != null ? data.Product.Name : "",
                    Amount = data.Amount,
                    Price = data.Price,
                    DocumentId = documentId,
                   
                };
               
                return cashSale;
            }
            return cashSale;
        }


        public IEnumerable<CashSale> MapEFToModel(IEnumerable<EF.Models.CashSale> data)
        {
            var list = new List<CashSale>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        private long GetCashSaleDocumentCategoryId()
        {
            long cashSaleDocumentCategoryId = 0;
             var documentCategories = _documentService.GetAllDocumentCategories();
            if (documentCategories.Any())
            {
                foreach (var documentCategory in documentCategories)
                {
                    if (documentCategory.Name == "CashReceipt")
                    {
                        cashSaleDocumentCategoryId = documentCategory.DocumentCategoryId;
                        return cashSaleDocumentCategoryId;
                    }
                }
            }

            return cashSaleDocumentCategoryId;
          
        }
       
        public CashSale MapEFToModel(EF.Models.CashSale data)
        {
            var cashSale = new CashSale();
            long documentId = 0;
            long documentNumber = 0;
            long cashSaleDocumentCategoryId = 0;
            cashSaleDocumentCategoryId = GetCashSaleDocumentCategoryId();
            if (data != null)
            {
                var document = _documentService.GetDocumentForAParticularItemAndCategory(data.CashSaleId,cashSaleDocumentCategoryId);
                if (document != null)
                {
                    documentId = document.DocumentId;
                    documentNumber = document.DocumentNumber;
                }
             

                cashSale = new CashSale()
                {
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    TransactionSubTypeId = data.TransactionSubTypeId,
                    TransactionSubTypeName = data.TransactionSubType != null ? data.TransactionSubType.Name : "",
                   CashSaleId = data.CashSaleId,
                   Quantity = data.Quantity,
                    BranchId = data.BranchId,
                  DocumentNumber = documentNumber,
                    PaymentModeId = data.PaymentModeId,
                    PaymentModeName = data.PaymentMode != null ? data.PaymentMode.Name : "",
                    SectorId = data.SectorId,
                    StoreId = data.StoreId,
                    StoreName = data.Store != null ? data.Store.Name : "",
                   ProductId = data.ProductId,
                    CreatedOn = Convert.ToDateTime(data.CreatedOn),
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    Cancelled = data.Cancelled,
                    ReceiptLimit = data.ReceiptLimit == null? 0: data.ReceiptLimit,
                    ProductName = data.Product != null ? data.Product.Name : "",
                    Amount = data.Amount,
                    Price = data.Price,
                    DocumentId = documentId,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };
                var batches = GetAllBatchesForACashSale(data.CashSaleId);
                List<CashSaleBatch> cashSaleBatchList = new List<CashSaleBatch>();
                if (batches.Any())
                {
                    foreach (var batch in batches)
                    {
                        var cashSalebatch = new CashSaleBatch()
                        {
                            BatchId = batch.BatchId,
                            BatchNumber = batch.BatchNumber,
                            CashSaleId = batch.CashSaleId,
                            BatchQuantity = batch.BatchQuantity,

                        };
                        cashSaleBatchList.Add(cashSalebatch);

                    }
                    cashSale.CashSaleBatches = cashSaleBatchList;
                }
                if (data.CashSaleGradeSizes != null)
                {
                    if (data.CashSaleGradeSizes.Any())
                    {
                        List<Grade> grades = new List<Grade>();
                        //var distinctGrades = data.CashSaleGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        var distinctGrades = data.CashSaleGradeSizes.Where(a => a.CashSaleId == data.CashSaleId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        foreach (var cashSaleGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = cashSaleGradeSize.Grade.GradeId,
                                Value = cashSaleGradeSize.Grade.Value,
                                CreatedOn = cashSaleGradeSize.Grade.CreatedOn,
                                TimeStamp = cashSaleGradeSize.Grade.TimeStamp,
                                Deleted = cashSaleGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(cashSaleGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(cashSaleGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (cashSaleGradeSize.Grade.CashSaleGradeSizes != null)
                            {
                                if (cashSaleGradeSize.Grade.CashSaleGradeSizes.Any())
                                {
                                    var distinctSizes = cashSaleGradeSize.Grade.CashSaleGradeSizes.Where(a => a.CashSaleId == data.CashSaleId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                   
                                    // var distinctSizes = cashSaleGradeSize.Grade.CashSaleGradeSizes.GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity,
                                            Price = ogs.Price,
                                            Amount = ogs.Amount,
                                        };
                                        //delivery.Quantity += (ogs.Quantity * ogs.Size.Value);
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                        }
                        cashSale.Grades = grades;
                    }
                }
                return cashSale;
            }
            return cashSale;
        }


        public CashSaleBatch MapEFToModel(EF.Models.CashSaleBatch data)
        {

            var cashSaleBatch = new CashSaleBatch()
            {

                BatchId = data.BatchId,
                CashSaleId = data.CashSaleId,
                BatchQuantity = data.BatchQuantity,
                CreatedOn = data.CreatedOn,
                BatchNumber = data.Batch != null ? data.Batch.Name : "",
                TimeStamp = data.TimeStamp,
                ProductId = data.ProductId,
                Price = data.Price,
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
        #endregion
    }
    
}
