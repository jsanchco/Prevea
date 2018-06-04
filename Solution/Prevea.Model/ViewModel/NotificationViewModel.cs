namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class NotificationViewModel
    {
        public int Id { get; set; }

        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public string NotificationTypeDescription { get; set; }
        public int NotificationStateId { get; set; }
        public string NotificationStateName { get; set; }
        public string NotificationStateDescription { get; set; }
        public int? SimulationId { get; set; }
        public string SimulationName { get; set; }
        public string Observations { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public bool Read { get; set; }
        public string ToUserInitials { get; set; }
        public string ToRolName { get; set; }
        public int? SimulationAssignedTo { get; set; }
    }
}
