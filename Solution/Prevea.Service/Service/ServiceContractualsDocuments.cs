namespace Prevea.Service.Service
{
    #region Using

    using System.Collections.Generic;
    using IService.IService;
    using Model.Model;
    using System;
    using System.Linq;

    #endregion

    public partial class Service
    {
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
                                                          (int) EnContractualDocumentType.Oferta);
                string type;
                if (hasOferta == null)
                {
                    contractualDocument.ContractualDocumentTypeId = (int) EnContractualDocumentType.Oferta;
                    type = "OF";
                }
                else
                {
                    var hasContrato = contractualsDocuments.FirstOrDefault(x => x.ContractualDocumentTypeId ==
                                                                                (int)EnContractualDocumentType.Contrato);
                    if (hasContrato == null)
                    {
                        contractualDocument.ContractualDocumentTypeId = (int) EnContractualDocumentType.Contrato;
                        type = "CO";
                    }
                    else
                    {
                        contractualDocument.ContractualDocumentTypeId = (int)EnContractualDocumentType.Anexo;
                        type = "AN";
                    }
                }
                contractualDocument.Enrollment =
                    $"{type}_{Repository.GetContractualsDocuments().Count + 1:00000}/{contractualDocument.BeginDate.Year}";
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

        public bool DeleteContractualDocument(int contractualDocumentId)
        {
            return Repository.DeleteContractualDocument(contractualDocumentId);
        }
    }
}
