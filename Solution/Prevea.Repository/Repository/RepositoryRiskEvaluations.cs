namespace Prevea.Repository.Repository
{
    #region Using

    using System;
    using System.Collections.Generic;
    using Model.Model;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<RiskEvaluation> GetRiskEvaluations()
        {
            return Context.RiskEvaluations
                .Include(x => x.WorkStation)
                .Include(x => x.DeltaCode)
                .ToList();
        }

        public List<RiskEvaluation> GetRiskEvaluationsByWorkStation(int workStationId)
        {
            return Context.RiskEvaluations
                .Include(x => x.WorkStation)
                .Include(x => x.DeltaCode)
                .Where(x => x.WorkStationId == workStationId)
                .ToList();
        }

        public List<RiskEvaluation> GetRiskEvaluationsByDeltaCode(int deltaCodeId)
        {
            return Context.RiskEvaluations
                .Include(x => x.WorkStation)
                .Include(x => x.DeltaCode)
                .Where(x => x.DeltaCodeId == deltaCodeId)
                .ToList();
        }

        public RiskEvaluation GetRiskEvaluationById(int id)
        {
            return Context.RiskEvaluations
                .Include(x => x.WorkStation)
                .Include(x => x.DeltaCode)
                .FirstOrDefault(x => x.Id == id);
        }

        public RiskEvaluation SaveRiskEvaluation(RiskEvaluation riskEvaluation)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.RiskEvaluations.AddOrUpdate(riskEvaluation);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return riskEvaluation;

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }
        }

        public RiskEvaluation UpdateRiskEvaluation(int id, RiskEvaluation riskEvaluation)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var riskEvaluationFind = Context.RiskEvaluations.Find(id);
                    if (riskEvaluationFind == null)
                        return null;

                    Context.Entry(riskEvaluationFind).CurrentValues.SetValues(riskEvaluation);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return riskEvaluation;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteRiskEvaluation(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var riskEvaluationFind = Context.RiskEvaluations.Find(id);
                    if (riskEvaluationFind == null)
                        return false;

                    Context.RiskEvaluations.Remove(riskEvaluationFind);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
