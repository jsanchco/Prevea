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
        public int RequestMedicalExaminationsStateId { get; set; }
        public DateTime Date { get; set; }
        public bool ChangeDate { get; set; }
        public string Observations { get; set; }
        public string SamplerNumber { get; set; }
        public int RequestMedicalExaminationEmployeeStateId { get; set; }
        public string RequestMedicalExaminationEmployeeStateDescription { get; set; }
        public bool Included { get; set; }
        public int? ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Doctors { get; set; }
        public int[] SplitDoctors { get; set; }
        public int MedicalExaminationStateId { get; set; }
        public string MedicalExaminationStateDescription { get; set; }
    }
}
