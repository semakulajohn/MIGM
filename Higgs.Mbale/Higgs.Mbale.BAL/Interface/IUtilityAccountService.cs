using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IUtilityAccountService
    {
     UtilityAccount GetUtilityAccount(long utilityAccountId);
        
         IEnumerable<UtilityAccount> GetAllUtilityAccountForAParticularBranch(long branchId);
       
       IEnumerable<UtilityAccount> GetAllUtilityAccounts();
      

       double GetBalanceForLastUtilityAccount(long branchId,long categoryId);
        double GetBalanceForLastUtilityAccountForAParticularDate(long branchId, long categoryId,DateTime dateTime);


        double GetBalanceForUtilityAccountForABranch(long branchId,long categoryId);

       long SaveUtilityAccount(UtilityAccount utilityAccount, string userId);
      
        void MarkAsDeleted(long utilityAccountId, string userId);
        IEnumerable<UtilityCategory> GetAllUtilityCategories();

        UtilityCategory GetUtilityCategory(long utilityCategoryId);
        IEnumerable<UtilityAccount> GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory(long branchId, long categoryId);
        IEnumerable<UtilityAccount> MapEFToModel(IEnumerable<EF.Models.UtilityAccount> data);
        
      
    }
}
