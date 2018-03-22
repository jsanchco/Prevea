namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;

    #endregion

    public partial class Repository
    {
        public List<DoctorMedicalExaminationEmployee> GetDoctorsMedicalExaminationEmployees()
        {
            return Context.DoctorMedicalExaminationEmployees
                .Include(x => x.MedicalExaminationEmployee)
                .ToList();
        }

        public DoctorMedicalExaminationEmployee GetDoctorMedicalExaminationEmployeeById(int id)
        {
            return Context.DoctorMedicalExaminationEmployees
                .Include(x => x.MedicalExaminationEmployee)
                .FirstOrDefault(x => x.Id == id);
        }

        public DoctorMedicalExaminationEmployee GetDoctorMedicalExaminationEmployeeByDoctorId(int medicalExaminationEmployeeId,
            int doctorId)
        {
            return Context.DoctorMedicalExaminationEmployees
                .Include(x => x.MedicalExaminationEmployee)
                .FirstOrDefault(x => x.MedicalExaminationEmployeeId == medicalExaminationEmployeeId && x.DoctorId == doctorId);
        }

        public DoctorMedicalExaminationEmployee SaveDoctorMedicalExaminationEmployee(DoctorMedicalExaminationEmployee doctorMedicalExaminationEmployee)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.DoctorMedicalExaminationEmployees.Add(doctorMedicalExaminationEmployee);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return doctorMedicalExaminationEmployee;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteDoctorMedicalExaminationEmployee(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var doctorMedicalExaminationEmployee = GetDoctorMedicalExaminationEmployeeById(id);
                    Context.
                        DoctorMedicalExaminationEmployees.Remove(doctorMedicalExaminationEmployee);

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

        public List<DateTime> GetDatesByWorkSheet(int doctorId)
        {
            return Context.RequestMedicalExaminationsEmployees
                .Where(x => x.DoctorsMedicalExaminationEmployee.Select(y => y.DoctorId).Contains(doctorId))
                .Select(x => x.DateOnlyDay)
                .Distinct()
                .ToList();
        }
    }
}
