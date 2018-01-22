namespace Prevea.Model.ViewModel
{
    public class TrainingCourseTrainingServiceViewModel
    {
        public int Id { get; set; }
        public int AssistantsNumber { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? Desviation { get; set; }
        public int TrainingCourseId { get; set; }
        public string TrainingCourseName { get; set; }
        public int TrainingServiceId { get; set; }
        public decimal Total { get; set; }
    }
}
