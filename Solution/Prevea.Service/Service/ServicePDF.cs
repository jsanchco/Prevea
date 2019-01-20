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
    }

    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            var pageN = writer.PageNumber;
            var text = pageN.ToString();
            var len = Service._STANDARFONT_8.BaseFont.GetWidthPoint(text, Service._STANDARFONT_8.Size);
            var pageSize = document.PageSize;

            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(Service._STANDARFONT_8.BaseFont, Service._STANDARFONT_8.Size);
            //cb.SetTextMatrix(document.LeftMargin, pageSize.GetBottom(document.BottomMargin));
            var position = Convert.ToInt32(pageSize.Width / 2) - 4;
            cb.SetTextMatrix(position, pageSize.GetBottom(document.BottomMargin));
            cb.ShowText(text);

            cb.EndText();

            cb.AddTemplate(template, document.LeftMargin + len, pageSize.GetBottom(document.BottomMargin));
        }
    }
}
