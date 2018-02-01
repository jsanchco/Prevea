namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;

    #endregion

    public class ContractualDocumentCompany
    {
        [Key, Required]
        public int Id { get; set; }

        public string Enrollment { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Observations { get; set; }

        public string UrlRelative { get; set; }

        public int ContractualDocumentTypeId { get; set; }
        public virtual ContractualDocumentType ContractualDocumentType { get; set; }

        public int? ContractualDocumentCompanyParentId { get; set; }
        public virtual ContractualDocumentCompany ContractualDocumentCompanyParent { get; set; }

        public int? ContractualDocumentCompanyFirmedId { get; set; }
        public virtual ContractualDocumentCompany ContractualDocumentCompanyFirmed { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
