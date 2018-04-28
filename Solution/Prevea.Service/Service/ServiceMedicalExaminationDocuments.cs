namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Web;
    using System.Diagnostics;
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

        public Result SaveMedicalExaminationDocument(MedicalExaminationDocuments medicalExaminationDocument)
        {
            try
            {
                //if (string.IsNullOrEmpty(medicalExaminationDocument.Enrollment))
                //{
                //    string enrollment;
                //    var countMaxDocuments = Repository.GetMaxMedicalExaminationDocumentByType(
                //        medicalExaminationDocument.RequestMedicalExaminationEmployeeId,
                //        medicalExaminationDocument.MedicalExaminationDocumentTypeId) + 1;

                //    switch (medicalExaminationDocument.MedicalExaminationDocumentTypeId)
                //    {
                //        case (int)EnMedicalExaminationDocumentType.BloodTest:
                //            enrollment = $"ME_BT.{medicalExaminationDocument.RequestMedicalExaminationEmployeeId}.{countMaxDocuments}";
                //            break;
                //        case (int)EnMedicalExaminationDocumentType.Electrocardiogram:
                //            enrollment = $"ME_EL.{medicalExaminationDocument.RequestMedicalExaminationEmployeeId}.{countMaxDocuments}";
                //            break;
                //        case (int)EnMedicalExaminationDocumentType.AudiometricReport:
                //            enrollment = $"ME_AR.{medicalExaminationDocument.RequestMedicalExaminationEmployeeId}.{countMaxDocuments}";
                //            break;
                //        case (int)EnMedicalExaminationDocumentType.Spirometry:
                //            enrollment = $"ME_SP.{medicalExaminationDocument.RequestMedicalExaminationEmployeeId}.{countMaxDocuments}";
                //            break;
                //        case (int)EnMedicalExaminationDocumentType.UrineAnalytics:
                //            enrollment = $"ME_UA.{medicalExaminationDocument.RequestMedicalExaminationEmployeeId}.{countMaxDocuments}";
                //            break;
                //        case (int)EnMedicalExaminationDocumentType.Others:
                //            enrollment = $"ME_OT.{medicalExaminationDocument.RequestMedicalExaminationEmployeeId}.{countMaxDocuments}";
                //            break;

                //        default:
                //            enrollment = $"ME_OT.{medicalExaminationDocument.RequestMedicalExaminationEmployeeId}.{countMaxDocuments}";
                //            break;
                //    }
                //    medicalExaminationDocument.Enrollment = enrollment;
                //}

                medicalExaminationDocument = Repository.SaveMedicalExaminationDocument(medicalExaminationDocument);
                if (medicalExaminationDocument == null)
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
                    Message = "La Grabación del la Clínica se ha producido con éxito",
                    Object = medicalExaminationDocument,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Documento",
                    Object = medicalExaminationDocument,
                    Status = Status.Error
                };
            }

        }

        public Result DeleteMedicalExaminationDocument(int id)
        {
            try
            {
                var medicalExaminationDocument = Repository.GetMedicalExaminationDocumentById(id);
                var physicalPath = HttpContext.Current.Server.MapPath(medicalExaminationDocument.Document.UrlRelative);
                if (!RemoveFile(physicalPath))
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Documento",
                        Object = null,
                        Status = Status.Error
                    };

                }

                var result = Repository.DeleteMedicalExaminationDocument(id);
                if (result == false)
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

        public Result SaveFileMedicalExaminationDocument(HttpPostedFileBase fileDocument, int medicalExaminationDocumentId)
        {
            try
            {
                var medicalExaminationDocument = GetMedicalExaminationDocumentById(medicalExaminationDocumentId);
                if (medicalExaminationDocument == null)
                    return new Result { Status = Status.Error };

                var requestMedicalExaminationEmployee =
                    Repository.GetRequestMedicalExaminationEmployeeById(medicalExaminationDocument
                        .RequestMedicalExaminationEmployeeId);

                //medicalExaminationDocument.Url = $"~/App_Data/Companies/{requestMedicalExaminationEmployee.RequestMedicalExaminations.Company.NIF}/MedicalExaminations/{medicalExaminationDocument.Enrollment}.pdf";

                //var url = Path.Combine(HttpContext.Current.Server.MapPath(medicalExaminationDocument.Url));
                //fileDocument.SaveAs(url);

                var result = SaveMedicalExaminationDocument(medicalExaminationDocument);
                if (result.Status == Status.Error)
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
    }
}
