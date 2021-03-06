﻿namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Linq;
    using IRepository.IRepository;

    #endregion

    public class StartController : BaseController
    {
        #region Constructor

        public StartController(IRepository repository) : base(repository)
        {

        }

        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal,PreveaCommercial,ContactPerson,Employee,Doctor")]
        public ActionResult Index(int userId)
        {
            var user = Service.GetUser(userId);
            if (user == null)
                return RedirectToAction("Index", "Login");

            var userRole = user.UserRoles.FirstOrDefault();
            if (userRole == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Notifications = Service.GetNumberNotificationsByUserId(userId);
            ViewBag.UserId = user.Id;
            ViewBag.UserInitials = user.Initials;
            ViewBag.UserRoleId = userRole.Role.Id;
            ViewBag.UserRoleName = userRole.Role.Name;
            ViewBag.UserRoleDescription = userRole.Role.Description;

            return View(user);
        }
    }
}