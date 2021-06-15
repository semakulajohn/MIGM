using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using System.Configuration;

namespace Higgs.Mbale.Web.Controllers
{
    public class BuveraApiController : ApiController
    {
             private IBuveraService _buveraService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(BuveraApiController));
            private string userId = string.Empty;

            public BuveraApiController()
            {
            }

            public BuveraApiController(IBuveraService buveraService,IUserService userService)
            {
                this._buveraService = buveraService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetBuvera")]
            public Buvera GetBuvera(long buveraId)
            {
                return _buveraService.GetBuvera(buveraId);
            }

            [HttpGet]
            [ActionName("GetAllBuveras")]
            public IEnumerable<Buvera> GetAllBuveras()
            {
                return _buveraService.GetAllBuveras();
            }

            [HttpGet]
            [ActionName("GetAllBuverasForAparticularStore")]
            public IEnumerable<Buvera> GetAllBuverasForAparticularStore(long storeId)
            {
                return _buveraService.GetAllBuverasForAParticularStore(storeId);
            }

            [HttpGet]
            [ActionName("GetStoreBuveraStock")]
            public StoreGrade GetStoreBuveraStock(long storeId)
            {
                return _buveraService.GetStoreBuveraStock(storeId);
            }

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteBuvera(long buveraId)
            {
                _buveraService.MarkAsDeleted(buveraId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(Buvera model)
            {
                var BuveraId = _buveraService.SaveBuvera(model, userId);
                return BuveraId;
            }
        [HttpGet]
        [ActionName("GetAllDamagedBuverasForAparticularStore")]
        public IEnumerable<Buvera> GetAllDamagedBuverasForAparticularStore(long storeId,long buveraCategoryId)
        {
            return _buveraService.GetAllDamagedBuverasForAparticularStore(storeId, buveraCategoryId);

        }
    }
}
