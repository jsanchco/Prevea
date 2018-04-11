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

        public int MedicalExaminationPending { get; set; }
        public int MedicalExaminationInProcess { get; set; }
        public int MedicalExaminationFinished { get; set; }
    }
}
