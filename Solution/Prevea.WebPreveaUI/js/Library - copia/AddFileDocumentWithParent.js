var AddFileDocumentWithParent = kendo.observable({
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    filesId: "files",
    formSaveFileDocumentWithParentId: "formSaveFileDocumentWithParent",
    errorFromFrontId: "errorFromFront",
    informationId: "information",
    textAreaObservationsId: "textAreaObservations",
    routeFileDelete: "",

    parentId: null,
    libraryAreaId: null,

    init: function (parentId, libraryAreaId) {
        if (parentId) {
            this.parentId = parentId;
        }
        
        this.libraryAreaId = libraryAreaId;

        this.routeFileDelete = "";

        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function() {
        $($("#" + this.btnCancelId)).on("click", function () {
            AddFileDocumentWithParent.goToDetailDocument();
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = AddFileDocumentWithParent.validateForm();
            AddFileDocumentWithParent.showErrors(errors);
            if (errors.length > 0) {
                e.preventDefault();
            }
        });
    },

    validateForm: function() {
        var error = [];

        if (!$("#" + this.textAreaObservationsId).val()) {
            error.push("Debes añadir una Observación");
        }
 
        var fileSelected = this.getFileSelected();
        if (fileSelected == null) {
            error.push("Debes añadir un Documento");
        }

        return error;
    },

    onUpload: function(e) {
        e.data = {
            libraryAreaId: AddFileDocumentWithParent.libraryAreaId,
            parentId: AddFileDocumentWithParent.parentId,
            routeFileDelete: AddFileDocumentWithParent.routeFileDelete
        };
    },

    onSuccess: function (e) {
        if (e.response.status === "Ok") {
            switch (e.response.from) {
                case "SaveFile":
                    AddFileDocumentWithParent.routeFileDelete = e.response.libraryViewModel.Url;

                    $("#LibraryAreaId").val(e.response.libraryViewModel.LibraryAreaId);
                    $("#Extension").val(e.response.libraryViewModel.Extension);
                    $("#Icon").val(e.response.libraryViewModel.Icon);
                    $("#Edition").val(e.response.libraryViewModel.Edition);
                    $("#DocumentNumber").val(e.response.libraryViewModel.DocumentNumber);
                    $("#Name").val(e.response.libraryViewModel.Name);
                    $("#FileName").val(e.response.libraryViewModel.FileName);
                    $("#Url").val(e.response.libraryViewModel.Url);

                    AddFileDocumentWithParent.showErrors(null);

                    break;

                case "RemoveFile":
                    AddFileDocumentWithParent.routeFileDelete = "";

                    AddFileDocumentWithParent.showErrors(null);

                    break;

                default:
                    break;
            }
 
        }
        if (e.response.status === "Error") {
            AddFileDocumentWithParent.showErrors(new Array(e.response.message));
        }
    },

    onRemove: function(e) {
        e.data = {
            routeFileDelete: AddFileDocumentWithParent.routeFileDelete
        };
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

    showErrors: function(errors) {
        var divError = $("#" + AddFileDocumentWithParent.errorFromFrontId);

        if (errors != null && errors.length > 0) {
            $("#" + AddFileDocumentWithParent.informationId).hide();

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

    goToAddDocumentWithParent: function (e) {
        var params = {
            url: "/Library/AddFileDocumentWithParent",
            data: {
                id: this.parentId
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDetailDocument: function (e) {
        var params = {
            url: "/Library/DetailDocument",
            data: {
                id: this.parentId
            }
        };
        GeneralData.goToActionController(params);
    }

});