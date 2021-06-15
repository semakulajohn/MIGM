using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Models.ViewModel
{
 public   class FlourTransferReportTotalModel
    {
        public long GradeId { get; set; }
     
        public long SizeId { get; set; }     

        public int SizeValue { get; set; }
        public string GradeValue { get; set; }

        public List<Grade> Grades { get; set; }
      
     public  int TotalSuperTenQuantity {get;set;}
     public int TotalSuperTwoFiveQuantity {get; set;}
     public int TotalSuperFiveZeroQuantity {get; set;}
     public int TotalSuperHundredQuantity {get; set;}
     public int TotalSuperFiveQuantity {get; set;}
     public int TotalSuperOneQuantity {get; set;}
     public int TotalNumberOneTenQuantity {get; set;}
     public int TotalNumberOneTwoFiveQuantity {get; set;}
     public int  TotalNumberOneFiveZeroQuantity {get; set;}
     public int TotalNumberOneHundredQuantity {get; set;}
     public int TotalNumberOneFiveQuantity{get; set;}
     public int TotalNumberOneOneQuantity {get; set;}
      public int TotalNumberOneHalfTenQuantity {get; set;}
     public int TotalNumberOneHalfTwoFiveQuantity {get; set;}
     public int TotalNumberOneHalfFiveZeroQuantity {get; set;}
     public int TotalNumberOneHalfHundredQuantity {get; set;}
     public int TotalNumberOneHalfFiveQuantity {get; set;}
     public int TotalNumberOneHalfOneQuantity { get; set; }
     public int TotalKabaleTenQuantity {get; set;}
     public int TotalKabaleTwoFiveQuantity{get; set;}
     public int TotalKabaleFiveZeroQuantity {get; set;}
     public int  TotalKabaleHundredQuantity {get; set;}
     public int  TotalKabaleFiveQuantity {get; set;}
     public int  TotalKabaleOneQuantity {get; set;}
        

    }
}
