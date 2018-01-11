namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public TrainingCourseTrainingService GetTrainingCourseTrainingService(int trainingCourseTrainingServiceId)
        {
            return Context.TrainingCoursesTrainingServices
                .Include(x => x.TrainingCourse)
                .Include(x => x.TrainingService)
                .Include(x => x.TrainingService.Simulation)
                .FirstOrDefault(x => x.Id == trainingCourseTrainingServiceId);
        }

        public List<TrainingCourseTrainingService> GetTrainingCoursesTrainingServiceByTrainingService(int trainingServiceId)
        {
            return Context.TrainingCoursesTrainingServices
                .Include(x => x.TrainingCourse)
                .Include(x => x.TrainingService)
                .Where(x => x.TrainingServiceId == trainingServiceId)
                .ToList();
        }

        public List<TrainingCourseTrainingService> GetTrainingCourseTrainingServicesByTrainingCourse(int trainingCourseId)
        {
            return Context.TrainingCoursesTrainingServices
                .Include(x => x.TrainingCourse)
                .Include(x => x.TrainingService)
                .Where(x => x.TrainingCourseId == trainingCourseId)
                .ToList();
        }

        public List<TrainingCourse> GetTrainingCoursesByTrainingService(int trainingServiceId)
        {
            var trainingCourses = from tc in Context.TrainingCoursesTrainingServices
                join ts in Context.TrainingServices on tc.TrainingServiceId equals ts.Id
                where tc.TrainingServiceId == trainingServiceId
                select tc.TrainingCourse;

            return trainingCourses.ToList();
        }

        public List<TrainingService> GetTrainingServicesByTrainingCourse(int trainingCourseId)
        {
            var trainingServices = from ts in Context.TrainingCoursesTrainingServices
                join tc in Context.TrainingCourses on ts.TrainingCourseId equals tc.Id
                where ts.TrainingCourseId == trainingCourseId
                select ts.TrainingService;

            return trainingServices.ToList();
        }

        public TrainingCourseTrainingService SaveTrainingCourseTrainingService(TrainingCourseTrainingService trainingCourseTrainingService)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.TrainingCoursesTrainingServices.AddOrUpdate(trainingCourseTrainingService);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return trainingCourseTrainingService;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteTrainingCourseTrainingService(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var trainingCourseTrainingServiceFind = Context.TrainingCoursesTrainingServices.Find(id);
                    if (trainingCourseTrainingServiceFind == null)
                        return false;

                    Context.TrainingCoursesTrainingServices.Remove(trainingCourseTrainingServiceFind);

                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
