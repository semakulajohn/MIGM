using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class UtilityApiController : ApiController
    {
        
         private IUtilityService _utilityService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(UtilityApiController));
            private string userId = string.Empty;

            public UtilityApiController()
            {
            }

            public UtilityApiController(IUtilityService utilityService,IUserService userService)
            {
                this._utilityService = utilityService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetUtility")]
            public Utility GetUtility(long utilityId)
            {
                return _utilityService.GetUtility(utilityId);
            }

            [HttpGet]
            [ActionName("GetAllUtilities")]
            public IEnumerable<Utility> GetAllUtilities()
            {
                return _utilityService.GetAllUtilities();
            }

            [HttpGet]
            [ActionName("GetAllUtilitiesForAParticularBatch")]
            public IEnumerable<Utility> GetAllUtilitiesForAParticularBatch(long batchId)
            {
                return _utilityService.GetAllUtilitiesForAParticularBatch(batchId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteUtility(long utilityId)
            {
                _utilityService.MarkAsDeleted(utilityId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(Utility model)
            {
                var utilityId = _utilityService.SaveUtility(model, userId);
                return utilityId;
            }
    }
}
