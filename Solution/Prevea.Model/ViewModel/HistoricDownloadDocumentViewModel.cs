namespace Prevea.Model.ViewModel
{
    #region Using

    using System;


    #endregion

    public class HistoricDownloadDocumentViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string UserName { get; set; }
        public string Icon { get; set; }
    }
}
