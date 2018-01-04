namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;
    using System.Data.Entity.Migrations;


    #endregion

    public partial class Repository
    {
        public List<TrainingCourse> GetAllTrainingCourses()
        {
            return Context.TrainingCourses
                .Include(x => x.TrainingCourseModality)
                .ToList();
        }

        public TrainingCourse GetTrainingCourse(int id)
        {
            return Context.TrainingCourses
                .Include(x => x.TrainingCourseModality)
                .FirstOrDefault(x => x.Id == id);
        }

        public TrainingCourse SaveTrainingCourse(TrainingCourse trainingCourse)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.TrainingCourses.AddOrUpdate(trainingCourse);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return trainingCourse;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public TrainingCourse UpdateTrainingCourse(int id, TrainingCourse trainingCourse)
        {
            return SaveTrainingCourse(trainingCourse);
        }

        public bool DeleteTrainingCourse(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var trainingCourseFind = Context.TrainingCourses.Find(id);
                    if (trainingCourseFind == null)
                        return false;

                    Context.TrainingCourses.Remove(trainingCourseFind);
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

        public List<TrainingCourse> GetTrainingCourses(int? trainingCourse)
        {
            return Context.TrainingCourses.
                Where(x => x.ReportsTo == trainingCourse).ToList();
        }
    }
}
