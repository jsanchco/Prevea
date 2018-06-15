namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using System.Web.Mvc;
    using HelpersClass;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using Model.ViewModel;
    using Common;
    using IRepository.IRepository;

    #endregion

    public class ClinicsController : BaseController
    {
        #region Controller
        public ClinicsController(IRepository repository) : base(repository)
        {
        }
        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal")]
        public ActionResult Clinics()
        {
            return PartialView("~/Views/Clinics/Clinics.cshtml");
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal")]
        public JsonResult Clinics_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<ClinicViewModel>>(Service.GetClincs());

            return this.Jsonp(data);
        }

        public JsonResult Clinic_Update()
        {
            try
            {
                var clinic = this.DeserializeObject<Clinic>("clinic");
                if (clinic == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Clínica" });
                }

                var result = Service.SaveClinic(clinic);

                return result.Status != Status.Error ? this.Jsonp(clinic) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Clínica" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Clínica" });
            }
        }

        public ActionResult Clinic_Destroy()
        {
            try
            {
                var clinic = this.DeserializeObject<Clinic>("clinic");
                if (clinic == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Clínica" });
                }

                var result = Service.DeleteClinic(clinic.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Clínica" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<ClinicViewModel>(clinic));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Clínica" });
            }
        }

        public ActionResult Clinic_Create()
        {
            try
            {
                var clinic = this.DeserializeObject<ClinicViewModel>("clinic");
                if (clinic == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Clínica" });
                }

                var result = Service.SaveClinic(AutoMapper.Mapper.Map<Clinic>(clinic));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<ClinicViewModel>(result.Object));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Clínica" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Clínica" });
            }
        }
    }
}