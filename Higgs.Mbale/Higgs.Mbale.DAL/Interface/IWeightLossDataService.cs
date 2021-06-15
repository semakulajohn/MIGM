using System;
using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
  public  interface IWeightLossDataService
    {
         WeightLoss GetWeightLoss(long weightLossId);

        IEnumerable<WeightLoss> GetAllWeightLossesForAParticularBranch(long branchId);



        IEnumerable<WeightLoss> GetAllWeightLossesForAParticularDelivery(long deliveryId);


        IEnumerable<WeightLoss> GetAllWeightLossesForAParticularCustomer(string customerId);

        long SaveWeightLoss(WeightLossDTO weightLossDTO, string userId);

        void PurgeWeightLoss(long deliveryId, string userId);

        void MarkAsDeleted(long weightLossId, string userId);
        WeightLoss GetWeightLossForDelivery(long deliveryId);


    }
}
