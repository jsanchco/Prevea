namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Data.Entity;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<PreventivePlan> GetPreventivePlans()
        {
            return Context.PreventivesPlans
                .Include(x => x.Company)
                .Include(x => x.Document)
                .ToList();
        }

        public List<PreventivePlan> GetPreventivePlans(int userId)
        {
            var user = GetUser(userId);
            var roleId = user.UserRoles.FirstOrDefault()?.RoleId;
            if (roleId == (int) EnRole.Super)
            {
                return Context.PreventivesPlans
                    .Include(x => x.Company)
                    .Include(x => x.Document)
                    .ToList();
            }
            if (roleId == (int)EnRole.PreveaPersonal)
            {
                return Context.PreventivesPlans
                    .Include(x => x.Company)
                    .Include(x => x.Document)
                    .Where(x => x.Company.SimulationCompanies.FirstOrDefault().Simulation.UserAssignedId == userId)
                    .ToList();
            }

            return Context.PreventivesPlans
                .Include(x => x.Company)
                .Include(x => x.Document)
                .Where(x => x.Company.GestorId == userId)
                .ToList();
        }

        public PreventivePlan GetPreventivePlanById(int id)
        {
            return Context.PreventivesPlans
                .Include(x => x.Company)
                .Include(x => x.Document)
                .FirstOrDefault(x => x.Id == id);
        }

        public bool ExistPreventivePlan(int companyId, int documentId)
        {
            return Context.PreventivesPlans.FirstOrDefault(x => x.CompanyId == companyId && x.DocumentId == documentId) != null;
        }

        public PreventivePlan SavePreventivePlan(PreventivePlan preventivePlan)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.PreventivesPlans.AddOrUpdate(preventivePlan);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return preventivePlan;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeletePreventivePlan(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var preventivePlanFind = Context.PreventivesPlans.Find(id);
                    if (preventivePlanFind == null)
                        return false;

                    Context.PreventivesPlans.Remove(preventivePlanFind);
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
