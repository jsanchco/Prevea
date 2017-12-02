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
        public SimulatorCompany GetSimulatorCompany(int simulatorId, int? companyId = null)
        {

            return Context.SimulatorCompanies
                .Include(x => x.Simulator)
                .Include(x => x.Company)
                .FirstOrDefault(x => x.SimulatorId == simulatorId && x.CompanyId == companyId);
        }

        public SimulatorCompany SaveSimulatorCompany(int simulatorId, int? companyId = null)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulatorCompany = new SimulatorCompany {SimulatorId = simulatorId, CompanyId = companyId};
                    Context.SimulatorCompanies.Add(simulatorCompany);
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

        public SimulatorCompany UpdateSimulatorCompany(int simulatorId, int companyId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulatorCompany = Context.SimulatorCompanies.FirstOrDefault(x => x.SimulatorId == simulatorId);
                    if (simulatorCompany == null)
                        return null;

                    var simulatorCompanyFind = Context.SimulatorCompanies.Find(simulatorCompany.Id);
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

        public bool DeleteSimulatorCompany(int simulatorId, int companyId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulatorCompany = Context.SimulatorCompanies.FirstOrDefault(x => x.SimulatorId == simulatorId);
                    if (simulatorCompany == null)
                        return false;

                    var simulatorCompanyFind = Context.SimulatorCompanies.Find(simulatorCompany.Id);
                    if (simulatorCompanyFind == null)
                        return false;

                    Context.SimulatorCompanies.Remove(simulatorCompanyFind);

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
