namespace Prevea.Repository.Repository
{
    #region Using

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Model.Model;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        #region Generic

        public List<Entity> GetEntities()
        {
            return Context.Entities
                .Include(x => x.Areas)
                .ToList();
        }

        public Entity GetEntity(int id)
        {
            return Context.Entities
                .Include(x => x.Areas)
                .FirstOrDefault(m => m.Id == id);
        }

        public Entity SaveEntity(Entity entity)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Entities.Add(entity);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return entity;

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }
        }

        public Entity UpdateEntity(int id, Entity entity)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var entityFind = Context.Entities.Find(id);
                    if (entityFind == null)
                        return null;

                    Context.Entry(entityFind).CurrentValues.SetValues(entity);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return entity;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }
        }

        public void DeleteEntity(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var entityFind = Context.Entities.Find(id);
                    if (entityFind == null)
                        return;

                    Context.Entities.Remove(entityFind);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

        #endregion
    }
}
