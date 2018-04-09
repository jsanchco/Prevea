namespace Prevea.Model.ViewModel
{
    #region Using

    using System;
    using Model;

    #endregion

    public class UserViewModel
    {
        #region Constructor

        public UserViewModel()
        {
            Guid = Guid.NewGuid();
            Password = "123456";
            UserStateId = (int)EnUserState.Alta;
        }

        #endregion

        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public string DNI { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkStation { get; set; }
        public string ProfessionalCategory { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ChargeDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string Password { get; set; }
        public int? UserParentId { get; set; }
        public string UserParentInitials { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEnrollment { get; set; }
        public int? AgencyId { get; set; }
        public string AgencyName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int UserStateId { get; set; }
        public string UserStateName { get; set; }
        public string CollegiateNumber { get; set; }
    }
}
