namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class RequestMedicalExaminationsViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Observations { get; set; }
        public int RequestMedicalExaminationStateId { get; set; }
        public string RequestMedicalExaminationStateDescription { get; set; }
        public int CompanyId { get; set; }
        public string ListEmployees { get; set; }
    }
}
