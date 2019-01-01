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
        public List<RiskEvaluation> GetRiskEvaluations()
        {
            return Repository.GetRiskEvaluations();
        }

        public List<RiskEvaluation> GetRiskEvaluationsByWorkStation(int workStationId)
        {
            return Repository.GetRiskEvaluationsByWorkStation(workStationId);
        }

        public List<RiskEvaluation> GetRiskEvaluationsByDeltaCode(int deltaCodeId)
        {
            return Repository.GetRiskEvaluationsByDeltaCode(deltaCodeId);
        }

        public RiskEvaluation GetRiskEvaluationById(int id)
        {
            return Repository.GetRiskEvaluationById(id);
        }

        public Result SaveRiskEvaluation(RiskEvaluation riskEvaluation)
        {
            try
            {
                riskEvaluation = Repository.SaveRiskEvaluation(riskEvaluation);

                if (riskEvaluation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del RiskEvaluation",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del RiskEvaluation se ha producido con éxito",
                    Object = riskEvaluation,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del RiskEvaluation",
                    Object = riskEvaluation,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateRiskEvaluation(int id, RiskEvaluation riskEvaluation)
        {
            try
            {
                riskEvaluation.RiskDetected = System.Net.WebUtility.HtmlDecode(riskEvaluation.RiskDetected);
                riskEvaluation = Repository.UpdateRiskEvaluation(id, riskEvaluation);

                if (riskEvaluation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Evaluación del Riesgo",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Evaluación del Riesgo se ha producido con éxito",
                    Object = riskEvaluation,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Evaluación del Riesgo",
                    Object = riskEvaluation,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteRiskEvaluation(int id)
        {
            try
            {
                var result = Repository.DeleteRiskEvaluation(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el RiskEvaluation",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del RiskEvaluation se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el RiskEvaluation",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
