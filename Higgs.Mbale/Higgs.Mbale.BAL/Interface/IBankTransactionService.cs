using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
public  interface IBankTransactionService
    {
        IEnumerable<BankTransaction> GetAllBankTransactions();
        BankTransaction GetBankTransaction(long bankTransactionId);
        long SaveBankTransaction(BankTransaction bankTransaction, string userId);
        void MarkAsDeleted(long bankTransactionId, string userId);

       // IEnumerable<BankTransaction> GetLatestTwentyBankTransactionsForAParticularBank(long bankId);
        IEnumerable<BankTransaction> GetLatestTwentyBankTransactionsForAParticularBranchAndBank(long branchId, long bankId);
    }
}
