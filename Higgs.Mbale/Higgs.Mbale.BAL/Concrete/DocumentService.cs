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
 public   class DocumentService : IDocumentService
    {
         ILog logger = log4net.LogManager.GetLogger(typeof(DocumentService));
        private IDocumentDataService _dataService;
        private IUserService _userService;
       // private ICashSaleService _cashSaleService;
        

        public DocumentService(IDocumentDataService dataService,IUserService userService
            //,ICashSaleService cashSaleService
            )
        {
            this._dataService = dataService;
            this._userService = userService;
            //this._cashSaleService = cashSaleService;
        }

        
        public Document GetDocument(long documentId)
        {
            var result = this._dataService.GetDocument(documentId);
            return MapEFToModel(result);
        }

        public Document GetDocumentForAParticularItem(long itemId)
        {
            var result = this._dataService.GetDocumentForAParticularItem(itemId);
            return MapEFToModel(result);
        }

        public Document GetDocumentForAParticularItemAndCategory(long itemId, long categoryId)
        {
            var result = this._dataService.GetDocumentForAParticularItemAndCategory(itemId, categoryId);
            return MapEFToModel(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Document> GetAllDocuments()
        {
            var results = this._dataService.GetAllDocuments();
            return MapEFToModel(results);
        }

        public IEnumerable<Document> GetAllDocumentsForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllDocumentsForAParticularBranch(branchId);
            return MapEFToModel(results);
        }

        public IEnumerable<Document> GetAllDocumentsForAParticularCategory(long documentCategoryId)
        {
            var results = this._dataService.GetAllDocumentsForAParticularCategory(documentCategoryId);
            return MapEFToModel(results);
        }
        public IEnumerable<DocumentCategory> GetAllDocumentCategories()
        {
            var results = this._dataService.GetAllDocumentCategories();
            return MapEFToModel(results);
        } 

     private long GetDocumentNumber(long documentCategoryId,long branchId)
     {
         long documentNumber = 0;
         var latestDocument = _dataService.GetLatestCreatedDocumentForAParticularCategory(documentCategoryId,branchId);
         if(latestDocument !=  null){
            documentNumber = latestDocument.DocumentNumber + 1;
         }
         else
         {
             documentNumber = documentNumber + 1;
             
         }
        return documentNumber;
     }
       
        public long SaveDocument(Document document, string userId)
        {
            long documentNumber = 0;
            if (document.DocumentId == 0)
            {
                documentNumber = GetDocumentNumber(document.DocumentCategoryId,document.BranchId);       

            }
            else
            {
                documentNumber = document.DocumentNumber;
            }
           
                 var documentDTO = new DTO.DocumentDTO()
                {
                    DocumentId = document.DocumentId,
                   
                    UserId = document.UserId,
                    DocumentCategoryId = document.DocumentCategoryId,
                    Amount = document.Amount,
                    BranchId = document.BranchId,
                    ItemId = document.ItemId,
                    Description = document.Description,
                    Quantity = document.Quantity,
                    DocumentNumber = documentNumber,
                    AmountInWords = document.AmountInWords,
                    CreatedOn = document.CreatedOn,
                    TimeStamp = document.TimeStamp,
                    CreatedBy = document.CreatedBy,
                    
                };
               
           var documentId = this._dataService.SaveDocument(documentDTO, userId);
           if (document.Grades != null)
           {
               if (document.Grades.Any())
               {
                   List<DocumentGradeSize> documentGradeSizeList = new List<DocumentGradeSize>();

                   foreach (var grade in document.Grades)
                   {
                       var gradeId = grade.GradeId;
                       if (grade.Denominations != null)
                       {
                           if (grade.Denominations.Any())
                           {
                               foreach (var denomination in grade.Denominations)
                               {
                                var   amount = (denomination.Quantity * denomination.Price);

                                   var documentGradeSize = new DocumentGradeSize()
                                   {
                                       GradeId = gradeId,
                                       SizeId = denomination.DenominationId,
                                       DocumentId = documentId,

                                       Amount = amount,
                                       Price = denomination.Price,
                                       Quantity = denomination.Quantity,
                                       TimeStamp = DateTime.Now
                                   };
                                   documentGradeSizeList.Add(documentGradeSize);

                               }
                           }
                       }
                   }
                   this._dataService.PurgeDocumentGradeSize(documentId);
                   this.SaveDocumentGradeSizeList(documentGradeSizeList);
               }
           }
           return documentId;
                      
        }

        
        public void MarkAsDeleted(long documentId, string userId)
        {
            _dataService.MarkAsDeleted(documentId, userId);
        }

        void SaveDocumentGradeSizeList(List<DocumentGradeSize> documentGradeSizeList)
        {
            if (documentGradeSizeList != null)
            {
                if (documentGradeSizeList.Any())
                {
                    foreach (var documentGradeSize in documentGradeSizeList)
                    {
                        var documentGradeSizeDTO = new DTO.DocumentGradeSizeDTO()
                        {
                            DocumentId = documentGradeSize.DocumentId,
                            GradeId = documentGradeSize.GradeId,
                           
                            Quantity = documentGradeSize.Quantity,
                            SizeId = documentGradeSize.SizeId,
                            Price = documentGradeSize.Price,
                            Amount = documentGradeSize.Amount,
                            TimeStamp = documentGradeSize.TimeStamp
                        };
                        this.SaveDocumentGradeSize(documentGradeSizeDTO);

                    }
                }
            }
        }


        void SaveDocumentGradeSize(DocumentGradeSizeDTO DocumentGradeSizeDTO)
        {
            _dataService.SaveDocumentGradeSize(DocumentGradeSizeDTO);
        }

        public long Cancelled(Document document, string userId)
        {
            var documentDTO = new DTO.DocumentDTO()
            {
                DocumentId = document.DocumentId,

                UserId = document.UserId,
                DocumentCategoryId = document.DocumentCategoryId,
                Amount = document.Amount,
                BranchId = document.BranchId,
                ItemId = document.ItemId,
                Description = document.Description,
                Quantity = document.Quantity,
                DocumentNumber =document.DocumentNumber,
                AmountInWords = document.AmountInWords,
                CreatedOn = document.CreatedOn,
                TimeStamp = document.TimeStamp,
                CreatedBy = document.CreatedBy,

            };
        var documentId=    _dataService.Cancelled(documentDTO, userId);
        return documentId;
        }

        #region Mapping Methods

        private IEnumerable<Document> MapEFToModel(IEnumerable<EF.Models.Document> data)
        {
            var list = new List<Document>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }


        public Document MapEFToModel(EF.Models.Document data)
        {

            var userName = string.Empty;
            if (data != null)
            {
                var user = _userService.GetAspNetUser(data.UserId);
                if (user != null)
                {
                    userName = user.FirstName + ' ' + user.LastName;
                }
                var document = new Document()
                   {
                       DocumentId = data.DocumentId,
                       UserId = data.UserId,
                       DocumentCategoryId = data.DocumentCategoryId,
                       DocumentCategoryName = data.DocumentCategory != null ? data.DocumentCategory.Name : "",
                       Amount = data.Amount,
                       ItemId = data.ItemId,
                       BranchId = data.BranchId,
                       BranchName = data.Branch != null ? data.Branch.Name : "",
                       Description = data.Description,
                       Quantity = Convert.ToDouble(data.Quantity),
                       UserName = userName,
                       DocumentNumber = data.DocumentNumber,
                       AmountInWords = data.AmountInWords,
                       CreatedOn = data.CreatedOn,
                       TimeStamp = data.TimeStamp,
                       CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                       UpdatedBy = _userService.GetUserFullName(data.AspNetUser1),

                   };
                if (data.DocumentGradeSizes != null)
                {
                    if (data.DocumentGradeSizes.Any())
                    {
                        List<Grade> grades = new List<Grade>();
                        var distinctGrades = data.DocumentGradeSizes.GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        foreach (var documentGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = documentGradeSize.Grade.GradeId,
                                Value = documentGradeSize.Grade.Value,
                                CreatedOn = documentGradeSize.Grade.CreatedOn,
                                TimeStamp = documentGradeSize.Grade.TimeStamp,
                                Deleted = documentGradeSize.Grade.Deleted,
                                CreatedBy = _userService.GetUserFullName(documentGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(documentGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (documentGradeSize.Grade.DocumentGradeSizes != null)
                            {
                                if (documentGradeSize.Grade.DocumentGradeSizes.Any())
                                {
                                    var distinctSizes = documentGradeSize.Grade.DocumentGradeSizes.GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity,
                                            Price = ogs.Price,
                                            Amount = ogs.Amount,
                                        };
                                        //  FlourTransfer.TotalQuantity += (ogs.Quantity * ogs.Size.Value);
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                        }
                        document.Grades = grades;
                    }
                }


                return document;
            }
            else
            {
                return null;
            }

        }

        public DocumentCategory MapEFToModel(EF.Models.DocumentCategory data)
        {
            
            if (data != null)
            {
               
                var documentCategory = new DocumentCategory()
                {
                    DocumentCategoryId = data.DocumentCategoryId,
                   
                    Name = data.Name,
                   TimeStamp = data.TimeStamp,
                    

                };
              

                return documentCategory;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DocumentCategory> MapEFToModel(IEnumerable<EF.Models.DocumentCategory> data)
        {
            var list = new List<DocumentCategory>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

       #endregion
    }
}
