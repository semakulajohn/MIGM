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
public    class MaizeBrandStoreService : IMaizeBrandStoreService
    {
      ILog logger = log4net.LogManager.GetLogger(typeof(MaizeBrandStoreService));
        private IMaizeBrandStoreDataService _dataService;
        private IUserService _userService;
        private ITransactionSubTypeService _transactionSubTypeService;
        private IDocumentService _documentService;
        private ITransactionDataService _transactionDataService;



        public MaizeBrandStoreService(IMaizeBrandStoreDataService dataService, IUserService userService, IDocumentService documentService,
            ITransactionSubTypeService transactionSubTypeService,
            ITransactionDataService transactionDataService)
            
        {
            this._dataService = dataService;
            this._userService = userService;
            this._transactionSubTypeService = transactionSubTypeService;
             this._transactionDataService = transactionDataService;
             this._documentService = documentService;
            
        }


        public MaizeBrandStore GetMaizeBrandStore(long maizeBrandStoreId)
        {
            var result = this._dataService.GetMaizeBrandStore(maizeBrandStoreId);
            return MapEFToModel(result);
        }


        public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranch(long branchId)
        {

            var results = this._dataService.GetAllMaizeBrandStoreForAParticularBranch(branchId);
                return MapEFToModel(results);
            
            
            
        }

        public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBranchToDeliver(long branchId)
        {
            List<MaizeBrandStore> batchesToDeliverFrom = new List<MaizeBrandStore>();
            var results = GetAllMaizeBrandStoreForAParticularBranch(branchId);
            foreach (var result in results)
            {
                if (result.Quantity > 0)
                {
                    batchesToDeliverFrom.Add(result);
                }
            }

            return batchesToDeliverFrom;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStore()
        {
            var results = this._dataService.GetAllMaizeBrandStore();
            return MapEFToModel(results);
        }

        public double GetBalanceForLastMaizeBrandStore(long branchId)
        {
            double balance = 0;

            var result = this._dataService.GetLatestMaizeBrandStoreForAParticularBranch(branchId);
                if (result != null)
                {
                    balance = result.Balance;
                }
               
                return balance;
            
           
        }

    public MaizeBrandStore GetLatestMaizeBrandStoreForAParticularBranch(long branchId)
    {
        var result = this._dataService.GetLatestMaizeBrandStoreForAParticularBranch(branchId);
        return MapEFToModel(result);
    }
        public double GetBalanceForMaizeBrandStoreForABranch(long branchId)
        {
            double balance = 0;

            var results = this._dataService.GetAllMaizeBrandStoreForAParticularBranch(branchId);
            foreach (var result in results)
            {
                balance = Convert.ToDouble(result.Quantity) + balance;
            }
            

            return balance;


        }

        public long SaveMaizeBrandStore(MaizeBrandStore maizeBrandStore, string userId)
        {
            long  maizeBrandStoreId = 0;
            double startAmount =0;
            double OldBalance = 0;
            double NewBalance = 0;

                OldBalance = GetBalanceForLastMaizeBrandStore(Convert.ToInt64(maizeBrandStore.BranchId));
               startAmount = OldBalance;


               if (maizeBrandStore.Action == "-")
                {
                    if (OldBalance < maizeBrandStore.Quantity)
                    {
                        maizeBrandStoreId = -1;
                        return maizeBrandStoreId;
                    }
                    NewBalance = OldBalance - Convert.ToDouble(maizeBrandStore.Quantity);
                }
                else
                {
                    NewBalance = OldBalance + Convert.ToDouble(maizeBrandStore.Quantity);
                }

                var maizeBrandStoreDTO = new DTO.MaizeBrandStoreDTO()
                {
                   
                    Quantity = maizeBrandStore.Quantity,
                    StartQuantity = startAmount,
                    Balance = NewBalance,
                    BatchId = maizeBrandStore.BatchId,
                    MaizeBrandStoreId = maizeBrandStore.MaizeBrandStoreId,
                    Action = maizeBrandStore.Action,
                    BranchId = maizeBrandStore.BranchId,
                    StoreId = maizeBrandStore.StoreId,
                    
                    Deleted = maizeBrandStore.Deleted,
                    CreatedBy = maizeBrandStore.CreatedBy,
                    CreatedOn = maizeBrandStore.CreatedOn

                };

                 maizeBrandStoreId = this._dataService.SaveMaizeBrandStore(maizeBrandStoreDTO, userId);
         
              
                
            return maizeBrandStoreId;
                 }

      
        public void MarkAsDeleted(long maizeBrandStoreId, string userId)
        {
            _dataService.MarkAsDeleted(maizeBrandStoreId, userId);
        }

        public void UpdateMaizeBrandBatchBalance(long batchId, double quantity, string userId)
        {

            _dataService.UpdateMaizeBrandBatchBalance(batchId, quantity, userId);
        }

        public IEnumerable<MaizeBrandStore> GetAllMaizeBrandStoreForAParticularBatch(long batchId)
        {
            var results = this._dataService.GetAllMaizeBrandStoreForAParticularBatch(batchId);
            return MapEFToModel(results);
        }

        public long UpdateBrandStore(long branchId,string action,string userId,double quantity)
        {
            long maizeBrandStoreId = 0;
            double balance = 0;
            var maizeBrandStore = new MaizeBrandStore();
            
           maizeBrandStore = GetLatestMaizeBrandStoreForAParticularBranch(branchId);
            if(action == "-")
            {
                balance = Convert.ToDouble(maizeBrandStore.Quantity) - quantity;
            }
            else
            {
                balance = Convert.ToDouble(maizeBrandStore.Quantity) + quantity;
            }
           
           var maizeBrandStoreDTO = new DTO.MaizeBrandStoreDTO()
           {

               Quantity = balance,
               StartQuantity = maizeBrandStore.StartQuantity,
               Balance = maizeBrandStore.Balance,
               BatchId = maizeBrandStore.BatchId,
               MaizeBrandStoreId = maizeBrandStore.MaizeBrandStoreId,
               Action = maizeBrandStore.Action,
               BranchId = maizeBrandStore.BranchId,
               StoreId = maizeBrandStore.StoreId,

               Deleted = maizeBrandStore.Deleted,
               CreatedBy = maizeBrandStore.CreatedBy,
               CreatedOn = maizeBrandStore.CreatedOn

           };

           maizeBrandStoreId = this._dataService.SaveMaizeBrandStore(maizeBrandStoreDTO, userId);
           return maizeBrandStoreId;
        }

        #region Mapping Methods

        public IEnumerable<MaizeBrandStore> MapEFToModel(IEnumerable<EF.Models.MaizeBrandStore> data)
        {
            var list = new List<MaizeBrandStore>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public MaizeBrandStore MapEFToModel(EF.Models.MaizeBrandStore data)
        {
            
            if (data != null)
            {

                var maizeBrandStore = new MaizeBrandStore()
                {

                    Action = data.Action,
                    StartQuantity = data.StartQuantity,
                    Balance = data.Balance,
                    StoreId = data.StoreId,
                    StoreName = data.Store != null? data.Store.Name:"",
                    MaizeBrandStoreId = data.MaizeBrandStoreId,
                    BranchId = data.BranchId,
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                   Quantity = data.Quantity,
                   BatchId = data.BatchId,
                   BatchNumber = data.Batch != null ? data.Batch.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),

                };
                return maizeBrandStore;
            }
            return null;
        }

     
       #endregion

    }
}
