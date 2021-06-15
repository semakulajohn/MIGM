using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IMaizeOffloadingDataService
    {
     IEnumerable<MaizeOffloading> GetAllMaizeOffloadings();
     MaizeOffloading GetMaizeOffloading(long maizeOffloadingId);
     long SaveMaizeOffloading(MaizeOffloadingDTO maizeOffloading, string userId);
     void MarkAsDeleted(long maizeOffloadingId, string userId);

     //IEnumerable<MaizeOffloading> GetLatestTwentyMaizeOffloadingsForAParticularBranch(long branchId);
     MaizeOffloading GetLatestMaizeOffloadingForAParticularBranch(long branchId);

    }
}
