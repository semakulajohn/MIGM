using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
public    interface IFinancialAccountTransactionDataService
    {
        IEnumerable<FinancialAccountTransaction> GetAllFinancialAccountTransactions();

        FinancialAccountTransaction GetFinancialAccountTransaction(long financialAccountTransactionId);

        IEnumerable<FinancialAccountTransaction> GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount(long financialAccountId);

        FinancialAccountTransaction GetLatestFinancialAccountTransactionForAParticularFinancialAccount(long financialAccountId);

        long SaveFinancialAccountTransaction(FinancialAccountTransactionDTO financialAccountTransactionDTO, string userId);


        void MarkAsDeleted(long financialAccountId, long financialAccountTransactionId, string userId);
     

    }
}
