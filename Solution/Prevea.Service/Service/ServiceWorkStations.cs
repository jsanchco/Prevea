namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public List<WorkStation> GetWorkStations()
        {
            return Repository.GetWorkStations();
        }

        public WorkStation GetWorkStationById(int id)
        {
            return Repository.GetWorkStationById(id);
        }

        public Result SaveWorkStation(WorkStation workStation)
        {
            try
            {
                workStation = Repository.SaveWorkStation(workStation);

                if (workStation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del WorkStation",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del WorkStation se ha producido con éxito",
                    Object = workStation,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del WorkStation",
                    Object = workStation,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteWorkStation(int id)
        {
            try
            {
                var result = Repository.DeleteWorkStation(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el WorkStation",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del WorkStation se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el WorkStation",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result SaveWorkStationsInCNAE(int cnaeSelected, int[] workStationsSelected)
        {
            var cnae = GetCnae(cnaeSelected);
            if (cnae == null)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Guardar estos WorkStations",
                    Object = null,
                    Status = Status.Error
                };
            }

            foreach (var idWorkStation in workStationsSelected)
            {
                var workStation = GetWorkStationById(idWorkStation);
                var result = SaveWorkStation(new WorkStation
                {
                    CnaeId = cnaeSelected,
                    Description = workStation.Description,
                    Name = workStation.Name,
                    ProfessionalCategory = workStation.ProfessionalCategory                
                });
                if (result.Status == Status.Ok)
                {
                    var riskEvaluations = GetRiskEvaluationsByWorkStation(idWorkStation);
                    foreach (var riskEvaluation in riskEvaluations)
                    {
                        var resultSaveRiskEvaluation = SaveRiskEvaluation(new RiskEvaluation
                        {
                            WorkStationId = ((WorkStation)result.Object).Id,
                            CollectiveProtectionEquipments = riskEvaluation.CollectiveProtectionEquipments,
                            DeltaCodeId = riskEvaluation.DeltaCodeId,
                            IndividualProtectionEquipments = riskEvaluation.IndividualProtectionEquipments,
                            Priority = riskEvaluation.Priority,
                            Probability = riskEvaluation.Probability,
                            Severity = riskEvaluation.Severity,
                            RiskValue = riskEvaluation.RiskValue,
                            RiskDetected = riskEvaluation.RiskDetected
                        });
                        if (resultSaveRiskEvaluation.Status == Status.Ok && resultSaveRiskEvaluation.Object != null)
                        {
                            var correctiveActions = GetCorrectiveActionsByRiskEvaluation(riskEvaluation.Id);
                            foreach (var correctiveAction  in correctiveActions)
                            {
                                var resultSaveCorrectiveAction = SaveCorrectiveAction(new CorrectiveAction
                                {
                                    Description = correctiveAction.Description,
                                    PriorityCorrectiveActionId = correctiveAction.PriorityCorrectiveActionId,
                                    RiskEvaluationId = ((RiskEvaluation)resultSaveRiskEvaluation.Object).Id
                                });
                                if (resultSaveCorrectiveAction.Status == Status.Error)
                                {
                                    return new Result
                                    {
                                        Message = "Se ha producido un error al Guardar estos WorkStations",
                                        Object = null,
                                        Status = Status.Error
                                    };
                                }
                            }
                        }
                        else
                        {
                            return new Result
                            {
                                Message = "Se ha producido un error al Guardar estos WorkStations",
                                Object = null,
                                Status = Status.Error
                            };
                        }
                    }
                }
                else
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Guardar estos WorkStations",
                        Object = null,
                        Status = Status.Error
                    };
                }
            }

            return new Result
            {
                Message = "La grabación del WorkStation se ha producido con éxito",
                Object = null,
                Status = Status.Ok
            };
        }
    }
}
