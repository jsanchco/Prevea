using System;
using System.Data.Entity.Migrations;

namespace Prevea.Repository.Repository
{
    #region

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public MedicalExamination GetMedicalExaminationById(int id)
        {
            return Context.MedicalExaminations
                .Include(x => x.RequestMedicalExaminationEmployee)
                .Include(x => x.MedicalExaminationState)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<MedicalExamination> GetMedicalExaminations()
        {
            return Context.MedicalExaminations
                .Include(x => x.RequestMedicalExaminationEmployee)
                .Include(x => x.MedicalExaminationState)
                .ToList();
        }

        public MedicalExamination SaveMedicalExamination(MedicalExamination medicalExamination)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.MedicalExaminations.AddOrUpdate(medicalExamination);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return medicalExamination;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteMedicalExamination(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var medicalExamination = GetMedicalExaminationById(id);
                    Context.MedicalExaminations.Remove(medicalExamination);

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
