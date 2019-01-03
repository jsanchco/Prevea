namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    #endregion

    public class PriorityCorrectiveAction
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnPriorityCorrectiveAction), Id));

        public virtual ICollection<CorrectiveAction> CorrectiveActions { get; set; }
    }

    public enum EnPriorityCorrectiveAction { Alta = 1, Media, Baja }
}
