namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;

    #endregion

    public partial class Service
    {
        public RequestMedicalExaminations GetRequestMedicalExaminationById(int id)
        {
            return Repository.GetRequestMedicalExaminationById(id);
        }

        public List<RequestMedicalExaminations> GetRequestMedicalExaminations()
        {
            return Repository.GetRequestMedicalExaminations();
        }

        public Result SaveRequestMedicalExaminations(RequestMedicalExaminations requestMedicalExamination)
        {
            try
            {
                requestMedicalExamination = Repository.SaveRequestMedicalExaminations(requestMedicalExamination);

                if (requestMedicalExamination == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de la RequestMedicalExamination",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de la RequestMedicalExamination se ha producido con éxito",
                    Object = requestMedicalExamination,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de la RequestMedicalExamination",
                    Object = requestMedicalExamination,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteRequestMedicalExamination(int id)
        {
            try
            {
                var result = Repository.DeleteRequestMedicalExamination(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar la RequestMedicalExamination",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de la RequestMedicalExamination se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar la RequestMedicalExamination",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
