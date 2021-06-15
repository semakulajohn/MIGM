using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
   public  interface IDashBoardDataService
    {
        
        GetDashBoardNotifications_Result GetDashBoardNotifications();


    }
}
