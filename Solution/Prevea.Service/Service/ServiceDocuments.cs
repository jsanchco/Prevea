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

        public Result SaveDocument(int userCreatorId, int? userOwnerId, Document document)
        {
            try
            {
                document = FillDataDocument(userCreatorId, document);

                RestoreFile(userCreatorId, document.UrlRelative);

                document = Repository.SaveDocument(userCreatorId, userOwnerId, document);

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

        public Result SaveDocumentWithParent(int userCreatorId, Document document)
        {
            try
            {
                document = FillDataDocumentWithParent(document);

                RestoreFile(userCreatorId, document.UrlRelative);

                document = Repository.SaveDocumentWithParent(userCreatorId, document);

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

                if (updateFile)
                    RestoreFile(document.DocumentUserCreatorId, document.UrlRelative);

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

            var fileName = $"{area.Name}_{document.DocumentNumber:000}_{document.Edition}{extension}";

            document.UrlRelative = $"{DocumentUpload}/{area.Name}/{fileName}";
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

            document.DocumentUserCreatorId = documentParent.DocumentUserCreatorId;

            var extension = GetExtension(documentParent.DocumentUserCreator.UserId);
            var area = Repository.GetArea(documentParent.AreaId);
            document.AreaId = area.Id;

            document.DocumentNumber = documentParent.DocumentNumber;

            var documentsByParent = Repository.GetDocumentsByParent(document.Id, document.DocumentParentId);
            document.Edition = documentsByParent.Count + 1;

            if (document.DocumentParentId == null)
                document.Observations = "*** Documento Original ***";
            else
                document.Description = documentsByParent[documentsByParent.Count - 1].Description;

            var fileName = $"{area.Name}_{document.DocumentNumber:000}_{document.Edition}{extension}";

            document.UrlRelative = $"{DocumentUpload}/{area.Name}/{fileName}";
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

            var extension = GetExtension(documentOriginal.DocumentUserCreatorId);
            var fileName = $"{documentOriginal.Area.Name}_{documentOriginal.DocumentNumber:000}_{documentOriginal.Edition}{extension}";
            documentOriginal.UrlRelative = $"{DocumentUpload}/{documentOriginal.Area.Name}/{fileName}";

            return documentOriginal;
        }
    }
}
