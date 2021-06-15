using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models
{
    public class CashTransfer
    {
        public long CashTransferId { get; set; }
        public string Response { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public long ToReceiverBranchId { get; set; }
        public bool Accept { get; set; }
        public bool Reject { get; set; }
        public long FromBranchId { get; set; }
        public double Amount { get; set; }
        public string AmountInWords { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public long SectorId { get; set; }
        public string Notes { get; set; }
        public string   CreatedByName {get; set;}

        public string ReceiverBranch { get; set; }
        public string FromBranch { get; set; }
        public string Department { get; set; }
    }
}
