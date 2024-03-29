﻿namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;

    #endregion

    public class RequestMedicalExaminations
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public string Observations { get; set; }

        public int RequestMedicalExaminationStateId { get; set; }
        public virtual RequestMedicalExaminationState RequestMedicalExaminationState { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<RequestMedicalExaminationEmployee> RequestMedicalExaminationEmployees { get; set; }
    }
}
