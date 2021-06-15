using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class RiceInputApiController : ApiController
    {
        private IRiceInputService _riceInputService;
        private IUserService _userService;
        private string userId = string.Empty;

        public RiceInputApiController()
        {
        }

        public RiceInputApiController(IRiceInputService riceInputService, IUserService userService)
        {
            this._riceInputService = riceInputService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetRiceInput")]
        public RiceInput GetRiceInput(long riceInputId)
        {
            return _riceInputService.GetRiceInput(riceInputId);
        }

        [HttpGet]
        [ActionName("GetAllRiceInputs")]
        public IEnumerable<RiceInput> GetAllRiceInputs()
        {
            return _riceInputService.GetAllRiceInputs();
        }

        [HttpGet]
        [ActionName("GetAllRiceInputsForAParticularBranch")]
        public IEnumerable<RiceInput> GetAllRiceInputsForAParticularBranch(long branchId)
        {
            return _riceInputService.GetAllRiceInputsForAParticularBranch(branchId);
        }

        [HttpGet]
        [ActionName("GetAllUnApprovedRiceInputs")]
        public IEnumerable<RiceInput> GetAllUnApprovedRiceInputs()
        {
            return _riceInputService.GetAllUnApprovedRiceInputs();
        }
        [HttpGet]
        [ActionName("GetAllApprovedRiceInputs")]
        public IEnumerable<RiceInput> GetAllApprovedRiceInputs()
        {
            return _riceInputService.GetAllApprovedRiceInputs();
        }
        [HttpGet]
        [ActionName("GetAllApprovedRiceInputsForAParticularBranch")]
        public IEnumerable<RiceInput> GetAllApprovedRiceInputsForAParticularBranch(long branchId)
        {
            return _riceInputService.GetAllApprovedRiceInputsForAParticularBranch(branchId);
        }
        [HttpGet]
        [ActionName("Delete")]
        public void DeleteRiceInput(long riceInputId)
        {
            _riceInputService.MarkAsDeleted(riceInputId, userId);
        }



        [HttpPost]
        [ActionName("Save")]
        public long Save(RiceInput model)
        {

            var riceInputId = _riceInputService.SaveRiceInput(model, userId);
            return riceInputId;
        }
    }
}