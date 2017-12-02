namespace Prevea.Repository.Repository
{
    #region Using

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.Model;

    #endregion

    partial class Repository
    {
        #region Generic

        public List<User> GetUsers()
        {
            return Context.Users
                .Include(x => x.UserState)
                .Include(x => x.UserRoles.Select(y => y.Role))
                .ToList();
        }

        public User GetUser(int id)
        {
            return Context.Users
                .Include(x => x.UserState)
                .Include(x => x.UserRoles.Select(y => y.Role))
                .FirstOrDefault((m => (m.Id == id)));
        }

        public User CreateUser()
        {
            return new User
            {
                Guid = Guid.NewGuid(),
                Password = "123456",
            };
        }

        public User SaveUser(User user)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    if (Context.Users.Any(m => m.Email == user.Email) || Context.Users.Any(m => m.DNI == user.DNI))
                        return null;

                    if (user.Guid == Guid.Empty)
                        user.Guid = Guid.NewGuid();

                    Context.Users.Add(user);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return user;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public User SaveUser(int? roleId, User user)
        {
            if (user.Id != 0)
            {
                return UpdateUser(user.Id, user, roleId);
            }

            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    if (Context.Users.Any(m => m.Email == user.Email) || Context.Users.Any(m => m.DNI == user.DNI))
                        return null;

                    Context.Users.Add(user);
                    Context.SaveChanges();

                    var role = Context.Roles.FirstOrDefault(x => x.Id == roleId);
                    if (role == null)
                    {
                        return null;
                    }

                    Context.UserRoles.Add(new UserRole { Role = role, User = user });
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return user;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public User UpdateUser(int id, User user, int? roleId = null)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var userFind = Context.Users.Find(id);
                    if (userFind?.UserRoles.FirstOrDefault() == null)
                        return null;

                    if (roleId != null && userFind.UserRoles.FirstOrDefault().RoleId != roleId)
                    {
                        var userRole = Context.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
                        if (userRole != null && userRole.RoleId != roleId)
                        {
                            var userRoleFind = Context.UserRoles.Find(userRole.Id);
                            if (userRoleFind != null)
                            {
                                var role = Context.Roles.FirstOrDefault(x => x.Id == roleId);
                                if (role != null)
                                {
                                    userRoleFind.Role = role;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }

                    Context.Entry(userFind).CurrentValues.SetValues(user);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();
                   
                    return GetUser(user.Id);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public User SubscribeUser(int id, bool subscribe)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var userFind = Context.Users.Find(id);
                    if (userFind == null)
                        return null;

                    userFind.UserStateId = subscribe ? (int) EnUserState.Alta : (int) EnUserState.BajaPorAdmin;
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return userFind;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteUser(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var user = Context.Users.Find(id);
                    if (user == null)
                        return false;

                    Context.Users.Remove(user);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    dbContextTransaction.Rollback();

                    return false;
                }
            }
        }

        #endregion

        public User GetUserByGuid(string guid)
        {
            return (Context.Users
                .Include(x => x.UserState)
                .Include(x => x.UserRoles)
                .Include(x => x.UserRoles.Select(y => y.Role))
                .FirstOrDefault((m => (m.Guid.ToString() == guid))));
        }

        public List<string> GetRolesByUser(int id)
        {
            var usersRoles = from ur in Context.UserRoles
                join r in Context.Roles on ur.RoleId equals r.Id
                where (ur.UserId == id)
                select r.Name;

            return (usersRoles.ToList());
        }

        public User ValidateUser(string user, string password)
        {
            return (Context.Users
                .FirstOrDefault((m => (m.Email == user && m.Password == password))));
        }

        public async Task<User> ValidateUserAsync(string user, string password)
        {
            return (await Context.Users.FirstAsync((m => (m.Email == user && m.Password == password))));
        }

        public List<User> GetUsersInRoles(List<string> roles)
        {
            //Context.Configuration.ProxyCreationEnabled = false;

            var users = from ur in Context.UserRoles
                             join r in Context.Roles on ur.RoleId equals r.Id
                             join u in Context.Users on ur.UserId equals u.Id
                        where (roles.Contains(r.Name))
                             select u;

            return users.ToList();
        }

        public List<User> GetContatcPersonsByCompany(int companyId)
        {
            var users = from cp in Context.ContactPersons
                        join c in Context.Companies on cp.CompanyId equals c.Id
                        join u in Context.Users on cp.UserId equals u.Id
                        where (c.Id == companyId)
                        select u;

            return users.ToList();
        }

        public List<User> GetEmployeesByCompany(int companyId)
        {
            var users = from cp in Context.Employees
                join c in Context.Companies on cp.CompanyId equals c.Id
                join u in Context.Users on cp.UserId equals u.Id
                where (c.Id == companyId)
                select u;

            return users.ToList();
        }
    }
}
