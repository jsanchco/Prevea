namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using Newtonsoft.Json;

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
                if (string.IsNullOrEmpty(medicalExamination.MedicalExaminationJSON))
                    medicalExamination.MedicalExaminationJSON = GenerateMedicalExaminationJSON(medicalExamination.RequestMedicalExaminationEmployee);

                medicalExamination.EndDate = DateTime.Now;
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

        public string GenerateMedicalExaminationJSON(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
        {
            var listInputTemplateMedicalExamination = new List<InputTemplate>
            {
                new InputTemplate
                {
                    Name = "me-1",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "40"
                },
                new InputTemplate
                {
                    Name = "me-2",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "No, actualmente se encuentra trabajando."
                },
                new InputTemplate
                {
                    Name = "me-3",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "No refiere haber sufrido accidentes laborales desde el último reconocimiento."
                },
                new InputTemplate
                {
                    Name = "me-4",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "No refiere diagnóstico de Enfermedad Profesional."
                },
                new InputTemplate
                {
                    Name = "me-5",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = $"{requestMedicalExaminationEmployee.Employee.User.WorkStation} [meses]"
                },
                new InputTemplate
                {
                    Name = "me-6",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "hombre"
                },
                new InputTemplate
                {
                    Name = "me-7",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "desconocer la existencia de antecedentes patológicos de interés en su familia."
                }
            };

            return JsonConvert.SerializeObject(listInputTemplateMedicalExamination);
        }
    }
}
