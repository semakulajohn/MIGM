using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;
using System.Configuration;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.Branch.Controllers
{
    public class CashSaleApiController : ApiController
    {
            private ICashSaleService _cashSaleService;
            private IUserService _userService;
            private IStoreService _storeService;
            ILog logger = log4net.LogManager.GetLogger(typeof(CashSaleApiController));
            private string userId = string.Empty;
            long branchId = 0, storeId = 0;

            public CashSaleApiController()
            {
            }

            public CashSaleApiController(ICashSaleService cashSaleService, IUserService userService,IStoreService storeService)
            {
                this._cashSaleService = cashSaleService;
                this._userService = userService;
                this._storeService = storeService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
                storeId = _storeService.GetAStoreForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetCashSale")]
            public CashSale GetCashSale(long cashSaleId)
            {
                return _cashSaleService.GetCashSale(cashSaleId);
            }


            [HttpGet]
            [ActionName("GetAllCashSalesForAparticularStore")]
            public IEnumerable<CashSale> GetAllCashSalesForAparticularStore(long storeId)
            {
                return _cashSaleService.GetAllCashSalesForAParticularStore(storeId);
            }

           [HttpGet]
            [ActionName("GetAllCashSalesForAparticularBranch")]
            public IEnumerable<CashSaleViewModel> GetAllCashSalesForAparticularBranch(int offSet, int pageSize)
            {
                //return _cashSaleService.GetAllCashSalesForAParticularBranch(branchId);
                return _cashSaleService.GetTenLatestCashSalesForAParticularBranch(branchId,offSet,pageSize);

            }

          

            [HttpGet]
            [ActionName("Delete")]
            public void DeleteCashSale(long cashSaleId)
            {
                _cashSaleService.MarkAsDeleted(cashSaleId, userId);
            }

            [HttpPost]
            [ActionName("Save")]
            public long Save(CashSale model)
            {
                model.BranchId = branchId;
                model.StoreId = storeId;
                var cashSaleId = _cashSaleService.SaveCashSale(model, userId);
                return cashSaleId;
            }

           
        }
}