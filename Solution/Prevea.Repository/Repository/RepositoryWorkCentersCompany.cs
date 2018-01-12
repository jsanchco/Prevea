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
        public List<WorkCenter> GetWorkCentersByCompany(int companyId)
        {
            return Context.WorkCentersCompany.Where(x => x.CompanyId == companyId).Select(x => x.WorkCenter)
                .Include(x => x.EstablishmentType)
                .ToList();
        }

        public WorkCenterCompany GetWorkCenterCompanyByWorkCenter(int workCenterId)
        {
            return Context.WorkCentersCompany.FirstOrDefault(x => x.WorkCenterId == workCenterId);
        }

        public WorkCenterCompany SaveWorkCenterCompany(WorkCenterCompany workCenterCompany)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.WorkCentersCompany.AddOrUpdate(workCenterCompany);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return workCenterCompany;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteWorkCenterCompany(int workCenterId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var workCenterCompanyFind = Context.WorkCentersCompany.Find(workCenterId);

                    if (workCenterCompanyFind == null)
                        return false;

                    Context.WorkCentersCompany.Remove(workCenterCompanyFind);
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
