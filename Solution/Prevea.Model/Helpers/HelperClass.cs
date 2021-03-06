﻿namespace Prevea.Model.Helpers
{
    #region Using

    using System;
    using System.Web;
    using System.IO;

    #endregion

    public static class HelperClass
    {
        public static bool HasFlag(Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException($"Enumeration type mismatch.  The flag is of type '{value.GetType()}', was expecting '{variable.GetType()}'.");
            }

            var num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);
        }

        public static string GetExension(string urlRelative)
        {
            var url = HttpContext.Current.Server.MapPath(urlRelative);
            var extension = Path.GetExtension(urlRelative);

            if (!File.Exists(url))
                extension = string.Empty;

            switch (extension)
            {
                case ".doc":
                case ".docx":
                    return "doc_opt.png";
                case ".xls":
                case ".xlsx":
                    return "xls_opt.png";
                case ".pdf":
                    return "pdf_opt.png";
                default:
                    return "unknown_opt.png";
            }
        }

        public static string GetDescription(string name)
        {
            switch (name)
            {
                case "Super":
                    return "Super";
                case "Admin":
                    return "Administrador";
                case "Library":
                    return "Bibliotecario";
                case "ContactPerson":
                    return "Persona de Contacto";
                case "Employee":
                    return "Trabajador";
                case "Agency":
                    return "Gestor";  
                case "Doctor":
                    return "Médico";
                case "PreveaPersonal":
                    return "Personal de Prevea";
                case "PreveaCommercial":
                    return "Comercial de Prevea";
                case "ExternalPersonal":
                    return "Personal Externo";
                case "FromSimulation":
                    return "Desde la Simulación";
                case "FromForeignPrevention":
                    return "Desde el Servicio de Prevención Ajeno";
                case "FromAgency":
                    return "Desde Gestoría";
                case "FromTraining":
                    return "Desde Formación";
                case "FromSede":
                    return "Desde SEDE";
                case "FromUser":
                    return "Desde Usuario";
                case "FromRole":
                    return "Desde Rol";
                case "FromRequestMedicalExamination":
                    return "Petición de Reconocimiento Médico";                    
                case "Issued":
                    return "Emitida";
                case "Assigned":
                    return "Asignada";
                case "ReAssigned":
                    return "Reasignada";
                case "Validated":
                    return "Validada";
                case "ValidationPending":
                    return "Pendiente de Validación";
                case "Modified":
                    return "Modificada";
                case "SendToCompany":
                    return "Enviada a Empresa";                    
                case "Deleted":
                    return "Borrado";
                case "Offer":
                    return "Oferta";
                case "Contract":
                    return "Contrato";
                case "Annex":
                    return "Anexo";
                case "ALaFirmaDelContrato":
                    return "A la Firma del Contrato";
                case "ALaRealizacion":
                    return "A la Realización";
                case "OnLine":
                    return "On Line";
                case "Otros":
                    return "Otros";
                case "Permanent":
                    return "Permanente";
                case "Society":
                    return "Sociedad";
                case "Autonomous":
                    return "Autónomo";
                case "UnSubscribe":
                    return "Baja";
                case "Firmed":
                    return "Documento Firmado";
                case "OfferSPA":
                    return "Oferta SPA";
                case "OfferGES":
                    return "Oferta Gestoría";
                case "OfferFOR":
                    return "Oferta Formación";
                case "ContractSPA":
                    return "Contrato SPA";
                case "ContractGES":
                    return "Contrato Gestoría";
                case "ContractFOR":
                    return "Contrato Formación";
                case "UnSubscribeContract":
                    return "Baja de Contrato";
                case "OtherDocuments":
                    return "Otros Documentos";
                case "Pending":
                    return "Pendiente";
                case "InProcess":
                    return "En Proceso";
                case "Finished":
                    return "Finalizado";
                case "Electrocardiogram":
                    return "Electrocardiograma";
                case "OpticalReport":
                    return "Informe Óptico";
                case "AudiometricReport":
                    return "Informe Audiométrico";
                case "Spirometry":
                    return "Espirometría";
                case "UrineAnalytics":
                    return "Análitica de Orina";
                case "BloodTest":
                    return "Análitica de Sangre";
                case "Others":
                    return "Otros";
                case "Blocked":
                    return "Bloqueada";
                case "Sent":
                    return "Enviado";
                case "Error":
                    return "Error";
                case "Open":
                    return "Abierta";
                case "ReOpen":
                    return "Re-Abierta";
                case "Closed":
                    return "Cerrada";
                case "Low":
                    return "Baja";
                case "Middle":
                    return "Media";
                case "High":
                    return "Alta";
                case "Critical":
                    return "Crítica";
                case "Baja":
                    return "Baja";
                case "LegalRepresentative":
                    return "Representante Legal";
                case "Invited":
                    return "Invitado";

                default:
                    return name;
            }
        }

        public static void WriteLog(string trace)
        {
            const string routeLog = @"C:\Log\PreveaWEB.txt";
            using (var file = new StreamWriter(routeLog, true))
            {
                trace = $"{DateTime.Now:dd-MM-yyyyy} -> {trace}";
                file.WriteLine(trace);
            }
        }
    }
}
