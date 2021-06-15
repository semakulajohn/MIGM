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
 public   class BatchProjectionService : IBatchProjectionService
    {
       
        ILog logger = log4net.LogManager.GetLogger(typeof(BatchProjectionService));
        private IBatchProjectionDataService _dataService;
        private IUserService _userService;
        private IBatchService _batchService;
        

        public BatchProjectionService(IBatchProjectionDataService dataService,IUserService userService,IBatchService batchService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._batchService = batchService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchProjectionId"></param>
        /// <returns></returns>
        public BatchProjection GetBatchProjection(long batchProjectionId)
        {
            var result = this._dataService.GetBatchProjection(batchProjectionId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BatchProjection> GetAllBatchProjections()
        {
            var results = this._dataService.GetAllBatchProjections();
            return MapEFToModel(results);
        }

        public IEnumerable<BatchProjection> GetAllBatchProjectionsForAParticularBatch(long batchId)
        {
            var results = this._dataService.GetAllBatchProjectionsForAParticularBatch(batchId);
            return MapEFToModel(results);
        }

        public long SaveBatchProjection(BatchProjection batchProjection, string userId)
        {
            var batchProjectionObject = CalculateProjection(batchProjection);
            var batchProjectionDTO = new DTO.BatchProjectionDTO()
            {
                BatchProjectionId = batchProjectionObject.BatchProjectionId,
                FlourPrice = batchProjectionObject.FlourPrice,
                FlourSales = batchProjectionObject.FlourSales,
                FlourOutPut = batchProjectionObject.FlourOutPut,
                BrandOutPut = batchProjectionObject.BrandOutPut,
                BatchId = batchProjectionObject.BatchId,
                BrandPrice = batchProjectionObject.BrandPrice,
                BrandSales = batchProjectionObject.BrandSales,
                BrandPercentage = batchProjectionObject.BrandPercentage,
                FlourPercentage = batchProjectionObject.FlourPercentage,
                BranchId = batchProjection.BranchId,
               
                UnitCost = batchProjectionObject.UnitCost,
                ProductionCost = batchProjectionObject.ProductionCost,
                ExpectedContribution = batchProjectionObject.ExpectedContribution,

                CreatedOn = DateTime.Now,
                TimeStamp = DateTime.Now,
                CreatedBy = userId,
                Deleted = false,


            };

           var batchProjectionId = this._dataService.SaveBatchProjection(batchProjectionDTO, userId);

           return batchProjectionId;
                      
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchProjectionId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long batchProjectionId, string userId)
        {
            _dataService.MarkAsDeleted(batchProjectionId, userId);
        }

        public BatchProjectionDTO CalculateProjection(BatchProjection batchProjection)
        {
            double productionCost = 0, brandOutPut = 0, flourOutPut = 0, brandSales = 0, flourSales = 0, totalCost = 0, totalExpectedSales = 0,
                expectedContribution = 0, maizeAmount = 0, purchasedMaize = 0;
            BatchProjectionDTO batchProjectionDTO = new BatchProjectionDTO();
            var batch = _batchService.GetBatch(batchProjection.BatchId);
            if(batch != null)
            {
                purchasedMaize = batch.Quantity;
                maizeAmount = batch.TotalSupplyAmount;
                
                flourOutPut = ((batchProjection.FlourPercentage / 100) * purchasedMaize);
                brandOutPut = ((batchProjection.BrandPercentage / 100) * purchasedMaize);
                productionCost = batchProjection.UnitCost * flourOutPut;
                flourSales = flourOutPut * batchProjection.FlourPrice;
                brandSales = brandOutPut * batchProjection.BrandPrice;
                totalCost = maizeAmount + productionCost;

                totalExpectedSales = flourSales + brandSales;
                expectedContribution = totalExpectedSales - totalCost;

                 batchProjectionDTO = new BatchProjectionDTO()
                {
                    BatchProjectionId = batchProjection.BatchProjectionId,
                    FlourPrice = batchProjection.FlourPrice,
                    FlourSales = flourSales,
                    FlourOutPut = flourOutPut,
                    BrandOutPut = brandOutPut,
                    BatchId = batchProjection.BatchId,
                    BrandPrice = batchProjection.BrandPrice,
                    BrandSales = brandSales,
                    BrandPercentage = batchProjection.BrandPercentage,
                    FlourPercentage = batchProjection.FlourPercentage,
                    BranchId = batchProjection.BranchId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,

                    UnitCost =batchProjection.UnitCost,
                    ProductionCost = productionCost,
                    ExpectedContribution = expectedContribution,
                };
            }
            return batchProjectionDTO;
        }

        
      
        #region Mapping Methods

        private IEnumerable<BatchProjection> MapEFToModel(IEnumerable<EF.Models.BatchProjection> data)
        {
            var list = new List<BatchProjection>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps BatchProjection EF object to BatchProjection Model Object and
        /// returns the BatchProjection model object.
        /// </summary>
        /// <param name="result">EF BatchProjection object to be mapped.</param>
        /// <returns>BatchProjection Model Object.</returns>
        public BatchProjection MapEFToModel(EF.Models.BatchProjection data)
        {
            if (data != null)
            {
                if (data.BatchId != 0)
                {
                    var batch = _batchService.GetBatch(data.BatchId);

                    var batchProjection = new BatchProjection()
                    {
                        BatchProjectionId = data.BatchProjectionId,
                        FlourPrice = data.FlourPrice,
                        FlourSales = data.FlourSales,
                        FlourOutPut = data.FlourOutPut,
                        BrandOutPut = data.BrandOutPut,
                        BatchId = data.BatchId,
                        BrandPrice = data.BrandPrice,
                        BrandSales = data.BrandSales,
                        BrandPercentage = data.BrandPercentage,
                        FlourPercentage = data.FlourPercentage,
                        BranchId = data.BranchId,
                        BatchNumber = batch.Name,
                        CreatedOn = data.CreatedOn,
                        TimeStamp = data.TimeStamp,
                        BranchName = data.Branch != null ? data.Branch.Name : "",
                        Supplies = batch.Supplies,
                        TotalSupplyAmount = batch.TotalSupplyAmount,
                        MaizeQuantity = batch.Quantity,
                        UnitCost = data.UnitCost,
                        ProductionCost = data.ProductionCost,
                        ExpectedContribution = data.ExpectedContribution,
                        TotalExpectedSales = (data.BrandSales + data.FlourSales),
                        TotalProductionCost = (batch.TotalSupplyAmount + data.ProductionCost),
                        Deleted = data.Deleted,
                        CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                        UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),


                    };
                    return batchProjection;
                }
                
            }
            return null;
        }



       #endregion
    }
}
