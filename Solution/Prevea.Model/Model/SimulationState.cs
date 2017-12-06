namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    #endregion

    public class SimulationState
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnSimulationState), Id));

        public virtual ICollection<Simulation> Simulators { get; set; }
    }

    public enum EnSimulationState { NotMapped, ValidationPending, Modificated, Validated }
}
