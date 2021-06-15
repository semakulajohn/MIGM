using System;

namespace Higgs.Mbale.Models
{
 public   class Asset
    {
        public long AssetId { get; set; }
        public long AssetCategoryId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public double AssetCount { get; set; }
        public System.DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string Name { get; set; }

        public string BranchName { get; set; }
        public string AssetCategoryName { get; set; }

    }
}
