using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface IDebtorService
    {
        IEnumerable<Debtor> GetAllDebtors();
        Debtor GetDebtor(long debtorId);
        long SaveDebtor(Debtor debtor, string userId);
        void MarkAsDeleted(long debtorId, string userId);
        IEnumerable<Debtor> GetAllDebtorsForAParticularBranch(long branchId);
        IEnumerable<Debtor> GetAllDistinctDebtorRecords();
        IEnumerable<Debtor> GetAllDebtorRecordsForParticularAccount(string aspNetUserId, long casualWorkerId);
        IEnumerable<DebtorView> GetDebtorView();
        IEnumerable<DebtorView> GetDebtorViewForAParticularDate(DateTime dateTime);
        IEnumerable<DebtorView> GetAdvancePaymentView();
        IEnumerable<DebtorView> GetAdvancePaymentViewForAParticularDate(DateTime dateTime);
    }
}
