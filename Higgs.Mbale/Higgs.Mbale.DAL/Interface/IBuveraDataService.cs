using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
 public   interface IBuveraDataService
    {

        IEnumerable<Buvera> GetAllBuveras();
        Buvera GetBuvera(long buveraId);
        long SaveBuvera(BuveraDTO buvera, string userId);
        void MarkAsDeleted(long buveraId, string userId);
        IEnumerable<Buvera> GetAllBuverasForAParticularStore(long storeId);
        IEnumerable<Buvera> GetAllDamagedBuverasForAparticularStore(long storeId, long buveraCategoryId);
        void SaveBuveraGradeSize(BuveraGradeSizeDTO buveraGradeSizeDTO);
        void PurgeBuveraGradeSize(long buveraId);
        IEnumerable<StoreBuveraGradeSize> GetStoreBuveraStock(long storeId);
        void SaveStoreBuveraGradeSize(StoreBuveraGradeSizeDTO storeBuveraGradeSizeDTO, bool inOrOut);
        StoreBuveraGradeSize GetStoreBuveraGradeSize(long gradeId,long sizeId, long storeId);
        IEnumerable<BuveraCategory> GetAllBuveraCategories();
       }
}
