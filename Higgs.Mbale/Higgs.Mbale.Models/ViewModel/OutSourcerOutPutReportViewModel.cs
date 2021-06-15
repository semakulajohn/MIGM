using System;
using System.Collections.Generic;


namespace Higgs.Mbale.Models.ViewModel
{
  public  class OutSourcerOutPutReportViewModel
    {
        public IEnumerable<OutSourcerOutPut> OutSourcerOutPuts { get; set; }
        public double TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        
    }
}
