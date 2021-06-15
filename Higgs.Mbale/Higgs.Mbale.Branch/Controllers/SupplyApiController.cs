using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Branch.Controllers
{
    public class SupplyApiController : ApiController
    {
       private ISupplyService _SupplyService;
            private IUserService _userService;
        private IStoreService _storeService;
        private IWeightNoteNumberService _weightNoteNumberService;
        ILog logger = log4net.LogManager.GetLogger(typeof(SupplyApiController));
            private string userId = string.Empty;
            long branchId = 0,storeId =0;

            public SupplyApiController()
            {
            }

            public SupplyApiController(ISupplyService SupplyService,IUserService userService,IWeightNoteNumberService weightNoteNumberService, IStoreService storeService)
            {
                this._SupplyService = SupplyService;
                this._userService = userService;
            this._weightNoteNumberService = weightNoteNumberService;
            this._storeService = storeService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            storeId = _storeService.GetAStoreForAParticularBranch(branchId);
        }

            [HttpGet]
            [ActionName("GetSupply")]
            public Supply GetSupply(long supplyId)
            {
                return _SupplyService.GetSupply(supplyId);
            }

          

               [HttpGet]
             [ActionName("GetAllSuppliesToBeUsed")]
            public IEnumerable<Supply> GetAllSuppliesToBeUsed()
            {
                return _SupplyService.GetAllSuppliesToBeUsed();
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
            public IEnumerable<Supply> GetAllSuppliesForAParticularBranch()
            {
                return _SupplyService.GetAllSuppliesForAParticularBranch(branchId);
            }

             [HttpGet]
             [ActionName("GetAllSuppliesToBeUsedForAParticularBranch")]
            public IEnumerable<Supply> GetAllSuppliesToBeUsedForAParticularBranch()
            {
                return _SupplyService.GetAllSuppliesToBeUsedForAParticularBranch(branchId);
            }
         
          [HttpGet]
          [ActionName("GetAllUnApprovedSuppliesForABranch")]
          public IEnumerable<Supply> GetAllUnApprovedSuppliesForABranch()
            {
                return _SupplyService.GetAllUnApprovedSuppliesForABranch(branchId);
            }
         [HttpGet]
         [ActionName("GetAllMaizeStocksForAparticularStore")]
            public IEnumerable<StoreMaizeStock> GetAllMaizeStocksForAparticularStore(long storeId)
            {
                return _SupplyService.GetMaizeStocksForAParticularStore(storeId);
            }
        [HttpGet]
        [ActionName("GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch")]
        public IEnumerable<WeightNoteNumberViewModel> GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch()
        {
            return _weightNoteNumberService.GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch(branchId);
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
                model.BranchId = branchId;
            model.StoreId = storeId;
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
