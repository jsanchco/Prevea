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
        public RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeById(int id)
        {
            return Context.RequestMedicalExaminationsEmployees
                .Include(x => x.Employee)
                .Include(x => x.RequestMedicalExaminations)
                .FirstOrDefault(x => x.Id == id);
        }

        public RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeByEmployeeId(int requestMedicalExaminationsId,
            int employeeId)
        {
            return Context.RequestMedicalExaminationsEmployees
                .Include(x => x.Employee)
                .Include(x => x.RequestMedicalExaminations)
                .FirstOrDefault(x => x.EmployeeId == employeeId && x.RequestMedicalExaminationsId == requestMedicalExaminationsId);
        }

        public List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployees()
        {
            return Context.RequestMedicalExaminationsEmployees
                .Include(x => x.Employee)
                .Include(x => x.RequestMedicalExaminations)
                .ToList();
        }

        public List<Employee> GetEmployeesByRequestMedicalExamination(int requestMedicalExaminationId)
        {
            return  GetRequestMedicalExaminationEmployees()
                .Where(x => x.RequestMedicalExaminationsId == requestMedicalExaminationId)
                .Select(x => x.Employee)
                .ToList();
        }

        public RequestMedicalExaminationEmployee SaveRequestMedicalExaminationEmployee(
            RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.RequestMedicalExaminationsEmployees.AddOrUpdate(requestMedicalExaminationEmployee);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return requestMedicalExaminationEmployee;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteRequestMedicalExaminationEmployee(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var requestMedicalExaminationEmployee = GetRequestMedicalExaminationEmployeeById(id);
                    Context.RequestMedicalExaminationsEmployees.Remove(requestMedicalExaminationEmployee);

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
