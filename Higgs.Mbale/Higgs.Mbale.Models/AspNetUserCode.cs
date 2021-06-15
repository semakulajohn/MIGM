using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
    public class AspNetUserCode
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public int Code { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

    }
}
