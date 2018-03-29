using System.Collections.Generic;
using System.Linq;

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
    using Common;
    using Rotativa.MVC;
    using IService.IService;

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
            requestMedicalExaminationEmployee.MedicalExamination.MedicalExaminationJSON = Service.GenerateMedicalExaminationJSON(requestMedicalExaminationEmployee);

            var data = GetOptimizedRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);

            return PartialView("~/Views/MedicalExamination/TemplateMedicalExamination.cshtml", data);
        }

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult TemplateMedicalExaminationReport(int medicalExaminationId)
        {
            var requestMedicalExaminationEmployee = Service.GetRequestMedicalExaminationEmployeeById(medicalExaminationId);
            requestMedicalExaminationEmployee.MedicalExamination.MedicalExaminationJSON = Service.GenerateMedicalExaminationJSON(requestMedicalExaminationEmployee);

            var data = GetOptimizedRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);

            return View("~/Views/MedicalExamination/TemplateMedicalExaminationReport.cshtml", data);
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
                    MedicalExaminationJSON = medicalExaminationEmployee.MedicalExamination.MedicalExaminationJSON,
                    BeginDate = medicalExaminationEmployee.MedicalExamination.BeginDate,
                    EndDate = medicalExaminationEmployee.MedicalExamination.EndDate,
                    MedicalExaminationStateId = medicalExaminationEmployee.MedicalExamination.MedicalExaminationStateId,
                    Enrollment = medicalExaminationEmployee.MedicalExamination.Enrollment
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
                        PhoneNumber = medicalExaminationEmployee.Employee.User.PhoneNumber
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

            requestMedicalExaminationEmployeeFind.MedicalExamination.MedicalExaminationJSON =
                requestMedicalExaminationEmployee.MedicalExamination.MedicalExaminationJSON;

            var result = Service.SaveMedicalExamination(requestMedicalExaminationEmployeeFind.MedicalExamination);

            return Json(result.Status == Status.Ok ? 
                new { resultStatus = Status.Ok } : 
                new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult SaveMedicalExamination(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        //{
        //    return Json(CreatePdf(requestMedicalExaminationEmployee) ? new { resultStatus = Status.Ok } : new { resultStatus = Status.Error }, JsonRequestBehavior.AllowGet);
        //}

        private bool CreatePdf(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        {
            try
            {
                //var physicalPath = Server.MapPath(directory);
                //var exits = Directory.Exists(physicalPath);
                //if (!exits && physicalPath != null)
                //    Directory.CreateDirectory(physicalPath);

                var filePath = Server.MapPath("~/App_Data/Companies/44445555R/MedicalExaminations/1.pdf");

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
    }
}
