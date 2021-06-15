using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using log4net;
using System.Configuration;


namespace Higgs.Mbale.BAL.Concrete
{
  public  class WeightLossService : IWeightLossService
    {
       
        ILog logger = log4net.LogManager.GetLogger(typeof(WeightLossService));
        private IWeightLossDataService _dataService;
        private IUserService _userService;
      


        public WeightLossService(IWeightLossDataService dataService, IUserService userService
           )
        {
            this._dataService = dataService;
            this._userService = userService;
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WeightLossId"></param>
        /// <returns></returns>
        public WeightLoss GetWeightLoss(long weightLossId)
        {
            var result = this._dataService.GetWeightLoss(weightLossId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        public WeightLoss GetWeightLossForDelivery(long deliveryId)
        {
            var result = this._dataService.GetWeightLossForDelivery(deliveryId);
            return MapEFToModel(result);
        }

        public IEnumerable<WeightLoss> GetAllWeightLossesForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllWeightLossesForAParticularBranch(branchId);
            return MapEFToModel(results);

        }
        public IEnumerable<WeightLoss> GetAllWeightLossesForAParticularCustomer(string customerId)
        {
            var results = this._dataService.GetAllWeightLossesForAParticularCustomer(customerId);
            return MapEFToModel(results);

        }
       




        public long SaveWeightLoss(WeightLoss weightLoss, string userId)
        {
            
            long weightLossId = 0;
           
            var weightLossDTO = new DTO.WeightLossDTO()
                    {
                        
                        CustomerId = weightLoss.CustomerId,
                       
                        DeliveryId = weightLoss.DeliveryId,
                        
                        BranchId = weightLoss.BranchId,
                        
                        DeliveryDate = weightLoss.DeliveryDate,
                        Price = weightLoss.Price,
                        Quantity = weightLoss.Quantity,
                        
                        WeightLossId = weightLoss.WeightLossId,
                       
                        Deleted = weightLoss.Deleted,
                        CreatedBy = weightLoss.CreatedBy,
                        CreatedOn = weightLoss.CreatedOn,
                        Approved = weightLoss.Approved,


                    };

            weightLossId = this._dataService.SaveWeightLoss(weightLossDTO, userId);

            return weightLossId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WeightLossId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long weightLossId, string userId)
        {
            _dataService.MarkAsDeleted(weightLossId, userId);
        }

       public void PurgeWeightLoss(long deliveryId, string userId)
        {
            _dataService.PurgeWeightLoss(deliveryId, userId);
        }
    
      

        #region Mapping Methods

        public IEnumerable<WeightLoss> MapEFToModel(IEnumerable<EF.Models.WeightLoss> data)
        {
            var list = new List<WeightLoss>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        /// <summary>
        /// Maps WeightLoss EF object to WeightLoss Model Object and
        /// returns the WeightLoss model object.
        /// </summary>
        /// <param name="result">EF WeightLoss object to be mapped.</param>
        /// <returns>WeightLoss Model Object.</returns>
        public WeightLoss MapEFToModel(EF.Models.WeightLoss data)
        {
            
            var weightLoss = new WeightLoss();
            
            if (data != null)
            {
               
                var customerName = string.Empty;
                var customer = _userService.GetAspNetUser(data.CustomerId);
                if (customer != null)
                {
                    customerName = customer.FirstName + ' ' + customer.LastName;
                }

                weightLoss = new WeightLoss()
                {
                    CustomerName = customerName,
                 
                    DeliveryId = data.DeliveryId,

                    BranchName = data.Branch != null ? data.Branch.Name : "",
              
                    BranchId = data.BranchId,
                  
                    WeightLossId = data.WeightLossId,
                    Quantity = data.Quantity,
                   
                    DeliveryDate = data.DeliveryDate,
                  
                    CreatedOn = Convert.ToDateTime(data.CreatedOn),
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                 
                    Price = data.Price,
                  
                    Approved = data.Approved,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                };
             
               
                return weightLoss;
            }
            return weightLoss;
        }


      
          #endregion
    }
}
