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

namespace Higgs.Mbale.BAL.Concrete
{
 public   class UtilityService : IUtilityService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(UtilityService));
        private IUtilityDataService _dataService;
        private IUserService _userService;
        

        public UtilityService(IUtilityDataService dataService,IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UtilityId"></param>
        /// <returns></returns>
        public Utility GetUtility(long utilityId)
        {
            var result = this._dataService.GetUtility(utilityId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Utility> GetAllUtilities()
        {
            var results = this._dataService.GetAllUtilities();
            return MapEFToModel(results);
        }

        public IEnumerable<Utility> GetAllUtilitiesForAParticularBatch(long batchId)
        {
            var results = this._dataService.GetAllUtilitiesForAParticularBatch(batchId);
            return MapEFToModel(results);
        }
       
        public long SaveUtility(Utility utility, string userId)
        {
            var utilityDTO = new DTO.UtilityDTO()
            {
                UtilityId = utility.UtilityId,
                BatchId = utility.BatchId,
                Description = utility.Description,
                BranchId = utility.BranchId,
                Amount = utility.Amount,
                 Deleted = utility.Deleted,
                CreatedBy = utility.CreatedBy,
                CreatedOn = utility.CreatedOn,
                SectorId = utility.SectorId,

            };

           var utilityId = this._dataService.SaveUtility(utilityDTO, userId);

           return utilityId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UtilityId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long utilityId, string userId)
        {
            _dataService.MarkAsDeleted(utilityId, userId);
        }

      
        #region Mapping Methods

        public IEnumerable<Utility> MapEFToModel(IEnumerable<EF.Models.Utility> data)
        {
            var list = new List<Utility>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Utility EF object to Utility Model Object and
        /// returns the Utility model object.
        /// </summary>
        /// <param name="result">EF Utility object to be mapped.</param>
        /// <returns>Utility Model Object.</returns>
        public Utility MapEFToModel(EF.Models.Utility data)
        {
            if (data != null)
            {


                var utility = new Utility()
                {
                    UtilityId = data.UtilityId,
                    Amount = data.Amount,
                    SectorId = data.SectorId,
                    BranchId = data.BranchId,
                    BatchId = data.BatchId,
                    Description = data.Description,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),
                    BatchNumber = data.Batch != null ? data.Batch.Name : "",


                };
                return utility;
            }
            return null;
        }



       #endregion
    }
}
