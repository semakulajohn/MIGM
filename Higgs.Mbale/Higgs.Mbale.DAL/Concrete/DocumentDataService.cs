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
 public   class DocumentDataService : DataServiceBase,IDocumentDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(DocumentDataService));

       public DocumentDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }
              
        public IEnumerable<Document> GetAllDocuments()
        {
            return this.UnitOfWork.Get<Document>().AsQueryable().Where(e => e.Deleted == false); 
        }

        public IEnumerable<Document> GetAllDocumentsForAParticularCategory(long documentCategoryId)
        {
            return this.UnitOfWork.Get<Document>().AsQueryable().Where(e => e.Deleted == false && e.DocumentCategoryId == documentCategoryId);
        }

        public IEnumerable<Document> GetAllDocumentsForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<Document>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
        }

        public IEnumerable<DocumentCategory> GetAllDocumentCategories()
        {
            return this.UnitOfWork.Get<DocumentCategory>().AsQueryable();
        }
        public Document GetDocument(long documentId)
        {
            return this.UnitOfWork.Get<Document>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.DocumentId == documentId &&
                    c.Deleted == false
                );
        }

        public Document GetDocumentForAParticularItem(long itemId)
        {
            return this.UnitOfWork.Get<Document>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.ItemId == itemId &&
                    c.Deleted == false
                );
        }

        public Document GetDocumentForAParticularItemAndCategory(long itemId,long categoryId)
        {
            return this.UnitOfWork.Get<Document>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.ItemId == itemId &&
                    c.Deleted == false && c.DocumentCategoryId == categoryId
                );
        }
        public Document GetLatestCreatedDocumentForAParticularCategory(long documentCategoryId,long branchId)
        {
            Document document = new Document();
            var documents = this.UnitOfWork.Get<Document>().AsQueryable().Where(e => e.DocumentCategoryId == documentCategoryId && e.BranchId == branchId);
            if (documents.Any())
            {
                document = documents.AsQueryable().OrderByDescending(e => e.CreatedOn).First();
                return document;
            }
            else
            {
                return document;
            }

        }
       
        public long SaveDocument(DocumentDTO documentDTO, string userId)
        {
            long documentId = 0;
            
            if (documentDTO.DocumentId == 0)
            {
           
                var document = new Document()
                {
                    
                    UserId = documentDTO.UserId,
                    DocumentCategoryId = documentDTO.DocumentCategoryId,
                    Amount = documentDTO.Amount,
                    ItemId = documentDTO.ItemId,
                    BranchId = documentDTO.BranchId,
                    Description = documentDTO.Description,
                    Quantity = documentDTO.Quantity,
                    DocumentNumber = documentDTO.DocumentNumber,
                    AmountInWords = documentDTO.AmountInWords,
                    CreatedOn = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    CreatedBy = userId,
                    Deleted = false, 
                };

                this.UnitOfWork.Get<Document>().AddNew(document);
                this.UnitOfWork.SaveChanges();
                documentId = document.DocumentId;
                return documentId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Document>().AsQueryable()
                    .FirstOrDefault(e => e.DocumentId == documentDTO.DocumentId);
                if (result != null)
                {
                    
                    result.DocumentNumber = documentDTO.DocumentNumber;
                    result.Description = documentDTO.Description;
                    result.DocumentCategoryId = documentDTO.DocumentCategoryId;
                    result.Amount = documentDTO.Amount;
                    result.Quantity = documentDTO.Quantity;
                    result.ItemId = documentDTO.ItemId;
                    result.BranchId = documentDTO.BranchId;
                    result.UserId = documentDTO.UserId;
                    result.AmountInWords = documentDTO.AmountInWords;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    result.Deleted = documentDTO.Deleted;
                    result.DeletedBy = documentDTO.DeletedBy;
                    result.DeletedOn = documentDTO.DeletedOn;

                    this.UnitOfWork.Get<Document>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return documentDTO.DocumentId;
            }            
        }

        public void MarkAsDeleted(long documentId, string userId)
        {

            //using (var dbContext = new MbaleEntities())
            //{
            //    dbContext.Mark_Activity_AsDeleted(activityId, userId);
            //}

        }


        public long Cancelled(DocumentDTO documentDTO,string userId)
        {

            var result = this.UnitOfWork.Get<Document>().AsQueryable()
                    .FirstOrDefault(e => e.DocumentId == documentDTO.DocumentId);
            if (result != null)
            {

                result.DocumentNumber = documentDTO.DocumentNumber;
                result.Description = documentDTO.Description;
                result.DocumentCategoryId = documentDTO.DocumentCategoryId;
                result.Amount = documentDTO.Amount;
                result.Quantity = documentDTO.Quantity;
                result.ItemId = documentDTO.ItemId;
                result.BranchId = documentDTO.BranchId;
                result.UserId = documentDTO.UserId;
                result.AmountInWords = documentDTO.AmountInWords;
                result.UpdatedBy = userId;
                result.TimeStamp = DateTime.Now;
                result.Deleted = true;
                result.DeletedBy = documentDTO.DeletedBy;
                result.DeletedOn = documentDTO.DeletedOn;

                this.UnitOfWork.Get<Document>().Update(result);
                this.UnitOfWork.SaveChanges();
            }
            return documentDTO.DocumentId;

        }
        public void SaveDocumentGradeSize(DocumentGradeSizeDTO documentGradeSizeDTO)
        {
            var documentGradeSize = new DocumentGradeSize()
            {
                DocumentId = documentGradeSizeDTO.DocumentId,
                GradeId = documentGradeSizeDTO.GradeId,
               
                SizeId = documentGradeSizeDTO.SizeId,
                Quantity = documentGradeSizeDTO.Quantity,
                Price = documentGradeSizeDTO.Price,
                Amount = documentGradeSizeDTO.Amount,
                TimeStamp = DateTime.Now
            };
            this.UnitOfWork.Get<DocumentGradeSize>().AddNew(documentGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeDocumentGradeSize(long documentId)
        {
            this.UnitOfWork.Get<DocumentGradeSize>().AsQueryable()
                .Where(m => m.DocumentId == documentId)
                .Delete();
        }

    }
}
