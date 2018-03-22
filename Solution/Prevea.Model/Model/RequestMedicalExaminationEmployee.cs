namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class RequestMedicalExaminationEmployee
    {
        #region Constructor

        public RequestMedicalExaminationEmployee()
        {
            DateOnlyDay = new DateTime(Date.Year, Date.Month, Date.Day);
        }

        #endregion

        [Key, Required]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int RequestMedicalExaminationsId { get; set; }
        public virtual RequestMedicalExaminations RequestMedicalExaminations { get; set; }

        public DateTime Date { get; set; }
        
        public DateTime DateOnlyDay { get; set; }

        public bool ChangeDate { get; set; }

        public string Observations { get; set; }
        public string SamplerNumber { get; set; }

        public int RequestMedicalExaminationEmployeeStateId { get; set; }
        public virtual RequestMedicalExaminationEmployeeState RequestMedicalExaminationEmployeeState { get; set; }

        public int? ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }

        public virtual MedicalExamination MedicalExamination { get; set; }

        public virtual ICollection<DoctorMedicalExaminationEmployee> DoctorsMedicalExaminationEmployee { get; set; }

        [NotMapped]
        public bool Included { get; set; }

        [NotMapped]
        public string Doctors { get; set; }

        [NotMapped]
        public int[] SplitDoctors { get; set; }
    }    
}
