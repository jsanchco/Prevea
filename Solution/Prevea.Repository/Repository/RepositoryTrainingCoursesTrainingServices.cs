namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<TrainingCourse> GetTrainingCoursesByTrainingService(int trainingServiceId)
        {
            var trainingCourse = from tc in Context.TrainingCoursesTrainingServices
                join ts in Context.TrainingServices on tc.TrainingServiceId equals ts.Id
                where tc.TrainingServiceId == trainingServiceId
                select tc.TrainingCourse;

            return trainingCourse.ToList();
        }

        public List<TrainingService> GetTrainingServicesByTrainingCourse(int trainingCourseId)
        {
            var trainingService = from ts in Context.TrainingCoursesTrainingServices
                join tc in Context.TrainingCourses on ts.TrainingCourseId equals tc.Id
                where ts.TrainingCourseId == trainingCourseId
                select ts.TrainingService;

            return trainingService.ToList();
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
            throw new System.NotImplementedException();
        }
    }
}
