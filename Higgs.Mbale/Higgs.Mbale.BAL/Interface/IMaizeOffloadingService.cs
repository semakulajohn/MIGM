using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IMaizeOffloadingService
    {
        IEnumerable<MaizeOffloading> GetAllMaizeOffloadings();
        MaizeOffloading GetMaizeOffloading(long maizeOffloadingId);
        long SaveMaizeOffloading(MaizeOffloading maizeOffloading, string userId);
        void MarkAsDeleted(long maizeOffloadingId, string userId);

        //IEnumerable<MaizeOffloading> GetLatestTwentyMaizeOffloadingsForAParticularBranch(long branchId);

    }
}
