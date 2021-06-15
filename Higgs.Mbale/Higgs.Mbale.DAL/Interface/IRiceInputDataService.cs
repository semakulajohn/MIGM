using System;
using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IRiceInputDataService
    {
        IEnumerable<RiceInput> GetAllRiceInputs();
        IEnumerable<RiceInput> GetAllRiceInputsForAParticularBranch(long branchId);
        RiceInput GetRiceInput(long riceInputId);
        long SaveRiceInput(RiceInputDTO riceInputDTO, string userId);
        void MarkAsDeleted(long riceInputId, string userId);
        void SaveRiceInputGradeSize(RiceInputGradeSizeDTO riceInputGradeSizeDTO);
        void PurgeRiceInputGradeSize(long riceInputId);
        long UpdateRiceInputOnApprovalOrRejection(long riceInputId, bool approved, string userId);
        IEnumerable<RiceInput> GetAllUnApprovedRiceInputs();
        IEnumerable<RiceInput> GetAllApprovedRiceInputs();
        IEnumerable<RiceInput> GetAllApprovedRiceInputsForAParticularBranch(long branchId);
    }
}
