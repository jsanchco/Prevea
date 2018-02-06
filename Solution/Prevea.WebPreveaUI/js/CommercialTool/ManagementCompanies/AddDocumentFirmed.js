var AddDocumentFirmed = kendo.observable({

    addDocumentFirmedId: "addDocumentFirmed",

    contractualDocumentId: null,
    fileDocumentFirmedId: "fileDocumentFirmed",

    init: function (contractualDocumentId) {
        this.contractualDocumentId = contractualDocumentId;

        this.setUpWidgets();
    },

    setUpWidgets: function () {
        $("#" + this.fileDocumentFirmedId).kendoUpload({
            async: {
                saveUrl: "/Companies/SaveDocumentFirmed",
                autoUpload: false
            },
            multiple: false,
            localization: {
                select: "Selecciona documento ...",
                clearSelectedFiles: "Eliminar",
                uploadSelectedFiles: "Guardar"
            },
            upload:function(e) {
                e.data = { contractualDocumentId: AddDocumentFirmed.contractualDocumentId };
            },
            success: AddDocumentFirmed.onSuccessSaveDocument,
            error: AddDocumentFirmed.onErrorSaveDocument
        });
    },

    onSuccessSaveDocument: function (e) {
        if (e.response.Status === Constants.resultStatus.Ok) {
            GeneralData.showNotification(Constants.ok, "", "success");

            var addDocumentFirmedWindow = $("#" + AddDocumentFirmed.addDocumentFirmedId);
            addDocumentFirmedWindow.data("kendoWindow").close();

            ContractualsDocumentsCompany.updateRow(e.response.Object);
        } else {
            GeneralData.showNotification(Constants.ko, "", "error");
        }
    },

    onErrorSaveDocument: function () {
        GeneralData.showNotification(Constants.ko, "", "error");
    }
});