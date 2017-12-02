namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System;
    using System.Linq;

    #endregion

    public partial class Repository
    {
        public ContactPerson SaveContactPerson(ContactPerson contactPerson)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.ContactPersons.Add(contactPerson);
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
