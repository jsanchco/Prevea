namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    #endregion

    [NotMapped]
    public class DoctorWorkSheet
    {
        public DateTime Date { get; set; }

        public int MedicalExaminationPendings { get; set; }
        public int MedicalExaminationInCourse { get; set; }
        public int MedicalExaminationFinished { get; set; }
    }
}
