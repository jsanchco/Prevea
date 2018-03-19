namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class User
    {
        #region Constructor

        public User()
        {
            Guid = Guid.NewGuid();
            Password = "123456";
            UserStateId = (int)EnUserState.Alta;
            BirthDate = DateTime.Now;
            ChargeDate = DateTime.Now;
        }

        #endregion

        [Key, Required]
        public int Id { get; set; }

        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string DNI { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Initials
        {
            get
            {
                var initials = string.Empty;
                var words = FirstName.Split(null as string[], StringSplitOptions.RemoveEmptyEntries);
                initials = words.Aggregate(initials, (current, word) => current + word.Substring(0, 1));

                if (!string.IsNullOrEmpty(LastName))
                    words = LastName.Split(null as string[], StringSplitOptions.RemoveEmptyEntries);

                initials = words.Aggregate(initials, (current, word) => current + word.Substring(0, 1));

                return initials;
            }
        }

        public string PhoneNumber { get; set; }
        public string WorkStation { get; set; }
        public string ProfessionalCategory { get; set; }

        public string Email { get; set; }
        public string Nick { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ChargeDate { get; set; }
        public DateTime? DischargeDate { get; set; }

        [Required]
        public string Password { get; set; }

        public int? UserParentId { get; set; }   
        public virtual User UserParent { get; set; }

        [Required]
        public int UserStateId { get; set; }
        public virtual UserState UserState { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<HistoricDownloadDocument> HistoricDownloadDocuments { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Simulation> SimulationsAssigned { get; set; }
        public virtual ICollection<Simulation> Simulations { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<DoctorWorkSheet> DoctorWorkSheets { get; set; }
        public virtual ICollection<DoctorMedicalExaminationEmployee> DoctorsMedicalExaminationEmployee { get; set; }
    }
}
