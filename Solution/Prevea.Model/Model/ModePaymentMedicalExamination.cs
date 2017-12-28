namespace Prevea.Model.Model
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ModePaymentMedicalExamination
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnModePaymentMedicalExamination), Id));


        public virtual ICollection<EconomicData> EconomicsDatasCompany { get; set; }
    }

    public enum EnModePaymentMedicalExamination { NotMapped, ALaFirmaDelContrato, ALaRealizacion, Otros }
}
