namespace Prevea.Model.Model
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class EstablishmentType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnEstablishmentType), Id));

        public virtual ICollection<WorkCenter> WorkCenters { get; set; }
    }

    public enum EnEstablishmentType { NotMapped, Permanent, Movil }
}
