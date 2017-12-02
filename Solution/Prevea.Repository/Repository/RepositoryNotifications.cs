using System.Runtime.InteropServices.WindowsRuntime;

namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<Notification> GetNotifications()
        {
            return Context.Notifications
                .Include(x => x.NotificationType)
                .ToList();
        }

        public Notification GetNotification(int id)
        {
            return Context.Notifications
                .Include(x => x.NotificationType)
                .FirstOrDefault(x => x.Id == id);
        }

        public Notification SaveNotification(Notification notification)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Notifications.AddOrUpdate(notification);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return notification;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Notification UpdateNotification(int id, Notification notification)
        {
            return SaveNotification(notification);
        }

        public bool DeleteNotification(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var notification = GetNotification(id);
                    Context.Notifications.Remove(notification);

                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return true;
                }
            }
        }

        public List<Notification> GetNotificationsByUserId(int userId)
        {
            throw new System.NotImplementedException();
        }

        public List<Notification> GetNotificationsByRoleId(int roleId)
        {
            throw new System.NotImplementedException();
        }
    }
}
