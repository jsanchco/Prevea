using System.IO;

namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Linq;
    using System.Web;

    #endregion

    public partial class Service
    {
        private const string COMPANIES = "~/App_Data/Companies";
        private const string CONTRACTUAL_DOCUMENTS_PATH = "ContractualDocuments";

        public ContractualDocumentCompany GetContractualDocument(int contractualDocumentId)
        {
            return Repository.GetContractualDocument(contractualDocumentId);
        }

        public List<ContractualDocumentCompany> GetContractualsDocuments(int? companyId = null)
        {
            return Repository.GetContractualsDocuments(companyId);
        }

        public Result SaveContractualDocument(ContractualDocumentCompany contractualDocument)
        {
            try
            {
                var contractualsDocuments = Repository.GetContractualsDocuments(contractualDocument.CompanyId);
                var hasOferta = contractualsDocuments.FirstOrDefault(x => x.ContractualDocumentTypeId ==
                                                          (int) EnContractualDocumentType.Offer);
                string type;
                if (hasOferta == null)
                {
                    contractualDocument.ContractualDocumentTypeId = (int) EnContractualDocumentType.Offer;
                    type = "OF";
                }
                else
                {
                    var hasContrato = contractualsDocuments.FirstOrDefault(x => x.ContractualDocumentTypeId ==
                                                                                (int)EnContractualDocumentType.Contract);
                    if (hasContrato == null)
                    {
                        contractualDocument.ContractualDocumentTypeId = (int) EnContractualDocumentType.Contract;
                        type = "CO";
                    }
                    else
                    {
                        contractualDocument.ContractualDocumentTypeId = (int)EnContractualDocumentType.Annex;
                        type = "AN";
                    }
                }
                contractualDocument.Enrollment =
                    $"{type}_{Repository.GetContractualsDocuments().Count + 1:00000}/{contractualDocument.BeginDate.Year}";
                contractualDocument.UrlRelative = GetUrlRelativeContractualDocument(contractualDocument);

                contractualDocument = Repository.SaveContractualDocument(contractualDocument);

                if (contractualDocument == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la Grabación del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La Grabación del Documento se ha producido con éxito",
                    Object = contractualDocument,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la Grabación del Documento",
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result UpdateContractualDocument(int contractualDocumentId, ContractualDocumentCompany contractualDocument)
        {
            try
            {
                contractualDocument = Repository.UpdateContractualDocument(contractualDocumentId, contractualDocument);

                if (contractualDocument == null)
                {
                    return new Result
                    {
                        Message = "Se ha producido un error en la actualización del Documento",
                        Object = null,
                        Status = Status.Error
                    };
                }

                return new Result
                {
                    Message = "La actualización del Documento se ha producido con éxito",
                    Object = contractualDocument,
                    Status = Status.Ok
                };
            }
            catch (Exception)
            {
                return new Result
                {
                    Message = "Se ha producido un error en la actualización del Documento",
                    Object = contractualDocument,
                    Status = Status.Error
                };
            }
        }

        public bool DeleteContractualDocument(int contractualDocumentId)
        {
            var contractualDocument = Repository.GetContractualDocument(contractualDocumentId);
            var physicalPath = HttpContext.Current.Server.MapPath(contractualDocument.UrlRelative);
            if (!RemoveFile(physicalPath))
                return false;

            return Repository.DeleteContractualDocument(contractualDocumentId);
        }

        private string GetUrlRelativeContractualDocument(ContractualDocumentCompany contractualDocument)
        {
            var pathContractualDocument = COMPANIES;
            CreateDirectoryIfNotExists(pathContractualDocument);

            var company = GetCompany(contractualDocument.CompanyId);
            pathContractualDocument += $"/{company.NIF}";
            CreateDirectoryIfNotExists(pathContractualDocument);

            pathContractualDocument += $"/{CONTRACTUAL_DOCUMENTS_PATH}";
            CreateDirectoryIfNotExists(pathContractualDocument);

            pathContractualDocument += $"/{contractualDocument.Enrollment.Substring(0, contractualDocument.Enrollment.Length - 5)}";
            //return HttpContext.Current.Server.MapPath($"{pathContractualDocument}.pdf");
            return $"{pathContractualDocument}.pdf";
        }

        private void CreateDirectoryIfNotExists(string directory)
        {
            var physicalPath = HttpContext.Current.Server.MapPath(directory);

            var exits = Directory.Exists(physicalPath);

            if (!exits && physicalPath != null)
                Directory.CreateDirectory(physicalPath);
        }
    }
}
