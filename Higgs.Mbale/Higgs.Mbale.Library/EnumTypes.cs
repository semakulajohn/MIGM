using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higgs.Mbale.Library
{
  public  class EnumTypes
    {
      public enum DocumentType : long
      {
          Receipt = 1,
          PaymentVoucher = 2,
          Invoice = 3,
          DeliveryNote = 4,
      }
    }
}
