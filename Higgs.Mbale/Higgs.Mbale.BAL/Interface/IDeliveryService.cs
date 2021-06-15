using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;
using Higgs.Mbale.Models.WebViewModel;

namespace Higgs.Mbale.BAL.Interface
{
    public interface IDeliveryService
    {
        IEnumerable<Delivery> GetAllDeliveries();
        Delivery GetDelivery(long deliveryId);
        long SaveDelivery(Delivery delivery, string userId);
        void MarkAsDeleted(long deliveryId, string userId);
        IEnumerable<Delivery> GetAllDeliveriesForAParticularBranch(long branchId);
        IEnumerable<Delivery> GetAllDeliveriesForAParticularOrder(long orderId);
        IEnumerable<Delivery> MapEFToModel(IEnumerable<EF.Models.Delivery> data);
        IEnumerable<DeliveryViewModel> GetAllUnApprovedDeliveries();
        IEnumerable<Delivery> GetAllRejectedDeliveries();

        IEnumerable<Delivery> GetAllApprovedDeliveries();

        IEnumerable<Delivery> GetAllBranchUnApprovedDeliveries(long branchId);
        IEnumerable<Delivery> GetAllBranchRejectedDeliveries(long branchId);
        IEnumerable<Delivery> GetAllBranchApprovedDeliveries(long branchId);

    }
}
