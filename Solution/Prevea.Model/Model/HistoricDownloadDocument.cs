namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System;

    #endregion

    public class HistoricDownloadDocument
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }

        [Required]
        public int UserId { get; set; }        
        public virtual User User { get; set; }

        #region Constructor

        public HistoricDownloadDocument()
        {
            Date = DateTime.Now;
        }

        #endregion
    }
}
