﻿namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    #endregion

    public class Employee
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<RequestMedicalExaminationEmployee> RequestMedicalExaminationEmployees { get; set; }
    }
}
