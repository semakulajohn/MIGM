using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IMillingChargeDataService
    {
     IEnumerable<MillingCharge> GetAllMillingCharges();
     MillingCharge GetMillingCharge(long millingChargeId);
     long SaveMillingCharge(MillingChargeDTO MillingCharge, string userId);
     void MarkAsDeleted(long MillingChargeId, string userId);

     //IEnumerable<MillingCharge> GetLatestTwentyMillingChargesForAParticularBranch(long branchId);
      
    }
}
