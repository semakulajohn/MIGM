using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;


namespace Higgs.Mbale.DAL.Interface
{
    public interface IDeliveryDataService
    {
        IEnumerable<Delivery> GetAllDeliveries();
        Delivery GetDelivery(long deliveryId);
        long SaveDelivery(DeliveryDTO delivery, string userId);
        void MarkAsDeleted(long deliveryId, string userId);
        IEnumerable<Delivery> GetAllDeliveriesForAParticularBranch(long branchId);
        IEnumerable<Delivery> GetAllDeliveriesForAParticularOrder(long orderId);
        void SaveDeliveryStock(DeliveryStockDTO deliveryStockDTO);
        IEnumerable<DeliveryStock> GetDeliveryStocksForDelivery(long deliveryId);
        void SaveDeliveryGradeSize(DeliveryGradeSizeDTO deliveryGradeSizeDTO);
        void PurgeDeliveryGradeSize(long deliveryId);
        void SaveDeliveryBatch(DeliveryBatchDTO deliveryBatchDTO);
        void SaveBatchDeliveryGradeSize(List<BatchDeliveryGradeSizeDTO> batchDeliveryGradeSizeDTOS);
        long UpdateDeliveryOnApprovalOrRejection(long deliveryId, bool approved, string userId);
        void PurgeBatchDeliveryGradeSize(long deliveryId);
        void PurgeDeliveryBatch(long deliveryId);

         IEnumerable<Delivery> GetAllUnApprovedDeliveries();
         IEnumerable<Delivery> GetAllRejectedDeliveries();

         IEnumerable<Delivery> GetAllApprovedDeliveries();
        IEnumerable<Delivery> GetAllBranchUnApprovedDeliveries(long branchId);
        IEnumerable<Delivery> GetAllBranchApprovedDeliveries(long branchId);
        IEnumerable<Delivery> GetAllBranchRejectedDeliveries(long branchId);


    }
}
