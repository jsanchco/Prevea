namespace Prevea.Model.Model
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class ModePayment
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<EconomicData> EconomicsDatasCompany { get; set; }
    }

    public enum EnModePayment { NotMapped, Remesa, Transferencia, Caja, Otros }
}
