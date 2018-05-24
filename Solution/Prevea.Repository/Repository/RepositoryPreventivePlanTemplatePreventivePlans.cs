namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<PreventivePlanTemplatePreventivePlan> GetPreventivePlanTemplatePreventivePlans()
        {
            return Context.PreventivePlanTemplatePreventivePlans
                    .Include(x => x.PreventivePlan)
                    .Include(x => x.TemplatePreventivePlan)
                    .ToList();
        }

        public List<PreventivePlanTemplatePreventivePlan> GetPreventivePlanTemplatePreventivePlansByPreventivePlanId(int preventivePlanId)
        {
            return Context.PreventivePlanTemplatePreventivePlans
                    .Where(x => x.PreventivePlanId == preventivePlanId)
                    .Include(x => x.PreventivePlan)
                    .Include(x => x.TemplatePreventivePlan)
                    .ToList();
        }

        public PreventivePlanTemplatePreventivePlan ExistPreventivePlanTemplatePreventivePlan(int preventivePlanId, int templatePreventivePlanId)
        {
            return Context.PreventivePlanTemplatePreventivePlans
                .Include(x => x.PreventivePlan)
                .Include(x => x.TemplatePreventivePlan)
                .FirstOrDefault(x => x.PreventivePlanId == preventivePlanId && x.TemplatePreventivePlanId == templatePreventivePlanId);
        }

        public PreventivePlanTemplatePreventivePlan GetPreventivePlanTemplatePreventivePlanById(int id)
        {
            return Context.PreventivePlanTemplatePreventivePlans
                .Include(x => x.PreventivePlan)
                .Include(x => x.TemplatePreventivePlan)
                .FirstOrDefault(x => x.Id == id);
        }

        public PreventivePlanTemplatePreventivePlan SavePreventivePlanTemplatePreventivePlan(
            PreventivePlanTemplatePreventivePlan preventivePlanTemplatePreventivePlan)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.PreventivePlanTemplatePreventivePlans.AddOrUpdate(preventivePlanTemplatePreventivePlan);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return preventivePlanTemplatePreventivePlan;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeletePreventivePlanTemplatePreventivePlan(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var preventivePlanTemplatePreventivePlanFind = Context.PreventivePlanTemplatePreventivePlans.Find(id);
                    if (preventivePlanTemplatePreventivePlanFind == null)
                        return false;

                    Context.PreventivePlanTemplatePreventivePlans.Remove(preventivePlanTemplatePreventivePlanFind);
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
