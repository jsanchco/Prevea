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

        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public int RequestMedicalExaminationEmployeeId  { get; set; }
        public virtual RequestMedicalExaminationEmployee RequestMedicalExaminationEmployee { get; set; }
    }
}
