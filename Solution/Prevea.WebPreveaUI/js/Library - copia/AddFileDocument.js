var AddFileDocument = kendo.observable({
    comboBoxLibraryAreasId: "LibraryAreaId",
    comboBoxUsersId: "UserId",
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    filesId: "files",
    formSaveFileDocumentId: "formSaveFileDocument",
    errorFromFrontId: "errorFromFront",
    informationId: "information",
    textAreaDescriptionId: "textAreaDescription",
    routeFileDelete: "",

    init: function () {
        this.routeFileDelete = "";

        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function() {
        $($("#" + this.btnCancelId)).on("click", function () {
            AddFileDocument.goToLibrary();
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = AddFileDocument.validateForm();
            AddFileDocument.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });
    },

    validateForm: function() {
        var error = [];

        if (!$("#" + this.textAreaDescriptionId).val()) {
            error.push("Debes añadir una Descripción");
        }
        var comboBox = $("#" + this.comboBoxLibraryAreasId).data("kendoDropDownList");
        if (!comboBox.value()) {
            error.push("Debes añadir un Area");
        }
        comboBox = $("#" + this.comboBoxUsersId).data("kendoDropDownList");
        if (!comboBox.value()) {
            error.push("Debes añadir un Control");
        }
        var fileSelected = this.getFileSelected();
        if (fileSelected == null) {
            error.push("Debes añadir un Documento");
        }

        return error;
    },

    onUpload: function(e) {
        e.data = {
            libraryAreaId: $("#" + AddFileDocument.comboBoxLibraryAreasId).data("kendoDropDownList").value(),
            parentId: null,
            routeFileDelete: AddFileDocument.routeFileDelete
        };
    },

    onSuccess: function (e) {
        if (e.response.status === "Ok") {
            switch (e.response.from) {
                case "SaveFile":
                    AddFileDocument.routeFileDelete = e.response.libraryViewModel.Url;

                    $("#LibraryAreaId").val(e.response.libraryViewModel.LibraryAreaId);
                    $("#Extension").val(e.response.libraryViewModel.Extension);
                    $("#Icon").val(e.response.libraryViewModel.Icon);
                    $("#Edition").val(e.response.libraryViewModel.Edition);
                    $("#DocumentNumber").val(e.response.libraryViewModel.DocumentNumber);
                    $("#Name").val(e.response.libraryViewModel.Name);
                    $("#FileName").val(e.response.libraryViewModel.FileName);
                    $("#Url").val(e.response.libraryViewModel.Url);

                    AddFileDocument.showErrors(null);

                    break;

                case "RemoveFile":
                    AddFileDocument.routeFileDelete = "";

                    AddFileDocument.showErrors(null);

                    break;

                default:
                    break;
            }
 
        }
        if (e.response.status === "Error") {
            AddFileDocument.showErrors(new Array(e.response.message));
        }
    },

    onRemove: function(e) {
        e.data = {
            routeFileDelete: AddFileDocument.routeFileDelete
        };
    },

    onChangeComboBoxLibraryArea: function(e) {
        if (!$("#" + AddFileDocument.comboBoxLibraryAreasId).data("kendoDropDownList").value()) {
            $("#" + AddFileDocument.filesId).kendoUpload().data("kendoUpload").disable();
        } else {
            $("#" + AddFileDocument.filesId).kendoUpload().data("kendoUpload").enable();
        }
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

    updateStateUpload: function() {
        var active = true;
        if ($("#" + AddFileDocument.comboBoxLibraryAreasId).data("kendoDropDownList").value() === 0) {
            active = false;
        }
        if ($("#" + AddFileDocument.comboBoxUsersId).data("kendoDropDownList").value() === 0) {
            active = false;
        }
        if (!$("#" + AddFileDocument.textAreaDescriptionId).val()) {
            active = false;
        }

        if (active === true) {
            $("#" + AddFileDocument.filesId).kendoUpload().data("kendoUpload").enable();
        } else {
            $("#" + AddFileDocument.filesId).kendoUpload().data("kendoUpload").disable();
        }
    },

    showErrors: function(errors) {
        var divError = $("#" + AddFileDocument.errorFromFrontId);

        if (errors != null && errors.length > 0) {
            $("#" + AddFileDocument.informationId).hide();

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

    goToLibrary: function (e) {
        var params = {
            url: "/Library/Index"
        };
        GeneralData.goToActionController(params);
    }

});