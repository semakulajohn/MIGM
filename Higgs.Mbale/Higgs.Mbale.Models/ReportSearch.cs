using System;


namespace Higgs.Mbale.Models
{
 public   class ReportSearch
    {
       
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public string SupplierId { get; set; }
            public long BranchId { get; set; }
            public long FromBranchId { get; set; }
            public long ToBranchId { get; set; }
            public string CustomerId { get; set; }
            public long ProductId { get; set; }
            public long CategoryId { get; set; }
            public string Status { get; set; }
            public long TransactionSubTypeId { get; set; }
            public string Position { get; set; }
            public long RequistionCategoryId { get; set; }

        }
    
}
