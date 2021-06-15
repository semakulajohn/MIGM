using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Helpers;
using log4net;
using System.Configuration;

namespace Higgs.Mbale.BAL.Concrete
{
 public   class MillingChargeService : IMillingChargeService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(MillingChargeService));
        private IMillingChargeDataService _dataService;
        private IUserService _userService;
        

        public MillingChargeService(IMillingChargeDataService dataService,IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MillingChargeId"></param>
        /// <returns></returns>
        public MillingCharge GetMillingCharge(long millingChargeId)
        {
            var result = this._dataService.GetMillingCharge(millingChargeId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MillingCharge> GetAllMillingCharges()
        {
            var results = this._dataService.GetAllMillingCharges();
            return MapEFToModel(results);
        } 

       
        public long SaveMillingCharge(MillingCharge millingCharge, string userId)
        {
            var millingChargeDTO = new DTO.MillingChargeDTO()
            {
                MillingChargeId = millingCharge.MillingChargeId,
                Amount = millingCharge.Amount,
                StartAmount = millingCharge.StartAmount,
                Notes = millingCharge.Notes,
                Action = millingCharge.Action,
                Balance = millingCharge.Balance,

                TransactionSubTypeId = millingCharge.TransactionSubTypeId,
                BranchId = millingCharge.BranchId,
                SectorId = millingCharge.SectorId,
                CreatedOn = millingCharge.CreatedOn,
                TimeStamp = millingCharge.TimeStamp,
              
                BatchId = millingCharge.BatchId,
                Deleted = millingCharge.Deleted,
                CreatedBy = millingCharge.CreatedBy,
               
                DeletedOn = millingCharge.DeletedOn,
                
              
            };

           var millingChargeId = this._dataService.SaveMillingCharge(millingChargeDTO, userId);

           return millingChargeId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MillingChargeId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long MillingChargeId, string userId)
        {
            _dataService.MarkAsDeleted(MillingChargeId, userId);
        }

      
        #region Mapping Methods

        private IEnumerable<MillingCharge> MapEFToModel(IEnumerable<EF.Models.MillingCharge> data)
        {
            var list = new List<MillingCharge>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps MillingCharge EF object to MillingCharge Model Object and
        /// returns the MillingCharge model object.
        /// </summary>
        /// <param name="result">EF MillingCharge object to be mapped.</param>
        /// <returns>MillingCharge Model Object.</returns>
        public MillingCharge MapEFToModel(EF.Models.MillingCharge data)
        {
            if (data != null)
            {
                var MillingCharge = new MillingCharge()
                {
                    MillingChargeId = data.MillingChargeId,
                    Amount = data.Amount,
                    StartAmount = data.StartAmount,
                    Notes = data.Notes,
                    Action = data.Action,
                    Balance = data.Balance,

                    TransactionSubTypeId = data.TransactionSubTypeId,
                    BranchId = data.BranchId,
                    SectorId = data.SectorId,
                  
                    BatchId = data.BatchId,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                   
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                   

                };
                return MillingCharge;
            }
            return null;
        }



       #endregion
    }
}
