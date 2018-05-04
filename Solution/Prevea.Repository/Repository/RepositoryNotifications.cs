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
                .Include(x => x.ToUser)
                .Include(x => x.ToRole)
                .OrderByDescending(x => x.DateCreation)
                .ToList();
        }

        public Notification GetNotification(int id)
        {
            return Context.Notifications
                .Include(x => x.NotificationType)
                .Include(x => x.ToUser)
                .Include(x => x.ToRole)
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
                    return false;
                }
            }
        }

        public List<Notification> GetNotificationsByUserId(int userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);

            var userRole = user?.UserRoles.FirstOrDefault();
            if (userRole == null)
                return null;
            switch (userRole.RoleId)
            {
                case (int) EnRole.Super:
                    return Context.Notifications
                        .Include(x => x.NotificationType)
                        .Include(x => x.ToUser)
                        .Include(x => x.ToRole)
                        .OrderByDescending(x => x.DateCreation)
                        .ToList();
                case (int) EnRole.PreveaPersonal:
                    return Context.Notifications
                        .Include(x => x.NotificationType)
                        .Include(x => x.ToUser)
                        .Include(x => x.ToRole)
                        .OrderByDescending(x => x.DateCreation)
                        .Where(x => x.ToRoleId == (int) EnRole.PreveaPersonal || x.ToUserId == userId)
                        .ToList();
                case (int) EnRole.PreveaCommercial:
                    return Context.Notifications
                        .Include(x => x.NotificationType)
                        .Include(x => x.ToUser)
                        .Include(x => x.ToRole)
                        .OrderByDescending(x => x.DateCreation)
                        .Where(x => x.ToUserId == userId)
                        .ToList();
                case (int)EnRole.ContactPerson:
                    return Context.Notifications
                        .Include(x => x.NotificationType)
                        .Include(x => x.ToUser)
                        .Include(x => x.ToRole)
                        .OrderByDescending(x => x.DateCreation)
                        .Where(x => x.ToUserId == userId)
                        .ToList();
                default:
                    return Context.Notifications
                        .Include(x => x.NotificationType)
                        .Include(x => x.ToUser)
                        .Include(x => x.ToRole)
                        .OrderByDescending(x => x.DateCreation)
                        .ToList();
            }
        }
        public List<Notification> GetNotificationsByRoleId(int roleId)
        {
            return Context.Notifications
                .Include(x => x.NotificationType)
                .Include(x => x.ToUser)
                .Include(x => x.ToRole)
                .OrderByDescending(x => x.DateCreation)
                .Where(x => x.ToRoleId == roleId)
                .ToList();
        }

        public int GetNumberNotificationsByUserId(int userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);

            var userRole = user?.UserRoles.FirstOrDefault();
            if (userRole == null)
                return 0;

            if (userRole.RoleId == (int)EnRole.Super)
                return Context.Notifications.Count();

            return Context.Notifications.Count(x => x.ToUserId == userId || x.ToRoleId == userRole.RoleId);
        }
    }
}
