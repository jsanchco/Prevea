var AddOtherDocument = kendo.observable({

    addOtherDocumentId: "addOtherDocument",

    contractualDocumentId: null,
    fileOtherDocumentId: "fileOtherDocument",

    init: function (contractualDocumentId) {
        this.contractualDocumentId = contractualDocumentId;

        this.setUpWidgets();
    },

    setUpWidgets: function () {
        $("#" + this.fileOtherDocumentId).kendoUpload({
            async: {
                saveUrl: "/Companies/SaveOtherDocument",
                autoUpload: false
            },
            multiple: false,
            localization: {
                select: "Selecciona documento ...",
                clearSelectedFiles: "Eliminar",
                uploadSelectedFiles: "Guardar"
            },
            upload: function (e) {
                e.data = { contractualDocumentId: AddOtherDocument.contractualDocumentId };
            },
            success: AddOtherDocument.onSuccessSaveDocument,
            error: AddOtherDocument.onErrorSaveDocument
        });
    },

    onSuccessSaveDocument: function (e) {
        if (e.response.Status === Constants.resultStatus.Ok) {
            GeneralData.showNotification(Constants.ok, "", "success");

            var addOtherDocumentWindow = $("#" + AddOtherDocument.addOtherDocumentId);
            addOtherDocumentWindow.data("kendoWindow").close();

            ContractualsDocumentsCompany.updateRowOtherDocument(e.response.Object);
        } else {
            GeneralData.showNotification(Constants.ko, "", "error");
        }
    },

    onErrorSaveDocument: function () {
        GeneralData.showNotification(Constants.ko, "", "error");
    }
});