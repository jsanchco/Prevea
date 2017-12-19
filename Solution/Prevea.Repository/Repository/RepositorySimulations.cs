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
            return Context.Simulations
                .Include(x => x.User)
                .Include(x => x.UserAssigned)
                .Include(x => x.SimulationState)
                .Include(x => x.SimulationCompanies)
                .FirstOrDefault(m => m.Id == id);
        }

        public List<Simulation> GetSimulations()
        {
            return Context.Simulations
                .Include(x => x.User)
                .Include(x => x.UserAssigned)
                .Include(x => x.SimulationState)
                .Include(x => x.SimulationCompanies)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public Simulation SaveSimulation(Simulation simulation)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Simulations.Add(simulation);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return simulation;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Simulation UpdateSimulation(int id, Simulation simulator)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulationFind = Context.Simulations.Find(id);
                    if (simulationFind == null)
                        return null;

                    Context.Entry(simulationFind).CurrentValues.SetValues(simulator);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return simulator;
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

                    Context.Simulations.Remove(simulationFind);

                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return true;
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
                    return Context.Simulations
                        .Include(x => x.User)
                        .Include(x => x.UserAssigned)
                        .Include(x => x.SimulationState)
                        .Include(x => x.SimulationCompanies)
                        .OrderByDescending(x => x.Date)
                        .ToList();

                case (int)EnRole.PreveaPersonal:
                    return Context.Simulations
                    .Include(x => x.User)
                    .Include(x => x.UserAssigned)
                    .Include(x => x.SimulationState)
                    .Include(x => x.SimulationCompanies)
                    .OrderByDescending(x => x.Date)
                    .Where(x => x.SimulationStateId != (int)EnSimulationState.Validated || x.UserAssignedId == userId)
                    .ToList();

                default:
                    return Context.Simulations
                        .Include(x => x.User)
                        .Include(x => x.UserAssigned)
                        .Include(x => x.SimulationState)
                        .Include(x => x.SimulationCompanies)
                        .OrderByDescending(x => x.Date)
                        .Where(x => x.UserId == userId)
                        .ToList();
            }
        }
    }
}
