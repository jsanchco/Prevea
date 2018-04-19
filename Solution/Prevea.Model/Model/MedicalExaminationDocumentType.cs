namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class MedicalExaminationDocumentType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnMedicalExaminationDocumentType), Id));

        public virtual ICollection<MedicalExaminationDocuments> MedicalExaminationDocuments { get; set; }
    }

    public enum EnMedicalExaminationDocumentType { MedicalExamination = 1, BloodTest, Electrocardiogram, AudiometricReport, Spirometry, UrineAnalytics, Others }
}
