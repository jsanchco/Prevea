namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;

    #endregion

    public class CoursesController : BaseController
    {
        #region Constructor
        public CoursesController(IService.IService.IService service) : base(service)
        {
        }
        #endregion

        public ActionResult Courses()
        {
            return PartialView();
        }
    }
}