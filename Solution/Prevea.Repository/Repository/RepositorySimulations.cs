using System.Data.Entity.Migrations;

namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;

    #endregion

    public partial class Repository
    {
        public Simulation GetSimulation(int id)
        {
            var simulation = Context.Simulations
                .Include(x => x.User)
                .Include(x => x.UserAssigned)
                .Include(x => x.SimulationState)
                .Include(x => x.SimulationCompanies)
                .FirstOrDefault(m => m.Id == id);

            var simulationCompany = simulation?.SimulationCompanies.FirstOrDefault();
            if (simulationCompany?.CompanyId != null)
            {
                simulation.SimulationCompanyActive = GetSimulationsCompanyByCompany((int) simulationCompany.CompanyId)
                    .FirstOrDefault(x => x.Simulation.Active);
            }
           
            return simulation;
        }

        public List<Simulation> GetSimulations()
        {
            var simulations = Context.Simulations
                .Include(x => x.User)
                .Include(x => x.UserAssigned)
                .Include(x => x.SimulationState)
                .Include(x => x.SimulationCompanies)
                .OrderByDescending(x => x.Date)
                .ToList();

            foreach (var simulation in simulations)
            {
                var simulationCompany = simulation?.SimulationCompanies.FirstOrDefault();
                if (simulationCompany?.CompanyId != null)
                {
                    simulation.SimulationCompanyActive = GetSimulationsCompanyByCompany((int)simulationCompany.CompanyId)
                        .FirstOrDefault(x => x.Simulation.Active);
                }
            }

            return simulations;
        }

        public Simulation SaveSimulation(Simulation simulation)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Simulations.AddOrUpdate(simulation);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return GetSimulation(simulation.Id);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Simulation UpdateSimulation(int id, Simulation simulation)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulationFind = Context.Simulations.Find(id);
                    if (simulationFind == null)
                        return null;

                    Context.Entry(simulationFind).CurrentValues.SetValues(simulation);
                    Context.SaveChanges();
                   
                    dbContextTransaction.Commit();

                    return GetSimulation(simulation.Id);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteSimulation(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulationFind = Context.Simulations.Find(id);
                    if (simulationFind == null)
                        return false;

                    foreach (var notification in simulationFind.Notifications)
                    {
                        notification.Simulation = null;
                    }
                    Context.Simulations.Remove(simulationFind);


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

        public bool SubscribeSimulation(int id, bool subscribe)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulationFind = Context.Simulations.Find(id);
                    if (simulationFind == null)
                        return false;

                    simulationFind.SimulationStateId = subscribe
                        ? (int) EnSimulationState.ValidationPending
                        : (int) EnSimulationState.Deleted;

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

        public List<Simulation> GetSimulationByUser(int userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);

            var userRole = user?.UserRoles.FirstOrDefault();
            if (userRole == null)
                return null;
            switch (userRole.RoleId)
            {
                case (int) EnRole.Super:
                    return GetSimulations();

                case (int)EnRole.PreveaPersonal:
                    var usersChildren = Context.Users.Where(x => x.UserParentId == userId).Select(x => x.Id);

                    return GetSimulations()
                        .Where(x => x.SimulationStateId != (int)EnSimulationState.Deleted && usersChildren.Contains(x.UserId))
                        .ToList();

                default:
                    return GetSimulations()
                        .Where(x => x.UserId == userId && x.SimulationStateId != (int)EnSimulationState.Deleted)
                        .ToList();
            }
        }
    }
}
