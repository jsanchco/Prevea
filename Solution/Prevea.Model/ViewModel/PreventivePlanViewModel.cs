namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class PreventivePlanViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEnrollment { get; set; }
        public int DocumentId { get; set; }
        public DateTime DocumentBeginDate { get; set; }
        public DateTime DocumentEndDate { get; set; }
    }
}
