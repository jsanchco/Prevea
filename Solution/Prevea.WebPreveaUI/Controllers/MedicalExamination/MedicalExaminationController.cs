namespace Prevea.WebPreveaUI.Controllers.MedicalExamination
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using Model.ViewModel;

    #endregion

    public class MedicalExaminationController : BaseController
    {
        #region Constructor

        public MedicalExaminationController(IService.IService.IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult DetailMedicalExamination(int medicalExaminationId)
        {
            var medicalExamination = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            var data = AutoMapper.Mapper.Map<RequestMedicalExaminationEmployeeViewModel>(medicalExamination);

            ViewBag.SelectTabId = 0;

            return PartialView("~/Views/MedicalExamination/DetailMedicalExamination.cshtml", data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult TemplateMedicalExamination(int medicalExaminationId)
        {
            var medicalExamination = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            var data = AutoMapper.Mapper.Map<RequestMedicalExaminationEmployeeViewModel>(medicalExamination);

            return PartialView("~/Views/MedicalExamination/TemplateMedicalExamination.cshtml", data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult DocumentsMedicalExamination(int medicalExaminationId)
        {
            var medicalExamination = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            var data = AutoMapper.Mapper.Map<RequestMedicalExaminationEmployeeViewModel>(medicalExamination);

            return PartialView("~/Views/MedicalExamination/DocumentsMedicalExamination.cshtml", data);
        }
    }
}
