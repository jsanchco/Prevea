namespace Prevea.Model.ViewModel
{
    public class DataMailViewModel
    {
        public int Id { get; set; }
        public int MailingId { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorInitials { get; set; }
        public string EMail { get; set; }
        public string Data { get; set; }
        public string Observations { get; set; }
        public int DataMailStateId { get; set; }
        public string DataMailStateDescription { get; set; }
    }
}
