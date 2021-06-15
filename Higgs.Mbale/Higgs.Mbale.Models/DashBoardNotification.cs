using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
  public  class DashBoardNotification
    {
        public Nullable<int> cashtransfers { get; set; }
        public Nullable<int> supplies { get; set; }
        public Nullable<int> deliveries { get; set; }
        public Nullable<int> outsourceroutputs { get; set; }
        public int transactions { get; set; }
        public int requistions { get; set; }
    }
}
