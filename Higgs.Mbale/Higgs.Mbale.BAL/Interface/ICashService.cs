
using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface ICashService
    {
        Cash GetCash(long cashId);
        IEnumerable<Cash> GetAllCash();

        IEnumerable<Cash> GetAllCashForAParticularBranch(long branchId);
        IEnumerable<Cash> MapEFToModel(IEnumerable<EF.Models.Cash> data);
        long SaveCash(Cash cash, string userId);

        void MarkAsDeleted(long cashId, string userId,long branchId);
        void SaveApplicationCash(Cash cash,string userId);
        IEnumerable<Cash> GetThirtyLatestCashForAParticularBranch(long branchId);
        long CheckIfBranchHasEnoughCash(long branchId, double amount, string action);
       // Cash GetLatestCashForAParticularBranch(long branchId);
    }
}
