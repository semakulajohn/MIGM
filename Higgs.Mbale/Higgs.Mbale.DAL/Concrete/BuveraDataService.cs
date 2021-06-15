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
  public  class BuveraDataService : DataServiceBase,IBuveraDataService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(BuveraDataService));

        public BuveraDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        public IEnumerable<Buvera> GetAllBuveras()
        {
            return this.UnitOfWork.Get<Buvera>().AsQueryable()
                .Where(e => e.Deleted == false);
        }

        public IEnumerable<Buvera> GetAllBuverasForAParticularStore(long storeId)
        {
            return this.UnitOfWork.Get<Buvera>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId);
        }

        public IEnumerable<Buvera> GetAllDamagedBuverasForAparticularStore(long storeId,long buveraCateogryId)
        {
            return this.UnitOfWork.Get<Buvera>().AsQueryable().Where(e => e.Deleted == false && e.StoreId == storeId && e.BuveraCategoryId == buveraCateogryId);
        }

        public IEnumerable<BuveraCategory> GetAllBuveraCategories()
        {
            return this.UnitOfWork.Get<BuveraCategory>().AsQueryable().Where(e => e.Deleted == false);
        }

        public Buvera GetBuvera(long buveraId)
        {
            return this.UnitOfWork.Get<Buvera>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.BuveraId == buveraId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new Buvera or updates an already existing Buvera.
        /// </summary>
        /// <param name="BuveraDTO">Buvera to be saved or updated.</param>
        /// <param name="userId">userId of the Buvera creating or updating</param>
        /// <returns>BuveraId</returns>
        public long SaveBuvera(BuveraDTO buveraDTO, string userId)
        {
            long buveraId = 0;

            if (buveraDTO.BuveraId == 0)
            {
                var buvera = new Buvera()
                {
                    TotalCost = buveraDTO.TotalCost,
                    StoreId = buveraDTO.StoreId,
                    FromSupplier = buveraDTO.FromSupplier,
                    ToReceiver = buveraDTO.ToReceiver,
                    BranchId = buveraDTO.BranchId,                   
                    TotalQuantity = buveraDTO.TotalQuantity,  
                    InvoiceNumber = buveraDTO.InvoiceNumber,
                    BuveraCategoryId = buveraDTO.BuveraCategoryId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                    ProductId = buveraDTO.ProductId,
                };

                this.UnitOfWork.Get<Buvera>().AddNew(buvera);
                this.UnitOfWork.SaveChanges();
                buveraId = buvera.BuveraId;
                

                
                return buveraId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Buvera>().AsQueryable()
                    .FirstOrDefault(e => e.BuveraId == buveraDTO.BuveraId);
                if (result != null)
                {
                    result.BuveraId = buveraDTO.BuveraId;
                    result.TotalCost = buveraDTO.TotalCost;
                    result.FromSupplier = buveraDTO.FromSupplier;
                    result.ToReceiver = buveraDTO.ToReceiver;
                    result.InvoiceNumber = buveraDTO.InvoiceNumber;
                    result.TotalQuantity = buveraDTO.TotalQuantity;               
                    result.BranchId = buveraDTO.BranchId;
                    result.ProductId = buveraDTO.ProductId;
                    result.StoreId = buveraDTO.StoreId;
                    result.BuveraCategoryId = buveraDTO.BuveraCategoryId;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<Buvera>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return buveraDTO.BuveraId;
            }
        }

        public void MarkAsDeleted(long buveraId, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
            }

        }


        public IEnumerable<StoreBuveraGradeSize> GetStoreBuveraStock(long storeId)
        {
            return this.UnitOfWork.Get<StoreBuveraGradeSize>().AsQueryable().Where(e => e.StoreId == storeId);

        }

        public StoreBuveraGradeSize GetStoreBuveraGradeSize(long gradeId, long sizeId, long storeId)
        {
            
            return  this.UnitOfWork.Get<StoreBuveraGradeSize>().AsQueryable()
           .FirstOrDefault(e => e.StoreId == storeId && e.GradeId == gradeId && e.SizeId == sizeId);
           
        }
      
        public void SaveStoreBuveraGradeSize(StoreBuveraGradeSizeDTO storeBuveraGradeSizeDTO, bool inOrOut)
        {
            double sizeQuantity = 0;
            var result = this.UnitOfWork.Get<StoreBuveraGradeSize>().AsQueryable()
           .FirstOrDefault(e => e.StoreId == storeBuveraGradeSizeDTO.StoreId && e.GradeId == storeBuveraGradeSizeDTO.GradeId && e.SizeId == storeBuveraGradeSizeDTO.SizeId);


            if (result == null)
            {
                var storeBuveraGradeSize = new StoreBuveraGradeSize()
                {

                    GradeId = storeBuveraGradeSizeDTO.GradeId,
                    SizeId = storeBuveraGradeSizeDTO.SizeId,
                    StoreId = storeBuveraGradeSizeDTO.StoreId,
                    Quantity = storeBuveraGradeSizeDTO.Quantity,
                    TimeStamp = DateTime.Now
                };
                this.UnitOfWork.Get<StoreBuveraGradeSize>().AddNew(storeBuveraGradeSize);
                this.UnitOfWork.SaveChanges();

            

            }

            else
            {
                if (inOrOut)
                {
                    sizeQuantity = Convert.ToDouble(result.Quantity) + Convert.ToDouble(storeBuveraGradeSizeDTO.Quantity);

                    result.StoreId = storeBuveraGradeSizeDTO.StoreId;
                    result.SizeId = storeBuveraGradeSizeDTO.SizeId;
                    result.GradeId = storeBuveraGradeSizeDTO.GradeId;
                    result.Quantity = sizeQuantity;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<StoreBuveraGradeSize>().Update(result);
                    this.UnitOfWork.SaveChanges();
                    

                }
                else
                {
                   
                    sizeQuantity = Convert.ToDouble(result.Quantity)- Convert.ToDouble(storeBuveraGradeSizeDTO.Quantity);

                    result.StoreId = storeBuveraGradeSizeDTO.StoreId;
                    result.SizeId = storeBuveraGradeSizeDTO.SizeId;
                    result.GradeId = storeBuveraGradeSizeDTO.GradeId;
                    result.Quantity = sizeQuantity;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<StoreBuveraGradeSize>().Update(result);
                    this.UnitOfWork.SaveChanges();
                   
                }


            }
        }

     
        public void SaveBuveraGradeSize(BuveraGradeSizeDTO buveraGradeSizeDTO)
        {
            var buveraGradeSize = new BuveraGradeSize()
            {
                BuveraId = buveraGradeSizeDTO.BuveraId,
                GradeId = buveraGradeSizeDTO.GradeId,
                StoreId = buveraGradeSizeDTO.StoreId,
                SizeId = buveraGradeSizeDTO.SizeId,
                Quantity = buveraGradeSizeDTO.Quantity,
                Rate = buveraGradeSizeDTO.Rate,
                Amount = buveraGradeSizeDTO.Amount,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<BuveraGradeSize>().AddNew(buveraGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeBuveraGradeSize(long buveraId)
        {
            this.UnitOfWork.Get<BuveraGradeSize>().AsQueryable()
                .Where(m => m.BuveraId == buveraId)
                .Delete();
        }

      
    }
}
