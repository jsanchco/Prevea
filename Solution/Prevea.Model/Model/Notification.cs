namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class Notification
    {
        [Key, Required]
        public int Id { get; set; }

        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }

        public int NotificationStateId { get; set; }
        public virtual NotificationState NotificationState { get; set; }

        public int? SimulationId { get; set; }
        public virtual Simulation Simulation { get; set; }

        public string Observations { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public bool Read { get; set; }

        [NotMapped]
        public User FromUser
        {
            get
            {
                if (Simulation != null)
                {
                    return Simulation.User;
                }

                return null;
            }
        }

        public int? ToUserId { get; set; }
        public virtual User ToUser { get; set; }

        public int? ToRoleId { get; set; }
        public virtual Role ToRole { get; set; }
    }
}
