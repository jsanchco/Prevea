namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using IService.IService;

    #endregion

    public class DashboardController : BaseController
    {
        #region Constructor

        public DashboardController(IService service) : base(service)
        {

        }

        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,Library,Basic")]
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}