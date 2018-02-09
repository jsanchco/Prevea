var AddAnnex = kendo.observable({

    addAnnexId: "addAnnex",

    contractualDocumentId: null,
    fileAnnexId: "fileAnnex",

    init: function (contractualDocumentId) {
        this.contractualDocumentId = contractualDocumentId;

        this.setUpWidgets();
    },

    setUpWidgets: function () {
        $("#" + this.fileAnnexId).kendoUpload({
            async: {
                saveUrl: "/Companies/SaveAnnex",
                autoUpload: false
            },
            multiple: false,
            localization: {
                select: "Selecciona documento ...",
                clearSelectedFiles: "Eliminar",
                uploadSelectedFiles: "Guardar"
            },
            upload: function (e) {
                e.data = { contractualDocumentId: AddAnnex.contractualDocumentId };
            },
            success: AddAnnex.onSuccessSaveDocument,
            error: AddAnnex.onErrorSaveDocument
        });
    },

    onSuccessSaveDocument: function (e) {
        if (e.response.Status === Constants.resultStatus.Ok) {
            GeneralData.showNotification(Constants.ok, "", "success");

            var addAnnexWindow = $("#" + AddAnnex.addAnnexId);
            addAnnexWindow.data("kendoWindow").close();

            ContractualsDocumentsCompany.updateRowAnnex(e.response.Object);
        } else {
            GeneralData.showNotification(Constants.ko, "", "error");
        }
    },

    onErrorSaveDocument: function () {
        GeneralData.showNotification(Constants.ko, "", "error");
    }
});