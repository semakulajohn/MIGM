using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class BatchFactoryExpense
    {
      public long BatchId { get; set; }
      public List<FactoryExpense> FactoryExpenses { get; set; }
      public long BranchId { get; set; }
      public long SectorId { get; set; }
    }
}
