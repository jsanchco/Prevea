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
                if (string.IsNullOrEmpty(medicalExamination.InputTemplatesJSON))
                    medicalExamination.InputTemplatesJSON = GenerateMedicalExaminationInputTemplatesJSON(medicalExamination.RequestMedicalExaminationEmployee);

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

        public string GenerateMedicalExaminationInputTemplatesJSON(RequestMedicalExaminationEmployee requestMedicalExaminationEmployee)
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
                    Type = (int) EnInputTemplateType.Single,
                    DefaultValue = 0,
                    DefaultText = "hombre",
                    DataSource = new List<string> { "hombre", "mujer" }
                },
                new InputTemplate
                {
                    Name = "me-7",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "desconocer la existencia de antecedentes patológicos de interés en su familia."
                },
                new InputTemplate
                {
                    Name = "me-8",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "no haber sufrido accidentes laborales de interés o enfermedades profesionales previas.\nRelata haber sido vacunado de las vacunas propias de la infancia.\nEn el momento actual manifiesta no padecer alergias conocidas."
                },
                new InputTemplate
                {
                    Name = "me-9",
                    Type = (int) EnInputTemplateType.Single,
                    DefaultValue = 0,
                    DefaultText = "no fumador/a, que no fuma ni ha fumado nunca.",
                    DataSource = new List<string> { "no fumador/a, que no fuma ni ha fumado nunca.", "fuma esporádicamente." }
                },
                new InputTemplate
                {
                    Name = "me-10",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "no beber alcohol habitualmente."
                },
                new InputTemplate
                {
                    Name = "me-11",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "seguir una dieta variada."
                },
                new InputTemplate
                {
                    Name = "me-12",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "practicar algún ejercicio o deporte de manera habitual."
                },
                new InputTemplate
                {
                    Name = "me-13",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "dormir bien durante seis horas o más."
                },
                new InputTemplate
                {
                    Name = "me-14",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "llevarlas a cabo de forma periódica."
                },
                new InputTemplate
                {
                    Name = "me-15",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "que es normal."
                },
                new InputTemplate
                {
                    Name = "me-16",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "no presenta patología o sintomatología relacionada con su trabajo, tiene sensación subjetiva de buena salud."
                },
                new InputTemplate
                {
                    Name = "me-17",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "no consumir habitualmente ningún medicamento."
                },
                new InputTemplate
                {
                    Name = "me-18",
                    Type = (int) EnInputTemplateType.Single,
                    DefaultValue = 0,
                    DefaultText = "hombre",
                    DataSource = new List<string> { "hombre", "mujer" }
                },
                new InputTemplate
                {
                    Name = "me-19",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "1.70"
                },
                new InputTemplate
                {
                    Name = "me-20",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "70"
                },
                new InputTemplate
                {
                    Name = "me-21",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "119/79"
                },
                new InputTemplate
                {
                    Name = "me-22",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "60"
                },
                new InputTemplate
                {
                    Name = "me-23",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "normal."
                },
                new InputTemplate
                {
                    Name = "me-24",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "buena coloración de las mismas."
                },
                new InputTemplate
                {
                    Name = "me-25",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "sin sarro."
                },
                new InputTemplate
                {
                    Name = "me-26",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "normal."
                },
                new InputTemplate
                {
                    Name = "me-27",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "normal."
                },
                new InputTemplate
                {
                    Name = "me-28",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "normal, sin que se aprecien contracturas musculares."
                },
                new InputTemplate
                {
                    Name = "me-29",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "normal y no se observan alteraciones patológicas en los conductos auditivos externos ni en los timpanos."
                },
                new InputTemplate
                {
                    Name = "me-30",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "normal."
                },
                new InputTemplate
                {
                    Name = "me-31",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "una configuración normal y simétrica de las estructuras óseas, con movimientos respiratorios libres y regulares. No se observa tiraje ni cornaje respiratorio, ni circulación venosa superficial colateral."
                },
                new InputTemplate
                {
                    Name = "me-32",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "tonos cardiacos puros y rítmicos en todos los focos, que se encuentran en su posición normal. No se auscultan soplos, extratonos ni otros ruidos anormales."
                },
                new InputTemplate
                {
                    Name = "me-33",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "una auscultacion normal, sin roncus ni sibilancias. No se aprecian roces pleurales, con buena ventilación en ambas bases y murmullo vesicular conservado."
                },
                new InputTemplate
                {
                    Name = "me-34",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "blando, depresible, no doloroso a la palpación y sin masas ni visceromegalias palpables."
                },
                new InputTemplate
                {
                    Name = "me-35",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "inexistencia de deformidades ni signos articulares anormales. No se aprecian distrofias, tono normal y movilidad no dolorosa."
                },
                new InputTemplate
                {
                    Name = "me-36",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "discretas varículas en muslos (asintomático)."
                },
                new InputTemplate
                {
                    Name = "me-37",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "rectificación de la cifosis dorsal."
                },
                new InputTemplate
                {
                    Name = "me-38",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "ausencia de signos patológicos, compatible con la normalidad."
                },
                new InputTemplate
                {
                    Name = "me-39",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "la movilidad articular está conservada, no es dolorosa, sin datos de afectación de nervios periféricos. No se aprecian zonas dolorosas a la palpación."
                },
                new InputTemplate
                {
                    Name = "me-40",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "test de Phalen (-) derecho, test de Tinel (-) derecho, test de Phalen (-) izquierdo, test de Tinel (-) izquierdo."
                },
                new InputTemplate
                {
                    Name = "me-41",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "Exploracion dermatológica normal."
                },
                new InputTemplate
                {
                    Name = "me-42",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "Sin hallazgos significativos."
                },
                new InputTemplate
                {
                    Name = "me-43",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "dentro de los límites de la normalidad."
                },
                new InputTemplate
                {
                    Name = "me-44",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "98."
                },
                new InputTemplate
                {
                    Name = "me-45",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "un nivel normal de audición."
                },
                new InputTemplate
                {
                    Name = "me-46",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "una visión cromática normal, apreciamos un control visión normal."
                },
                new InputTemplate
                {
                    Name = "me-47",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "el patrón pulmonar está dentro de la normalidad."
                },
                new InputTemplate
                {
                    Name = "me-48",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "un electrocardiograma con parámetros normales."
                },
                new InputTemplate
                {
                    Name = "me-49",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "5000"
                },
                new InputTemplate
                {
                    Name = "me-50",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "4181"
                },
                new InputTemplate
                {
                    Name = "me-51",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "80"
                },
                new InputTemplate
                {
                    Name = "me-52",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "4691"
                },
                new InputTemplate
                {
                    Name = "me-53",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "10"
                },
                new InputTemplate
                {
                    Name = "me-54",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "10"
                },
                new InputTemplate
                {
                    Name = "me-55",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "10"
                },
                new InputTemplate
                {
                    Name = "me-56",
                    Type = (int) EnInputTemplateType.Single,
                    DefaultValue = 0,
                    DefaultText = "No",
                    DataSource = new List<string> { "No", "Si" }
                },
                new InputTemplate
                {
                    Name = "me-57",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "10"
                },
                new InputTemplate
                {
                    Name = "me-58",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "10"
                },
                new InputTemplate
                {
                    Name = "me-59",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "10"
                },
                new InputTemplate
                {
                    Name = "me-60",
                    Type = (int) EnInputTemplateType.Single,
                    DefaultValue = 0,
                    DefaultText = "No",
                    DataSource = new List<string> { "No", "Si" }
                },
                new InputTemplate
                {
                    Name = "me-61",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "Normal"
                },
                new InputTemplate
                {
                    Name = "me-62",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "Normal"
                },
                new InputTemplate
                {
                    Name = "me-63",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "0"
                },
                new InputTemplate
                {
                    Name = "me-64",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "0"
                },
                new InputTemplate
                {
                    Name = "me-65",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "0"
                },
                new InputTemplate
                {
                    Name = "me-66",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "0"
                },
                new InputTemplate
                {
                    Name = "me-67",
                    Type = (int) EnInputTemplateType.Input,
                    DefaultText = "0"
                },
                new InputTemplate
                {
                    Name = "me-68",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = "Normal"
                },
            };

            return JsonConvert.SerializeObject(listInputTemplateMedicalExamination);
        }
    }
}
