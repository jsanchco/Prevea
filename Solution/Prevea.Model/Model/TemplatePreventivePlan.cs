namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;

    #endregion

    public class TemplatePreventivePlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Template { get; set; }

        public virtual ICollection<PreventivePlanTemplatePreventivePlan> PreventivePlanTemplatePreventivePlans { get; set; }
    }
}
