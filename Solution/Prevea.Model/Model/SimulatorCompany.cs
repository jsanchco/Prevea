namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class SimulatorCompany
    {
        [Key, Required]
        public int Id { get; set; }

        public int SimulatorId { get; set; }
        public virtual Simulator Simulator { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
