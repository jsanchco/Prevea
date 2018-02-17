namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;
    using System.Collections.Generic;

    #endregion

    public partial class Repository
    {
        public SimulationCompany GetSimulationCompany(int simulationId, int? companyId = null)
        {
            return companyId != null
                ? Context.SimulationCompanies
                    .Include(x => x.Simulation)
                    .Include(x => x.Company)
                    .FirstOrDefault(x => x.SimulationId == simulationId && x.CompanyId == companyId)
                : Context.SimulationCompanies
                    .Include(x => x.Simulation)
                    .Include(x => x.Company)
                    .FirstOrDefault(x => x.SimulationId == simulationId);
        }

        public SimulationCompany SaveSimulationCompany(int simulationId, int? companyId = null)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulationCompany = new SimulationCompany {SimulationId = simulationId, CompanyId = companyId};
                    Context.SimulationCompanies.Add(simulationCompany);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return simulationCompany;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public SimulationCompany UpdateSimulationCompany(int simulationId, int companyId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulationCompany = Context.SimulationCompanies.FirstOrDefault(x => x.SimulationId == simulationId);
                    if (simulationCompany == null)
                        return null;

                    var simulationCompanyFind = Context.SimulationCompanies.Find(simulationCompany.Id);
                    if (simulationCompanyFind == null)
                        return null;

                    simulationCompanyFind.CompanyId = companyId;
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return simulationCompanyFind;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteSimulationCompany(int simulationId, int companyId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulationCompany = Context.SimulationCompanies.FirstOrDefault(x => x.SimulationId == simulationId);
                    if (simulationCompany == null)
                        return false;

                    var simulationCompanyFind = Context.SimulationCompanies.Find(simulationCompany.Id);
                    if (simulationCompanyFind == null)
                        return false;

                    Context.SimulationCompanies.Remove(simulationCompanyFind);

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

        public List<SimulationCompany> GetSimulationsCompanyByCompany(int companyId)
        {
            return Context.SimulationCompanies
                .Include(x => x.Simulation)
                .Include(x => x.Company)
                .Where(x => x.CompanyId == companyId).ToList();
        }
    }
}
