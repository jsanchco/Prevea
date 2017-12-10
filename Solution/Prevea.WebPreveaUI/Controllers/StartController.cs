﻿namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Linq;
    using IService.IService;

    #endregion

    public class StartController : BaseController
    {
        #region Constructor

        public StartController(IService service) : base(service)
        {

        }

        #endregion

        [HttpGet]
        public ActionResult Index(int userId)
        {
            var user = Service.GetUser(userId);
            if (user == null)
                return RedirectToAction("Index", "Login");

            var userRole = user.UserRoles.FirstOrDefault();
            if (userRole == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Notifications = 2;
            ViewBag.UserId = user.Id;
            ViewBag.UserRoleId = userRole.Role.Id;
            ViewBag.UserRoleName = userRole.Role.Name;
            ViewBag.UserRoleDescription = userRole.Role.Description;

            return View(user);
        }
    }
}