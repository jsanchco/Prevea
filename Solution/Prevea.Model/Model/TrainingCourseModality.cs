namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class TrainingCourseModality
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string Description => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnModePayment), Id));

        public virtual ICollection<TrainingCourse> TrainingCourses { get; set; }
    }

    public enum EnTrainingCourseModality { NotMapped, Presencial, OnLine }
}
