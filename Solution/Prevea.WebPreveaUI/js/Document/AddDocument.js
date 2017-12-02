var AddDocument = kendo.observable({
    comboBoxAreasId: "AreaId",
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    filesId: "files",
    formSaveDocumentId: "formSaveDocument",
    errorFromFrontId: "errorFromFront",
    informationId: "information",
    textAreaDescriptionId: "textAreaDescription",

    init: function () {
        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function () {
        $($("#" + this.btnCancelId)).on("click", function () {
            AddDocument.goToLibrary();
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = AddDocument.validateForm();
            AddDocument.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });
    },

    validateForm: function () {
        var error = [];

        if (!$("#" + this.textAreaDescriptionId).val()) {
            error.push("Debes añadir una Descripción");
        }
        var comboBox = $("#" + this.comboBoxAreasId).data("kendoDropDownList");
        if (!comboBox.value()) {
            error.push("Debes añadir un Area");
        }
        var fileSelected = this.getFileSelected();
        if (fileSelected == null) {
            error.push("Debes añadir un Documento");
        }

        return error;
    },

    onUpload: function () {
    },

    onSuccess: function (e) {
        if (e.response.status === "Ok") {
            AddDocument.showErrors(null);
        }

        if (e.response.status === "Error") {
            AddDocument.showErrors(new Array(e.response.message));
        }
    },

    onRemove: function () {
    },

    getFileSelected: function () {
        var upload = $("#" + this.filesId).kendoUpload().data("kendoUpload");
        var files = upload.getFiles();
        if (files.length === 0) {
            return null;
        }
        else {
            return files[0];
        }
    },

    showErrors: function (errors) {
        var divError = $("#" + AddDocument.errorFromFrontId);

        if (errors != null && errors.length > 0) {
            $("#" + AddDocument.informationId).hide();

            var html = "<button type='button' class='close' data-dismiss='alert'>&times;</button>";
            html += "<ul>";
            $.each(errors,
                function (index, value) {
                    html += kendo.format("<li>{0}</li>", value);
                });
            html += "</ul>";

            divError.html(html);
            divError.show();
        } else {
            divError.hide();
        }
    },

    goToLibrary: function () {
        var params = {
            url: "/Document/Documents"
        };
        GeneralData.goToActionController(params);
    }

});