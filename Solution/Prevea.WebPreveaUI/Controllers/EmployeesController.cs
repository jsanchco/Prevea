namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using HelpersClass;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Common;
    using System.Linq;
    using Model.ViewModel;
    using IRepository.IRepository;

    #endregion

    public class EmployeesController : BaseController
    {
        #region Constructor

        public EmployeesController(IRepository repository) : base(repository)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult Documents()
        {
            return PartialView("~/Views/Employees/EmployeeDocuments.cshtml");
        }

        [HttpGet]
        public JsonResult HeaderEmployeeDocuments_Read([DataSourceRequest] DataSourceRequest request)
        {
            var user = Service.GetEmployeeByUser(User.Id);
            if (user == null)
            {
                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Médico" });
            }

            var data = Service.GetHeaderEmployeeDocuments(user.Id); 

            return this.Jsonp(data);
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminationDocuments_Read([DataSourceRequest] DataSourceRequest request, int requestMedicalExaminationEmployeeId)
        {
            var data = Service.GetRequestMedicalExaminationEmployeeById(requestMedicalExaminationEmployeeId);
            var documents = data.MedicalExaminationDocuments.Select(medicalExaminationDocument => medicalExaminationDocument.Document).ToList();
            return this.Jsonp(AutoMapper.Mapper.Map<List<DocumentViewModel>>(documents));
        }
    }
}
