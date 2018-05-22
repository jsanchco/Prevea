var DetailTemplatePreventivePlan = kendo.observable({

    id: null,
    template: null,

    editorTemplateDetailPreventivePlanId: "editorTemplateDetailPreventivePlan",
    btnSaveId: "btnSave",

    init: function (id) {
        this.id = id;
        
        $.ajax({
            url: "/Tecniques/GetTemplate",
            data: JSON.stringify({ "templateId": DetailTemplatePreventivePlan.id }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            beforeSend: function () {
                kendo.ui.progress($("#pageDetailTemplatePreventivePlan"), true);
            },
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    if (response.template == null) {
                        DetailTemplatePreventivePlan.template = "";
                    } else {
                        DetailTemplatePreventivePlan.template = response.template;
                    }                    
                    DetailTemplatePreventivePlan.createKendoWidgets();

                    kendo.ui.progress($("#pageDetailTemplatePreventivePlan"), false);
                } else {
                    GeneralData.showNotification(error, "", "error");

                    kendo.ui.progress($("#pageDetailTemplatePreventivePlan"), false);
                }
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");

                kendo.ui.progress($("#pageDetailTemplatePreventivePlan"), false);
            }
        });
    },

    createKendoWidgets: function () {
        $("#" + this.editorTemplateDetailPreventivePlanId).kendoEditor({
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

        var editor = $("#" + DetailTemplatePreventivePlan.editorTemplateDetailPreventivePlanId).data("kendoEditor");
        editor.value(DetailTemplatePreventivePlan.template);
    },

    goToTemplatePreventivePlans: function () {
        var params = {
            url: "/Tecniques/TemplatePreventivePlans",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailTemplatePreventivePlans: function () {
        var params = {
            url: "/Tecniques/DetailTemplatePreventivePlan",
            data: {
                id: this.id
            }
        };
        GeneralData.goToActionController(params);
    },

    saveTemplate: function () {
        var editor = $("#" + this.editorTemplateDetailPreventivePlanId).data("kendoEditor");
        var text = editor.value();

        $.ajax({
            url: "/Tecniques/SaveTemplate",
            data: JSON.stringify({ "templateId": DetailTemplatePreventivePlan.id, "text": text }),
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
    }
});