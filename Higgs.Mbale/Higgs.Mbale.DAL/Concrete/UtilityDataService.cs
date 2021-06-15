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

namespace Higgs.Mbale.DAL.Concrete
{
 public   class UtilityDataService : DataServiceBase, IUtilityDataService
    {
           ILog logger = log4net.LogManager.GetLogger(typeof(UtilityDataService));

       public UtilityDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

      
        public IEnumerable<Utility> GetAllUtilities()
        {
            return this.UnitOfWork.Get<Utility>().AsQueryable().Where(e => e.Deleted == false); 
        }


        public IEnumerable<Utility> GetAllUtilitiesForAParticularBatch(long batchId)
        {
            return this.UnitOfWork.Get<Utility>().AsQueryable().Where(e => e.Deleted == false && e.BatchId == batchId);
        }

        public Utility GetUtility(long utilityId)
        {
            return this.UnitOfWork.Get<Utility>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.UtilityId == utilityId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new Utility or updates an already existing Utility.
        /// </summary>
        /// <param name="Utility">Utility to be saved or updated.</param>
        /// <param name="UtilityId">UtilityId of the Utility creating or updating</param>
        /// <returns>UtilityId</returns>
        public long SaveUtility(UtilityDTO utilityDTO, string userId)
        {
            long utilityId = 0;
            
            if (utilityDTO.UtilityId == 0)
            {
           
                var utility = new Utility()
                {
                     Amount= utilityDTO.Amount,
                    Description = utilityDTO.Description,
                    BranchId = utilityDTO.BranchId,
                    SectorId = utilityDTO.SectorId,
                    BatchId = utilityDTO.BatchId,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false,
                     

                };

                this.UnitOfWork.Get<Utility>().AddNew(utility);
                this.UnitOfWork.SaveChanges();
                utilityId = utility.UtilityId;
                return utilityId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Utility>().AsQueryable()
                    .FirstOrDefault(e => e.UtilityId == utilityDTO.UtilityId);
                if (result != null)
                {
                    result.Amount = utilityDTO.Amount;
                    result.UpdatedBy = userId;
                    result.Description = utilityDTO.Description;
                    result.BatchId = utilityDTO.BatchId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = utilityDTO.Deleted;
                    result.BranchId = utilityDTO.BranchId;
                    result.SectorId = utilityDTO.SectorId;
                    result.DeletedBy = utilityDTO.DeletedBy;
                    result.DeletedOn = utilityDTO.DeletedOn;

                    this.UnitOfWork.Get<Utility>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return utilityDTO.UtilityId;
            }
            
        }

        public void MarkAsDeleted(long utilityId, string userId)
        {


            using (var dbContext = new MbaleEntities())
            {
               dbContext.Mark_Utility_AsDeleted(utilityId, userId);
            }

        }
    }
}
