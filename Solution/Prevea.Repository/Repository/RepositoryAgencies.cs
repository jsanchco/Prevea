using System;

namespace Prevea.Repository.Repository
{
    #region

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    partial class Repository
    {
        public List<Agency> GetAgencies()
        {
            return Context.Agencies
                .Include(x => x.Companies)
                .ToList();
        }

        public Agency GetAgency(int id)
        {
            return Context.Agencies
                .Include(x => x.Companies)
                .FirstOrDefault(x => x.Id == id);
        }

        public Agency SaveAgency(Agency agency, int companyId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Agencies.Add(agency);
                    Context.SaveChanges();

                    var company = Context.Companies.Find(companyId);
                    if (company != null)
                    {
                        company.AgencyId = agency.Id;
                        Context.SaveChanges();
                    }

                    dbContextTransaction.Commit();

                    return agency;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Agency UpdateAgency(int id, Agency agency)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var agencyFind = Context.Agencies.Find(id);
                    if (agencyFind == null)
                        return null;

                    Context.Entry(agencyFind).CurrentValues.SetValues(agency);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return agency;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteAgency(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var agencyFind = Context.Agencies.Find(id);
                    if (agencyFind == null)
                        return false;

                    Context.Agencies.Remove(agencyFind);
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
