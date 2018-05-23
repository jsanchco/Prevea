namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    #endregion

    public class PreventivePlan
    {
        [Key]
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public virtual ICollection<PreventivePlanTemplatePreventivePlan> PreventivePlanTemplatePreventivePlans { get; set; }
    }
}
