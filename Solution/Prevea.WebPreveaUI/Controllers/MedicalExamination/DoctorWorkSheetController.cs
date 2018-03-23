using System;

namespace Prevea.WebPreveaUI.Controllers.MedicalExamination
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System.Collections.Generic;
    using Kendo.Mvc.UI;
    using Common;
    using Model.Model;

    #endregion

    public class DoctorWorkSheetController : BaseController
    {
        #region Constructor

        public DoctorWorkSheetController(IService.IService.IService service) : base(service)
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
    }
};