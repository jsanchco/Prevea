namespace Prevea.Repository.Repository
{
    #region

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    #endregion

    public partial class Repository
    {
        public MedicalExamination GetMedicalExaminationById(int id)
        {
            return Context.MedicalExaminations
                .Include(x => x.RequestMedicalExaminationEmployee)
                .Include(x => x.RequestMedicalExaminationEmployee.DoctorsMedicalExaminationEmployee)
                .Include(x => x.MedicalExaminationState)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<MedicalExamination> GetMedicalExaminations()
        {
            return Context.MedicalExaminations
                .Include(x => x.RequestMedicalExaminationEmployee)
                .Include(x => x.RequestMedicalExaminationEmployee.DoctorsMedicalExaminationEmployee)
                .Include(x => x.MedicalExaminationState)
                .ToList();
        }

        public MedicalExamination SaveMedicalExamination(MedicalExamination medicalExamination)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    SetFiledsIfNotExist(medicalExamination);

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

        private void SetFiledsIfNotExist(MedicalExamination medicalExamination)
        {
            if (medicalExamination.RequestMedicalExaminationEmployee?.RequestMedicalExaminations?.Company == null)
                return;
            if (medicalExamination.RequestMedicalExaminationEmployee.Clinic == null)
                return;
            var countMedicalExamination = 0;
            if (string.IsNullOrEmpty(medicalExamination.Enrollment))
            {
                countMedicalExamination = Context.MedicalExaminations.Count(x => x.Enrollment != null) + 1;
            }
            else
            {
                if (File.Exists(medicalExamination.Url))
                    File.Delete(medicalExamination.Url);

                var splitEnrollment = medicalExamination.Enrollment.Split('.');
                countMedicalExamination = Convert.ToInt32(splitEnrollment[splitEnrollment.Length - 1]);
            }

            //Company.Clinic.CountMedicalExamination
            medicalExamination.Enrollment = $"RM_{medicalExamination.RequestMedicalExaminationEmployee.RequestMedicalExaminations.Company.Enrollment}." +
                             $"{medicalExamination.RequestMedicalExaminationEmployee.Clinic.Id}." +
                             $"{countMedicalExamination}";
            medicalExamination.Url = $"~/App_Data/Companies/{medicalExamination.RequestMedicalExaminationEmployee.RequestMedicalExaminations.Company.Enrollment}/MedicalExaminations/{medicalExamination.Enrollment}.pdf";
        }
    }
}
