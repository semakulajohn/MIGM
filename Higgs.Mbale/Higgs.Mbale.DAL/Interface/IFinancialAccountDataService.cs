using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IFinancialAccountDataService
    {
        IEnumerable<FinancialAccount> GetAllFinancialAccounts();

        FinancialAccount GetFinancialAccount(long financialAccountId);
        
         long SaveFinancialAccount(FinancialAccountDTO financialAccountDTO, string userId);
        
       

    }
}
