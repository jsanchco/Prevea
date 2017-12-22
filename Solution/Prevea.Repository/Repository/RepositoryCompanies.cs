namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        #region Generic

        public Company GetCompany(int id)
        {
            return Context.Companies
                .Include(x => x.Cnae)
                .Include(x => x.Agency)
                .Include(x => x.ContactPersons)
                .Include(x => x.SimulationCompanies)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Company> GetCompanies()
        {
            return Context.Companies
                .Include(x => x.Cnae)
                .Include(x => x.Agency)
                .Include(x => x.ContactPersons)
                .ToList();
        }

        public Company SaveCompany(Company company)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Companies.Add(company);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return company;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Company UpdateCompany(int id, Company company)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var companyFind = Context.Companies.Find(id);
                    if (companyFind == null)
                        return null;

                    Context.Entry(companyFind).CurrentValues.SetValues(company);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return company;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteCompany(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var company = GetCompany(id);
                    Context.Companies.Remove(company);

                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return true;
                }
            }
        }

        public List<Company> GetCompaniesByAgency(int agencyId)
        {
            return Context.Companies
                .Include(x => x.Agency)
                .Where(x => x.Agency.Id == agencyId)
                .ToList();
        }

        #endregion

        public List<Company> GetCompaniesByUser(int userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);

            var userRole = user?.UserRoles.FirstOrDefault();
            if (userRole == null)
                return null;
            switch (userRole.RoleId)
            {
                case (int)EnRole.Super:
                    return Context.Companies
                        .Include(x => x.Cnae)
                        .Include(x => x.Agency)
                        .Include(x => x.ContactPersons)
                        .Include(x => x.SimulationCompanies)
                        .OrderByDescending(x => x.SimulationCompanies.FirstOrDefault().Simulation.Date)
                        .ToList();

                case (int)EnRole.PreveaPersonal:
                    return Context.Companies
                        .Include(x => x.Cnae)
                        .Include(x => x.Agency)
                        .Include(x => x.ContactPersons)
                        .Include(x => x.SimulationCompanies)
                        .Where(x => x.SimulationCompanies.FirstOrDefault().Simulation.UserAssignedId == userId)
                        .OrderByDescending(x => x.SimulationCompanies.FirstOrDefault().Simulation.Date)
                        .ToList();

                case (int)EnRole.PreveaCommercial:
                    return Context.Companies
                        .Include(x => x.Cnae)
                        .Include(x => x.Agency)
                        .Include(x => x.ContactPersons)
                        .Include(x => x.SimulationCompanies)
                        .Where(x => x.SimulationCompanies.FirstOrDefault().Simulation.UserId == userId)
                        .OrderByDescending(x => x.SimulationCompanies.FirstOrDefault().Simulation.Date)
                        .ToList();
                default:
                    return null;
            }
        }
    }
}
