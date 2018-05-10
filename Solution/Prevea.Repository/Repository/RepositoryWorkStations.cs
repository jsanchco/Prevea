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
        public List<WorkStation> GetWorkStations()
        {
            return Context.WorkStations
                .Include(x => x.Sector)
                .ToList();
        }

        public List<WorkStation> GetWorkStationsBySectorId(int sectorId)
        {
            return Context.WorkStations
                .Include(x => x.Sector)
                .Where(x => x.SectorId == sectorId)
                .ToList();
        }

        public WorkStation GetWorkStationById(int id)
        {
            return Context.WorkStations
                .Include(x => x.Sector)
                .FirstOrDefault(x => x.Id == id);
        }

        public WorkStation SaveWorkStation(WorkStation workStation)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.WorkStations.AddOrUpdate(workStation);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return workStation;

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }
        }

        public bool DeleteWorkStation(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var workStationFind = Context.WorkStations.Find(id);
                    if (workStationFind == null)
                        return false;

                    Context.WorkStations.Remove(workStationFind);
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
