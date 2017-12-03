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
        public AgencyService GetAgencyService(int id)
        {
            return Context.AgencyServices
                .Include(x => x.Simulator)
                .FirstOrDefault(x => x.Id == id);
        }

        public AgencyService SaveAgencyService(AgencyService agencyService)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.AgencyServices.AddOrUpdate(agencyService);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return agencyService;

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
