namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class IncidenceState
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnIncidenceState), Id));

        public virtual ICollection<Incidence> Incidences { get; set; }
    }

    public enum EnIncidenceState { Alta = 1, InProcess, Closed, ReOpen }
}
