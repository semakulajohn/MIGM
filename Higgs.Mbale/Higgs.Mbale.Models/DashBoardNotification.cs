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
        public  Nullable<int> transactions { get; set; }
        public Nullable<int> requistions { get; set; }

        public Nullable<int> orders { get; set; }
    }
}
