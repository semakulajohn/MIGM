using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Web.Controllers
{
    public class SupplyApiController : ApiController
    {
       private ISupplyService _SupplyService;
            private IUserService _userService;
        private IWeightNoteNumberService _weightNoteNumberService;
            ILog logger = log4net.LogManager.GetLogger(typeof(SupplyApiController));
            private string userId = string.Empty;

            public SupplyApiController()
            {
            }

            public SupplyApiController(ISupplyService SupplyService,IUserService userService,IWeightNoteNumberService weightNoteNumberService)
            {
                this._SupplyService = SupplyService;
                this._userService = userService;
            this._weightNoteNumberService = weightNoteNumberService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetSupply")]
            public Supply GetSupply(long supplyId)
            {
                return _SupplyService.GetSupply(supplyId);
            }

            [HttpGet]
            [ActionName("GetAllSupplies")]
            public IEnumerable<Supply> GetAllSupplies()
            {
                return _SupplyService.GetAllSupplies();
            }

            [HttpGet]
            [ActionName("GetAllUnApprovedSupplies")]
            public IEnumerable<Supply> GetAllUnApprovedSupplies()
            {
                return _SupplyService.GetAllUnApprovedSupplies();
            }
            [HttpGet]
            [ActionName("GetAllSuppliesToBeUsed")]
            public IEnumerable<Supply> GetAllSuppliesToBeUsed()
            {
                return _SupplyService.GetAllSuppliesToBeUsed();
            }

            [HttpGet]
            [ActionName("GetAllUnPaidSupplies")]
            public IEnumerable<Supply> GetAllUnPaidSupplies()
            {
                return _SupplyService.GetAllUnPaidSupplies();
            }
       
            [HttpGet]
            [ActionName("GetAllSuppliesForAParticularSupplier")]
            public IEnumerable<Supply> GetAllSuppliesForAParticularSupplier(string supplierId)
            {
                return _SupplyService.GetAllSuppliesForAParticularSupplier(supplierId);
            }

            [HttpGet]
            [ActionName("GetAllUnPaidSuppliesForAParticularSupplier")]
            public IEnumerable<Supply> GetAllUnPaidSuppliesForAParticularSupplier(string supplierId)
            {
                return _SupplyService.GetAllUnPaidSuppliesForAParticularSupplier(supplierId);
            }
            [HttpGet]
            [ActionName("GetAllPaidSuppliesForAParticularSupplier")]
            public IEnumerable<Supply> GetAllPaidSuppliesForAParticularSupplier(string supplierId)
            {
                return _SupplyService.GetAllPaidSuppliesForAParticularSupplier(supplierId);
            }

            [HttpGet]
            [ActionName("GetAllSuppliesForAParticularBranch")]
            public IEnumerable<Supply> GetAllSuppliesForAParticularBranch(long branchId)
            {
                return _SupplyService.GetAllSuppliesForAParticularBranch(branchId);
            }

        [HttpGet]
        [ActionName("GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch")]
        public IEnumerable<WeightNoteNumberViewModel> GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(long branchId)
        {
            return _weightNoteNumberService.GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(branchId);
        }
        [HttpGet]
         [ActionName("GetAllSuppliesToBeUsedForAParticularBranch")]
            public IEnumerable<Supply> GetAllSuppliesToBeUsedForAParticularBranch(long branchId)
            {
                return _SupplyService.GetAllSuppliesToBeUsedForAParticularBranch(branchId);
            }
         
          
         [HttpGet]
         [ActionName("GetAllMaizeStocksForAparticularStore")]
            public IEnumerable<StoreMaizeStock> GetAllMaizeStocksForAparticularStore(long storeId)
            {
                return _SupplyService.GetMaizeStocksForAParticularStore(storeId);
            }
        

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteSupply(long supplyId)
            {
                _SupplyService.MarkAsDeleted(supplyId, userId);
            }

         

            [HttpPost]
            [ActionName("Save")]
            public long Save(Supply model)
            {

                var supplyId = _SupplyService.SaveSupply(model, userId);
                return supplyId;
            }

            [HttpPost]
            [ActionName("PayMultipleSupplies")]
            public long PayMultipleSupplies(MultipleSupplies model)
            {

                var Id = _SupplyService.MakeSupplyPayment(model, model.AccountActivity, userId);
                return Id;
            }
    }
}
