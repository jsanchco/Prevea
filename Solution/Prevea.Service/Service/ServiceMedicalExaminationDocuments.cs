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
    }
}
