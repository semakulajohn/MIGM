using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Web.Controllers
{
    public class DocumentApiController : ApiController
    {
         private IDocumentService _documentService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(DocumentApiController));
            private string userId = string.Empty;

            public DocumentApiController()
            {
            }

            public DocumentApiController(IDocumentService documentService,IUserService userService)
            {
                this._documentService = documentService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
            }

            [HttpGet]
            [ActionName("GetDocument")]
            public Document GetDocument(long documentId)
            {
                return _documentService.GetDocument(documentId);
            }

            [HttpGet]
            [ActionName("GetAllDocuments")]
            public IEnumerable<Document> GetAllDocuments()
            {
                return _documentService.GetAllDocuments();
            }

            [HttpGet]
            [ActionName("GetAllBranchDocuments")]
            public IEnumerable<Document> GetAllBranchDocuments(long branchId)
            {
                return _documentService.GetAllDocumentsForAParticularBranch(branchId);
            }
            [HttpGet]
            [ActionName("GetAllDocumentsForAParticularCategory")]
            public IEnumerable<Document> GetAllDocumentsForAParticularCategory(long documentCategoryId)
            {
                return _documentService.GetAllDocumentsForAParticularCategory(documentCategoryId);
            }

           
    }
}
