namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    #endregion

    public class MedicalExaminationDocuments
    {
        [Key, Required]
        public int Id { get; set; }

        public string Url { get; set; }

        public string Enrollment { get; set; }

        public int RequestMedicalExaminationEmployeeId  { get; set; }
        public virtual RequestMedicalExaminationEmployee RequestMedicalExaminationEmployee { get; set; }

        public int MedicalExaminationDocumentTypeId { get; set; }
        public virtual MedicalExaminationDocumentType MedicalExaminationDocumentType { get; set; }

        public virtual ICollection<ModelDocument> ModelsDocuments { get; set; }
    }
}
