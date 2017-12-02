namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class ContractualDocumentType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ContractualDocumentCompany> ContractualsDocumentsCompany { get; set; }
    }

    public enum EnContractualDocumentType { NotMapped, Oferta, Contrato, Anexo }
}
