using System.Linq;

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
        public TrainingCourseTrainingService GetTrainingCourseTrainingService(int trainingCourseTrainingServiceId)
        {
            return Repository.GetTrainingCourseTrainingService(trainingCourseTrainingServiceId);
        }

        public List<TrainingCourseTrainingService> GetTrainingCoursesTrainingServiceByTrainingService(int trainingServiceId)
        {
            return Repository.GetTrainingCoursesTrainingServiceByTrainingService(trainingServiceId);
        }

        public List<TrainingCourseTrainingService> GetTrainingCourseTrainingServicesByTrainingCourse(int trainingCourseId)
        {
            return Repository.GetTrainingCourseTrainingServicesByTrainingCourse(trainingCourseId);
        }

        public List<TrainingCourse> GetTrainingCoursesByTrainingService(int trainingServiceId)
        {
            return Repository.GetTrainingCoursesByTrainingService(trainingServiceId);
        }

        public List<TrainingService> GetTrainingServicesByTrainingCourse(int trainingCourseId)
        {
            return Repository.GetTrainingServicesByTrainingCourse(trainingCourseId);
        }

        public Result SaveTrainingCourseTrainingService(TrainingCourseTrainingService trainingCourseTrainingService)
        {
            try
            {
                var trainingService = Repository.GetTrainingService(trainingCourseTrainingService.TrainingServiceId);
                if (trainingService == null)
                {
                    trainingService = Repository.SaveTrainingService(new TrainingService
                    {
                        Id = trainingCourseTrainingService.TrainingServiceId
                    });                    

                    if (trainingService == null)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error en la Grabación de Curso-Servicio",
                            Object = null,
                            Status = Status.Error
                        };
                    }                    
                }

                trainingCourseTrainingService = Repository.SaveTrainingCourseTrainingService(trainingCourseTrainingService);

                if (trainingCourseTrainingService == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de Curso-Servicio",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var courses = Repository.GetTrainingCoursesTrainingServiceByTrainingService(trainingService.Id);
                var total = courses.Sum(course => course.Total);
                trainingService = Repository.GetTrainingService(trainingService.Id);
                if (trainingService == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de Curso-Servicio",
                        Object = null,
                        Status = Status.Error
                    };
                }
                trainingService.Total = total;
                trainingService = Repository.SaveTrainingService(trainingService);
                if (trainingService == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de Curso-Servicio",
                        Object = null,
                        Status = Status.Error
                    };
                }

                var simulation = Repository.GetSimulation(trainingService.Simulation.Id);
                if (simulation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de Curso-Servicio",
                        Object = null,
                        Status = Status.Error
                    };
                }
                simulation.SimulationStateId = (int) EnSimulationState.ValidationPending;
                simulation = Repository.UpdateSimulation(simulation.Id, simulation);
                if (simulation == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación de Curso-Servicio",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación de Curso-Servicio se ha producido con éxito",
                    Object = trainingCourseTrainingService,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación de Curso-Servicio",
                    Object = trainingCourseTrainingService,
                    Status = Status.Error
                };
            }
        }

        public Result DeleteTrainingCourseTrainingService(int id)
        {
            try
            {
                var result = Repository.DeleteTrainingCourseTrainingService(id);

                if (result == false)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar Curso-Servicio",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "El Borrado de Curso-Servicio se ha producido con éxito",
                    Object = null,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error al Borrar Curso-Servicio",
                    Object = null,
                    Status = Status.Error
                };
            }
        }
    }
}
