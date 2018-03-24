namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public RequestMedicalExaminations GetRequestMedicalExaminationById(int id)
        {
            return Context.RequestMedicalExaminations                
                .Include(x => x.RequestMedicalExaminationEmployees)
                .Include(x => x.RequestMedicalExaminationState)
                .Include(x => x.Company)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<RequestMedicalExaminations> GetRequestMedicalExaminations()
        {
            return Context.RequestMedicalExaminations
                .Include(x => x.RequestMedicalExaminationEmployees)
                .Include(x => x.RequestMedicalExaminationState)
                .Include(x => x.Company)
                .ToList();
        }

        public List<RequestMedicalExaminations> GetRequestMedicalExaminationsByCompany(int companyId)
        {
            return Context.RequestMedicalExaminations
                .Include(x => x.RequestMedicalExaminationEmployees)
                .Include(x => x.RequestMedicalExaminationState)
                .Include(x => x.Company)
                .Where(x => x.CompanyId == companyId)
                .ToList();
        }


        public RequestMedicalExaminations SaveRequestMedicalExaminations(RequestMedicalExaminations requestMedicalExamination)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.RequestMedicalExaminations.AddOrUpdate(requestMedicalExamination);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return requestMedicalExamination;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteRequestMedicalExamination(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var requestMedicalExamination = Context.RequestMedicalExaminations.Find(id);
                    if (requestMedicalExamination == null)
                        return false;

                    foreach (var requestMedicalExaminationEmployee in requestMedicalExamination.RequestMedicalExaminationEmployees)
                    {
                        var medicalExaminationFind = GetMedicalExaminationById(requestMedicalExaminationEmployee.Id);
                        Context.MedicalExaminations.Remove(medicalExaminationFind);

                        //var requestMedicalExaminationEmployeeFind = GetRequestMedicalExaminationEmployeeById(requestMedicalExaminationEmployee.Id);
                        //Context.RequestMedicalExaminationsEmployees.Remove(requestMedicalExaminationEmployeeFind);
                    }

                    Context.RequestMedicalExaminations.Remove(requestMedicalExamination);
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
