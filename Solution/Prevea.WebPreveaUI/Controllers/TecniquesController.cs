using System.Linq;

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
    using Model.CustomModel;

    #endregion

    public class TecniquesController : BaseController
    {
        #region Constructor

        public TecniquesController(IService service) : base(service)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult WorkStations(int? sectorSelected)
        {
            ViewBag.SectorSelected = sectorSelected;

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
        public ActionResult RiskEvaluation(int sectorId, int workStationId)
        {
            ViewBag.SectorId = sectorId;
            ViewBag.WorkStationId = workStationId;

            var workStation = Service.GetWorkStationById(workStationId);
            ViewBag.WorkStation = workStation.Name;
            if (!string.IsNullOrEmpty(workStation.ProfessionalCategory))
                ViewBag.WorkStation = $"{ViewBag.WorkStation} ({workStation.ProfessionalCategory})";

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

        [HttpGet]
        public JsonResult RiskEvaluations_Read([DataSourceRequest] DataSourceRequest request, int sectorId, int workStationId)
        {
            var riskEvaluations = Service.GetRiskEvaluationsByWorkStation(workStationId);
            var data = new List<RiskEvaluationViewModel>();
            foreach (var riskEvaluation in riskEvaluations)
            {
                data.Add(UpdateRiskValues(riskEvaluation));
            }
 
            return this.Jsonp(data);
        }

        public ActionResult RiskEvaluations_Create()
        {
            try
            {
                var riskEvaluation = this.DeserializeObject<RiskEvaluationViewModel>("riskEvaluation");
                if (riskEvaluation == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
                }

                var workStation = Service.GetWorkStationById(riskEvaluation.WorkStationId);
                if (workStation.RiskEvaluations.FirstOrDefault(x => x.DeltaCodeId == riskEvaluation.DeltaCodeId) != null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
                }

                var result = Service.SaveRiskEvaluation(AutoMapper.Mapper.Map<RiskEvaluation>(riskEvaluation));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(UpdateRiskValues(Service.GetRiskEvaluationById(((RiskEvaluation)result.Object).Id)));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
            }
        }

        public JsonResult RiskEvaluations_Update()
        {
            try
            {
                var riskEvaluation = this.DeserializeObject<RiskEvaluation>("riskEvaluation");
                if (riskEvaluation == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
                }

                var workStation = Service.GetWorkStationById(riskEvaluation.WorkStationId);
                if (workStation.RiskEvaluations.FirstOrDefault(x => x.DeltaCodeId == riskEvaluation.DeltaCodeId && x.Id != riskEvaluation.Id) != null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
                }

                var result = Service.SaveRiskEvaluation(riskEvaluation);

                return result.Status != Status.Error ? this.Jsonp(UpdateRiskValues(Service.GetRiskEvaluationById(((RiskEvaluation)result.Object).Id))) : this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del RiskEvaluation" });
            }
        }

        public ActionResult RiskEvaluations_Destroy()
        {
            try
            {
                var riskEvaluation = this.DeserializeObject<RiskEvaluation>("riskEvaluation");
                if (riskEvaluation == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del RiskEvaluation" });
                }

                var result = Service.DeleteRiskEvaluation(riskEvaluation.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del RiskEvaluation" });
                }

                return this.Jsonp(AutoMapper.Mapper.Map<RiskEvaluationViewModel>(riskEvaluation));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del RiskEvaluation" });
            }
        }

        [HttpGet]
        public JsonResult GetDeltaCodes([DataSourceRequest] DataSourceRequest request, int workStationId)
        {
            var data = AutoMapper.Mapper.Map<List<DeltaCodeViewModel>>(Service.GetDeltaCodes());

            return this.Jsonp(data);
        }

        [HttpGet]
        public JsonResult GetProbabilities([DataSourceRequest] DataSourceRequest request)
        {
            var probabilities = new List<GenericModelDropDown>
            {
                new GenericModelDropDown { Id = 1, Name = "Baja" },
                new GenericModelDropDown { Id = 2, Name = "Media" },
                new GenericModelDropDown { Id = 3, Name = "Alta" }
            };

            return this.Jsonp(probabilities);
        }

        [HttpGet]
        public JsonResult GetSeverities([DataSourceRequest] DataSourceRequest request)
        {
            var severities = new List<GenericModelDropDown>
            {
                new GenericModelDropDown { Id = 1, Name = "Ligeramente Dañino" },
                new GenericModelDropDown { Id = 2, Name = "Dañino" },
                new GenericModelDropDown { Id = 3, Name = "Extremadamente Dañino" }
            };

            return this.Jsonp(severities);
        }

        private RiskEvaluationViewModel UpdateRiskValues(RiskEvaluation riskEvaluation)
        {
            var riskEvaluationViewModel = AutoMapper.Mapper.Map<RiskEvaluationViewModel>(riskEvaluation);
            var probabilities = new List<GenericModelDropDown>
            {
                new GenericModelDropDown { Id = 1, Name = "Baja" },
                new GenericModelDropDown { Id = 2, Name = "Media" },
                new GenericModelDropDown { Id = 3, Name = "Alta" }
            };
            var severities = new List<GenericModelDropDown>
            {
                new GenericModelDropDown { Id = 1, Name = "Ligeramente Dañino" },
                new GenericModelDropDown { Id = 2, Name = "Dañino" },
                new GenericModelDropDown { Id = 3, Name = "Extremadamente Dañino" }
            };

            riskEvaluationViewModel.ProbabilityName = probabilities.FirstOrDefault(x => x.Id == riskEvaluation.Probability)?.Name;
            riskEvaluationViewModel.SeverityName = severities.FirstOrDefault(x => x.Id == riskEvaluation.Severity)?.Name;

            if (riskEvaluationViewModel.RiskValue == 1)
            {
                riskEvaluationViewModel.RiskValueName = "Trivial";
                riskEvaluationViewModel.PriorityName = "Baja";
            }
            if (riskEvaluationViewModel.RiskValue == 2)
            {
                riskEvaluationViewModel.RiskValueName = "Tolerable";
                riskEvaluationViewModel.PriorityName = "Mediana";
            }
            if (riskEvaluationViewModel.RiskValue == 3)
            {
                riskEvaluationViewModel.RiskValueName = "Moderado";
                riskEvaluationViewModel.PriorityName = "Mediana - Alta";
            }
            if (riskEvaluationViewModel.RiskValue == 4)
            {
                riskEvaluationViewModel.RiskValueName = "Importante";
                riskEvaluationViewModel.PriorityName = "Alta";
            }
            if (riskEvaluationViewModel.RiskValue == 5)
            {
                riskEvaluationViewModel.RiskValueName = "Intolerable";
                riskEvaluationViewModel.PriorityName = "Inmediata";
            }

            return riskEvaluationViewModel;
        }
    }
}