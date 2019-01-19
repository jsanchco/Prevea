namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<ContactPerson> GetContactPersons()
        {
            return Context.ContactPersons
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.ContactPersonType)
                .ToList();
        }

        public ContactPerson GetContactPersonById(int contactPersonId)
        {
            return Context.ContactPersons
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.ContactPersonType)
                .FirstOrDefault(x => x.Id == contactPersonId);
        }

        public ContactPerson GetContactPersonByUserId(int userId)
        {
            return Context.ContactPersons
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.ContactPersonType)
                .FirstOrDefault(x => x.UserId == userId);
        }

        public ContactPerson SaveContactPerson(ContactPerson contactPerson)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.ContactPersons.AddOrUpdate(contactPerson);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return contactPerson;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteContactPerson(ContactPerson contactPerson)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var contactPersonFind = Context.ContactPersons.FirstOrDefault(x => x.CompanyId == contactPerson.CompanyId && x.UserId == contactPerson.UserId);

                    if (contactPersonFind == null)
                        return false;

                    Context.ContactPersons.Remove(contactPersonFind);
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
