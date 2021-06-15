using System;
using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Concrete
{
  public  class WeightNoteRangeService : IWeightNoteRangeService
    {
        private IWeightNoteRangeDataService _dataService;
        private IUserService _userService;
        private IWeightNoteNumberService _weightNoteNumberService;
            

        public WeightNoteRangeService(IWeightNoteRangeDataService dataService, IUserService userService,
            IWeightNoteNumberService weightNoteNumberService )
        {
            this._dataService = dataService;
            this._userService = userService;
            this._weightNoteNumberService = weightNoteNumberService;
        }

       
        public WeightNoteRange GetWeightNoteRange(long weightNoteRangeId)
        {
            var result = this._dataService.GetWeightNoteRange(weightNoteRangeId);
            return MapEFToModel(result);
        }

        public IEnumerable<WeightNoteRangeViewModel> GetAllWeightNoteRangesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllWeightNoteRangesForAParticularBranch(branchId);
            return MapEFToWebViewModel(results);

        }

        public IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId)
        {
            var results = _weightNoteNumberService.GetAllWeightNoteNumbersForAParticularWeightNoteRange(weightNoteRangeId);
            return results;

        }

        public IEnumerable<WeightNoteRangeViewModel> GetAllWeightNoteRangeViewModel()
        {
            var results = this._dataService.GetAllWeightNoteRanges();
            return MapEFToWebViewModel(results);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WeightNoteRange> GetAllWeightNoteRanges()
        {
            var results = this._dataService.GetAllWeightNoteRanges();
            return MapEFToModel(results);
        }

        public  IEnumerable<WeightNoteRangeViewModel> GetAllPrintedWeightNoteRangesForAParticularBranch(long branchId)
        {
                    var results = this._dataService.GetAllPrintedWeightNoteRangesForAParticularBranch(branchId);
                    return MapEFToWebViewModel(results);

            }

        public IEnumerable<WeightNoteRangeViewModel> GetAllPrintedWeightNoteRanges()
        {
            var results = this._dataService.GetAllPrintedWeightNoteRanges();
            return MapEFToWebViewModel(results);

        }

        public IEnumerable<WeightNoteRangeViewModel> GetLatestTenPrintedWeightNoteRangeForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetLatestTenPrintedWeightNoteRangeForAParticularBranch(branchId);
            return MapEFToWebViewModel(results);
        }


        public long SaveWeightNoteRange(WeightNoteRange weightNoteRange, string userId)
        {
           
                long weightNoteRangeId = 0;
               
                 var weightNoteRangeDTO = new DTO.WeightNoteRangeDTO()
                        {
                           WeightNoteRangeId = weightNoteRange.WeightNoteRangeId,
                            BranchId = weightNoteRange.BranchId,
                          StartNumber = weightNoteRange.StartNumber,
                          EndNumber = weightNoteRange.EndNumber,
                          Printed = weightNoteRange .Printed,
                          
                            
                            CreatedBy = weightNoteRange.CreatedBy,
                            CreatedOn = weightNoteRange.CreatedOn,
                          


                        };

            WeightNoteRange latestWeightNoteRange = GetLatestWeightNoteRange();
            if (latestWeightNoteRange != null)
            {
                if (weightNoteRange.StartNumber > latestWeightNoteRange.EndNumber)
                {
                    weightNoteRangeId = this._dataService.SaveWeightNoteRange(weightNoteRangeDTO, userId);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                weightNoteRangeId = this._dataService.SaveWeightNoteRange(weightNoteRangeDTO, userId);

            }
            //var weightNote = GenerateWeightNoteNumbers(weightNoteRange, weightNoteRangeId, userId);

            return weightNoteRangeId;
            
        }
       
        public void MarkAsDeleted(long weightNoteRangeId, string userId)
        {
            _dataService.MarkAsDeleted(weightNoteRangeId, userId);
        }

        public long GenerateWeightNoteNumbers(long weightNoteRangeId,string userId)
        {
           
            if(weightNoteRangeId != 0)
            {
                WeightNoteRange weightNoteRange = GetWeightNoteRange(weightNoteRangeId);
                if(weightNoteRange != null)
                {
                    var startNumber = 0;
                    if (weightNoteRange.StartNumber == 0)
                    {
                        startNumber = 1;
                    }
                    else
                    {
                        startNumber = Convert.ToInt32(weightNoteRange.StartNumber);
                    }
                    
                        for (int i = startNumber; i <= Convert.ToInt32(weightNoteRange.EndNumber); i++)
                        {
                            var weightNoteNumber = new WeightNoteNumber()
                            {

                                BranchId = weightNoteRange.BranchId,
                                WeightNoteValue = i,
                                WeightNoteRangeId = weightNoteRangeId,
                                Used = false,
                                

                            };

                         var weightNoteNumberId =  _weightNoteNumberService.SaveWeightNoteNumber(weightNoteNumber, userId);
                        }
                        return weightNoteRangeId;
                   
                   
                }
                else
                {
                    //weightnoterange doesn't exist
                    return -3;
                }
            }
            else
            {
                return -2;
            }
        }
        public WeightNoteRange GetLatestWeightNoteRange()
        {
            var result = this._dataService.GetLatestWeightNoteRange();
            return MapEFToModel(result);

        }

        #region Mapping Methods

        public IEnumerable<WeightNoteRange> MapEFToModel(IEnumerable<EF.Models.WeightNoteRange> data)
        {
            var list = new List<WeightNoteRange>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        public IEnumerable<WeightNoteRangeViewModel> MapEFToWebViewModel(IEnumerable<EF.Models.WeightNoteRange> data)
        {
            var list = new List<WeightNoteRangeViewModel>();
            foreach (var result in data)
            {
                list.Add(MapEFToWebViewModel(result));
            }
            return list;
        }

       
        public WeightNoteRange MapEFToModel(EF.Models.WeightNoteRange data)
        {
           
            var weightNoteRange = new WeightNoteRange();
           
            if (data != null)
            {
                var weightNoteNumbers = GetAllWeightNoteNumbersForAParticularWeightNoteRange(data.WeightNoteRangeId);
                weightNoteRange = new WeightNoteRange()
                {
                   WeightNoteRangeId = data.WeightNoteRangeId,
                   StartNumber = data.StartNumber,
                   EndNumber = data.EndNumber,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                   Printed = data.Printed,
                    BranchId = data.BranchId,
                   WeightNoteNumbers = weightNoteNumbers,
                    CreatedOn = Convert.ToDateTime(data.CreatedOn),
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                   
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };
               
                return weightNoteRange;
            }
            return weightNoteRange;
        }

        public WeightNoteRangeViewModel MapEFToWebViewModel(EF.Models.WeightNoteRange data)
        {

            var weightNoteRange = new WeightNoteRangeViewModel();

            if (data != null)
            {

                weightNoteRange = new WeightNoteRangeViewModel()
                {
                    WeightNoteRangeId = data.WeightNoteRangeId,
                    StartNumber = data.StartNumber,
                    EndNumber = data.EndNumber,
                   
                    BranchId = data.BranchId,

                    Printed = data.Printed,
                   
                };

                return weightNoteRange;
            }
            return weightNoteRange;
        }


        #endregion
    }
}
