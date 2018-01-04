namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class TrainingCourseTrainingService
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int TrainingCourseId { get; set; }
        public TrainingCourse TrainingCourse { get; set; }

        [Required]
        public int TrainingServiceId { get; set; }
        public TrainingService TrainingService { get; set; }
    }
}
