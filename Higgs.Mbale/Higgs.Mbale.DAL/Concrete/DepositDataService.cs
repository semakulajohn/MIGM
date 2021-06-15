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
  public  class DepositDataService : DataServiceBase, IDepositDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(AccountTransactionActivityDataService));

        public DepositDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
        {

        }



        public IEnumerable<Deposit> GetAllDeposits()
        {
            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false);
        }

        public Deposit GetDeposit(long depositId)
        {
            return this.UnitOfWork.Get<Deposit>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.DepositId == depositId &&
                    c.Deleted == false
                );
        }

        public IEnumerable<Deposit> GetAllDepositsForAParticularAspNetUser(string accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.AspNetUserId == accountId);
        }
        public IEnumerable<Deposit> GetAllUnApprovedDepositsForAParticularAspNetUser(string accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.AspNetUserId == accountId && e.Approved == null);
        }

        public IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForAParticularAspNetUser(string accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.AspNetUserId == accountId && e.Approved == true ).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForAParticularAspNetUser(string accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.AspNetUserId == accountId && e.Approved == false).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Deposit> GetAllDepositsForAParticularCasualWorker(long accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.CasualWorkerId == accountId);
        }
        public IEnumerable<Deposit> GetAllLatestTwentyApprovedDepositsForAParticularCasualWorker(long accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.CasualWorkerId == accountId && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Deposit> GetAllUnpprovedDepositsForAParticularCasualWorker(long accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.CasualWorkerId == accountId && e.Approved == null);
        }

        public IEnumerable<Deposit> GetAllLatestTwentyRejectedDepositsForAParticularCasualWorker(long accountId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.CasualWorkerId == accountId && e.Approved == false).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Deposit> GetLatestTwentyRejectedDeposits()
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.Approved == false).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Deposit> GetLatestTwentyApprovedDeposits()
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.Approved == true).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public IEnumerable<Deposit> GetLatestTwentyUnApprovedDeposits()
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.Approved == null).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Deposit> GetLatestTwentyRejectedDepositsForABranch(long branchId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.Approved == false && e.BranchId == branchId).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Deposit> GetLatestTwentyApprovedDepositsForABranch(long branchId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.Approved == true && e.BranchId == branchId).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public IEnumerable<Deposit> GetLatestTwentyUnApprovedDepositsForABranch(long branchId)
        {

            return this.UnitOfWork.Get<Deposit>().AsQueryable().Where(e => e.Deleted == false && e.Approved == null && e.BranchId == branchId).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public long SaveDeposit(DepositDTO depositDTO, string userId)
        {
            long depositId = 0;

            if (depositDTO.DepositId == 0)
            {

                var deposit = new Deposit()
                {
                    AspNetUserId = depositDTO.AspNetUserId,
                    CasualWorkerId = depositDTO.CasualWorkerId,
                    Amount = depositDTO.Amount,
                    StartAmount = depositDTO.StartAmount,
                    Notes = depositDTO.Notes,
                    Action = depositDTO.Action,
                    Balance = depositDTO.Balance,
                    SupplyId = depositDTO.SupplyId,
                    TransactionSubTypeId = depositDTO.TransactionSubTypeId,
                    BranchId = depositDTO.BranchId,
                    SectorId = depositDTO.SectorId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                    WeightNote = depositDTO.WeightNote,
                    Bags = depositDTO.Bags,
                    Price = depositDTO.Price,
                    Quantity = depositDTO.Quantity,
                    Approved = null,
                };

                this.UnitOfWork.Get<Deposit>().AddNew(deposit);
                this.UnitOfWork.SaveChanges();
                depositId = deposit.DepositId;
                return depositId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Deposit>().AsQueryable()
                    .FirstOrDefault(e => e.DepositId == depositDTO.DepositId);
                if (result != null)
                {
                    result.Action = depositDTO.Action;
                    result.Amount = depositDTO.Amount;
                    result.AspNetUserId = depositDTO.AspNetUserId;
                    result.CasualWorkerId = depositDTO.CasualWorkerId;
                    result.StartAmount = depositDTO.StartAmount;
                    result.Balance = depositDTO.Balance;
                    result.Notes = depositDTO.Notes;
                    result.SupplyId = depositDTO.SupplyId;
                    result.TransactionSubTypeId = depositDTO.TransactionSubTypeId;
                    result.BranchId = depositDTO.BranchId;
                    result.SectorId = depositDTO.SectorId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = depositDTO.Deleted;
                    result.DeletedBy = depositDTO.DeletedBy;
                    result.DeletedOn = depositDTO.DeletedOn;
                    result.WeightNote = depositDTO.WeightNote;
                    result.Quantity = depositDTO.Quantity;
                    result.Price = depositDTO.Price;
                    result.Bags = depositDTO.Bags;
                    result.Approved = depositDTO.Approved;

                    this.UnitOfWork.Get<Deposit>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return depositDTO.DepositId;
            }
        }

        public void MarkAsDeleted(long depositId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }

       
    }
}
