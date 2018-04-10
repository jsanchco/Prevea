namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Data.Entity;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public List<MedicalExaminationDocumentType> GetMedicalExaminationDocumentTypes()
        {
            return Context.MedicalExaminationDocumentTypes
                .Include(x => x.MedicalExaminationDocuments)
                .ToList();
        }

        public MedicalExaminationDocumentType GetMedicalExaminationDocumentTypeById(int id)
        {
            return Context.MedicalExaminationDocumentTypes
                .Include(x => x.MedicalExaminationDocuments)
                .FirstOrDefault(x => x.Id == id);
        }

        public int GetMaxMedicalExaminationDocumentByType(int requestMedicalExaminationEmployeeId, int medicalExaminationDocumentTypeId)
        {
            return Context.MedicalExaminationDocuments
                .Count(x => x.RequestMedicalExaminationEmployeeId == requestMedicalExaminationEmployeeId &&
                            x.MedicalExaminationDocumentTypeId == medicalExaminationDocumentTypeId);
        }
    }
}
