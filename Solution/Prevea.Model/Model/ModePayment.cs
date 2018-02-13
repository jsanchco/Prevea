namespace Prevea.Model.Model
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ModePayment
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnModePayment), Id));
    }

    public enum EnModePayment { Remesa = 1, Transferencia, Caja, Otros }
}
