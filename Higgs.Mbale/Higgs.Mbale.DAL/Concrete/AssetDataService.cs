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
    public class AssetDataService: DataServiceBase, IAssetDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(AssetDataService));

    public AssetDataService(IUnitOfWork<MbaleEntities> unitOfWork)
         : base(unitOfWork)
    {

    }


    public IEnumerable<Asset> GetAllAssets()
    {
        return this.UnitOfWork.Get<Asset>().AsQueryable().Where(e => e.Deleted == false);
    }
    public Asset GetAsset(long assetId)
    {
        return this.UnitOfWork.Get<Asset>().AsQueryable()
             .FirstOrDefault(c =>
                c.AssetId == assetId &&
                c.Deleted == false
            );

    }
    public long SaveAsset(AssetDTO assetDTO, string userId)
    {
        long assetId = 0;

        if (assetDTO.AssetId == 0)
        {

            var asset = new Asset()
            {

                AssetCategoryId = assetDTO.AssetCategoryId,
                Name = assetDTO.Name,
                CreatedOn = DateTime.Now,
                TimeStamp = DateTime.Now,
                CreatedBy = userId,
                Deleted = false,
                AssetId = assetDTO.AssetId,
                BranchId = assetDTO.BranchId,
                AssetCount = assetDTO.AssetCount,
                PurchaseDate = assetDTO.PurchaseDate,
                Notes  = assetDTO.Notes,
                Amount = assetDTO.Amount, 


    };

            this.UnitOfWork.Get<Asset>().AddNew(asset);
            this.UnitOfWork.SaveChanges();
            assetId = asset.AssetId;
            return assetId;
        }

        else
        {
            var result = this.UnitOfWork.Get<Asset>().AsQueryable()
                .FirstOrDefault(e => e.AssetId == assetDTO.AssetId);
            if (result != null)
            {
                result.AssetCategoryId = assetDTO.AssetCategoryId;
                result.Name = assetDTO.Name;
                result.UpdatedBy = userId;
                result.TimeStamp = DateTime.Now;
                result.Deleted = assetDTO.Deleted;
                result.DeletedBy = assetDTO.DeletedBy;
                result.DeletedOn = assetDTO.DeletedOn;
                result.AssetId = assetDTO.AssetId;
                result.BranchId = assetDTO.BranchId;
                result.AssetCount = assetDTO.AssetCount;
                result.PurchaseDate = assetDTO.PurchaseDate;
                result.Notes = assetDTO.Notes;
                result.Amount = assetDTO.Amount;

                this.UnitOfWork.Get<Asset>().Update(result);
                this.UnitOfWork.SaveChanges();
            }
            return assetDTO.AssetId;
        }
    }

    public IEnumerable<Asset> GetAllAssetsForAParticularCategory(long assetCategoryId)
    {
            return this.UnitOfWork.Get<Asset>().AsQueryable().Where(e => e.Deleted == false && e.AssetCategoryId == assetCategoryId);

    }
    public IEnumerable<Asset> GetAllAssetsForAParticularCategoryForAParticularBranch(long assetCategoryId,long branchId)
    {
        return this.UnitOfWork.Get<Asset>().AsQueryable().Where(e => e.Deleted == false && e.AssetCategoryId == assetCategoryId && e.BranchId == branchId);

    }

    public IEnumerable<Asset> GetAllAssetsForAParticularBranch(long branchId)
    {
        return this.UnitOfWork.Get<Asset>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);

    }


        public void MarkAsDeleted(long assetId, string userId)
    {


        using (var dbContext = new MbaleEntities())
        {
            dbContext.Mark_Asset_AsDeleted(assetId, userId);
        }


    }

    }
}
