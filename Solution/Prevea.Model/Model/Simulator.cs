namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public string CompanyName { get; set; }

        [Required]
        public string NIF { get; set; }

        [Required]
        public int NumberEmployees { get; set; }

        public int SimulatorStateId { get; set; }
        public virtual SimulatorState SimulatorState { get; set; }

        public virtual ForeignPreventionService ForeignPreventionService { get; set; }
        public virtual AgencyService AgencyService { get; set; }
        public virtual TrainingService TrainingService { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public SimulatorCompany SimulatorCompany => SimulatorCompanies?.FirstOrDefault();

        public virtual ICollection<SimulatorCompany> SimulatorCompanies { get; set; }
    }
}
