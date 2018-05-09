namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public List<Employee> GetEmployees()
        {
            return Context.Employees
                .Include(x => x.User)
                .Include(x => x.Company)
                .ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return Context.Employees
                .Include(x => x.User)
                .Include(x => x.Company)
                .FirstOrDefault(x => x.Id == id);
        }

        public Employee GetEmployeeByUser(int userId)
        {
            return Context.Employees
                .Include(x => x.User)
                .Include(x => x.Company)
                .FirstOrDefault(x => x.UserId == userId);
        }

        public List<Employee> GetEmployeesByCompany(int companyId)
        {
            return Context.Employees.Where(x => x.CompanyId == companyId)
                .Include(x => x.User)
                .Include(x => x.Company)
                .ToList();
        }

        public Employee SaveEmployee(Employee employee)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Employees.Add(employee);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return employee;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteEmployee(Employee employee)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var employeeFind = Context.Employees.FirstOrDefault(x => x.CompanyId == employee.CompanyId && x.UserId == employee.UserId);

                    if (employeeFind == null)
                        return false;

                    Context.Employees.Remove(employeeFind);
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
