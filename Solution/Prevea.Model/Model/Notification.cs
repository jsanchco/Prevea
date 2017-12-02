namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;

    #endregion

    public class Notification
    {
        [Key, Required]
        public int Id { get; set; }

        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }

        public string Observations { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
    }
}
