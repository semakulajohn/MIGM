using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;
using System.Configuration;
using Higgs.Mbale.Models.WebViewModel;




namespace Higgs.Mbale.BAL.Concrete
{
    public class DeliveryService: IDeliveryService
    {
        private long riceTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["RiceSaleTransactionSubTypeId"]);
        private long orderStatusIdComplete = Convert.ToInt64(ConfigurationManager.AppSettings["StatusIdComplete"]);
        private long orderStatusIdInProgress = Convert.ToInt64(ConfigurationManager.AppSettings["StatusIdInProgress"]);
        private long flourTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["FlourSaleTransactionSubTypeId"]);
        private long branTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["BranSaleTransactionSubTypeId"]);
        private long invoiceId = Convert.ToInt64(ConfigurationManager.AppSettings["Invoice"]);
        private long receiptId = Convert.ToInt64(ConfigurationManager.AppSettings["Receipt"]);
        private double orderBalance = 0,batchBrandBalance=0,soldOutAmount =0;
        private long documentId = 0;
        List<BatchDeliveryDetails> batchDeliveryList = new List<BatchDeliveryDetails>();
        ILog logger = log4net.LogManager.GetLogger(typeof(DeliveryService));
        private IDeliveryDataService _dataService;
        private IUserService _userService;
        private ITransactionDataService _transactionDataService;
        private ITransactionSubTypeService _transactionSubTypeService;
        private IOrderService _orderService;
        private IStockService _stockService;
        private IStockDataService _stockDataService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        private ICashService _cashService;
        private IBatchService _batchService;
        private IDocumentService _documentService;
        private IMaizeBrandStoreService _maizeBrandStoreService;
        private IWeightLossService _weightLossService;


        public DeliveryService(IDeliveryDataService dataService, IUserService userService, ITransactionDataService transactionDataService,
            ITransactionSubTypeService transactionSubTypeService, IDocumentService documentService,
            IOrderService orderService, IStockService stockService, IStockDataService stockDataService,
            IAccountTransactionActivityService accountTransactionActivityService, ICashService cashService,
            IWeightLossService weightLossService,
            IBatchService batchService,IMaizeBrandStoreService maizeBrandStoreService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionDataService = transactionDataService;
            this._transactionSubTypeService = transactionSubTypeService;
            this._documentService = documentService;
            this._orderService = orderService;
            this._stockService = stockService;
            this._stockDataService = stockDataService;
            this._accountTransactionActivityService = accountTransactionActivityService;
            this._cashService = cashService;
            this._batchService = batchService;
            this._maizeBrandStoreService = maizeBrandStoreService;
            this._weightLossService = weightLossService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        public Delivery GetDelivery(long deliveryId)
        {
            var result = this._dataService.GetDelivery(deliveryId);
            return MapEFToModel(result);
        }

        public IEnumerable<Delivery> GetAllDeliveriesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllDeliveriesForAParticularBranch(branchId);
            return MapEFToModel(results);

        }
        public IEnumerable<Delivery> GetAllDeliveriesForAParticularOrder(long orderId)
        {
            var results = this._dataService.GetAllDeliveriesForAParticularOrder(orderId);
            return MapEFToModel(results);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Delivery> GetAllDeliveries()
        {
            var results = this._dataService.GetAllDeliveries();
            return MapEFToModel(results);
        }

        public IEnumerable<Delivery> GetAllBranchUnApprovedDeliveries(long branchId)
        {
            var results = this._dataService.GetAllBranchUnApprovedDeliveries(branchId);
            return MapEFToModel(results);
        }
        public IEnumerable<Delivery> GetAllBranchRejectedDeliveries(long branchId)
        {
            var results = this._dataService.GetAllBranchRejectedDeliveries(branchId);
            return MapEFToModel(results);
        }

        public IEnumerable<Delivery> GetAllBranchApprovedDeliveries(long branchId)
        {
            var results = this._dataService.GetAllBranchApprovedDeliveries(branchId);
            return MapEFToModel(results);
        }


        #region makeDelivery



        #region flour 
        private bool CheckIfStockHasOrderGrade(long orderGrade, List<long> stockGrades)
        {
            bool hasGrade = false;
           
            foreach (var stockGrade in stockGrades)
	            {
                    if (orderGrade == stockGrade)
                    {
                        hasGrade = true;
                        return hasGrade;
                    }
	            }
               
            
            return hasGrade;
        }

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
        private long UpdateStoreStockDetailsOnTransfer(StoreStock storeStock)
        {
            var storeStockId = _stockService.SaveStoreStockFlourTransfer(storeStock);
            return storeStockId;

        }
       

        private MakeDelivery ReduceBatchFlourStock(Delivery delivery,List<BatchOutPut> batchOutPuts, string userId)
        {
            BatchOutPut batchOutPut = new BatchOutPut();
            if(batchOutPuts.Any())
            {
                batchOutPut = batchOutPuts.First();
            }
            
          
           bool orderHasBalance = false;
           Grade foundGrade = new Grade();
           var makeDelivery = new MakeDelivery();
           #region NewCode
             if(batchOutPut.Grades.Any())
                {   
                   
                    bool hasGrade = false;
                  List<OrderGradeSize> orderGradeSizes = new List<OrderGradeSize>();
                  List<BatchGradeSize> batchGradeSizes = new List<BatchGradeSize>();
                var order = _orderService.GetOrder(delivery.OrderId);
#region foreach
                     foreach (var orderGrade in order.Grades)
	                         {
                                 hasGrade = CheckIfBatchHasOrderGrade(orderGrade.GradeId, batchOutPut.Grades);
                                     if(hasGrade)
                                     {
                                  foundGrade =  GetGradeSameAsOrderGrade(batchOutPut.Grades,  orderGrade.GradeId);
                                  int j = 0;
                                  foreach (var foundGradeDenom in foundGrade.Denominations)
                                  {
                                      //int j = 0;
                                      //int count = orderGrade.Denominations.Count();
                                      double quantityToRemove = 0;
                                      bool flag = false;
                                      quantityToRemove = foundGradeDenom.QuantityToRemove;
                                      double balanceValue = 0,quantity = 0;

                                      var batchGradeSizeBalance = foundGradeDenom.Balance - foundGradeDenom.QuantityToRemove;
                                      var batchGradeSize = new BatchGradeSize()
                                              {
                                                  BatchOutPutId = batchOutPut.BatchOutPutId,
                                                  GradeId = foundGrade.GradeId,
                                                  SizeId = foundGradeDenom.DenominationId,
                                                  Quantity = foundGradeDenom.Quantity,
                                                  Balance = batchGradeSizeBalance,
                                              };

                                      batchGradeSizes.Add(batchGradeSize);
#region forloop
                                      for (int i = j; i < orderGrade.Denominations.Count(); i++)
			                                {
                                                j = j + 1;
                                                balanceValue = orderGrade.Denominations[i].Balance;
                                                quantity = orderGrade.Denominations[i].Quantity;
			                      
                                      var orderGradeSizeBalance =balanceValue - quantityToRemove;
                                          var orderGradeSize = new OrderGradeSize()
                                            {
                                                OrderId = delivery.OrderId,
                                                GradeId = orderGrade.GradeId,
                                                SizeId = foundGradeDenom.DenominationId,
                                                Quantity = quantity,
                                                Balance = orderGradeSizeBalance,
                                            };

                                          orderGradeSizes.Add(orderGradeSize);

                                          var inOrOut = false;
                                          //Method that removes flour from storeGradeSize table
                                          var storeGradeSize = new StoreGradeSize()
                                          {
                                              StoreId = delivery.StoreId,
                                              GradeId = orderGrade.GradeId,
                                              SizeId = foundGradeDenom.DenominationId,
                                              Quantity = quantityToRemove,
                                          };

                                          this._stockService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                      //}
                                          if (i < 6)
                                          {
                                              flag = true;
                                              break;
                                          }
                                            }
                                      if (flag)
                                      {
                                          continue;
                                      }
                                     }
#endregion
                                 orderHasBalance =  FindIfFlourOrderHasBalance(orderGradeSizes);
                                 _orderService.PurgeOrderGradeSize(delivery.OrderId);
                                 
                                 foreach (var item in orderGradeSizes)
                                 {
                                     var orderGradeSize = new OrderGradeSize()
                                     {
                                         OrderId = item.OrderId,
                                         GradeId = item.GradeId,
                                         SizeId = item.SizeId,
                                         Quantity = item.Quantity,
                                         Balance = item.Balance

                                     };
                                     _orderService.UpdateOrderGradeSizes(orderGradeSize.OrderId, orderGradeSize.GradeId, orderGradeSize.SizeId, orderGradeSize.Quantity,Convert.ToDouble(orderGradeSize.Balance));
                                 }
                                      }
                                     _batchService.UpdateBatchGradeSizes(batchGradeSizes); 
                        
                                 }
                                 
#endregion
                     if (orderHasBalance)
                     {
                         makeDelivery = new MakeDelivery()
                         {
                             StockReduced = true,
                             OrderQuantityBalance = 1,
                         };
                         return makeDelivery;
                     }
                     else
                     {
                         makeDelivery = new MakeDelivery()
                         {
                             StockReduced = true,
                             OrderQuantityBalance = 0,
                         };
                         return makeDelivery;
                     }
                       
                 }
             return makeDelivery;

            
    
           #endregion
          

        }

        private MakeDelivery MakeBatchDeliveryGradeSize(long deliveryId,Delivery delivery)
        {
            var makeDelivery = new MakeDelivery();
            BatchOutPut batchOutPut = new BatchOutPut();
            foreach (var item in delivery.Batches)
            {
                double batchAmount = 0, batchQuantity = 0;
                //var batchOutPuts = item.BatchOutPuts.ToList();

                //new code for june 2020
                #region NewCodex 
               

                #endregion

                //new code for july 12th 2020
                #region NewestCode
               

                        Grade foundGrade = new Grade();


                        if (item.AvailabeBatchGrades.Any())
                        {

                            bool hasGrade = false;
                            List<OrderGradeSize> orderGradeSizes = new List<OrderGradeSize>();
                            List<BatchDeliveryGradeSizeDTO> batchDeliveryGradeSizes = new List<BatchDeliveryGradeSizeDTO>();
                            var order = _orderService.GetOrder(delivery.OrderId);
                            #region foreach
                            foreach (var orderGrade in order.Grades)
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
                                        var batchDeliveryGradeSize = new BatchDeliveryGradeSizeDTO()
                                        {
                                            BatchId = item.BatchId,
                                            GradeId = foundGrade.GradeId,
                                            SizeId = foundGradeDenom.DenominationId,
                                            Quantity = foundGradeDenom.QuantityToRemove,
                                            //Amount = Convert.ToDouble(foundGradeDenom.QuantityToRemove * foundGradeDenom.Price),
                                            Amount = Convert.ToDouble(foundGradeDenom.QuantityToRemove * orderGrade.Denominations[denom].Price),
                                            Price = orderGrade.Denominations[denom].Price,
                                            DeliveryId = deliveryId,

                                        };

                                        batchDeliveryGradeSizes.Add(batchDeliveryGradeSize);

                                        denom = denom + 1;
                                    }
                                    #endregion
                                    _dataService.SaveBatchDeliveryGradeSize(batchDeliveryGradeSizes);
                                    batchDeliveryGradeSizes.Clear();

                                }

                            }



                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = 0,
                            };



                        }
                    

                    var batchDelivery = new DeliveryBatchDTO()
                    {
                        BatchId = item.BatchId,
                        BatchQuantity = batchQuantity,
                        Price = delivery.Price,
                        DeliveryId = deliveryId,
                        ProductId = delivery.ProductId,
                        Amount = batchAmount,
                    };
                    this.SaveDeliveryBatch(batchDelivery);

               

                #endregion

            }
            return makeDelivery;

            
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
                        if(grade.GradeId == foundGradeId)
                        {
                            gradeFound = grade;
                            
                            
                        }
                    }
                }
            }
            return gradeFound;
        }

        private List<OrderGradeSize> GetOrderGradeSizes(Order order, long gradeId)
        {
            List<OrderGradeSize> orderGradeSizes = new List<OrderGradeSize>();
            foreach (var orderGrade in order.Grades)
            {
                if (orderGrade.GradeId == gradeId)
                {
                    foreach (var denomination in orderGrade.Denominations)
                    {
                        var orderGradeSize = new OrderGradeSize()
                        {
                            GradeId = orderGrade.GradeId,
                            SizeId = denomination.DenominationId,
                            Quantity = denomination.Quantity,
                        };
                        orderGradeSizes.Add(orderGradeSize);
                    }
                   
                }
            }
            return orderGradeSizes;

        }

      
        private bool FindIfFlourOrderHasBalance(List<OrderGradeSize> orderGradeSizeList)
        {
           bool hasBalance = false;
            foreach (var denomBalance in orderGradeSizeList)
            {
                if (denomBalance.Balance > 0)
                {
                    hasBalance = true;
                    return hasBalance;
                }
            }
            return hasBalance;            
        }
       
        private MakeDelivery MakeFlourDeliveryRecord(Delivery delivery, string userId)
        {
            bool orderHasBalance = false;
            Grade foundGrade = new Grade();
            var makeDelivery = new MakeDelivery();
            #region withoutBatches
            if ((delivery.Batches == null || !delivery.Batches.Any()) && delivery.BranchId == 20006)
            {
                if(delivery.SelectedDeliveryGrades.Any())
                {   
                   
                    bool hasGrade = false;
                  List<OrderGradeSize> orderGradeSizes = new List<OrderGradeSize>();
                var order = _orderService.GetOrder(delivery.OrderId);
                     foreach (var orderGrade in order.Grades)
	                         {
                                 hasGrade = CheckIfBatchHasOrderGrade(orderGrade.GradeId, delivery.SelectedDeliveryGrades);
                                     if(hasGrade)
                                     {
                                  foundGrade =  GetGradeSameAsOrderGrade(delivery.SelectedDeliveryGrades,  orderGrade.GradeId);
                                  int j = 0;
                                  foreach (var foundGradeDenom in foundGrade.Denominations)
                                  {
                                      //int j = 0;
                                      //int count = orderGrade.Denominations.Count();
                                      double quantityToRemove = 0;
                                      bool flag = false;
                                      quantityToRemove = foundGradeDenom.Quantity;
                                      double balanceValue = 0,quantity = 0;
                                      for (int i = j; i < orderGrade.Denominations.Count(); i++)
			                                {
                                                j = j + 1;
                                                balanceValue = orderGrade.Denominations[i].Balance;
                                                quantity = orderGrade.Denominations[i].Quantity;
			                      
                                      var orderGradeSizeBalance =balanceValue - quantityToRemove;
                                          var orderGradeSize = new OrderGradeSize()
                                            {
                                                OrderId = delivery.OrderId,
                                                GradeId = orderGrade.GradeId,
                                                SizeId = foundGradeDenom.DenominationId,
                                                Quantity = quantity,
                                                Balance = orderGradeSizeBalance,
                                            };

                                          orderGradeSizes.Add(orderGradeSize);

                                          var inOrOut = false;
                                          //Method that removes flour from storeGradeSize table
                                          var storeGradeSize = new StoreGradeSize()
                                          {
                                              StoreId = delivery.StoreId,
                                              GradeId = orderGrade.GradeId,
                                              SizeId = foundGradeDenom.DenominationId,
                                              Quantity = quantityToRemove,
                                          };

                                          this._stockService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                      //}
                                          if (i < 6)
                                          {
                                              flag = true;
                                              break;
                                          }
                                            }
                                      if (flag)
                                      {
                                          continue;
                                      }
                                     }
                                 orderHasBalance =  FindIfFlourOrderHasBalance(orderGradeSizes);
                                 _orderService.PurgeOrderGradeSize(delivery.OrderId);
                                 
                                 foreach (var item in orderGradeSizes)
                                 {
                                     var orderGradeSize = new OrderGradeSize()
                                     {
                                         OrderId = item.OrderId,
                                         GradeId = item.GradeId,
                                         SizeId = item.SizeId,
                                         Quantity = item.Quantity,
                                         Balance = item.Balance

                                     };
                                     _orderService.UpdateOrderGradeSizes(orderGradeSize.OrderId, orderGradeSize.GradeId, orderGradeSize.SizeId, orderGradeSize.Quantity,Convert.ToDouble(orderGradeSize.Balance));
                                 }
                                      }
                                 }
                     if (orderHasBalance)
                     {
                         makeDelivery = new MakeDelivery()
                         {
                             StockReduced = true,
                             OrderQuantityBalance = 1,
                         };
                         return makeDelivery;
                     }
                     else
                     {
                         makeDelivery = new MakeDelivery()
                         {
                             StockReduced = true,
                             OrderQuantityBalance = 0,
                         };
                         return makeDelivery;
                     }
                       
                 }

            }
            #endregion
            
                #region WithBatches
            else
            {
               
                List<Batch> SortedBatchList = delivery.Batches.OrderBy(o => o.CreatedOn).ToList();
                foreach (var batch in SortedBatchList)
                {
                    makeDelivery = ReduceBatchFlourStock(delivery,batch.BatchOutPuts,userId);
                    if (makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance == 0)
                    {
                        makeDelivery = new MakeDelivery()
                        {
                            StockReduced = true,
                            OrderQuantityBalance = 0,
                        };
                        return makeDelivery;
                    }
                    else if (!makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance == 1)
                    {

                        makeDelivery = new MakeDelivery()
                        {
                            StockReduced = false,
                            OrderQuantityBalance = 1,
                        };
                        return makeDelivery;
                    }
                   
                   
                   
                }


            }
            #endregion

            return makeDelivery;
        }
        #endregion


        #region Brand

        private MakeDelivery ReduceBatchBrandStock(Batch batch, double orderQuantity, double totalQuantity, string userId)
        {
            var soldOut = false;
            MakeDelivery makeDelivery = new MakeDelivery();
            if (orderQuantity >= totalQuantity)
            {

                if (batch != null)
                {
                    if (orderQuantity >= totalQuantity)
                    {
                        if (totalQuantity > batch.BrandBalance)
                        {
                            orderBalance = orderQuantity - batch.BrandBalance;
                        }
                        else
                        {
                            orderBalance = orderQuantity - totalQuantity;
                        }

                        if (totalQuantity <= batch.BrandBalance)
                        {
                            batchBrandBalance = batch.BrandBalance - totalQuantity;
                            _batchService.UpdateBatchBrandBalance(batch.BatchId, batchBrandBalance, userId);
                            if (batchBrandBalance > 0)
                            {

                                makeDelivery = new MakeDelivery()
                                {
                                    StockReduced = soldOut,
                                    OrderQuantityBalance = orderBalance,
                                    BatchBrandBalance = batchBrandBalance,

                                };



                                return makeDelivery;
                            }
                            else
                            {

                                makeDelivery = new MakeDelivery()
                                {
                                    StockReduced = true,
                                    OrderQuantityBalance = orderBalance,
                                    BatchBrandBalance = batchBrandBalance,

                                };
                                return makeDelivery;
                            }

                        }
                        else
                        {
                            batchBrandBalance = 0;
                            _batchService.UpdateBatchBrandBalance(batch.BatchId, batchBrandBalance, userId);
                            orderBalance = orderQuantity - batch.BrandBalance;


                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = orderBalance,
                                BatchBrandBalance = batchBrandBalance,

                            };
                            return makeDelivery;
                        }

                    }
                    else
                    {

                        batchBrandBalance = batch.BrandBalance - orderQuantity;
                        orderBalance = batchBrandBalance - orderQuantity;
                        _batchService.UpdateBatchBrandBalance(batch.BatchId, batchBrandBalance, userId);
                        if (batchBrandBalance > 0)
                        {

                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = soldOut,
                                OrderQuantityBalance = orderBalance,
                                BatchBrandBalance = batchBrandBalance,

                            };
                            return makeDelivery;
                        }
                        else
                        {

                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = orderBalance,
                                BatchBrandBalance = batchBrandBalance,

                            };
                            return makeDelivery;
                        }
                    }



                }
                else
                {
                    //batch doesn't have enough quantities
                }
            }
            return makeDelivery;
        }
       

        private MakeDelivery MakeBrandDeliveryRecord(long storeId, Delivery delivery, string userId)
        {
            var makeDelivery = new MakeDelivery();
            
            if (delivery.Batches == null || !delivery.Batches.Any())
            {
                return makeDelivery;
            }
            else
            {
                List<Batch> batchesList = new List<Batch>();
                foreach (var deliveryBatch in delivery.Batches)
                {
                    var batch = _batchService.GetBatch(deliveryBatch.BatchId);
                    batchesList.Add(batch);
                }

                List<Batch> SortedBatchList = batchesList.OrderBy(o => o.CreatedOn).ToList();
                foreach (var batch in SortedBatchList)
                {
                    Order order = null;
                    order = _orderService.GetOrder(delivery.OrderId);
                    if (orderBalance <= 0)
                    {
                        orderBalance = Convert.ToDouble(order.Balance);
                    }
                    else
                    {
                        delivery.Quantity = orderBalance;
                        
                    }
                    
                    if (batch.BrandBalance > 0)
                    {
                        
                       makeDelivery = ReduceBatchBrandStock(batch, orderBalance, delivery.Quantity, userId);
                        if (makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance == 0)
                        {
                            soldOutAmount = batch.BrandBalance - batchBrandBalance;
                            BatchDeliveryDetails batchDetails = new BatchDeliveryDetails()
                            {
                                BatchId = batch.BatchId,
                                BatchNumber = batch.Name,
                                BatchQuantity = soldOutAmount
                            };

                            batchDeliveryList.Add(batchDetails);
                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = 0,
                                BatchList = batchDeliveryList,
                            };
                            orderBalance = makeDelivery.OrderQuantityBalance;
                            _orderService.UpdateOrderWithBalance(order.OrderId, makeDelivery.OrderQuantityBalance, userId);

                            return makeDelivery;
                        }
                        else if (!makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance == 0)
                        {
                            soldOutAmount = batch.BrandBalance - batchBrandBalance;
                            BatchDeliveryDetails batchDetails = new BatchDeliveryDetails()
                            {
                                BatchId = batch.BatchId,
                                BatchNumber = batch.Name,
                                BatchQuantity = soldOutAmount
                            };

                            batchDeliveryList.Add(batchDetails);
                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = 0,
                                BatchList = batchDeliveryList,
                            };
                            _orderService.UpdateOrderWithBalance(order.OrderId, makeDelivery.OrderQuantityBalance, userId);
                            orderBalance = makeDelivery.OrderQuantityBalance;
                            return makeDelivery;
                        }

                        else if (!makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance > 0)
                        {
                            soldOutAmount = batch.BrandBalance - batchBrandBalance;
                            BatchDeliveryDetails batchDetails = new BatchDeliveryDetails()
                            {
                                BatchId = batch.BatchId,
                                BatchNumber = batch.Name,
                                BatchQuantity = soldOutAmount
                            };

                            batchDeliveryList.Add(batchDetails);
                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = false,
                                OrderQuantityBalance = makeDelivery.OrderQuantityBalance,
                                BatchList = batchDeliveryList,
                            };
                            _orderService.UpdateOrderWithBalance(order.OrderId, makeDelivery.OrderQuantityBalance, userId);
                            orderBalance = makeDelivery.OrderQuantityBalance;
                            
                        }
                        else if (makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance > 0)
                        {
                            soldOutAmount = batch.BrandBalance - batchBrandBalance;
                            BatchDeliveryDetails batchDetails = new BatchDeliveryDetails()
                            {
                                BatchId = batch.BatchId,
                                BatchNumber = batch.Name,
                                BatchQuantity = soldOutAmount
                            };

                            batchDeliveryList.Add(batchDetails);
                            makeDelivery = new MakeDelivery()
                            {
                                StockReduced = true,
                                OrderQuantityBalance = makeDelivery.OrderQuantityBalance,
                                BatchList = batchDeliveryList,
                            };
                            _orderService.UpdateOrderWithBalance(order.OrderId, makeDelivery.OrderQuantityBalance, userId);
                            orderBalance = makeDelivery.OrderQuantityBalance;

                        }
                       
                    }
                   
                }
            }
            return makeDelivery;
        }
        #endregion
        #endregion


        public long SaveDelivery(Delivery delivery, string userId)
        {
            double totalQuantity = delivery.Quantity;
            if (delivery.Approved == null)
            { 

               
            long deliveryId = 0;
            MakeDelivery makeDelivery = new MakeDelivery();

            if (delivery.OrderId != 0)
            {
                #region deliver brand
                if (delivery.ProductId == 2 && delivery.WeightLoss >= 0)
                {

                    var deliveryDTO = new DTO.DeliveryDTO()
                    {
                        StoreId = delivery.StoreId,
                        CustomerId = delivery.CustomerId,
                        DeliveryCost = delivery.DeliveryCost,
                        OrderId = delivery.OrderId,
                        VehicleNumber = delivery.VehicleNumber,
                        BranchId = delivery.BranchId,
                        SectorId = delivery.SectorId,
                        PaymentModeId = delivery.PaymentModeId,
                        DeliveryDate = delivery.DeliveryDate,
                        Price = delivery.Price,
                        Quantity = delivery.Quantity,
                        ProductId = delivery.ProductId,
                        Amount = delivery.Amount,
                        Location = delivery.Location,
                        TransactionSubTypeId = delivery.TransactionSubTypeId,
                        MediaId = delivery.MediaId,
                        DeliveryId = delivery.DeliveryId,
                        DriverName = delivery.DriverName,
                        DriverNIN = delivery.DriverNIN,
                        Deleted = delivery.Deleted,
                        CreatedBy = delivery.CreatedBy,
                        CreatedOn = delivery.CreatedOn,
                        Approved = delivery.Approved,


                    };

                    //this has been commented out on 29/4/2020 coz of change in functionality
                    //makeDelivery = MakeBrandDeliveryRecord(delivery.StoreId, delivery, userId);
                    //if (makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance == 0)
                    //{
                    deliveryId = this._dataService.SaveDelivery(deliveryDTO, userId);

                    if (delivery.WeightLoss > 0)
                    {
                        var weightLoss = new WeightLoss()
                        {

                            CustomerId = delivery.CustomerId,
                            BranchId = delivery.BranchId,
                            DeliveryDate = delivery.DeliveryDate,
                            Price = Convert.ToDouble(delivery.Price),
                            Quantity = delivery.WeightLoss,
                            DeliveryId = deliveryId,
                            Deleted = delivery.Deleted,
                            CreatedBy = delivery.CreatedBy,
                            CreatedOn = delivery.CreatedOn,
                            Approved = delivery.Approved,


                        };
                        var weightLossId = this._weightLossService.SaveWeightLoss(weightLoss, userId);

                    }
                    double totalBrandQuantity = (delivery.WeightLoss + delivery.Quantity);
                    this._maizeBrandStoreService.UpdateBrandStore(deliveryDTO.BranchId,"-", userId, totalBrandQuantity);
                    // changed order balance to zero on 29/4/2020

                       _orderService.UpdateOrderWithCompletedStatus(delivery.OrderId, orderStatusIdComplete, 0, userId);

                    foreach (var deliveryBatch in delivery.Batches)
                    {

                        var batchDelivery = new DeliveryBatchDTO()
                        {
                            BatchId = deliveryBatch.BatchId,
                            BatchQuantity = deliveryBatch.QuantityToRemove,
                            Price = delivery.Price,
                            DeliveryId = deliveryId,
                            ProductId = delivery.ProductId,
                            Amount = Convert.ToDouble(delivery.Price * deliveryBatch.QuantityToRemove),
                        };
                        this.SaveDeliveryBatch(batchDelivery);


                    }
                    #region generate documents
                    if (deliveryDTO.PaymentModeId == 2 || deliveryDTO.PaymentModeId == 1 || deliveryDTO.PaymentModeId == 10004)
                    {
                        //generate receipt
                        // throw new NotImplementedException();
                        var username = string.Empty;
                        var user = _userService.GetAspNetUser(delivery.CustomerId);
                        if (user != null)
                        {
                            username = user.FirstName + " " + user.LastName;
                        }
                        var document = new Document()
                        {
                            DocumentId = 0,

                            UserId = username,
                            DocumentCategoryId = receiptId,
                            Amount = delivery.Amount,
                            BranchId = delivery.BranchId,
                            ItemId = deliveryId,
                            Description = "Delivery of maize brand of " + totalQuantity.ToString() + " kgs" + " at" + delivery.Price + " Per Kilogram",



                        };

                        var documentId = _documentService.SaveDocument(document, userId);
                    }
                    else
                    {
                        //Generate  Invoice
                        // throw new NotImplementedException();
                        var username = string.Empty;
                        var user = _userService.GetAspNetUser(delivery.CustomerId);
                        if (user != null)
                        {
                            username = user.FirstName + " " + user.LastName;
                        }
                        var document = new Document()
                        {
                            DocumentId = 0,

                            UserId = username,
                            DocumentCategoryId = invoiceId,
                            Amount = delivery.Amount,
                            BranchId = delivery.BranchId,
                            ItemId = deliveryId,
                            Description = "Delivery of maize brand of " + totalQuantity.ToString() + " kgs" + " at" + delivery.Price + " Per Kilogram",



                        };

                        var documentId = _documentService.SaveDocument(document, userId);
                    }
                    #endregion



                }
                #endregion

                #region deliver flour
                else if(delivery.ProductId == 1)
                {
                        if(delivery.Batches == null && (delivery.BranchId == 20006 || delivery.BranchId == 20013))
                        {
                            if (delivery.SelectedDeliveryGrades.Any())
                            {
                                var gradeStore = CheckForFlourInStore(delivery.SelectedDeliveryGrades, delivery.StoreId);
                                if (gradeStore == -2)
                                {
                                    deliveryId = gradeStore;
                                    return deliveryId;
                                }
                                else if (gradeStore == -1)
                                {
                                    gradeStore = -4;
                                    deliveryId = gradeStore;
                                    return deliveryId;
                                }
                                else
                                {

                                    var deliveryDTO = new DTO.DeliveryDTO()
                                    {
                                        StoreId = delivery.StoreId,
                                        CustomerId = delivery.CustomerId,
                                        DeliveryCost = delivery.DeliveryCost,
                                        OrderId = delivery.OrderId,
                                        VehicleNumber = delivery.VehicleNumber,
                                        BranchId = delivery.BranchId,
                                        SectorId = delivery.SectorId,
                                        PaymentModeId = delivery.PaymentModeId,
                                        DeliveryDate = delivery.DeliveryDate,
                                        Price = delivery.Price,
                                        ProductId = delivery.ProductId,
                                        Amount = delivery.Amount,
                                        Quantity = delivery.Quantity,
                                        Location = delivery.Location,
                                        TransactionSubTypeId = delivery.TransactionSubTypeId,
                                        MediaId = delivery.MediaId,
                                        DeliveryId = delivery.DeliveryId,
                                        DriverName = delivery.DriverName,
                                        DriverNIN = delivery.DriverNIN,
                                        Deleted = delivery.Deleted,
                                        CreatedBy = delivery.CreatedBy,
                                        CreatedOn = delivery.CreatedOn,
                                        Approved = delivery.Approved,


                                    };

                                    deliveryId = this._dataService.SaveDelivery(deliveryDTO, userId);
                                  

                                        _orderService.UpdateOrderWithCompletedStatus(delivery.OrderId, orderStatusIdComplete, 0, userId);


                                        List<DeliveryGradeSize> deliveryGradeSizeList = new List<DeliveryGradeSize>();
                                        if (delivery.SelectedDeliveryGrades != null)
                                        {
                                            if (delivery.SelectedDeliveryGrades.Any())
                                            {
                                                foreach (var grade in delivery.SelectedDeliveryGrades)
                                                {
                                                    long sizeId = 0;
                                                    double amount = 0, price = 0, quantity = 0;

                                                    foreach (var denomination in grade.Denominations)
                                                    {
                                                        sizeId = denomination.DenominationId;
                                                        price = denomination.Price;
                                                        quantity = denomination.Quantity;
                                                        amount = (denomination.Quantity * denomination.Price);

                                                        var deliveryGradeSize = new DeliveryGradeSize()
                                                        {
                                                            DeliveryId = deliveryId,
                                                            GradeId = grade.GradeId,
                                                            SizeId = sizeId,
                                                            Quantity = quantity,
                                                            Price = price,
                                                            Amount = amount,
                                                            TimeStamp = DateTime.Now,

                                                        };
                                                        deliveryGradeSizeList.Add(deliveryGradeSize);
                                                        var inOrOut = false;
                                                        //Method that removes flour from storeGradeSize table
                                                        var storeGradeSize = new StoreGradeSize()
                                                        {
                                                            StoreId = delivery.StoreId,
                                                            GradeId = grade.GradeId,
                                                            SizeId = denomination.DenominationId,
                                                            Quantity = denomination.Quantity,
                                                        };

                                                        this._stockService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                                        //}

                                                    }

                                                }
                                                this._dataService.PurgeDeliveryGradeSize(deliveryId);
                                                this.SaveDeliveryGradeSizeList(deliveryGradeSizeList);
                                            }
                                        }

                                        #region generate documents
                                        if (deliveryDTO.PaymentModeId == 2 || deliveryDTO.PaymentModeId == 1 || deliveryDTO.PaymentModeId == 10004)
                                        {
                                            //generate receipt
                                            //throw new NotImplementedException();
                                            var username = string.Empty;
                                            var user = _userService.GetAspNetUser(delivery.CustomerId);
                                            if (user != null)
                                            {
                                                username = user.FirstName + " " + user.LastName;
                                            }
                                            var document = new Document()
                                            {
                                                DocumentId = 0,

                                                UserId = username,
                                                DocumentCategoryId = receiptId,
                                                Amount = delivery.Amount,
                                                BranchId = delivery.BranchId,
                                                ItemId = deliveryId,
                                                Description = "Delivery of maize flour of " + delivery.Quantity.ToString() + " kgs",

                                                Grades = delivery.SelectedDeliveryGrades,

                                            };

                                            documentId = _documentService.SaveDocument(document, userId);
                                        }
                                        else
                                        {
                                            //Generate  Invoice
                                            //throw new NotImplementedException();
                                            var username = string.Empty;
                                            var user = _userService.GetAspNetUser(delivery.CustomerId);
                                            if (user != null)
                                            {
                                                username = user.FirstName + " " + user.LastName;
                                            }
                                            var document = new Document()
                                            {
                                                DocumentId = 0,

                                                UserId = username,
                                                DocumentCategoryId = invoiceId,
                                                Amount = delivery.Amount,
                                                BranchId = delivery.BranchId,
                                                ItemId = deliveryId,
                                                Description = "Delivery of maize flour of " + delivery.Quantity.ToString() + " kgs",

                                                Grades = delivery.SelectedDeliveryGrades,

                                            };

                                            documentId = _documentService.SaveDocument(document, userId);
                                        }
                                        #endregion

                                }

                            }
                        }
                        else
                        {
                            if(delivery.Batches == null && (delivery.BranchId == 20006 || delivery.BranchId == 20013))
                            {
                                //no batch selected
                                deliveryId = -1;
                                return deliveryId;
                            }
                            else
                            {
                                var gradeStore = CheckForFlourInStore(delivery.SelectedDeliveryGrades, delivery.StoreId);
                                if (gradeStore == -2)
                                {
                                    deliveryId = gradeStore;
                                    return deliveryId;
                                }
                                else if (gradeStore == -1)
                                {
                                    gradeStore = -4;
                                    deliveryId = gradeStore;
                                    return deliveryId;
                                }
                                else
                                {

                                    var deliveryDTO = new DTO.DeliveryDTO()
                                    {
                                        StoreId = delivery.StoreId,
                                        CustomerId = delivery.CustomerId,
                                        DeliveryCost = delivery.DeliveryCost,
                                        OrderId = delivery.OrderId,
                                        VehicleNumber = delivery.VehicleNumber,
                                        BranchId = delivery.BranchId,
                                        SectorId = delivery.SectorId,
                                        PaymentModeId = delivery.PaymentModeId,
                                        DeliveryDate = delivery.DeliveryDate,
                                        Price = delivery.Price,
                                        ProductId = delivery.ProductId,
                                        Amount = delivery.Amount,
                                        Quantity = delivery.Quantity,
                                        Location = delivery.Location,
                                        TransactionSubTypeId = delivery.TransactionSubTypeId,
                                        MediaId = delivery.MediaId,
                                        DeliveryId = delivery.DeliveryId,
                                        DriverName = delivery.DriverName,
                                        DriverNIN = delivery.DriverNIN,
                                        Deleted = delivery.Deleted,
                                        CreatedBy = delivery.CreatedBy,
                                        CreatedOn = delivery.CreatedOn,
                                        Approved = delivery.Approved,


                                    };

                                    deliveryId = this._dataService.SaveDelivery(deliveryDTO, userId);
                                    if (delivery.Batches != null)
                                    {
                                        makeDelivery = MakeBatchDeliveryGradeSize(deliveryId, delivery);
                                        if (makeDelivery.StockReduced && makeDelivery.OrderQuantityBalance == 0)
                                        {

                                            _orderService.UpdateOrderWithCompletedStatus(delivery.OrderId, orderStatusIdComplete, 0, userId);


                                            List<DeliveryGradeSize> deliveryGradeSizeList = new List<DeliveryGradeSize>();
                                            if (delivery.SelectedDeliveryGrades != null)
                                            {
                                                if (delivery.SelectedDeliveryGrades.Any())
                                                {
                                                    foreach (var grade in delivery.SelectedDeliveryGrades)
                                                    {
                                                        long sizeId = 0;
                                                        double amount = 0, price = 0, quantity = 0;

                                                        foreach (var denomination in grade.Denominations)
                                                        {
                                                            sizeId = denomination.DenominationId;
                                                            price = denomination.Price;
                                                            quantity = denomination.Quantity;
                                                            amount = (denomination.Quantity * denomination.Price);

                                                            var deliveryGradeSize = new DeliveryGradeSize()
                                                            {
                                                                DeliveryId = deliveryId,
                                                                GradeId = grade.GradeId,
                                                                SizeId = sizeId,
                                                                Quantity = quantity,
                                                                Price = price,
                                                                Amount = amount,
                                                                TimeStamp = DateTime.Now,

                                                            };
                                                            deliveryGradeSizeList.Add(deliveryGradeSize);
                                                            var inOrOut = false;
                                                            //Method that removes flour from storeGradeSize table
                                                            var storeGradeSize = new StoreGradeSize()
                                                            {
                                                                StoreId = delivery.StoreId,
                                                                GradeId = grade.GradeId,
                                                                SizeId = denomination.DenominationId,
                                                                Quantity = denomination.Quantity,
                                                            };

                                                            this._stockService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                                            //}

                                                        }

                                                    }
                                                    this._dataService.PurgeDeliveryGradeSize(deliveryId);
                                                    this.SaveDeliveryGradeSizeList(deliveryGradeSizeList);
                                                }
                                            }

                                            #region generate documents
                                            if (deliveryDTO.PaymentModeId == 2 || deliveryDTO.PaymentModeId == 1 || deliveryDTO.PaymentModeId == 10004)
                                            {
                                                //generate receipt
                                                //throw new NotImplementedException();
                                                var username = string.Empty;
                                                var user = _userService.GetAspNetUser(delivery.CustomerId);
                                                if (user != null)
                                                {
                                                    username = user.FirstName + " " + user.LastName;
                                                }
                                                var document = new Document()
                                                {
                                                    DocumentId = 0,

                                                    UserId = username,
                                                    DocumentCategoryId = receiptId,
                                                    Amount = delivery.Amount,
                                                    BranchId = delivery.BranchId,
                                                    ItemId = deliveryId,
                                                    Description = "Delivery of maize flour of " + delivery.Quantity.ToString() + " kgs",

                                                    Grades = delivery.SelectedDeliveryGrades,

                                                };

                                                documentId = _documentService.SaveDocument(document, userId);
                                            }
                                            else
                                            {
                                                //Generate  Invoice
                                                //throw new NotImplementedException();
                                                var username = string.Empty;
                                                var user = _userService.GetAspNetUser(delivery.CustomerId);
                                                if (user != null)
                                                {
                                                    username = user.FirstName + " " + user.LastName;
                                                }
                                                var document = new Document()
                                                {
                                                    DocumentId = 0,

                                                    UserId = username,
                                                    DocumentCategoryId = invoiceId,
                                                    Amount = delivery.Amount,
                                                    BranchId = delivery.BranchId,
                                                    ItemId = deliveryId,
                                                    Description = "Delivery of maize flour of " + delivery.Quantity.ToString() + " kgs",

                                                    Grades = delivery.SelectedDeliveryGrades,

                                                };

                                                documentId = _documentService.SaveDocument(document, userId);
                                            }
                                            #endregion


                                        }
                                        else
                                        {
                                            _dataService.MarkAsDeleted(deliveryId, userId);
                                            return deliveryId = -1;

                                        }
                                    }
                                   
                                }
                            }
                        }
                  
                   

                }
                    #endregion

                    #region deliver rice
                    
                        if (delivery.SelectedDeliveryGrades.Any())
                        {
                            var gradeStore = CheckForFlourInStore(delivery.SelectedDeliveryGrades, delivery.StoreId);
                            if (gradeStore == -2)
                            {
                                deliveryId = gradeStore;
                                return deliveryId;
                            }
                            else if (gradeStore == -1)
                            {
                                gradeStore = -4;
                                deliveryId = gradeStore;
                                return deliveryId;
                            }
                            else
                            {

                                var deliveryDTO = new DTO.DeliveryDTO()
                                {
                                    StoreId = delivery.StoreId,
                                    CustomerId = delivery.CustomerId,
                                    DeliveryCost = delivery.DeliveryCost,
                                    OrderId = delivery.OrderId,
                                    VehicleNumber = delivery.VehicleNumber,
                                    BranchId = delivery.BranchId,
                                    SectorId = delivery.SectorId,
                                    PaymentModeId = delivery.PaymentModeId,
                                    DeliveryDate = delivery.DeliveryDate,
                                    Price = delivery.Price,
                                    ProductId = delivery.ProductId,
                                    Amount = delivery.Amount,
                                    Quantity = delivery.Quantity,
                                    Location = delivery.Location,
                                    TransactionSubTypeId = delivery.TransactionSubTypeId,
                                    MediaId = delivery.MediaId,
                                    DeliveryId = delivery.DeliveryId,
                                    DriverName = delivery.DriverName,
                                    DriverNIN = delivery.DriverNIN,
                                    Deleted = delivery.Deleted,
                                    CreatedBy = delivery.CreatedBy,
                                    CreatedOn = delivery.CreatedOn,
                                    Approved = delivery.Approved,


                                };

                                deliveryId = this._dataService.SaveDelivery(deliveryDTO, userId);


                                _orderService.UpdateOrderWithCompletedStatus(delivery.OrderId, orderStatusIdComplete, 0, userId);


                                List<DeliveryGradeSize> deliveryGradeSizeList = new List<DeliveryGradeSize>();
                                if (delivery.SelectedDeliveryGrades != null)
                                {
                                    if (delivery.SelectedDeliveryGrades.Any())
                                    {
                                        foreach (var grade in delivery.SelectedDeliveryGrades)
                                        {
                                            long sizeId = 0;
                                            double amount = 0, price = 0, quantity = 0;

                                            foreach (var denomination in grade.Denominations)
                                            {
                                                sizeId = denomination.DenominationId;
                                                price = denomination.Price;
                                                quantity = denomination.Quantity;
                                                amount = (denomination.Quantity * denomination.Price);

                                                var deliveryGradeSize = new DeliveryGradeSize()
                                                {
                                                    DeliveryId = deliveryId,
                                                    GradeId = grade.GradeId,
                                                    SizeId = sizeId,
                                                    Quantity = quantity,
                                                    Price = price,
                                                    Amount = amount,
                                                    TimeStamp = DateTime.Now,

                                                };
                                                deliveryGradeSizeList.Add(deliveryGradeSize);
                                                var inOrOut = false;
                                                //Method that removes flour from storeGradeSize table
                                                var storeGradeSize = new StoreGradeSize()
                                                {
                                                    StoreId = delivery.StoreId,
                                                    GradeId = grade.GradeId,
                                                    SizeId = denomination.DenominationId,
                                                    Quantity = denomination.Quantity,
                                                };

                                                this._stockService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                                //}

                                            }

                                        }
                                        this._dataService.PurgeDeliveryGradeSize(deliveryId);
                                        this.SaveDeliveryGradeSizeList(deliveryGradeSizeList);
                                    }
                                }

                                #region generate documents
                                if (deliveryDTO.PaymentModeId == 2 || deliveryDTO.PaymentModeId == 1 || deliveryDTO.PaymentModeId == 10004)
                                {
                                    //generate receipt
                                    //throw new NotImplementedException();
                                    var username = string.Empty;
                                    var user = _userService.GetAspNetUser(delivery.CustomerId);
                                    if (user != null)
                                    {
                                        username = user.FirstName + " " + user.LastName;
                                    }
                                    var document = new Document()
                                    {
                                        DocumentId = 0,

                                        UserId = username,
                                        DocumentCategoryId = receiptId,
                                        Amount = delivery.Amount,
                                        BranchId = delivery.BranchId,
                                        ItemId = deliveryId,
                                        Description = "Delivery of Rice Worth " + delivery.Quantity.ToString() + " kgs",

                                        Grades = delivery.SelectedDeliveryGrades,

                                    };

                                    documentId = _documentService.SaveDocument(document, userId);
                                }
                                else
                                {
                                    //Generate  Invoice
                                    //throw new NotImplementedException();
                                    var username = string.Empty;
                                    var user = _userService.GetAspNetUser(delivery.CustomerId);
                                    if (user != null)
                                    {
                                        username = user.FirstName + " " + user.LastName;
                                    }
                                    var document = new Document()
                                    {
                                        DocumentId = 0,

                                        UserId = username,
                                        DocumentCategoryId = invoiceId,
                                        Amount = delivery.Amount,
                                        BranchId = delivery.BranchId,
                                        ItemId = deliveryId,
                                        Description = "Delivery of Rice Worth " + delivery.Quantity.ToString() + " kgs",

                                        Grades = delivery.SelectedDeliveryGrades,

                                    };

                                    documentId = _documentService.SaveDocument(document, userId);
                                }
                                #endregion

                            }

                        }
                   
                  

                    #endregion


                    return deliveryId;
            }



        }
            else
            {
                if(delivery.CreatedById == userId)
                {
                    long deliveryId = -22;
                    return deliveryId;
                }
                else
                {
                    //update delivery with either approved == true or rejected == true
                    long deliveryId = 0;
                    if (Convert.ToBoolean(delivery.Approved))
                    {
                        deliveryId = _dataService.UpdateDeliveryOnApprovalOrRejection(delivery.DeliveryId, Convert.ToBoolean(delivery.Approved), userId);

                        #region transactions

                        long transactionSubTypeId = 0;
                        var notes = string.Empty;
                        if (delivery.ProductId == 1)
                        {
                            transactionSubTypeId = flourTransactionSubTypeId;
                            notes = "Maize Flour Sale of " + delivery.Quantity.ToString() + " kgs";
                        }
                        else if (delivery.ProductId == 2)
                        {
                            transactionSubTypeId = branTransactionSubTypeId;
                            notes = "Brand Sale of " + totalQuantity + "kgs at " + delivery.Price + " Per Kilogram";
                        }
                        else if (delivery.ProductId == 10003)
                        {
                            transactionSubTypeId = riceTransactionSubTypeId;
                            notes = "Rice Sale of " + delivery.Quantity.ToString() + " kgs";
                        }
                        var paymentMode = _accountTransactionActivityService.GetPaymentMode(delivery.PaymentModeId);
                        var paymentModeName = paymentMode.Name;
                        if (paymentModeName == "Credit" || paymentModeName == "AdvancePayment")
                        {

                            var accountActivity = new AccountTransactionActivity()
                            {

                                AspNetUserId = delivery.CustomerId,
                                Amount = delivery.Amount,
                                Notes = notes,
                                Action = "-",
                                BranchId = delivery.BranchId,
                                TransactionSubTypeId = transactionSubTypeId,
                                SectorId = delivery.SectorId,
                                Deleted = delivery.Deleted,
                                CreatedBy = userId,
                                PaymentMode = paymentModeName,
                                SupplyId = deliveryId,
                                Quantity = delivery.ProductId == 2 ? totalQuantity : delivery.Quantity,
                                WeightNote = Convert.ToString(delivery.DocumentId),



                            };
                            var accountActivityId = this._accountTransactionActivityService.SaveAccountTransactionActivity(accountActivity, userId);

                        }


                        long transactionTypeId = 0;
                        var transactionSubtype = _transactionSubTypeService.GetTransactionSubType(delivery.TransactionSubTypeId);
                        if (transactionSubtype != null)
                        {
                            transactionTypeId = transactionSubtype.TransactionTypeId;
                        }

                        var transaction = new TransactionDTO()
                        {
                            BranchId = delivery.BranchId,
                            SectorId = delivery.SectorId,
                            Amount = delivery.DeliveryCost,
                            TransactionSubTypeId = delivery.TransactionSubTypeId,
                            TransactionTypeId = transactionTypeId,
                            CreatedOn = DateTime.Now,
                            TimeStamp = DateTime.Now,
                            CreatedBy = userId,
                            Deleted = false,

                        };
                        var transactionId = _transactionDataService.SaveTransaction(transaction, userId);
                        return deliveryId;





                        #endregion
                        //return deliveryId;
                    }
                    else
                    {
                        if (delivery.ProductId == 1)
                        {
                            //_dataService.PurgeDeliveryGradeSize(delivery.DeliveryId);
                            _dataService.PurgeBatchDeliveryGradeSize(delivery.DeliveryId);


                            foreach (var item in delivery.Grades)
                            {
                                int j = 0;
                                bool flag = false;
                                foreach (var denom in item.Denominations)
                                {
                                    for (int i = j; i < item.Denominations.Count(); i++)
                                    {
                                        j = j + 1;

                                        var inOrOut = true;
                                        //Method that adds flour back to storeGradeSize table
                                        var storeGradeSize = new StoreGradeSize()
                                        {
                                            StoreId = delivery.StoreId,
                                            GradeId = item.GradeId,
                                            SizeId = denom.DenominationId,
                                            Quantity = denom.Quantity,
                                        };

                                        this._stockService.SaveStoreGradeSize(storeGradeSize, inOrOut);
                                        if (i < 6)
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (flag)
                                    {
                                        continue;
                                    }
                                }

                            }

                        }
                        else
                        {
                            var weightLoss = this._weightLossService.GetWeightLossForDelivery(delivery.DeliveryId);
                            var deliveryQuantity = Convert.ToDouble(delivery.Quantity + weightLoss.Quantity);
                            this._maizeBrandStoreService.UpdateBrandStore(delivery.BranchId, "+", userId, deliveryQuantity);
                            this._weightLossService.PurgeWeightLoss(delivery.DeliveryId, userId);
                        }
                        _dataService.PurgeDeliveryBatch(delivery.DeliveryId);
                        deliveryId = _dataService.UpdateDeliveryOnApprovalOrRejection(delivery.DeliveryId, Convert.ToBoolean(delivery.Approved), userId);
                        _orderService.UpdateOrderWithCompletedStatus(delivery.OrderId, 10002, delivery.Quantity, userId);

                        return delivery.DeliveryId;
                    }
                }
               
                
            }
            return delivery.DeliveryId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long deliveryId, string userId)
        {
            _dataService.MarkAsDeleted(deliveryId, userId);
        }


        public long CheckForFlourInStore(List<Grade> grades, long storeId)
        {

            foreach (var grade in grades)
            {
                //var result = _buveraService.GetStoreBuveraGradeSize(grade.GradeId, storeId);
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

        public void SaveDeliveryBatchList(List<DeliveryBatch> deliveryBatchList)
        {
            if (deliveryBatchList != null)
            {
                if (deliveryBatchList.Any())
                {
                    foreach (var deliveryBatch in deliveryBatchList)
                    {
                        var deliveryBatchDTO = new DTO.DeliveryBatchDTO()
                        {
                            DeliveryId = deliveryBatch.DeliveryId,
                            BatchQuantity = deliveryBatch.BatchQuantity,
                            BatchId  = deliveryBatch.BatchId,
                            Price = deliveryBatch.Price,
                            Amount = deliveryBatch.Amount,
                            ProductId = deliveryBatch.ProductId,
                             CreatedOn = deliveryBatch.CreatedOn,
                            TimeStamp = deliveryBatch.TimeStamp
                        };
                        this.SaveDeliveryBatch(deliveryBatchDTO);
                    }
                }
            }
        }

         public void SaveDeliveryBatch(DeliveryBatchDTO deliveryBatchDTO)
            {
                 _dataService.SaveDeliveryBatch(deliveryBatchDTO);
            }

        public  void SaveDeliveryGradeSizeList(List<DeliveryGradeSize> deliveryGradeSizeList)
        {
            if (deliveryGradeSizeList != null)
            {
                if (deliveryGradeSizeList.Any())
                {
                    foreach (var deliveryGradeSize in deliveryGradeSizeList)
                    {
                        var deliveryGradeSizeDTO = new DTO.DeliveryGradeSizeDTO()
                        {
                            DeliveryId = deliveryGradeSize.DeliveryId,
                            GradeId = deliveryGradeSize.GradeId,
                            Quantity = deliveryGradeSize.Quantity,
                            SizeId = deliveryGradeSize.SizeId,
                            Price = deliveryGradeSize.Price,
                            Amount = deliveryGradeSize.Amount,
                            TimeStamp = deliveryGradeSize.TimeStamp
                        };
                        this.SaveDeliveryGradeSize(deliveryGradeSizeDTO);
                    }
                }
            }
        }
        public  void SaveDeliveryGradeSize(DeliveryGradeSizeDTO deliveryGradeSizeDTO)
        {
            _dataService.SaveDeliveryGradeSize(deliveryGradeSizeDTO);
        }

        public IEnumerable<DeliveryViewModel> GetAllUnApprovedDeliveries()
        {
            var results = this._dataService.GetAllUnApprovedDeliveries();
            return MapEFToModelViewModel(results);
        }
        public IEnumerable<Delivery> GetAllRejectedDeliveries()
        {
            var results = this._dataService.GetAllRejectedDeliveries();
            return MapEFToModel(results);
        }

        public IEnumerable<Delivery> GetAllApprovedDeliveries()
        {
            var results = this._dataService.GetAllApprovedDeliveries();
            return MapEFToModel(results);
        }



        #region Mapping Methods

        public IEnumerable<Delivery> MapEFToModel(IEnumerable<EF.Models.Delivery> data)
        {
            var list = new List<Delivery>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        public IEnumerable<DeliveryViewModel> MapEFToModelViewModel(IEnumerable<EF.Models.Delivery> data)
        {
            var list = new List<DeliveryViewModel>();
            foreach (var result in data)
            {
                list.Add(MapEFToModelViewModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Delivery EF object to Delivery Model Object and
        /// returns the Delivery model object.
        /// </summary>
        /// <param name="result">EF Delivery object to be mapped.</param>
        /// <returns>Delivery Model Object.</returns>
        public Delivery MapEFToModel(EF.Models.Delivery data)
        {
            Document documentOne, documentTwo;
            var delivery = new Delivery();
            long documentId = 0;
            if (data != null)
            {
                documentOne = _documentService.GetDocumentForAParticularItemAndCategory(data.DeliveryId, receiptId);
                if (documentOne != null)
                {
                    documentId = documentOne.DocumentId;

                }
                else if (documentOne == null)
                {

                    documentTwo = _documentService.GetDocumentForAParticularItemAndCategory(data.DeliveryId, invoiceId);

                    if (documentTwo != null)
                    {
                        documentId = documentTwo.DocumentId;
                    }
                    else
                    {

                        var document = _documentService.GetDocumentForAParticularItem(data.DeliveryId);

                    }

                }



                var customerName = string.Empty;
                var customer = _userService.GetAspNetUser(data.CustomerId);
                if (customer != null)
                {
                    customerName = customer.FirstName + ' ' + customer.LastName;
                }
                
                 delivery = new Delivery()
                {
                    CustomerName = customerName,
                    DeliveryCost = data.DeliveryCost,
                    OrderId = data.OrderId,
                    CustomerId = data.CustomerId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    TransactionSubTypeId = data.TransactionSubTypeId,
                    TransactionSubTypeName = data.TransactionSubType != null ? data.TransactionSubType.Name : "",
                    VehicleNumber = data.VehicleNumber,
                    BranchId = data.BranchId,
                    Location = data.Location,
                    PaymentModeId = data.PaymentModeId,
                    PaymentModeName = data.PaymentMode != null?data.PaymentMode.Name:"",
                    SectorId = data.SectorId,
                    StoreId = data.StoreId,
                    StoreName = data.Store != null ? data.Store.Name : "",
                    MediaId = data.MediaId,
                    DeliveryId = data.DeliveryId,
                    Quantity = data.Quantity,
                    DriverName = data.DriverName,
                    DeliveryDate = data.DeliveryDate,
                    DriverNIN = data.DriverNIN,
                    CreatedOn =Convert.ToDateTime(data.CreatedOn),
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    ProductId = data.ProductId,
                    ProductName = data.Product != null? data.Product.Name:"",
                    Amount = data.Amount,
                    CreatedById = data.CreatedBy,
                    Price = data.Price,
                   DocumentId = documentId,
                   //Document = data.Document
                   Approved = data.Approved,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };
                 var batches = _batchService.GetAllBatchesForADelivery(data.DeliveryId);
                 List<DeliveryBatch> deliveryBatchList = new List<DeliveryBatch>();
                 if (batches.Any())
                 {
                     foreach (var batch in batches)
                     {
                         var deliverybatch = new DeliveryBatch()
                         {
                             BatchId = batch.BatchId,
                             BatchNumber = batch.BatchNumber,
                             DeliveryId = batch.DeliveryId,
                              BatchQuantity = batch.BatchQuantity,
       
                         };
                         deliveryBatchList.Add(deliverybatch);

                     }
                     delivery.DeliveryBatches = deliveryBatchList;
                 }
                 if (data.DeliveryGradeSizes != null)
                 {
                     if (data.DeliveryGradeSizes.Any())
                     {
                         List<Grade> grades = new List<Grade>();
                        // var distinctGrades = data.DeliveryGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        var distinctGrades = data.DeliveryGradeSizes.Where(a => a.DeliveryId == data.DeliveryId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        foreach (var deliveryGradeSize in distinctGrades)
                         {
                             var grade = new Grade()
                             {
                                 GradeId = deliveryGradeSize.Grade.GradeId,
                                 Value = deliveryGradeSize.Grade.Value,
                                 CreatedOn = deliveryGradeSize.Grade.CreatedOn,
                                 TimeStamp = deliveryGradeSize.Grade.TimeStamp,
                                 Deleted = deliveryGradeSize.Grade.Deleted,
                                 CreatedBy = _userService.GetUserFullName(deliveryGradeSize.Grade.AspNetUser),
                                 UpdatedBy = _userService.GetUserFullName(deliveryGradeSize.Grade.AspNetUser1),
                             };
                             List<Denomination> denominations = new List<Denomination>();
                             if (deliveryGradeSize.Grade.DeliveryGradeSizes != null)
                             {
                                 if (deliveryGradeSize.Grade.DeliveryGradeSizes.Any())
                                 {

                                    var distinctSizes = deliveryGradeSize.Grade.DeliveryGradeSizes.Where(a => a.DeliveryId == data.DeliveryId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();

                                    //var distinctSizes = deliveryGradeSize.Grade.DeliveryGradeSizes.GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
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
                         delivery.Grades = grades;
                     }
                 }
                return delivery;
            }
            return delivery;
        }

        public DeliveryViewModel MapEFToModelViewModel(EF.Models.Delivery data)
        {
          
            var delivery = new DeliveryViewModel();
         
            if (data != null)
            {
            
                var customerName = string.Empty;
                var customer = _userService.GetAspNetUser(data.CustomerId);
                if (customer != null)
                {
                    customerName = customer.FirstName + ' ' + customer.LastName;
                }

                delivery = new DeliveryViewModel()
                {
                    CustomerName = customerName,
                  
                    CustomerId = data.CustomerId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                   
                    VehicleNumber = data.VehicleNumber,
                    BranchId = data.BranchId,
                    Location = data.Location,
                  
             
                    DeliveryId = data.DeliveryId,
                    Quantity = data.Quantity,
                    DriverName = data.DriverName,
                   
                    ProductName = data.Product != null ? data.Product.Name : "",
                    Amount = data.Amount,
                   

                };
              
                return delivery;
            }
            return delivery;
        }

        #endregion
    }
}
