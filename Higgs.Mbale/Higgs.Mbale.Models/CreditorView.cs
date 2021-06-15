using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
 public   class CreditorView
    {
     public string CreditorNumber { get; set; }
     public string CreditorName { get; set; }
     public double Amount { get; set; }
     public string Id { get; set; }
        public string BranchName { get; set; }
    
    }
}
