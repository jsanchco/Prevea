var TemplateEmployeeCitationReport = kendo.observable({
    divName: null,
    fileName: null,

    btnPrintId: "btnPrint",

    init: function (divName, fileName) {
        this.divName = divName;
        this.fileName = fileName;
    },

    printPage: function() {
        kendo.drawing.drawDOM($("#" + TemplateEmployeeCitationReport.divName))
            .then(function (group) {
                // Render the result as a PDF file
                return kendo.drawing.exportPDF(group,
                    {
                        paperSize: "auto",
                        margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                    });
            })
            .done(function (data) {
                // Save the PDF file
                kendo.saveAs({
                    dataURI: data,
                    fileName: TemplateEmployeeCitationReport.fileName,
                    proxyURL: "https://demos.telerik.com/kendo-ui/service/export"
                });
            });
    }
});