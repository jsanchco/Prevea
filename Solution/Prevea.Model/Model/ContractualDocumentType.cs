﻿namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ContractualDocumentType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnContractualDocumentType), Id));

        public virtual ICollection<ContractualDocumentCompany> ContractualsDocumentsCompany { get; set; }
    }

    public enum EnContractualDocumentType { OfferSpa = 1, OfferGes, OfferFor, ContractSpa, ContractGes, ContractFor, Annex, OtherDocuments, UnSubscribeContract, Firmed }
}
