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
    public class MillingChargeApiController : ApiController
    {
        
         private IMillingChargeService _millingChargeService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(MillingChargeApiController));
            private string userId = string.Empty;

            public MillingChargeApiController()
            {
            }

            public MillingChargeApiController(IMillingChargeService millingChargeService,IUserService userService)
            {
                this._millingChargeService = millingChargeService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetMillingCharge")]
            public MillingCharge GetMillingCharge(long millingChargeId)
            {
                return _millingChargeService.GetMillingCharge(millingChargeId);
            }

            [HttpGet]
            [ActionName("GetAllMillingCharges")]
            public IEnumerable<MillingCharge> GetAllMillingCharges()
            {
                return _millingChargeService.GetAllMillingCharges();
            }

            //[HttpGet]
            //[ActionName("GetAllMillingChargesForAParticularBranch")]
            //public IEnumerable<MillingCharge> GetAllMillingChargesForAParticularBranch(long branchId)
            //{
            //    return _millingChargeService.GetAllMillingChargesForAParticularBranch(branchId);
            //}

            //[HttpGet]
            //[ActionName("GetLatestMillingChargesForAParticularBranch")]
            //public IEnumerable<MillingCharge> GetLatestMillingChargesForAParticularBranch(long branchId)
            //{
            //    return _millingChargeService.GetTenMillingChargesForAParticularBranch(branchId);
            //}

         

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteMillingCharge(long millingChargeId)
            {
                _millingChargeService.MarkAsDeleted(millingChargeId, userId);
            }

         

            [HttpPost]
            [ActionName("Save")]
            public long Save(MillingCharge model)
            {

                var millingChargeId = _millingChargeService.SaveMillingCharge(model, userId);
                return millingChargeId;
            }
    }
}
