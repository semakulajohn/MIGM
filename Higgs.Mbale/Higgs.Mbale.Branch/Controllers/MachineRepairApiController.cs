using System;
using System.Collections.Generic;

using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class MachineRepairApiController : ApiController
    {
         private IMachineRepairService _machineRepairService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(MachineRepairApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public MachineRepairApiController()
            {
            }

            public MachineRepairApiController(IMachineRepairService MachineRepairService,IUserService userService)
            {
                this._machineRepairService = MachineRepairService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }

            [HttpGet]
            [ActionName("GetMachineRepair")]
            public MachineRepair GetMachineRepair(long machineRepairId)
            {
                return _machineRepairService.GetMachineRepair(machineRepairId);
            }



            [HttpGet]
            [ActionName("GetAllMachineRepairsForAParticularBatch")]
            public IEnumerable<MachineRepair> GetAllMachineRepairsForAParticularBatch(long batchId)
            {
                return _machineRepairService.GetAllMachineRepairsForAParticularBatch(batchId);
            }
            [HttpGet]
            [ActionName("Delete")]
            public void DeleteMachineRepair(long machineRepairId)
            {
                _machineRepairService.MarkAsDeleted(machineRepairId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(MachineRepair model)
            {
                model.BranchId = branchId;
                var machineRepairId = _machineRepairService.SaveMachineRepair(model, userId);
                return machineRepairId;
            }
    }
}
