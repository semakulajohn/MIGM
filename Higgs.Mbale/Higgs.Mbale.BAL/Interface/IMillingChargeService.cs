using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IMillingChargeService
    {
        IEnumerable<MillingCharge> GetAllMillingCharges();
        MillingCharge GetMillingCharge(long millingChargeId);
        long SaveMillingCharge(MillingCharge millingCharge, string userId);
        void MarkAsDeleted(long MillingChargeId, string userId);

        //IEnumerable<MillingCharge> GetLatestTwentyMillingChargesForAParticularBranch(long branchId);
      
    }
}
