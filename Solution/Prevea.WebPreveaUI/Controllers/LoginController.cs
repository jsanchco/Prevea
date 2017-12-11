using Prevea.Model.Model;

namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Web.Security;
    using ViewModels;
    using System;
    using System.Web;
    using Newtonsoft.Json;
    using Security;
    using IService.IService;

    #endregion

    public class LoginController : BaseController
    {
        #region Constructor

        public LoginController(IService service) : base(service)
        {
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CallbacksValidateUser(UserLogin infoUser)
        {
            if (ModelState.IsValid)
            {
                var user = Service.ValidateUser(infoUser.User, infoUser.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(infoUser.User, false);

                    var roles = Service.GetRolesByUser(user.Id);

                    var serializeModel = new AppPrincipalSerializeModel
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = roles
                    };

                    var userData = JsonConvert.SerializeObject(serializeModel);
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        user.Nick,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(15),
                        false,
                        userData);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    return Json(new { login_status = "success", urlSartPage = Url.Action("Index", "Start", new { userId = user.Id }) });
                }

                ModelState.AddModelError("", "Incorrect username and/or password");

                return Json(new { login_status = "invalid" });
            }

            return Json(new { login_status = "invalid" });
        }

        public ActionResult LogOff()
        {
            Session.Clear();

            FormsAuthentication.SignOut();
            return Json(new { status = "Ok", from = "LogOff" }, JsonRequestBehavior.AllowGet);
        }
    }
}