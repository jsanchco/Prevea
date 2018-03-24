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
        public MedicalExamination GetMedicalExaminationById(int id)
        {
            return Repository.GetMedicalExaminationById(id);
        }

        public List<MedicalExamination> GetMedicalExaminations()
        {
            return Repository.GetMedicalExaminations();
        }

        public Result SaveMedicalExamination(MedicalExamination medicalExamination)
        {
            try
            {
                medicalExamination = Repository.SaveMedicalExamination(medicalExamination);

                if (medicalExamination == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Reconocimiento Médico",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Reconocimiento Médico se ha producido con éxito",
                    Object = medicalExamination,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Reconocimiento Médico",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteMedicalExamination(int id)
        {
            try
            {
                var result = Repository.DeleteMedicalExamination(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Reconocimiento Médico",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado del Reconocimiento Médico se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar el Reconocimiento Médico",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
