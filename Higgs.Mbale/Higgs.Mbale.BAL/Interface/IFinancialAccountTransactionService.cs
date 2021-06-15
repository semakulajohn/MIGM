using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IFinancialAccountTransactionService
    {
        IEnumerable<FinancialAccountTransaction> GetAllFinancialAccountTransactions();
        FinancialAccountTransaction GetFinancialAccountTransaction(long financialAccountTransactionId);
        long SaveFinancialAccountTransaction(FinancialAccountTransaction financialAccountTransaction, string userId);
        void MarkAsDeleted(long financialAccountId,long financialAccountTransactionId, string userId);
       IEnumerable<FinancialAccountTransaction> GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount(long financialAccountId);

    }
}
