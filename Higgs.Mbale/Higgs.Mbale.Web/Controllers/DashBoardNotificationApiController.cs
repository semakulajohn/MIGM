using System;
using System.Collections.Generic;
using Higgs.Mbale.Models ;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;

namespace Higgs.Mbale.Web.Controllers
{
    public class DashBoardNotificationApiController : ApiController
    {
        private IDashBoardNotificationService _dashBoardNotificationService;
       
       
        public DashBoardNotificationApiController()
        {
        }

        public DashBoardNotificationApiController(IDashBoardNotificationService dashBoardNotificationService)
        {
            this._dashBoardNotificationService = dashBoardNotificationService;
            
        }

       

        [HttpGet]
        [ActionName("GetDashBoardNotifications")]
        public DashBoardNotification GetDashBoardNotifications()
        {
            return _dashBoardNotificationService.GetDashBoardNotifications();
        }
    }
}