namespace Prevea.Model.ViewModel
{
    public class MedicalExaminationDocumentsViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Enrollment { get; set; }
        public int RequestMedicalExaminationEmployeeId { get; set; }
        public int MedicalExaminationDocumentTypeId { get; set; }
        public string MedicalExaminationDocumentTypeDescription { get; set; }
    }
}
