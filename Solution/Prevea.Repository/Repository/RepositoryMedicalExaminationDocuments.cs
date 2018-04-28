namespace Prevea.Repository.Repository
{
    #region Using

    using System.Collections.Generic;
    using Model.Model;
    using System.Linq;
    using System.Data.Entity;
    using System;
    using System.Data.Entity.Migrations;

    #endregion

    public partial class Repository
    {
        public List<MedicalExaminationDocuments> GetMedicalExaminationDocuments()
        {
            return Context.MedicalExaminationDocuments
                .Include(x => x.RequestMedicalExaminationEmployee)
                .Include(x => x.Document)
                .ToList();
        }

        public MedicalExaminationDocuments GetMedicalExaminationDocumentById(int id)
        {
            return Context.MedicalExaminationDocuments
                .Include(x => x.RequestMedicalExaminationEmployee)
                .Include(x => x.Document)
                .FirstOrDefault(x => x.Id == id);
        }

        public MedicalExaminationDocuments SaveMedicalExaminationDocument(MedicalExaminationDocuments medicalExaminationDocument)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.MedicalExaminationDocuments.AddOrUpdate(medicalExaminationDocument);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();

                    return medicalExaminationDocument;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool DeleteMedicalExaminationDocument(int id)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var medicalExaminationDocument = GetMedicalExaminationDocumentById(id);
                    Context.MedicalExaminationDocuments.Remove(medicalExaminationDocument);

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

        public List<MedicalExaminationDocuments> GetMedicalExaminationDocumentsByRequestMedicalExaminationEmployeeId(int requestMedicalExaminationEmployeeId)
        {
            return Context.MedicalExaminationDocuments
                .Include(x => x.RequestMedicalExaminationEmployee)
                .Include(x => x.Document)
                .Where(x => x.RequestMedicalExaminationEmployeeId == requestMedicalExaminationEmployeeId)
                .ToList();
        }
    }
}
