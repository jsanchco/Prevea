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

    #endregion

    public partial class Service
    {
        public const string _CREATOR = "Servicio de Report (PREVEA)";
        public static readonly Font _STANDARFONT_10 = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        public static readonly Font _STANDARFONT_8 = FontFactory.GetFont(FontFactory.HELVETICA, 8);
        public static readonly Font _STANDARFONT_10_BOLD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        public static readonly Font _STANDARFONT_10_BOLD_CUSTOMCOLOR = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, new BaseColor(144, 54, 25));
        public static readonly Font _STANDARFONT_14_BOLD = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
        public static readonly Font _STANDARFONT_14_BOLD_WHITE = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);

        public Result GenerateOfferSPAReport(Model.Model.Document documentSPA, string route)
        {
            try
            {
                var pdf = new Document(PageSize.LETTER); 
                var fileName = $"{route}\\App_Data\\PDF\\{documentSPA.Name}{documentSPA.Extension}";
                var pdfWriter = PdfWriter.GetInstance(pdf, new FileStream(fileName, FileMode.Create));
                var pageEventHelper = new PageEventHelper();
                pdfWriter.PageEvent = pageEventHelper;

                pdf.AddTitle(documentSPA.Name);
                pdf.AddCreator(_CREATOR);
                pdf.Open();
  
                pdf.Add(GetHeader(route));
                pdf.Add(new Paragraph(" ", _STANDARFONT_14_BOLD));
                pdf.Add(new Paragraph("OFERTA DE SERVICIO", _STANDARFONT_14_BOLD));
                pdf.Add(GetTableN_Document(documentSPA));
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
                pdf.Add(GetTableTitleTransparent("SEGURIDAD EN EL TRABAJO, HIGIENE INDUSTRIAL, ERGONOMÍA Y PSICOSOCIOLOGÍA APLICADA"));
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
            var pdfPTable = new PdfPTable(2) { WidthPercentage = 100 };

            var image = Image.GetInstance($"{route}\\Images\\Logo_report.png");
            image.ScalePercent(50f);
            var pdfCellImage = new PdfPCell(image)
            {
                Rowspan = 4,
                BorderWidth = 0
            };
            pdfPTable.AddCell(pdfCellImage);

            var pdfCell = new PdfPCell(new Phrase("PREVEA SPA", _STANDARFONT_14_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("C/ Abad Juan Catalán, 38 28032-Madrid", _STANDARFONT_14_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Tfno.- 917602713", _STANDARFONT_14_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("prevea@preveaspa.com", _STANDARFONT_14_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableN_Document(Model.Model.Document document)
        {
            var pdfPTable = new PdfPTable(2) { WidthPercentage = 40, HorizontalAlignment = 2 };

            var pdfCell = new PdfPCell(new Phrase("Nº OFERTA", _STANDARFONT_10_BOLD)) { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthBottom = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Name, _STANDARFONT_10_BOLD)) { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthBottom = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("FECHA", _STANDARFONT_10_BOLD)) { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthRight = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Date.ToShortDateString(), _STANDARFONT_10_BOLD)) { BackgroundColor = new BaseColor(204, 204, 255), BorderWidthLeft = 0, BorderWidthTop = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private Chunk GetLineSeparator()
        {
            return new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_LEFT, 1));
        }

        private PdfPTable GetTableTitle(string title)
        {
            var pdfPTable = new PdfPTable(1) { WidthPercentage = 100 };
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
            var pdfPTable = new PdfPTable(1) { WidthPercentage = 100 };
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
            var pdfPTable = new PdfPTable(2) { WidthPercentage = 100 };
            var widths = new[] { 30f, 70f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("R.SOCIAL/AUTÓNOMO", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.Name, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("CIF/NIF", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.NIF, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("DOMICILIO SOCIAL", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.Address, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("ACTIVIDAD", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.Cnae.Name, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("CONTACTO", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            var contactPerson = document.Company.ContactPersons.FirstOrDefault(x =>
                x.ContactPersonTypeId == (int)Model.Model.EnContactPersonType.ContactPerson);
            pdfCell = contactPerson != null ? 
                new PdfPCell(new Phrase($"{contactPerson.User.FirstName} {contactPerson.User.LastName}", _STANDARFONT_10)) { BorderWidth = 0 } : 
                new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("CARGO", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = contactPerson != null ?
                new PdfPCell(new Phrase($"{contactPerson.User.WorkStationCustom}", _STANDARFONT_10)) { BorderWidth = 0 } :
                new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("TELÉFONO", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = contactPerson != null ?
                new PdfPCell(new Phrase($"{contactPerson.User.PhoneNumber}", _STANDARFONT_10)) { BorderWidth = 0 } :
                new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("E-MAIL", _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = contactPerson != null ?
                new PdfPCell(new Phrase($"{contactPerson.User.Email}", _STANDARFONT_10)) { BorderWidth = 0 } :
                new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private PdfPTable GetTableEspecialidadesObjeto(Model.Model.Document document)
        {
            var tecniches = "No";
            var health = "No";

            if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService != null)
            {
                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques != null)
                {
                    tecniches = "Si";
                }
                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountHealthVigilance != null &&
                    document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountMedicalExamination != null)
                {
                    health = "Si";
                }
            }

            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 40f, 10f, 40f, 10f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("SEGURIDAD EN EL TRABAJO", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(tecniches, _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("HIGIENE INDUSTRIAL", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(tecniches, _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("ERGONOMÍA Y PSICOSOCIOLOGÍA", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(tecniches, _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("MEDICINA DEL TRABAJO", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(health, _STANDARFONT_10_BOLD)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);

            return pdfPTable;
        }

        private Phrase GetCentrosTrabajo(Model.Model.Document document)
        {
            if (document.CompanyId == null)
                return new Phrase(" ", _STANDARFONT_10_BOLD);

            var workCenters = GetWorkCentersByCompany((int)document.CompanyId).Where(x => x.WorkCenterStateId == (int)Model.Model.EnWorkCenterState.Alta).ToList();
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
                    provincesWorkCenters = distinctWorkCenters[0].Count == 1 ?
                        $"{distinctWorkCenters[0].Field.Trim()}." :
                        $"{distinctWorkCenters[0].Field.Trim()}({distinctWorkCenters[0].Count}).";
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
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}." :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}).";
                            }
                            else
                            {
                                provincesWorkCenters += distinctWorkCenters[i].Count == 1 ?
                                    $"{newWorkCenter}, " :
                                    $"{newWorkCenter}({distinctWorkCenters[i].Count}), ";
                            }
                        }
                    }
                }
            }

            var workCentersText = numberWorkCenters == 0 ?
                "No tiene declarados Centros de Trabajo" :
                "Centros de trabajo permanentes y móviles con domicilio en ";

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

            var paragraph = new Paragraph("Aceptada la oferta y firmado el contrato, se realizará visita previa a centro/s comunicado/s por la empresa, a los efectos de verificación de la toma de datos que posteriormente servirá para la elaboración de la evaluación de riesgos y planificación preventiva, así como el resto de información que componen el plan de prevención", _STANDARFONT_10);
            pdf.Add(paragraph);
        }

        private PdfPTable GetFormaPago(Model.Model.Document document)
        {
            var pdfPTable = new PdfPTable(4) { WidthPercentage = 100 };
            var widths = new[] { 20f, 30f, 20f, 30f };
            pdfPTable.SetWidths(widths);

            var pdfCell = new PdfPCell(new Phrase("MÉTDODO PAGO", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.PaymentMethod.ModePayment.Description, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("COBRO R. MÉDICOS", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.PaymentMethod.ModePaymentMedicalExamination.Description, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase("Nº CUENTA", _STANDARFONT_10_BOLD_CUSTOMCOLOR)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(document.Company.PaymentMethod.AccountNumber, _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
            pdfPTable.AddCell(pdfCell);
            pdfCell = new PdfPCell(new Phrase(" ", _STANDARFONT_10)) { BorderWidth = 0 };
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
            phrase = new Phrase(", colaborando en el seguimiento del mismo: análisis de las necesidades relativas a la estructura organizativa de prevención de acuerdo con la legislación vigente y las características de ésta, con objeto de definir un sistema organizativo y los procedimientos que permitan gestionar la prevención de los riesgos laborales de una forma integral, eficaz, efectiva y fiable, mediante la programación de actividades.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Realización, revisión y/o actualización ", _STANDARFONT_10)
            {
                new Chunk("Evaluación de Riesgos ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("Laborales por puestos de trabajo, gestionando ésta mediante una aplicación informática propia: revisión del informe técnico de la Evaluación de Riesgos, evaluando los nuevos riesgos, en caso de existir, y reevaluando aquellos riesgos identificados con anterioridad y para los que ya se planificaron medidas preventivas. La Revisión de la Evaluación de los Riesgos no incluirá, a no ser que se especifiquen en la presente renovación, la realización de evaluaciones especificas determinados riesgos, tales como evaluaciones higiénicas, evaluaciones ergonómicas y psicosociales, análisis de adecuación de equipos de trabajo y análisis de instalaciones sometidas a reglamentación específica. Estas actuaciones serán propuestas y/o incluidas en la Planificación de la Actividad Preventiva y deberán ser contratadas adicionalmente.", _STANDARFONT_10);
            pdf.Add(phrase);

            phrase = new Phrase("·  Diseño y elaboración de la ", _STANDARFONT_10)
            {
                new Chunk("Planificación de la Actividad Preventiva", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(", gestionando ésta mediante una aplicación informática propia, y colaborando en el seguimiento de la implantación de las medidas correctoras y acciones propuestas: especificación en el informe técnico de las actividades preventivas a desarrollar, relacionadas con los riesgos identificados y valorados; incluyendo su priorización, propuesta de designación de responsables de su ejecución y recursos económicos.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Análisis de las posibles situaciones de emergencia y las medidas necesarias a adoptar en materia de primeros auxilios, lucha contra incendios y evacuación de los trabajadores.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Verificación de la Eficacia de las Medidas Preventivas: verificación, en función de la entidad de los riesgos y del tamaño de la empresa, del grado de implantación de las medidas programadas, mediante comprobación de las condiciones de seguridad del centro de trabajo y del seguimiento y control del sistema de gestión de prevención de riesgos laborales.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Se excluyen en estos precios los gastos correspondientes al laboratorio por analíticas de contaminantes en higiene industrial y estudios específicos de ergonomia y psicosociología aplicada.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Información a los trabajadores sobre los riesgos para la seguridad y salud en el trabajo, sobre las medidas y actividades de protección y prevención y sobre las medidas de emergencia. Queda incluido lo concerniente a la formación inicial de los trabajadores.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Asesoramiento y apoyo en la elaboración de normas de actuación en caso de emergencia.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Asesoramiento y apoyo para definir las necesidades formativas de los trabajadores, de manera que se puedan incluir en un plan de formación general que elabore la Empresa Contratante, de acuerdo a la evaluación de riesgos.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Asesoramiento en el control y gestión de seguridad en el sector de actividad que corresponda, así como en los aspectos relativos a la coordinación de actividades empresariales.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Elaboración de la ", _STANDARFONT_10)
            {
                new Chunk("Memoria Anual ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("a la finalización de la actividad, como resumen de las actividades de Prevención Técnica desarrolladas por PREVEA. Dado su carácter recapitulativo, se entregará transcurrido el período anual de referencia.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.NewPage();

            phrase = new Phrase("·  Diseño del sistema necesario para el registro y mantenimiento de la documentación que puede ser solicitada por cualquier administración, en materia de seguridad y salud, según las obligaciones establecidas en la legislación vigente.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Asesoramiento y apoyo para el cumplimiento de la obligación de realizar la investigación de accidentes e incidentes, siempre que sea requerido por la Empresa Contratante.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Evaluación de riesgos de las instalaciones fijas estipuladas en el presente contrato-concierto, incluyendo estudio ergonómico, de higiene industrial y psicología aplicada de dichas instalaciones.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Asesoría y asistencia jurídica relacionada con las especialidades preventivas contratadas.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Asesoramiento ante posibles inspecciones que se pudieran presentar a la Empresa Contratante relacionadas con las especialidades contratadas.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  Asesoramiento para la integración de la prevención de riesgos laborales en el conjunto de actividades y decisiones de la Empresa Contratante, así como para la elaboración de un Sistema de Gestión de prevención de riesgos laborales.", _STANDARFONT_10);
            pdf.Add(phrase);
        }

        private void GetDescriptionVigilanciaSalud(Document pdf)
        {
            var phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Planificación de Vigilancia de la Salud: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("planificación de las acciones necesarias para un eficaz control de los riesgos laborales sobre la salud de los trabajadores y establecimiento de las prioridades de las actividades sanitarias a realizar en función de la magnitud de los riesgos, del número de trabajadores expuestos y de la posible patología laboral asociada a los mismos.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Estudio descriptivo de los resultados obtenidos de los Exámenes de Salud Específicos", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase(" con la finalidad de identificar problemas de salud abordables o modificables mediante intervenciones colectivas. Se elaborará un informe cuando se hayan realizado un mínimo de 10 Exámenes de Salud Específicos. Dado su carácter recapitulativo, podrá entregarse transcurrido el período anual de referencia.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Memoria Anual de Vigilancia de la Salud: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("resumen de las actividades preventivas desarrolladas por PREVEA en la empresa. Dado su carácter recapitulativo, podrá entregarse transcurrido el período anual de referencia.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("·  ", _STANDARFONT_10)
            {
                new Chunk("Exámenes de Salud Específicos: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("práctica de exámenes de salud relacionados con los riesgos laborales que darán lugar a la elaboración de un informe médico personalizado que se entregará al trabajador, y a la valoración de la aptitud del trabajador para su puesto de trabajo, indicando, en su caso, la necesidad de adoptar medidas preventivas complementarias. Los exámenes de Salud están compuestos de: ", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("    o ", _STANDARFONT_10)
            {
                new Chunk("Analítica: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("Análisis de una muestra biológica de un trabajador (Sangre y Orina), mediante un proceso técnico, obteniéndose un resultado que facilita la valoración de su estado de salud. ", _STANDARFONT_10);
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
            phrase = new Phrase("Determinación de la Agudeza Visual tanto cercana como lejana y un test de cromatografía.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Audiometría: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("Valoración de la agudeza auditiva de los trabajadores realizada en una cabina audiométrica analizando las siguientes frecuencias en ambos oídos: 250 Hz, 500 Hz, 1000 Hz, 2000 Hz, 3000Hz, 4000 Hz, 6000 Hz y 8000 Hz.", _STANDARFONT_10);
            pdf.Add(phrase);

            pdf.Add(new Chunk("\n"));

            phrase = new Phrase("        • ", _STANDARFONT_10)
            {
                new Chunk("Espirometría: ", _STANDARFONT_10_BOLD)
            };
            pdf.Add(phrase);
            phrase = new Phrase("Determinación de la Capacidad Vital Forzada de los trabajadores, las curvas de Flujo-Volumen y de Volumen-Tiempo.", _STANDARFONT_10);
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

            phrase = new Phrase("Oro-faringe, Tórax, Auscultación Pulmonar y Cardiaca, Abdomen, Sistema Músculo Esquelético, Sistema Venoso Periférico, Piel y Lesiones Dérmicas, Sistema Nervioso, Otoscopia.", _STANDARFONT_10);
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
                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques != null)
                {
                    totalAmountTecniques = (decimal)document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques * document.Company.SimulationCompanyActive.Simulation.NumberEmployees;
                }
                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountHealthVigilance != null)
                {
                    totalAmountHealthVigilance = (decimal)document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountHealthVigilance * document.Company.SimulationCompanyActive.Simulation.NumberEmployees;
                }
                if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountMedicalExamination != null)
                {
                    totalAmountMedicalExamination = (decimal)document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountMedicalExamination * document.Company.SimulationCompanyActive.Simulation.NumberEmployees;
                }
            }

            if (document.Company.PaymentMethod.ModePaymentMedicalExaminationId == (int)EnModePaymentMedicalExamination.ALaFirmaDelContrato)
            {
                total = totalAmountTecniques * Convert.ToDecimal(IVA) + totalAmountHealthVigilance * Convert.ToDecimal(IVA) + totalAmountMedicalExamination;
            }
            else
            {
                total = totalAmountTecniques * Convert.ToDecimal(IVA) + totalAmountHealthVigilance * Convert.ToDecimal(IVA);
            }
            total = Math.Round(total, 2);

            var amountTecniques = 0.0m;
            if (document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService != null &&
                document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques != null)
            {
                amountTecniques = (decimal)document.Company.SimulationCompanyActive.Simulation.ForeignPreventionService.AmountTecniques;
            }

            var phrase = new Phrase("Como contraprestación económica por las citadas actividades, la Empresa Contratante abonará a PREVEA las siguientes cantidades:", _STANDARFONT_10);
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
