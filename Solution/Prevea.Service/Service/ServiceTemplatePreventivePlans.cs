namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public List<TemplatePreventivePlan> GetTemplatePreventivePlans()
        {
            return Repository.GetTemplatePreventivePlans();
        }

        public TemplatePreventivePlan GetTemplatePreventivePlanById(int id)
        {
            return Repository.GetTemplatePreventivePlanById(id);
        }

        public Result SaveTemplatePreventivePlan(TemplatePreventivePlan templatePreventivePlan)
        {
            try
            {
                //templatePreventivePlan.Template = templatePreventivePlan.Template.Replace("\"", "'");
                templatePreventivePlan = Repository.SaveTemplatePreventivePlan(templatePreventivePlan);

                if (templatePreventivePlan == null)
                {                    
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del TemplatePreventivePlan",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del TemplatePreventivePlan se ha producido con éxito",
                    Object = templatePreventivePlan,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del TemplatePreventivePlan",
                    Object = templatePreventivePlan,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteTemplatePreventivePlan(int id)
        {
            try
            {
                var result = Repository.DeleteTemplatePreventivePlan(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el TemplatePreventivePlan",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del TemplatePreventivePlan se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el TemplatePreventivePlan",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Dictionary<string, string> GetEditorSnippets(int preventivePlanId, int templateId)
        {
            var preventivePlan = GetPreventivePlanById(preventivePlanId);
            var template = GetTemplatePreventivePlanById(templateId);

            var editorSnippets = new Dictionary<string, string>
            {
                { "Título", GetSnippetTitle(template.Name, preventivePlan) },
                { "Tablas de Riesgos", GetSnippetTablesRisks(preventivePlan) },
                { "Información Básica de la Empresa", GetSnippetBasicInformationCompany(preventivePlan) },
                { "Puestos y Personas", GetSnippetWorkStationAndEmployees(preventivePlan) },
                { "Medidas Preventivas de los Puestos de Trabajo", GetSnippetPreventiveMeasureWorkStation(preventivePlan) }
            };

            return editorSnippets;
        }

        private static string GetSnippetTitle(string title, PreventivePlan preventivePlan)
        {
            var coverPage = string.Empty;

            coverPage += "<br/><br/><br/><br/><br/>";
            coverPage += "<table style='width: 500px; margin-left: auto; margin-right: auto;'>";
            coverPage += "<tr>";
            coverPage += "<td>";
            coverPage += $"<div style='text-align: center; background: #1F497D; color: white; font-size: 24px; padding: 10px;'>{title}</div>";
            coverPage += "</td>";
            coverPage += "</tr>";
            coverPage += "<tr>";
            coverPage += "<td style='font-weight: bold; font-size: 16px; font-family: Arial; color: #1F497D;'>";
            coverPage += "<br /><br />";
            coverPage += $"{preventivePlan.Company.Name}";
            coverPage += "<br />";
            coverPage += $"{preventivePlan.Company.Address} {preventivePlan.Company.Province}";
            coverPage += "</td>";
            coverPage += "</tr>";
            coverPage += "<tr>";
            coverPage += "<td style='font-weight: bold; font-size: 12px; font-family: Arial; color: #1F497D; text-align: right;'>";
            coverPage += "<br /><br /><br /><br />";
            coverPage += "PREVEA CONSULTORES Y PROYECTOS";
            coverPage += "<br />";
            coverPage += "Servicio de Prevención Ajeno";
            coverPage += "<br />";
            coverPage += "C/ Abad Juan Catalán, 38";
            coverPage += "<br />";
            coverPage += "28032  Madrid";
            coverPage += "<br />";
            coverPage += "Tel. 91 760 27 13";
            coverPage += "<br />";
            coverPage += "prevea@preveaspa.com";
            coverPage += "</td>";
            coverPage += "</tr>";
            coverPage += "</table>";
            coverPage += "<br/><br/>";

            return coverPage;
        }

        private string GetSnippetTablesRisks(PreventivePlan preventivePlan)
        {
            var tables = string.Empty;
            var users = preventivePlan.Company.Employees.Select(x => x.User);
            var distinctWorkStations = users
                .GroupBy(y => y.WorkStationId)
                .Select(z => new
                {
                    Id = z.Key,
                    Count = z.Select(l => l.Id).Distinct().Count()
                })
                .ToList();
            foreach (var workStationCount in distinctWorkStations)
            {
                if (workStationCount.Id == null)
                    continue;

                var workSation = GetWorkStationById((int)workStationCount.Id);
                tables += "<table style='width: 800px; margin-left: auto; margin-right: auto; border: 1px solid black; font-size: 12px; font-family: Arial;'>";
                tables += "<tr style='border: 1px solid black; padding: 6px; border: 1px solid black; '>";
                tables += "<td colspan='3' style='padding: 6px; border: 1px solid black; '>";
                tables += "<p style='font-weight: bold;'>PREVEA</p>";
                tables += "<p style='font-weight: bold;'>CONSULTORES Y PROYECTOS, S.L.</p>";
                tables += "<p>C/ Abad Juan Catalán. 38. 28032 Madrid</p><br />";
                tables += "<p style='font-weight: bold;'>SERVICIO DE PREVENCIÓN AJENO</p>";
                tables += "</td>";
                tables += "<td colspan='5' style='padding: 6px; border: 1px solid black; border: 1px solid black; '>";
                tables += "<p style='text-align: center; font-weight: bold;'>DATOS IDENTIFICATIVOS</p>";
                tables += $"<p style='text-align: center; font-weight: bold;'>EMPRESA: {preventivePlan.Company.Name}</p>";
                tables += "</td>";
                tables += "</tr>";
                tables += "<tr style='border: 1px solid black; padding: 6px;  font-size: 12px; font-family: Arial; padding: 6px;'>";
                tables += "<td colspan='4' style='padding: 6px; border: 1px solid black; '>";
                tables += $"<strong>ACTIVIDAD:</strong> {preventivePlan.Company.Cnae.Name}";
                tables += "</td>";
                tables += "<td colspan='3' style='padding: 6px; border: 1px solid black; '>";
                tables += string.IsNullOrEmpty(workSation.ProfessionalCategory) ? 
                    $"<strong>PUESTO:</strong> {workSation.Name}" 
                    : $"<strong>PUESTO:</strong> {workSation.Name} ({workSation.ProfessionalCategory})";
                tables += "</td>";
                tables += "<td colspan='1' style='padding: 6px; border: 1px solid black; '>";
                tables += $"Nº TRABAJADORES: {workStationCount.Count}";
                tables += "</td>";
                tables += "</tr>";
                tables += "<tr style='border: 1px solid black; font-size: 12px; font-family: Arial; padding: 6px;'>";
                tables += "<td colspan='8' style='padding: 6px; border: 1px solid black; '>";
                tables += "<p style='text-align: center; font-weight: bold;'>EVALUACION DE RIESGOS Y PLANIFICACION PREVENTIVA</p>";
                tables += "</td>";
                tables += "</tr>";
                tables += "<tr style='border: 1px solid black; padding: 6px; font-size: 12px; font-family: Arial; padding: 6px;'>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "IDENTIFICACIÓN del RIESGO";
                tables += "</td>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "CÓDIGO";
                tables += "</td>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "PROBABILIDAD";
                tables += "</td>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "SEVERIDAD";
                tables += "</td>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "VALOR del RIESGO";
                tables += "</td>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "PRIORIDAD";
                tables += "</td>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "COSTE";
                tables += "</td>";
                tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                tables += "MEDIDAS PREVENTIVAS";
                tables += "</td>";
                tables += "</tr>";
                foreach (var riskEvaluation in workSation.RiskEvaluations)
                {
                    tables += "<tr style='border: 1px solid black; padding: 6px;  font-size: 12px; font-family: Arial'>";
                    tables += "<td style='text-align: justify; border: 1px solid black; padding: 6px;'>";
                    tables += riskEvaluation.DeltaCode.Name;
                    tables += "</td>";
                    tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                    tables += riskEvaluation.DeltaCode.Id;
                    tables += "</td>";
                    tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                    tables += GetProbability(riskEvaluation.Probability);
                    tables += "</td>";
                    tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                    tables += GetSeverity(riskEvaluation.Severity);
                    tables += "</td>";
                    tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                    tables += GetRiskValue(riskEvaluation.RiskValue);
                    tables += "</td>";
                    tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                    tables += GetPriority(riskEvaluation.Priority);
                    tables += "</td>";
                    tables += "<td style='text-align: center; border: 1px solid black; padding: 6px;'>";
                    tables += "S/COSTE";
                    tables += "</td>";
                    tables += "<td style='text-align: justify; border: 1px solid black; padding: 6px;'>";
                    tables += GetFormatHTML(riskEvaluation.RiskDetected);
                    tables += "</td>";
                    tables += "</tr>";
                }

                tables += "</table>";
                tables += "<br/><br/>";
            }

            return tables;
        }

        private string GetSnippetBasicInformationCompany(PreventivePlan preventivePlan)
        {
            var result = string.Empty;

            result += "<br/><br/>";

            result += "<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>Nº EVALUACIÓN</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>&nbsp;</h3></td></tr></tbody></table>";
            result += "<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>TIPO</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>PERIÓDICA</h3></td></tr></tbody></table>";
            result += $"<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>FECHA</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>{DateTime.Now.ToShortDateString()}</h3></td></tr></tbody></table>";
            result += $"<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>ACTIVIDAD CNAE</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>{preventivePlan.Company.Cnae.Name}</h3></td></tr></tbody></table>";
            result += $"<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>RAZÓN SOCIAL</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>{preventivePlan.Company.Name}</h3></td></tr></tbody></table>";

            var contactPerson = preventivePlan.Company.ContactPersons.FirstOrDefault(x =>
                x.ContactPersonTypeId == (int)EnContactPersonType.LegalRepresentative);
            var nameContactPerson = contactPerson != null ? 
                $"{contactPerson.User.FirstName} {contactPerson.User.LastName}" : 
                string.Empty;

            result += $"<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>REPRESENTANTE</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>{nameContactPerson}</h3></td></tr></tbody></table>";
            result += $"<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>DOMICILIO</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>{preventivePlan.Company.Address}</h3></td></tr></tbody></table>";
            result += $"<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>C.P.</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>{preventivePlan.Company.PostalCode}</h3></td></tr></tbody></table>";
            result += $"<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:33%; color:white; padding-left:20px; background-color: #007CC0;'><h3>LOCALIDAD</h3></td><td style='width:67%; border: 1px solid black; padding-left:20px; '><h3>{preventivePlan.Company.Province}</h3></td></tr></tbody></table>";
            result += "<table style='width:90%; margin: 0 auto;'><tbody><tr style='height:100%;'><td style='width:100%; color:white; padding-left:20px; background-color: #007CC0;'><h3>CENTROS DE TRABAJO</h3></td></tr></tbody></table>";

            result += "<table style='width:90%; margin: 0 auto;'><tbody>";
            var workCenters = GetWorkCentersByCompany(preventivePlan.Company.Id);

            for (var i = 0; i < workCenters.Count; i++)
            {
                if (i == 0)
                {
                    result += "<tr style='height:100%;'>";
                    var address = $"{workCenters[i].Address} {workCenters[i].PostalCode}, {workCenters[i].Province} ({workCenters[i].Location})";
                    result += $"<td style='width:33%; border-left: 1px solid black; border-top: 1px solid black; border-bottom: 1px solid black; padding-left:20px;'>{address}</td>";
                    result += $"<td style='width:33%; border-top: 1px solid black; border-bottom: 1px solid black; padding-left:20px;'>{workCenters[i].EstablishmentType.Description}</td>";
                    result += $"<td style='width:33%; border-right: 1px solid black; border-top: 1px solid black; border-bottom: 1px solid black; padding-left:20px;'>{workCenters[i].Description}</td>";
                    result += "</tr>";
                }
                else
                {
                    result += "<tr style='height:100%;'>";
                    var address = $"{workCenters[i].Address} {workCenters[i].PostalCode}, {workCenters[i].Province} ({workCenters[i].Location})";
                    result += $"<td style='width:33%; border-left: 1px solid black; border-bottom: 1px solid black; padding-left:20px;'>{address}</td>";
                    result += $"<td style='width:33%; border-bottom: 1px solid black; padding-left:20px;'>{workCenters[i].EstablishmentType.Description}</td>";
                    result += $"<td style='width:33%; border-right: 1px solid black; border-bottom: 1px solid black; padding-left:20px;'>{workCenters[i].Description}</td>";
                    result += "</tr>";
                }
            }

            result += "</tbody></table>";

            result += "<br/><br/>";

            return result;
        }

        private string GetSnippetWorkStationAndEmployees(PreventivePlan preventivePlan)
        {
            var result = string.Empty;

            var users = preventivePlan.Company.Employees.Select(x => x.User);
            var distinctWorkStations = users
                .GroupBy(y => y.WorkStationId)
                .Select(z => new
                {
                    Id = z.Key,
                    Count = z.Select(l => l.Id).Distinct().Count()
                })
                .ToList();

            var index = 1;
            foreach (var workStationCount in distinctWorkStations)
            {
                if (workStationCount.Id == null)
                    continue;

                var workSation = GetWorkStationById((int) workStationCount.Id);
                result += "<br/><br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          $"<td style='width:95%; border-left: 1px solid black; border-top: 1px solid black;'><p><strong><span style='font-size:medium;'>[{workSation.Cnae.CustomKey}] {workSation.Name}</span></strong></p></td>" +
                          $"<td rowspan='2' style='text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black; border-top: 1px solid black;'><h1>P{index:00}</h1></td>" +
                          "</tr>" +
                          "<tr style='height:100%;'>" +
                          $"<td style='border-left: 1px solid black; border-bottom: 1px solid black;'> {workSation.ProfessionalCategory}</td>" +
                          "</tr>" +
                          "</tbody></table>";

                result += "<br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          "<td style=''>Descripción Puesto</td>" +
                          "</tr>" +
                          "<tr style='height:100%;'>" +
                          $"<td style='width:95%;'><p><strong><span style='font-size:medium;'>{workSation.Description}</span></strong></p></td>" +
                          "</tr>" +
                          "</tbody></table>";

                result += "<br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          "<td colspan='2' style=''>Riesgos Detectados del Puesto</td>" +
                          "</tr>";                

                foreach (var riskEvaluation in workSation.RiskEvaluations)
                {
                    result += "<tr style='height:100%;'>";
                    result += "<td style='font-weight: bold;'>";
                    result += $"<P> - [R{riskEvaluation.DeltaCodeId:000}] {riskEvaluation.DeltaCode.Name}</P>";
                    result += "</td'>";

                    result += "<td style='font-weight: bold;'>";
                    result += $"<P style='text-align: center;'>{GetRiskValueName(riskEvaluation.RiskValue)}</P>";
                    result += "</td'>";
                    result += "</tr>";
                }

                result += "</tbody></table>";

                result += "<br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          "<td style=''>Trabajadores vinculados al Puesto</td>" +
                          "</tr>";
                foreach (var user in users)
                {
                    if (user.WorkStationId != workSation.Id)
                        continue;

                    result += "<tr style='height:100%;'>" +
                              $"<td style='width:95%;'><p><strong> - {user.FirstName} {user.LastName}</strong></p></td>" +
                              "</tr>";
                }

                result += "</tbody></table>";

                index++;
            }

            return result;
        }

        private string GetSnippetPreventiveMeasureWorkStation(PreventivePlan preventivePlan)
        {
            var result = string.Empty;

            var users = preventivePlan.Company.Employees.Select(x => x.User);
            var distinctWorkStations = users
                .GroupBy(y => y.WorkStationId)
                .Select(z => new
                {
                    Id = z.Key,
                    Count = z.Select(l => l.Id).Distinct().Count()
                })
                .ToList();

            var index = 1;
            foreach (var workStationCount in distinctWorkStations)
            {
                if (workStationCount.Id == null)
                    continue;

                var workSation = GetWorkStationById((int)workStationCount.Id);
                result += "<br/><br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          $"<td style='width:95%; border-left: 1px solid black; border-top: 1px solid black;'><p><strong><span style='font-size:medium;'>[{workSation.Cnae.CustomKey}] {workSation.Name}</span></strong></p></td>" +
                          $"<td rowspan='2' style='text-align: center; border-right: 1px solid black; border-left: 1px solid black; border-bottom: 1px solid black; border-top: 1px solid black;'><h1>P{index:00}</h1></td>" +
                          "</tr>" +
                          "<tr style='height:100%;'>" +
                          $"<td style='border-left: 1px solid black; border-bottom: 1px solid black;'> {workSation.ProfessionalCategory}</td>" +
                          "</tr>" +
                          "</tbody></table>";

                result += "<br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          "<td style=''>Descripción Puesto</td>" +
                          "</tr>" +
                          "<tr style='height:100%;'>" +
                          $"<td style='width:95%;'><p><strong><span style='font-size:medium;'>{workSation.Description}</span></strong></p></td>" +
                          "</tr>" +
                          "</tbody></table>";

                result += "<br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          "<td colspan='2' style=''>Riesgos Detectados del Puesto</td>" +
                          "</tr>";

                foreach (var riskEvaluation in workSation.RiskEvaluations)
                {
                    result += "<tr style='height:100%;'>";
                    result += "<td style='font-weight: bold;'>";
                    result += $"<P> - [R{riskEvaluation.Id:000}] {riskEvaluation.DeltaCode.Name}</P>";
                    result += "</td'>";

                    result += "<td style='font-weight: bold;'>";
                    result += $"<P style='text-align: center;'>{GetRiskValueName(riskEvaluation.RiskValue)}</P>";
                    result += "</td'>";
                    result += "</tr>";
                }

                result += "</tbody></table>";

                result += "<br/>";

                result += "<table style='width:90%; margin: 0 auto;'><tbody>" +
                          "<tr style='height:100%;'>" +
                          "<td style=''>Trabajadores vinculados al Puesto</td>" +
                          "</tr>";
                foreach (var user in users)
                {
                    if (user.WorkStationId != workSation.Id)
                        continue;

                    result += "<tr style='height:100%;'>" +
                              $"<td style='width:95%;'><p><strong> - {user.FirstName} {user.LastName}</strong></p></td>" +
                              "</tr>";
                }

                result += "</tbody></table>";

                index++;
            }

            return result;
        }

        private string GetRiskValueName(int riskValue)
        {
            switch (riskValue)
            {
                case 1:
                    return "<span style='color: green;'>TRIVIAL</span>";
                case 2:
                    return "<span style='color: yellow;'>TOLERABLE</span>";
                case 3:
                    return "<span style='color: orange;'>MODERADO</span>";
                case 4:
                    return "<span style='color: red;'>IMPORTANTE</span>";
                case 5:
                    return "<span style='color: black;'>INTOLERABLE</span>";

                default:
                    return string.Empty;
            }
        }

        private static string GetProbability(int probability)
        {
            switch (probability)
            {
                case 1:
                    return "Baja";
                case 2:
                    return "Media";
                case 3:
                    return "Alta";

                default:
                    return string.Empty;
            }
        }

        private static string GetSeverity(int severity)
        {
            switch (severity)
            {
                case 1:
                    return "Ligeramente Dañino";
                case 2:
                    return "Dañino";
                case 3:
                    return "Extremadamente Dañino";

                default:
                    return string.Empty;
            }
        }

        private static string GetRiskValue(int riskValue)
        {
            switch (riskValue)
            {
                case 1:
                    return "Trivial";
                case 2:
                    return "Tolerable";
                case 3:
                    return "Moderado";
                case 4:
                    return "Importante";
                case 5:
                    return "Intolerable";

                default:
                    return string.Empty;
            }
        }

        private static string GetPriority(int priority)
        {
            switch (priority)
            {
                case 1:
                    return "Baja";
                case 2:
                    return "Mediana";
                case 3:
                    return "Mediana - Alta";
                case 4:
                    return "Alta";
                case 5:
                    return "Inmediata";

                default:
                    return string.Empty;
            }
        }

        private static string GetFormatHTML(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            if (text.IndexOf("\n", StringComparison.Ordinal) == -1)
                return text;

            text = text.Replace("\r\n", @"</P><P>");
            text = text.Replace("\n", @"</P><P>");

            text = "<P>" + text;

            return text + "</P>";
        }
    }
}
