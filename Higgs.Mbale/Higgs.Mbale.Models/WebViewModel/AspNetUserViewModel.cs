using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.WebViewModel
{
 public   class AspNetUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }
        public string UniqueNumber { get; set; }
     
        public Nullable<long> RegionId { get; set; }
        
    }
}
