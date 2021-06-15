using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class StockApiController : ApiController
    {
            private IStockService _stockService;
            private IUserService _userService;
            private IStoreService _storeService;
            ILog logger = log4net.LogManager.GetLogger(typeof(StockApiController));
            private string userId = string.Empty;
            long branchId = 0, storeId = 0;

            public StockApiController()
            {
            }

            public StockApiController(IStockService stockService,IUserService userService,IStoreService storeService)
            {
                this._stockService = stockService;
                this._userService = userService;
                this._storeService = storeService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
                storeId = _storeService.GetAStoreForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetStock")]
            public Stock GetStock(long stockId)
            {
                return _stockService.GetStock(stockId);
            }

            [HttpGet]
            [ActionName("GetAllStocks")]
            public IEnumerable<Stock> GetAllStocks()
            {
                return _stockService.GetAllStocks();
            }

            [HttpGet]
            [ActionName("GetAllStocksForAparticularBranch")]
            public IEnumerable<Stock> GetAllStocksForAparticularBranch()
            {
                return _stockService.GetAllStocksForAParticularBranch(branchId);
            }

            [HttpGet]
            [ActionName("GetAllStocksForAparticularStore")]
            public IEnumerable<StoreStock> GetAllStocksForAparticularStore()
            {
                return _stockService.GetStocksForAParticularStore(storeId);
            }
            [HttpGet]
            [ActionName("GetStoreFlourStock")]
            public StoreGrade GetStoreFlourStock()
            {
                return _stockService.GetStoreFlourStock(storeId);
            }
          

         
    }
}
