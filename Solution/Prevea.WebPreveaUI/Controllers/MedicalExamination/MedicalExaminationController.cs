namespace Prevea.WebPreveaUI.Controllers.MedicalExamination
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using Model.ViewModel;
    using System;
    using System.Diagnostics;
    using System.IO;
    using Model.Model;
    using System.Collections.Generic;
    using Rotativa.MVC;
    using IService.IService;
    using Newtonsoft.Json;
    using System.Linq;
    using Kendo.Mvc.UI;
    using Common;
    using System.Web;

    #endregion

    public class MedicalExaminationController : BaseController
    {
        #region Constructor

        public MedicalExaminationController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult DetailMedicalExamination(int medicalExaminationId)
        {
            var medicalExamination = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            var data = AutoMapper.Mapper.Map<RequestMedicalExaminationEmployeeViewModel>(medicalExamination);

            var medicalExaminationDocument = medicalExamination.MedicalExaminationDocuments.FirstOrDefault(x => x.Document.AreaId == 16);
            if (medicalExaminationDocument != null)
            {
                data.MedicalExaminationStateId = medicalExaminationDocument.Document.DocumentStateId;
                data.MedicalExaminationStateDescription = medicalExaminationDocument.Document.DocumentState.Description;
            }
            else
            {
                data.MedicalExaminationStateId = (int) EnDocumentState.Pending;
                data.MedicalExaminationStateDescription = "Pendiente";
            }

            ViewBag.SelectTabId = 0;

            return PartialView("~/Views/MedicalExamination/DetailMedicalExamination.cshtml", data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult TemplateMedicalExamination(int requestMedicalExaminationEmployeeId)
        {
            return PartialView("~/Views/MedicalExamination/TemplateMedicalExamination.cshtml", Service.GetTemplateMedicalExaminationViewModel(requestMedicalExaminationEmployeeId));
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult TemplateMedicalExaminationReport(int documentId)
        {
            var requestMedicalExaminationEmployee =
                Service.GetRequestMedicalExaminationEmployeesByDocumentId(documentId);
            var data = Service.GetTemplateMedicalExaminationViewModel(requestMedicalExaminationEmployee.Id);
            data.DocumentInputTemplates = JsonConvert.DeserializeObject<List<InputTemplate>>(data.DocumentInputTemplateJSON);

            #region IMC
            decimal height = 0;
            var inme19 = data.DocumentInputTemplates.FirstOrDefault(x => x.Name == "me-19");
            if (inme19 != null)
            {
                height = Convert.ToDecimal(!string.IsNullOrEmpty(inme19.Text) ? inme19.Text.Replace(".", ",") : inme19.DefaultText.Replace(".", ","));
            }

            decimal weight = 0;
            var inme20 = data.DocumentInputTemplates.FirstOrDefault(x => x.Name == "me-20");
            if (inme20 != null)
            {
                weight = Convert.ToDecimal(!string.IsNullOrEmpty(inme20.Text) ? inme20.Text.Replace(".", ",") : inme20.DefaultText.Replace(".", ","));
            }

            var imc = Math.Round(weight / (height * height), 2);
            ViewBag.IMC = imc;
            #endregion

            #region FVC            
            decimal value;
            var percentage = 0;
            int constant;
            var inme49 =
                data.DocumentInputTemplates.FirstOrDefault(x => x.Name == "me-49");
            if (inme49 != null)
            {
                value = Convert.ToDecimal(!string.IsNullOrEmpty(inme49.Text) ? inme49.Text.Replace(".", ",") : inme49.DefaultText.Replace(".", ","));
                constant = 5000;
                percentage = Convert.ToInt32(value * 100 / constant);
            }
            ViewBag.FVC = percentage;
            #endregion

            #region FEV1
            percentage = 0;
            var inme50 = data.DocumentInputTemplates.FirstOrDefault(x => x.Name == "me-50");
            if (inme50 != null)
            {
                value = Convert.ToDecimal(!string.IsNullOrEmpty(inme50.Text) ? inme50.Text.Replace(".", ",") : inme50.DefaultText.Replace(".", ","));
                constant = 4181;
                percentage = Convert.ToInt32(value * 100 / constant);
            }
            ViewBag.FEV1 = percentage;
            #endregion

            #region FEF
            percentage = 0;
            var inme52 = data.DocumentInputTemplates.FirstOrDefault(x => x.Name == "me-52");
            if (inme52 != null)
            {
                value = Convert.ToDecimal(!string.IsNullOrEmpty(inme52.Text) ? inme52.Text.Replace(".", ",") : inme52.DefaultText.Replace(".", ","));
                constant = 4691;
                percentage = Convert.ToInt32(value * 100 / constant);
            }
            ViewBag.FEF = percentage;
            #endregion

            return View("~/Views/MedicalExamination/TemplateMedicalExaminationReport.cshtml", data);
        }

        [HttpGet]
        public ActionResult TemplateEmployeeCitationReport(int documentId)
        {
            var requestMedicalExaminationEmployeeFind = Service.GetRequestMedicalExaminationEmployeeById(documentId);

            return View("~/Views/MedicalExamination/TemplateEmployeeCitationReport.cshtml", requestMedicalExaminationEmployeeFind);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult DocumentsMedicalExamination(int requestMedicalExaminationEmployeeId)
        {
            var medicalExamination = Service.GetRequestMedicalExaminationEmployeeById(requestMedicalExaminationEmployeeId);
            
            return PartialView("~/Views/MedicalExamination/DocumentsMedicalExamination.cshtml", medicalExamination);
        }


        public JsonResult SaveMedicalExamination(TemplateMedicalExaminationViewModel templateMedicalExaminationViewModel)
        {
            var result = Service.SaveMedicalExamination(templateMedicalExaminationViewModel, User.Id);
            if (result.Status == Status.Error)
            {
                return Json(
                    new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new { resultStatus = Status.Ok, documentState = (int)result.Object },
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult PrintMedicalExamination(TemplateMedicalExaminationViewModel templateMedicalExaminationViewModel)
        {
            var result = Service.SaveMedicalExamination(templateMedicalExaminationViewModel, User.Id);
            if (result.Status == Status.Error)
            {
                return Json(
                    new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            var requestMedicalExaminationEmployee =
                Service.GetRequestMedicalExaminationEmployeeById(templateMedicalExaminationViewModel
                    .RequestMedicalExaminationEmployeeId);
            if (requestMedicalExaminationEmployee == null)
            {
                return Json(
                    new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            var medicalExaminationDocument = requestMedicalExaminationEmployee.MedicalExaminationDocuments.FirstOrDefault(x => x.Document.AreaId == 16);
            if (medicalExaminationDocument?.Document == null)
            {
                return Json(
                    new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            var createPdf = CreatePdf(medicalExaminationDocument.Document, null);
            if (!createPdf)
            {
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new { resultStatus = Status.Ok, documentId = medicalExaminationDocument.Document.Id },
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult PrintEmployeeCitation(int id)
        {
            var createPdf = CreateEmployeeCitationReportPdf(id);
            if (string.IsNullOrEmpty(createPdf))
            {
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { resultStatus = Status.Ok, url = createPdf }, JsonRequestBehavior.AllowGet);
        }

        private string CreateEmployeeCitationReportPdf(int id)
        {
            try
            {
                var area = Service.GetAreaByName("CIT");
                var document = new Document
                {
                    Id = id,
                    AreaId = area.Id,
                    UrlRelative = $"{area.Url}CIT_{User.Id}.pdf"
                };

                var result = CreatePdf(document, null);

                return result ? document.UrlRelative : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return null;
            }
        }

        public JsonResult DocumentsMedicalExamination_Read([DataSourceRequest] DataSourceRequest request, int requestMedicalExaminationEmployeeId)
        {
            var data = AutoMapper.Mapper.Map<List<DocumentViewModel>>(Service.GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(
                requestMedicalExaminationEmployeeId).Select(x => x.Document));
            foreach (var documentVM in data)
            {
                documentVM.RequestMedicalExaminationEmployeeId = requestMedicalExaminationEmployeeId;
            }

            return this.Jsonp(data);
        }

        public ActionResult DocumentsMedicalExamination_Destroy()
        {
            try
            {
                var documentViewModel = this.DeserializeObject<DocumentViewModel>("medicalExaminationDocument");
                if (documentViewModel?.RequestMedicalExaminationEmployeeId == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
                }

                var result = Service.DeleteMedicalExaminationDocument(documentViewModel.Id, (int)documentViewModel.RequestMedicalExaminationEmployeeId);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
                }

                return this.Jsonp(new { Status = Status.Ok });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
            }
        }

        public ActionResult DocumentsMedicalExamination_Create()
        {
            try
            {
                var documentViewModel = this.DeserializeObject<DocumentViewModel>("medicalExaminationDocument");
                if (documentViewModel?.RequestMedicalExaminationEmployeeId == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
                }

                var document = AutoMapper.Mapper.Map<Document>(documentViewModel);
                var result = Service.SaveMedicalExaminationDocument(
                    (int)documentViewModel.RequestMedicalExaminationEmployeeId, 
                    document,
                    User.Id);

                if (result.Status != Status.Error)
                {
                    if (result.Object is Document)
                        return this.Jsonp(AutoMapper.Mapper.Map<DocumentViewModel>((Document)result.Object));

                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
            }
        }

        public JsonResult DocumentMedicalExaminationTypes_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<AreaViewModel>>(Service.GetMedicalExaminationDocumentTypes());

            return this.Jsonp(data);
        }

        [HttpGet]
        public ActionResult AddDocument(int documentId)
        {
            var document = Service.GetDocument(documentId);

            return PartialView("~/Views/MedicalExamination/AddDocument.cshtml", document);
        }

        [HttpPost]
        public ActionResult SaveMedicalExaminationDocument(IEnumerable<HttpPostedFileBase> fileDocument, int medicalExaminationDocumentId)
        {
            if (fileDocument == null || !fileDocument.Any())
                return Json(new Result { Status = Status.Error }, JsonRequestBehavior.AllowGet);

            var result =
                Service.SaveFileMedicalExaminationDocument(fileDocument.FirstOrDefault(), medicalExaminationDocumentId);

            if (result.Status == Status.Error)
            {
                return Json(new Result
                {
                    Status = Status.Error
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Result
            {
                Status = Status.Ok,
                Object = AutoMapper.Mapper.Map<DocumentViewModel>(result.Object)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
