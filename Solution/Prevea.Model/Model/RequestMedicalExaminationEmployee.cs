namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class RequestMedicalExaminationEmployee
    {
        [Key, Required]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int RequestMedicalExaminationId { get; set; }
        public RequestMedicalExaminations RequestMedicalExaminations { get; set; }
    }
}
