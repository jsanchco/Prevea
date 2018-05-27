namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class DataMail
    {
        [Key, Required]
        public int Id { get; set; }

        public int MailingId { get; set; }
        public virtual Mailing Mailing { get; set; }

        public int? CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public string EMail { get; set; }
        public string Data { get; set; }
        public string Observations { get; set; }

        public int DataMailStateId { get; set; }
        public virtual DataMailState DataMailState { get; set; }
    }
}
