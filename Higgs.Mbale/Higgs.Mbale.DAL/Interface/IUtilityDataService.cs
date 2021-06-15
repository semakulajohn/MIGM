using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IUtilityDataService
    {
        IEnumerable<Utility> GetAllUtilities();
        Utility GetUtility(long utilityId);
        long SaveUtility(UtilityDTO utility, string userId);
        void MarkAsDeleted(long utilityId, string userId);
        IEnumerable<Utility> GetAllUtilitiesForAParticularBatch(long batchId);
    }
}
