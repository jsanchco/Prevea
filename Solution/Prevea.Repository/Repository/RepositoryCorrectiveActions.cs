namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Prevea.Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<CorrectiveAction> GetCorrectiveActions()
        {
            return Context.CorrectiveActions
                .Include(x => x.PriorityCorrectiveAction)
                .ToList();
        }

        public CorrectiveAction GetCorrectiveActionById(int id)
        {
            return Context.CorrectiveActions
                .Include(x => x.PriorityCorrectiveAction)
                .FirstOrDefault(x => x.Id == id);
        }

        public CorrectiveAction SaveCorrectiveAction(CorrectiveAction correctiveAction)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.CorrectiveActions.AddOrUpdate(correctiveAction);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return correctiveAction;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteCorrectiveAction(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var correctiveActionFind = Context.CorrectiveActions.Find(id);
                    if (correctiveActionFind == null)
                        return false;

                    Context.CorrectiveActions.Remove(correctiveActionFind);
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
