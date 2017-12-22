namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System;
    using IService.IService;

    #endregion

    public partial class Service
    {
        #region Generic

        public Company GetCompany(int id)
        {
            return Repository.GetCompany(id);
        }

        public List<Company> GetCompanies()
        {
            return Repository.GetCompanies();
        }

        public Result SaveCompany(Company company)
        {
            try
            {
                company = Repository.SaveCompany(company);

                if (company == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Empresa",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Empresa se ha producido con éxito",
                    Object = company,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Empresa",
                    Object = company,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateCompany(int id, Company company)
        {
            try
            {
                company = Repository.UpdateCompany(id, company);

                if (company == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Documento se ha producido con éxito",
                    Object = company,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Documento",
                    Object = company,
                    Status = Status.Error
                };
            }
        }

        public bool DeleteCompany(int id)
        {
            return Repository.DeleteCompany(id);
        }

        #endregion

        public Result SubscribeCompany(int companyId, bool subscribe)
        {
            try
            {
                var company = Repository.GetCompany(companyId);

                if (company == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Compañia",
                        Object = null,
                        Status = Status.Error
                    };
                }

                company.CompanyStateId = subscribe ? (int) EnCompanyState.Alta : (int) EnCompanyState.BajaPorAdmin;
                company = Repository.UpdateCompany(companyId, company);

                if (company == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Compañia",
                        Object = null,
                        Status = Status.Error
                    };
                }

                foreach (var employee in company.Employees)
                {
                    var employeeFind = Repository.GetUser(employee.UserId);
                    employeeFind.UserStateId = subscribe ? (int)EnUserState.Alta : (int)EnUserState.BajaPorAdmin;
                    employeeFind = Repository.UpdateUser(employee.UserId, employeeFind);

                    if (employeeFind == null)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error en la Grabación de la Compañia",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                }

                foreach (var contactPerson in company.ContactPersons)
                {
                    var contactPersonFind = Repository.GetUser(contactPerson.UserId);
                    contactPersonFind.UserStateId = subscribe ? (int)EnUserState.Alta : (int)EnUserState.BajaPorAdmin;
                    contactPersonFind = Repository.UpdateUser(contactPerson.UserId, contactPersonFind);

                    if (contactPersonFind == null)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error en la Grabación de la Compañia",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                }

                return new Result
                {
                    Message = "La Grabación de la Compañia se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del la Compañia",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public List<Company> GetCompaniesByUser(int userId)
        {
            return Repository.GetCompaniesByUser(userId);
        }
    }
}
