using System;
using System.Collections.Generic;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface IWeightNoteRangeService
    {
        IEnumerable<WeightNoteRange> GetAllWeightNoteRanges();

        IEnumerable<WeightNoteRangeViewModel> GetAllWeightNoteRangeViewModel();
        WeightNoteRange GetWeightNoteRange(long weightNoteRangeId);
        long SaveWeightNoteRange(WeightNoteRange weightNoteRange, string userId);
        void MarkAsDeleted(long weightNoteRangeId, string userId);
        IEnumerable<WeightNoteRangeViewModel> GetAllWeightNoteRangesForAParticularBranch(long branchId);
        IEnumerable<WeightNoteRangeViewModel> GetAllPrintedWeightNoteRangesForAParticularBranch(long branchId);
        IEnumerable<WeightNoteRangeViewModel> GetAllPrintedWeightNoteRanges();
        WeightNoteRange GetLatestWeightNoteRange();
        IEnumerable<WeightNoteRangeViewModel> GetLatestTenPrintedWeightNoteRangeForAParticularBranch(long branchId);

        long GenerateWeightNoteNumbers(long weightNoteRangeId,string userId);
    }
}
