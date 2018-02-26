namespace Prevea.WebPreveaUI.Controllers.MedicalExamination
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using IService.IService;
    using System.Linq;

    #endregion

    public class HistoricMedicalExaminationController : BaseController
    {
        #region Constructor
        public HistoricMedicalExaminationController(IService service) : base(service)
        {
        }
        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "ContactPerson")]
        public ActionResult HistoricMedicalExamination()
        {
            var companies = Service.GetCompanies();
            foreach (var company in companies)
            {
                if (company.ContactPersons?.FirstOrDefault(x => x.UserId == User.Id) != null)
                {
                    ViewBag.CompanyId = company.Id;
                    return PartialView("~/Views/MedicalExamination/Historic/HistoricMedicalExamination.cshtml");
                }
            }

            return PartialView("~/Views/Error/AccessDenied.cshtml");
        }
    }
}