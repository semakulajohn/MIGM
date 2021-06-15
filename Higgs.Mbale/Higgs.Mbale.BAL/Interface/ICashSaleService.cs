using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Interface
{
  public  interface ICashSaleService
    {
      IEnumerable<CashSale> GetAllCashSales();
      CashSale GetCashSale(long cashSaleId);
      long SaveCashSale(CashSale cashSale, string userId);
      void MarkAsDeleted(long cashSaleId, string userId);
      IEnumerable<CashSale> GetAllCashSalesForAParticularStore(long storeId);
      IEnumerable<CashSale> GetAllCashSalesForAParticularBranch(long branchId);
      IEnumerable<CashSale> MapEFToModel(IEnumerable<EF.Models.CashSale> data);

        IEnumerable<CashSaleViewModel> MapEFCashSaleToModelCashSaleViewModel(IEnumerable<EF.Models.CashSale> data);

        IEnumerable<CashSaleViewModel> GetTenLatestCashSalesForAParticularBranch(long branchId,int offSet, int pageSize);
      long Cancelled(CashSale cashSale, string userId);
    }
}
