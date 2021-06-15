using System;
using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface IRiceInputService
    {
        IEnumerable<RiceInput> GetAllRiceInputs();
        IEnumerable<RiceInput> GetAllRiceInputsForAParticularBranch(long branchId);
        RiceInput GetRiceInput(long riceInputId);
        long SaveRiceInput(RiceInput riceInput, string userId);
        void MarkAsDeleted(long riceInputId, string userId);
        
        IEnumerable<RiceInput> GetAllUnApprovedRiceInputs();
        IEnumerable<RiceInput> GetAllApprovedRiceInputs();
        IEnumerable<RiceInput> GetAllApprovedRiceInputsForAParticularBranch(long branchId);

        IEnumerable<RiceInput> MapEFToModel(IEnumerable<EF.Models.RiceInput> data);

    }
}
