﻿namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.IO;
    using System.Web;

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

        public List<Document> GetChildrenDocument(int parentId)
        {
            return Repository.GetChildrenDocument(parentId);
        }

        public Document GetDocument(int id)
        {
            return Repository.GetDocument(id);
        }

        public Document GetDocument(string name)
        {
            return Repository.GetDocument(name);
        }

        public List<Document> GetDocumentsContractualsByCompany(int? companyId)
        {
            return Repository.GetDocumentsContractualsByCompany(companyId);
        }

        public Result SaveDocument(Document document, bool restoreFile, List<DocumentUserCreator> usersCreators, List<DocumentUserOwner> usersOwners, string extension = null)
        {
            try
            {
                document.DocumentUserCreators = usersCreators;
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

                if (document.Area == null)
                    document.Area = Repository.GetArea(document.AreaId);

                if (!document.IsFirmedDocument && document.Area.EntityId != 3)
                    document = FillDataDocument(documentUserCreator.UserId, document, extension);

                document.DocumentUserOwners = usersOwners;

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
                var document = GetDocument(id);
                List<Document> children;
                bool deleteDocument;
                string url;

                if (document.Area.EntityId == 1)
                {
                    children = GetDocumentsByParent(id, document.DocumentParentId);
                    foreach (var child in children)
                    {
                        url = HttpContext.Current.Server.MapPath(child.UrlRelative);
                        deleteDocument = Repository.DeleteDocument(child.Id);
                        if (deleteDocument == false)
                        {
                            return new Result
                            {
                                Message = "Se ha producido un error en al Borrar el Documento",
                                Object = null,
                                Status = Status.Error
                            };
                        }
                        RemoveFile(url);
                    }
                }
                else
                {
                    children = GetChildrenDocument(id);
                    foreach (var child in children)
                    {
                        url = HttpContext.Current.Server.MapPath(child.UrlRelative);
                        deleteDocument = Repository.DeleteDocument(child.Id);
                        if (deleteDocument == false)
                        {
                            return new Result
                            {
                                Message = "Se ha producido un error en al Borrar el Documento",
                                Object = null,
                                Status = Status.Error
                            };
                        }
                        RemoveFile(url);
                    }

                    if (document.IsFirmedDocument)
                    {
                        document.DocumentParent.DocumentFirmedId = null;
                        UpdateDocument(document, false);
                    }

                    url = HttpContext.Current.Server.MapPath(document.UrlRelative);
                    deleteDocument = Repository.DeleteDocument(document.Id);
                    if (deleteDocument == false)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error en al Borrar el Documento",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                    RemoveFile(url);
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

        public string VerifyNewContractualDocument(Document document)
        {
            var error = string.Empty;
            string errorGeneralData;
            string errorModePayment;

            switch (document.AreaId)
            {
                case 6: // Oferta SPA
                case 7: // Oferta FOR
                case 8: // Oferta GES
                    errorGeneralData = GetErrorInGeneralData(document.Company);
                    if (!string.IsNullOrEmpty(errorGeneralData))
                    {
                        error += $"<H2 style='color: white;'>Error</H2><br />DATOS GENERALES<ul>{errorGeneralData}</ul>";
                    }
                    if (document.Company.ContactPersons == null || document.Company.ContactPersons.Count == 0)
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += "PERSONAS de CONTACTO<ul><li>Agregar persona de contacto</li></ul>";
                    }
                    errorModePayment = GetErrorInPaymentMethod(document.Company);
                    if (!string.IsNullOrEmpty(errorModePayment))
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += $"FORMA de PAGO<ul>{errorModePayment}</ul>";
                    }

                    break;

                case 9: // Contrato SPA
                case 10: // Contrato FOR
                case 11: // Contrato GES
                    errorGeneralData = GetErrorInGeneralData(document.Company);
                    if (!string.IsNullOrEmpty(errorGeneralData))
                    {
                        error += $"<H2>Error</H2><br />DATOS GENERALES<ul>{errorGeneralData}</ul>";
                    }
                    if (document.Company.ContactPersons == null || document.Company.ContactPersons.Count == 0)
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += "PERSONAS de CONTACTO<ul><li>Agregar persona de contacto</li></ul>";
                    }
                    errorModePayment = GetErrorInPaymentMethod(document.Company);
                    if (!string.IsNullOrEmpty(errorModePayment))
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += $"FORMA de PAGO<ul>{errorModePayment}</ul>";
                    }

                    break;

                case 12: // Anexo
                case 14: // Baja Contrato
                    errorGeneralData = GetErrorInGeneralData(document.Company);
                    if (!string.IsNullOrEmpty(errorGeneralData))
                    {
                        error += $"<H2>Error</H2><br />DATOS GENERALES<ul>{errorGeneralData}</ul>";
                    }
                    if (document.Company.ContactPersons == null || document.Company.ContactPersons.Count == 0)
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += "PERSONAS de CONTACTO<ul><li>Agregar persona de contacto</li></ul>";
                    }
                    errorModePayment = GetErrorInPaymentMethod(document.Company);
                    if (!string.IsNullOrEmpty(errorModePayment))
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += $"FORMA de PAGO<ul>{errorModePayment}</ul>";
                    }

                    break;
            }

            return error;
        }

        public Result SaveOtherDocument(HttpPostedFileBase fileOtherDocument, int documentId)
        {
            try
            {
                var contractualDocument = GetDocument(documentId);
                if (contractualDocument == null)
                    return new Result { Status = Status.Error };

                contractualDocument.UrlRelative += Path.GetExtension(fileOtherDocument.FileName);                

                var url = HttpContext.Current.Server.MapPath(contractualDocument.UrlRelative);
                if (url == null)
                    return new Result { Status = Status.Error };

                var directory = Path.GetDirectoryName(url);
                if (directory != null)
                    Directory.CreateDirectory(directory);

                fileOtherDocument.SaveAs(url);

                var result = Repository.UpdateDocument(documentId, contractualDocument);

                if (result == null)
                    return new Result { Status = Status.Error };

                return new Result
                {
                    Status = Status.Ok,
                    Object = result
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return new Result { Status = Status.Error };
            }
        }

        public Result SaveContractualDocumentFirmed(HttpPostedFileBase fileDocumentFirmed, int companyId, int documentId, int userId)
        {
            try
            {
                var contractualDocument = GetDocument(documentId);
                if (contractualDocument == null)
                    return new Result { Status = Status.Error };

                var area = Repository.GetArea(contractualDocument.AreaId);

                var document = new Document
                {
                    CompanyId = companyId,
                    SimulationId = contractualDocument.SimulationId,
                    AreaId = contractualDocument.AreaId,
                    Date = DateTime.Now,
                    DocumentParentId = contractualDocument.Id,
                    DocumentNumber = contractualDocument.DocumentNumber,
                    Edition = contractualDocument.Edition,
                    DocumentStateId = 1,
                    Description = $"{area.Description} Firmad@",
                    IsFirmedDocument = true,                    
                };

                var fileName = Path.GetFileName(contractualDocument.UrlRelative);
                var extension = Path.GetExtension(contractualDocument.UrlRelative);                
                if (fileName == null || extension == null)
                    return new Result { Status = Status.Error };

                fileName = fileName.Replace(extension, "");
                var newFileName = $"{fileName}_FIRMED";
                document.UrlRelative = contractualDocument.UrlRelative.Replace(fileName, newFileName);

                var url = HttpContext.Current.Server.MapPath(document.UrlRelative);
                if (url == null)
                    return new Result { Status = Status.Error };

                var directory = Path.GetDirectoryName(url);
                if (directory != null)
                    Directory.CreateDirectory(directory);

                fileDocumentFirmed.SaveAs(url);

                var documentUserCreators = new List<DocumentUserCreator> {new DocumentUserCreator {UserId = userId}};
                var documentUserOwners = contractualDocument.DocumentUserOwners.Select(documentUserOwner => new DocumentUserOwner {UserId = documentUserOwner.UserId}).ToList();                

                var result = SaveDocument(document, false, documentUserCreators, documentUserOwners);
                if (result.Status == Status.Error)
                    return new Result { Status = Status.Error };
                if (!(result.Object is Document))
                    return new Result { Status = Status.Error };

                contractualDocument.DocumentFirmedId = ((Document) result.Object).Id;
                result = UpdateDocument(contractualDocument, false);
                if (result.Status == Status.Error)
                    return new Result { Status = Status.Error };
                if (!(result.Object is Document))
                    return new Result { Status = Status.Error };

                return new Result
                {
                    Status = Status.Ok,
                    Object = result.Object
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return new Result { Status = Status.Error };
            }
        }

        public Document GetDocumentByArea(int areaId)
        {
            var document = new Document();

            var area = GetArea(areaId);
            document.Area = area;
            document.AreaId = area.Id;

            string fileName;

            switch (areaId)
            {
                case 16:
                    document.DocumentNumber = Repository.GetNumberDocumentsByArea(areaId) + 1;
                    document.Edition = 1;
                    document.Description = area.Description;

                    fileName = $"{area.Name}_{document.DocumentNumber:00000}_{document.Edition}.pdf";

                    document.UrlRelative = $"{area.Url}{fileName}";
                    document.Date = DateTime.Now;
                    document.DateModification = null;
                    document.DocumentStateId = (int) EnDocumentState.Pending;

                    break;

                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                    document.DocumentNumber = Repository.GetNumberDocumentsByArea(areaId) + 1;
                    document.Edition = 1;
                    document.Description = area.Description;

                    fileName = $"{area.Name}_{document.DocumentNumber:00000}_{document.Edition}.xxx";

                    document.UrlRelative = $"{area.Url}{fileName}";
                    document.Date = DateTime.Now;
                    document.DateModification = null;
                    document.DocumentStateId = (int)EnDocumentState.Activo;

                    break;
            }

            return document;
        }

        public Result SaveSignature(int documentId, byte[] signature)
        {
            try
            {
                var document = Repository.SaveSignature(documentId, signature);

                if (document == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la grabacion del Documento",
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
                    Message = "Se ha producido un error en la grabacion del Documento",
                    Object = null,
                    Status = Status.Error
                };
            }           
        }

        private Document FillDataDocument(int userId, Document document, string extension)
        {
            var area = Repository.GetArea(document.AreaId);

            if (extension == null)
                extension = GetExtension(userId);

            document.DocumentNumber = Repository.GetNumberDocumentsByArea(document.AreaId) + 1;

            var documentsByParent = Repository.GetDocumentsByParent(document.Id, document.DocumentParentId);
            document.Edition = documentsByParent.Count + 1;

            switch (area.EntityId)
            {
                case 1:
                    if (document.DocumentParentId == null)
                        document.Observations = "*** Documento Original ***";
                    else
                        document.Description = documentsByParent[documentsByParent.Count - 1].Description;
                    break;

                case 2:
                case 3:
                    document.Description = area.Description;
                    break;
            }

            var fileName = $"{area.Name}_{document.DocumentNumber:00000}_{document.Edition}{extension}";

            document.UrlRelative = $"{area.Url}{fileName}";
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

            switch (area.EntityId)
            {
                case 1:
                    if (document.DocumentParentId == null)
                        document.Observations = "*** Documento Original ***";
                    else
                        document.Description = documentsByParent[documentsByParent.Count - 1].Description;
                    break;

                case 2:
                    document.Description = document.HasFirm ? document.Description = $"{area.Description} Firmad@" : document.Description = area.Description;
                    break;
            }

            var fileName = $"{area.Name}_{document.DocumentNumber:00000}_{document.Edition}{extension}";

            document.UrlRelative = $"{area.Url}{fileName}";
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
            documentOriginal.UrlRelative = $"{documentOriginal.Area.Url}{fileName}";

            return documentOriginal;
        }

        private static string GetErrorInGeneralData(Company company)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(company.Address))
                error += "<li>Seleccionar Dirección</li>";
            if (string.IsNullOrEmpty(company.Province))
                error += "<li>Seleccionar Provincia</li>";
            if (company.Cnae == null)
                error += "<li>Seleccionar Actividad</li>";

            return error;
        }

        private static string GetErrorInPaymentMethod(Company company)
        {
            var error = string.Empty;

            if (company.PaymentMethod == null)
                return "Faltan datos por rellenar";

            if (company.PaymentMethod.ModePayment == null)
                error += "<li>Seleccionar Modalidad de Pago</li>";
            if (string.IsNullOrEmpty(company.PaymentMethod.AccountNumber))
                error += "<li>Seleccionar Nº de Cuenta</li>";

            return error;
        }
    }
}
