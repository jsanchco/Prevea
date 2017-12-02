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
        public EconomicData GetEconomicData(int economicDataId)
        {
            return Context.EconomicsDatas
                .Include(x => x.Company)
                .FirstOrDefault(x => x.Id == economicDataId);
        }

        public EconomicData SaveEconomicData(EconomicData economicData)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.EconomicsDatas.AddOrUpdate(economicData);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return economicData;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public EconomicData UpdateEconomicData(int economicDataId, EconomicData economicData)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteEconomicData(int economicDataId)
        {
            throw new System.NotImplementedException();
        }
    }
}
