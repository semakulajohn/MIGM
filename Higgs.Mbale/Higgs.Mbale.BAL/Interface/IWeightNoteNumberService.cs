using System;
using System.Collections.Generic;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface IWeightNoteNumberService
    {
        IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbers();

      
        WeightNoteNumber GetWeightNoteNumber(long weightNoteNumberId);
        long SaveWeightNoteNumber(WeightNoteNumber weightNoteNumber, string userId);
        void MarkAsDeleted(long weightNoteRangeId, string userId);
        IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId);
        IEnumerable<WeightNoteNumber> GetAllNotUsedWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId);
        IEnumerable<WeightNoteNumberViewModel> GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(long branchId);
        void SaveWeightNoteSupply(WeightNoteSupply weightNoteSupplyDTO);
    }
}
