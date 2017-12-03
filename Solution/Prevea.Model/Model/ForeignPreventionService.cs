namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class ForeignPreventionService
    {
        #region Constructor

        public ForeignPreventionService()
        {
            Date = DateTime.Now;
        }

        #endregion

        [ForeignKey("SimulatorCompany")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NIF { get; set; }

        [Required]
        public int NumberEmployees { get; set; }

        public decimal? AmountTecniques { get; set; }
        public decimal? AmountHealthVigilance { get; set; }
        public decimal? AmountMedicalExamination { get; set; }

        public bool IsBlocked { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual SimulatorCompany SimulatorCompany { get; set; }
    }
}
