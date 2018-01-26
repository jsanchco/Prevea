namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ForeignPreventionService
    {
        #region Constructor

        public ForeignPreventionService()
        {
            IncludeInContractualDocument = true;
        }

        #endregion

        [ForeignKey("Simulation")]
        public int Id { get; set; }

        public decimal? AmountTecniques { get; set; }
        public decimal? AmountHealthVigilance { get; set; }
        public decimal? AmountMedicalExamination { get; set; }
        public bool IncludeInContractualDocument { get; set; }
        public decimal Total { get; set; }

        public virtual Simulation Simulation { get; set; }
    }
}
