// ReSharper disable InconsistentNaming
using Prevea.Model.Model;

namespace Prevea.Service.Service
{
    #region Using

    using Prevea.IService.IService;
    using System.IO;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Linq;
    using System.Globalization;
    using System.Web;

    #endregion

    public partial class Service
    {
        public const string _CREATOR = "Servicio de Report (PREVEA)";
        public static readonly Font _STANDARFONT_10 = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        public static readonly Font _STANDARFONT_8 = FontFactory.GetFont(FontFactory.HELVETICA, 8);
        public static readonly Font _STANDARFONT_10_BOLD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

        public static readonly Font _STANDARFONT_10_BOLD_CUSTOMCOLOR =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, new BaseColor(144, 54, 25));

        public static readonly Font _STANDARFONT_14_BOLD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);

        public static readonly Font _STANDARFONT_14_BOLD_WHITE =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);

        public Result GenerateOfferSPAReport(Model.Model.Document documentSPA, string route)
        {
            try
            {
                var pdf = new Document(PageSize.LETTER);
                var path = Path.GetDirectoryName(HttpContext.Current.Server.MapPath(documentSPA.UrlRelative));
                Directory.CreateDirectory(path);
                var pdfWriter = PdfWriter.GetInstance(pdf,
                    new FileStream(HttpContext.Current.Server.MapPath(documentSPA.UrlRelative), FileMode.Create));
                var pageEventHelper = new PageEventHelper();
                pdfWriter.PageEvent = pageEventHelper;

                pdf.AddTitle(documentSPA.Name);
                pdf.AddCreator(_CREATOR);
                pdf.Open();

                pdf.Add(GetHeader(route));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(new Paragraph("OFERTA DE SERVICIO", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableN_DocumentOffer(documentSPA));
                pdf.Add(GetLineSeparator());
                pdf.Add(GetTableTitle("EMPRESA"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableCompanyData(documentSPA));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle("ESPECIALIDADES OBJETO DE LA OFERTA"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableEspecialidadesObjeto(documentSPA));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle("CENTROS DE TRABAJO"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetCentrosTrabajo(documentSPA));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle("OBSERVACIONES (nº de trabajadores, maquinaria y medios auxiliares, etc.,)"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                GetObservaciones(pdf, documentSPA);
                pdf.NewPage();
                pdf.Add(GetTableTitle("FORMA DE PAGO"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetFormaPago(documentSPA));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle("ACTIVIDADES OFERTADAS POR ESPECIALIDADES"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitleTransparent(
                    "SEGURIDAD EN EL TRABAJO, HIGIENE INDUSTRIAL, ERGONOMÍA Y PSICOSOCIOLOGÍA APLICADA"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                GetDescriptionSeguridadTrabajo(pdf);
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitleTransparent("VIGILANCIA DE LA SALUD"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                GetDescriptionVigilanciaSalud(pdf);
                pdf.NewPage();
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle("PRESUPUESTO"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitleTransparent("PRECIO"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                GetDescriptionPrecio(pdf, documentSPA);
                pdf.NewPage();
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableTitle("ACEPTACIÓN DE PRESUPUESTO"));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                GetAceptacionPresupuesto(pdf, documentSPA, route);

                pdf.Close();
                pdfWriter.Close();

                return new Result
                {
                    Message = $"Se generó correctamente el archivo PDF: {documentSPA.Name}",
                    Object = pdf,
                    Status = Status.Ok
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        public Result GenerateConractSPAReport(Model.Model.Document documentSPA, string route)
        {
            try
            {
                var pdf = new Document(PageSize.LETTER);
                var path = Path.GetDirectoryName(HttpContext.Current.Server.MapPath(documentSPA.UrlRelative));
                Directory.CreateDirectory(path);
                var pdfWriter = PdfWriter.GetInstance(pdf,
                    new FileStream(HttpContext.Current.Server.MapPath(documentSPA.UrlRelative), FileMode.Create));
                var pageEventHelper = new PageEventHelper();
                pdfWriter.PageEvent = pageEventHelper;

                pdf.AddTitle(documentSPA.Name);
                pdf.AddCreator(_CREATOR);
                pdf.Open();

                pdf.Add(GetHeader(route));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableN_DocumentContract(documentSPA));
                pdf.Add(new Chunk("\n"));
                pdf.Add(new Paragraph("CONTRATO PARA LA PRESTACIÓN DE SERVICIO DE PREVENCIÓN", _STANDARFONT_14_BOLD));
                pdf.Add(new Chunk("\n"));
                GetReunidos(pdf, documentSPA);
                pdf.Add(new Chunk("\n"));
                pdf.Add(new Chunk("\n"));
                GetManifiestan(pdf, documentSPA, route);

                pdf.Close();
                pdfWriter.Close();

                return new Result
                {
                    Message = $"Se generó correctamente el archivo PDF: {documentSPA.Name}",
                    Object = pdf,
                    Status = Status.Ok
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Object = null,
                    Status = Status.Error
                };
            }
        }

        private PdfPTable GetHeader(string route)
        {
            var pdfPTable = new PdfPTable(2) {WidthPercentage = 100};

            var image = Image.GetInstance($"{route}\\Images\\Logo_report.png");
            image.ScalePercent(50f);
            var pdfCellImage = new PdfPCell(image)
            {
                Rowspan = 4,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCellImage);

            var pdfCell = new PdfPCell(new Phrase("PREVEA SPA", _STANDARFONT_14_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("C/ Abad Juan Catalán, 38 28032-Madrid", _STANDARFONT_14_BOLD))
                {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Tfno.- 917602713", _STANDARFONT_14_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("prevea@preveaspa.com", _STANDARFONT_14_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableN_DocumentOffer(Model.Model.Document document)
        {
            var pdfPTable = new PdfPTable(2) {WidthPercentage = 40, HorizontalAlignment = 2};

            var pdfCell = new PdfPCell(new Phrase("Nº OFERTA", _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Name, _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("FECHA", _STANDARFONT_10_BOLD))
                {BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthTop = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Date.ToShortDateString(), _STANDARFONT_10_BOLD))
                {BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthTop = 0};
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableN_DocumentContract(Model.Model.Document document)
        {
            var pdfPTable = new PdfPTable(2) {WidthPercentage = 40, HorizontalAlignment = 2};

            var pdfCell = new PdfPCell(new Phrase("Nº CONTRATO", _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Name, _STANDARFONT_10_BOLD))
            {
                BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthBottom = 0
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("FECHA", _STANDARFONT_10_BOLD))
                {BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthTop = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Date.ToShortDateString(), _STANDARFONT_10_BOLD))
                {BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthTop = 0};
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private Chunk GetLineSeparator()
        {
            return new Chunk(
                new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_LEFT, 1));
        }

        private PdfPTable GetTableTitle(string title)
        {
            var pdfPTable = new PdfPTable(1) {WidthPercentage = 100};
            var pdfCell = new PdfPCell(new Phrase(title, _STANDARFONT_14_BOLD_WHITE))
            {
                BackgroundColor = new BaseColor(20, 66, 92),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableTitleTransparent(string title)
        {
            var pdfPTable = new PdfPTable(1) {WidthPercentage = 100};
            var pdfCell = new PdfPCell(new Phrase(title, _STANDARFONT_14_BOLD))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableCompanyData(Model.Model.Document document)
        {
            var pdfPTable = new PdfPTable(2) {WidthPercentage = 100};
            var widths = new[] {30f, 70f};
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("R.SOCIAL/AUTÓNOMO", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.Name, _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("CIF/NIF", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.NIF, _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("DOMICILIO SOCIAL", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.Address, _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("ACTIVIDAD", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.Cnae.Name, _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("CONTACTO", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            var contactPerson = document.Company.ContactPersons.FirstOrDefault(x =>
                x.ContactPersonTypeId == (int) EnContactPersonType.ContactPerson);
            pdfCell = contactPerson != null
                ? new PdfPCell(new Phrase($"{contactPerson.User.FirstName} {contactPerson.User.LastName}",
                    _STANDARFONT_10)) {BorderWidth = 0}
                : new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("CARGO", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = contactPerson != null
                ? new PdfPCell(new Phrase($"{contactPerson.User.WorkStationCustom}", _STANDARFONT_10)) {BorderWidth = 0}
                : new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("TELÉFONO", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = contactPerson != null
                ? new PdfPCell(new Phrase($"{contactPerson.User.PhoneNumber}", _STANDARFONT_10)) {BorderWidth = 0}
                : new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("E-MAIL", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = contactPerson != null
                ? new PdfPCell(new Phrase($"{contactPerson.User.Email}", _STANDARFONT_10)) {BorderWidth = 0}
                : new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableEspecialidadesObjeto(Model.Model.Document document)
        {
            var tecniches = "No";
            var health = "No";

            if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService != null)
            {
                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques !=
                    null)
                {
                    tecniches = "Si";
                }

                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                        .AmountHealthVigilance != null &&
                    document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                        .AmountMedicalExamination != null)
                {
                    health = "Si";
                }
            }

            var pdfPTable = new PdfPTable(4) {WidthPercentage = 100};
            var widths = new[] {40f, 10f, 40f, 10f};
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("SEGURIDAD EN EL TRABAJO", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(tecniches, _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("HIGIENE INDUSTRIAL", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(tecniches, _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("ERGONOMÍA Y PSICOSOCIOLOGÍA", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(tecniches, _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("MEDICINA DEL TRABAJO", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(health, _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private Phrase GetCentrosTrabajo(Model.Model.Document document)
        {
            if (document.CompanyId == null)
                return new Phrase(" ", _STANDARFONT_10_BOLD);

            var workCenters = GetWorkCentersByCompany((int) document.CompanyId)
                .Where(x => x.WorkCenterStateId == (int) EnWorkCenterState.Alta).ToList();
            var numberWorkCenters = workCenters.Count;

            var provincesWorkCenters = string.Empty;
            if (workCenters.Count > 0)
            {
                var distinctWorkCenters = workCenters
                    .GroupBy(x => x.Province.Trim())
                    .Select(g => new
                    {
                        Field = g.Key,
                        Count = g.Count()
                    }).ToList();


                if (distinctWorkCenters.Count == 1)
                {
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1
                        ? $"{distinctWorkCenters[0].Field.Trim()}."
                        : $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
                }
                else
                {
                    for (var i = 0; i < distinctWorkCenters.Count; i++)
                    {
                        var workCenter = distinctWorkCenters[i];
                        var newWorkCenter = workCenter.Field.Trim();
                        if (newWorkCenter != string.Empty)
                        {
                            if (i == distinctWorkCenters.Count - 1)
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1
                                    ? $"{newWorkCenter}."
                                    : $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1
                                    ? $"{newWorkCenter}, "
                                    : $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }

            var workCentersText = numberWorkCenters == 0
                ? "No tiene declarados Centros de Trabajo"
                : "Centros de trabajo permanentes y móviles con domicilio en ";

            var phrase = new Phrase(workCentersText, _STANDARFONT_10)
            {
                new Chunk(provincesWorkCenters, _STANDARFONT_10_BOLD)
            };

            return phrase;
        }

        private void GetObservaciones(Document pdf, Model.Model.Document document)
        {
            var phrase = new Phrase("TRABAJADORES DECLARADOS POR LA EMPRESA: ", _STANDARFONT_10)
            {
                new Chunk($"{document.Company.Employees.Count}", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);

            var paragraph = new Paragraph(
                "Aceptada la oferta y firmado el contrato, se realizará visita previa a centro/s comunicado/s por la empresa, a los efectos de verificación de la toma de datos que posteriormente servirá para la elaboración de la evaluación de riesgos y planificación preventiva, así como el resto de información que componen el plan de prevención",
                _STANDARFONT_10);
            pdf.Add(paragraph);
        }

        private PdfPTable GetFormaPago(Model.Model.Document document)
        {
            var pdfPTable = new PdfPTable(4) {WidthPercentage = 100};
            var widths = new[] {20f, 30f, 20f, 30f};
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("MÉTDODO PAGO", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.PaymentMethod.ModePayment.Description, _STANDARFONT_10))
                {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("COBRO R. MÉDICOS", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.PaymentMethod.ModePaymentMedicalExamination.Description,
                _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Nº CUENTA", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.PaymentMethod.AccountNumber, _STANDARFONT_10))
                {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private void GetDescriptionSeguridadTrabajo(Document pdf)
        {
            var phrase = new Phrase("·  Diseño y elaboración del ", _STANDARFONT_10)
            {
                new Chunk("Plan de Prevención", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                ", colaborando en el seguimiento del mismo: análisis de las necesidades relativas a la estructura organizativa de prevención de acuerdo con la legislación vigente y las características de ésta, con objeto de definir un sistema organizativo y los procedimientos que permitan gestionar la prevención de los riesgos laborales de una forma integral, eficaz, efectiva y fiable, mediante la programación de actividades.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Realización, revisión y/o actualización ", _STANDARFONT_10)
            {
                new Chunk("Evaluación de Riesgos ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "Laborales por puestos de trabajo, gestionando ésta mediante una aplicación informática propia: revisión del informe técnico de la Evaluación de Riesgos, evaluando los nuevos riesgos, en caso de existir, y reevaluando aquellos riesgos identificados con anterioridad y para los que ya se planificaron medidas preventivas. La Revisión de la Evaluación de los Riesgos no incluirá, a no ser que se especifiquen en la presente renovación, la realización de evaluaciones especificas determinados riesgos, tales como evaluaciones higiénicas, evaluaciones ergonómicas y psicosociales, análisis de adecuación de equipos de trabajo y análisis de instalaciones sometidas a reglamentación específica. Estas actuaciones serán propuestas y/o incluidas en la Planificación de la Actividad Preventiva y deberán ser contratadas adicionalmente.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            phrase = new Phrase("·  Diseño y elaboración de la ", _STANDARFONT_10)
            {
                new Chunk("Planificación de la Actividad Preventiva", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                ", gestionando ésta mediante una aplicación informática propia, y colaborando en el seguimiento de la implantación de las medidas correctoras y acciones propuestas: especificación en el informe técnico de las actividades preventivas a desarrollar, relacionadas con los riesgos identificados y valorados; incluyendo su priorización, propuesta de designación de responsables de su ejecución y recursos económicos.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Análisis de las posibles situaciones de emergencia y las medidas necesarias a adoptar en materia de primeros auxilios, lucha contra incendios y evacuación de los trabajadores.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Verificación de la Eficacia de las Medidas Preventivas: verificación, en función de la entidad de los riesgos y del tamaño de la empresa, del grado de implantación de las medidas programadas, mediante comprobación de las condiciones de seguridad del centro de trabajo y del seguimiento y control del sistema de gestión de prevención de riesgos laborales.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Se excluyen en estos precios los gastos correspondientes al laboratorio por analíticas de contaminantes en higiene industrial y estudios específicos de ergonomia y psicosociología aplicada.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Información a los trabajadores sobre los riesgos para la seguridad y salud en el trabajo, sobre las medidas y actividades de protección y prevención y sobre las medidas de emergencia. Queda incluido lo concerniente a la formación inicial de los trabajadores.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Asesoramiento y apoyo en la elaboración de normas de actuación en caso de emergencia.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Asesoramiento y apoyo para definir las necesidades formativas de los trabajadores, de manera que se puedan incluir en un plan de formación general que elabore la Empresa Contratante, de acuerdo a la evaluación de riesgos.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Asesoramiento en el control y gestión de seguridad en el sector de actividad que corresponda, así como en los aspectos relativos a la coordinación de actividades empresariales.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Elaboración de la ", _STANDARFONT_10)
            {
                new Chunk("Memoria Anual ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "a la finalización de la actividad, como resumen de las actividades de Prevención Técnica desarrolladas por PREVEA. Dado su carácter recapitulativo, se entregará transcurrido el período anual de referencia.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            phrase = new Phrase(
                "·  Diseño del sistema necesario para el registro y mantenimiento de la documentación que puede ser solicitada por cualquier administración, en materia de seguridad y salud, según las obligaciones establecidas en la legislación vigente.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Asesoramiento y apoyo para el cumplimiento de la obligación de realizar la investigación de accidentes e incidentes, siempre que sea requerido por la Empresa Contratante.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Evaluación de riesgos de las instalaciones fijas estipuladas en el presente contrato-concierto, incluyendo estudio ergonómico, de higiene industrial y psicología aplicada de dichas instalaciones.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Asesoría y asistencia jurídica relacionada con las especialidades preventivas contratadas.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Asesoramiento ante posibles inspecciones que se pudieran presentar a la Empresa Contratante relacionadas con las especialidades contratadas.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase(
                "·  Asesoramiento para la integración de la prevención de riesgos laborales en el conjunto de actividades y decisiones de la Empresa Contratante, así como para la elaboración de un Sistema de Gestión de prevención de riesgos laborales.",
                _STANDARFONT_10);
            pdf.Add(phrase);
        }

        private void GetDescriptionVigilanciaSalud(Document pdf)
        {
            var phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Planificación de Vigilancia de la Salud: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "planificación de las acciones necesarias para un eficaz control de los riesgos laborales sobre la salud de los trabajadores y establecimiento de las prioridades de las actividades sanitarias a realizar en función de la magnitud de los riesgos, del número de trabajadores expuestos y de la posible patología laboral asociada a los mismos.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Estudio descriptivo de los resultados obtenidos de los Exámenes de Salud Específicos",
                    _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                " con la finalidad de identificar problemas de salud abordables o modificables mediante intervenciones colectivas. Se elaborará un informe cuando se hayan realizado un mínimo de 10 Exámenes de Salud Específicos. Dado su carácter recapitulativo, podrá entregarse transcurrido el período anual de referencia.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Memoria Anual de Vigilancia de la Salud: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "resumen de las actividades preventivas desarrolladas por PREVEA en la empresa. Dado su carácter recapitulativo, podrá entregarse transcurrido el período anual de referencia.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Exámenes de Salud Específicos: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "práctica de exámenes de salud relacionados con los riesgos laborales que darán lugar a la elaboración de un informe médico personalizado que se entregará al trabajador, y a la valoración de la aptitud del trabajador para su puesto de trabajo, indicando, en su caso, la necesidad de adoptar medidas preventivas complementarias. Los exámenes de Salud están compuestos de: ",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("    o ", _STANDARFONT_10)
            {
                new Chunk("Analítica: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "Análisis de una muestra biológica de un trabajador (Sangre y Orina), mediante un proceso técnico, obteniéndose un resultado que facilita la valoración de su estado de salud. ",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("    o ", _STANDARFONT_10)
            {
                new Chunk("Pruebas Complementarias: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Control Visión: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "Determinación de la Agudeza Visual tanto cercana como lejana y un test de cromatografía.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Audiometría: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "Valoración de la agudeza auditiva de los trabajadores realizada en una cabina audiométrica analizando las siguientes frecuencias en ambos oídos: 250 Hz, 500 Hz, 1000 Hz, 2000 Hz, 3000Hz, 4000 Hz, 6000 Hz y 8000 Hz.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Espirometría: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(
                "Determinación de la Capacidad Vital Forzada de los trabajadores, las curvas de Flujo-Volumen y de Volumen-Tiempo.",
                _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Electrocardigrama: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("Según protocolo o criterio medico.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("    o ", _STANDARFONT_10)
            {
                new Chunk("Examen Médico: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Estudio Biométrico de los trabajadores: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("Peso, Talla, Índice de Masa Corporal, Tensión Arterial, Pulso.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Hábitos e Historia Médica", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Inspección general y por aparatos: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);

            phrase = new Phrase(
                "Oro-faringe, Tórax, Auscultación Pulmonar y Cardiaca, Abdomen, Sistema Músculo Esquelético, Sistema Venoso Periférico, Piel y Lesiones Dérmicas, Sistema Nervioso, Otoscopia.",
                _STANDARFONT_10);
            pdf.Add(phrase);
        }

        private void GetDescriptionPrecio(Document pdf, Model.Model.Document document)
        {
            var totalAmountTecniques = 0.0m;
            var totalAmountHealthVigilance = 0.0m;
            var totalAmountMedicalExamination = 0.0m;
            decimal total;

            var IVA = GetTagValue("IVA");
            if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService != null)
            {
                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques !=
                    null)
                {
                    totalAmountTecniques =
                        (decimal) document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                            .AmountTecniques * document.Company.SimulationCompanyActive.Simulation.NumberEmployees;
                }

                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                        .AmountHealthVigilance != null)
                {
                    totalAmountHealthVigilance =
                        (decimal) document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                            .AmountHealthVigilance *
                        document.Company.SimulationCompanyActive.Simulation.NumberEmployees;
                }

                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                        .AmountMedicalExamination != null)
                {
                    totalAmountMedicalExamination =
                        (decimal) document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                            .AmountMedicalExamination *
                        document.Company.SimulationCompanyActive.Simulation.NumberEmployees;
                }
            }

            if (document.Company.PaymentMethod.ModePaymentMedicalExaminationId ==
                (int) EnModePaymentMedicalExamination.ALaFirmaDelContrato)
            {
                total = totalAmountTecniques * Convert.ToDecimal(IVA) +
                        totalAmountHealthVigilance * Convert.ToDecimal(IVA) + totalAmountMedicalExamination;
            }
            else
            {
                total = totalAmountTecniques * Convert.ToDecimal(IVA) +
                        totalAmountHealthVigilance * Convert.ToDecimal(IVA);
            }

            total = Math.Round(total, 2);

            var amountTecniques = 0.0m;
            if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService != null &&
                document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques != null)
            {
                amountTecniques = (decimal) document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService
                    .AmountTecniques;
            }

            var phrase =
                new Phrase(
                    "Como contraprestación económica por las citadas actividades, la Empresa Contratante abonará a PREVEA las siguientes cantidades:",
                    _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            var pdfPTable = new PdfPTable(4) {WidthPercentage = 100};
            var widths = new[] {40f, 10f, 25f, 25f};
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("ESPECIALIDADES TÉCNICAS", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(amountTecniques.ToString(CultureInfo.InvariantCulture), _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("€/Trabajador", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("(IVA NO INCLUIDO)", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdf.Add(pdfPTable);
            pdf.Add(new Chunk("\n"));

            var amountHealthVigilance = 0.0m;
            if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService != null &&
                document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountHealthVigilance !=
                null)
            {
                amountHealthVigilance = (decimal) document.Company.SimulationCompanyActive.Simulation
                    .ForeignPreventionService.AmountHealthVigilance;
            }

            pdfPTable = new PdfPTable(4) {WidthPercentage = 100};
            widths = new[] {40f, 10f, 25f, 25f};
            pdfPTable.SetWidths(widths);

            pdfCell = new PdfPCell(new Phrase("VIGILANCIA DE LA SALUD", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(amountHealthVigilance.ToString(CultureInfo.InvariantCulture),
                _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("€/Trabajador", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("(IVA NO INCLUIDO)", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdf.Add(pdfPTable);
            pdf.Add(new Chunk("\n"));

            var amountMedicalExamination = 0.0m;
            if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService != null &&
                document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountMedicalExamination !=
                null)
            {
                amountMedicalExamination = (decimal) document.Company.SimulationCompanyActive.Simulation
                    .ForeignPreventionService.AmountMedicalExamination;
            }

            pdfPTable = new PdfPTable(4) {WidthPercentage = 100};
            widths = new[] {40f, 10f, 25f, 25f};
            pdfPTable.SetWidths(widths);

            pdfCell = new PdfPCell(new Phrase("REC. MÉDICO REALIZADO EN CLÍNICA", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(amountMedicalExamination.ToString(CultureInfo.InvariantCulture),
                _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("€/Trabajador", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("(IVA NO INCLUIDO)", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdf.Add(pdfPTable);

            var textModePaymentMedicalExamination = string.Empty;
            if (document.Company.PaymentMethod.ModePaymentMedicalExaminationId ==
                (int) EnModePaymentMedicalExamination.ALaFirmaDelContrato)
            {
                textModePaymentMedicalExamination =
                    "(Los reconocimientos médicos se facturarán a la firma del contrato)";
            }

            if (document.Company.PaymentMethod.ModePaymentMedicalExaminationId ==
                (int) EnModePaymentMedicalExamination.ALaRealizacion)
            {
                textModePaymentMedicalExamination = "(Los reconocimientos médicos se facturarán una vez realizados)";
            }

            phrase = new Phrase(textModePaymentMedicalExamination, _STANDARFONT_10);
            pdf.Add(phrase);

            var iva = IVA.Substring(2) + "%";
            decimal sum = totalAmountTecniques * Convert.ToDecimal(IVA) +
                          totalAmountHealthVigilance * Convert.ToDecimal(IVA);
            sum = Math.Round(sum, 2);

            pdfPTable = new PdfPTable(2) {WidthPercentage = 100};
            widths = new[] {45f, 55f};
            pdfPTable.SetWidths(widths);

            pdfCell = new PdfPCell(new Phrase(
                $"({amountTecniques} * {document.Company.SimulationCompanyActive.Simulation.NumberEmployees} * {iva}) + ({amountHealthVigilance} * {document.Company.SimulationCompanyActive.Simulation.NumberEmployees} * {iva}) = {sum} €",
                _STANDARFONT_10))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("ESPECIALIDADES TÉCNICAS + V. SALUD (I.V.A. incluido)", _STANDARFONT_10))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(
                $"({amountMedicalExamination} * {document.Company.SimulationCompanyActive.Simulation.NumberEmployees}) = {totalAmountMedicalExamination} €",
                _STANDARFONT_10))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("RECONOCIMIENTOS MÉDICOS (Exentos de I.V.A.) ", _STANDARFONT_10))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdf.Add(pdfPTable);
            pdf.Add(new Chunk("\n"));

            pdfPTable = new PdfPTable(3) {WidthPercentage = 100};
            widths = new[] {25f, 25f, 50f};
            pdfPTable.SetWidths(widths);

            pdfCell = new PdfPCell(new Phrase("TOTAL:", _STANDARFONT_10_BOLD))
            {
                BorderWidth = 0,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase($"{total} €", _STANDARFONT_10))
            {
                BorderWidth = 0,
                BackgroundColor = new BaseColor(204, 204, 255),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("(I.V.A. incluido)", _STANDARFONT_10))
            {
                BorderWidth = 0,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 2f,
                PaddingBottom = 6f
            };
            pdfPTable.AddCell(pdfCell);
            pdf.Add(pdfPTable);
        }

        private void GetAceptacionPresupuesto(Document pdf, Model.Model.Document document, string route)
        {
            var pdfPTable = new PdfPTable(2) {WidthPercentage = 100};
            var widths = new[] {30f, 70f};
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("EMPRESA", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.Name, _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("NOMBRE", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            var contactPerson = document.Company.ContactPersons.FirstOrDefault(x =>
                x.ContactPersonTypeId == (int) EnContactPersonType.ContactPerson);
            pdfCell = contactPerson != null
                ? new PdfPCell(new Phrase($"{contactPerson.User.FirstName} {contactPerson.User.LastName}",
                    _STANDARFONT_10)) {BorderWidth = 0}
                : new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("CARGO", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);
            pdfCell = contactPerson != null
                ? new PdfPCell(new Phrase($"{contactPerson.User.WorkStationCustom}", _STANDARFONT_10)) {BorderWidth = 0}
                : new PdfPCell(new Phrase(" ", _STANDARFONT_10)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            pdf.Add(pdfPTable);
            pdf.Add(new Chunk("\n"));

            var paragraph = new Paragraph("FIRMA Y SELLO", _STANDARFONT_10_BOLD)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdf.Add(paragraph);
            pdf.Add(new Chunk("\n"));

            pdfPTable = new PdfPTable(2) {WidthPercentage = 100};
            widths = new[] {50f, 50f};
            pdfPTable.SetWidths(widths);

            if (document.Signature == null)
            {
                pdfCell = new PdfPCell(new Phrase("", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
                pdfPTable.AddCell(pdfCell);
            }
            else
            {
                using (var memoryStream = new MemoryStream(document.Signature))
                {
                    var imageFirm = Image.GetInstance(memoryStream);
                    imageFirm.ScalePercent(50f);
                    pdfCell = new PdfPCell(imageFirm)
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };
                    pdfPTable.AddCell(pdfCell);
                }
            }

            var image = Image.GetInstance($"{route}\\Images\\companySeal.png");
            image.ScalePercent(50f);
            var pdfCellImage = new PdfPCell(image)
            {
                BorderWidth = 0,
                HorizontalAlignment = Element.ALIGN_CENTER,
            };
            pdfPTable.AddCell(pdfCellImage);

            pdfCell = new PdfPCell(new Phrase("", _STANDARFONT_10_BOLD)) {BorderWidth = 0};
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase($"FECHA  {DateTime.Now.ToShortDateString()}", _STANDARFONT_10))
            {
                BorderWidth = 0,
                HorizontalAlignment = Element.ALIGN_CENTER,
            };
            pdfPTable.AddCell(pdfCell);

            pdf.Add(pdfPTable);
        }

        private void GetReunidos(Document pdf, Model.Model.Document document)
        {
            var phrase = new Phrase($"En la ciudad de MADRID a {DateTime.Now.ToLongDateString()}", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("REUNIDOS", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("De una parte ", _STANDARFONT_10)
            {
                new Chunk("D. VIRGILIO CARRASCO MARTINEZ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(", mayor de edad, con D.N.I. número ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase("51919038B", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(", actuando en nombre y representación de la Entidad ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase("PREVEA CONSULTORES Y PROYECTOS, S.L.", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase("., con C.I.F. número: ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase("B86252962", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(", con domicilio social sito en ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase("Calle Abad Juan Catalán Nº 38 2ºA", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(", en la ciudad de Madrid, acreditado por el Instituto Regional de Seguridad y Salud en el Trabajo de la Comunidad de Madrid como Servicio Ajeno de Prevención de Riesgos Laborales para las especialidades siguientes: Seguridad en el Trabajo, Higiene Industrial, y Ergonomía, Psicosociología Aplicada, y Medicina del Trabajo con número de expediente CM108-5 (en adelante PREVEA).", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Y de otra ", _STANDARFONT_10);
            pdf.Add(phrase);

            var legalRepresentative = document.Company.ContactPersons.FirstOrDefault(x =>
                x.ContactPersonTypeId == (int)EnContactPersonType.LegalRepresentative);

            string textLegalRepresentative;
            if (legalRepresentative != null)
            {
                textLegalRepresentative = $"{legalRepresentative.User.FirstName} {legalRepresentative.User.LastName}";
            }
            else
            {
                return;
            }

            phrase = new Phrase($"D. {textLegalRepresentative}", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(" mayor de edad, con D.N.I número: ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase($"{legalRepresentative.User.DNI}", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(", actuando en representación de la Entidad ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase(", actuando en representación de la Entidad ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase($"{legalRepresentative.Company.Name}", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(", con C.I.F. número: ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase($"{legalRepresentative.Company.NIF}", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(", con domicilio social sito en ", _STANDARFONT_10);
            pdf.Add(phrase);
            phrase = new Phrase($"{legalRepresentative.Company.Address}", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            phrase = new Phrase(" (en adelante EMPRESA CONTRATANTE).", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Ambas partes se reconocen con capacidad y representación suficientes y por ello:", _STANDARFONT_10);
            pdf.Add(phrase);
        }

        private void GetManifiestan(Document pdf, Model.Model.Document document, string route)
        {
            var phrase = new Phrase("MAIFIESTAN", _STANDARFONT_10_BOLD);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("MANIFIESTAN", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("I.- Que la Ley 31-995 de 8 de noviembre de Prevención de Riesgos Laborales establece los principios generales relativos a la prevención de los riesgos derivados de las condiciones de trabajo para tratar de establecer un adecuado nivel de protección de la seguridad y salud de los trabajadores", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("II.- De acuerdo con el artículo 10 del Reglamento de los Servicios de Prevención", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("III.- El Reglamento de los Servicios de Prevención", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("IV.- Que la entidad PREVEA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("V.- Que la EMPRESA CONTRATANTE tras estudiar las distintas modalidades organizativas recogidas en la Ley 31-995", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("En consecuencia", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("ACUERDAN", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Suscribir el correspondiente contrato para la prestación de servicio de prevención", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("CLAUSULAS", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PRIMERA: OBJETO DEL CONTRATO. El objeto del presente contrato son las actividades preventivas de asesoramiento y apoyo que PREVEA desarrollará para la EMPRESA CONTRATANTE en las condiciones establecidas en el ANEXO I al presente concierto. La ejecución de dichas actividades se realizará de forma programada a lo largo de todo el periodo de vigencia del Concierto.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("La EMPRESA CONTRATANTE", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("El alcance de las actividades preventivas incluidas en el presente concierto se limita a las expresamente descritas en el ANEXO I.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("SEGUNDA: OBLIGACIONES DE LA EMPRESA CONTRATANTE. Para el desarrollo de las actuaciones pactadas", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("a) Permitir el acceso al centro o centros de trabajo de las personas que", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("b) Comunicar a PREVEA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("c) Facilitar al servicio de prevención PREVEA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("d) Poner a disposición de PREVEA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("e) Firmar la recepción de informes y recomendaciones emitidos por PREVEA.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("f) Facilitar cualquier información no contemplada en los supuestos anteriores y que con criterio técnico de las personas que vayan a emitir el asesoramiento y apoyo al empresario", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("g) Integrar la actividad preventiva", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("h) La empresa contratante manifiesta que asume directamente y bajo su total responsabilidad", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("i) Comunicar a PREVEA aquellos posibles riesgos no detectados en las evaluaciones y que son conocidos por la EMPRESA CONTRATANTE", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("j) Comunicar a PREVEA de manera fehaciente los accidentes de trabajo que se produzcan durante la vigencia de este contrato.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("k) Comunicar a PREVEA las actividades o funciones realizadas con recursos propios o con otros recursos ajenos", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("l) Comunicar a PREVEA los daños a la salud derivados del trabajo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("m) Comunicar a PREVEA de la existencia o apertura de centros de trabajo sometidos a la normativa de Seguridad y Salud en obras de Construcción.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("TERCERA: OBLICACIONES DE PREVEA. Para el desarrollo de las actuaciones pactadas", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("a) Realizar las actividades preventivas para cada una de las especialidades contratadas recogidas en el ANEXO I al presente contrato.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("b) En caso de tener que revisar la evaluación de riesgos", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("c) PREVEA realizará", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("d) PREVEA realizará la memoria anual de actividades preventivas y la valoración de la efectividad de la integración de la prevención de riesgos laborales en el sistema general de gestión de la empresa a través de la implantación y aplicación del plan de prevención de riesgos laborales en relación con las actividades preventivas concertadas. Igualmente facilitará la memoria y la programación anual a las que se refiere el apartado artículo 39.2 de la Ley 31•995.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("e) PREVEA dedicará anualmente los recursos humanos y materiales necesarios para la realización de las actividades concertadas.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("f) PREVEA asesorará al empresario", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("g) PREVEA informará a la EMPRESA CONTRATANTE de las actividades preventivas concretas que sean legalmente exigibles y que no queden cubiertas por el concierto", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("h) Cuando la EMPRESA CONTRATANTE realice actividades o cuente con centros de trabajo sometidos a la normativa de Seguridad y Salud en obras de Construcción", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            phrase = new Phrase("CUARTA: VIGILANCIA DE LA SALUD. A tenor de lo estipulado en la Ley 31-995", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n")); pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Por lo expuesto en el párrafo precedente", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Si se tienen contratados RECONOCIMIENTOS MÉDICOS INDIVIDUALES", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Cuando los reconocimientos médicos concertados", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Una vez concertadas las fechas para efectuar los reconocimientos médicos", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("-En caso de no poder acudir a las citas concertadas", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("-Si la EMPRESA CONTRATANTE", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("-Asimismo y en cualquiera de los dos casos anteriores", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("-En el caso de que durante la vigencia del contrato", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("QUINTA: DURACIÓN DEL CONTRATO. El presente contrato se pacta", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("El presente contrato se prorrogará automáticamente por el mismo plazo determinado en el párrafo anterior una vez llegado la fecha de su vencimiento sin que medie preaviso o renuncia por cualquiera de las dos partes. Dicha renuncia se habrá de comunicar fehacientemente con una antelación de dos meses para que la misma sea efectiva.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("SEXTA: CONDICIONES ECONÓMICAS. Se estipula un precio de", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Ambas partes acuerdan expresamente que la vigencia de este contrato quedará condicionada al pago por parte de la EMPRESA CONTRATANTE de la cantidad acordada. La falta de pago", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("SÉPTIMA: FORMACIÓN E INFORMACIÓN. En relación al cumplimiento de los artículos 18 y 19 de la Ley 31-995", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("OCTAVA: CONTRATOS ANTERIORES. Con la firma del presente contrato", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("NOVENA: PROTECCIÓN DE DATOS. En virtud de lo dispuesto en el artículo 9 de la Ley Orgánica de Protección de Datos de carácter Personal", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("a) Que la EMPRESA CONTRATANTE es RESPONSABLE de un Fichero de Datos Personales de sus trabajadores.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("b) Que la EMPRESA CONTRATANTE pone el indicado Fichero a disposición de PREVEA para la prestación del servicio con ella contratado a los efectos del cumplimiento de la Ley 31-995 de 8 de noviembre de Prevención de Riesgos Laborales.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("c) Que el fichero de los trabajadores puesto a disposición de PREVEA está calificado como de nivel BASICO", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("d) Que de acuerdo con los expositivos precedentes", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Cláusula de Confidencialidad y de Encargado de Tratamiento", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("1.- PREVEA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.- La obligación de confidencialidad recogida en el presente contrato tendrá carácter indefinido", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("3.- PREVEA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("4.- PREVEA y EMPRESA CONTRATANTE asumen independientemente las responsabilidades que le corresponden a cada una de las partes en el cumplimiento de sus obligaciones de acuerdo con la Ley 15-.999 de Protección de Datos de Carácter Personal. Ambas partes acuerdan dejar indemne a la otra parte por los daños y perjuicios ocasionados en caso de incumplimiento de sus responsabilidades en relación con lo dispuesto en la Ley Orgánica 15•999 y de su Reglamento de Desarrollo el R.D. 1720/2007.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("5.- PREVEA realizará sus acciones como encargado de tratamiento de acuerdo a las instrucciones que le indique EMPRESA CONTRATANTE", _STANDARFONT_10);
            pdf.Add(phrase);


            pdf.NewPage();

            phrase = new Phrase("6.- A la finalización de la relación entre las partes", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("7.- La EMPRESA CONTRATANTE", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("DÉCIMA: FUERO. Para resolver cualquier diferencia que pudiera surgir de la aplicación del presente contrato", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("UNDÉCIMA: TRABAJADORES AUTÓNOMOS. En el caso de trabajadores autónomos (sin trabajadores a su cargo)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));

            var pdfPTable = new PdfPTable(2) { WidthPercentage = 100 };
            var widths = new[] { 50f, 50f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("Por la EMPRESA CONTRATANTE. S.L.   Por PREVEA CONSULTORES Y PROYECTOS", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Fdo: D.       Fdo: D. VIRGILIO CARRASCO MARTINEZ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            if (document.Signature == null)
            {
                pdfCell = new PdfPCell(new Phrase("", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
                pdfPTable.AddCell(pdfCell);
            }
            else
            {
                using (var memoryStream = new MemoryStream(document.Signature))
                {
                    var imageFirm = Image.GetInstance(memoryStream);
                    imageFirm.ScalePercent(50f);
                    pdfCell = new PdfPCell(imageFirm)
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };
                    pdfPTable.AddCell(pdfCell);
                }
            }

            var image = Image.GetInstance($"{route}\\Images\\companySeal.png");
            image.ScalePercent(50f);
            var pdfCellImage = new PdfPCell(image)
            {
                BorderWidth = 0,
                HorizontalAlignment = Element.ALIGN_CENTER,
            };
            pdfPTable.AddCell(pdfCellImage);
            pdf.Add(pdfPTable);

            pdf.Add(new Chunk("\n"));
            phrase = new Phrase(" ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("ANEXO \"I\" AL CONTRATO DE PREVENCIÓN DE RIESGOS LABORALES ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("1.-CENTROS DE TRABAJO CONTRATADOS", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("En desarrollo a la cláusula Primera del contrato suscrito por las partes", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("DOMICILIO SOCIAL: ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("CENTROS DE TRABAJO: ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Nº DE CENTROS: ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("NÚMERO TRABAJADORES ESPECIALIDADES TÉCNICAS: ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("NÚMERO DE TRABAJADORES EN VIGILANCIA DE LA SALUD COLECTIVA: ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("NÚMERO DE TRABAJADORES CON RECONOCIMIENTOS MÉDICOS INDIVIDUALES:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("ACTIVIDAD EMPRESARIAL: ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Los servicios concertados especificados en el presente Anexo I", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("La EMPRESA CONTRATANTE declara la veracidad de los datos anteriores", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("En caso de centros de trabajo de naturaleza ITINERANTE (centros sin localización fija)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("En caso de centros de trabajo sometidos a la normativa de Seguridad y Salud en obras de Construcción", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.- ESPECIALIDADES Y ACTIVIDADES CONCERTADAS.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Desde la firma del presente contrato", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1. Actividades preventivas incluidas con carácter general:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.1. Programación Anual de la Actividad Preventiva.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.2. Diseño y redacción del Plan de Prevención de Riesgos Laborales", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            phrase = new Phrase("2.1.3. Evaluación de los factores de riesgo que puedan afectar a la seguridad y salud de los trabajadores en los términos previstos en el artículo 16 de la Ley 31-995 de Prevención de Riesgos Laborales", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.4. Planificación de la Actividad preventiva y determinación de las prioridades en la adopción de las medidas preventivas adecuadas.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.5. Información de los Trabajadores de su puesto de trabajo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.6. Formación de los Trabajadores de su puesto de trabajo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.7. Medidas de Emergencia y primeros auxilios", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.8. Investigación de accidentes de trabajo y enfermedades profesionales.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.9. Valoración mediante mediciones puntuales de condiciones ambientales de ruido", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.10. Memoria anual de actividades y valoración de la efectividad de la integración de la prevención de riesgos laborales en el sistema general de gestión de la empresa a través de la implantación y aplicación del plan de prevención de riesgos laborales en relación con las actividades preventivas concertadas.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.11. Asesoramiento al empresario", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.12. Asesoramiento", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.1.13. Revisión de la Evaluación de Riesgos en los casos legalmente exigidos", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Todas estas actuaciones concertados están contratados para cada una de las ESPECIALIDADES TÉCNICAS; Seguridad en el Trabajo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("a) Las exclusiones establecidas en la cláusula tercera de este anexo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("b) La formación será impartida exclusivamente a los trabajadores incluidos en el concierto", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("c) En el supuesto de que durante el desarrollo de las actividades contratadas se entendiera por ambas partes la conveniencia o necesidad de llevar a cabo alguna otra no prevista en este Anexo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("d) La EMPRESA CONTRATANTE acepta expresamente que es su responsabilidad la integración del Plan de Prevención de Riesgos Laborales", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.2. Especialidades Preventivas de Seguridad en el Trabajo:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.2.1. Actividades generales: Con respecto a la Especialidad de SEGURIDAD EN EL TRABAJO", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PREVEA se compromete a comunicar a la EMPRESA CONTRATANTE la necesidad de realizar otras actividades específicas cuando resulte legalmente exigibles", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.2.2. Otras actividades específicas:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Documento de Protección ATEX (conforme R.D. 681/2003)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Planes Amianto (conforme 396/2006)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Planes Seguridad en Obras de Construcción (R.D. 1627-997)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Coordinación de Actividades Empresariales", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Asistencia en calidad de Recurso Preventivo de la EMPRESA CONTRATANTE.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Realización de Planes de Autoprotección (conforme R.D. 393/2007)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Documento de Seguridad en el ámbito de la Industria Extractiva.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Visitas de chequeo a obras o instalaciones concretas y emisión de sus respectivos informes.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Realización de visitas no programadas de seguimiento", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Las actividades recogidas en el apartado 2.2.2.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.3. Especialidades Preventivas de Higiene Industrial:", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.3.1. Actividades generales: Con respecto a la especialidad de HIGIENE INDUSTRIAL", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PREVEA se compromete a comunicar a la EMPRESA CONTRATANTE la necesidad de realizar otras actividades específicas cuando resulte legalmente exigibles", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.3.2. Otras actividades específicas:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Mediciones de agentes físicos (ruido", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Mediciones de contaminantes químicos y realización de sus respectivos estudios o informes", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Estudios de exposición a riesgos biológicos y realización de sus respectivos informes", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Realización de visitas no programadas de seguimiento", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Las actividades recogidas en el apartado 2.3.2.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.4. Especialidades Preventivas de Ergonomía y Psicosociología Aplicada:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.4.1. Actividades generales: Con respecto a la especialidad de ERGONOMIA Y PSICOSOCIOLOGIA APLICADA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PREVEA se compromete a la revisión de la Evaluación de Riesgos en los casos legalmente exigidos", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PREVEA se compromete a comunicar a la EMPRESA CONTRATANTE la necesidad de realizar otras actividades específicas cuando resulte legalmente exigibles", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.4.2. Otras actividades específicas:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Estudios específicos de Ergonomía: Estudios de Manipulación Manual de Cargas", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Estudios Psicosociales.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Mediciones de agentes físicos ambientales y emisión de sus respectivos estudios o informes.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Realización de visitas no programadas de seguimiento", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Las actividades recogidas en el apartado 2.4.2.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.5. Especialidad Preventiva de Medicina en el Trabajo. Con respecto a la especialidad de Medicina del Trabajo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.5.1. Vigilancia de la Salud Colectiva. Se realizarán las siguientes actividades:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Análisis con criterios epidemiológicos", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Análisis de las ausencias que", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Proporcionar información a los trabajadores en relación con los efectos para la salud derivados de los riesgos del trabajo y realizar actividades formativas en primeros auxilios.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Colaboración con los servicios de atención primaria de salud y de asistencia sanitaria especializada para el diagnóstico", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Estudio y valoración de los riesgos que puedan afectar a las trabajadoras embarazadas o en situación de parto reciente", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Resumen de las actividades realizadas durante el concierto en la memoria anual de actividades.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2.5.2. Reconocimientos Médicos Individuales: Se realizarán las siguientes actuaciones:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Realización de los exámenes de salud de los trabajadores", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Entrega al Empresario de la información sobre la Aptitud del trabajador para desempeñar el puesto de trabajo y de las conclusiones de los exámenes de salud con objeto de mejorar las medidas de prevención y protección de los trabajadores.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Entrega del resultado de los exámenes de salud dirigidos a los trabajadores", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Las pruebas que se realizarán al contratar los reconocimientos médicos individuales", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            phrase = new Phrase("2.5.3. Otras Pruebas Complementarias:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• Pruebas complementarias o parámetros analíticos suplementarios que se tengan que realizar en función de lo estipulado en el protocolo médico específico a aplicar en cada caso", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Pruebas de radio diagnóstico.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Las pruebas complementarias recogidas en el apartado 2.5.3", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("3.-ACTIVIDADES EXCLUIDAS:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("No se incluyen en el presente concierto las siguientes actividades", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• Certificaciones de instalaciones o maquinaría", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• Asesoramiento jurídico frente a frente a requerimientos", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• Actividades preventivas que afecten a instalaciones o a trabajadores situadas o que presten servicio fuera del ámbito territorial de actuación de PREVEA.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Otras exclusiones:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• No incluirá", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("4.-CONDICIONES ECONÓMICAS:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("La empresa contratante se compromete a satisfacer a PREVEA.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("•Actividades correspondientes A LAS ESPECIALIDADES TECNICAS DE SEGURIDAD", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("1 Especialidades Técnicas 10 Trabajadores 45", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("1 Vigilancia de Salud 10 Trabajadores 20", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2 Reconocimiento Medico 10 Trabajadores 50", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("1- A los que habrá que añadir el 21% de IVA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("2- Exentos de IVA ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Los reconocimientos médicos serán abonados:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("A la realización A la Firma del Contrato", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase(" ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("5. FORMA DE PAGO.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• La forma de pago", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• El pago se realizará por RECIBO BANCO", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("• Nº C.C. ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("    ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("6. IMPUESTOS.• Todos los impuestos que graven la prestación de los servicios concertados mediante la suscripción del presente contrato serán de cuenta de la empresa contratante.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Por la EMPRESA CONTRATANTE", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            phrase = new Phrase("ANEXO DE AMPLIACION DE SERVICIOS CONCERTADOS", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("AL CONTRATO DE SERVICIO DE PREVENCIÓN CON FECHA: 27-0/2016", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PRIMERO.• La Entidad __________________________", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("SEGUNDO.• Desde la firma del presente ANEXO del contrato de Servicio de Prevención", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("SERVICIOS CONCERTADOS", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("SERVICIO DE PREVENCION PREVEA no se responsabiliza de la no subsanación por parte de la entidad o personal responsable de la empresa contratante", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("TERCERO.• En el supuesto de que durante el desarrollo de las actividades contratados se entendiera por ambas partes la conveniencia de llevar a cabo alguna otra no prevista en este Anexo", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("CUARTO.• Todos los impuestos que graven la prestación de los servicios concertados mediante la suscripción del presente contrato serán de cuenta de la empresa contratante.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Por la EMPRESA CONTRATANTE", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("CARTA DE GARANTÍA DE PRESTACIÓN DE SERVICIOS DE PREVEA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PROGRAMACIÓN ANUAL PARA LA ACTIVIDAD", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("La Entidad _________________________planifica con la Empresa __________________________ la realización de la Formación", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("VIGILANCIA DE LA SALUD", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Citas fijadas:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Fecha y Hora Trabajadores Lugar", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Rechazo las citas de Vigilancia de la Salud no solicitando nuevas citas", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("FORMACIÓN PRESENCIAL DE LOS TRABAJADORES", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Citas fijadas:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Fecha y Hora Trabajadores Lugar y Tipo de Curso", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Formación en la propia empresa el día de la visita técnica", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("VISITAS DEL TÉCNICO DE PREVENCIÓN", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Cita fijada:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Toma de Datos:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("En caso de que", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("La Empresa __________________________", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Fdo:  Fdo:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PREVEA  LA EMPRESA.", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("(Lugar Y Fecha)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("___ de _________ de 201_", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase(" ", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PROGRAMACIÓN ANUAL PARA LA ACTIVIDAD", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("La Entidad PREVEA CONSULTORES Y PROYECTOS", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("VIGILANCIA DE LA SALUD", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Citas fijadas:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Fecha y Hora Trabajadores Lugar", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Rechazo las citas de Vigilancia de la Salud no solicitando nuevas citas", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("FORMACIÓN PRESENCIAL DE LOS TRABAJADORES", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Citas fijadas:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Fecha y Hora Trabajadores Lugar y Tipo de Curso", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Formación en la propia empresa el día de la visita técnica", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("VISITAS DEL TÉCNICO DE PREVENCIÓN", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Cita fijada:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Toma de Datos:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("En caso de que", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("La Empresa __________________________", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("Fdo:           Fdo:", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("PREVEA          LA EMPRESA", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("(Lugar Y Fecha)", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
            phrase = new Phrase("____ de __________ de 201_", _STANDARFONT_10);
            pdf.Add(phrase);
            pdf.Add(new Chunk("\n"));
        }
    }

    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte _cb;
        PdfTemplate _template;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            _cb = writer.DirectContent;
            _template = _cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            var pageN = writer.PageNumber;
            var text = pageN.ToString();
            var len = Service._STANDARFONT_8.BaseFont.GetWidthPoint(text, Service._STANDARFONT_8.Size);
            var pageSize = document.PageSize;

            _cb.SetRGBColorFill(100, 100, 100);

            _cb.BeginText();
            _cb.SetFontAndSize(Service._STANDARFONT_8.BaseFont, Service._STANDARFONT_8.Size);
            //cb.SetTextMatrix(document.LeftMargin, pageSize.GetBottom(document.BottomMargin));
            var position = Convert.ToInt32(pageSize.Width / 2) - 4;
            _cb.SetTextMatrix(position, pageSize.GetBottom(document.BottomMargin));
            _cb.ShowText(text);

            _cb.EndText();

            _cb.AddTemplate(_template, document.LeftMargin + len, pageSize.GetBottom(document.BottomMargin));
        }
    }
}
