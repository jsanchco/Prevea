namespace Prevea.Model.Model
{
    #region Using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class TrainingCourseFamilyType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<TrainingCourse> TrainingCourses { get; set; }
    }
}
