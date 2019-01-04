namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class CorrectiveAction
    {
        [Key, Required]
        public int Id { get; set; }
        public string Description { get; set; }
        public int PriorityCorrectiveActionId { get; set; }
        public virtual PriorityCorrectiveAction PriorityCorrectiveAction { get; set; }
    }
}
