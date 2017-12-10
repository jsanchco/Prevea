namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System.Data.Entity;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public ForeignPreventionService GetForeignPreventionService(int id)
        {
            return Context.ForeignPreventionServices
                .Include(x => x.Simulation)
                .FirstOrDefault(x => x.Id == id);
        }

        public ForeignPreventionService SaveForeignPreventionService(ForeignPreventionService foreignPreventionService)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.ForeignPreventionServices.AddOrUpdate(foreignPreventionService);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return foreignPreventionService;

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
