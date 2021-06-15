using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.WebViewModel
{
 public   class DeliveryViewModel
    {
        public long DeliveryId { get; set; }
        public string CustomerId { get; set; }
        public string DriverName { get; set; }
        public long ProductId { get; set; }
        public string VehicleNumber { get; set; }
        public long BranchId { get; set; }
        public double Amount { get; set; }
        public string Location { get; set; }   
        public double Quantity { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string BranchName { get; set; }
       
        
       
       
    }
}
