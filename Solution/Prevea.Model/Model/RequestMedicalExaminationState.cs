namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    #endregion

    public class RequestMedicalExaminationState
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnSimulationState), Id));

        public virtual ICollection<RequestMedicalExaminations> RequestMedicalExaminations { get; set; }
    }

    public enum EnRequestMedicalExaminationState { Pending = 1, Validated }
}
