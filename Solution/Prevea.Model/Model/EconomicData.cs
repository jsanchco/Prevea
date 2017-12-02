namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class EconomicData
    {
        [ForeignKey("Company")]
        public int Id { get; set; }

        public int ActualNumberEmployees { get; set; }
        public decimal? AmountTecniques { get; set; }
        public decimal? AmountHealthVigilance { get; set; }
        public decimal? AmountMedicalExamination { get; set; }

        public virtual Company Company { get; set; }
    }
}
