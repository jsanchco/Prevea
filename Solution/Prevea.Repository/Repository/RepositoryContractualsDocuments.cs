using System;

namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;

    #endregion

    public partial class Repository
    {
        public ContractualDocumentCompany GetContractualDocument(int contractualDocumentId)
        {
            return Context.ContractualsDocumentsCompany
                .Include(x => x.Company)
                .Include(x => x.ContractualDocumentType)
                .FirstOrDefault(x => x.Id == contractualDocumentId);
        }

        public List<ContractualDocumentCompany> GetContractualsDocuments(int? companyId = null)
        {
            if (companyId == null)
                return Context.ContractualsDocumentsCompany
                    .Include(x => x.Company)
                    .Include(x => x.ContractualDocumentType).ToList();

            return Context.ContractualsDocumentsCompany
                .Include(x => x.Company)
                .Include(x => x.ContractualDocumentType)
                .Where(x => x.CompanyId == companyId).ToList();
        }

        public ContractualDocumentCompany SaveContractualDocument(ContractualDocumentCompany contractualDocument)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.ContractualsDocumentsCompany.Add(contractualDocument);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return contractualDocument;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteContractualDocument(int contractualDocumentId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var contractualDocument = GetContractualDocument(contractualDocumentId);
                    Context.ContractualsDocumentsCompany.Remove(contractualDocument);

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
    }
}
