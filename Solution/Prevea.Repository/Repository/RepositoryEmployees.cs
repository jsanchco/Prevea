namespace Prevea.Repository.Repository
{
    #region Using

    using Model.Model;
    using System;
    using System.Linq;

    #endregion

    public partial class Repository
    {
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
