namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

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
        public int? ContractualDocumentCompanyFirmedId { get; set; }
        public string ContractualDocumentCompanyFirmedEnrollment { get; set; }
        public string ContractualDocumentCompanyFirmedUrlRelative { get; set; }
        public int? ContractualDocumentCompanyParentId { get; set; }
        public string UrlRelative { get; set; }        
        public int CompanyId { get; set; }
        public int SimulationId { get; set; }
    }
}
