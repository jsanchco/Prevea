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
        public TrainingService GetTrainingService(int id)
        {
            return Context.TrainingServices
                .Include(x => x.Simulator)
                .FirstOrDefault(x => x.Id == id);
        }

        public TrainingService SaveTrainingService(TrainingService trainingService)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.TrainingServices.AddOrUpdate(trainingService);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return trainingService;

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
