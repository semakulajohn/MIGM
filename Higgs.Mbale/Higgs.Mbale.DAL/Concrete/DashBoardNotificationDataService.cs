using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.EF.UnitOfWork;
using EntityFramework.Extensions;

namespace Higgs.Mbale.DAL.Concrete
{
    public class DashBoardNotificationDataService : DataServiceBase, IDashBoardDataService
    {
        public DashBoardNotificationDataService(IUnitOfWork<MbaleEntities> unitOfWork)
           : base(unitOfWork)
        {

        }


       

    
        public GetDashBoardNotifications_Result GetDashBoardNotifications()
        {

            using (var dbContext = new MbaleEntities())
            {
                var dasboardNotifications = dbContext.GetDashBoardNotifications().ToList();
                return dasboardNotifications.FirstOrDefault();
            }
            
        }
    }
}
