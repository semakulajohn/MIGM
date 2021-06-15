using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;


namespace Higgs.Mbale.Branch.Controllers
{
    public class BuveraApiController : ApiController
    {
             private IBuveraService _buveraService;
            private IUserService _userService;
            private IStoreService _storeService;
            ILog logger = log4net.LogManager.GetLogger(typeof(BuveraApiController));
            private string userId = string.Empty;
            long branchId = 0,storeId =0;

            public BuveraApiController()
            {
            }

            public BuveraApiController(IBuveraService buveraService,IUserService userService,IStoreService storeService)
            {
                this._buveraService = buveraService;
                this._userService = userService;
                this._storeService = storeService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
                storeId = _storeService.GetAStoreForAParticularBranch(branchId);
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
            public IEnumerable<Buvera> GetAllBuverasForAparticularStore()
            {
                return _buveraService.GetAllBuverasForAParticularStore(storeId);
            }

            [HttpGet]
            [ActionName("GetAllDamagedBuverasForAparticularStore")]
            public IEnumerable<Buvera> GetAllDamagedBuverasForAparticularStore(long buveraCategoryId)
            {
                return _buveraService.GetAllDamagedBuverasForAparticularStore(storeId,buveraCategoryId);
           
            }
       

            [HttpGet]
            [ActionName("GetStoreBuveraStock")]
            public StoreGrade GetStoreBuveraStock()
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
                model.BranchId = branchId;
                model.StoreId = storeId;
                var BuveraId = _buveraService.SaveBuvera(model, userId);
                return BuveraId;
            }
    }
}
