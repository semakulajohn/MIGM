using System;
using System.Collections.Generic;

namespace Higgs.Mbale.Models
{
   public class Delivery
    {
        public long DeliveryId { get; set; }
        public string CustomerId { get; set; }
        public string DriverName { get; set; }
        public Nullable<double> Price { get; set; }
        public string CreatedById { get; set; }
        public long ProductId { get; set; }
        public string ApprovedById { get; set; }
        public double DeliveryCost { get; set; }
        public string DriverNIN { get; set; }
        public string VehicleNumber { get; set; }
        public long OrderId { get; set; }
        public long TransactionSubTypeId { get; set; }
        public long MediaId { get; set; }
        public long BranchId { get; set; }
        public long SectorId { get; set; }
        public double Amount { get; set; }
        public long StoreId { get; set; }
        public long PaymentModeId { get; set; }
        public string Location { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public double Quantity { get; set; }
        public List<Grade> SelectedDeliveryGrades { get; set; }
        public List<Grade> SelectedGrades { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public Nullable<bool> Approved { get; set; }

        public List<Grade> Grades { get; set; }
        public List<Batch> Batches { get; set; }
        public List<DeliveryBatch> DeliveryBatches { get; set; }
        
       public string TransactionSubTypeName { get; set; }
       public string CustomerName { get; set; }
       public string ProductName { get; set; }
       public string BranchName { get; set; }
       public string SectorName { get; set; }
       public string StoreName { get; set; }

        public double WeightLoss { get; set; }

        public string PaymentModeName { get; set; }
       public string OrderNumber { get; set; }
       public long DocumentId { get; set; }

        public IEnumerable<BatchToDeliverGrades> BatchGradesToDeliver { get; set; }
      
    }
}
