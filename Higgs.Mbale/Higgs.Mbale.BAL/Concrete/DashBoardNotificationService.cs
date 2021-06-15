using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Concrete
{
  public  class DashBoardNotificationService : IDashBoardNotificationService
    {
        
        private IDashBoardDataService _dataService;
       
        public DashBoardNotificationService(IDashBoardDataService dataService)
        {
            this._dataService = dataService;
           
        }
       

        public DashBoardNotification GetDashBoardNotifications()
        {
            DashBoardNotification dashBoardData = new DashBoardNotification();
            var result = _dataService.GetDashBoardNotifications();
            if (result != null)
            {
                dashBoardData = new DashBoardNotification()
                {
                    cashtransfers = result.cashtransfers,
                    supplies = result.supplies,
                    outsourceroutputs = result.outsourceroutputs,
                    deliveries = result.deliveries,
                    transactions = result.transactions,
                    requistions = result.requistions

                };
            }
            return dashBoardData;
        }
    }
}
