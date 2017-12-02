namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    #endregion

    public class NotificationType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Enum.GetName(typeof(EnNotificationType), Id);

        public virtual ICollection<Notification> Notifications { get; set; }
    }

    public enum EnNotificationType { NotMapped, FromSimulator, FromFormation, FromSede, FromUser }
}
