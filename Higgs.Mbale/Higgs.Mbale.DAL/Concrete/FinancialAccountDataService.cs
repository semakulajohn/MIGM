using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;

namespace Higgs.Mbale.DAL.Concrete
{
  public  class FinancialAccountDataService : DataServiceBase,IFinancialAccountDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(FinancialAccountDataService));

        public FinancialAccountDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
        {

        }
        public IEnumerable<FinancialAccount> GetAllFinancialAccounts()
        {
            return this.UnitOfWork.Get<FinancialAccount>().AsQueryable().Where(e => e.Deleted == false);
        }

        public FinancialAccount GetFinancialAccount(long financialAccountId)
        {
            return this.UnitOfWork.Get<FinancialAccount>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.FinancialAccountId == financialAccountId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new FinancialAccount or updates an already existing FinancialAccount.
        /// </summary>
        /// <param name="FinancialAccount">FinancialAccount to be saved or updated.</param>
        /// <param name="FinancialAccountId">FinancialAccountId of the FinancialAccount creating or updating</param>
        /// <returns>FinancialAccountId</returns>
        public long SaveFinancialAccount(FinancialAccountDTO financialAccountDTO, string userId)
        {
            long financialAccountId = 0;

            if (financialAccountDTO.FinancialAccountId == 0)
            {

                var financialAccount = new FinancialAccount()
                {
                    Name = financialAccountDTO.Name,
                    AccountNumber = financialAccountDTO.AccountNumber,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,

                };

                this.UnitOfWork.Get<FinancialAccount>().AddNew(financialAccount);
                this.UnitOfWork.SaveChanges();
                financialAccountId = financialAccount.FinancialAccountId;
                return financialAccountId;
            }

            else
            {
                var result = this.UnitOfWork.Get<FinancialAccount>().AsQueryable()
                    .FirstOrDefault(e => e.FinancialAccountId == financialAccountDTO.FinancialAccountId);
                if (result != null)
                {
                    result.Name = financialAccountDTO.Name;
                    result.UpdatedBy = userId;
                    result.AccountNumber = financialAccountDTO.AccountNumber;

                    result.TimeStamp = DateTime.Now;
                    result.Deleted = financialAccountDTO.Deleted;

                    result.DeletedBy = financialAccountDTO.DeletedBy;
                    result.DeletedOn = financialAccountDTO.DeletedOn;

                    this.UnitOfWork.Get<FinancialAccount>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return financialAccountDTO.FinancialAccountId;
            }

        }

    }
}
