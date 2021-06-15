using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
   public  class AspNetUserProduct
    {
        public string Id { get; set; }
        public long ProductId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string AspNetUserName { get; set; }
        public string ProductName { get; set; }

    }
}
