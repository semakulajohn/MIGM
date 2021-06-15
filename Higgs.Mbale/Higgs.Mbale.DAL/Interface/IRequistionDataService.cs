using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IRequistionDataService
    {
        IEnumerable<Requistion> GetAllRequistions();
        Requistion GetRequistion(long requistionId);
        long SaveRequistion(RequistionDTO requistion, string userId);
        void MarkAsDeleted(long requistionId, string userId);
        IEnumerable<Requistion> GetAllRequistionsForAParticularStatus(long statusId);
        IEnumerable<Requistion> GetAllRequistionsForAParticularBranch(long branchId);
        void UpdateRequistionWithCompletedStatus(long requistionId, long statusId, string userId);
        Requistion GetLatestCreatedRequistion();
        IEnumerable<Requistion> GetAllRequistionsForAParticularStatusForBranch(long statusId, long branchId);
        IEnumerable<Requistion> GetLatestThirtyRequistionsForAParticularStatusForBranch(long statusId, long branchId);
        IEnumerable<Requistion> GetLatestSixtyRequistionsForAParticularBranch(long branchId);

        IEnumerable<RequistionCategory> GetAllRequistionCategories();
    }
}
