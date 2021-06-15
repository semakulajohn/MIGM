using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IWeightNoteRangeDataService
    {
        IEnumerable<WeightNoteRange> GetAllWeightNoteRanges();
        WeightNoteRange GetWeightNoteRange(long weightNoteRangeId);
        long SaveWeightNoteRange(WeightNoteRangeDTO weightNoteRange, string userId);
        void MarkAsDeleted(long weightNoteRangeId, string userId);
        IEnumerable<WeightNoteRange> GetAllWeightNoteRangesForAParticularBranch(long branchId);
        IEnumerable<WeightNoteRange> GetAllPrintedWeightNoteRangesForAParticularBranch(long branchId);
        WeightNoteRange GetLatestWeightNoteRange();
        IEnumerable<WeightNoteRange> GetAllPrintedWeightNoteRanges();

        IEnumerable<WeightNoteRange> GetLatestTenPrintedWeightNoteRangeForAParticularBranch(long branchId);

    }
}
