namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class NotificationState
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnNotificationState), Id));

        public virtual ICollection<Notification> Notifications { get; set; }
    }

    public enum EnNotificationState { NotMapped, Issued, Assigned, ReAssigned, Validated }
}
