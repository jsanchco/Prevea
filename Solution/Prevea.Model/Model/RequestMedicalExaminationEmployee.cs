namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;

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

        public int RequestMedicalExaminationEmployeeStateId { get; set; }
        public RequestMedicalExaminationEmployeeState RequestMedicalExaminationEmployeeState { get; set; }
    }    
}
