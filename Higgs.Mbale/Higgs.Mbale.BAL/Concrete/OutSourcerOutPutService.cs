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
   public class OutSourcerOutPutService : IOutSourcerOutPutService
    {
        private long flourTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["FlourSaleTransactionSubTypeId"]);
        private long buveraCategoryId = Convert.ToInt64(ConfigurationManager.AppSettings["UsedBuveraCategoryId"]);
        ILog logger = log4net.LogManager.GetLogger(typeof(OutSourcerOutPutService));
        private IOutSourcerOutPutDataService _dataService;
        private IUserService _userService;
        private IStoreService _storeService;
        private IStockService _stockService;
        private IBuveraService _buveraService;
        private IAccountTransactionActivityService _accountTransactionActivityService;
        


        public OutSourcerOutPutService(IOutSourcerOutPutDataService dataService, IUserService userService,  
            IStoreService storeService,  IStockService stockService, IBuveraService buveraService,IAccountTransactionActivityService accountTransactionActivityService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._storeService = storeService;
            this._stockService = stockService;
            this._buveraService = buveraService;
            this._accountTransactionActivityService = accountTransactionActivityService;
           
        }

        
        public OutSourcerOutPut GetOutSourcerOutPut(long outSourcerOutPutId)
        {
            var result = this._dataService.GetOutSourcerOutPut(outSourcerOutPutId);
            return MapEFToModel(result);
        }

       
        public IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPuts()
        {
            var results = this._dataService.GetAllOutSourcerOutPuts();
            return MapEFToModel(results);
        }


        public IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPutsForAParticularOutSourcerStore(long storeId)
        {
            var results = this._dataService.GetAllOutSourcerOutPutsForAParticularOutSourcerStore(storeId);
            return MapEFToModel(results);
        }

     
        public long CheckForBuveraInStore(List<Grade> grades, long storeId)
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
        public long SaveOutSourcerOutPut(OutSourcerOutPut outSourcerOutPut, string userId)
        {
            if (outSourcerOutPut.Approved == null)
            {
                long outSourcerOutPutId = 0;

                      var outSourcerOutPutDTO = new DTO.OutSourcerOutPutDTO()
                        {
                            StoreId = outSourcerOutPut.StoreId,
                            TotalAmount = outSourcerOutPut.TotalAmount,
                            TotalQuantity = outSourcerOutPut.TotalQuantity,
                             Price = outSourcerOutPut.Price,
                             PersonLoaded = outSourcerOutPut.PersonLoaded,
                            Deleted = outSourcerOutPut.Deleted,
                            CreatedBy = outSourcerOutPut.CreatedBy,
                            Approved = outSourcerOutPut.Approved,
                            CreatedOn = outSourcerOutPut.CreatedOn
                        };

                        outSourcerOutPutId = this._dataService.SaveOutSourcerOutPut(outSourcerOutPutDTO, userId);
         

                     if (outSourcerOutPut.Grades.Any())
                            {
                                List<OutSourcerOutPutGradeSize> outSourcerOutPutGradeSizeList = new List<OutSourcerOutPutGradeSize>();

                                foreach (var grade in outSourcerOutPut.Grades)
                                {
                                    var gradeId = grade.GradeId;
                                    if (grade.Denominations != null)
                                    {
                                        if (grade.Denominations.Any())
                                        {
                                            foreach (var denomination in grade.Denominations)
                                            {
                                                var outSourcerOutPutGradeSize = new OutSourcerOutPutGradeSize()
                                                {
                                                    GradeId = gradeId,
                                                    SizeId = denomination.DenominationId,
                                                    OutSourcerOutPutId = outSourcerOutPutId,
                                                    //Price = denomination.Price,
                                                    Price = denomination.Value * outSourcerOutPut.Price,
                                                    Quantity = denomination.Quantity,
                                                    Amount = denomination.Quantity * denomination.Value *outSourcerOutPut.Price,
                                                   // Amount = denomination.Quantity * denomination.Price,
                                                    TimeStamp = DateTime.Now
                                                };
                                                outSourcerOutPutGradeSizeList.Add(outSourcerOutPutGradeSize);


                                                //var inOrOut = false;
                                                ////method that updates flour in store
                                                //var storeGradeSize = new StoreGradeSize()
                                                //{

                                                //    GradeId = gradeId,
                                                //    SizeId = denomination.DenominationId,
                                                //    StoreId = outSourcerOutPut.StoreId,
                                                //    Quantity = denomination.Quantity,
                                                //    TimeStamp = DateTime.Now
                                                //};
                                                //this._stockService.SaveStoreGradeSize(storeGradeSize, true);
                                                ////Method that updates buvera into storeBuveraGradeSize table(storeBuvera stock)
                                                //var storeBuveraGradeSize = new StoreBuveraGradeSize()
                                                //{
                                                //    StoreId = outSourcerOutPut.StoreId,
                                                //    GradeId = outSourcerOutPutGradeSize.GradeId,
                                                //    SizeId = outSourcerOutPutGradeSize.SizeId,
                                                //    Quantity = outSourcerOutPutGradeSize.Quantity,
                                                //};

                                                //this._buveraService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, inOrOut);
                                                ////totalBuveraQuantity = denomination.Quantity + totalBuveraQuantity;
                                            }
                                        }
                                    }
                                }

                                this._dataService.PurgeOutSourcerOutPutGradeSize(outSourcerOutPutId);
                                this.SaveOutSourcerOutPutGradeSizeList(outSourcerOutPutGradeSizeList);

                            }



                        return outSourcerOutPutId;
             }
                           
                      
            else
            {
                if (outSourcerOutPut.CreatedById == userId)
                {
                    long outSourcerOutPutId = -22;
                    return outSourcerOutPutId;
                }
                else
                {
                    //update output with either approved == true or rejected == true
                    long outSourcerOutPutId = 0;
                    var store = this._storeService.GetStore(outSourcerOutPut.StoreId);
                if (Convert.ToBoolean(outSourcerOutPut.Approved))
                {
                    if (outSourcerOutPut.Grades != null)
                    {
                        var gradeStore = CheckForBuveraInStore(outSourcerOutPut.Grades, outSourcerOutPut.StoreId);
                        if (gradeStore == -2)
                        {
                            outSourcerOutPutId = gradeStore;
                            return outSourcerOutPutId;
                        }
                        else if (gradeStore == -1)
                        {
                            outSourcerOutPutId = gradeStore;
                            return outSourcerOutPutId;
                        }
                        if (outSourcerOutPut.Grades.Any())
                                    {
                                        List<OutSourcerOutPutGradeSize> outSourcerOutPutGradeSizeList = new List<OutSourcerOutPutGradeSize>();

                                        foreach (var grade in outSourcerOutPut.Grades)
                                        {
                                            var gradeId = grade.GradeId;
                                            if (grade.Denominations != null)
                                            {
                                                if (grade.Denominations.Any())
                                                {
                                                    foreach (var denomination in grade.Denominations)
                                                    {
                                                        

                                                        var inOrOut = false;
                                                        //method that updates flour in store
                                                        var storeGradeSize = new StoreGradeSize()
                                                        {

                                                            GradeId = gradeId,
                                                            SizeId = denomination.DenominationId,
                                                            StoreId = outSourcerOutPut.StoreId,
                                                            Quantity = denomination.Quantity,
                                                            TimeStamp = DateTime.Now
                                                        };
                                                        this._stockService.SaveStoreGradeSize(storeGradeSize, true);
                                                        //Method that updates buvera into storeBuveraGradeSize table(storeBuvera stock)
                                                        var storeBuveraGradeSize = new StoreBuveraGradeSize()
                                                        {
                                                            StoreId = outSourcerOutPut.StoreId,
                                                            GradeId = gradeId,
                                                            SizeId = denomination.DenominationId,
                                                            Quantity = denomination.Quantity,
                                                        };

                                                        this._buveraService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, inOrOut);
                                                
                                                    }
                                                }
                                            }
                                        }

                                        this._dataService.PurgeOutSourcerOutPutGradeSize(outSourcerOutPutId);
                                        this.SaveOutSourcerOutPutGradeSizeList(outSourcerOutPutGradeSizeList);

                                    }
                         outSourcerOutPutId = _dataService.UpdateOutPutOnApprovalOrRejection(outSourcerOutPut.OutSourcerOutPutId, Convert.ToBoolean(outSourcerOutPut.Approved), userId);

                        #region transactions

                        long transactionSubTypeId = 0;
                        var notes = string.Empty;

                        transactionSubTypeId = flourTransactionSubTypeId;
                        notes = "Maize Flour of " + outSourcerOutPut.TotalQuantity.ToString() + " kgs"+ " at "+ outSourcerOutPut.Price +" perkg" + " Loaded to " + outSourcerOutPut.PersonLoaded;


                        var accountActivity = new AccountTransactionActivity()
                        {

                            AspNetUserId = store.OutSourcerId,
                            Amount = outSourcerOutPut.TotalAmount,
                            Notes = notes,
                            Action = "+",
                            Price = outSourcerOutPut.Price,
                            BranchId = store.BranchId,
                            TransactionSubTypeId = transactionSubTypeId,
                            SectorId = 1,
                            Deleted = outSourcerOutPut.Deleted,
                            CreatedBy = userId,

                            Quantity = outSourcerOutPut.TotalQuantity,
                            // WeightNote = Convert.ToString(delivery.DocumentId),



                        };
                        var accountActivityId = this._accountTransactionActivityService.SaveAccountTransactionActivity(accountActivity, userId);


                        return outSourcerOutPutId;





                        #endregion

                    }

                    return outSourcerOutPut.OutSourcerOutPutId;
                }
                    else
                    {
                        if (outSourcerOutPut.Grades != null)
                        {
                            
                            if (outSourcerOutPut.Grades.Any())
                            {
                                List<OutSourcerOutPutGradeSize> outSourcerOutPutGradeSizeList = new List<OutSourcerOutPutGradeSize>();

                                foreach (var grade in outSourcerOutPut.Grades)
                                {
                                    var gradeId = grade.GradeId;
                                    if (grade.Denominations != null)
                                    {
                                        if (grade.Denominations.Any())
                                        {
                                            foreach (var denomination in grade.Denominations)
                                            {
                                                //Method that updates buvera into storeBuveraGradeSize table(storeBuvera stock)
                                                var storeBuveraGradeSize = new StoreBuveraGradeSize()
                                                {
                                                    StoreId = outSourcerOutPut.StoreId,
                                                    GradeId = gradeId,
                                                    SizeId = denomination.DenominationId,
                                                    Quantity = denomination.Quantity,
                                                };

                                                this._buveraService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, true);

                                            }
                                        }
                                    }
                                }

                                
                            }
                            outSourcerOutPutId = _dataService.UpdateOutPutOnApprovalOrRejection(outSourcerOutPut.OutSourcerOutPutId, Convert.ToBoolean(outSourcerOutPut.Approved), userId);

                           

                        }

                       
                        return outSourcerOutPut.OutSourcerOutPutId;
                    }
       
                }
            }

        }

        public IEnumerable<OutSourcerOutPut> GetAllUnApprovedOutSourcerOutPuts()
        {
            var results = this._dataService.GetAllUnApprovedOutSourcerOutPuts();
            return MapEFToModel(results);
        }
        public IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPuts()
        {
            var results = this._dataService.GetAllApprovedOutSourcerOutPuts();
            return MapEFToModel(results);
        }
        public IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore(long storeId)
        {
            var results = this._dataService.GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore(storeId);
            return MapEFToModel(results);
        }

        void SaveOutSourcerOutPutGradeSizeList(List<OutSourcerOutPutGradeSize> outSourcerOutPutGradeSizeList)
        {
            if (outSourcerOutPutGradeSizeList != null)
            {
                if (outSourcerOutPutGradeSizeList.Any())
                {
                    foreach (var outSourcerOutPutGradeSize in outSourcerOutPutGradeSizeList)
                    {
                        var outSourcerOutPutGradeSizeDTO = new DTO.OutSourcerOutPutGradeSizeDTO()
                        {
                            OutSourcerOutPutId = outSourcerOutPutGradeSize.OutSourcerOutPutId,
                            GradeId = outSourcerOutPutGradeSize.GradeId,
                            Quantity = outSourcerOutPutGradeSize.Quantity,
                            SizeId = outSourcerOutPutGradeSize.SizeId,
                            Price = outSourcerOutPutGradeSize.Price,
                            Amount = outSourcerOutPutGradeSize.Amount,
                            TimeStamp = outSourcerOutPutGradeSize.TimeStamp
                        };
                        this.SaveOutSourcerOutPutGradeSize(outSourcerOutPutGradeSizeDTO);
                    }
                }
            }
        }

        void SaveOutSourcerOutPutGradeSize(OutSourcerOutPutGradeSizeDTO outSourcerOutPutGradeSizeDTO)
        {
            _dataService.SaveOutSourcerOutPutGradeSize(outSourcerOutPutGradeSizeDTO);
        }

                
        public void MarkAsDeleted(long outSourcerOutPutId, string userId)
        {
            _dataService.MarkAsDeleted(outSourcerOutPutId, userId);
        }

       
        #region Mapping Methods

        public IEnumerable<OutSourcerOutPut> MapEFToModel(IEnumerable<EF.Models.OutSourcerOutPut> data)
        {
            var list = new List<OutSourcerOutPut>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        
        public OutSourcerOutPut MapEFToModel(EF.Models.OutSourcerOutPut data)
        {
            if (data != null)
            {
                var outSourcerOutPut = new OutSourcerOutPut()
                {
                    OutSourcerOutPutId = data.OutSourcerOutPutId,
                    StoreId = data.StoreId,
                    TotalQuantity = data.TotalQuantity,
                    TotalAmount = data.TotalAmount,
                    PersonLoaded = data.PersonLoaded,
                    Approved = data.Approved,
                    Price = data.Price,
                     CreatedOn = data.CreatedOn,
                     CreatedById = data.CreatedBy,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),
                    StoreName = data.Store != null ? data.Store.Name : "",
                };

                List<Grade> grades = new List<Grade>();
                List<Grade> availableGrades = new List<Grade>();
                if (data.OutSourcerOutPutGradeSizes != null)
                {
                    if (data.OutSourcerOutPutGradeSizes.Any())
                    {

                        // var distinctGrades = data.BatchGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        var distinctGrades = data.OutSourcerOutPutGradeSizes.Where(a => a.OutSourcerOutPutId == data.OutSourcerOutPutId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        foreach (var outSourcerOutPutGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = outSourcerOutPutGradeSize.Grade.GradeId,
                                Value = outSourcerOutPutGradeSize.Grade.Value,
                                
                                CreatedOn = outSourcerOutPutGradeSize.Grade.CreatedOn,
                                TimeStamp = outSourcerOutPutGradeSize.Grade.TimeStamp,
                                Deleted = outSourcerOutPutGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(outSourcerOutPutGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(outSourcerOutPutGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (outSourcerOutPutGradeSize.Grade.OutSourcerOutPutGradeSizes != null)
                            {
                                if (outSourcerOutPutGradeSize.Grade.OutSourcerOutPutGradeSizes.Any())
                                {


                                    var distinctSizes = outSourcerOutPutGradeSize.Grade.OutSourcerOutPutGradeSizes.Where(a => a.OutSourcerOutPutId == data.OutSourcerOutPutId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();


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
                                       // outSourcerOutPut.TotalQuantity += (ogs.Quantity * ogs.Size.Value);

                                        //batchOutPut.TotalBuveraCost += denomination.Amount;
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                           // availableGrades.Add(grade);
                        }

                    }

                }
                outSourcerOutPut.Grades = grades;

               

                return outSourcerOutPut;
            }
            return null;

        }



        #endregion
    }
}
