var AddDocumentWithParent = kendo.observable({
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    filesId: "files",
    formSaveFileDocumentWithParentId: "formSaveDocumentWithParent",
    errorFromFrontId: "errorFromFront",
    informationId: "information",
    textAreaObservationsId: "textAreaObservations",
    routeFileDelete: "",

    parentId: null,
    areaId: null,

    init: function (parentId, areaId) {
        if (parentId) {
            this.parentId = parentId;
        }
        
        this.areaId = areaId;

        this.routeFileDelete = "";

        this.setKendoUIWidgets();
    },

    setKendoUIWidgets: function() {
        $($("#" + this.btnCancelId)).on("click", function () {
            AddDocumentWithParent.goToDetailDocument();
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var errors = AddDocumentWithParent.validateForm();
            AddDocumentWithParent.showErrors(errors);
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
 
    },

    onSuccess: function (e) {
 
    },

    onRemove: function(e) {
  
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
        var divError = $("#" + AddDocumentWithParent.errorFromFrontId);

        if (errors != null && errors.length > 0) {
            $("#" + AddDocumentWithParent.informationId).hide();

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
            url: "/Document/AddDocumentWithParent",
            data: {
                id: this.parentId
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDetailDocument: function (e) {
        var params = {
            url: "/Document/DetailDocument",
            data: {
                id: this.parentId
            }
        };
        GeneralData.goToActionController(params);
    }

});