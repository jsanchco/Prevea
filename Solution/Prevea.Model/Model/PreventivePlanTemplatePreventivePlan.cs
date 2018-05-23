namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class PreventivePlanTemplatePreventivePlan
    {
        [Key]
        public int Id { get; set; }

        public int PreventivePlanId { get; set; }
        public PreventivePlan PreventivePlan { get; set; }

        public int TemplatePreventivePlanId { get; set; }
        public TemplatePreventivePlan TemplatePreventivePlan { get; set; }

        public string Text { get; set; }
    }
}
