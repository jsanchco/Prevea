namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    #endregion

    public class Company
    {
        #region Constructor

        public Company()
        {
            Date = DateTime.Now;
            CompanyStateId = (int)EnCompanyState.Alta;
        }

        #endregion

        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NIF { get; set; }

        public string Enrollment
        {
            get
            {
                if (NIF.Length < 6)
                    return NIF;

                var enrollment = NIF.Substring(NIF.Length - 6);
                return char.IsNumber(enrollment, enrollment.Length - 1) ? enrollment.Substring(1, enrollment.Length - 1) : enrollment.Substring(0, enrollment.Length - 1);
            }
        }

        public string Address { get; set; }
        public string Province { get; set; }
        public string Location { get; set; }
        public string PostalCode { get; set; }
        public DateTime Date { get; set; }
        public bool FromSimulation { get; set; }
        public int EmployeesNumber { get; set; }

        public int? CnaeId { get; set; }
        public virtual Cnae Cnae { get; set; }

        public int? AgencyId { get; set; }
        public virtual Agency Agency { get; set; }

        public int GestorId { get; set; }
        public virtual User Gestor { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<ContactPerson> ContactPersons { get; set; }
        public virtual ICollection<Employee> Employees{ get; set; }

        public virtual ICollection<SimulationCompany> SimulationCompanies { get; set; }

        [NotMapped]
        public SimulationCompany SimulationCompanyActive => SimulationCompanies?.FirstOrDefault(x => x.Simulation.Active);

        [Required]
        public int CompanyStateId { get; set; }
        public virtual CompanyState CompanyState { get; set; }

        public virtual ICollection<RequestMedicalExaminations> RequestMedicalExaminations { get; set; }

        public virtual ICollection<PreventivePlan> PreventivesPlans { get; set; }
    }
}
