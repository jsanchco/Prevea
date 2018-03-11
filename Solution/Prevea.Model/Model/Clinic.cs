namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using Model;
    using System.Collections.Generic;

    #endregion

    public class Clinic
    {
        [Key, Required]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }

        public string Description { get; set; }

        public virtual ICollection<RequestMedicalExaminationEmployee> RequestMedicalExaminationsEmployees { get; set; }
    }
}
