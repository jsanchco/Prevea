var EditDocument = kendo.observable({
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    iconViewFileId: "iconViewFile",
    textAreaDescriptionId: "textAreaDescription",
    textAreaObservationsId: "textAreaObservations",
    rowFilesId: "rowFiles",
    spanNotificationId: "spanNotification",

    // Fields
    id: null,
    icon: null,
    notification: null,

    init: function (icon, notification) {
        $("#UpdateFile").val(false);

        this.id = $("#Id").val();

        if (icon) {
            this.icon = icon;
        } else {
            this.icon = null;
        }

        if (notification) {
            this.notification = notification;
        } else {
            this.notification = null;
        }
           
        this.createKendoWidgets();
        this.createIconViewFile();
    },

    createKendoWidgets: function () {
        $($("#" + this.btnCancelId)).on("click", function () {
            EditDocument.goToLibrary();
        });

        if (this.notification) {
            //$("#" + this.spanNotificationId).kendoNotification().data("kendoNotification").show(this.notification);
            GeneralData.showNotification(Constants.ok, "", "success");
        } else {
            $("#" + this.spanNotificationId).hide();
        }        
    },

    createIconViewFile: function () {
        if (this.icon !== "unknown_opt.png") {
            var html = kendo.format("<div align='center'><a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenFile(\"{0}\")' target='_blank' style='cursor: pointer;'>", this.id);
            html += kendo.format("<img src='../../Images/{0}' width='25px'></a></div>", this.icon);

            $("#" + this.iconViewFileId).html(html);
        }
    },

    goToEditDocument: function () {
        var params = {
            url: "/Document/EditDocument",
            data: {
                    id: this.id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToLibrary: function () {
        var params = {
            url: "/Document/Documents"
        };
        GeneralData.goToActionController(params);
    },

    onUpload: function () {
 
    },

    onSuccess: function (e) {
        if (e.response.status === "Ok") {
            $("#UpdateFile").val(true);
        }
    },

    onRemove: function (e) {
        e.data = {
        };
    }

});