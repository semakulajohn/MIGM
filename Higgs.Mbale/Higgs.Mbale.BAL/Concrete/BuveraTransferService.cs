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
 public   class BuveraTransferService : IBuveraTransferService
    {
        
      ILog logger = log4net.LogManager.GetLogger(typeof(BuveraTransferService));
        private IBuveraTransferDataService _dataService;
        private IUserService _userService;
        private IGradeService _gradeService;
        private IStoreService _storeService;
        private IBuveraService _buveraService;


        public BuveraTransferService(IBuveraTransferDataService dataService, IUserService userService, IGradeService gradeService,
            IStoreService storeService,IBuveraService buveraService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._gradeService = gradeService;
            this._storeService = storeService;
            this._buveraService = buveraService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BuveraTransferId"></param>
        /// <returns></returns>
        public BuveraTransfer GetBuveraTransfer(long buveraTransferId)
        {
            var result = this._dataService.GetBuveraTransfer(buveraTransferId);
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
        public IEnumerable<BuveraTransfer> GetAllBuveraTransfers()
        {
            var results = this._dataService.GetAllBuveraTransfers();
            return MapEFToModel(results);
        }

        public IEnumerable<BuveraTransfer> GetAllBuveraTransfersForAParticularStore(long storeId)
        {
            var results = this._dataService.GetAllBuveraTransfersForAParticularStore(storeId);
            return MapEFToModel(results);
        }

        public long RejectBuvera(BuveraTransfer buveraTransfer, string userId)
        {
            bool inOrOut = false;

            var buveraTransferDTO = new DTO.BuveraTransferDTO()
            {
                BuveraTransferId = buveraTransfer.BuveraTransferId,
                TotalQuantity = buveraTransfer.TotalQuantity,
                BranchId = buveraTransfer.BranchId,
                FromSupplierStoreId = buveraTransfer.FromSupplierStoreId,
                ToReceiverStoreId = buveraTransfer.ToReceiverStoreId,
                Accept = buveraTransfer.Accept,
                Reject = true,
             
                StoreId = buveraTransfer.StoreId,
                Deleted = buveraTransfer.Deleted,
                CreatedBy = buveraTransfer.CreatedBy,
                CreatedOn = buveraTransfer.CreatedOn
            };

            var buveraTransferId = this._dataService.SaveBuveraTransfer(buveraTransferDTO, userId);
            if (buveraTransfer.Grades != null)
            {
                if (buveraTransfer.Grades.Any())
                {
                    List<BuveraTransferGradeSize> buveraTransferGradeSizeList = new List<BuveraTransferGradeSize>();

                    foreach (var grade in buveraTransfer.Grades)
                    {
                        var gradeId = grade.GradeId;
                        if (grade.Denominations != null)
                        {
                            if (grade.Denominations.Any())
                            {
                                foreach (var denomination in grade.Denominations)
                                {
                                    inOrOut = true;
                                    //Method that adds BuveraTransfer into receiver storeBuveraTransferGradeSize table(storeBuveraTransfer stock)
                                    var storeBuveraTransferGradeSize = new StoreBuveraTransferGradeSizeDTO()
                                    {
                                        StoreId = buveraTransfer.FromSupplierStoreId,
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        Quantity = denomination.Quantity,
                                    };

                                    //storeBuveraTransfer = this._dataService.SaveStoreGradeSize(storeBuveraTransferGradeSize, inOrOut);
                                    this._dataService.SaveStoreBuveraTransferGradeSize(storeBuveraTransferGradeSize, inOrOut);


                                }
                            }
                        }
                    }

                }
            }
            return buveraTransferId;

        }


        public long AcceptBuvera(BuveraTransfer buveraTransfer, string userId)
        {
            bool inOrOut = false;
            
            var buveraTransferDTO = new DTO.BuveraTransferDTO()
            {
                BuveraTransferId = buveraTransfer.BuveraTransferId,
                TotalQuantity = buveraTransfer.TotalQuantity,
                BranchId = buveraTransfer.BranchId,
                FromSupplierStoreId = buveraTransfer.FromSupplierStoreId,
                ToReceiverStoreId = buveraTransfer.ToReceiverStoreId,
                Accept = true,
                Reject = buveraTransfer.Reject,
                ProductId = buveraTransfer.ProductId,
                StoreId = buveraTransfer.StoreId,
                Deleted = buveraTransfer.Deleted,
                CreatedBy = buveraTransfer.CreatedBy,
                CreatedOn = buveraTransfer.CreatedOn
            };

            var buveraTransferId = this._dataService.SaveBuveraTransfer(buveraTransferDTO, userId);
            if (buveraTransfer.Grades != null)
            {
                if (buveraTransfer.Grades.Any())
                {
                    List<BuveraTransferGradeSize> buveraTransferGradeSizeList = new List<BuveraTransferGradeSize>();

                    foreach (var grade in buveraTransfer.Grades)
                    {
                        var gradeId = grade.GradeId;
                        if (grade.Denominations != null)
                        {
                            if (grade.Denominations.Any())
                            {
                                foreach (var denomination in grade.Denominations)
                                {
                                    inOrOut = true;
                                    //Method that adds BuveraTransfer into receiver storeBuveraTransferGradeSize table(storeBuveraTransfer stock)
                                    var storeBuveraTransferGradeSize = new StoreBuveraTransferGradeSizeDTO()
                                    {
                                        StoreId = buveraTransfer.ToReceiverStoreId,
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        Quantity = denomination.Quantity,
                                    };

                                    //storeBuveraTransfer = this._dataService.SaveStoreGradeSize(storeBuveraTransferGradeSize, inOrOut);
                                    this._dataService.SaveStoreBuveraTransferGradeSize(storeBuveraTransferGradeSize, inOrOut);

                                   
                                }
                            }
                        }
                    }
                
                }
            }
            return buveraTransferId;

        }

        public long IssueBuveraTransfer(BuveraTransfer buveraTransfer, string userId)
        {
            //bool inOrOut = false;
                     
            var buveraTransferDTO = new DTO.BuveraTransferDTO()
            {
                BuveraTransferId = buveraTransfer.BuveraTransferId,
                TotalQuantity = buveraTransfer.TotalQuantity,
                BranchId = buveraTransfer.BranchId,
                FromSupplierStoreId = buveraTransfer.FromSupplierStoreId,
                ToReceiverStoreId = buveraTransfer.ToReceiverStoreId,
                Accept = buveraTransfer.Accept,
                Reject = buveraTransfer.Reject,
                StoreId = buveraTransfer.StoreId,
                ProductId = buveraTransfer.ProductId,
                Deleted = buveraTransfer.Deleted,
                CreatedBy = buveraTransfer.CreatedBy,
                CreatedOn = buveraTransfer.CreatedOn
            };

            var buveraTransferId = this._dataService.SaveBuveraTransfer(buveraTransferDTO, userId);
            if (buveraTransfer.Grades != null)
            {
                if (buveraTransfer.Grades.Any())
                {
                    List<BuveraTransferGradeSize> buveraTransferGradeSizeList = new List<BuveraTransferGradeSize>();

                    foreach (var grade in buveraTransfer.Grades)
                    {
                        var gradeId = grade.GradeId;
                        if (grade.Denominations != null)
                        {
                            if (grade.Denominations.Any())
                            {
                                foreach (var denomination in grade.Denominations)
                                {
                                    var sizeRate = GetRateOfAParticularSize(denomination.DenominationId);

                                    var buveraTransferGradeSize = new BuveraTransferGradeSize()
                                    {
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        BuveraTransferId = buveraTransferId,
                                        StoreId = buveraTransfer.ToReceiverStoreId,
                                        //Amount = Convert.ToDouble(denomination.Quantity * sizeRate),
                                        Rate = sizeRate,
                                        Quantity = denomination.Quantity,
                                        TimeStamp = DateTime.Now
                                    };
                                    buveraTransferGradeSizeList.Add(buveraTransferGradeSize);

                                    //Method that updates From store Buvera into storeGradeSize table(storeBuveraTransfer BuveraTransfer)
                                    var fromStoreBuveraTransferGradeSize = new StoreBuveraTransferGradeSizeDTO()
                                    {
                                        StoreId = buveraTransfer.FromSupplierStoreId,
                                        GradeId = buveraTransferGradeSize.GradeId,
                                        SizeId = buveraTransferGradeSize.SizeId,
                                        Quantity = buveraTransferGradeSize.Quantity,
                                    };

                                    this._dataService.SaveStoreBuveraTransferGradeSize(fromStoreBuveraTransferGradeSize, false);
                                }
                            }
                        }
                    }
                    this._dataService.PurgeBuveraTransferGradeSize(buveraTransferId);
                    this.SaveBuveraTransferGradeSizeList(buveraTransferGradeSizeList);
                }
            }
            return buveraTransferId;

        }


        public long SaveBuveraTransfer(BuveraTransfer buveraTransfer, string userId)
        {
            
             long issueId = 0;
            if (buveraTransfer.Issuing == "YES")
            {
                var buveraTransferObject = new BuveraTransfer()
                {

                    BuveraTransferId = buveraTransfer.BuveraTransferId,
                 FromSupplierStoreId = buveraTransfer.FromSupplierStoreId,
                    TotalQuantity = buveraTransfer.TotalQuantity,
                    BranchId = buveraTransfer.BranchId,
                    ToReceiverStoreId = buveraTransfer.ToReceiverStoreId,
                    StoreId = buveraTransfer.StoreId,
                    Accept = buveraTransfer.Accept,
                    Reject = buveraTransfer.Reject,
                    Deleted = buveraTransfer.Deleted,
                    CreatedBy = buveraTransfer.CreatedBy,
                    CreatedOn = buveraTransfer.CreatedOn,
                    Grades = buveraTransfer.Grades,
                    ProductId = buveraTransfer.ProductId,
                };

                 issueId = IssueBuveraTransfer(buveraTransferObject, userId);

                return issueId;
            }
            return issueId;
        }


        public StoreGrade GetStoreBuveraTransferStock(long storeId)
        {
            var result = this._dataService.GetStoreBuveraTransferStock(storeId);
            var storeGrade = GetStoreBuveraTransferStockForView(MapEFToModel(result));
            return storeGrade;
        }
        public StoreGrade GetStoreBuveraTransferStockForView(IEnumerable<Models.StoreBuveraTransferGradeSize> list)
        {
            var storeGrade = new StoreGrade()
            {
                StoreBuveraTransferGradeSizes = list,
            };


            return storeGrade;
        }

        //public long CheckForBuveraInStore(List<Grade> grades, long storeId)
        //{
        //    var buveraStock = _buveraService.GetStoreBuveraStock(storeId);
        //    foreach (var grade in grades)
        //    {

        //        if (grade.Denominations != null)
        //        {
        //            foreach (var denomination in grade.Denominations)
        //            {
        //                var result = buveraStock.StoreSizeGrades..Contains(grade);
        //                if (result == null)
        //                {
        //                    //grade doesn't exist in store
        //                    return -2;
        //                }
        //                else
        //                {
        //                    if (result.Quantity < denomination.Quantity)
        //                    {
        //                        //quantity is greater than what you have in store
        //                        return -1;
        //                    }
        //                }
        //            }
        //        }


        //    }
        //    return 1;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="BuveraTransferId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long buveraTransferId, string userId)
        {
            _dataService.MarkAsDeleted(buveraTransferId, userId);
        }

        void SaveBuveraTransferGradeSizeList(List<BuveraTransferGradeSize> buveraTransferGradeSizeList)
        {
            if (buveraTransferGradeSizeList != null)
            {
                if (buveraTransferGradeSizeList.Any())
                {
                    foreach (var buveraTransferGradeSize in buveraTransferGradeSizeList)
                    {
                        var buveraTransferGradeSizeDTO = new DTO.BuveraTransferGradeSizeDTO()
                        {
                            BuveraTransferId = buveraTransferGradeSize.BuveraTransferId,
                            GradeId = buveraTransferGradeSize.GradeId,
                            StoreId = buveraTransferGradeSize.StoreId,
                            Quantity = buveraTransferGradeSize.Quantity,
                            SizeId = buveraTransferGradeSize.SizeId,
                            Rate = buveraTransferGradeSize.Rate,
                            Amount = buveraTransferGradeSize.Amount,
                            TimeStamp = buveraTransferGradeSize.TimeStamp
                        };
                        this.SaveBuveraTransferGradeSize(buveraTransferGradeSizeDTO);

                    }
                }
            }
        }
        void SaveBuveraTransferGradeSize(BuveraTransferGradeSizeDTO buveraTransferGradeSizeDTO)
        {
            _dataService.SaveBuveraTransferGradeSize(buveraTransferGradeSizeDTO);
        }
      
        #region Mapping Methods

        public IEnumerable<BuveraTransfer> MapEFToModel(IEnumerable<EF.Models.BuveraTransfer> data)
        {
            var list = new List<BuveraTransfer>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps BuveraTransfer EF object to BuveraTransfer Model Object and
        /// returns the BuveraTransfer model object.
        /// </summary>
        /// <param name="result">EF BuveraTransfer object to be mapped.</param>
        /// <returns>BuveraTransfer Model Object.</returns>
        public BuveraTransfer MapEFToModel(EF.Models.BuveraTransfer data)
        {
            if (data != null)
            {


                var buveraTransfer = new BuveraTransfer()
                {


                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    BranchId = data.BranchId,
                    TotalQuantity = data.TotalQuantity,
                    StoreId = data.StoreId,
                    BuveraTransferId = data.BuveraTransferId,
                    FromSupplierStoreId = data.FromSupplierStoreId,
                    Accept = data.Accept,
                    Reject = data.Reject,
                    ProductId = data.ProductId,
                    ToReceiverStoreId = data.ToReceiverStoreId,
                    ReceiverStoreName = data.Store2 != null ? data.Store2.Name : "",
                    SupplierStoreName = data.Store1 != null ? data.Store1.Name : "",
                    ProductName = data.Product != null? data.Product.Name : "",
                    StoreName = data.Store != null ? data.Store.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser2)
                };

                if (data.BuveraTransferGradeSizes != null)
                {
                    if (data.BuveraTransferGradeSizes.Any())
                    {
                        List<Grade> grades = new List<Grade>();
                       // var distinctGrades = data.BuveraTransferGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        var distinctGrades = data.BuveraTransferGradeSizes.Where(a => a.BuveraTransferId == data.BuveraTransferId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        foreach (var buveraTransferGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = buveraTransferGradeSize.Grade.GradeId,
                                Value = buveraTransferGradeSize.Grade.Value,
                                CreatedOn = buveraTransferGradeSize.Grade.CreatedOn,
                                TimeStamp = buveraTransferGradeSize.Grade.TimeStamp,
                                Deleted = buveraTransferGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(buveraTransferGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(buveraTransferGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (buveraTransferGradeSize.Grade.BuveraTransferGradeSizes != null)
                            {
                                if (buveraTransferGradeSize.Grade.BuveraTransferGradeSizes.Any())
                                {
                                      var distinctSizes = buveraTransferGradeSize.Grade.BuveraTransferGradeSizes.Where(a => a.BuveraTransferId == data.BuveraTransferId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();

                                   
                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity
                                        };
                                        //  BuveraTransfer.TotalQuantity += (ogs.Quantity * ogs.Size.Value);
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                        }
                        buveraTransfer.Grades = grades;
                    }
                }

                return buveraTransfer;
            }
            return null;
        }


        public StoreBuveraTransferGradeSize MapEFToModel(EF.Models.StoreBuveraTransferGradeSize data)
        {
            var storeBuveraTransferGradeSize = new StoreBuveraTransferGradeSize()
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
            return storeBuveraTransferGradeSize;

        }


        private IEnumerable<StoreBuveraTransferGradeSize> MapEFToModel(IEnumerable<EF.Models.StoreBuveraTransferGradeSize> data)
        {
            var list = new List<StoreBuveraTransferGradeSize>();

            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }

            return list;
        }

       #endregion
    }
}
