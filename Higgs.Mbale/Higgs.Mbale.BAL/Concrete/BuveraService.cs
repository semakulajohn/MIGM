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
 public   class BuveraService : IBuveraService
    {
       
        ILog logger = log4net.LogManager.GetLogger(typeof(BuveraService));
        private IBuveraDataService _dataService;
        private IUserService _userService;
        private IGradeService _gradeService;
        private IStoreService _storeService;
        

        public BuveraService(IBuveraDataService dataService,IUserService userService, IGradeService gradeService,IStoreService storeService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._gradeService = gradeService;
            this._storeService = storeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BuveraId"></param>
        /// <returns></returns>
        public Buvera GetBuvera(long buveraId)
        {
            var result = this._dataService.GetBuvera(buveraId);
            return MapEFToModel(result);
        }

        private double GetRateOfAParticularSize(long sizeId)
        {
            double rate = 0;
            var size = this._gradeService.GetSize(sizeId);
            if (size != null)
            {
                rate =Convert.ToDouble(size.Rate);
                //return rate;
            }
            return rate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Buvera> GetAllBuveras()
        {
            var results = this._dataService.GetAllBuveras();
            return MapEFToModel(results);
        }

        public IEnumerable<BuveraCategory> GetAllBuveraCategories()
        {
            var results = this._dataService.GetAllBuveraCategories();
            return MapEFToModel(results);
        }
        public IEnumerable<Buvera> GetAllBuverasForAParticularStore(long storeId)
        {
            var results = this._dataService.GetAllBuverasForAParticularStore(storeId);
            return MapEFToModel(results);
        }

        public IEnumerable<Buvera> GetAllDamagedBuverasForAparticularStore(long storeId,long buveraCategoryId)
        {
            var results = this._dataService.GetAllDamagedBuverasForAparticularStore(storeId,buveraCategoryId);
            return MapEFToModel(results);
        }
        private string GetStoreName(long storeId)
        {
            var storeName = string.Empty;
           var store =  _storeService.GetStore(storeId);
           if (store != null)
           {
               storeName = store.Name;
           }
          return storeName;
        }


        public long RejectBuvera(Buvera buvera, string userId)
        {
            bool inOrOut = false;

            var buveraDTO = new DTO.BuveraDTO()
            {
                BuveraId = buvera.BuveraId,
                TotalCost = buvera.TotalCost,
                TotalQuantity = buvera.TotalQuantity,
                BranchId = buvera.BranchId,
                FromSupplier = buvera.FromSupplier,
                InvoiceNumber = buvera.InvoiceNumber,
                BuveraCategoryId = buvera.BuveraCategoryId,
                ToReceiver = buvera.ToReceiver,
                StoreId = Convert.ToInt64(buvera.ToReceiver),
                Deleted = buvera.Deleted,
                CreatedBy = buvera.CreatedBy,
                CreatedOn = buvera.CreatedOn,
                Approved = buvera.Approved,
                Rejected = buvera.Rejected,
            };

            var buveraId = this._dataService.SaveBuvera(buveraDTO, userId);
            if (buvera.Grades != null)
            {
                if (buvera.Grades.Any())
                {
                    List<BuveraGradeSize> buveraGradeSizeList = new List<BuveraGradeSize>();

                    foreach (var grade in buvera.Grades)
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
                                    var storeBuveraGradeSize = new StoreBuveraGradeSizeDTO()
                                    {
                                        StoreId = buvera.StoreId,
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        Quantity = denomination.Quantity,
                                    };

                                    //storeBuveraTransfer = this._dataService.SaveStoreGradeSize(storeBuveraTransferGradeSize, inOrOut);
                                    this._dataService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, inOrOut);


                                }
                            }
                        }
                    }

                }
            }
            return buveraId;

        }


        public long AcceptBuvera(Buvera buvera, string userId)
        {
           
            var buveraDTO = new DTO.BuveraDTO()
            {
                BuveraId = buvera.BuveraId,
                TotalQuantity = buvera.TotalQuantity,
                BranchId = buvera.BranchId,
                Approved = true,
                Rejected = buvera.Rejected,
                BuveraCategoryId = buvera.BuveraCategoryId,
                StoreId = buvera.StoreId,
                Deleted = buvera.Deleted,
                CreatedBy = buvera.CreatedBy,
                CreatedOn = buvera.CreatedOn,
                ProductId = buvera.ProductId,
                FromSupplier = buvera.FromSupplier,
                InvoiceNumber = buvera.InvoiceNumber,
              
                ToReceiver = buvera.ToReceiver,
              
            };

            var buveraId = this._dataService.SaveBuvera(buveraDTO, userId);
            
            return buveraId;

        }

        public long IssueBuvera(Buvera buvera, string userId)
        {
            bool inOrOut = false;
           // var storeBuvera = 0;  

            var fromSupplierStore = GetStoreName(buvera.StoreId);
            var toReceiverStore = GetStoreName(Convert.ToInt64(buvera.ToReceiver));

            var buveraDTO = new DTO.BuveraDTO()
            {
                BuveraId = buvera.BuveraId,
                TotalCost = buvera.TotalCost,
                TotalQuantity = buvera.TotalQuantity,
                BranchId = buvera.BranchId,
                ProductId = buvera.ProductId,
                FromSupplier = fromSupplierStore,
                InvoiceNumber = buvera.InvoiceNumber,
                BuveraCategoryId = buvera.BuveraCategoryId,
                ToReceiver = toReceiverStore,
                StoreId = Convert.ToInt64(buvera.ToReceiver),
                Deleted = buvera.Deleted,
                CreatedBy = buvera.CreatedBy,
                CreatedOn = buvera.CreatedOn,
                Approved = buvera.Approved,
                Rejected = buvera.Rejected,
            };

            var buveraId = this._dataService.SaveBuvera(buveraDTO, userId);
            if (buvera.Grades != null)
            {
                if (buvera.Grades.Any())
                {
                    List<BuveraGradeSize> buveraGradeSizeList = new List<BuveraGradeSize>();

                    foreach (var grade in buvera.Grades)
                    {
                        var gradeId = grade.GradeId;
                        if (grade.Denominations != null)
                        {
                            if (grade.Denominations.Any())
                            {
                                foreach (var denomination in grade.Denominations)
                                {
                                    var sizeRate = GetRateOfAParticularSize(denomination.DenominationId);

                                    var buveraGradeSize = new BuveraGradeSize()
                                    {
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        BuveraId = buveraId,
                                        StoreId = Convert.ToInt64(buvera.ToReceiver),
                                        Amount = Convert.ToDouble(denomination.Quantity * sizeRate),
                                        Rate = sizeRate,
                                        Quantity = denomination.Quantity,
                                        TimeStamp = DateTime.Now
                                    };
                                    buveraGradeSizeList.Add(buveraGradeSize);

                                    inOrOut = true;
                                    //Method that adds buvera into storeBuveraGradeSize table(storeBuvera stock)
                                    var storeBuveraGradeSize = new StoreBuveraGradeSizeDTO()
                                    {
                                        StoreId = Convert.ToInt64(buvera.ToReceiver),
                                        GradeId = buveraGradeSize.GradeId,
                                        SizeId = buveraGradeSize.SizeId,
                                        Quantity = buveraGradeSize.Quantity,
                                    };

                                this._dataService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, inOrOut);

                                    //Method that updates buvera into storeBuveraGradeSize table(storeBuvera stock)
                                    var fromStoreBuveraGradeSize = new StoreBuveraGradeSizeDTO()
                                    {
                                        StoreId = buvera.StoreId,
                                        GradeId = buveraGradeSize.GradeId,
                                        SizeId = buveraGradeSize.SizeId,
                                        Quantity = buveraGradeSize.Quantity,
                                    };

                                   this._dataService.SaveStoreBuveraGradeSize(fromStoreBuveraGradeSize, false);
                                }
                            }
                        }
                    }
                    this._dataService.PurgeBuveraGradeSize(buveraId);
                    this.SaveBuveraGradeSizeList(buveraGradeSizeList);
                }
            }
            return buveraId;

        }

        public StoreBuveraGradeSize GetStoreBuveraGradeSize(long gradeId,long sizeId, long storeId)
        {
            var result = this._dataService.GetStoreBuveraGradeSize(gradeId,sizeId, storeId);
            return MapEFToModel(result);
        }

        public long SaveBuveraOnBatchUsage(Buvera buvera, string userId)
        {
            var buveraDTO = new BuveraDTO()
            {

                TotalQuantity = buvera.TotalQuantity,
                BranchId = buvera.BranchId,
                FromSupplier =buvera.FromSupplier,
                ToReceiver = buvera.ToReceiver,
                StoreId = buvera.StoreId,
                BuveraCategoryId = buvera.BuveraCategoryId,
                ProductId = buvera.ProductId,

            };
            var buveraId = this._dataService.SaveBuvera(buveraDTO, userId);
            return buveraId;
        }
        public long SaveBuvera(Buvera buvera, string userId)
        {
            bool inOrOut = false;
            if (buvera.Issuing == "YES")
            {
                var buveraObject = new Buvera()
                {

                    BuveraId = buvera.BuveraId,
                    TotalCost = buvera.TotalCost,
                    TotalQuantity = buvera.TotalQuantity,
                    ProductId = buvera.ProductId,
                    BranchId = buvera.BranchId,
                    ToReceiver = buvera.ToReceiver,
                    StoreId = buvera.StoreId,
                    BuveraCategoryId = buvera.BuveraCategoryId,
                    Deleted = buvera.Deleted,
                    CreatedBy = buvera.CreatedBy,
                    CreatedOn = buvera.CreatedOn,
                    Grades = buvera.Grades,
                };

                var issueId = IssueBuvera(buveraObject, userId);

                return issueId;
            }
            else
            {
                var buveraDTO = new DTO.BuveraDTO()
                {

                    BuveraId = buvera.BuveraId,
                    TotalCost = buvera.TotalCost,
                    TotalQuantity = buvera.TotalQuantity,
                    BranchId = buvera.BranchId,
                    FromSupplier = buvera.FromSupplier,
                    ToReceiver = buvera.ToReceiver,
                    StoreId = buvera.StoreId,
                    ProductId = buvera.ProductId,
                    BuveraCategoryId = buvera.BuveraCategoryId,
                    InvoiceNumber = buvera.InvoiceNumber,
                    Deleted = buvera.Deleted,
                    CreatedBy = buvera.CreatedBy,
                    CreatedOn = buvera.CreatedOn
                };

                var buveraId = this._dataService.SaveBuvera(buveraDTO, userId);
                if (buvera.Grades != null)
                {
                    if (buvera.Grades.Any())
                    {
                        List<BuveraGradeSize> buveraGradeSizeList = new List<BuveraGradeSize>();

                        foreach (var grade in buvera.Grades)
                        {
                            var gradeId = grade.GradeId;
                            if (grade.Denominations != null)
                            {
                                if (grade.Denominations.Any())
                                {
                                    foreach (var denomination in grade.Denominations)
                                    {
                                        var sizeRate = GetRateOfAParticularSize(denomination.DenominationId);

                                        var buveraGradeSize = new BuveraGradeSize()
                                        {
                                            GradeId = gradeId,
                                            SizeId = denomination.DenominationId,
                                            BuveraId = buveraId,
                                            StoreId = buvera.StoreId,
                                            Amount = Convert.ToDouble(denomination.Quantity * sizeRate),
                                            Rate = sizeRate,
                                            Quantity = denomination.Quantity,
                                            TimeStamp = DateTime.Now
                                        };
                                        buveraGradeSizeList.Add(buveraGradeSize);

                                        inOrOut = true;
                                        //Method that adds buvera into storeBuveraGradeSize table(storeBuvera stock)
                                        var storeBuveraGradeSize = new StoreBuveraGradeSizeDTO()
                                        {
                                            StoreId = buvera.StoreId,
                                            GradeId = buveraGradeSize.GradeId,
                                            SizeId = buveraGradeSize.SizeId,
                                            Quantity = buveraGradeSize.Quantity,
                                        };

                                        this._dataService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, inOrOut);
                                    }
                                }
                            }
                        }
                        this._dataService.PurgeBuveraGradeSize(buveraId);
                        this.SaveBuveraGradeSizeList(buveraGradeSizeList);
                    }
                }
                return buveraId;
            }            
        }

        public void SaveStoreBuveraGradeSize(StoreBuveraGradeSize storeBuveraGradeSize, bool inOrOut)
        {
            var storeBuveraGradeSizeDTO = new StoreBuveraGradeSizeDTO()
            {
                StoreId = storeBuveraGradeSize.StoreId,
                GradeId = storeBuveraGradeSize.GradeId,
                SizeId = storeBuveraGradeSize.SizeId,
                Quantity = storeBuveraGradeSize.Quantity,
            };

            this._dataService.SaveStoreBuveraGradeSize(storeBuveraGradeSizeDTO, inOrOut);
        }

        public StoreGrade GetStoreBuveraStock(long storeId)
        {
            var result = this._dataService.GetStoreBuveraStock(storeId);
            var storeGrade = GetStoreBuveraStockForView(MapEFToModel(result));
            return storeGrade;
        }

        public StoreGrade GetStoreBuveraStockForView(IEnumerable<Models.StoreBuveraGradeSize> list)
        {
            var storeGrade = new StoreGrade()
            {
                StoreBuveraSizeGrades = list,
            };


            return storeGrade;
        }


    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BuveraId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long buveraId, string userId)
        {
            _dataService.MarkAsDeleted(buveraId, userId);
        }

        void SaveBuveraGradeSizeList(List<BuveraGradeSize> buveraGradeSizeList)
        {
            if (buveraGradeSizeList != null)
            {
                if (buveraGradeSizeList.Any())
                {
                    foreach (var buveraGradeSize in buveraGradeSizeList)
                    {
                        var buveraGradeSizeDTO = new DTO.BuveraGradeSizeDTO()
                        {
                            BuveraId = buveraGradeSize.BuveraId,
                            GradeId = buveraGradeSize.GradeId,
                            StoreId = buveraGradeSize.StoreId,
                            Quantity = buveraGradeSize.Quantity,
                            SizeId = buveraGradeSize.SizeId,
                            Rate = buveraGradeSize.Rate,
                            Amount = buveraGradeSize.Amount,
                            TimeStamp = buveraGradeSize.TimeStamp
                        };
                        this.SaveBuveraGradeSize(buveraGradeSizeDTO);

                    }
                }
            }
        }
        void SaveBuveraGradeSize(BuveraGradeSizeDTO buveraGradeSizeDTO)
        {
            _dataService.SaveBuveraGradeSize(buveraGradeSizeDTO);
        }
      
        #region Mapping Methods

        public IEnumerable<Buvera> MapEFToModel(IEnumerable<EF.Models.Buvera> data)
        {
            var list = new List<Buvera>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Buvera EF object to Buvera Model Object and
        /// returns the Buvera model object.
        /// </summary>
        /// <param name="result">EF Buvera object to be mapped.</param>
        /// <returns>Buvera Model Object.</returns>
        public Buvera MapEFToModel(EF.Models.Buvera data)
        {
            if (data != null)
            {
                var buvera = new Buvera()
                {

                    TotalCost = data.TotalCost,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    BranchId = data.BranchId,
                    TotalQuantity = data.TotalQuantity,
                    StoreId = data.StoreId,
                    BuveraId = data.BuveraId,
                    FromSupplier = data.FromSupplier,
                    ToReceiver = data.ToReceiver,
                    ProductId = data.ProductId,
                    InvoiceNumber = data.InvoiceNumber,
                    BuveraCategoryId = data.BuveraCategoryId,
                    BuveraCategoryName = data.BuveraCategory != null ? data.BuveraCategory.Name:"",
                    StoreName = data.Store != null ? data.Store.Name : "",
                    ProductName = data.Product != null ? data.Product.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser2)
                };

                if (data.BuveraGradeSizes != null)
                {
                    if (data.BuveraGradeSizes.Any())
                    {
                        List<Grade> grades = new List<Grade>();
                       // var distinctGrades = data.BuveraGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        //new query as at 8/7/2019
                        var distinctGrades = data.BuveraGradeSizes.Where(a => a.BuveraId == data.BuveraId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        foreach (var buveraGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = buveraGradeSize.Grade.GradeId,
                                Value = buveraGradeSize.Grade.Value,
                                CreatedOn = buveraGradeSize.Grade.CreatedOn,
                                TimeStamp = buveraGradeSize.Grade.TimeStamp,
                                Deleted = buveraGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(buveraGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(buveraGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (buveraGradeSize.Grade.BuveraGradeSizes != null)
                            {
                                if (buveraGradeSize.Grade.BuveraGradeSizes.Any())
                                {
                                    //  var distinctSizes = buveraGradeSize.Grade.BuveraGradeSizes.GroupBy(s => s.SizeId).Select(o => o.First()).ToList();

                                    //new query as at 8/7/2019
                                    var distinctSizes = buveraGradeSize.Grade.BuveraGradeSizes.Where(a => a.BuveraId == data.BuveraId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();

                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity
                                        };
                                        //  Buvera.TotalQuantity += (ogs.Quantity * ogs.Size.Value);
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                        }
                        buvera.Grades = grades;
                    }
                }

                return buvera;
            }
            return null;
        }

        public IEnumerable<BuveraCategory> MapEFToModel(IEnumerable<EF.Models.BuveraCategory> data)
        {
            var list = new List<BuveraCategory>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Buvera category EF object to Buvera Category Model Object and
        /// returns the Buvera category model object.
        /// </summary>
        /// <param name="result">EF Buvera Category object to be mapped.</param>
        /// <returns>Buvera Category Model Object.</returns>
        public BuveraCategory MapEFToModel(EF.Models.BuveraCategory data)
        {
            if (data != null)
            {
                var buveraCategory = new BuveraCategory()
                {

                    BuveraCategoryId = data.BuveraCategoryId,
                    Name = data.Name,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    
                };


                return buveraCategory;
            }
            return null;
        }


        public StoreBuveraGradeSize MapEFToModel(EF.Models.StoreBuveraGradeSize data)
        {
            if (data != null)
            {


                var storeBuveraGradeSize = new StoreBuveraGradeSize()
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
                return storeBuveraGradeSize;
            }
            return null;
        }


        private IEnumerable<StoreBuveraGradeSize> MapEFToModel(IEnumerable<EF.Models.StoreBuveraGradeSize> data)
        {
            var list = new List<StoreBuveraGradeSize>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }

            return list;
        }

       #endregion
    }
}
