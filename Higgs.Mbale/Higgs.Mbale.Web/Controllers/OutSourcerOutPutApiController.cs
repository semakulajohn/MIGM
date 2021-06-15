using System.Collections.Generic;

using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class OutSourcerOutPutApiController : ApiController
    {
        private IOutSourcerOutPutService _outSourcerOutPutService;
        private IUserService _userService;
        private string userId = string.Empty;

        public OutSourcerOutPutApiController()
        {
        }

        public OutSourcerOutPutApiController(IOutSourcerOutPutService outSourcerOutPutService, IUserService userService)
        {
            this._outSourcerOutPutService = outSourcerOutPutService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetOutSourcerOutPut")]
        public OutSourcerOutPut GetOutSourcerOutPut(long outSourcerOutPutId)
        {
            return _outSourcerOutPutService.GetOutSourcerOutPut(outSourcerOutPutId);
        }

        [HttpGet]
        [ActionName("GetAllOutSourcerOutPuts")]
        public IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPuts()
        {
            return _outSourcerOutPutService.GetAllOutSourcerOutPuts();
        }

        [HttpGet]
        [ActionName("GetAllOutSourcerOutPutsForAParticularOutSourcerStore")]
        public IEnumerable<OutSourcerOutPut> GetAllOutSourcerOutPutsForAParticularOutSourcer(long storeId)
        {
            return _outSourcerOutPutService.GetAllOutSourcerOutPutsForAParticularOutSourcerStore(storeId);
        }

        [HttpGet]
        [ActionName("GetAllUnApprovedOutSourcerOutPuts")]
        public IEnumerable<OutSourcerOutPut> GetAllUnApprovedOutSourcerOutPuts()
        {
            return _outSourcerOutPutService.GetAllUnApprovedOutSourcerOutPuts();
        }
        [HttpGet]
        [ActionName("GetAllApprovedOutSourcerOutPuts")]
        public IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPuts()
        {
            return _outSourcerOutPutService.GetAllApprovedOutSourcerOutPuts();
        }
        [HttpGet]
        [ActionName("GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore")]
        public IEnumerable<OutSourcerOutPut> GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore(long storeId)
        {
            return _outSourcerOutPutService.GetAllApprovedOutSourcerOutPutsForAParticularOutSourcerStore(storeId);
        }
        [HttpGet]
        [ActionName("Delete")]
        public void DeleteOutSourcerOutPut(long outSourcerOutPutId)
        {
            _outSourcerOutPutService.MarkAsDeleted(outSourcerOutPutId, userId);
        }



        [HttpPost]
        [ActionName("Save")]
        public long Save(OutSourcerOutPut model)
        {

            var outSourcerOutPutId = _outSourcerOutPutService.SaveOutSourcerOutPut(model, userId);
            return outSourcerOutPutId;
        }
    }
}
