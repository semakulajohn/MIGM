using System;
using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using log4net;
using System.Configuration;

namespace Higgs.Mbale.BAL.Concrete
{
    public class CashTransferService : ICashTransferService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(CashTransferService));
         private long transactionSubTypeId = Convert.ToInt64(ConfigurationManager.AppSettings["CashTransferId"]);
     
        private ICashTransferDataService _dataService;
        private IUserService _userService;
        private IBranchService _branchService;
        private ICashService _cashService;
       


        public CashTransferService(ICashTransferDataService dataService, IUserService userService, IBranchService branchService,
            ICashService cashService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._branchService = branchService;
            this._cashService = cashService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CashTransferId"></param>
        /// <returns></returns>
        public CashTransfer GetCashTransfer(long cashTransferId)
        {
            var result = this._dataService.GetCashTransfer(cashTransferId);
            return MapEFToModel(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CashTransfer> GetAllCashTransfers()
        {
            var results = this._dataService.GetAllCashTransfers();
            return MapEFToModel(results);
        }

        public IEnumerable<CashTransfer> GetAllCashTransfersForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllCashTransfersForParticularBranch(branchId);
            return MapEFToModel(results);
        }

       public IEnumerable<CashTransfer> GetLatestTenCashTransfersForParticularBranch(long branchId)
        {
            var results = this._dataService.GetLatestTenCashTransfersForParticularBranch(branchId);
            return MapEFToModel(results);
        }

        public IEnumerable<CashTransfer> GetAllUnApprovedCashTransfers()
        {
            var results = this._dataService.GetAllUnApprovedCashTransfers();
            return MapEFToModel(results);
        }
        public long RejectCashTransfer(CashTransfer cashTransfer, string userId)
        {
           var toBranch = _branchService.GetBranch(cashTransfer.ToReceiverBranchId);
            
            var cash = new Cash()
            {

                Amount = cashTransfer.Amount,

                Notes = "Rejected cash from " + toBranch.Name,
                Action = "+",
                BranchId = cashTransfer.FromBranchId,
                TransactionSubTypeId = transactionSubTypeId,
                SectorId = cashTransfer.SectorId,
                CreatedBy = userId,

            };
            var cashId = _cashService.SaveCash(cash, userId);
            if (cashId > 0)
            {
                var cashTransferDTO = new CashTransferDTO()
                {

                    CashTransferId = cashTransfer.CashTransferId,
                    FromBranchId = cashTransfer.FromBranchId,
                    ToReceiverBranchId = cashTransfer.ToReceiverBranchId,
                    AmountInWords = cashTransfer.AmountInWords,
                    Amount = cashTransfer.Amount,
                    SectorId = cashTransfer.SectorId,
                    Response = cashTransfer.Response,
                    Accept = cashTransfer.Accept,
                    Notes = cashTransfer.Notes,
                    Reject = true,
                    Deleted = cashTransfer.Deleted,
                    CreatedBy = cashTransfer.CreatedBy,
                    CreatedOn = cashTransfer.CreatedOn,

                };
                var cashTransferId = _dataService.SaveCashTransfer(cashTransferDTO, userId);
                return cashTransferId;
            }
            else
            {
                return cashId;
            }
           
        }


        public long AcceptCashTransfer(CashTransfer cashTransfer, string userId)
        {
           
            if(cashTransfer.CreatedBy != userId)
            {
                var fromBranch = _branchService.GetBranch(cashTransfer.FromBranchId);
                var cash = new Cash()
                {

                    Amount = cashTransfer.Amount,

                    Notes = "Accepted cash from " + fromBranch.Name + " for " + cashTransfer.Notes,
                    Action = "+",
                    BranchId = cashTransfer.ToReceiverBranchId,
                    TransactionSubTypeId = transactionSubTypeId,
                    SectorId = cashTransfer.SectorId,
                    CreatedBy = userId,

                };
                var cashId = _cashService.SaveCash(cash, userId);
                if (cashId > 0)
                {
                    var cashTransferDTO = new CashTransferDTO()
                    {

                        CashTransferId = cashTransfer.CashTransferId,
                        FromBranchId = cashTransfer.FromBranchId,
                        ToReceiverBranchId = cashTransfer.ToReceiverBranchId,
                        AmountInWords = cashTransfer.AmountInWords,
                        Amount = cashTransfer.Amount,
                        SectorId = cashTransfer.SectorId,
                        Response = cashTransfer.Response,
                        Accept = true,
                        Notes = cashTransfer.Notes,
                        Reject = cashTransfer.Reject,
                        Deleted = cashTransfer.Deleted,
                        CreatedBy = cashTransfer.CreatedBy,
                        CreatedOn = cashTransfer.CreatedOn,

                    };
                    var cashTransferId = _dataService.SaveCashTransfer(cashTransferDTO, userId);
                    return cashTransferId;
                }
                else
                {
                    return cashId;
                }

            }
            else
            {
                var  cashTransferId = -44;
                return cashTransferId;
            }



        }

        public long SaveCashTransfer(CashTransfer cashTransfer, string userId)
        {
            var fromBranch = _branchService.GetBranch(cashTransfer.FromBranchId);
            var toBranch = _branchService.GetBranch(cashTransfer.ToReceiverBranchId);
            if (cashTransfer.FromBranchId == cashTransfer.ToReceiverBranchId)
            {
               long  cashId = -2;

                return cashId;


            }
            else
            {
                long cashTransferId = 0;
                var cash = new Cash()
                {

                    Amount = cashTransfer.Amount,

                    Notes = "Transfering cash from " + fromBranch.Name + " to" + " " + toBranch.Name,
                    Action = "-",
                    BranchId = cashTransfer.FromBranchId,
                    TransactionSubTypeId = transactionSubTypeId,
                    SectorId = cashTransfer.SectorId,


                };
                var cashId = _cashService.SaveCash(cash, userId);
                if (cashId == -1)
                {
                    return cashId;
                }

                else
                {
                    var cashTransferObject = new CashTransferDTO()
                    {

                        CashTransferId = cashTransfer.CashTransferId,
                        FromBranchId = cashTransfer.FromBranchId,
                        ToReceiverBranchId = cashTransfer.ToReceiverBranchId,
                        AmountInWords = cashTransfer.AmountInWords,
                        Amount = cashTransfer.Amount,
                        SectorId = cashTransfer.SectorId,
                        Notes = cashTransfer.Notes,
                        Response = cashTransfer.Response,
                        Accept = cashTransfer.Accept,
                        Reject = cashTransfer.Reject,
                        Deleted = cashTransfer.Deleted,
                        CreatedBy = cashTransfer.CreatedBy,
                        CreatedOn = cashTransfer.CreatedOn,

                    };
                    cashTransferId = _dataService.SaveCashTransfer(cashTransferObject, userId);

                    return cashTransferId;
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CashTransferId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long cashTransferId, string userId)
        {
            _dataService.MarkAsDeleted(cashTransferId, userId);
        }

      
        #region Mapping Methods

        public IEnumerable<CashTransfer> MapEFToModel(IEnumerable<EF.Models.CashTransfer> data)
        {
            var list = new List<CashTransfer>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps CashTransfer EF object to CashTransfer Model Object and
        /// returns the CashTransfer model object.
        /// </summary>
        /// <param name="result">EF CashTransfer object to be mapped.</param>
        /// <returns>CashTransfer Model Object.</returns>
        public CashTransfer MapEFToModel(EF.Models.CashTransfer data)
        {
            if (data != null)
            {
                var cashTransfer = new CashTransfer()
                {

                    ReceiverBranch = data.Branch1 != null ? data.Branch1.Name : "",
                    FromBranch = data.Branch != null?data.Branch.Name:"",
                    Amount = data.Amount,
                    AmountInWords = data.AmountInWords,
                    CashTransferId = data.CashTransferId,
                    FromBranchId = data.FromBranchId,
                    Response = data.Response,
                    ToReceiverBranchId = data.ToReceiverBranchId,
                    Accept = data.Accept,
                    Reject = data.Reject,
                    Notes = data.Notes,
                    SectorId = data.SectorId,
                    Department = data.Sector != null? data.Sector.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = data.CreatedBy,
                    CreatedByName = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser2)
                };

                
          

                return cashTransfer;
            }
            return null;
        }



   
       #endregion
    }
}
