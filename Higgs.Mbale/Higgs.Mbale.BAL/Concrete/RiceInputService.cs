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
 public   class RiceInputService : IRiceInputService
    {
        private long riceTransactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["RiceSaleTransactionSubTypeId"]);
        private long buveraCategoryId = Convert.ToInt64(ConfigurationManager.AppSettings["UsedBuveraCategoryId"]);
        ILog logger = log4net.LogManager.GetLogger(typeof(RiceInputService));
        private IRiceInputDataService _dataService;
        private IUserService _userService;
        private IStoreService _storeService;
        private IStockService _stockService;
        private IBuveraService _buveraService;
        private IAccountTransactionActivityService _accountTransactionActivityService;



        public RiceInputService(IRiceInputDataService dataService, IUserService userService,
            IStoreService storeService, IStockService stockService, IBuveraService buveraService, IAccountTransactionActivityService accountTransactionActivityService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._storeService = storeService;
            this._stockService = stockService;
            this._buveraService = buveraService;
            this._accountTransactionActivityService = accountTransactionActivityService;

        }


        public RiceInput GetRiceInput(long riceInputId)
        {
            var result = this._dataService.GetRiceInput(riceInputId);
            return MapEFToModel(result);
        }


        public IEnumerable<RiceInput> GetAllRiceInputs()
        {
            var results = this._dataService.GetAllRiceInputs();
            return MapEFToModel(results);
        }


        public IEnumerable<RiceInput> GetAllRiceInputsForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllRiceInputsForAParticularBranch(branchId);
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
        public long SaveRiceInput(RiceInput riceInput, string userId)
        {
            if (riceInput.Approved == null)
            {
                long riceInputId = 0;

                var riceInputDTO = new DTO.RiceInputDTO()
                {
                    StoreId = riceInput.StoreId,
                    TotalAmount = riceInput.TotalAmount,
                    TotalQuantity = riceInput.TotalQuantity,
                    Price = riceInput.Price,
                    BranchId = riceInput.BranchId,
                    Deleted = riceInput.Deleted,
                    CreatedBy = riceInput.CreatedBy,
                    Approved = riceInput.Approved,
                    CreatedOn = riceInput.CreatedOn
                };

                riceInputId = this._dataService.SaveRiceInput(riceInputDTO, userId);


                if (riceInput.Grades.Any())
                {
                    List<RiceInputGradeSize> riceInputGradeSizeList = new List<RiceInputGradeSize>();

                    foreach (var grade in riceInput.Grades)
                    {
                        var gradeId = grade.GradeId;
                        if (grade.Denominations != null)
                        {
                            if (grade.Denominations.Any())
                            {
                                foreach (var denomination in grade.Denominations)
                                {
                                    var RiceInputGradeSize = new RiceInputGradeSize()
                                    {
                                        GradeId = gradeId,
                                        SizeId = denomination.DenominationId,
                                        RiceInputId = riceInputId,
                                         Price = denomination.Value * riceInput.Price,
                                        Quantity = denomination.Quantity,
                                        Amount = denomination.Quantity * denomination.Value * riceInput.Price,
                                        
                                        TimeStamp = DateTime.Now
                                    };
                                    riceInputGradeSizeList.Add(RiceInputGradeSize);


                                      }
                            }
                        }
                    }

                    this._dataService.PurgeRiceInputGradeSize(riceInputId);
                    this.SaveRiceInputGradeSizeList(riceInputGradeSizeList);

                }



                return riceInputId;
            }


            else
            {
                if (riceInput.CreatedById == userId)
                {
                    long riceInputId = -22;
                    return riceInputId;
                }
                else
                {
                    //update output with either approved == true or rejected == true
                    long riceInputId = 0;
                    var store = this._storeService.GetStore(riceInput.StoreId);
                    if (Convert.ToBoolean(riceInput.Approved))
                    {
                        if (riceInput.Grades != null)
                        {
                            //var gradeStore = CheckForBuveraInStore(riceInput.Grades, riceInput.StoreId);
                            //if (gradeStore == -2)
                            //{
                            //    riceInputId = gradeStore;
                            //    return riceInputId;
                            //}
                            //else if (gradeStore == -1)
                            //{
                            //    riceInputId = gradeStore;
                            //    return riceInputId;
                            //}
                            if (riceInput.Grades.Any())
                            {
                                List<RiceInputGradeSize> RiceInputGradeSizeList = new List<RiceInputGradeSize>();

                                foreach (var grade in riceInput.Grades)
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
                                                    StoreId = riceInput.StoreId,
                                                    Quantity = denomination.Quantity,
                                                    TimeStamp = DateTime.Now
                                                };
                                                this._stockService.SaveStoreGradeSize(storeGradeSize, true);
                                                //Method that updates buvera into storeBuveraGradeSize table(storeBuvera stock)
                                                var storeBuveraGradeSize = new StoreBuveraGradeSize()
                                                {
                                                    StoreId = riceInput.StoreId,
                                                    GradeId = gradeId,
                                                    SizeId = denomination.DenominationId,
                                                    Quantity = denomination.Quantity,
                                                };

                                                this._buveraService.SaveStoreBuveraGradeSize(storeBuveraGradeSize, inOrOut);

                                            }
                                        }
                                    }
                                }

                                this._dataService.PurgeRiceInputGradeSize(riceInputId);
                                this.SaveRiceInputGradeSizeList(RiceInputGradeSizeList);

                            }
                            riceInputId = _dataService.UpdateRiceInputOnApprovalOrRejection(riceInput.RiceInputId, Convert.ToBoolean(riceInput.Approved), userId);

                          
                         

                            return riceInputId;






                        }

                        return riceInput.RiceInputId;
                    }
                    else
                    {
                        if (riceInput.Grades != null)
                        {

                            if (riceInput.Grades.Any())
                            {
                                List<RiceInputGradeSize> RiceInputGradeSizeList = new List<RiceInputGradeSize>();

                                foreach (var grade in riceInput.Grades)
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
                                                    StoreId = riceInput.StoreId,
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
                            riceInputId = _dataService.UpdateRiceInputOnApprovalOrRejection(riceInput.RiceInputId, Convert.ToBoolean(riceInput.Approved), userId);



                        }


                        return riceInput.RiceInputId;
                    }

                }
            }

        }

        public IEnumerable<RiceInput> GetAllUnApprovedRiceInputs()
        {
            var results = this._dataService.GetAllUnApprovedRiceInputs();
            return MapEFToModel(results);
        }
        public IEnumerable<RiceInput> GetAllApprovedRiceInputs()
        {
            var results = this._dataService.GetAllApprovedRiceInputs();
            return MapEFToModel(results);
        }
        public IEnumerable<RiceInput> GetAllApprovedRiceInputsForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllApprovedRiceInputsForAParticularBranch(branchId);
            return MapEFToModel(results);
        }

        void SaveRiceInputGradeSizeList(List<RiceInputGradeSize> riceInputGradeSizeList)
        {
            if (riceInputGradeSizeList != null)
            {
                if (riceInputGradeSizeList.Any())
                {
                    foreach (var riceInputGradeSize in riceInputGradeSizeList)
                    {
                        var riceInputGradeSizeDTO = new DTO.RiceInputGradeSizeDTO()
                        {
                            RiceInputId = riceInputGradeSize.RiceInputId,
                            GradeId = riceInputGradeSize.GradeId,
                            Quantity = riceInputGradeSize.Quantity,
                            SizeId = riceInputGradeSize.SizeId,
                            Price = riceInputGradeSize.Price,
                            Amount = riceInputGradeSize.Amount,
                            TimeStamp = riceInputGradeSize.TimeStamp
                        };
                        this.SaveRiceInputGradeSize(riceInputGradeSizeDTO);
                    }
                }
            }
        }

        void SaveRiceInputGradeSize(RiceInputGradeSizeDTO riceInputGradeSizeDTO)
        {
            _dataService.SaveRiceInputGradeSize(riceInputGradeSizeDTO);
        }


        public void MarkAsDeleted(long riceInputId, string userId)
        {
            _dataService.MarkAsDeleted(riceInputId, userId);
        }


        #region Mapping Methods

        public IEnumerable<RiceInput> MapEFToModel(IEnumerable<EF.Models.RiceInput> data)
        {
            var list = new List<RiceInput>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public RiceInput MapEFToModel(EF.Models.RiceInput data)
        {
            if (data != null)
            {
                var riceInput = new RiceInput()
                {
                    RiceInputId = data.RiceInputId,
                    StoreId = data.StoreId,
                    TotalQuantity = data.TotalQuantity,
                    TotalAmount = data.TotalAmount,
                    BranchId = data.BranchId,
                    Approved = data.Approved,
                    Price = data.Price,
                    CreatedOn = data.CreatedOn,
                    CreatedById = data.CreatedBy,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),
                    StoreName = data.Store != null ? data.Store.Name : "",
                    BranchName = data.Branch != null? data.Branch.Name : "",
                };

                List<Grade> grades = new List<Grade>();
                List<Grade> availableGrades = new List<Grade>();
                if (data.RiceInputGradeSizes != null)
                {
                    if (data.RiceInputGradeSizes.Any())
                    {

                        // var distinctGrades = data.BatchGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        var distinctGrades = data.RiceInputGradeSizes.Where(a => a.RiceInputId == data.RiceInputId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();

                        foreach (var riceInputGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = riceInputGradeSize.Grade.GradeId,
                                Value = riceInputGradeSize.Grade.Value,

                                CreatedOn = riceInputGradeSize.Grade.CreatedOn,
                                TimeStamp = riceInputGradeSize.Grade.TimeStamp,
                                Deleted = riceInputGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(riceInputGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(riceInputGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (riceInputGradeSize.Grade.RiceInputGradeSizes != null)
                            {
                                if (riceInputGradeSize.Grade.RiceInputGradeSizes.Any())
                                {


                                    var distinctSizes = riceInputGradeSize.Grade.RiceInputGradeSizes.Where(a => a.RiceInputId == data.RiceInputId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();


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
                                        // RiceInput.TotalQuantity += (ogs.Quantity * ogs.Size.Value);

                                        
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
                riceInput.Grades = grades;



                return riceInput;
            }
            return null;

        }



        #endregion
    }
}
