namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public List<Notification> GetNotifications()
        {
            return Repository.GetNotifications();
        }

        public Notification GetNotification(int notificationId)
        {
            return Repository.GetNotification(notificationId);
        }

        public Result SaveNotification(Notification notification)
        {
            try
            {
                notification = Repository.SaveNotification(notification);

                if (notification == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Noticicación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Notificación se ha producido con éxito",
                    Object = notification,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Notificación",
                    Object = notification,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateNotification(int notificationId, Notification notification)
        {
            try
            {
                notification = Repository.UpdateNotification(notificationId, notification);

                if (notification == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Notificación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Notificación se ha producido con éxito",
                    Object = notification,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Notificación",
                    Object = notification,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteNotification(int notificationId)
        {
            try
            {
                var result = Repository.DeleteNotification(notificationId);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Notificación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la Notificación se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la Notificación",
                    Object = null,
                    Status = Status.Error
                };
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
