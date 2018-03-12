namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class DoctorMedicalExaminationEmployee
    {
        [Key, Required]
        public int Id { get; set; }

        public int DoctorId { get; set; }
        public User Doctor { get; set; }

        public int MedicalExaminationEmployeeId { get; set; }
        public RequestMedicalExaminationEmployee MedicalExaminationEmployee { get; set; }
    }
}
