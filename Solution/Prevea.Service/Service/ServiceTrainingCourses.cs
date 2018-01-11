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
        public List<TrainingCourse> GetAllTrainingCourses()
        {
            return Repository.GetAllTrainingCourses();
        }

        public TrainingCourse GetTrainingCourse(int id)
        {
            return Repository.GetTrainingCourse(id);
        }

        public Result SaveTrainingCourse(TrainingCourse trainingCourse)
        {
            try
            {
                trainingCourse = Repository.SaveTrainingCourse(trainingCourse);

                if (trainingCourse == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Curso",
                        Object = null,
                        Status = Status.Error
                    };
                }

                if (trainingCourse.ReportsTo != null)
                {
                    var parenTrainingCourse = Repository.GetTrainingCourse((int)trainingCourse.ReportsTo);
                    if (!parenTrainingCourse.HasChildren)
                    {
                        parenTrainingCourse.HasChildren = true;
                        parenTrainingCourse = Repository.UpdateTrainingCourse((int)trainingCourse.ReportsTo, parenTrainingCourse);
                        if (parenTrainingCourse == null)
                        {
                            return new Result
                            {
                                Message = "Se ha producido un error en la Grabación del Curso",
                                Object = null,
                                Status = Status.Error
                            };
                        }
                    }
                }
  
                return new Result
                {
                    Message = "La Grabación del Curso se ha producido con éxito",
                    Object = trainingCourse,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Curso",
                    Object = trainingCourse,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateTrainingCourse(int id, TrainingCourse trainingCourse)
        {
            return SaveTrainingCourse(trainingCourse);
        }

        public Result DeleteTrainingCourse(int id)
        {
            {
                try
                {
                    var result = Repository.DeleteTrainingCourse(id);

                    if (result == false)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error al Borrar el Curso",
                            Object = null,
                            Status = Status.Error
                        };
                    }

                    return new Result
                    {
                        Message = "El Borrado del Curso se ha producido con éxito",
                        Object = null,
                        Status = Status.Ok
                    };
                }
                catch (Exception)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error al Borrar el Curso",
                        Object = null,
                        Status = Status.Error
                    };
                }
            }
        }

        public List<TrainingCourse> GetTrainingCourses(int? trainingCourse)
        {
            return Repository.GetTrainingCourses(trainingCourse);
        }

        public TrainingCourse FindTrainingCourse(string text)
        {
            return Repository.FindTrainingCourse(text);
        }
    }
}
