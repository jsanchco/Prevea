namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;

    #endregion

    public partial class Service
    {
        #region Generic

        public List<HistoricDownloadDocument> GetHistoricDownloadDocuments()
        {
            return Repository.GetHistoricDownloadDocuments();
        }

        public HistoricDownloadDocument SaveHistoricDownloadDocument(HistoricDownloadDocument historicDownloadDocument)
        {
            return Repository.SaveHistoricDownloadDocument(historicDownloadDocument);
        }

        #endregion

        public List<HistoricDownloadDocument> GetHistoricDownloadDocumentsByDocument(int documentId)
        {
            return Repository.GetHistoricDownloadDocumentsByDocument(documentId);
        }

        public List<HistoricDownloadDocument> GetHistoricDownloadDocumentsByUser(int userId)
        {
            return Repository.GetHistoricDownloadDocumentsByUser(userId);
        }
    }
}
