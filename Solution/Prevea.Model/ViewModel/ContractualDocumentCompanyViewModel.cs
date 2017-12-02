namespace Prevea.Model.ViewModel
{
    #region Using

    using System;
    using Model;

    #endregion

    public class ContractualDocumentCompanyViewModel
    {
        public int Id { get; set; }
        public string Enrollment { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Observations { get; set; }
        public int ContractualDocumentTypeId { get; set; }
        public string ContractualDocumentTypeName { get; set; }
        public int CompanyId { get; set; }
    }
}
