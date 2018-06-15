namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Model.ViewModel;
    using Common;
    using System;
    using Prevea.IService.IService;
    using IRepository.IRepository;

    #endregion

    public class NotificationsController : BaseController
    {
        #region Constructor

        public NotificationsController(IRepository repository) : base(repository)
        {
        }

        #endregion

        public ActionResult Notifications()
        {
            return PartialView();
        }

        [HttpGet]
        //[AppAuthorize(Roles = "Super,Admin")]
        public JsonResult Notifications_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<NotificationViewModel>>(Service.GetNotificationsByUserId(User.Id));

            return this.Jsonp(data);
        }

        [HttpPost]
        public JsonResult AssignNotification(int notificationId)
        {
            var notification = Service.GetNotification(notificationId);
            notification.DateModification = DateTime.Now;
            notification.ToUserId = User.Id;

            var result = Service.SaveNotification(notification);

            result.Object = AutoMapper.Mapper.Map<NotificationViewModel>(result.Object);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadNotification(int id, bool read)
        {
            var notification = Service.GetNotification(id);
            if (notification == null)
            {
                return Json(new { resultStatus = Status.Error });
            }

            notification.Read = read;
            var result = Service.SaveNotification(notification);

            result.Object = AutoMapper.Mapper.Map<NotificationViewModel>(result.Object);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}