namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class ModelDocument
    {
        [Key]
        public int Id { get; set; }

        public int MedicalExaminationDocumentId { get; set; }
        public virtual MedicalExaminationDocuments MedicalExaminationDocument { get; set; }

        public int ContractualDocumentCompanyId { get; set; }
        public virtual ContractualDocumentCompany ContractualDocumentCompany { get; set; }

        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }
}
