namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;

    #endregion

    public class RequestMedicalExaminationEmployee
    {
        [Key, Required]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int RequestMedicalExaminationsId { get; set; }
        public RequestMedicalExaminations RequestMedicalExaminations { get; set; }

        public DateTime Date { get; set; }

        public bool ChangeDate { get; set; }

        public string Observations { get; set; }
        public string SamplerNumber { get; set; }

        public int RequestMedicalExaminationEmployeeStateId { get; set; }
        public RequestMedicalExaminationEmployeeState RequestMedicalExaminationEmployeeState { get; set; }

        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        public virtual ICollection<DoctorMedicalExaminationEmployee> DoctorsMedicalExaminationEmployee { get; set; }
    }    
}
