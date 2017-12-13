namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class Simulation
    {
        #region Constructor

        public Simulation()
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

        public int SimulationStateId { get; set; }
        public virtual SimulationState SimulationState { get; set; }

        public virtual ForeignPreventionService ForeignPreventionService { get; set; }
        public virtual AgencyService AgencyService { get; set; }
        public virtual TrainingService TrainingService { get; set; }

        public DateTime Date { get; set; }
        public DateTime? DateAssigned { get; set; }

        public decimal Total { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int? UserAssignedId { get; set; }
        public virtual User UserAssigned { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        [NotMapped]
        public SimulationCompany SimulationCompany => SimulationCompanies?.FirstOrDefault();

        public virtual ICollection<SimulationCompany> SimulationCompanies { get; set; }
    }
}
