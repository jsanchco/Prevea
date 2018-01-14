namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;

    #endregion

    public class DoctorWorkSheet
    {
        [Key, Required]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int DoctorId { get; set; }
        public virtual User Doctor { get; set; }

        public virtual ICollection<EmployeeDoctorWorkSheet> EmployeesDoctorWorkSheets { get; set; }
    }
}
