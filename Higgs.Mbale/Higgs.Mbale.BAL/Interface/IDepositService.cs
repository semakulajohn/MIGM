
using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface IDepositService
    {
        IEnumerable<Deposit> GetAllDeposits();

        Deposit GetDeposit(long depositId);


        IEnumerable<Deposit> GetAllDepositsForAParticularAccount(string accountId);
       
        IEnumerable<Deposit> GetAllUnApprovedDepositsForAParticularAccount(string accountId);
        
        IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForAParticularAccount(string accountId);
        
        IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForAParticularAccount(string accountId);
        long ApproveOrRejectDeposit(Deposit deposit, bool status, string userId);

        long SaveDeposit(Deposit deposit, string userId);

        void MarkAsDeleted(long depositId, string userId);

        IEnumerable<Deposit> GetLatestTwentyRejectedDeposits();

        IEnumerable<Deposit> GetLatestTwentyApprovedDeposits();

        IEnumerable<Deposit> GetLatestTwentyUnApprovedDeposits();

        IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForABranch(long branchId);

        IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForABranch(long branchId);

        IEnumerable<Deposit> GetLatestTwentyUnApprovedDepositsForABranch(long branchId);




    }
}
