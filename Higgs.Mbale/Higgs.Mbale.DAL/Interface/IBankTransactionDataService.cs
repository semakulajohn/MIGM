using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IBankTransactionDataService
    {
     IEnumerable<BankTransaction> GetAllBankTransactions();
     BankTransaction GetBankTransaction(long bankTransactionId);
     long SaveBankTransaction(BankTransactionDTO bankTransaction, string userId);
     void MarkAsDeleted(long bankTransactionId, string userId);

     //IEnumerable<BankTransaction> GetLatestTwentyBankTransactionsForAParticularBank(long bankId);
     IEnumerable<BankTransaction> GetLatestTwentyBankTransactionsForAParticularBranchAndBank(long branchId, long bankId);


    }
}
