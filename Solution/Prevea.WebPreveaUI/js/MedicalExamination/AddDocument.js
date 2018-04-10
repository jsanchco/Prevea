var AddDocument = kendo.observable({

    addDocumentId: "addDocument",

    medicalExaminationDocumentId: null,
    fileDocumentId: "fileDocument",

    init: function (medicalExaminationDocumentId) {
        this.medicalExaminationDocumentId = medicalExaminationDocumentId;

        this.setUpWidgets();
    },

    setUpWidgets: function () {
        $("#" + this.fileDocumentId).kendoUpload({
            async: {
                saveUrl: "/MedicalExamination/SaveMedicalExaminationDocument",
                autoUpload: false
            },
            multiple: false,
            localization: {
                select: "Selecciona documento ...",
                clearSelectedFiles: "Eliminar",
                uploadSelectedFiles: "Guardar"
            },
            upload: function (e) {
                e.data = { medicalExaminationDocumentId: AddDocument.medicalExaminationDocumentId };
            },
            success: AddDocument.onSuccessSaveDocument,
            error: AddDocument.onErrorSaveDocument
        });
    },

    onSuccessSaveDocument: function (e) {
        if (e.response.Status === Constants.resultStatus.Ok) {
            GeneralData.showNotification(Constants.ok, "", "success");

            var addDocumentWindow = $("#" + AddDocument.addDocumentId);
            addDocumentWindow.data("kendoWindow").close();

            DocumentsMedicalExamination.updateRow(e.response.Object);
        } else {
            GeneralData.showNotification(Constants.ko, "", "error");
        }
    },

    onErrorSaveDocument: function () {
        GeneralData.showNotification(Constants.ko, "", "error");
    }
});