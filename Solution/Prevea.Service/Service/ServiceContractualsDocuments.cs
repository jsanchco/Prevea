namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Linq;
    using System.Web;
    using System.IO;

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

        public EnContractualDocumentType GetNewContractualDocumentType(int companyId)
        {
            var contractualsDocuments = Repository.GetContractualsDocuments(companyId);
            var hasOferta = contractualsDocuments.FirstOrDefault(x => x.ContractualDocumentTypeId ==
                                                                      (int)EnContractualDocumentType.Offer);
            if (hasOferta == null)
            {
                return EnContractualDocumentType.Offer;
            }

            var hasContrato = contractualsDocuments.FirstOrDefault(x => x.ContractualDocumentTypeId ==
                                                                        (int)EnContractualDocumentType.Contract);

            return hasContrato == null ? EnContractualDocumentType.Contract : EnContractualDocumentType.Annex;
        }

        public string VerifyNewContractualDocument(int companyId)
        {
            var error = string.Empty;
            string errorGeneralData;
            string errorModePayment;
            var company = Repository.GetCompany(companyId);
            var contractualDocumentType = GetNewContractualDocumentType(companyId);
            switch (contractualDocumentType)
            {
                case EnContractualDocumentType.Offer:
                    errorGeneralData = GetErrorInGeneralData(company);
                    if (!string.IsNullOrEmpty(errorGeneralData))
                    {
                        error += $"<H2 style='color: white;'>Error</H2><br />DATOS GENERALES<ul>{errorGeneralData}</ul>";
                    }
                    if (company.ContactPersons == null || company.ContactPersons.Count == 0)
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += "PERSONAS de CONTACTO<ul><li>Agregar persona de contacto</li></ul>";
                    }
                    errorModePayment = GetErrorInPaymentMethod(company); 
                    if (!string.IsNullOrEmpty(errorModePayment))
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += $"FORMA de PAGO<ul>{errorModePayment}</ul>";
                    }

                    break;

                case EnContractualDocumentType.Contract:
                    errorGeneralData = GetErrorInGeneralData(company);
                    if (!string.IsNullOrEmpty(errorGeneralData))
                    {
                        error += $"<H2>Error</H2><br />DATOS GENERALES<ul>{errorGeneralData}</ul>";
                    }
                    if (company.ContactPersons == null || company.ContactPersons.Count == 0)
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += "PERSONAS de CONTACTO<ul><li>Agregar persona de contacto</li></ul>";
                    }
                    errorModePayment = GetErrorInPaymentMethod(company);
                    if (!string.IsNullOrEmpty(errorModePayment))
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += $"FORMA de PAGO<ul>{errorModePayment}</ul>";
                    }

                    break;

                case EnContractualDocumentType.Annex:
                    errorGeneralData = GetErrorInGeneralData(company);
                    if (!string.IsNullOrEmpty(errorGeneralData))
                    {
                        error += $"<H2>Error</H2><br />DATOS GENERALES<ul>{errorGeneralData}</ul>";
                    }
                    if (company.ContactPersons == null || company.ContactPersons.Count == 0)
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += "PERSONAS de CONTACTO<ul><li>Agregar persona de contacto</li></ul>";
                    }
                    errorModePayment = GetErrorInPaymentMethod(company);
                    if (!string.IsNullOrEmpty(errorModePayment))
                    {
                        if (string.IsNullOrEmpty(error))
                            error += "<H2>Error</H2><br />";

                        error += $"FORMA de PAGO<ul>{errorModePayment}</ul>";
                    }

                    break;
            }

            return error;
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
            return $"{pathContractualDocument}.pdf";
        }

        private void CreateDirectoryIfNotExists(string directory)
        {
            var physicalPath = HttpContext.Current.Server.MapPath(directory);

            var exits = Directory.Exists(physicalPath);

            if (!exits && physicalPath != null)
                Directory.CreateDirectory(physicalPath);
        }

        private string GetErrorInGeneralData(Company company)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(company.Address))
                error += "<li>Seleccionar Dirección</li>";
            if (string.IsNullOrEmpty(company.Province))
                error += "<li>Seleccionar Provincia</li>";
            if (company.Cnae == null)
                error += "<li>Seleccionar Actividad</li>";

            return error;
        }

        private string GetErrorInPaymentMethod(Company company)
        {
            var error = string.Empty;

            if (company.PaymentMethod == null)
                return "Faltan datos por rellenar";

            if (company.PaymentMethod.ModePayment == null)
                error += "<li>Seleccionar Modalidad de Pago</li>";
            if (string.IsNullOrEmpty(company.PaymentMethod.AccountNumber))
                error += "<li>Seleccionar Nº de Cuenta</li>";

            return error;
        }
    }
}
