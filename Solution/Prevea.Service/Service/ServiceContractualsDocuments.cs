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
                if (contractualDocument.ContractualDocumentTypeId == (int) EnContractualDocumentType.Firmed)
                {
                    if (contractualDocument.ContractualDocumentCompanyParentId == null)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error en la Grabación del Documento",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                    var parent =
                        Repository.GetContractualDocument((int)contractualDocument.ContractualDocumentCompanyParentId);
                    contractualDocument.Enrollment = parent.Enrollment + "_F";
                }
                else
                {
                    var typeId = GetTypeEnrollmentInit(contractualDocument.ContractualDocumentTypeId);
                    if (typeId == string.Empty)
                    {
                        return new Result
                        {
                            Message = "Se ha producido un error en la Grabación del Documento",
                            Object = null,
                            Status = Status.Error
                        };
                    }
                    contractualDocument.Enrollment =
                        $"{typeId}_{Repository.GetContractualsDocuments().Count + 1:00000}/{contractualDocument.BeginDate.Year}";
                }

                contractualDocument.UrlRelative = GetUrlRelativeContractualDocument(contractualDocument);

                contractualDocument = Repository.SaveContractualDocument(contractualDocument);

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

        public string CanAddContractualDocument(int companyId, int contractualDocumentTypeId)
        {
            var contractualsDocuments = Repository.GetContractualsDocuments(companyId);

            string error = null;
            List<ContractualDocumentCompany> exist;
            switch (contractualDocumentTypeId)
            {
                case (int)EnContractualDocumentType.OfferSPA:
                    exist = contractualsDocuments.Where(
                        x => x.ContractualDocumentTypeId == (int) EnContractualDocumentType.OfferSPA).ToList();
                    if (exist.Count > 0)
                    {
                        error = "Existe una Oferta de SPA para esta Empresa";
                    }
                    break;
                case (int)EnContractualDocumentType.OfferGES:
                    exist = contractualsDocuments.Where(
                        x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.OfferGES).ToList();
                    if (exist.Count > 0)
                    {
                        error = "Existe una Oferta de Gestoría para esta Empresa";
                    }
                    break;
                case (int)EnContractualDocumentType.OfferFOR:
                    exist = contractualsDocuments.Where(
                        x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.OfferFOR).ToList();
                    if (exist.Count > 0)
                    {
                        error = "Existe una Oferta de Formación para esta Empresa";
                    }
                    break;
                case (int)EnContractualDocumentType.ContractSPA:
                    exist = contractualsDocuments.Where(
                        x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.ContractSPA).ToList();
                    if (exist.Count > 0)
                    {
                        error = "Existe un Contrato de SPA para esta Empresa";
                    }
                    break;
                case (int)EnContractualDocumentType.ContractGES:
                    exist = contractualsDocuments.Where(
                        x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.ContractGES).ToList();
                    if (exist.Count > 0)
                    {
                        error = "Existe un Contrato de Gestoría para esta Empresa";
                    }
                    break;
                case (int)EnContractualDocumentType.ContractFOR:
                    exist = contractualsDocuments.Where(
                        x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.ContractFOR).ToList();
                    if (exist.Count > 0)
                    {
                        error = "Existe un Contrato de Formación para esta Empresa";
                    }
                    break;
                case (int)EnContractualDocumentType.UnSubscribeContract:
                    exist = contractualsDocuments.Where(
                        x => x.ContractualDocumentTypeId == (int)EnContractualDocumentType.UnSubscribeContract).ToList();
                    if (exist.Count > 0)
                    {
                        error = "Existe un Contrato de Baja para esta Empresa";
                    }
                    break;
                case (int)EnContractualDocumentType.Annex:
                    error = string.Empty;
                    break;

                default:
                    error = "Debes seleccionar un Tipo de Documento Contractual válido";
                    break;
            }

            return error;
        }

        public string VerifyNewContractualDocument(int companyId, int contractualDocumentTypeId)
        {
            var error = string.Empty;
            string errorGeneralData;
            string errorModePayment;
            var company = Repository.GetCompany(companyId);
            if (company == null)
            {
                return "Empresa no encontrada";
            }

            var errorInAdd = CanAddContractualDocument(companyId, contractualDocumentTypeId);
            if (!string.IsNullOrEmpty(errorInAdd))
            {
                return errorInAdd;
            }

            switch (contractualDocumentTypeId)
            {
                case (int)EnContractualDocumentType.OfferSPA:
                case (int)EnContractualDocumentType.OfferGES:
                case (int)EnContractualDocumentType.OfferFOR:
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

                case (int)EnContractualDocumentType.ContractSPA:
                case (int)EnContractualDocumentType.ContractGES:
                case (int)EnContractualDocumentType.ContractFOR:
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

                case (int)EnContractualDocumentType.Annex:
                case (int)EnContractualDocumentType.UnSubscribeContract:
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

        private static string GetTypeEnrollmentInit(int contractualDocumentTypeId)
        {
            string typeId;
            switch (contractualDocumentTypeId)
            {
                case (int)EnContractualDocumentType.OfferSPA:
                    typeId = "OFSPA";
                    break;
                case (int)EnContractualDocumentType.OfferGES:
                    typeId = "OFGES";
                    break;
                case (int)EnContractualDocumentType.OfferFOR:
                    typeId = "OFFOR";
                    break;
                case (int)EnContractualDocumentType.ContractSPA:
                    typeId = "COSPA";
                    break;
                case (int)EnContractualDocumentType.ContractGES:
                    typeId = "COGES";
                    break;
                case (int)EnContractualDocumentType.ContractFOR:
                    typeId = "COFOR";
                    break;
                case (int)EnContractualDocumentType.Annex:
                    typeId = "AN";
                    break;
                case (int)EnContractualDocumentType.UnSubscribeContract:
                    typeId = "COBAJ";
                    break;
                default:
                    typeId = string.Empty;
                    break;
            }

            return typeId;
        }
    }
}
