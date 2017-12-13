﻿namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Model.ViewModel;
    using Common;
    using System;

    #endregion

    public class NotificationsController : BaseController
    {
        #region Constructor

        public NotificationsController(IService.IService.IService service) : base(service)
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
    }
}