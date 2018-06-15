namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using IRepository.IRepository;

    #endregion

    public class DashboardController : BaseController
    {
        #region Constructor

        public DashboardController(IRepository repository) : base(repository)
        {

        }

        #endregion

        [HttpGet]        
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}