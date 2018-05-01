namespace Prevea.Model.ViewModel
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Model;

    #endregion

    public class TemplateMedicalExaminationViewModel
    {
        public int RequestMedicalExaminationEmployeeId { get; set; }
        public DateTime RequestMedicalExaminationEmployeeDate { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public string ClinicProvince { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentInputTemplateJSON { get; set; }
        public DateTime? DocumentBeginDate { get; set; }
        public DateTime? DocumentEndDate { get; set; }
        public int DocumentStateId { get; set; }
        public string DocumentName { get; set; }
        public List<InputTemplate> DocumentInputTemplates { get; set; }
        public string DocumentUrlRelative { get; set; }
        public string EmployeeName { get; set; }
        public DateTime EmployeeBirthDate { get; set; }
        public string EmployeeDNI { get; set; }
        public string EmployeeWorkStation { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeeProvince { get; set; }
        public DateTime EmployeeChargeDate { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNIF { get; set; }
        public string DoctorName { get; set; }
        public string DoctorCollegiateNumber { get; set; }
    }
}
