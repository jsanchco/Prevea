using System.Linq;

namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public List<Document> GetDocuments(int documentStateId = 0)
        {
            return documentStateId == 0 ? Repository.GetDocuments() : Repository.GetDocumentsByState(documentStateId);
        }

        public List<Document> GetDocumentsByParent(int id, int? parentId)
        {
            return Repository.GetDocumentsByParent(id, parentId);
        }

        public Document GetDocument(int id)
        {
            return Repository.GetDocument(id);
        }

        public Result SaveDocument(Document document, bool restoreFile)
        {
            try
            {
                var documentUserCreator = document.DocumentUserCreators.FirstOrDefault();
                if (documentUserCreator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }
                document = FillDataDocument(documentUserCreator.UserId, document);

                if (restoreFile)
                    RestoreFile(documentUserCreator.UserId, document.UrlRelative);

                document = Repository.SaveDocument(document);

                if (document == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }
                
                return new Result
                {
                    Message = "La Grabación del Documento se ha producido con éxito",
                    Object = document,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Documento",
                    Object = document,
                    Status = Status.Error
                };
            }                       
        }

        public Result SaveDocumentWithParent(Document document)
        {
            try
            {
                var documentUserCreator = document.DocumentUserCreators.FirstOrDefault();
                if (documentUserCreator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }
                document = FillDataDocumentWithParent(document);

                RestoreFile(documentUserCreator.UserId, document.UrlRelative);

                document = Repository.SaveDocumentWithParent(document);

                if (document == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Documento se ha producido con éxito",
                    Object = document,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Documento",
                    Object = document,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateDocument(Document document, bool updateFile)
        {
            try
            {
                document = FillDataDocumentUpdate(document, updateFile);

                var documentUserCreator = document.DocumentUserCreators.FirstOrDefault();
                if (documentUserCreator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                if (updateFile)
                    RestoreFile(documentUserCreator.UserId, document.UrlRelative);

                document = Repository.UpdateDocument(document.Id, document);

                if (document == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Documento se ha producido con éxito",
                    Object = document,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Documento",
                    Object = document,
                    Status = Status.Error
                };
            }
        }

        public Result UnsubscribeDocument(int id)
        {
            try
            {
                var document = Repository.UnsubscribeDocument(id);

                if (document == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en al Dar de Baja el Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Documento se ha producido con éxito",
                    Object = document,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en al Dar de Baja el Documento",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result SubscribeDocument(int id)
        {
            try
            {
                var document = Repository.SubscribeDocument(id);

                if (document == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Dar de Alta el Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Documento se ha producido con éxito",
                    Object = document,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Dar de Alta el Documento",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteDocument(int id)
        {
            try
            {
                var result = Repository.DeleteDocument(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en al Borrar el Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Documento se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en al Borrar el Documento",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        private Document FillDataDocument(int userId, Document document)
        {
            var extension = GetExtension(userId);
            var area = Repository.GetArea(document.AreaId);

            document.DocumentNumber = Repository.GetNumberDocumentsByArea(document.AreaId) + 1;

            var documentsByParent = Repository.GetDocumentsByParent(document.Id, document.DocumentParentId);
            document.Edition = documentsByParent.Count + 1;

            if (document.DocumentParentId == null)
                document.Observations = "*** Documento Original ***";
            else
                document.Description = documentsByParent[documentsByParent.Count - 1].Description;

            var fileName = $"{area.Name}_{document.DocumentNumber:00000}_{document.Edition}{extension}";

            document.UrlRelative = $"{area.Entity.Directory}/{area.Name}/{fileName}";
            document.Date = DateTime.Now;
            document.DateModification = documentsByParent.Count == 0 ? document.Date : documentsByParent[documentsByParent.Count - 1].Date;
            document.DocumentStateId = 1;            

            return document;
        }

        private Document FillDataDocumentWithParent(Document document)
        {
            if (document.DocumentParentId == null)
                return null;

            var documentParent = Repository.GetDocument((int)document.DocumentParentId);

            var extension = string.Empty;
            var documentUserCreator = documentParent.DocumentUserCreators.FirstOrDefault();
            if (documentUserCreator != null)
                extension = GetExtension(documentUserCreator.UserId);

            var area = Repository.GetArea(documentParent.AreaId);
            document.AreaId = area.Id;

            document.DocumentNumber = documentParent.DocumentNumber;

            var documentsByParent = Repository.GetDocumentsByParent(document.Id, document.DocumentParentId);
            document.Edition = documentsByParent.Count + 1;

            if (document.DocumentParentId == null)
                document.Observations = "*** Documento Original ***";
            else
                document.Description = documentsByParent[documentsByParent.Count - 1].Description;

            var fileName = $"{area.Name}_{document.DocumentNumber:00000}_{document.Edition}{extension}";

            document.UrlRelative = $"{area.Entity.Directory}/{area.Name}/{fileName}";
            document.Date = DateTime.Now;
            document.DateModification = documentsByParent.Count == 0 ? document.Date : documentsByParent[documentsByParent.Count - 1].Date;
            document.DocumentStateId = 1;

            return document;
        }

        private Document FillDataDocumentUpdate(Document document, bool updateFile)
        {
            var documentOriginal = Repository.GetDocument(document.Id);

            documentOriginal.Observations = document.Observations;
            documentOriginal.Description = document.Description;
            documentOriginal.Date = DateTime.Now;

            if (!updateFile)
                return documentOriginal;

            var extension = string.Empty;
            var documentUserCreator = documentOriginal.DocumentUserCreators.FirstOrDefault();
            if (documentUserCreator != null)
                extension = GetExtension(documentUserCreator.UserId);
            var fileName = $"{documentOriginal.Area.Name}_{documentOriginal.DocumentNumber:00000}_{documentOriginal.Edition}{extension}";
            documentOriginal.UrlRelative = $"{document.Area.Entity.Directory}/{documentOriginal.Area.Name}/{fileName}";

            return documentOriginal;
        }
    }
}
