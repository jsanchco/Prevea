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
        public List<Clinic> GetClincs()
        {
            return Context.Clinics
                .Include(x => x.RequestMedicalExaminationsEmployees)
                .ToList();
        }

        public Clinic GetClinic(int id)
        {
            return Context.Clinics
                .Include(x => x.RequestMedicalExaminationsEmployees)
                .FirstOrDefault(x => x.Id == id);
        }

        public Clinic SaveClinic(Clinic clinic)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Clinics.AddOrUpdate(clinic);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return clinic;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteClinic(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var clinicFind = Context.Clinics.Find(id);
                    if (clinicFind == null)
                        return false;

                    Context.Clinics.Remove(clinicFind);
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
