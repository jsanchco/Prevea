namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    #endregion

    [NotMapped]
    public class HeaderEmployeeDocuments
    {
        public int RequestMedicalExaminationEmployeeId { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
