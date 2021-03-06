﻿namespace Prevea.Repository.Repository
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

        public List<DateTime?> GetDatesByWorkSheet(int doctorId)
        {
            return Context.RequestMedicalExaminationsEmployees
                .Where(x => x.DoctorsMedicalExaminationEmployee.Select(y => y.DoctorId).Contains(doctorId))
                .Select(x => DbFunctions.TruncateTime(x.Date))
                .Distinct()
                .OrderByDescending(x => x.Value)
                .ToList();
        }

        public int GetCountMedicalExaminationByState(int doctorId, DateTime date, EnDocumentState medicalExaminationState)
        {
            var requestMedicalExaminationsEmployeesByDoctorId = GetRequestMedicalExaminationEmployeesByDate(doctorId, date);
            var cont = 0;
            switch (medicalExaminationState)
            {
                case EnDocumentState.Pending:
                    cont = 0;
                    foreach (var requestMedicalExamination in requestMedicalExaminationsEmployeesByDoctorId)
                    {
                        var document =
                            requestMedicalExamination.MedicalExaminationDocuments.FirstOrDefault(x =>
                                x.Document.AreaId == 16);
                        if (document != null && document.Document.DocumentStateId == (int) EnDocumentState.Pending)
                            cont++;
                        if (document == null)
                            cont++;                        
                    }

                    return cont;

                case EnDocumentState.InProcess:
                    cont = 0;
                    foreach (var requestMedicalExamination in requestMedicalExaminationsEmployeesByDoctorId)
                    {
                        var document =
                            requestMedicalExamination.MedicalExaminationDocuments.FirstOrDefault(x =>
                                x.Document.AreaId == 16);
                        if (document != null && document.Document.DocumentStateId == (int)EnDocumentState.InProcess)
                            cont++;
                    }

                    return cont;

                case EnDocumentState.Finished:
                    cont = 0;
                    foreach (var requestMedicalExamination in requestMedicalExaminationsEmployeesByDoctorId)
                    {
                        var document =
                            requestMedicalExamination.MedicalExaminationDocuments.FirstOrDefault(x =>
                                x.Document.AreaId == 16);
                        if (document != null && document.Document.DocumentStateId == (int)EnDocumentState.Finished)
                            cont++;
                    }

                    return cont;
            }

            return cont;
        }
    }
}
