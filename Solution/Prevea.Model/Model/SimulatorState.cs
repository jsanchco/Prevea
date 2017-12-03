namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    #endregion

    public class SimulatorState
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnSimulatorState), Id));

        public virtual ICollection<Simulator> Simulators { get; set; }
    }

    public enum EnSimulatorState { NotMapped, ValidationPending, Modificated, Validated }
}
