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
        public List<DeltaCode> GetDeltaCodes()
        {
            return Context.DeltaCodes
                .ToList();
        }

        public DeltaCode GetDeltaCodeById(int id)
        {
            return Context.DeltaCodes
                .FirstOrDefault(x => x.Id == id);
        }

        public DeltaCode SaveDeltaCode(DeltaCode deltaCode)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    if (deltaCode.Id == 0)
                    {
                        deltaCode.Id = Context.DeltaCodes.Max(x => x.Id) + 1;
                    }
                    Context.DeltaCodes.AddOrUpdate(deltaCode);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return deltaCode;

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }
        }

        public bool DeleteDeltaCode(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var deltaCodeFind = Context.DeltaCodes.Find(id);
                    if (deltaCodeFind == null)
                        return false;

                    Context.DeltaCodes.Remove(deltaCodeFind);
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
