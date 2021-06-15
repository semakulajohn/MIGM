using System.Collections.Generic;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
    public class FinancialAccountService : IFinancialAccountService
    {

        ILog logger = log4net.LogManager.GetLogger(typeof(FinancialAccountService));
        private IFinancialAccountDataService _dataService;
        private IUserService _userService;


        public FinancialAccountService(IFinancialAccountDataService dataService, IUserService userService)
        {
            this._dataService = dataService;
            this._userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FinancialAccountId"></param>
        /// <returns></returns>
        public FinancialAccount GetFinancialAccount(long financialAccountId)
        {
            var result = this._dataService.GetFinancialAccount(financialAccountId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FinancialAccount> GetAllFinancialAccounts()
        {
            var results = this._dataService.GetAllFinancialAccounts();
            return MapEFToModel(results);
        }

       
        public long SaveFinancialAccount(FinancialAccount financialAccount, string userId)
        {
            var financialAccountDTO = new DTO.FinancialAccountDTO()
            {
                Name = financialAccount.Name,
                AccountNumber = financialAccount.AccountNumber,
                CreatedOn = financialAccount.CreatedOn,
                TimeStamp = financialAccount.TimeStamp,

                Deleted = financialAccount.Deleted,
                CreatedBy = financialAccount.CreatedBy,
                FinancialAccountId = financialAccount.FinancialAccountId,
                DeletedOn = financialAccount.DeletedOn,


            };

            var financialAccountTransactionId = this._dataService.SaveFinancialAccount(financialAccountDTO, userId);

            return financialAccountTransactionId;

        }


      

        #region Mapping Methods

        private IEnumerable<FinancialAccount> MapEFToModel(IEnumerable<EF.Models.FinancialAccount> data)
        {
            var list = new List<FinancialAccount>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps FinancialAccount EF object to FinancialAccount Model Object and
        /// returns the FinancialAccount model object.
        /// </summary>
        /// <param name="result">EF FinancialAccount object to be mapped.</param>
        /// <returns>FinancialAccount Model Object.</returns>
        public FinancialAccount MapEFToModel(EF.Models.FinancialAccount data)
        {
            if (data != null)
            {
                var financialAccount = new FinancialAccount()
                {
                    FinancialAccountId = data.FinancialAccountId,
                    AccountNumber = data.AccountNumber,
                    Name = data.Name,
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,

                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),

                };
                return financialAccount;
            }
            return null;
        }



        #endregion

    }
}
