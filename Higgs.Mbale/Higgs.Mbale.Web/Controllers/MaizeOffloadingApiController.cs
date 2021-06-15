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
    public class MaizeOffloadingApiController : ApiController
    {
        
         private IMaizeOffloadingService _maizeOffloadingService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(MaizeOffloadingApiController));
            private string userId = string.Empty;

            public MaizeOffloadingApiController()
            {
            }

            public MaizeOffloadingApiController(IMaizeOffloadingService maizeOffloadingService,IUserService userService)
            {
                this._maizeOffloadingService = maizeOffloadingService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetMaizeOffloading")]
            public MaizeOffloading GetMaizeOffloading(long maizeOffloadingId)
            {
                return _maizeOffloadingService.GetMaizeOffloading(maizeOffloadingId);
            }

            [HttpGet]
            [ActionName("GetAllMaizeOffloadings")]
            public IEnumerable<MaizeOffloading> GetAllMaizeOffloadings()
            {
                return _maizeOffloadingService.GetAllMaizeOffloadings();
            }

            //[HttpGet]
            //[ActionName("GetAllMaizeOffloadingsForAParticularBranch")]
            //public IEnumerable<MaizeOffloading> GetAllMaizeOffloadingsForAParticularBranch(long branchId)
            //{
            //    return _maizeOffloadingService.GetAllMaizeOffloadingsForAParticularBranch(branchId);
            //}

            //[HttpGet]
            //[ActionName("GetLatestMaizeOffloadingsForAParticularBranch")]
            //public IEnumerable<MaizeOffloading> GetLatestMaizeOffloadingsForAParticularBranch(long branchId)
            //{
            //    return _maizeOffloadingService.GetTenMaizeOffloadingsForAParticularBranch(branchId);
            //}

          
           

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteMaizeOffloading(long maizeOffloadingId)
            {
                _maizeOffloadingService.MarkAsDeleted(maizeOffloadingId, userId);
            }

         

            [HttpPost]
            [ActionName("Save")]
            public long Save(MaizeOffloading model)
            {

                var maizeOffloadingId = _maizeOffloadingService.SaveMaizeOffloading(model, userId);
                return maizeOffloadingId;
            }
    }
}
