namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class RequestMedicalExaminationEmployeeState
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnRequestMedicalExaminationEmployeeState), Id));

        public virtual ICollection<RequestMedicalExaminationEmployee> RequestMedicalExaminationEmployees { get; set; }
    }

    public enum EnRequestMedicalExaminationEmployeeState { Pending = 1, Validated, Modified }
}
