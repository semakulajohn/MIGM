using System;
using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;


namespace Higgs.Mbale.BAL.Concrete
{
  public  class WeightNoteNumberService : IWeightNoteNumberService
    {
        private IWeightNoteNumberDataService _dataService;
        private IUserService _userService;


        public WeightNoteNumberService(IWeightNoteNumberDataService dataService, IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;

        }


        public WeightNoteNumber GetWeightNoteNumber(long weightNoteNumberId)
        {
            var result = this._dataService.GetWeightNoteNumber(weightNoteNumberId);
            return MapEFToModel(result);
        }

        public IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId)
        {
            var results = this._dataService.GetAllWeightNoteNumbersForAParticularWeightNoteRange(weightNoteRangeId);
            return MapEFToModel(results);

        }

        public IEnumerable<WeightNoteNumber> GetAllNotUsedWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId)
        {
            var results = this._dataService.GetAllNotUsedWeightNoteNumbersForAParticularWeightNoteRange(weightNoteRangeId);
            return MapEFToModel(results);

        }

        /// </summary>
        /// <returns></returns>
        public IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbers()
        {
            var results = this._dataService.GetAllWeightNoteNumbers();
            return MapEFToModel(results);
        }

        public long SaveWeightNoteNumber(WeightNoteNumber weightNoteNumber, string userId)
        {

            long weightNoteNumberId = 0;

            var weightNoteNumberDTO = new DTO.WeightNoteNumberDTO()
            {
                WeightNoteNumberId = weightNoteNumber.WeightNoteNumberId,
                BranchId = weightNoteNumber.BranchId,
                WeightNoteValue = weightNoteNumber.WeightNoteValue,
                WeightNoteRangeId = weightNoteNumber.WeightNoteRangeId,
                Used = weightNoteNumber.Used,
                CreatedBy = weightNoteNumber.CreatedBy,
                CreatedOn = weightNoteNumber.CreatedOn,



            };


            weightNoteNumberId = this._dataService.SaveWeightNoteNumber(weightNoteNumberDTO, userId);


            return weightNoteNumberId;



        }

        public void SaveWeightNoteSupply(WeightNoteSupply weightNoteSupply)
        {
            var weightNoteSupplyDTO = new DTO.WeightNoteSupplyDTO()
            {
               
                SupplyId = weightNoteSupply.SupplyId,
                WeightNoteNumberId = weightNoteSupply.WeightNoteNumberId,
               


            };


            this._dataService.SaveWeightNoteSupply(weightNoteSupplyDTO);

        }
        public void MarkAsDeleted(long weightNoteNumberId, string userId)
        {
            _dataService.MarkAsDeleted(weightNoteNumberId, userId);
        }

        public IEnumerable<WeightNoteNumberViewModel> GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(branchId);
            return MapEFToViewModel(results);
        }

        #region Mapping Methods

        public IEnumerable<WeightNoteNumber> MapEFToModel(IEnumerable<EF.Models.WeightNoteNumber> data)
        {
            var list = new List<WeightNoteNumber>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        public WeightNoteNumber MapEFToModel(EF.Models.WeightNoteNumber data)
        {

            var weightNoteNumber = new WeightNoteNumber();

            if (data != null)
            {

                weightNoteNumber = new WeightNoteNumber()
                {
                    WeightNoteNumberId = data.WeightNoteNumberId,
                    WeightNoteRangeId = data.WeightNoteRangeId,
                    WeightNoteValue = data.WeightNoteValue,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    Used = data.Used,
                    BranchId = data.BranchId,

                    CreatedOn = Convert.ToDateTime(data.CreatedOn),
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,

                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };

                return weightNoteNumber;
            }
            return weightNoteNumber;
        }

        public IEnumerable<WeightNoteNumberViewModel> MapEFToViewModel(IEnumerable<EF.Models.WeightNoteNumber> data)
        {
            var list = new List<WeightNoteNumberViewModel>();
            foreach (var result in data)
            {
                list.Add(MapEFToViewModel(result));
            }
            return list;
        }

        public WeightNoteNumberViewModel MapEFToViewModel(EF.Models.WeightNoteNumber data)
        {

            var weightNoteNumber = new WeightNoteNumberViewModel();

            if (data != null)
            {

                weightNoteNumber = new WeightNoteNumberViewModel()
                {
                    WeightNoteNumberId = data.WeightNoteNumberId,
                    WeightNoteRangeId = data.WeightNoteRangeId,
                    WeightNoteValue = data.WeightNoteValue,
                   
                    Used = data.Used,
                    BranchId = data.BranchId,
                  

                };

                return weightNoteNumber;
            }
            return weightNoteNumber;
        }

        #endregion

    }
}
