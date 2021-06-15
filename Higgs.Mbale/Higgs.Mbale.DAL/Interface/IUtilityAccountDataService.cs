using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IUtilityAccountDataService
    {

     IEnumerable<UtilityAccount> GetAllUtilityAccounts();
      
        UtilityAccount GetUtilityAccount(long utilityAccountId);
     
        IEnumerable<UtilityAccount> GetAllUtilityAccountForAParticularBranch(long branchId);

        UtilityAccount GetLatestUtilityAccountForAParticularBranchAndCategory(long branchId, long categoryId);
        long SaveUtilityAccount(UtilityAccountDTO utilityAccountDTO, string userId);
  

        void MarkAsDeleted(long utilityAccountId, string userId);
       IEnumerable<UtilityCategory> GetAllUtilityCategories();

       UtilityCategory GetUtilityCategory(long utilityCategoryId);
       IEnumerable<UtilityAccount> GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory(long branchId, long categoryId);
        UtilityAccount GetLatestUtilityAccountForAParticularBranchAndCategoryForAParticularDate(long branchId, long categoryId, DateTime dateTime);
    }
}
