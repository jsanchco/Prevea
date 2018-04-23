namespace Prevea.Repository.Repository
{
    #region Using

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;
    using Model.Model;

    #endregion

    partial class Repository
    {
        #region Generic

        public List<Document> GetDocuments()
        {
            return Context.Documents
                .Include(x => x.Area)
                .Include(x => x.DocumentState)
                .Include(x => x.Company)
                .OrderBy(x => x.Date)
                .ToList();
        }

        public Document GetDocument(int id)
        {
            return Context.Documents
                .Include(x => x.DocumentState)
                .Include(x => x.Area)
                .Include(x => x.Company)
                .FirstOrDefault(m => m.Id == id);
        }

        public Document SaveDocument(Document document)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Documents.Add(document);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return document;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Document SaveDocumentWithParent(Document document)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var documentsFind = Context.Documents.Where(l =>
                        l.Id == document.DocumentParentId || l.DocumentParentId == document.DocumentParentId).ToList();
                    foreach (var dcF in documentsFind)
                    {
                        if (dcF.DocumentStateId != 1)
                            continue;

                        dcF.DocumentStateId = 2;
                    }

                    Context.Documents.Add(document);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return document;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Document UpdateDocument(int id, Document document)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var documentFind = Context.Documents.Find(id);
                    if (documentFind == null)
                        return null;

                    Context.Entry(documentFind).CurrentValues.SetValues(document);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return document;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Document UnsubscribeDocument(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var documentFind = Context.Documents.Find(id);
                    if (documentFind == null)
                        return null;

                    documentFind.DocumentStateId = 3;

                    Context.Entry(documentFind).CurrentValues.SetValues(documentFind);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return documentFind;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public Document SubscribeDocument(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var documentFind = Context.Documents.Find(id);
                    if (documentFind == null)
                        return null;

                    documentFind.DocumentStateId = 1;

                    Context.Entry(documentFind).CurrentValues.SetValues(documentFind);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return documentFind;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteDocument(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var document = GetDocument(id);
                    var children = GetDocumentsByParent(id, document.DocumentParentId);
                    foreach (var child in children)
                        Context.Documents.Remove(child);

                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return true;
                }
            }
        }

        #endregion

        public List<Document> GetDocumentsByState(int documentStateId)
        {
            return Context.Documents
                .Include(x => x.DocumentState)
                .Include(x => x.Area)
                .Include(x => x.Company)
                .Where(x => x.DocumentStateId == documentStateId)
                .OrderBy(x => x.DateModification)
                .ToList();
        }

        public List<Document> GetDocumentsByParent(int id, int? parentId)
        {
            if (parentId != null)
            {
                return Context.Documents
                    .Include(x => x.DocumentState)
                    .Include(x => x.Area)
                    .Include(x => x.Company)
                    .Where(x => x.Id == parentId || x.DocumentParentId == parentId)
                    .OrderBy(l => l.Edition)
                    .ToList();
            }

            return Context.Documents
                .Include(x => x.DocumentState)
                .Include(x => x.Area)
                .Include(x => x.Company)
                .Where(x => x.DocumentStateId == 1 && x.Id == id)
                .OrderBy(l => l.Edition)
                .ToList();
        }

        public List<Document> GetChildrenDocument(int parentId)
        {
            return Context.Documents
                .Where(x => x.DocumentParentId == parentId)
                .ToList();
        }

        public int GetNumberDocumentsByArea(int areaId)
        {
            return Context.Documents
                .Include(x => x.Area)
                .Count(x => x.AreaId == areaId && x.DocumentStateId == 1);
        }

        public List<Document> GetDocumentsContractualsByCompany(int? companyId)
        {
            var areasName = new List<string> { "OFE", "CON", "ANX", "OTR", "UNS" };

            return Context.Documents
                .Include(x => x.DocumentState)
                .Include(x => x.Area)
                .Include(x => x.Company)
                .Where(x => areasName.Contains(x.Area.Name) && x.CompanyId == companyId)
                .ToList();
        }
    }
}

