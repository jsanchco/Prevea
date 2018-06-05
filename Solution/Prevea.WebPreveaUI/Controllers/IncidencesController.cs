using Prevea.Model.CustomModel;

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

    #endregion

    public class IncidencesController : BaseController
    {
        #region Constructor

        public IncidencesController(IService service) : base(service)
        {
        }

        #endregion

        // GET: Incidences
        public ActionResult Incidences()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult Incidences_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<IncidenceViewModel>>(Service.GetIncidencesByUserId(User.Id));

            return this.Jsonp(data);
        }

        public JsonResult Incidences_Update()
        {
            try
            {
                var incidence = this.DeserializeObject<Incidence>("incidence");
                if (incidence == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Incidencia" });
                }

                incidence.EndDate = incidence.IncidenceStateId == 3 ? (DateTime?) DateTime.Now : null;

                var result = Service.SaveIncidence(incidence);

                return result.Status != Status.Error ? 
                    this.Jsonp(AutoMapper.Mapper.Map<IncidenceViewModel>(Service.GetIncidenceById(((Incidence)result.Object).Id))) : 
                    this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Incidencia" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Incidencia" });
            }
        }

        public ActionResult Incidences_Destroy()
        {
            try
            {
                var incidence = this.DeserializeObject<Incidence>("incidence");
                if (incidence == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Incidencia" });
                }

                var result = Service.DeleteIncidence(incidence.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Incidencia" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<IncidenceViewModel>(incidence));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado de la Incidencia" });
            }
        }

        public ActionResult Incidences_Create()
        {
            try
            {
                var incidence = this.DeserializeObject<IncidenceViewModel>("incidence");
                if (incidence == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Incidencia" });
                }

                var result = Service.SaveIncidence(AutoMapper.Mapper.Map<Incidence>(incidence));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<IncidenceViewModel>(Service.GetIncidenceById(((Incidence)result.Object).Id)));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Incidencia" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación de la Incidencia" });
            }
        }

        [HttpGet]
        public JsonResult GetIncidenceStates([DataSourceRequest] DataSourceRequest request)
        {
            var incidenceStates = Service.GetIncidenceStates();
            var data = new List<GenericModelDropDown>();
            foreach (var incidenceState in incidenceStates)
            {
                data.Add(new GenericModelDropDown
                {
                    Id = incidenceState.Id,
                    Name = incidenceState.Description
                });
            }
            return this.Jsonp(data);
        }

        [HttpGet]
        public JsonResult GetCriticalNivels([DataSourceRequest] DataSourceRequest request)
        {
            var criticalNivels = Service.GetCriticalNivels();
            var data = new List<GenericModelDropDown>();
            foreach (var criticalNivel in criticalNivels)
            {
                data.Add(new GenericModelDropDown
                {
                    Id = criticalNivel.Id,
                    Name = criticalNivel.Description
                });
            }
            return this.Jsonp(data);
        }
    }
}