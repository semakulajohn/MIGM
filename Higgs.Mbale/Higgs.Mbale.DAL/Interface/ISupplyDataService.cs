
using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
    public interface ISupplyDataService
    {
        IEnumerable<Supply> GetAllSupplies();
        Supply GetSupply(long supplyId);
        IEnumerable<Supply> GetAllSuppliesForAParticularSupplier(string supplierId);
        long SaveSupply(SupplyDTO supply, string userId);
        void MarkAsDeleted(long supplyId, string userId);
        IEnumerable<Supply> GetAllSuppliesForAParticularBranch(long branchId);
        IEnumerable<Supply> GetAllSuppliesToBeUsed();
        IEnumerable<Supply> GetAllUnPaidSuppliesForAParticularSupplier(string supplierId);
        IEnumerable<Supply> GetAllPaidSuppliesForAParticularSupplier(string supplierId);
        long UpdateBatchSupplyWithCompletedStatus(long supplyId, long statusId, string userId);
        long UpdateSupplyWithInProgressStatus(long supplyId, long statusId, string userId);
        void SaveStoreMaizeStock(StoreMaizeStockDTO storeMaizeStockDTO);

        IEnumerable<Supply> GetAllUnApprovedSuppliesForABranch(long branchId);

        StoreMaizeStock GetLatestMaizeStockForAParticularStore(long storeId);

        IEnumerable<StoreMaizeStock> GetMaizeStocksForAParticularStore(long storeId);
        IEnumerable<Supply> GetAllSuppliesToBeUsedForAParticularBranch(long branchId);
        IEnumerable<Supply> GetAllUnPaidSupplies();
        
        IEnumerable<Supply> GetAllUnApprovedSupplies();
        long UpdateSupplyOnRequistionAprroval(SupplyDTO supplyDTO, string userId);
        IEnumerable<Supply> GetAllApprovedSupplies();

        int CheckIfWeightNoteExists(string weightNoteNumber);
    }
  
}
