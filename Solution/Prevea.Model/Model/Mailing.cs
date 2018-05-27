namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;

    #endregion

    public class Mailing
    {
        [Key, Required]
        public int Id { get; set; }

        public string Name { get; set; }              
        public DateTime CreateDate { get; set; }
        public DateTime? SendDate { get; set; }
        public bool Sent { get; set; }
        public string Subject { get; set; }
        public string Mail { get; set; }

        public virtual ICollection<DataMail> DataMails { get; set; }
    }
}
