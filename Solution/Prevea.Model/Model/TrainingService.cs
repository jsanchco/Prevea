namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    #endregion

    public class TrainingService
    {
        [ForeignKey("Simulation")]
        public int Id { get; set; }

        public string Observations { get; set; }

        public decimal Total { get; set; }

        public virtual Simulation Simulation { get; set; }

        public virtual ICollection<TrainingCourseTrainingService> TrainingCoursesTrainingServices { get; set; }
    }
}
