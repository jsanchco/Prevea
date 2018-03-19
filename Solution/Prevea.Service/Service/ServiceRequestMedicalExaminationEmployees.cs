namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System.Linq;

    #endregion

    public partial class Service
    {
        public RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeById(int id)
        {
            return Repository.GetRequestMedicalExaminationEmployeeById(id);
        }

        public List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployees()
        {
            return Repository.GetRequestMedicalExaminationEmployees();
        }

        public List<Employee> GetEmployeesByRequestMedicalExamination(int requestMedicalExaminationId)
        {
            return Repository.GetEmployeesByRequestMedicalExamination(requestMedicalExaminationId);
        } 

        public Result SaveRequestMedicalExaminationEmployee(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        {
            try
            {
                requestMedicalExaminationEmployee = Repository.SaveRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);

                if (requestMedicalExaminationEmployee == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de RequestMedicalExaminationEmployee",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la RequestMedicalExaminationEmployee se ha producido con éxito",
                    Object = requestMedicalExaminationEmployee,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la RequestMedicalExaminationEmployee",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteRequestMedicalExaminationEmployee(int id)
        {
            try
            {
                var result = Repository.DeleteRequestMedicalExaminationEmployee(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la RequestMedicalExaminationEmployee",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la RequestMedicalExaminationEmployee se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la RequestMedicalExaminationEmployee",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        //public Result UpdateRequestHistoricMedicalExaminationEmployees(List<RequestMedicalExaminationEmployee> listEmployees, int userId)
        //{
        //    try
        //    {
        //        if (listEmployees == null || listEmployees.Count == 0)
        //        {
        //            return new Result
        //            {
        //                Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                Object = null,
        //                Status = Status.Error
        //            };
        //        }                

        //        var requestMedicalExaminationsId = listEmployees[0].RequestMedicalExaminationsId;
        //        var requestMedicalExaminationEmployeesByRequestMedicalExamination = GetRequestMedicalExaminationEmployees()
        //                .Where(x => x.RequestMedicalExaminationsId == requestMedicalExaminationsId)
        //                .ToList();
        //        foreach (var employee in requestMedicalExaminationEmployeesByRequestMedicalExamination)
        //        {
        //            var exist = listEmployees.FirstOrDefault(x => x.Id == employee.Id);
        //            if (exist != null)
        //            {
        //                var delete = DeleteRequestMedicalExaminationEmployee(employee.Id);
        //                if (delete.Status == Status.Error)
        //                {
        //                    return new Result
        //                    {
        //                        Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                        Object = null,
        //                        Status = Status.Error
        //                    };
        //                }
        //            }
        //        }

        //        var allValidated = true;
        //        foreach (var employee in listEmployees)
        //        {
        //            var exist = GetRequestMedicalExaminationEmployeeById(employee.Id);
        //            if (exist != null)
        //            {
        //                exist.Date = employee.Date;
        //                exist.Observations = employee.Observations;
        //                exist.SamplerNumber = employee.SamplerNumber;
        //                exist.RequestMedicalExaminationEmployeeStateId =
        //                    (int) EnRequestMedicalExaminationEmployeeState.Pending;
        //                exist.ChangeDate = employee.ChangeDate;
        //                exist.ClinicId = employee.ClinicId;

        //                var saveExist = SaveRequestMedicalExaminationEmployee(exist);
        //                if (saveExist.Status == Status.Error)
        //                {
        //                    return new Result
        //                    {
        //                        Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                        Object = null,
        //                        Status = Status.Error
        //                    };
        //                }
        //                allValidated = false;

        //                var listDoctors = doctors.Split(',');
        //                var doctorsMedicalExaminationEmployees = GetDoctorsMedicalExaminationEmployees()
        //                    .Where(x => x.MedicalExaminationEmployeeId == requestMedicalExaminationsId).ToList();

        //                foreach (var doctor in doctorsMedicalExaminationEmployees)
        //                {
        //                    var existDoctor = listDoctors.FirstOrDefault(x => x.Contains(doctor.ToString()));
        //                    if (existDoctor != null)
        //                    {
        //                        var delete = DeleteDoctorMedicalExaminationEmployee(doctor.Id);
        //                        if (delete.Status == Status.Error)
        //                        {
        //                            return new Result
        //                            {
        //                                Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                                Object = null,
        //                                Status = Status.Error
        //                            };
        //                        }
        //                    }
        //                }

        //                foreach (var doctor in listDoctors)
        //                {
        //                    var findDoctor = GetDoctorsMedicalExaminationEmployees().FirstOrDefault(x =>
        //                        x.MedicalExaminationEmployeeId == ((RequestMedicalExaminations) saveExist.Object).Id &&
        //                        x.DoctorId == Convert.ToInt32(doctor));
        //                    if (findDoctor == null)
        //                    {
        //                        var saveDoctor = SaveDoctorMedicalExaminationEmployee(
        //                            new DoctorMedicalExaminationEmployee
        //                            {
        //                                MedicalExaminationEmployeeId = ((RequestMedicalExaminations)saveExist.Object).Id,
        //                                DoctorId = Convert.ToInt32(doctor)
        //                            });
        //                        if (saveDoctor.Status == Status.Error)
        //                        {
        //                            return new Result
        //                            {
        //                                Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                                Object = null,
        //                                Status = Status.Error
        //                            };
        //                        }
        //                    }
        //                }

        //                continue;
        //            }

        //            var requestMedicalExaminationEmployee = new RequestMedicalExaminationEmployee
        //            {
        //                Date = employee.Date,
        //                EmployeeId = employee.EmployeeId,
        //                Observations = employee.Observations,
        //                SamplerNumber = employee.SamplerNumber,
        //                RequestMedicalExaminationEmployeeStateId = (int)EnRequestMedicalExaminationEmployeeState.Pending,
        //                RequestMedicalExaminationsId = requestMedicalExaminationsId,
        //                ChangeDate = employee.ChangeDate,
        //                ClinicId = employee.ClinicId
        //            };
        //            if (requestMedicalExaminationEmployee.ChangeDate == false)
        //                allValidated = false;

        //            var saveEmployee = SaveRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);
        //            if (saveEmployee.Status == Status.Error)
        //            {
        //                return new Result
        //                {
        //                    Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                    Object = null,
        //                    Status = Status.Error
        //                };
        //            }
        //        }

        //        var user = GetUser(userId);
        //        var requestMedicalExamination = GetRequestMedicalExaminationById(requestMedicalExaminationsId);
        //        if (user.UserRoles.First().RoleId == (int)EnRole.ContactPerson)
        //        {
        //            requestMedicalExamination.RequestMedicalExaminationStateId =
        //                (int)EnRequestMedicalExaminationState.Pending;

        //            var resultSaveRequestMedicalExaminations = SaveRequestMedicalExaminations(requestMedicalExamination);
        //            if (resultSaveRequestMedicalExaminations.Status == Status.Error)
        //            {
        //                return new Result
        //                {
        //                    Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                    Object = null,
        //                    Status = Status.Error
        //                };
        //            }
        //        }
        //        else
        //        {
        //            if (allValidated)
        //            {
        //                requestMedicalExamination.RequestMedicalExaminationStateId =
        //                    (int)EnRequestMedicalExaminationState.Validated;
        //            }
        //            else
        //            {
        //                requestMedicalExamination.RequestMedicalExaminationStateId =
        //                    (int)EnRequestMedicalExaminationState.Pending;
        //            }

        //            var resultSaveRequestMedicalExaminations = SaveRequestMedicalExaminations(requestMedicalExamination);
        //            if (resultSaveRequestMedicalExaminations.Status == Status.Error)
        //            {
        //                return new Result
        //                {
        //                    Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //                    Object = null,
        //                    Status = Status.Error
        //                };
        //            }
        //        }

        //        return new Result
        //        {
        //            Message = "Se ha producido con éxito la Grabación de RequestMedicalExamination",
        //            Object = GetRequestMedicalExaminationById(requestMedicalExaminationsId),
        //            Status = Status.Ok
        //        };
        //    }
        //    catch (Exception)
        //    {
        //        return new Result
        //        {
        //            Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
        //            Object = null,
        //            Status = Status.Error
        //        };
        //    }
        //}
    }
}
