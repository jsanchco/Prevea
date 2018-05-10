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

    public class TecniquesController : BaseController
    {
        #region Constructor

        public TecniquesController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult WorkStations()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult DeltaCodes()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult HistoricTecniques()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult Sectors_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<SectorViewModel>>(Service.GetSectors());

            return this.Jsonp(data);
        }

        public ActionResult Sectors_Create()
        {
            try
            {
                var sector = this.DeserializeObject<SectorViewModel>("sector");
                if (sector == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Sector" });
                }

                var result = Service.SaveSector(AutoMapper.Mapper.Map<Sector>(sector));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<SectorViewModel>(result.Object));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Sector" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Sector" });
            }
        }

        public JsonResult Sectors_Update()
        {
            try
            {
                var sector = this.DeserializeObject<Sector>("sector");
                if (sector == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Sector" });
                }

                var result = Service.SaveSector(sector);

                return result.Status != Status.Error ? this.Jsonp(AutoMapper.Mapper.Map<SectorViewModel>(sector)) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Sector" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del Sector" });
            }
        }

        public ActionResult Sectors_Destroy()
        {
            try
            {
                var sector = this.DeserializeObject<Sector>("sector");
                if (sector == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Sector" });
                }

                var result = Service.DeleteSector(sector.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Sector" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<SectorViewModel>(sector));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del Sector" });
            }
        }

        [HttpGet]
        public JsonResult WorkStations_Read([DataSourceRequest] DataSourceRequest request, int sectorId)
        {
            var data = AutoMapper.Mapper.Map<List<WorkStationViewModel>>(Service.GetWorkStationsBySectorId(sectorId));

            return this.Jsonp(data);
        }

        public ActionResult WorkStations_Create()
        {
            try
            {
                var workStation = this.DeserializeObject<WorkStationViewModel>("workStation");
                if (workStation == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del WorkStation" });
                }

                var result = Service.SaveWorkStation(AutoMapper.Mapper.Map<WorkStation>(workStation));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<WorkStationViewModel>(result.Object));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del WorkStation" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del WorkStation" });
            }
        }

        public JsonResult WorkStations_Update()
        {
            try
            {
                var workStation = this.DeserializeObject<WorkStation>("workStation");
                if (workStation == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del WorkStation" });
                }

                var result = Service.SaveWorkStation(workStation);

                return result.Status != Status.Error ? this.Jsonp(AutoMapper.Mapper.Map<WorkStationViewModel>(workStation)) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del WorkStation" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del WorkStation" });
            }
        }

        public ActionResult WorkStations_Destroy()
        {
            try
            {
                var workStation = this.DeserializeObject<WorkStation>("workStation");
                if (workStation == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del WorkStation" });
                }

                var result = Service.DeleteWorkStation(workStation.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del WorkStation" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<WorkStationViewModel>(workStation));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del WorkStation" });
            }
        }

        [HttpGet]
        public JsonResult DeltaCodes_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<DeltaCodeViewModel>>(Service.GetDeltaCodes());

            return this.Jsonp(data);
        }

        public ActionResult DeltaCodes_Create()
        {
            try
            {
                var deltaCode = this.DeserializeObject<DeltaCodeViewModel>("deltaCode");
                if (deltaCode == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DeltaCode" });
                }

                var result = Service.SaveDeltaCode(AutoMapper.Mapper.Map<DeltaCode>(deltaCode));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<DeltaCodeViewModel>(result.Object));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DeltaCode" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DeltaCode" });
            }
        }

        public JsonResult DeltaCodes_Update()
        {
            try
            {
                var deltaCode = this.DeserializeObject<DeltaCode>("deltaCode");
                if (deltaCode == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DeltaCode" });
                }

                var result = Service.SaveDeltaCode(deltaCode);

                return result.Status != Status.Error ? this.Jsonp(AutoMapper.Mapper.Map<DeltaCodeViewModel>(deltaCode)) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DeltaCode" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del DeltaCode" });
            }
        }

        public ActionResult DeltaCodes_Destroy()
        {
            try
            {
                var deltaCode = this.DeserializeObject<DeltaCode>("deltaCode");
                if (deltaCode == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del DeltaCode" });
                }

                var result = Service.DeleteDeltaCode(deltaCode.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del DeltaCode" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<DeltaCodeViewModel>(deltaCode));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del DeltaCode" });
            }
        }
    }
}