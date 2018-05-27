namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<Mailing> GetMailings()
        {
            return Context.Mailings
                .ToList();
        }

        public Mailing GetMailingById(int id)
        {
            return Context.Mailings
                .FirstOrDefault(x => x.Id == id);
        }

        public Mailing SaveMailing(Mailing mailing)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    if (mailing.Id == 0)
                    {
                        mailing.CreateDate = DateTime.Now;
                        mailing.SendDate = null;
                    }

                    Context.Mailings.AddOrUpdate(mailing);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return mailing;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteMailing(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var mailingFind = Context.Mailings.Find(id);
                    if (mailingFind == null)
                        return false;

                    Context.Mailings.Remove(mailingFind);
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

        public bool DeleteAllDataMails(int mailingId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var mailingFind = Context.Mailings.Find(mailingId);
                    if (mailingFind == null)
                        return false;

                    Context.DataMails.RemoveRange(mailingFind.DataMails);
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
