using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class StoreGrade
    {
        
            public IEnumerable<StoreGradeSize> StoreSizeGrades { get; set; }
            public IEnumerable<StoreBuveraTransferGradeSize> StoreBuveraTransferGradeSizes { get; set; }
            public IEnumerable<StoreFlourTransferGradeSize> StoreFlourTransferGradeSizes { get; set; }
            public IEnumerable<StoreBuveraGradeSize> StoreBuveraSizeGrades { get; set; }
       
    }
}
