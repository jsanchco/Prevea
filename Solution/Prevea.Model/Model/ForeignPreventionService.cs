namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ForeignPreventionService
    {
        [ForeignKey("Simulation")]
        public int Id { get; set; }

        public decimal? AmountTecniques { get; set; }
        public decimal? AmountHealthVigilance { get; set; }
        public decimal? AmountMedicalExamination { get; set; }
        public decimal Total { get; set; }

        public virtual Simulation Simulation { get; set; }
    }
}
