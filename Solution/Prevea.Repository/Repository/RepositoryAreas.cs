namespace Prevea.Repository.Repository
{
    #region Using 

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model.Model;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        #region Generic

        public List<Area> GetAreas()
        {
            return Context.Areas
                .Include(x => x.Entity)
                .Include(x => x.Documents)
                .ToList();
        }

        public Area GetArea(int id)
        {
            return Context.Areas
                .Include(x => x.Documents)
                .FirstOrDefault(m => m.Id == id);
        }

        public Area SaveArea(Area area)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Areas.Add(area);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return area;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Area UpdateArea(int id, Area area)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var areaFind = Context.Areas.Find(id);
                    if (areaFind == null)
                        return null;

                    Context.Entry(areaFind).CurrentValues.SetValues(area);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return area;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public void DeleteArea(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var areaFind = Context.Areas.Find(id);
                    if (areaFind == null)
                        return;

                    Context.Areas.Remove(areaFind);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        #endregion

        public List<Area> GetAreasByEntity(int entityId)
        {
            return Context.Areas
                .Include(x => x.Entity)
                .Include(x => x.Documents)
                .Where(x => x.Entity.Id == entityId)
                .ToList();
        }
    }
}
