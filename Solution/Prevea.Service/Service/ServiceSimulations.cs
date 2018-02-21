namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System;
    using IService.IService;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public Simulation GetSimulation(int simulationId)
        {
            return Repository.GetSimulation(simulationId);
        }

        public List<Simulation> GetSimulations()
        {
            return Repository.GetSimulations();
        }

        public Result SaveSimulation(Simulation simulation, int? companyId = null)
        {
            try
            {
                simulation = Repository.SaveSimulation(simulation);                

                if (simulation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var simulationCompany = Repository.SaveSimulationCompany(simulation.Id, companyId);

                if (simulationCompany == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Simulación se ha producido con éxito",
                    Object = simulation,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Simulación",
                    Object = simulation,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateSimulation(int simulationId, Simulation simulation)
        {
            try
            {
                simulation = Repository.UpdateSimulation(simulationId, simulation);

                if (simulation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la Simulación se ha producido con éxito",
                    Object = simulation,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Simulación",
                    Object = simulation,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteSimulation(int simulationId)
        {
            try
            {
                var result = Repository.DeleteSimulation(simulationId);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la Simulación se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la Simulación",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result SubscribeSimulation(int simulationId, bool subscribe)
        {
            try
            {
                var result = Repository.SubscribeSimulation(simulationId, subscribe);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al dar de Baja la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "Dar de Baja a la Simulación se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al dar de Baja la Simulación",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public List<Simulation> GetSimulationsByUser(int userId)
        {
            return Repository.GetSimulationByUser(userId);
        }

        public List<Simulation> GetSimulationsOriginalsByUser(int userId)
        {
            return GetSimulationsByUser(userId).Where(x => x.Original).ToList();
        }

        public List<Simulation> GetSimulationsChildrenByUser(int userId, int simulationParentId)
        {
            return GetSimulationsByUser(userId).Where(x => x.SimulationParentId == simulationParentId).ToList();
        }

        public List<Simulation> GetSimulationsByCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public Result SendToCompanies(int simulationId)
        {
            try
            {
                var random = new Random();

                var simulation = Repository.GetSimulation(simulationId);
                if (simulation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al recuperar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }
                simulation.SimulationStateId = (int) EnSimulationState.SendToCompany;
                simulation.Active = true;
                simulation = Repository.UpdateSimulation(simulationId, simulation);
                if (simulation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al actualizar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var simulationCompany = Repository.GetSimulationCompany(simulationId);
                if (simulationCompany == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al recuperar la Simulación-Empresa",
                        Object = null,
                        Status = Status.Error
                    };
                }

                #region Creamos la Empresa si no existe
                if (simulationCompany.Company == null)
                {
                    var company = new Company
                    {
                        Name = simulation.CompanyName,
                        NIF = simulation.NIF,
                        FromSimulation = true,
                        GestorId = simulation.UserId,
                        EmployeesNumber = simulation.NumberEmployees
                    };
                    company = Repository.SaveCompany(company);
                    if (company == null)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error al crear la Empresa",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                    
                    var randomNumber = random.Next(0, 10000000);
                    for (var i = 0; i < simulation.NumberEmployees; i++)
                    {
                        var dni = $"{randomNumber}{RandomString(1)}";

                        var user = new User
                        {
                            FirstName = $"Trabajador {randomNumber++}",
                            DNI = dni
                        };
                        var resultEmployee = SaveEmployeeCompany((int)EnRole.Employee, company.Id, user);
                        if (resultEmployee.Status == Status.Error)
                        {
                            return new Result
                            {
                                Message = "Se ha producido un error al crear el Trabajador",
                                Object = null,
                                Status = Status.Error
                            };
                        }
                    }

                    simulationCompany.CompanyId = company.Id;
                    simulationCompany = Repository.UpdateSimulationCompany(simulationId, company.Id);
                    if (simulationCompany == null)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error al actualizar la Simulación-Empresa",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                }
                #endregion

                if (simulationCompany.CompanyId == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al actualizar la Simulación-Empresa",
                        Object = null,
                        Status = Status.Error
                    };
                }
                var simulationsCompanyByCompany = Repository.GetSimulationsCompanyByCompany((int)simulationCompany.CompanyId);
                foreach (var simComp in simulationsCompanyByCompany)
                {
                    if (simComp.Simulation.Id == simulationId)
                        continue;

                    simComp.Simulation.Active = false;
                    var resultSim =  Repository.UpdateSimulation(simComp.Simulation.Id, simComp.Simulation);
                    if (resultSim == null)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error al actualizar la Simulación-Empresa",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                }

                var message = "La creación de la Empresa se ha producido con éxito";
                var status = Status.Ok;                
                if (!simulation.Original)
                {
                    if (simulation.NumberEmployees > simulationCompany.Company.EmployeesNumber)
                    {
                        var randomNumber = random.Next(0, 10000000);
                        for (var i = 0;
                            i < (simulation.NumberEmployees - simulationCompany.Company.EmployeesNumber);
                            i++)
                        {
                            var dni = $"{randomNumber}{RandomString(1)}";

                            var user = new User
                            {
                                FirstName = $"Trabajador {randomNumber++}",
                                DNI = dni
                            };
                            var resultEmployee =
                                SaveEmployeeCompany((int) EnRole.Employee, (int) simulationCompany.CompanyId, user);
                            if (resultEmployee.Status == Status.Error)
                            {
                                return new Result
                                {
                                    Message = "Se ha producido un error al crear el Trabajador",
                                    Object = null,
                                    Status = Status.Error
                                };
                            }
                        }
                    }
                    
                    if (simulation.NumberEmployees < simulationCompany.Company.EmployeesNumber)
                    {
                        message = "Debes revisar a los Empleados de la Empresa. El número no coincide con los datos de la Simulación";
                        status = Status.Warning;
                    }
                }

                return new Result
                {
                    Message = message,
                    Object = simulationCompany.CompanyId,
                    Status = status
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al crear la Empresa",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public decimal GetTotalSimulation(int simulationId)
        {
            var IVA = Convert.ToDecimal(Repository.GetTagValue("IVA"));
            var simulation = Repository.GetSimulation(simulationId);

            var total = 0.0m;

            if (simulation.ForeignPreventionService != null)
            {
                if (simulation.ForeignPreventionService.AmountTecniques != null)
                {
                    total = (decimal)simulation.ForeignPreventionService.AmountTecniques * simulation.NumberEmployees * IVA;
                }
                if (simulation.ForeignPreventionService.AmountHealthVigilance != null)
                {
                    total = (decimal)simulation.ForeignPreventionService.AmountHealthVigilance * simulation.NumberEmployees * IVA;
                }
                if (simulation.ForeignPreventionService.AmountMedicalExamination != null)
                {
                    total = (decimal)simulation.ForeignPreventionService.AmountMedicalExamination * simulation.NumberEmployees;
                }
                total = simulation.ForeignPreventionService.Total;
            }                

            if (simulation.AgencyService != null)
                total += simulation.AgencyService.Total * IVA;

            if (simulation.TrainingService != null)
                total += simulation.TrainingService.Total * IVA;

            return total;
        }

        public Simulation GetSimulationActive(int companyId)
        {
            return Repository.GetSimulationsCompanyByCompany(companyId).FirstOrDefault(x => x.Simulation.Active)?.Simulation;
        }
    }
}
