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
        public Simulation GetSimulation(int simulatorId)
        {
            return Repository.GetSimulation(simulatorId);
        }

        public List<Simulation> GetSimulations()
        {
            return Repository.GetSimulations();
        }

        public Result SaveSimulation(Simulation simulator)
        {
            try
            {
                simulator = Repository.SaveSimulation(simulator);                

                if (simulator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var simulatorCompany = Repository.SaveSimulationCompany(simulator.Id);

                if (simulatorCompany == null)
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
                    Object = simulator,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Simulación",
                    Object = simulator,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateSimulation(int simulatorId, Simulation simulator)
        {
            try
            {
                simulator = Repository.UpdateSimulation(simulatorId, simulator);

                if (simulator == null)
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
                    Object = simulator,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la Simulación",
                    Object = simulator,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteSimulation(int simulatorId)
        {
            try
            {
                var result = Repository.DeleteSimulation(simulatorId);

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

        public List<Simulation> GetSimulationsByUser(int userId)
        {
            return Repository.GetSimulationByUser(userId);
        }

        public Result SendToCompanies(int simulatorId)
        {
            try
            {
                var simulator = Repository.GetSimulation(simulatorId);
                if (simulator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                simulator = Repository.UpdateSimulation(simulatorId, simulator);
                if (simulator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var company = new Company
                {
                    Name = simulator.CompanyName,
                    NIF = simulator.NIF,
                    FromSimulator = true,
                    GestorId = simulator.UserId
                };
                company = Repository.SaveCompany(company);
                if (company == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }
                var random = new Random();
                var randomNumber = random.Next(0, 10000000);
                for (var i = 0; i < simulator.NumberEmployees; i++)
                {
                    var dni = $"{randomNumber}{RandomString(1)}";

                    var user = new User
                    {
                        FirstName = $"Trabajador {randomNumber++}",
                        DNI = dni,
                        Email = $"Trabajador{randomNumber}_{dni}@{company.Name.Replace(" ", string.Empty)}.com"
                    };
                    var resultEmployee = SaveEmployeeCompany((int) EnRole.Employee, company.Id, user);
                    if (resultEmployee.Status == Status.Error)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error al Borrar la Simulación",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                }

                var economicData = new EconomicData
                {
                    Id = company.Id,
                    //AmountTecniques = simulator.AmountTecniques,
                    //AmountHealthVigilance = simulator.AmountHealthVigilance,
                    //AmountMedicalExamination = simulator.AmountMedicalExamination,
                };
                var resultEconomicData = SaveEconomicData(economicData);
                if (resultEconomicData.Status == Status.Error)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }
                var simulatorCompany = Repository.UpdateSimulationCompany(simulatorId, company.Id);
                if (simulatorCompany == null)
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
                    Message = "La creación de la Empresa se ha producido con éxito",
                    Object = company.Id,
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
    }
}
