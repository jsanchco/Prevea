namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        public List<NotificationType> GetNotificationTypes()
        {
            return Repository.GetNotificationTypes();
        }

        public NotificationType GetNotificationType(int notificationTypeId)
        {
            return Repository.GetNotificationType(notificationTypeId);
        }
    }
}
