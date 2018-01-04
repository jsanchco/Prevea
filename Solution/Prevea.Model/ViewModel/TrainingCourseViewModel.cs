namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class TrainingCourseViewModel
    {
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
        public string TrainingCourseModalityName { get; set; }
        public bool HasChildren { get; set; }
    }
}
