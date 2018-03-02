namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class RequestMedicalExaminationEmployeeViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeDNI { get; set; }
        public int RequestMedicalExaminationsId { get; set; }
        public DateTime Date { get; set; }
        public int RequestMedicalExaminationEmployeeStateId { get; set; }
        public string RequestMedicalExaminationEmployeeStateDescription { get; set; }
        public bool Included { get; set; }
    }
}
