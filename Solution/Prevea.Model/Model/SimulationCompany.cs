namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class SimulationCompany
    {
        [Key, Required]
        public int Id { get; set; }

        public int SimulatorId { get; set; }
        public virtual Simulation Simulator { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
