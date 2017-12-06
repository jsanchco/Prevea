namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class AgencyService
    {
        [ForeignKey("Simulator")]
        public int Id { get; set; }

        public string Observations { get; set; }

        public virtual Simulation Simulator { get; set; }
    }
}
