using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Concrete;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;
using EntityFramework.Extensions;


namespace Higgs.Mbale.DAL.Concrete
{
    public class CashTransferDataService : DataServiceBase, ICashTransferDataService
    {
        
         ILog logger = log4net.LogManager.GetLogger(typeof(CashTransferDataService));

       public CashTransferDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        public IEnumerable<CashTransfer> GetAllCashTransfers()
        {
            return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                .Where(e => e.Deleted == false);
        }


        public IEnumerable<CashTransfer> GetAllCashTransfersForParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<CashTransfer>().AsQueryable().Where(e => e.Deleted == false && e.FromBranchId == branchId || e.ToReceiverBranchId == branchId);
        }

        public IEnumerable<CashTransfer> GetAllUnApprovedCashTransfers()
        {
            return this.UnitOfWork.Get<CashTransfer>().AsQueryable().Where(e => e.Deleted == false && e.Accept == false && e.Reject == false);
        }

        public IEnumerable<CashTransfer> GetLatestTenCashTransfersForParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<CashTransfer>().AsQueryable().Where(e => e.Deleted == false && e.FromBranchId == branchId || e.ToReceiverBranchId == branchId)
                .OrderByDescending(e => e.CreatedOn).Take(10);
        }


        public CashTransfer GetCashTransfer(long CashTransferId)
        {
            return this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.CashTransferId == CashTransferId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new CashTransfer or updates an already existing CashTransfer.
        /// </summary>
        /// <param name="CashTransferDTO">CashTransfer to be saved or updated.</param>
        /// <param name="userId">userId of the CashTransfer creating or updating</param>
        /// <returns>CashTransferId</returns>
        public long SaveCashTransfer(CashTransferDTO cashTransferDTO, string userId)
        {
            long cashTransferId = 0;

            if (cashTransferDTO.CashTransferId == 0)
            {
                var cashTransfer = new CashTransfer()
                {
                                       
                    FromBranchId = cashTransferDTO.FromBranchId,
                   ToReceiverBranchId = cashTransferDTO.ToReceiverBranchId,
                    Amount= cashTransferDTO.Amount,
                    AmountInWords = cashTransferDTO.AmountInWords,
                    Response = cashTransferDTO.Response,
                    Accept = cashTransferDTO.Accept,
                    SectorId = cashTransferDTO.SectorId,
                    Reject = cashTransferDTO.Reject,
                    Notes = cashTransferDTO.Notes,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                };

                this.UnitOfWork.Get<CashTransfer>().AddNew(cashTransfer);
                this.UnitOfWork.SaveChanges();
                cashTransferId = cashTransfer.CashTransferId;



                return cashTransferId;
            }

            else
            {
                var result = this.UnitOfWork.Get<CashTransfer>().AsQueryable()
                    .FirstOrDefault(e => e.CashTransferId == cashTransferDTO.CashTransferId);
                if (result != null)
                {
                    result.CashTransferId = cashTransferDTO.CashTransferId;

                    result.FromBranchId = cashTransferDTO.FromBranchId;
                    result.ToReceiverBranchId = cashTransferDTO.ToReceiverBranchId;
                    result.Amount = cashTransferDTO.Amount;
                    result.Response = cashTransferDTO.Response;
                    result.AmountInWords = cashTransferDTO.AmountInWords;
                    result.Accept = cashTransferDTO.Accept;
                    result.SectorId = cashTransferDTO.SectorId;
                    result.Reject = cashTransferDTO.Reject;
                    result.Notes = cashTransferDTO.Notes;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<CashTransfer>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return cashTransferDTO.CashTransferId;
            }
        }

        public void MarkAsDeleted(long CashTransferId, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }
     
      
    }
}
