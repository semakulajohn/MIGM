using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IWeightLossService
    {
        WeightLoss GetWeightLoss(long weightLossId);
                       
         IEnumerable<WeightLoss> GetAllWeightLossesForAParticularBranch(long branchId);

         IEnumerable<WeightLoss> GetAllWeightLossesForAParticularCustomer(string customerId);
        
         long SaveWeightLoss(WeightLoss weightLoss, string userId);

        void MarkAsDeleted(long weightLossId, string userId);
      
        IEnumerable<WeightLoss> MapEFToModel(IEnumerable<EF.Models.WeightLoss> data);
        void PurgeWeightLoss(long deliveryId, string userId);
        WeightLoss GetWeightLossForDelivery(long deliveryId);

    }
}
