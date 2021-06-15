using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DAL.Interface;
using log4net;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;

namespace Higgs.Mbale.DAL.Concrete
{
 public  class AssetCategoryDataService : DataServiceBase,IAssetCategoryDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(AssetCategoryDataService));

        public AssetCategoryDataService(IUnitOfWork<MbaleEntities> unitOfWork)
             : base(unitOfWork)
        {

        }


        public IEnumerable<AssetCategory> GetAllAssetCategories()
        {
            return this.UnitOfWork.Get<AssetCategory>().AsQueryable().Where(e => e.Deleted == false);
        }
        public AssetCategory GetAssetCategory(long assetCategoryId)
        {
            return this.UnitOfWork.Get<AssetCategory>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.AssetCategoryId == assetCategoryId &&
                    c.Deleted == false
                );

        }
        public long SaveAssetCategory(AssetCategoryDTO assetCategoryDTO, string userId)
        {
            long assetCategoryId = 0;

            if (assetCategoryDTO.AssetCategoryId == 0)
            {

                var assetCategory = new AssetCategory()
                {

                    AssetCategoryId = assetCategoryDTO.AssetCategoryId,
                    Name = assetCategoryDTO.Name,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,

                   
                };

                this.UnitOfWork.Get<AssetCategory>().AddNew(assetCategory);
                this.UnitOfWork.SaveChanges();
                assetCategoryId = assetCategory.AssetCategoryId;
                return assetCategoryId;
            }

            else
            {
                var result = this.UnitOfWork.Get<AssetCategory>().AsQueryable()
                    .FirstOrDefault(e => e.AssetCategoryId == assetCategoryDTO.AssetCategoryId);
                if (result != null)
                {
                    result.AssetCategoryId = assetCategoryDTO.AssetCategoryId;
                    result.Name = assetCategoryDTO.Name;
                    result.UpdatedBy = userId;               
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = assetCategoryDTO.Deleted;
                    result.DeletedBy = assetCategoryDTO.DeletedBy;
                    result.DeletedOn = assetCategoryDTO.DeletedOn;
               

                    this.UnitOfWork.Get<AssetCategory>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return assetCategoryDTO.AssetCategoryId;
            }
        }

        public void MarkAsDeleted(long assetCategoryId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
                dbContext.Mark_AssetCategory_AsDeleted(assetCategoryId, userId);
            }


        }

    }
}
