namespace Prevea.WebPreveaUI.Controllers.Courses
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;

    #endregion

    public class ManagementCoursesController : BaseController
    {
        #region Constructor
        public ManagementCoursesController(IService.IService.IService service) : base(service)
        {
        }
        #endregion

        public ActionResult ManagementCourses()
        {
            return PartialView("~/Views/Courses/ManagementCourses/ManagementCourses.cshtml");
        }
    }
}