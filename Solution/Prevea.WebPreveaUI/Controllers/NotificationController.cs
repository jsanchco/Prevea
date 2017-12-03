namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Model.ViewModel;
    using Common;

    #endregion

    public class NotificationController : BaseController
    {
        #region Constructor

        public NotificationController(IService.IService.IService service) : base(service)
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
            var data = AutoMapper.Mapper.Map<List<NotificationViewModel>>(Service.GetNotifications());

            return this.Jsonp(data);
        }
    }
}