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
        public Simulator GetSimulator(int simulatorId)
        {
            return Repository.GetSimulator(simulatorId);
        }

        public List<Simulator> GetSimulators()
        {
            return Repository.GetSimulators();
        }

        public Result SaveSimulator(Simulator simulator)
        {
            try
            {
                simulator = Repository.SaveSimulator(simulator);                

                if (simulator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var simulatorCompany = Repository.SaveSimulatorCompany(simulator.Id);

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

        public Result UpdateSimulator(int simulatorId, Simulator simulator)
        {
            try
            {
                simulator = Repository.UpdateSimulator(simulatorId, simulator);

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

        public Result DeleteSimulator(int simulatorId)
        {
            try
            {
                var result = Repository.DeleteSimulator(simulatorId);

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

        public List<Simulator> GetSimulatorsByUser(int userId)
        {
            return Repository.GetSimulatorsByUser(userId);
        }

        public Result SendToCompanies(int simulatorId)
        {
            try
            {
                var simulator = Repository.GetSimulator(simulatorId);
                if (simulator == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la Simulación",
                        Object = null,
                        Status = Status.Error
                    };
                }

                simulator = Repository.UpdateSimulator(simulatorId, simulator);
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
                var simulatorCompany = Repository.UpdateSimulatorCompany(simulatorId, company.Id);
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
