﻿namespace Prevea.WebPreveaUI.Controllers.MedicalExamination
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Common;
    using Model.Model;
    using System;
    using Model.ViewModel;
    using System.Diagnostics;
    using IService.IService;

    #endregion

    public class DoctorWorkSheetController : BaseController
    {
        #region Constructor

        public DoctorWorkSheetController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Doctor")]
        public ActionResult DoctorWorkSheet()
        {
            return PartialView("~/Views/MedicalExamination/DoctorWorkSheet.cshtml");
        }

        [HttpGet]
        public JsonResult DoctorWorkSheet_Read([DataSourceRequest] DataSourceRequest request)
        {
            var dates = Service.GetDatesByWorkSheet(User.Id);
            var doctorWorkSheets = new List<DoctorWorkSheet>();
            foreach (var date in dates)
            {
                if (date == null)
                    continue;

                doctorWorkSheets.Add(new DoctorWorkSheet
                {
                    Date = (DateTime)date
                });
            }

            return this.Jsonp(doctorWorkSheets);
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminationEmployees_Read([DataSourceRequest] DataSourceRequest request, string dateString)
        {
            try
            {
                var splitDateString = dateString.Split('/');
                var date = new DateTime(Convert.ToInt16(splitDateString[0]), Convert.ToInt16(splitDateString[1]), Convert.ToInt16(splitDateString[2]));
                var requestMedicalExaminationEmployees = Service.GetRequestMedicalExaminationEmployeesByDate(User.Id, date);

                var data = AutoMapper.Mapper.Map<List<RequestMedicalExaminationEmployeeViewModel>>(requestMedicalExaminationEmployees);
                return this.Jsonp(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.Jsonp(null);
            }
        }

        [HttpGet]
        public JsonResult RequestMedicalExaminationEmployees_Update()
        {
            const string errorRequestMedicalExaminationEmployee = "Se ha producido un error en la Actualización de RequestMedicalExaminationEmployee";

            try
            {
                var requestMedicalExamination = this.DeserializeObject<RequestMedicalExaminationEmployee>("medicalExamination");
                if (requestMedicalExamination == null)
                    return this.Jsonp(new { Errors = errorRequestMedicalExaminationEmployee });

                var requestMedicalExaminationEmployeeFind =
                    Service.GetRequestMedicalExaminationEmployeeById(requestMedicalExamination.Id);
                if (requestMedicalExaminationEmployeeFind == null)
                    return this.Jsonp(new { Errors = errorRequestMedicalExaminationEmployee });

                requestMedicalExaminationEmployeeFind.SamplerNumber = requestMedicalExamination.SamplerNumber;
                requestMedicalExaminationEmployeeFind.Observations = requestMedicalExamination.Observations;

                var result = Service.SaveRequestMedicalExaminationEmployee(requestMedicalExaminationEmployeeFind);
                if (result.Status == Status.Error)
                    return this.Jsonp(new { Errors = errorRequestMedicalExaminationEmployee });

                return this.Jsonp(AutoMapper.Mapper.Map<RequestMedicalExaminationEmployeeViewModel>(result.Object));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = errorRequestMedicalExaminationEmployee });
            }
        }
    }
};