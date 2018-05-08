namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using HelpersClass;
    using System.Web.Mvc;

    #endregion

    public class EmployeesController : BaseController
    {
        #region Constructor

        public EmployeesController(IService.IService.IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult Documents()
        {
            return PartialView("~/Views/Employees/EmployeeDocuments.cshtml");
        }
    }
}
