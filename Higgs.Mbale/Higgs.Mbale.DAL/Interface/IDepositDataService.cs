using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IDepositDataService
    {

        IEnumerable<Deposit> GetAllDeposits();
       
        Deposit GetDeposit(long depositId);
       
        IEnumerable<Deposit> GetAllDepositsForAParticularAspNetUser(string accountId);
        
        IEnumerable<Deposit> GetAllUnApprovedDepositsForAParticularAspNetUser(string accountId);
       
        IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForAParticularAspNetUser(string accountId);

        IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForAParticularAspNetUser(string accountId);
               
        IEnumerable<Deposit> GetAllDepositsForAParticularCasualWorker(long accountId);
        
        IEnumerable<Deposit> GetAllLatestTwentyApprovedDepositsForAParticularCasualWorker(long accountId);
               
         IEnumerable<Deposit> GetAllUnpprovedDepositsForAParticularCasualWorker(long accountId);
        
         IEnumerable<Deposit> GetAllLatestTwentyRejectedDepositsForAParticularCasualWorker(long accountId);
       
        long SaveDeposit(DepositDTO depositDTO, string userId);
       
        void MarkAsDeleted(long depositId, string userId);

        IEnumerable<Deposit> GetLatestTwentyRejectedDeposits();

        IEnumerable<Deposit> GetLatestTwentyApprovedDeposits();

        IEnumerable<Deposit> GetLatestTwentyUnApprovedDeposits();

        IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForABranch(long branchId);

        IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForABranch(long branchId);

        IEnumerable<Deposit> GetLatestTwentyUnApprovedDepositsForABranch(long branchId);
       


    }
}
