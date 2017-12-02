namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Data.Entity;
    using System.Linq;
    using System;

    #endregion

    public partial class Repository
    {
        #region Generic

        public List<HistoricDownloadDocument> GetHistoricDownloadDocuments()
        {
            return Context.HistoricDownloadDocuments
                .Include(x => x.Document)
                .Include(x => x.Document.Area)
                .Include(x => x.User)
                .ToList();
        }

        public HistoricDownloadDocument SaveHistoricDownloadDocument(HistoricDownloadDocument historicDownloadDocument)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.HistoricDownloadDocuments.Add(historicDownloadDocument);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return historicDownloadDocument;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        #endregion

        public List<HistoricDownloadDocument> GetHistoricDownloadDocumentsByDocument(int documentId)
        {
            return Context.HistoricDownloadDocuments
                .Include(x => x.Document)
                .Include(x => x.User)
                .Where(x => x.DocumentId == documentId)
                .ToList();
        }

        public List<HistoricDownloadDocument> GetHistoricDownloadDocumentsByUser(int userId)
        {
            return Context.HistoricDownloadDocuments
                .Include(x => x.Document)
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .ToList();
        }
    }
}
