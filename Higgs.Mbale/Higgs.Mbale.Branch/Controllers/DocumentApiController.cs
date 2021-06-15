using System;
using System.Collections.Generic;
using System.Web.Http;
using Higgs.Mbale.BAL.Interface;
using log4net;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.Branch.Controllers
{
    public class DocumentApiController : ApiController
    {
         private IDocumentService _documentService;
            private IUserService _userService;
            ILog logger = log4net.LogManager.GetLogger(typeof(DocumentApiController));
            private string userId = string.Empty;
            long branchId = 0;

            public DocumentApiController()
            {
            }

            public DocumentApiController(IDocumentService documentService,IUserService userService)
            {
                this._documentService = documentService;
                this._userService = userService;
                userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                branchId = _userService.GetLoggedUserBranchId(userId);
            }

            [HttpGet]
            [ActionName("GetDocument")]
            public Document GetDocument(long documentId)
            {
                return _documentService.GetDocument(documentId);
            }

          

            [HttpGet]
            [ActionName("GetAllBranchDocuments")]
            public IEnumerable<Document> GetAllBranchDocuments()
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
