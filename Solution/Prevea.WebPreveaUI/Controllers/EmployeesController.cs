using System.Linq;
using Prevea.Model.ViewModel;

namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using HelpersClass;
    using System.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Model.Model;
    using Common;

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
