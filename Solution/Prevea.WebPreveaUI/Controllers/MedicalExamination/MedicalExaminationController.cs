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

            ViewBag.SelectTabId = 0;

            return PartialView("~/Views/MedicalExamination/DetailMedicalExamination.cshtml", data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult TemplateMedicalExamination(int medicalExaminationId)
        {
            var requestMedicalExaminationEmployee = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            //requestMedicalExaminationEmployee.MedicalExamination.InputTemplatesJSON = Service.GenerateMedicalExaminationInputTemplatesJSON(requestMedicalExaminationEmployee);

            var data = GetOptimizedRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);

            return PartialView("~/Views/MedicalExamination/TemplateMedicalExamination.cshtml", data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult TemplateMedicalExaminationReport(int medicalExaminationId)
        {
            var requestMedicalExaminationEmployee = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            requestMedicalExaminationEmployee.MedicalExamination.InputTemplates = JsonConvert.DeserializeObject<List<InputTemplate>>(requestMedicalExaminationEmployee.MedicalExamination.InputTemplatesJSON);
            //requestMedicalExaminationEmployee.MedicalExamination.InputTemplatesJSON = Service.GenerateMedicalExaminationInputTemplatesJSON(requestMedicalExaminationEmployee);

            var data = GetOptimizedRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);

            #region IMC
            decimal height = 0;
            var inme19 =
                requestMedicalExaminationEmployee.MedicalExamination.InputTemplates
                    .FirstOrDefault(x => x.Name == "me-19");
            if (inme19 != null)
            {
                height = Convert.ToDecimal(!string.IsNullOrEmpty(inme19.Text) ? inme19.Text.Replace(".", ",") : inme19.DefaultText.Replace(".", ","));
            }

            decimal weight = 0;
            var inme20 =
                requestMedicalExaminationEmployee.MedicalExamination.InputTemplates
                    .FirstOrDefault(x => x.Name == "me-20");
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
                requestMedicalExaminationEmployee.MedicalExamination.InputTemplates
                    .FirstOrDefault(x => x.Name == "me-49");
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
            var inme50 =
                requestMedicalExaminationEmployee.MedicalExamination.InputTemplates
                    .FirstOrDefault(x => x.Name == "me-50");
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
            var inme52 =
                requestMedicalExaminationEmployee.MedicalExamination.InputTemplates
                    .FirstOrDefault(x => x.Name == "me-52");
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
        public ActionResult TemplateEmployeeCitationReport(int id)
        {
            var requestMedicalExaminationEmployeeFind = Service.GetRequestMedicalExaminationEmployeeById(id);

            return View("~/Views/MedicalExamination/TemplateEmployeeCitationReport.cshtml", requestMedicalExaminationEmployeeFind);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult DocumentsMedicalExamination(int medicalExaminationId)
        {
            var medicalExamination = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            
            return PartialView("~/Views/MedicalExamination/DocumentsMedicalExamination.cshtml", medicalExamination);
        }

        private RequestMedicalExaminationEmployee GetOptimizedRequestMedicalExaminationEmployee(
            RequestMedicalExaminationEmployee medicalExaminationEmployee)
        {
            var data = new RequestMedicalExaminationEmployee
            {
                Id = medicalExaminationEmployee.Id,                 
                //EmployeeId = medicalExaminationEmployee.EmployeeId,
                //ClinicId = medicalExaminationEmployee.ClinicId,
                Clinic = new Clinic
                {
                    Name = medicalExaminationEmployee.Clinic.Name,
                    Address = medicalExaminationEmployee.Clinic.Address,
                    Province = medicalExaminationEmployee.Clinic.Province
                },
                MedicalExamination = new MedicalExamination
                {
                    Id = medicalExaminationEmployee.MedicalExamination.Id,
                    InputTemplatesJSON = medicalExaminationEmployee.MedicalExamination.InputTemplatesJSON,
                    BeginDate = medicalExaminationEmployee.MedicalExamination.BeginDate,
                    EndDate = medicalExaminationEmployee.MedicalExamination.EndDate,
                    MedicalExaminationStateId = medicalExaminationEmployee.MedicalExamination.MedicalExaminationStateId,
                    Enrollment = medicalExaminationEmployee.MedicalExamination.Enrollment,
                    InputTemplates = medicalExaminationEmployee.MedicalExamination.InputTemplates
                },
                Employee = new Employee
                {
                    //Id = medicalExaminationEmployee.Employee.Id,
                    //CompanyId = medicalExaminationEmployee.Employee.CompanyId,
                    User = new User
                    {
                        FirstName = medicalExaminationEmployee.Employee.User.FirstName,
                        LastName = medicalExaminationEmployee.Employee.User.LastName,
                        BirthDate = medicalExaminationEmployee.Employee.User.BirthDate,
                        DNI = medicalExaminationEmployee.Employee.User.DNI,
                        WorkStation = medicalExaminationEmployee.Employee.User.WorkStation,
                        PhoneNumber = medicalExaminationEmployee.Employee.User.PhoneNumber,
                        Address = medicalExaminationEmployee.Employee.User.Address,
                        Province = medicalExaminationEmployee.Employee.User.Province
                    },
                    Company = new Company
                    {
                        //Id = medicalExaminationEmployee.Employee.Company.Id,
                        Name = medicalExaminationEmployee.Employee.Company.Name,
                        NIF = medicalExaminationEmployee.Employee.Company.NIF
                    }                    
                },
                Date = medicalExaminationEmployee.Date,
                DoctorsMedicalExaminationEmployee = new List<DoctorMedicalExaminationEmployee>(),   
            };

            foreach (var doctor in medicalExaminationEmployee.DoctorsMedicalExaminationEmployee)
            {
                var doctorsMedicalExaminationEmployee = new DoctorMedicalExaminationEmployee
                {
                    Doctor = new User
                    {
                        FirstName = doctor.Doctor.FirstName,
                        LastName = doctor.Doctor.LastName
                    }
                };
                data.DoctorsMedicalExaminationEmployee.Add(doctorsMedicalExaminationEmployee);
            }

            return data;
        }

        public JsonResult SaveMedicalExamination(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        {
            var requestMedicalExaminationEmployeeFind = Service.GetRequestMedicalExaminationEmployeeById(requestMedicalExaminationEmployee.Id);
            if (requestMedicalExaminationEmployeeFind == null)
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);

            requestMedicalExaminationEmployeeFind.MedicalExamination.InputTemplatesJSON =
                requestMedicalExaminationEmployee.MedicalExamination.InputTemplatesJSON;

            var inputTemplates = JsonConvert.DeserializeObject<List<InputTemplate>>(requestMedicalExaminationEmployee.MedicalExamination.InputTemplatesJSON);
            var inputTemplateAptitude = inputTemplates.FirstOrDefault(x => x.Name == "me-71");
            if (inputTemplateAptitude != null)
            {
                if (inputTemplateAptitude.Value == 0 && string.IsNullOrEmpty(inputTemplateAptitude.Text))
                    requestMedicalExaminationEmployeeFind.MedicalExamination.MedicalExaminationStateId =
                        (int) EnMedicalExaminationState.InProcess;
                else
                    requestMedicalExaminationEmployeeFind.MedicalExamination.MedicalExaminationStateId =
                        (int)EnMedicalExaminationState.Finished;
            }

            var result = Service.SaveMedicalExamination(requestMedicalExaminationEmployeeFind.MedicalExamination);

            if (result.Status == Status.Ok)
            {
                return Json(
                    new {resultStatus = Status.Ok, medicalExaminationState = requestMedicalExaminationEmployeeFind.MedicalExamination.MedicalExaminationStateId },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(
                new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PrintMedicalExamination(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        {
            var requestMedicalExaminationEmployeeFind = Service.GetRequestMedicalExaminationEmployeeById(requestMedicalExaminationEmployee.Id);
            if (requestMedicalExaminationEmployeeFind == null)
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);

            requestMedicalExaminationEmployeeFind.MedicalExamination.InputTemplatesJSON =
                requestMedicalExaminationEmployee.MedicalExamination.InputTemplatesJSON;

            var result = Service.SaveMedicalExamination(requestMedicalExaminationEmployeeFind.MedicalExamination);
            if (result.Status == Status.Error)
            {
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            var createPdf = CreateMedicalExaminationReportPdf(requestMedicalExaminationEmployeeFind);
            if (!createPdf)
            {
                return Json(new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { resultStatus = Status.Ok, id = requestMedicalExaminationEmployeeFind.MedicalExamination.Id }, JsonRequestBehavior.AllowGet);
        }

        private bool CreateMedicalExaminationReportPdf(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        {
            try
            {
                var filePath = Server.MapPath(requestMedicalExaminationEmployee.MedicalExamination.Url);
                var directory = Path.GetDirectoryName(filePath);
                if (directory != null)
                    Directory.CreateDirectory(directory);

                var actionPdf = new ActionAsPdf(
                    "TemplateMedicalExaminationReport",
                    new { medicalExaminationId = requestMedicalExaminationEmployee.Id });
                //actionPdf.RotativaOptions.CustomSwitches = Constants.FooterPdf;

                var applicationPdfData = actionPdf.BuildPdf(ControllerContext);
                var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fileStream.Write(applicationPdfData, 0, applicationPdfData.Length);
                fileStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }
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
                var requestMedicalExaminationEmployeFind = Service.GetRequestMedicalExaminationEmployeeById(id);
                if (requestMedicalExaminationEmployeFind == null)
                    return null;

                var urlRelative = $"~/App_Data/Companies/{requestMedicalExaminationEmployeFind.RequestMedicalExaminations.Company.NIF}/MedicalExaminations/CIT_{requestMedicalExaminationEmployeFind.Employee.UserId}.pdf";
                var filePath = Server.MapPath(urlRelative);
                var directory = Path.GetDirectoryName(filePath);
                if (directory != null)
                    Directory.CreateDirectory(directory);

                var actionPdf = new ActionAsPdf(
                    "TemplateEmployeeCitationReport",
                    new
                    {
                        id
                    });
                //actionPdf.RotativaOptions.CustomSwitches = Constants.FooterPdf;

                var applicationPdfData = actionPdf.BuildPdf(ControllerContext);
                var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fileStream.Write(applicationPdfData, 0, applicationPdfData.Length);
                fileStream.Close();

                return urlRelative;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return null;
            }
        }

        public JsonResult DocumentsMedicalExamination_Read([DataSourceRequest] DataSourceRequest request, int requestMedicalExaminationEmployeeId)
        {
            var data = AutoMapper.Mapper.Map<List<MedicalExaminationDocumentsViewModel>>(Service.GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(requestMedicalExaminationEmployeeId));

            return this.Jsonp(data);
        }

        public ActionResult DocumentsMedicalExamination_Destroy()
        {
            try
            {
                var medicalExaminationDocument = this.DeserializeObject<MedicalExaminationDocumentsViewModel>("medicalExaminationDocument");
                if (medicalExaminationDocument == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
                }

                var result = Service.DeleteMedicalExaminationDocument(medicalExaminationDocument.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Documento" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<MedicalExaminationDocumentsViewModel>(medicalExaminationDocument));
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
                var medicalExaminationDocument = this.DeserializeObject<MedicalExaminationDocuments>("medicalExaminationDocument");
                if (medicalExaminationDocument == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Documento" });
                }

                var result = Service.SaveMedicalExaminationDocument(medicalExaminationDocument);

                if (result.Status != Status.Error)
                {
                    if (result.Object is MedicalExaminationDocuments)
                        return this.Jsonp(AutoMapper.Mapper.Map<MedicalExaminationDocumentsViewModel>(medicalExaminationDocument));

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
            var data = AutoMapper.Mapper.Map<List<MedicalExaminationDocumentTypeViewModel>>(Service.GetMedicalExaminationDocumentTypes());

            return this.Jsonp(data);
        }

        [HttpGet]
        public ActionResult AddDocument(int medicalExaminationDocumentId)
        {
            var medicalExaminationDocument = Service.GetMedicalExaminationDocumentById(medicalExaminationDocumentId);

            return PartialView("~/Views/MedicalExamination/AddDocument.cshtml", medicalExaminationDocument);
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
                Object = AutoMapper.Mapper.Map<MedicalExaminationDocumentsViewModel>(result.Object)
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
