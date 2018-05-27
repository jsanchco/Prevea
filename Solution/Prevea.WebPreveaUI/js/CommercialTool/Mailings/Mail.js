var Mail = kendo.observable({
    mailingId: null,

    editorBodyMailId: "editorBodyMail",
    btnSaveBodyMailId: "btnSaveBodyMail",

    bodyMail: null,

    init: function (mailingId) {
        this.mailingId = mailingId;

        $.ajax({
            url: "/Mailings/GetBodyMail",
            data: JSON.stringify({ "mailingId": Mail.mailingId }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            beforeSend: function () {
                kendo.ui.progress($("#tabStripDetailMailing"), true);
            },
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    if (response.bodyMail == null) {
                        Mail.bodyMail = "";
                    } else {
                        Mail.bodyMail = response.bodyMail;
                    }
                    Mail.createKendoWidgets();

                    kendo.ui.progress($("#tabStripDetailMailing"), false);
                } else {
                    GeneralData.showNotification(error, "", "error");

                    kendo.ui.progress($("#tabStripDetailMailing"), false);
                }
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");

                kendo.ui.progress($("#tabStripDetailMailing"), false);
            }
        });

        this.updateButtons();
    },

    createKendoWidgets: function () {
        $("#" + Mail.editorBodyMailId).kendoEditor({
            resizable: {
                content: false,
                toolbar: true
            },
            tools: [
                "bold",
                "italic",
                "underline",
                "strikethrough",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "insertUnorderedList",
                "insertOrderedList",
                "indent",
                "outdent",
                "createLink",
                "unlink",
                "insertImage",
                "insertFile",
                "subscript",
                "superscript",
                "tableWizard",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                "formatting",
                "cleanFormatting",
                "fontName",
                "fontSize",
                "foreColor",
                "backColor",
                "print"
            ]
        });

        var editor = $("#" + Mail.editorBodyMailId).data("kendoEditor");
        editor.value(Mail.bodyMail);
    },

    saveBodyMail: function() {
        var editor = $("#" + this.editorBodyMailId).data("kendoEditor");
        var text = editor.value();

        $.ajax({
            url: "/Mailings/SaveBodyMail",
            data: JSON.stringify({
                "mailingId": Mail.mailingId,
                "text": text
            }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    GeneralData.showNotification(Constants.ok, "", "success");

                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");
            }
        });
    },

    updateButtons: function() {
        if (DetailMailing.mailingState === "true") {
            $("#" + this.btnSaveBodyMailId).removeAttr("disabled");
            $("#" + this.btnSaveBodyMailId).prop("disabled", true);
        }
    }
});