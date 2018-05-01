namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Web;
    using System.Diagnostics;
    using System.Linq;
    using System.IO;

    #endregion

    public partial class Service
    {
        public List<MedicalExaminationDocuments> GetMedicalExaminationDocuments()
        {
            return Repository.GetMedicalExaminationDocuments();
        }

        public MedicalExaminationDocuments GetMedicalExaminationDocumentById(int id)
        {
            return Repository.GetMedicalExaminationDocumentById(id);
        }

        public Result SaveMedicalExaminationDocument(int requestMedicalExaminationEmployeeId, Document document, int userId)
        {
            try
            {
                var requestMedicalExaminationEmployee =
                    GetRequestMedicalExaminationEmployeeById(requestMedicalExaminationEmployeeId);

                document = GetDocumentByArea(document.AreaId);

                var documentUserOwners = requestMedicalExaminationEmployee.RequestMedicalExaminations.Company.ContactPersons.Select(contactPerson => new DocumentUserOwner { UserId = contactPerson.UserId }).ToList();
                documentUserOwners.Add(new DocumentUserOwner { UserId = requestMedicalExaminationEmployee.Employee.UserId });
                var result = SaveDocument(document,
                    true,
                    new List<DocumentUserCreator> { new DocumentUserCreator { UserId = userId } },
                    documentUserOwners,
                    ".xxx");
                if (result.Status == Status.Error)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en SaveMedicalExaminationDocument",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var medicalExaminationDocument =
                    Repository.SaveMedicalExaminationDocument(new MedicalExaminationDocuments
                    {
                        RequestMedicalExaminationEmployeeId = requestMedicalExaminationEmployeeId,
                        DocumentId = ((Document)result.Object).Id
                    });

                if (medicalExaminationDocument == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en SaveMedicalExaminationDocument",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de SaveMedicalExaminationDocument se ha producido con éxito",
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

        public Result DeleteMedicalExaminationDocument(int id, int requestMedicalExaminationEmployeeId)
        {
            try
            {
                var document = Repository.GetDocument(id);
                var physicalPath = HttpContext.Current.Server.MapPath(document.UrlRelative);
                RemoveFile(physicalPath);

                var medicalExaminationDocument =
                    GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(
                        requestMedicalExaminationEmployeeId).FirstOrDefault(x => x.DocumentId == id);
                if (medicalExaminationDocument == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }
                var result = Repository.DeleteMedicalExaminationDocument(medicalExaminationDocument.Id);
                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var resutDeleteDocument = DeleteDocument(id);
                if (resutDeleteDocument.Status == Status.Error)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Documento",
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
                    Message = "Se ha producido un error al Borrar el Documento",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public List<MedicalExaminationDocuments> GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(int requestMedicalExaminationEmployeeId)
        {
            return Repository.GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(
                requestMedicalExaminationEmployeeId);
        }

        public MedicalExaminationDocuments GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeIdAndAreaId(
            int requestMedicalExaminationEmployeeId, int areaId)
        {
            return Repository.GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeIdAndAreaId(
                requestMedicalExaminationEmployeeId, areaId);
        }

        public Result SaveFileMedicalExaminationDocument(HttpPostedFileBase fileDocument, int medicalExaminationDocumentId)
        {
            try
            {
                var document = GetDocument(medicalExaminationDocumentId);
                if (document == null)
                    return new Result { Status = Status.Error };

                if (document.UrlRelative.EndsWith("xxx"))
                {
                    document.UrlRelative = document.UrlRelative.Replace("xxx", "pdf");
                    var result = UpdateDocument(document, false);
                    if (result.Status == Status.Error)
                        return new Result { Status = Status.Error };
                }
                
                var url = Path.Combine(HttpContext.Current.Server.MapPath(document.UrlRelative));
                fileDocument.SaveAs(url);

                return new Result
                {
                    Status = Status.Ok,
                    Object = document
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return new Result { Status = Status.Error };
            }

        }
    }
}
