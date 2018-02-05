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
            select: function (e) {
                var fileInfo = e.files[0];

                var wrapper = this.wrapper;

                setTimeout(function () {
                    ChoosePhoto.addPreview(fileInfo, wrapper);
                });
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
        } else {
            GeneralData.showNotification(Constants.ko, "", "error");
        }
    },

    onErrorSaveDocument: function () {
        GeneralData.showNotification(Constants.ko, "", "error");
    }
});