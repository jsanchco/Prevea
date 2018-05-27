namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public List<DataMail> GetDataMails()
        {
            return Context.DataMails
                .Include(x => x.Mailing)
                .Include(x => x.Creator)
                .Include(x => x.DataMailState)
                .ToList();
        }

        public List<DataMail> GetDataMailsByMailing(int mailingId)
        {
            return Context.DataMails
                .Include(x => x.Mailing)
                .Include(x => x.Creator)
                .Include(x => x.DataMailState)
                .Where(x => x.MailingId == mailingId)
                .ToList();
        }

        public DataMail GetDataMailById(int id)
        {
            return Context.DataMails
                .Include(x => x.Mailing)
                .Include(x => x.Creator)
                .Include(x => x.DataMailState)
                .FirstOrDefault(x => x.Id == id);
        }

        public DataMail SaveDataMail(DataMail dataMail)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.DataMails.AddOrUpdate(dataMail);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return dataMail;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteDataMail(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var dataMailFind = Context.DataMails.Find(id);
                    if (dataMailFind == null)
                        return false;

                    Context.DataMails.Remove(dataMailFind);
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
