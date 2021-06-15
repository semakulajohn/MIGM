using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IUtilityService
    {
        IEnumerable<Utility> GetAllUtilities();
        Utility GetUtility(long utilityId);
        long SaveUtility(Utility utility, string userId);
        void MarkAsDeleted(long utilityId, string userId);
        IEnumerable<Utility> GetAllUtilitiesForAParticularBatch(long batchId);
        IEnumerable<Utility> MapEFToModel(IEnumerable<EF.Models.Utility> data);
    }
}
