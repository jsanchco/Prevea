using System.Linq;

namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;

    #endregion

    public class Simulator
    {
        #region Constructor

        public Simulator()
        {
            Date = DateTime.Now;
        }

        #endregion

        [Key, Required]
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

        public SimulatorCompany SimulatorCompany => SimulatorCompanies?.FirstOrDefault();

        public virtual ICollection<SimulatorCompany> SimulatorCompanies { get; set; }
    }
}
