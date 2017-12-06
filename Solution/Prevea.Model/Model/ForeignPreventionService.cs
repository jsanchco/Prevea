namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ForeignPreventionService
    {
        [ForeignKey("Simulator")]
        public int Id { get; set; }

        public decimal? AmountTecniques { get; set; }
        public decimal? AmountHealthVigilance { get; set; }
        public decimal? AmountMedicalExamination { get; set; }

        public virtual Simulation Simulator { get; set; }
    }
}
