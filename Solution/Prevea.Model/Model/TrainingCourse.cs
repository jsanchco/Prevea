namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System;

    #endregion

    public class TrainingCourse
    {
        #region Constructor

        public TrainingCourse()
        {
            Date = DateTime.Now;
        }

        #endregion

        [Key, Required]
        public int Id { get; set; }

        public int? ReportsTo { get; set; }

        public bool IsRoot { get; set; }
        public bool IsFamily { get; set; }
        public bool IsTitle { get; set; }
        public bool IsCourse { get; set; }

        public int? Hours { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }

        public DateTime Date { get; set; }

        public int? TrainingCourseModalityId { get; set; }
        public virtual TrainingCourseModality TrainingCourseModality { get; set; }

        public bool HasChildren { get; set; }

        public virtual ICollection<TrainingCourseTrainingService> TrainingCoursesTrainingServices { get; set; }
    }    
}
