namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class RiskEvaluation
    {
        [Key, Required]
        public int Id { get; set; }

        [ForeignKey("WorkStation")]
        public int WorkStationId { get; set; }
        public virtual WorkStation WorkStation { get; set; }

        [ForeignKey("DeltaCode")]
        public int DeltaCodeId { get; set; }
        public virtual DeltaCode DeltaCode { get; set; }

        public int Probability { get; set; }
        public int Severity { get; set; }
        public int RiskValue { get; set; }
        public int Priority { get; set; }
        public string Preventive { get; set; }
    }
}
