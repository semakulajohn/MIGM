using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface ICashSaleDataService
    {
        IEnumerable<CashSale> GetAllCashSales();
        CashSale GetCashSale(long cashSaleId);
        long SaveCashSale(CashSaleDTO cashSaleDTO, string userId);
        void MarkAsDeleted(long cashSaleId, string userId);
        IEnumerable<CashSale> GetAllCashSalesForAParticularStore(long storeId);
        IEnumerable<CashSale> GetAllCashSalesForAParticularBranch(long branchId);
        void SaveCashSaleGradeSize(CashSaleGradeSizeDTO cashSaleGradeSizeDTO);
        void PurgeCashSaleGradeSize(long cashSaleId);
       
        void SaveCashSaleBatch(CashSaleBatchDTO cashSaleBatch);
        IEnumerable<CashSaleBatch> GetAllBatchesForACashSale(long cashSaleId);
        IEnumerable<CashSale> GetTenLatestCashSalesForAParticularBranch(long branchId,int offSet,int pageSize);
        long Cancelled(CashSaleDTO cashSaleDTO, string userId);
        //void  PurgeCashSaleGradeSize(long cashSaleId);

        void SaveBatchCashSaleGradeSize(List<CashSaleBatchGradeSizeDTO> batchCashSaleGradeSizeDTOS);

    }
}
