using System;
using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
    public interface IOutSourcerOutPutDataService
    {

        IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPuts();
        IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPutsForAParticularOutSourcerStore(long storeId);
        OutSourcerOutPut GetOutSourcerOutPut(long outSourcerOutPutId);
        long SaveOutSourcerOutPut(OutSourcerOutPutDTO outSourcerOutPutDTO, string userId);
        void MarkAsDeleted(long outSourcerOutPutId, string userId);
        void SaveOutSourcerOutPutGradeSize(OutSourcerOutPutGradeSizeDTO outSourcerOutPutGradeSizeDTO);
        void PurgeOutSourcerOutPutGradeSize(long outSourcerOutPutId);
        long UpdateOutPutOnApprovalOrRejection(long outSourcerOutPutId, bool approved, string userId);
        IEnumerable<OutSourcerOutPut> GetAllUnApprovedOutSourcerOutPuts();
        IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPuts();
        IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore(long storeId);
    }  
}
