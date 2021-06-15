using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IDocumentService
    {
        IEnumerable<Document> GetAllDocuments();
        Document GetDocument(long documentId);
        long SaveDocument(Document document, string userId);
        void MarkAsDeleted(long documentId, string userId);
        IEnumerable<Document> GetAllDocumentsForAParticularCategory(long documentCategoryId);
        IEnumerable<Document> GetAllDocumentsForAParticularBranch(long branchId);
        Document GetDocumentForAParticularItem(long itemId);
        Document GetDocumentForAParticularItemAndCategory(long itemId, long categoryId);
        IEnumerable<DocumentCategory> GetAllDocumentCategories();
        long Cancelled(Document document, string userId);
    }
}
