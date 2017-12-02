namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using IRepository.IRepository;
    using HelpersClass;
    using IService.IService;

    #endregion

    public class ErrorController : BaseController
    {
        #region Constructor

        public ErrorController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult AccessDenied()
        {
            ViewBag.PageError = "AccessDenied";

            return PartialView();
        }

        [HttpGet]
        public ActionResult UserNoRegistred()
        {
            ViewBag.PageError = "UserNoRegistred";

            return PartialView();
        }
    }
}