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
    using System.Linq;
    using IRepository.IRepository;

    #endregion

    public class TecniquesController : BaseController
    {
        #region Constructor

        public TecniquesController(IRepository repository) : base(repository)
        {
        }

        #endregion

        [HttpGet]
        public ActionResult WorkStations(int? cnaeSelected)
        {
            ViewBag.CnaeSelected = cnaeSelected;

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
        public ActionResult TemplatePreventivePlans()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult DetailTemplatePreventivePlan(int id)
        {
            var templatePreventivePlan = Service.GetTemplatePreventivePlanById(id);

            ViewBag.TemplateId = id;
            ViewBag.Title = templatePreventivePlan.Name;

            return PartialView();
        }

        [HttpGet]
        public ActionResult RiskEvaluation(int cnaeId, int workStationId)
        {
            ViewBag.CnaeId = cnaeId;
            ViewBag.WorkStationId = workStationId;

            var workStation = Service.GetWorkStationById(workStationId);
            ViewBag.WorkStation = workStation.Name;
            if (!string.IsNullOrEmpty(workStation.ProfessionalCategory))
                ViewBag.WorkStation =
                    $"[{workStation.Cnae.Key}] {workStation.Cnae.Name} / {ViewBag.WorkStation} ({workStation.ProfessionalCategory})";

            return PartialView();
        }

        [HttpGet]
        public ActionResult ViewerHTML()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult DetailRiskEvaluation(int riskEvaluationId, int cnaeId, int workStationId, int selectTabId,
            string notification)
        {
            ViewBag.RiskEvaluationId = riskEvaluationId;
            ViewBag.CnaeId = cnaeId;
            ViewBag.WorkStationId = workStationId;
            ViewBag.SelectTabId = selectTabId;
            ViewBag.Notification = notification;

            var workStation = Service.GetWorkStationById(workStationId);
            ViewBag.WorkStation = workStation.Name;
            if (!string.IsNullOrEmpty(workStation.ProfessionalCategory))
                ViewBag.WorkStation =
                    $"[{workStation.Cnae.Key}] {workStation.Cnae.Name} / {ViewBag.WorkStation} ({workStation.ProfessionalCategory})";

            return PartialView(
                AutoMapper.Mapper.Map<RiskEvaluationViewModel>(Service.GetRiskEvaluationById(riskEvaluationId)));
        }

        [HttpGet]
        public ActionResult RiskDetected(int riskEvaluationId)
        {
            var riskEvaluationVM =
                AutoMapper.Mapper.Map<RiskEvaluationViewModel>(Service.GetRiskEvaluationById(riskEvaluationId));
            riskEvaluationVM.SelectTab = 0;

            return PartialView(riskEvaluationVM);
        }

        [HttpGet]
        public ActionResult CorrectiveActions(int riskEvaluationId)
        {
            var riskEvaluationVM =
                AutoMapper.Mapper.Map<RiskEvaluationViewModel>(Service.GetRiskEvaluationById(riskEvaluationId));
            riskEvaluationVM.SelectTab = 1;

            return PartialView(riskEvaluationVM);
        }

        [HttpGet]
        public ActionResult IndividualProtectionEquipments(int riskEvaluationId)
        {
            var riskEvaluationVM =
                AutoMapper.Mapper.Map<RiskEvaluationViewModel>(Service.GetRiskEvaluationById(riskEvaluationId));
            riskEvaluationVM.SelectTab = 2;

            return PartialView(riskEvaluationVM);
        }

        [HttpGet]
        public ActionResult CollectiveProtectionEquipments(int riskEvaluationId)
        {
            var riskEvaluationVM =
                AutoMapper.Mapper.Map<RiskEvaluationViewModel>(Service.GetRiskEvaluationById(riskEvaluationId));
            riskEvaluationVM.SelectTab = 3;

            return PartialView(riskEvaluationVM);
        }

        [HttpGet]
        public JsonResult Cnaes_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<CnaeViewModel>>(Service.GetCnaes());

            return this.Jsonp(data);
        }

        [HttpGet]
        public JsonResult WorkStations_Read([DataSourceRequest] DataSourceRequest request, int cnaeId)
        {
            var data = AutoMapper.Mapper.Map<List<WorkStationViewModel>>(Service.GetWorkStationsByCnaeId(cnaeId));

            return this.Jsonp(data);
        }

        public ActionResult WorkStations_Create()
        {
            try
            {
                var workStation = this.DeserializeObject<WorkStationViewModel>("workStation");
                if (workStation == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del WorkStation"});
                }

                var result = Service.SaveWorkStation(AutoMapper.Mapper.Map<WorkStation>(workStation));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<WorkStationViewModel>(result.Object));
                }

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del WorkStation"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del WorkStation"});
            }
        }

        public JsonResult WorkStations_Update()
        {
            try
            {
                var workStation = this.DeserializeObject<WorkStation>("workStation");
                if (workStation == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del WorkStation"});
                }

                var result = Service.SaveWorkStation(workStation);

                return result.Status != Status.Error
                    ? this.Jsonp(AutoMapper.Mapper.Map<WorkStationViewModel>(workStation))
                    : this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del WorkStation"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del WorkStation"});
            }
        }

        public ActionResult WorkStations_Destroy()
        {
            try
            {
                var workStation = this.DeserializeObject<WorkStation>("workStation");
                if (workStation == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del WorkStation"});
                }

                var result = Service.DeleteWorkStation(workStation.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del WorkStation"});
                }

                return this.Jsonp(AutoMapper.Mapper.Map<WorkStationViewModel>(workStation));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del WorkStation"});
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
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del DeltaCode"});
                }

                var result = Service.SaveDeltaCode(AutoMapper.Mapper.Map<DeltaCode>(deltaCode));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<DeltaCodeViewModel>(result.Object));
                }

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del DeltaCode"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del DeltaCode"});
            }
        }

        public JsonResult DeltaCodes_Update()
        {
            try
            {
                var deltaCode = this.DeserializeObject<DeltaCode>("deltaCode");
                if (deltaCode == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del DeltaCode"});
                }

                var result = Service.SaveDeltaCode(deltaCode);

                return result.Status != Status.Error
                    ? this.Jsonp(AutoMapper.Mapper.Map<DeltaCodeViewModel>(deltaCode))
                    : this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del DeltaCode"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del DeltaCode"});
            }
        }

        public ActionResult DeltaCodes_Destroy()
        {
            try
            {
                var deltaCode = this.DeserializeObject<DeltaCode>("deltaCode");
                if (deltaCode == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del DeltaCode"});
                }

                var result = Service.DeleteDeltaCode(deltaCode.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del DeltaCode"});
                }

                return this.Jsonp(AutoMapper.Mapper.Map<DeltaCodeViewModel>(deltaCode));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del DeltaCode"});
            }
        }

        [HttpGet]
        public JsonResult RiskEvaluations_Read([DataSourceRequest] DataSourceRequest request, int cnaeId,
            int workStationId)
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
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
                }

                var workStation = Service.GetWorkStationById(riskEvaluation.WorkStationId);
                if (workStation.RiskEvaluations.FirstOrDefault(x => x.DeltaCodeId == riskEvaluation.DeltaCodeId) !=
                    null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
                }

                var result = Service.SaveRiskEvaluation(AutoMapper.Mapper.Map<RiskEvaluation>(riskEvaluation));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(
                        UpdateRiskValues(Service.GetRiskEvaluationById(((RiskEvaluation) result.Object).Id)));
                }

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
            }
        }

        public JsonResult RiskEvaluations_Update()
        {
            try
            {
                var riskEvaluation = this.DeserializeObject<RiskEvaluation>("riskEvaluation");
                if (riskEvaluation == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
                }

                var workStation = Service.GetWorkStationById(riskEvaluation.WorkStationId);
                if (workStation.RiskEvaluations.FirstOrDefault(x =>
                        x.DeltaCodeId == riskEvaluation.DeltaCodeId && x.Id != riskEvaluation.Id) != null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
                }

                var result = Service.SaveRiskEvaluation(riskEvaluation);

                return result.Status != Status.Error
                    ? this.Jsonp(UpdateRiskValues(Service.GetRiskEvaluationById(((RiskEvaluation) result.Object).Id)))
                    : this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del RiskEvaluation"});
            }
        }

        public ActionResult RiskEvaluations_Destroy()
        {
            try
            {
                var riskEvaluation = this.DeserializeObject<RiskEvaluation>("riskEvaluation");
                if (riskEvaluation == null)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del RiskEvaluation"});
                }

                var result = Service.DeleteRiskEvaluation(riskEvaluation.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del RiskEvaluation"});
                }

                return this.Jsonp(AutoMapper.Mapper.Map<RiskEvaluationViewModel>(riskEvaluation));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del RiskEvaluation"});
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
                new GenericModelDropDown {Id = 1, Name = "Baja"},
                new GenericModelDropDown {Id = 2, Name = "Media"},
                new GenericModelDropDown {Id = 3, Name = "Alta"}
            };

            return this.Jsonp(probabilities);
        }

        [HttpGet]
        public JsonResult GetSeverities([DataSourceRequest] DataSourceRequest request)
        {
            var severities = new List<GenericModelDropDown>
            {
                new GenericModelDropDown {Id = 1, Name = "Ligeramente Dañino"},
                new GenericModelDropDown {Id = 2, Name = "Dañino"},
                new GenericModelDropDown {Id = 3, Name = "Extremadamente Dañino"}
            };

            return this.Jsonp(severities);
        }

        private RiskEvaluationViewModel UpdateRiskValues(RiskEvaluation riskEvaluation)
        {
            var riskEvaluationViewModel = AutoMapper.Mapper.Map<RiskEvaluationViewModel>(riskEvaluation);
            var probabilities = new List<GenericModelDropDown>
            {
                new GenericModelDropDown {Id = 1, Name = "Baja"},
                new GenericModelDropDown {Id = 2, Name = "Media"},
                new GenericModelDropDown {Id = 3, Name = "Alta"}
            };
            var severities = new List<GenericModelDropDown>
            {
                new GenericModelDropDown {Id = 1, Name = "Ligeramente Dañino"},
                new GenericModelDropDown {Id = 2, Name = "Dañino"},
                new GenericModelDropDown {Id = 3, Name = "Extremadamente Dañino"}
            };

            riskEvaluationViewModel.ProbabilityName =
                probabilities.FirstOrDefault(x => x.Id == riskEvaluation.Probability)?.Name;
            riskEvaluationViewModel.SeverityName =
                severities.FirstOrDefault(x => x.Id == riskEvaluation.Severity)?.Name;

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

        [HttpGet]
        public JsonResult TemplatePreventivePlans_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data =
                AutoMapper.Mapper.Map<List<TemplatePreventivePlanViewModel>>(Service.GetTemplatePreventivePlans());

            return this.Jsonp(data);
        }

        public ActionResult TemplatePreventivePlans_Create()
        {
            try
            {
                var templatePreventivePlan = this.DeserializeObject<TemplatePreventivePlan>("templatePreventivePlan");
                if (templatePreventivePlan == null)
                {
                    return this.Jsonp(new
                        {Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan"});
                }

                templatePreventivePlan.CreateDate = DateTime.Now;
                templatePreventivePlan.ModifyDate = DateTime.Now;
                var result = Service.SaveTemplatePreventivePlan(templatePreventivePlan);

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(result.Object);
                }

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan"});
            }
        }

        public JsonResult TemplatePreventivePlans_Update()
        {
            try
            {
                var templatePreventivePlan = this.DeserializeObject<TemplatePreventivePlan>("templatePreventivePlan");
                if (templatePreventivePlan == null)
                {
                    return this.Jsonp(new
                        {Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan"});
                }

                var templatePreventivePlanFind = Service.GetTemplatePreventivePlanById(templatePreventivePlan.Id);

                templatePreventivePlan.Template = templatePreventivePlanFind.Template;
                templatePreventivePlan.ModifyDate = DateTime.Now;
                var result = Service.SaveTemplatePreventivePlan(templatePreventivePlan);

                return result.Status != Status.Error
                    ? this.Jsonp(result.Object)
                    : this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan"});
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan"});
            }
        }

        public ActionResult TemplatePreventivePlans_Destroy()
        {
            try
            {
                var templatePreventivePlan = this.DeserializeObject<TemplatePreventivePlan>("templatePreventivePlan");
                if (templatePreventivePlan == null)
                {
                    return this.Jsonp(
                        new {Errors = "Se ha producido un error en el Borrado del TemplatePreventivePlan"});
                }

                var result = Service.DeleteTemplatePreventivePlan(templatePreventivePlan.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(
                        new {Errors = "Se ha producido un error en el Borrado del TemplatePreventivePlan"});
                }

                return this.Jsonp(templatePreventivePlan);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new {Errors = "Se ha producido un error en el Borrado del TemplatePreventivePlan"});
            }
        }

        public JsonResult SaveTemplate(int templateId, string text)
        {
            var template = Service.GetTemplatePreventivePlanById(templateId);
            if (template == null)
                return this.Jsonp(new {resultStatus = Status.Error});

            template.ModifyDate = DateTime.Now;
            template.Template = text;
            var result = Service.SaveTemplatePreventivePlan(template);

            return result.Status != Status.Error
                ? Json(new {resultStatus = Status.Ok})
                : Json(new {Errors = "Se ha producido un error en la Grabación del TemplatePreventivePlan"});
        }

        public JsonResult GetTemplate(int templateId)
        {
            var template = Service.GetTemplatePreventivePlanById(templateId);
            if (template == null)
                return this.Jsonp(new {resultStatus = Status.Error});

            return Json(new {resultStatus = Status.Ok, template = template.Template});
        }

        [HttpPost]
        public ActionResult UpdateRiskEvaluation(RiskEvaluationViewModel riskEvaluationVM)
        {
            var riskEvaluation = AutoMapper.Mapper.Map<RiskEvaluation>(riskEvaluationVM);
            var result = Service.UpdateRiskEvaluation(riskEvaluation.Id, riskEvaluation);

            ViewBag.RiskEvaluationId = riskEvaluationVM.Id;

            if (riskEvaluation.WorkStation == null)
            {
                var workStation = Service.GetWorkStationById(riskEvaluation.WorkStationId);
                if (workStation != null)
                    ViewBag.CnaeId = workStation.CnaeId;
            }
            else
            {
                ViewBag.CnaeId = riskEvaluation.WorkStation.CnaeId;
            }

            ViewBag.WorkStationId = riskEvaluation.WorkStationId;
            ViewBag.SelectTabId = riskEvaluationVM.SelectTab;

            if (result.Status != Status.Error)
            {
                ViewBag.Notification = "La Evaluación del Riesgo de ha actualizado correctamente";

                return PartialView("~/Views/Tecniques/DetailRiskEvaluation.cshtml", 
                    AutoMapper.Mapper.Map<RiskEvaluationViewModel>(Service.GetRiskEvaluationById(riskEvaluationVM.Id)));
            }

            ViewBag.Error = new List<string> { result.Message };
            return PartialView("~/Views/Tecniques/DetailRiskEvaluation.cshtml",
                AutoMapper.Mapper.Map<RiskEvaluationViewModel>(Service.GetRiskEvaluationById(riskEvaluationVM.Id)));
        }

        [HttpGet]
        public JsonResult CorrectiveActions_Read([DataSourceRequest] DataSourceRequest request, int riskEvaluationId)
        {
            var data = AutoMapper.Mapper.Map<List<CorrectiveActionViewModel>>(Service.GetCorrectiveActionsByRiskEvaluation(riskEvaluationId));

            return this.Jsonp(data);
        }

        public ActionResult CorrectiveActions_Create()
        {
            try
            {
                var correctiveAction = this.DeserializeObject<CorrectiveAction>("correctiveAction");
                if (correctiveAction == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del CorrectiveAction" });
                }

                var result = Service.SaveCorrectiveAction(AutoMapper.Mapper.Map<CorrectiveAction>(correctiveAction));

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<CorrectiveActionViewModel>(Service.GetCorrectiveActionById(correctiveAction.Id)));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del CorrectiveAction" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del CorrectiveAction" });
            }
        }

        public JsonResult CorrectiveActions_Update()
        {
            try
            {
                var correctiveAction = this.DeserializeObject<CorrectiveAction>("correctiveAction");
                if (correctiveAction == null)
                {
                    return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del CorrectiveAction" });
                }

                var result = Service.SaveCorrectiveAction(correctiveAction);

                if (result.Status != Status.Error)
                {
                    return this.Jsonp(AutoMapper.Mapper.Map<CorrectiveActionViewModel>(Service.GetCorrectiveActionById(correctiveAction.Id)));
                }

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del CorrectiveAction" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en la Grabación del CorrectiveAction" });
            }
        }

        public ActionResult CorrectiveActions_Destroy()
        {
            try
            {
                var correctiveAction = this.DeserializeObject<CorrectiveAction>("correctiveAction");
                if (correctiveAction == null)
                {
                    return this.Jsonp(
                        new { Errors = "Se ha producido un error en el Borrado del CorrectiveAction" });
                }

                var result = Service.DeleteCorrectiveAction(correctiveAction.Id);

                if (result.Status == Status.Error)
                {
                    return this.Jsonp(
                        new { Errors = "Se ha producido un error en el Borrado del correctiveAction" });
                }

                return this.Jsonp(correctiveAction);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return this.Jsonp(new { Errors = "Se ha producido un error en el Borrado del TemplatePreventivePlan" });
            }
        }

        [HttpGet]
        public JsonResult PriorityCorrectiveActions_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = AutoMapper.Mapper.Map<List<PriorityCorrectiveActionViewModel>>(Service.GetPriorityCorrectiveActions());

            return this.Jsonp(data);
        }
    }
}