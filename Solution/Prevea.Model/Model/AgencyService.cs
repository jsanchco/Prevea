namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class AgencyService
    {
        #region Constructor

        public AgencyService()
        {
            IncludeInContractualDocument = true;
        }

        #endregion

        [ForeignKey("Simulation")]
        public int Id { get; set; }

        public int EngagementTypeId { get; set; }
        public EngagementType EngagementType { get; set; }

        public decimal AmountByEngagementType { get; set; }
        public decimal AmountByRoster { get; set; }
        public string Observations { get; set; }
        public bool IncludeInContractualDocument { get; set; }
        public decimal Total { get; set; }

        public virtual Simulation Simulation { get; set; }
    }
}
