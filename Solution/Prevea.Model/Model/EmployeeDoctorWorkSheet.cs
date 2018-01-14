namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class EmployeeDoctorWorkSheet
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        public int DoctorWorkSheetId { get; set; }
        public virtual DoctorWorkSheet DoctorWorkSheet { get; set; }
    }
}
