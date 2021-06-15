using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IFinancialAccountService
    {

        FinancialAccount GetFinancialAccount(long financialAccountId);

        IEnumerable<FinancialAccount> GetAllFinancialAccounts();


        long SaveFinancialAccount(FinancialAccount financialAccount, string userId);
        
    }
}
