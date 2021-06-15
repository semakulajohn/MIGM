using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class DebtorView
    {
        
        public string DebtorName { get; set; }
        public double Amount { get; set; }
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string BranchName { get; set; }
        
    }
}
