namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public List<NotificationType> GetNotificationTypes()
        {
            return Context.NotificationTypes
                .Include(x => x.Notifications)
                .ToList();
        }

        public NotificationType GetNotificationType(int id)
        {
            return Context.NotificationTypes
                .Include(x => x.Notifications)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
