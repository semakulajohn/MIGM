using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.WebViewModel
{
 public   class RequistionViewModel
    {
        public long RequistionId { get; set; }
     
        public double Amount { get; set; }
        public string ApprovedById { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
     
        public string RequistionNumber { get; set; }
      
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public string UpdatedBy { get; set; }
      
     
      
        public string StatusName { get; set; }
        public string ApprovedByName { get; set; }
        public long DocumentId { get; set; }
        public string AmountInWords { get; set; }

    }
}
