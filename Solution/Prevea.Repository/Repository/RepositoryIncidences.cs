namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Data.Entity;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<Incidence> GetIncidences()
        {
            return Context.Incidences
                .Include(x => x.User)
                .Include(x => x.IncidenceState)
                .OrderByDescending(x => x.BeginDate)
                .ToList();
        }

        public List<Incidence> GetIncidencesByUserId(int userId)
        {
            return Context.Incidences
                .Include(x => x.User)
                .Include(x => x.IncidenceState)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.BeginDate)
                .ToList();
        }

        public Incidence GetIncidenceById(int id)
        {
            return Context.Incidences
                .Include(x => x.User)
                .Include(x => x.IncidenceState)
                .FirstOrDefault(x => x.Id == id);
        }

        public Incidence SaveIncidence(Incidence incidence)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Incidences.AddOrUpdate(incidence);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return incidence;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteIncidence(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var incidenceFind = Context.Incidences.Find(id);
                    if (incidenceFind == null)
                        return false;

                    Context.Incidences.Remove(incidenceFind);
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
