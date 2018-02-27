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

        public int RequestMedicalExaminationsId { get; set; }
        public RequestMedicalExaminations RequestMedicalExaminations { get; set; }

        public int ContactPersonId { get; set; }
        public User ContactPerson { get; set; }
    }
}
