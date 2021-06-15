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
using System.Configuration;

namespace Higgs.Mbale.Web.Controllers
{
    public class CashSaleApiController : ApiController
    {
            private ICashSaleService _cashSaleService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(CashSaleApiController));
            private string userId = string.Empty;

            public CashSaleApiController()
            {
            }

            public CashSaleApiController(ICashSaleService cashSaleService, IUserService userService)
            {
                this._cashSaleService = cashSaleService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetCashSale")]
            public CashSale GetCashSale(long cashSaleId)
            {
                return _cashSaleService.GetCashSale(cashSaleId);
            }

            [HttpGet]
            [ActionName("GetAllCashSales")]
            public IEnumerable<CashSale> GetAllCashSales()
            {
                return _cashSaleService.GetAllCashSales();
            }

            [HttpGet]
            [ActionName("GetAllCashSalesForAparticularStore")]
            public IEnumerable<CashSale> GetAllCashSalesForAparticularStore(long storeId)
            {
                return _cashSaleService.GetAllCashSalesForAParticularStore(storeId);
            }

           [HttpGet]
            [ActionName("GetAllCashSalesForAparticularBranch")]
            public IEnumerable<CashSaleViewModel> GetAllCashSalesForAparticularBranch(long branchId,int offSet,int pageSize)
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
                var cashSaleId = _cashSaleService.SaveCashSale(model, userId);
                return cashSaleId;
            }

            [HttpPost]
            [ActionName("Cancelled")]
            public long Cancelled(CashSale model)
            {
                var cashSaleId = _cashSaleService.Cancelled(model, userId);
                return cashSaleId;
            }
           
        }
}