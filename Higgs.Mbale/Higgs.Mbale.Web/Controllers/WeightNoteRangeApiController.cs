using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Web.Controllers
{
    public class WeightNoteRangeApiController : ApiController
    {
        private IWeightNoteRangeService _weightNoteRangeService;
        private IUserService _userService;
        ILog logger = log4net.LogManager.GetLogger(typeof(WeightNoteRangeApiController));
        private string userId = string.Empty;

        public WeightNoteRangeApiController()
        {
        }

        public WeightNoteRangeApiController(IWeightNoteRangeService weightNoteRangeService, IUserService userService)
        {
            this._weightNoteRangeService = weightNoteRangeService;
            this._userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [ActionName("GetWeightNoteRange")]
        public WeightNoteRange GetWeightNoteRange(long WeightNoteRangeId)
        {
            return _weightNoteRangeService.GetWeightNoteRange(WeightNoteRangeId);
        }

        [HttpGet]
        [ActionName("GetAllWeightNoteRanges")]
        public IEnumerable<WeightNoteRange> GetAllWeightNoteRanges()
        {
            return _weightNoteRangeService.GetAllWeightNoteRanges();
        }

        [HttpGet]
        [ActionName("GetAllWeightNoteRangesForWeb")]
        public IEnumerable<WeightNoteRangeViewModel> GetAllWeightNoteRangeViewModel()
        {
            return _weightNoteRangeService.GetAllWeightNoteRangeViewModel();
        }
    
        [HttpGet]
        [ActionName("GetAllWeightNoteRangesForAParticularBranch")]
        public IEnumerable<WeightNoteRangeViewModel> GetAllWeightNoteRangesForAParticularBranch(long branchId)
        {
            return _weightNoteRangeService.GetAllPrintedWeightNoteRangesForAParticularBranch(branchId);
        }
        [HttpGet]
        [ActionName("GetAllPrintedWeightNoteRangesForAParticularBranch")]
        public IEnumerable<WeightNoteRangeViewModel> GetAllPrintedWeightNoteRangesForAParticularBranch(long branchId)
        {
            return _weightNoteRangeService.GetAllPrintedWeightNoteRangesForAParticularBranch(branchId);
        }

        [HttpGet]
        [ActionName("GetAllPrintedWeightNoteRanges")]
        public IEnumerable<WeightNoteRangeViewModel> GetAllPrintedWeightNoteRanges()
        {
            return _weightNoteRangeService.GetAllPrintedWeightNoteRanges();
        }

        [HttpGet]
        [ActionName("GenerateWeightNoteNumbers")]
        public long GenerateWeightNoteNumbers(long weightNoteRangeId)
        {
          var weightNoteRangeIdValue =   _weightNoteRangeService.GenerateWeightNoteNumbers(weightNoteRangeId, userId);
            return weightNoteRangeIdValue;
        }

        [HttpGet]
        [ActionName("Delete")]
        public void DeleteWeightNoteRange(long weightNoteRangeId)
        {
            _weightNoteRangeService.MarkAsDeleted(weightNoteRangeId, userId);
        }
        [HttpPost]
        [ActionName("Save")]
        public long Save(WeightNoteRange model)
        {
            var weightNoteRangeId = _weightNoteRangeService.SaveWeightNoteRange(model, userId);
            return weightNoteRangeId;
        }
    }
}
