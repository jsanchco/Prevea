var AddFileDocument = kendo.observable({
    comboBoxLibraryAreasId: "comboBoxLibraryAreas",
    comboUsersId: "comboBoxUsers",
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    filesId: "files",
    formSaveFileDocumentId: "formSaveFileDocument",
    errorFromFrontId: "errorFromFront",
    informationId: "information",
    texboxDescriptionId: "texboxDescription",

    init: function () {
        this.createKendoUIWidgets();
    },

    createComboBoxLibraryAreas: function () {
        $("#" + this.comboBoxLibraryAreasId).kendoComboBox({
            placeholder: "-- Selecciona Area --",
            dataTextField: "Text",
            dataValueField: "Value"
        });
    },

    createComboBoxUsers: function () {
        $("#" + this.comboUsersId).kendoComboBox({
            placeholder: "-- Selecciona Control --",
            dataTextField: "Text",
            dataValueField: "Value"
        });

        //$("#" + this.comboUsersId).kendoComboBox().data("kendoDropDownList").setDataSource(AddFileDocument.usersDataSource);
    },

    createUploadFileDocument: function () {
        $("#" + this.filesId).kendoUpload({
            name: "files",
            multiple: false,       
            localization: {
                "select": "Selecciona ..."
            }           
        });
    },

    createKendoUIWidgets: function() {
        this.createComboBoxLibraryAreas();
        this.createComboBoxUsers();
        this.createUploadFileDocument();

        //$("#" + this.errorFromFrontId).hide();

        $($("#" + this.btnCancelId)).on("click", function () {
           
        });

        $($("#" + this.btnValidateId)).on("click", function (e) {
            var divError = $("#" + AddFileDocument.errorFromFrontId);

            var errors = AddFileDocument.validateForm();
            if (errors.length > 0) {
                e.preventDefault();

                $("#" + AddFileDocument.informationId).hide();

                var html = "<button type='button' class='close' data-dismiss='alert'>&times;</button>";
                html += "<ul>";
                $.each(errors,
                    function(index, value) {
                        html += kendo.format("<li>{0}</li>", value);
                    });
                html += "</ul>";

                divError.html(html);
                divError.show();
            } else {
                divError.hide();

                e.preventDefault();
                $("#library").val(AddFileDocument.getModel());
                var f = $(this).closest("form");
                $.post(f.attr("action"), f.serialize(), function (res) {
                    $("#SaveFileDocument").append(res);
                });
            }
        });
    },

    validateForm: function() {
        var error = [];

        if (AddFileDocumentViewModel.description === "") {
            error.push("Debes añadir una Descripción");
        }
        if (AddFileDocumentViewModel.area.Value === 0) {
            error.push("Debes añadir un Area");
        }
        if (AddFileDocumentViewModel.user.Value === 0) {
            error.push("Debes añadir un Control");
        }
        if (this.getFileSelected() == null) {
            error.push("Debes añadir un Documento");
        }

        return error;
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

    getModel: function() {
        var model = {
            Description: AddFileDocumentViewModel.description,
            LibraryAreaId: $("#" + this.comboBoxLibraryAreasId).data("kendoComboBox").value(),
            UserId: $("#" + this.comboUsersId).data("kendoComboBox").value()
        }

        return model;
    },

    //goToSave: function () {
    //    $.ajax({
    //        url: '/Library/Save',
    //        type: 'POST',
    //        data: {
    //            library: {
    //                Area
    //            } 
    //        },
    //        dataType: 'json',
    //        contentType: false,
    //        processData: false,
    //        success: function ($data) {
    //            alert("OK!!!!");
    //        }
    //    });
    //}

});