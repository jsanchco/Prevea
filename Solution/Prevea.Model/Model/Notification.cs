namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class Notification
    {
        [Key, Required]
        public int Id { get; set; }

        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }
    }
}
