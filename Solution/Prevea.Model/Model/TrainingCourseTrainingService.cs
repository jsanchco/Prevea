namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class TrainingCourseTrainingService
    {
        [Key, Required]
        public int Id { get; set; }

        public int AssistantsNumber { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }

        [Required]
        public int TrainingCourseId { get; set; }
        public virtual TrainingCourse TrainingCourse { get; set; }

        [Required]
        public int TrainingServiceId { get; set; }
        public virtual TrainingService TrainingService { get; set; }
    }
}
