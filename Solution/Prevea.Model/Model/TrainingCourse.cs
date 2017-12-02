namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class TrainingCourse
    {
        [Key, Required]
        public int Id { get; set; }

        public int TrainingCourseFamilyTypeId { get; set; }
        public virtual TrainingCourseFamilyType TrainingCourseFamilyType { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int Hours { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int Modality { get; set; }
    }

    public enum EnTrainingCourseModality { NotMapped, Presencial, OnLine }
}
