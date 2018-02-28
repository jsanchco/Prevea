namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System;
    using IService.IService;
    using Model.CustomModel;

    #endregion

    public partial class Service
    {
        public User ValidateUser(string user, string password)
        {
            return Repository.ValidateUser(user, password);
        }

        public List<string> GetRolesByUser(int id)
        {
            return Repository.GetRolesByUser(id);
        }

        public User GetUser(int id)
        {
            return Repository.GetUser(id);
        }

        public List<User> GetUsers()
        {
            return Repository.GetUsers();
        }

        public ContactPerson GetContactPersonById(int contactPersonId)
        {
            return Repository.GetContactPersonById(contactPersonId);
        }

        public ContactPerson GetContactPersonByUserId(int userId)
        {
            return Repository.GetContactPersons()
                .FirstOrDefault(x => x.UserId == userId);
        }

        public List<User> GetContactPersonsByCompany(int companyId)
        {
            return Repository.GetContatcPersonsByCompany(companyId);
        }

        public List<User> GetEmployeesByCompany(int companyId)
        {
            return Repository.GetEmployeesByCompany(companyId);
        }

        public List<User> GetUsersByUser(int id)
        {
            return Repository.GetUsersByUser(id);
        }

        public Result SaveUser(int? roleId, User user)
        {
            try
            {
                user = Repository.SaveUser(roleId, user);

                if (user == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Persona de Contacto",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Persona de Contacto se ha producido con éxito",
                    Object = user,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Persona de Contacto",
                    Object = user,
                    Status = Status.Error
                };
            }
        }

        public Result SaveContactPersonCompany(int roleId, int companyId, User user)
        {
            try
            {
                var isUpdate = user.Id != 0;

                user = Repository.SaveUser(roleId, user);

                if (user == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Persona de Contacto",
                        Object = null,
                        Status = Status.Error
                    };
                }

                if (isUpdate)
                {
                    return new Result
                    {
                        Message = "La Grabación de la Persona de Contacto se ha producido con éxito",
                        Object = user,
                        Status = Status.Ok
                    };
                }

                var contactPerson = new ContactPerson
                {
                    UserId = user.Id,
                    CompanyId = companyId
                };
                contactPerson = Repository.SaveContactPerson(contactPerson);
                if (contactPerson == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Persona de Contacto",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Persona de Contacto se ha producido con éxito",
                    Object = user,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Persona de Contacto",
                    Object = user,
                    Status = Status.Error
                };
            }
        }

        public Result SaveEmployeeCompany(int roleId, int companyId, User user)
        {
            try
            {
                var isUpdate = user.Id != 0;

                user = Repository.SaveUser(roleId, user);

                if (user == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Trabajador",
                        Object = null,
                        Status = Status.Error
                    };
                }

                if (isUpdate)
                {
                    return new Result
                    {
                        Message = "La Grabación del Trabajador se ha producido con éxito",
                        Object = user,
                        Status = Status.Ok
                    };
                }

                var employee = new Employee
                {
                    UserId = user.Id,
                    CompanyId = companyId
                };
                employee = Repository.SaveEmployee(employee);
                if (employee == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Trabajador",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Trabajador se ha producido con éxito",
                    Object = user,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Trabajador",
                    Object = user,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteContactPersonCompany(int companyId, int userId)
        {
            try
            {
                var result = Repository.DeleteContactPerson(new ContactPerson { CompanyId = companyId, UserId = userId });
                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Persona de Contacto",
                        Object = null,
                        Status = Status.Error
                    };
                }

                result = Repository.DeleteUser(userId);
                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Persona de Contacto",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la Persona de Contacto se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la Persona de Contacto",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteEmployeeCompany(int companyId, int userId)
        {
            try
            {
                var result = Repository.DeleteEmployee(new Employee { CompanyId = companyId, UserId = userId });
                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar al Trabajador",
                        Object = null,
                        Status = Status.Error
                    };
                }

                result = Repository.DeleteUser(userId);
                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar al Trabajador",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Trabajador se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar al Trabajador",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result SubscribeContactPersonCompany(int companyId, int userId, bool subscribe)
        {
            try
            {
                var result = Repository.SubscribeUser(userId, subscribe);
                if (result == null)
                {
                    return new Result
                    {
                        Message = subscribe ? "Se ha producido un error al Dar de Alta la Persona de Contacto" : "Se ha producido un error al Dar de Baja la Persona de Contacto",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = subscribe ? "Dar de Alta a la Persona de Contacto se ha producido con éxito" : "Dar de Baja a la Persona de Contacto se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = subscribe ? "Se ha producido un error al Dar de Alta la Persona de Contacto" : "Se ha producido un error al Dar de Baja la Persona de Contacto",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result SubscribeEmployeeCompany(int companyId, int userId, bool subscribe)
        {
            try
            {
                var result = Repository.SubscribeUser(userId, subscribe);
                if (result == null)
                {
                    return new Result
                    {
                        Message = subscribe ? "Se ha producido un error al Dar de Alta al Trabajador" : "Se ha producido un error al Dar de Baja al Trabajador",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {                    
                    Message = subscribe ? "Dar de Alta al Trabajador se ha producido con éxito" : "Dar de Baja al Trabajador se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = subscribe ? "Se ha producido un error al Dar de Alta al Trabajador" : "Se ha producido un error al Dar de Baja al Trabajador",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteUser(int id)
        {
            try
            {
                var result = Repository.DeleteUser(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Usuario",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Usuario se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el Usuario",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result SubscribeUser(int userId, bool subscribe)
        {
            try
            {
                var result = Repository.SubscribeUser(userId, subscribe);
                if (result == null)
                {
                    return new Result
                    {
                        Message = subscribe ? "Se ha producido un error al Dar de Alta al Usuario" : "Se ha producido un error al Dar de Baja al Usuario",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = subscribe ? "Dar de Alta al Usuario se ha producido con éxito" : "Dar de Baja al Usuario se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = subscribe ? "Se ha producido un error al Dar de Alta al Usuario" : "Se ha producido un error al Dar de Baja al Usuario",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public List<CustomRole> GetCustomRoles(List<int> listRoles)
        {
            return Repository.GetCustomRoles(listRoles);
        }

        public List<User> GetUsersInRoles(List<string> roles)
        {
            return Repository.GetUsersInRoles(roles).ToList();
        }
    }
}
