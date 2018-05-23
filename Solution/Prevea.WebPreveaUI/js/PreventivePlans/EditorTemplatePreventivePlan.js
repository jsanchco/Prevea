var EditorTemplatePreventivePlan = kendo.observable({

    id: null,
    templateId: null,
    snippets: null,
    editorTemplateId: "editorTemplate",

    init: function (id, templateId, snippets) {
        this.id = id;
        this.templateId = templateId;
        this.snippets = snippets;   

        this.createKendoWidgets();
    },

    createKendoWidgets: function () {
        var items = [];
        $.each(this.snippets, function (key, value) {
            items.push({ text: key, value: value } );
        });
        var snippets = {
            name: "insertHtml",
            items: items
        };
        $("#" + this.editorTemplateId).kendoEditor({
            resizable: {
                content: false,
                toolbar: true
            },
            tools: [
                snippets,
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
    },

    saveTemplate: function() {
        var editor = $("#" + this.editorTemplateId).data("kendoEditor");
        var text = editor.value();

        $.ajax({
            url: "/PreventivePlan/SaveTemplatePreventivePlan",
            data: JSON.stringify({
                    "preventivePlanId": EditorTemplatePreventivePlan.id,
                    "templateId": EditorTemplatePreventivePlan.templateId,
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
    }
});