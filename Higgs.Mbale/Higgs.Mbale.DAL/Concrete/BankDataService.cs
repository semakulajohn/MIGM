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
  public  class BankDataService : DataServiceBase,IBankDataService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(BankDataService));

       public BankDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

      
        public IEnumerable<Bank> GetAllBanks()
        {
            return this.UnitOfWork.Get<Bank>().AsQueryable().Where(e => e.Deleted == false); 
        }

        public Bank GetBank(long bankId)
        {
            return this.UnitOfWork.Get<Bank>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.BankId == bankId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new Bank or updates an already existing Bank.
        /// </summary>
        /// <param name="Bank">Bank to be saved or updated.</param>
        /// <param name="BankId">BankId of the Bank creating or updating</param>
        /// <returns>BankId</returns>
        public long SaveBank(BankDTO bankDTO, string userId)
        {
            long bankId = 0;
            
            if (bankDTO.BankId == 0)
            {
           
                var bank = new Bank()
                {
                    Name = bankDTO.Name,
                    AccountNumber = bankDTO.AccountNumber,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                  
                };

                this.UnitOfWork.Get<Bank>().AddNew(bank);
                this.UnitOfWork.SaveChanges();
                bankId = bank.BankId;
                return bankId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Bank>().AsQueryable()
                    .FirstOrDefault(e => e.BankId == bankDTO.BankId);
                if (result != null)
                {
                    result.Name = bankDTO.Name;
                    result.UpdatedBy = userId;
                    result.AccountNumber = bankDTO.AccountNumber;
                    
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = bankDTO.Deleted;
                    
                    result.DeletedBy = bankDTO.DeletedBy;
                    result.DeletedOn = bankDTO.DeletedOn;

                    this.UnitOfWork.Get<Bank>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return bankDTO.BankId;
            }
           
        }

        public void MarkAsDeleted(long bankId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
               // dbContext.Mark_Estate_And_RelatedData_AsDeleted(BankId, userId);
            }

        }
    }
}
