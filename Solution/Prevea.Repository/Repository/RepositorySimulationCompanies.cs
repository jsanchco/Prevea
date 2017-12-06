namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;

    #endregion

    public partial class Repository
    {
        public SimulationCompany GetSimulatìonCompany(int simulatorId, int? companyId = null)
        {

            return Context.SimulationCompanies
                .Include(x => x.Simulator)
                .Include(x => x.Company)
                .FirstOrDefault(x => x.SimulatorId == simulatorId && x.CompanyId == companyId);
        }

        public SimulationCompany SaveSimulationCompany(int simulatorId, int? companyId = null)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulatorCompany = new SimulationCompany {SimulatorId = simulatorId, CompanyId = companyId};
                    Context.SimulationCompanies.Add(simulatorCompany);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return simulatorCompany;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public SimulationCompany UpdateSimulationCompany(int simulatorId, int companyId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulatorCompany = Context.SimulationCompanies.FirstOrDefault(x => x.SimulatorId == simulatorId);
                    if (simulatorCompany == null)
                        return null;

                    var simulatorCompanyFind = Context.SimulationCompanies.Find(simulatorCompany.Id);
                    if (simulatorCompanyFind == null)
                        return null;

                    simulatorCompanyFind.CompanyId = companyId;
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return simulatorCompanyFind;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteSimulationCompany(int simulatorId, int companyId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulatorCompany = Context.SimulationCompanies.FirstOrDefault(x => x.SimulatorId == simulatorId);
                    if (simulatorCompany == null)
                        return false;

                    var simulatorCompanyFind = Context.SimulationCompanies.Find(simulatorCompany.Id);
                    if (simulatorCompanyFind == null)
                        return false;

                    Context.SimulationCompanies.Remove(simulatorCompanyFind);

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
