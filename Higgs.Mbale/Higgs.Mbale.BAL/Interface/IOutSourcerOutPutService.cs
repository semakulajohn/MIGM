using System;
using System.Collections.Generic;

using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
   public  interface IOutSourcerOutPutService
    {
       
        IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPuts();
        IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPutsForAParticularOutSourcerStore(long storeId);
        OutSourcerOutPut GetOutSourcerOutPut(long outSourcerOutPutId);
        long SaveOutSourcerOutPut(OutSourcerOutPut outSourcerOutPut, string userId);
        void MarkAsDeleted(long outSourcerOutPutId, string userId);
        //void SaveOutSourcerOutPutGradeSize(OutSourcerOutPutGradeSize outSourcerOutPutGradeSize);
        IEnumerable<OutSourcerOutPut> GetAllUnApprovedOutSourcerOutPuts();
        IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPuts();
        IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore(long storeId);

        IEnumerable<OutSourcerOutPut> MapEFToModel(IEnumerable<EF.Models.OutSourcerOutPut> data);
    }
}
