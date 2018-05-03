namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System.Linq;
    using Newtonsoft.Json;
    using Model.ViewModel;

    #endregion

    public partial class Service
    {
        public RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeById(int id)
        {
            return Repository.GetRequestMedicalExaminationEmployeeById(id);
        }

        public RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeeByEmployeeId(int requestMedicalExaminationsId,
            int employeeId)
        {
            return Repository.GetRequestMedicalExaminationEmployeeByEmployeeId(requestMedicalExaminationsId,
                employeeId);
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

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
                //var medicalExamination = Repository.GetMedicalExaminationById(id);
                //if (medicalExamination != null)
                //    Repository.DeleteMedicalExamination(id);

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

        public Result UpdateRequestHistoricMedicalExaminationEmployees(List<RequestMedicalExaminationEmployee> listEmployees, int userId)
        {
            try
            {
                if (listEmployees == null || listEmployees.Count == 0)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var requestMedicalExaminationsId = listEmployees[0].RequestMedicalExaminationsId;
                var requestMedicalExaminationEmployeesByRequestMedicalExamination = GetRequestMedicalExaminationEmployees()
                        .Where(x => x.RequestMedicalExaminationsId == requestMedicalExaminationsId)
                        .ToList();
                foreach (var employee in requestMedicalExaminationEmployeesByRequestMedicalExamination)
                {
                    var exist = listEmployees.FirstOrDefault(x => x.EmployeeId == employee.Id && !x.Included);
                    if (exist != null)
                    {
                        var delete = DeleteRequestMedicalExaminationEmployee(employee.Id);
                        if (delete.Status == Status.Error)
                        {
                            return new Result
                            {
                                Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                                Object = null,
                                Status = Status.Error
                            };
                        }
                    }
                }

                foreach (var employee in listEmployees)
                {
                    if (!string.IsNullOrEmpty(employee.Doctors))
                        employee.SplitDoctors = employee.Doctors.Split(',').Select(int.Parse).ToArray();

                    if (employee.Included)
                    {
                        var exist = GetRequestMedicalExaminationEmployeeByEmployeeId(
                            employee.RequestMedicalExaminationsId, employee.EmployeeId);
                        if (exist != null)
                        {
                            exist.Date = employee.Date;
                            exist.Observations = employee.Observations;
                            exist.SamplerNumber = employee.SamplerNumber;
 
                            exist.ChangeDate = employee.ChangeDate;

                            exist.RequestMedicalExaminationEmployeeStateId = employee.ChangeDate
                                ? (int) EnRequestMedicalExaminationEmployeeState.Validated
                                : (int) EnRequestMedicalExaminationEmployeeState.Pending;

                            exist.ClinicId = employee.ClinicId == 0 ? null : employee.ClinicId;

                            var saveExist = SaveRequestMedicalExaminationEmployee(exist);
                            if (saveExist.Status == Status.Error)
                            {
                                return new Result
                                {
                                    Message =
                                        "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                                    Object = null,
                                    Status = Status.Error
                                };
                            }

                            var doctorsMedicalExaminationEmployees = GetDoctorsMedicalExaminationEmployees()
                                .Where(x => x.MedicalExaminationEmployeeId == exist.Id).ToList();

                            foreach (var doctor in doctorsMedicalExaminationEmployees)
                            {
                                var existDoctor = employee.SplitDoctors.ToList().IndexOf(doctor.DoctorId);
                                if (existDoctor == -1)
                                {
                                    var delete = DeleteDoctorMedicalExaminationEmployee(doctor.Id);
                                    if (delete.Status == Status.Error)
                                    {
                                        return new Result
                                        {
                                            Message =
                                                "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                                            Object = null,
                                            Status = Status.Error
                                        };
                                    }
                                }
                            }

                            if (employee.SplitDoctors == null)
                                continue;

                            foreach (var doctor in employee.SplitDoctors)
                            {
                                var findDoctor = GetDoctorMedicalExaminationEmployeeByDoctorId(((RequestMedicalExaminationEmployee)saveExist.Object).Id, doctor);
                                if (findDoctor == null)
                                {
                                    var saveDoctor = SaveDoctorMedicalExaminationEmployee(
                                        new DoctorMedicalExaminationEmployee
                                        {
                                            MedicalExaminationEmployeeId =
                                                ((RequestMedicalExaminationEmployee) saveExist.Object).Id,
                                            DoctorId = doctor
                                        });
                                    if (saveDoctor.Status == Status.Error)
                                    {
                                        return new Result
                                        {
                                            Message =
                                                "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                                            Object = null,
                                            Status = Status.Error
                                        };
                                    }

                                }
                            }
                            continue;
                        }

                        var requestMedicalExaminationEmployee = new RequestMedicalExaminationEmployee
                        {
                            Date = employee.Date,
                            EmployeeId = employee.EmployeeId,
                            Employee = GetEmployeeById(employee.EmployeeId),
                            Observations = employee.Observations,
                            SamplerNumber = employee.SamplerNumber,
                            RequestMedicalExaminationsId = requestMedicalExaminationsId,
                            ChangeDate = employee.ChangeDate,
                            ClinicId = employee.ClinicId,
                            Clinic = employee.ClinicId != null ? GetClinicById((int)employee.ClinicId) : null
                        };
                        if (requestMedicalExaminationEmployee.ChangeDate == false)
                        {
                            requestMedicalExaminationEmployee.RequestMedicalExaminationEmployeeStateId =
                                (int) EnRequestMedicalExaminationEmployeeState.Pending;
                        }
                        else
                        {
                            requestMedicalExaminationEmployee.RequestMedicalExaminationEmployeeStateId =
                                (int)EnRequestMedicalExaminationEmployeeState.Validated;
                        }

                        var saveEmployee = SaveRequestMedicalExaminationEmployee(requestMedicalExaminationEmployee);
                        if (saveEmployee.Status == Status.Error)
                        {
                            return new Result
                            {
                                Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                                Object = null,
                                Status = Status.Error
                            };
                        }
                    }
                    else
                    {
                        var exist = GetRequestMedicalExaminationEmployeeByEmployeeId(
                            employee.RequestMedicalExaminationsId, employee.EmployeeId);
                        if (exist != null)
                        {
                            var delete = DeleteRequestMedicalExaminationEmployee(exist.Id);
                            if (delete.Status == Status.Error)
                            {
                                return new Result
                                {
                                    Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                                    Object = null,
                                    Status = Status.Error
                                };
                            }

                        }
                    }
                }

                var user = GetUser(userId);
                var requestMedicalExamination = GetRequestMedicalExaminationById(requestMedicalExaminationsId);
                if (user.UserRoles.First().RoleId == (int)EnRole.ContactPerson)
                {
                    requestMedicalExamination.RequestMedicalExaminationStateId =
                        (int)EnRequestMedicalExaminationState.Pending;

                    var resultSaveRequestMedicalExaminations = SaveRequestMedicalExaminations(requestMedicalExamination);
                    if (resultSaveRequestMedicalExaminations.Status == Status.Error)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                }
                else
                {
     
                    requestMedicalExamination.RequestMedicalExaminationStateId =
                        (int)EnRequestMedicalExaminationState.Validated;
     
                    var resultSaveRequestMedicalExaminations = SaveRequestMedicalExaminations(requestMedicalExamination);
                    if (resultSaveRequestMedicalExaminations.Status == Status.Error)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                }

                return new Result
                {
                    Message = "Se ha producido con éxito la Grabación de RequestMedicalExamination",
                    Object = GetRequestMedicalExaminationById(requestMedicalExaminationsId),
                    Status = Status.Ok
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return new Result
                {
                    Message = "Se ha producido un error al Actualizar la RequestMedicalExaminationEmployee",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public List<RequestMedicalExaminationEmployee> GetRequestMedicalExaminationEmployeesByDate(int doctorId, DateTime date)
        {
            return Repository.GetRequestMedicalExaminationEmployeesByDate(doctorId, date);
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
                new InputTemplate
                {
                    Name = "me-69",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = ""
                },
                new InputTemplate
                {
                    Name = "me-70",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = ""
                },
                new InputTemplate
                {
                    Name = "me-71",
                    Type = (int) EnInputTemplateType.Single,
                    DefaultValue = 0,
                    DefaultText = "Seleccionar ...",
                    DataSource = new List<string> { "Seleccionar ...", "Apto", "No Apto" }
                },
                new InputTemplate
                {
                    Name = "me-72",
                    Type = (int) EnInputTemplateType.TextArea,
                    DefaultText = ""
                }
            };

            return JsonConvert.SerializeObject(listInputTemplateMedicalExamination);
        }

        public TemplateMedicalExaminationViewModel GetTemplateMedicalExaminationViewModel(int requestMedicalExaminationEmployeeId)
        {
            var templateMedicalExaminationViewModel = new TemplateMedicalExaminationViewModel();

            var requestMedicalExaminationEmployee = GetRequestMedicalExaminationEmployeeById(requestMedicalExaminationEmployeeId);
            var medicalExaminationDocument = GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(requestMedicalExaminationEmployeeId);

            templateMedicalExaminationViewModel.RequestMedicalExaminationEmployeeId =
                requestMedicalExaminationEmployee.Id;
            templateMedicalExaminationViewModel.RequestMedicalExaminationEmployeeDate =
                requestMedicalExaminationEmployee.Date;
            templateMedicalExaminationViewModel.ClinicName =
                requestMedicalExaminationEmployee.Clinic.Name;
            templateMedicalExaminationViewModel.ClinicAddress =
                requestMedicalExaminationEmployee.Clinic.Address;
            templateMedicalExaminationViewModel.ClinicProvince =
                requestMedicalExaminationEmployee.Clinic.Province;
            templateMedicalExaminationViewModel.ClinicName =
                requestMedicalExaminationEmployee.Clinic.Name;
            var documentRCM = medicalExaminationDocument?.FirstOrDefault(x => x.Document.AreaId == 16);
            Document document;
            if (documentRCM == null)
            {
                document = GetDocumentByArea(16);
                document.Area = GetArea(16);
                templateMedicalExaminationViewModel.DocumentInputTemplateJSON = GenerateMedicalExaminationInputTemplatesJSON(requestMedicalExaminationEmployee);
            }
            else
            {
                document = documentRCM.Document;
                templateMedicalExaminationViewModel.DocumentInputTemplateJSON = document.InputTemplatesJSON;
            }
            templateMedicalExaminationViewModel.DocumentBeginDate = document.BeginDate;
            templateMedicalExaminationViewModel.DocumentEndDate = document.EndDate;
            templateMedicalExaminationViewModel.DocumentName = document.Name;
            templateMedicalExaminationViewModel.DocumentExtension = document.Extension;
            templateMedicalExaminationViewModel.DocumentStateId = (int)EnDocumentState.Pending;
            templateMedicalExaminationViewModel.DocumentUrlRelative = document.UrlRelative;
            templateMedicalExaminationViewModel.CompanyName = requestMedicalExaminationEmployee.Employee.Company.Name;
            templateMedicalExaminationViewModel.CompanyNIF = requestMedicalExaminationEmployee.Employee.Company.NIF;
            templateMedicalExaminationViewModel.EmployeeAddress = requestMedicalExaminationEmployee.Employee.User.Address;
            templateMedicalExaminationViewModel.EmployeeDNI = requestMedicalExaminationEmployee.Employee.User.DNI;
            templateMedicalExaminationViewModel.EmployeeProvince = requestMedicalExaminationEmployee.Employee.User.Province;
            templateMedicalExaminationViewModel.EmployeeWorkStation = requestMedicalExaminationEmployee.Employee.User.WorkStation;
            templateMedicalExaminationViewModel.EmployeeChargeDate = requestMedicalExaminationEmployee.Employee.User.ChargeDate;
            templateMedicalExaminationViewModel.EmployeePhoneNumber = requestMedicalExaminationEmployee.Employee.User.PhoneNumber;

            templateMedicalExaminationViewModel.EmployeeName = string.Empty;
            if (requestMedicalExaminationEmployee.Employee.User.LastName != null)
                templateMedicalExaminationViewModel.EmployeeName =
                    $"{requestMedicalExaminationEmployee.Employee.User.LastName.ToUpper()}, ";
            if (requestMedicalExaminationEmployee.Employee.User.FirstName != null)
                templateMedicalExaminationViewModel.EmployeeName +=
                    $"{requestMedicalExaminationEmployee.Employee.User.FirstName.ToUpper()}";

            var doctor = requestMedicalExaminationEmployee.DoctorsMedicalExaminationEmployee.FirstOrDefault();
            if (doctor != null)
            {
                templateMedicalExaminationViewModel.DoctorName = $"{doctor.Doctor.FirstName} {doctor.Doctor.LastName}";
                templateMedicalExaminationViewModel.DoctorCollegiateNumber = doctor.Doctor.CollegiateNumber;
            }

            return templateMedicalExaminationViewModel;
        }

        public RequestMedicalExaminationEmployee GetRequestMedicalExaminationEmployeesByDocumentId(int documentId)
        {
            return Repository.GetRequestMedicalExaminationEmployeesByDocumentId(documentId);
        }

        public Result SaveMedicalExamination(TemplateMedicalExaminationViewModel templateMedicalExaminationViewModel, int userId)
        {
            var requestMedicalExaminationEmployeeFind = GetRequestMedicalExaminationEmployeeById(templateMedicalExaminationViewModel.RequestMedicalExaminationEmployeeId);
            if (requestMedicalExaminationEmployeeFind == null)
            {
                return new Result
                {
                    Message = "Se ha producido un error en SaveMedicalExamination",
                    Object = null,
                    Status = Status.Error
                };
            }
  
            var requestMedicalExamination = GetRequestMedicalExaminationById(requestMedicalExaminationEmployeeFind.RequestMedicalExaminations.Id);
            requestMedicalExamination.RequestMedicalExaminationStateId = (int)EnRequestMedicalExaminationState.Blocked;
            var result = SaveRequestMedicalExaminations(requestMedicalExamination);
            if (result.Status == Status.Error)
            {
                return new Result
                {
                    Message = "Se ha producido un error en SaveMedicalExamination",
                    Object = null,
                    Status = Status.Error
                };
            }

            var medicalExaminationDocument =
                requestMedicalExaminationEmployeeFind.MedicalExaminationDocuments.FirstOrDefault(x =>
                    x.Document.AreaId == 16);
            if (medicalExaminationDocument == null)
            {
                medicalExaminationDocument = new MedicalExaminationDocuments
                {
                    RequestMedicalExaminationEmployeeId = requestMedicalExaminationEmployeeFind.Id,
                    Document = GetDocumentByArea(16)
                };

                result = SaveMedicalExaminationDocument(
                    requestMedicalExaminationEmployeeFind.Id,
                    medicalExaminationDocument.Document, 
                    userId);

                //var documentUserOwners = requestMedicalExamination.Company.ContactPersons.Select(contactPerson => new DocumentUserOwner { UserId = contactPerson.UserId }).ToList();
                //documentUserOwners.Add(new DocumentUserOwner { UserId = requestMedicalExaminationEmployeeFind.Employee.UserId });
                //result = SaveDocument(medicalExaminationDocument.Document,
                //    false,
                //    new List<DocumentUserCreator> { new DocumentUserCreator { UserId = userId } },
                //    documentUserOwners,
                //    ".pdf");
                if (result.Status == Status.Error)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en SaveMedicalExamination",
                        Object = null,
                        Status = Status.Error
                    };
                }

                medicalExaminationDocument.Document = (Document)result.Object;
            }

            medicalExaminationDocument.Document.InputTemplatesJSON =
                templateMedicalExaminationViewModel.DocumentInputTemplateJSON;

            var inputTemplates = JsonConvert.DeserializeObject<List<InputTemplate>>(medicalExaminationDocument.Document.InputTemplatesJSON);
            var inputTemplateAptitude = inputTemplates.FirstOrDefault(x => x.Name == "me-71");
            if (inputTemplateAptitude != null)
            {
                if (inputTemplateAptitude.Value == 0 && string.IsNullOrEmpty(inputTemplateAptitude.Text))
                    medicalExaminationDocument.Document.DocumentStateId = (int)EnDocumentState.InProcess;
                else
                    medicalExaminationDocument.Document.DocumentStateId = (int)EnDocumentState.Finished;
            }

            result = UpdateDocument(medicalExaminationDocument.Document, false);
            if (result.Status == Status.Error)
            {
                return new Result
                {
                    Message = "Se ha producido un error en SaveMedicalExamination",
                    Object = null,
                    Status = Status.Error
                };
            }

            return new Result
            {
                Message = "La grabación de SaveMedicalExamination se ha producido con éxito",
                Object = medicalExaminationDocument.Document.DocumentStateId,
                Status = Status.Ok
            };
        }
    }
}
