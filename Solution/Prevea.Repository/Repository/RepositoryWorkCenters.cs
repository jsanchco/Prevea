namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<WorkCenter> GetWorkCenters()
        {
            return Context.WorkCenters
                .Include(x => x.EstablishmentType)
                .ToList();
        }

        public WorkCenter GetWorkCenter(int id)
        {
            return Context.WorkCenters
                .Include(x => x.EstablishmentType)
                .FirstOrDefault(x => x.Id == id);
        }

        public WorkCenter SaveWorkCenter(WorkCenter workCenter)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.WorkCenters.AddOrUpdate(workCenter);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return workCenter;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteWorkCenter(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var workCenterFind = Context.WorkCenters.Find(id);
                    if (workCenterFind == null)
                        return false;

                    Context.WorkCenters.Remove(workCenterFind);
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

        public WorkCenter SubscribeWorkCenter(int id, bool subscribe)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var workCenterFind = Context.WorkCenters.Find(id);
                    if (workCenterFind == null)
                        return null;

                    workCenterFind.WorkCenterStateId = subscribe ? (int)EnWorkCenterState.Alta : (int)EnWorkCenterState.Baja;
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return workCenterFind;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }
    }
}
