namespace Prevea.WebPreveaUI.Controllers
{
    #region Using

    using HelpersClass;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Kendo.Mvc.UI;
    using IService.IService;
    using Model.Model;
    using Model.ViewModel;
    using Common;

    #endregion

    public class DoctorsController : BaseController
    {
        #region Constructor

        public DoctorsController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal")]
        public ActionResult Doctors()
        {
            return PartialView("~/Views/Doctors/Doctors.cshtml");
        }

        [HttpGet]
        [AppAuthorize(Roles = "Super,Admin,PreveaPersonal")]
        public JsonResult Doctors_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<UserViewModel>>(Service.GetUsersInRoles(new List<string> { "Doctor" }));

            return this.Jsonp(data);
        }

        public JsonResult Doctor_Update()
        {
            try
            {
                var doctor = this.DeserializeObject<UserViewModel>("doctor");
                if (doctor == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Médico" });
                }

                var data = AutoMapper.Mapper.Map<User>(doctor);
                var result = Service.SaveUser((int)EnRole.Doctor, data);

                return result.Status != Status.Error ? this.Jsonp(doctor) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Médico" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Médico" });
            }
        }

        public ActionResult Doctor_Destroy()
        {
            try
            {
                var doctor = this.DeserializeObject<User>("doctor");
                if (doctor == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Médico" });
                }

                var result = Service.DeleteUser(doctor.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Médico" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<UserViewModel>(doctor));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Médico" });
            }
        }

        public ActionResult Doctor_Create()
        {
            try
            {
                var doctor = this.DeserializeObject<User>("doctor");
                if (doctor == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Médico" });
                }

                var result = Service.SaveUser((int)EnRole.Doctor, AutoMapper.Mapper.Map<User>(doctor));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<UserViewModel>(result.Object));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Médico" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Médico" });
            }
        }
    }
}