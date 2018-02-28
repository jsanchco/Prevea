namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;

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
    }
}
