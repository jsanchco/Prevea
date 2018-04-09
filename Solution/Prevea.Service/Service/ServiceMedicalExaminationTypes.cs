namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<MedicalExaminationDocumentType> GetMedicalExaminationDocumentTypes()
        {
            return Repository.GetMedicalExaminationDocumentTypes();
        }

        public MedicalExaminationDocumentType GetMedicalExaminationDocumentTypeById(int id)
        {
            return Repository.GetMedicalExaminationDocumentTypeById(id);
        }
    }
}
