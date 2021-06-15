using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IRequistionService
    {
        IEnumerable<Requistion> GetAllRequistions();
        Requistion GetRequistion(long requistionId);
        long SaveRequistion(Requistion requistion, string userId);
        void MarkAsDeleted(long requistionId, string userId);
        IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularStatus(long statusId);
        IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularBranch(long branchId);
        IEnumerable<RequistionViewModel> GetAllRequistionsForAParticularStatusForBranch(long statusId, long branchId);
        IEnumerable<RequistionViewModel> GetLatestThirtyRequistionsForAParticularStatusForBranch(long statusId, long branchId);
        IEnumerable<RequistionViewModel> GetLatestSixtyRequistionsForAParticularBranch(long branchId);

        IEnumerable<RequistionCategory> GetAllRequistionCategories();
    }
}
