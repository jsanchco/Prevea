namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class MedicalExamination
    {
        [ForeignKey("RequestMedicalExaminationEmployee")]
        public int Id { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MedicalExaminationJSON { get; set; }

        public string Enrollment { get; set; }

        public string Url { get; set; }

        public int MedicalExaminationStateId { get; set; }
        public virtual MedicalExaminationState MedicalExaminationState { get; set; }

        public virtual RequestMedicalExaminationEmployee RequestMedicalExaminationEmployee { get; set; }
    }
}
