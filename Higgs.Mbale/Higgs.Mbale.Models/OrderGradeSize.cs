using System;

namespace Higgs.Mbale.Models
{
    public class OrderGradeSize
    {
        public long OrderId { get; set; }
        public long GradeId { get; set; }
        public long SizeId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public  Grade Grade { get; set; }
        public  Order Order { get; set; }
        public  Size Size { get; set; }
        public Nullable<double> Balance { get; set; }
    
    }
}
