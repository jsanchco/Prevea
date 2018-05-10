using System.Data.Entity.Migrations;

namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Data.Entity;
    using System.Linq;
    using System;

    #endregion

    public partial class Repository
    {
        public List<Sector> GetSectors()
        {
            return Context.Sectors
                .Include(x => x.WorkStations)
                .ToList();
        }

        public Sector GetSectorById(int id)
        {
            return Context.Sectors
                .Include(x => x.WorkStations)
                .FirstOrDefault(x => x.Id == id);
        }

        public Sector SaveSector(Sector sector)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Sectors.AddOrUpdate(sector);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return sector;

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }
        }

        public bool DeleteSector(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var sectorFind = Context.Sectors.Find(id);
                    if (sectorFind == null)
                        return false;

                    Context.Sectors.Remove(sectorFind);
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
