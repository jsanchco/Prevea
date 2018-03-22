namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class MedicalExaminationState
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnMedicalExaminationState), Id));

        public virtual ICollection<MedicalExamination> MedicalExaminations { get; set; }
    }

    public enum EnMedicalExaminationState { Pending = 1, InProcess, Finished }
}
