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
  public  class MaizeOffloadingService : IMaizeOffloadingService
    {
      
         ILog logger = log4net.LogManager.GetLogger(typeof(MaizeOffloadingService));
        private IMaizeOffloadingDataService _dataService;
        private IUserService _userService;
        

        public MaizeOffloadingService(IMaizeOffloadingDataService dataService,IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaizeOffloadingId"></param>
        /// <returns></returns>
        public MaizeOffloading GetMaizeOffloading(long maizeOffloadingId)
        {
            var result = this._dataService.GetMaizeOffloading(maizeOffloadingId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MaizeOffloading> GetAllMaizeOffloadings()
        {
            var results = this._dataService.GetAllMaizeOffloadings();
            return MapEFToModel(results);
        } 

       
        public long SaveMaizeOffloading(MaizeOffloading maizeOffloading, string userId)
        {
            var maizeOffloadingDTO = new DTO.MaizeOffloadingDTO()
            {
                MaizeOffloadingId = maizeOffloading.MaizeOffloadingId,
                Amount = maizeOffloading.Amount,
                StartAmount = maizeOffloading.StartAmount,
                Notes = maizeOffloading.Notes,
                Action = maizeOffloading.Action,
                Balance = maizeOffloading.Balance,

                TransactionSubTypeId = maizeOffloading.TransactionSubTypeId,
                BranchId = maizeOffloading.BranchId,
                SectorId = maizeOffloading.SectorId,
                CreatedOn = maizeOffloading.CreatedOn,
                TimeStamp = maizeOffloading.TimeStamp,
               WeightNoteNumber  = maizeOffloading.WeightNoteNumber,
               SupplyId = maizeOffloading.SupplyId,
              
                Deleted = maizeOffloading.Deleted,
                CreatedBy = maizeOffloading.CreatedBy,
               
                DeletedOn = maizeOffloading.DeletedOn,
                
              
            };

           var maizeOffloadingId = this._dataService.SaveMaizeOffloading(maizeOffloadingDTO, userId);

           return maizeOffloadingId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaizeOffloadingId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long maizeOffloadingId, string userId)
        {
            _dataService.MarkAsDeleted(maizeOffloadingId, userId);
        }

      
        #region Mapping Methods

        private IEnumerable<MaizeOffloading> MapEFToModel(IEnumerable<EF.Models.MaizeOffloading> data)
        {
            var list = new List<MaizeOffloading>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps MaizeOffloading EF object to MaizeOffloading Model Object and
        /// returns the MaizeOffloading model object.
        /// </summary>
        /// <param name="result">EF MaizeOffloading object to be mapped.</param>
        /// <returns>MaizeOffloading Model Object.</returns>
        public MaizeOffloading MapEFToModel(EF.Models.MaizeOffloading data)
        {
            if (data != null)
            {
                var maizeOffloading = new MaizeOffloading()
                {
                    MaizeOffloadingId = data.MaizeOffloadingId,
                    Amount = data.Amount,
                    StartAmount = data.StartAmount,
                    Notes = data.Notes,
                    Action = data.Action,
                    Balance = data.Balance,

                    TransactionSubTypeId = data.TransactionSubTypeId,
                    BranchId = data.BranchId,
                    SectorId = data.SectorId,
                  
                    SupplyId = data.SupplyId,
                    WeightNoteNumber = data.WeightNoteNumber,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                   
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    SectorName = data.Sector != null ? data.Sector.Name : "",
                   

                };
                return maizeOffloading;
            }
            return null;
        }



       #endregion
    }
}
