using System;
using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IWeightNoteNumberDataService
    {
        IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbers();
        WeightNoteNumber GetWeightNoteNumber(long weightNoteNumberId);
        long SaveWeightNoteNumber(WeightNoteNumberDTO weightNoteNumber, string userId);
        void MarkAsDeleted(long weightNoteRangeId, string userId);
        IEnumerable<WeightNoteNumber> GetAllWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId);
        IEnumerable<WeightNoteNumber> GetAllNotUsedWeightNoteNumbersForAParticularWeightNoteRange(long weightNoteRangeId);
        IEnumerable<WeightNoteNumber> GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(long branchId);
        void SaveWeightNoteSupply(WeightNoteSupplyDTO weightNoteSupplyDTO);
    }
}
