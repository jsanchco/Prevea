namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using IRepository.IRepository;
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

            ViewBag.HasKendo = true;

            return View(user);
        }
    }
}