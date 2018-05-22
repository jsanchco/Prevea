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
        public List<TemplatePreventivePlan> GetTemplatePreventivePlans()
        {
            return Context.TemplatePreventivePlans
                .ToList();
        }

        public TemplatePreventivePlan GetTemplatePreventivePlanById(int id)
        {
            return Context.TemplatePreventivePlans
                .FirstOrDefault(x => x.Id == id);
        }

        public TemplatePreventivePlan SaveTemplatePreventivePlan(TemplatePreventivePlan templatePreventivePlan)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.TemplatePreventivePlans.AddOrUpdate(templatePreventivePlan);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return templatePreventivePlan;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteTemplatePreventivePlan(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var templatePreventivePlanFind = Context.TemplatePreventivePlans.Find(id);
                    if (templatePreventivePlanFind == null)
                        return false;

                    Context.TemplatePreventivePlans.Remove(templatePreventivePlanFind);
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
