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
        public Simulator GetSimulator(int id)
        {
            return Context.Simulators
                .Include(x => x.User)
                .Include(x => x.SimulatorCompanies)
                .FirstOrDefault(m => m.Id == id);
        }

        public List<Simulator> GetSimulators()
        {
            return Context.Simulators
                .Include(x => x.User)
                .OrderBy(x => x.Date)
                .ToList();
        }

        public Simulator SaveSimulator(Simulator simulator)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Simulators.Add(simulator);
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

        public Simulator UpdateSimulator(int id, Simulator simulator)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulatorFind = Context.Simulators.Find(id);
                    if (simulatorFind == null)
                        return null;

                    Context.Entry(simulatorFind).CurrentValues.SetValues(simulator);
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

        public bool DeleteSimulator(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var simulator = GetSimulator(id);
                    Context.Simulators.Remove(simulator);

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

        public List<Simulator> GetSimulatorsByUser(int userId)
        {
            return Context.Simulators
                .Include(x => x.User)
                .OrderBy(x => x.Date)
                .Where(x => x.User.Id == userId)
                .ToList();
        }
    }
}
