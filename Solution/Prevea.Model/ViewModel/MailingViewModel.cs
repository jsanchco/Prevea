namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class MailingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? SendDate { get; set; }
        public bool Sent { get; set; }
        public string Subject { get; set; }
        public string Mail { get; set; }
    }
}
