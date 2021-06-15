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
 public   class BankService : IBankService
    {
          ILog logger = log4net.LogManager.GetLogger(typeof(BankService));
        private IBankDataService _dataService;
        private IUserService _userService;
        

        public BankService(IBankDataService dataService,IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BankId"></param>
        /// <returns></returns>
        public Bank GetBank(long bankId)
        {
            var result = this._dataService.GetBank(bankId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Bank> GetAllBanks()
        {
            var results = this._dataService.GetAllBanks();
            return MapEFToModel(results);
        } 

       
        public long SaveBank(Bank bank, string userId)
        {
            var bankDTO = new DTO.BankDTO()
            {
                BankId = bank.BankId,
                Name = bank.Name,
                AccountNumber = bank.AccountNumber,
                Deleted = bank.Deleted,
                CreatedBy = bank.CreatedBy,
                CreatedOn = bank.CreatedOn,
                DeletedOn = bank.DeletedOn,
                
              
            };

           var bankId = this._dataService.SaveBank(bankDTO, userId);

           return bankId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BankId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long bankId, string userId)
        {
            _dataService.MarkAsDeleted(bankId, userId);
        }

      
        #region Mapping Methods

        private IEnumerable<Bank> MapEFToModel(IEnumerable<EF.Models.Bank> data)
        {
            var list = new List<Bank>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Bank EF object to Bank Model Object and
        /// returns the Bank model object.
        /// </summary>
        /// <param name="result">EF Bank object to be mapped.</param>
        /// <returns>Bank Model Object.</returns>
        public Bank MapEFToModel(EF.Models.Bank data)
        {
            if (data != null)
            {
                var bank = new Bank()
                {
                    BankId = data.BankId,
                    Name = data.Name,
                    AccountNumber = data.AccountNumber,
                   
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                   
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),


                };
                return bank;
            }
            return null;
        }



       #endregion
    }
}
