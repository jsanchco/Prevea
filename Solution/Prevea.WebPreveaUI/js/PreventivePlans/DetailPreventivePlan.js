var DetailPreventivePlan = kendo.observable({
    titlePageId: "titlePage",
    navigationPageId: "navigationPage",
    tabStripDetailPreventivePlanId: "tabStripDetailPreventivePlan",
    panelDescriptionDetailPreventivePlanId: "panelDescriptionDetailPreventivePlan",
    selectTemplateDetailPreventivePlanId: "selectTemplateDetailPreventivePlan",
    buttonsDetailPreventivePlanId: "buttonsDetailPreventivePlan",
    inputTemplatePreventivePlanId: "inputTemplatePreventivePlan",
    footerPartialViewId: "footerPartialView",
    btnAddTemplateId: "btnAddTemplate",
    btnSavePreventivePlanTemplateId: "btnSavePreventivePlanTemplate",
    btnDeletePreventivePlanTemplateId: "btnDeletePreventivePlanTemplate",
    btnZoomPreventivePlanTemplateId: "btnZoomPreventivePlanTemplate",
    confirmId: "confirm",
    tabStrip: null,    

    // Fields
    id: null,
    selectTabId: null,
    listTemplates: null,

    init: function (id, selectTabId, listTemplates) {
        this.id = id;
        this.selectTabId = selectTabId;
        this.listTemplates = listTemplates;       

        kendo.ui.progress($("#pagePreventivePlans"), false);

        this.tabStrip = $("#" + DetailPreventivePlan.tabStripDetailPreventivePlanId).kendoTabStrip().data("kendoTabStrip");

        if (listTemplates == null || listTemplates.length === 0) {
            $("#" + this.btnSavePreventivePlanTemplateId).hide();
            $("#" + this.btnDeletePreventivePlanTemplateId).hide();
            $("#" + this.btnZoomPreventivePlanTemplateId).hide();
        } else {

            $("#" + this.btnSavePreventivePlanTemplateId).show();
            $("#" + this.btnDeletePreventivePlanTemplateId).show();
            $("#" + this.btnZoomPreventivePlanTemplateId).show();

            this.addTemplateFromList();
        }

        $("#" + this.btnAddTemplateId).removeAttr("disabled");
        $("#" + this.btnAddTemplateId).prop("disabled", true);  
    },

    goToPreventivePlans: function () {
        var params = {
            url: "/PreventivePlan/PreventivePlans",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailPlanPreventive: function () {
        var params = {
            url: "/PreventivePlan/DetailPreventivePlan",
            data: {
                id: this.id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    addTemplateFromList: function () {
        for (var i = 0; i < this.listTemplates.length; i++) {
            $.ajax({
                url: "/PreventivePlan/GetDataFromPreventivePlanTemplatePreventivePlan",
                data: {
                    "preventivePlanId": DetailPreventivePlan.id,
                    "templateId": DetailPreventivePlan.listTemplates[i]
                },
                type: "post",
                dataType: "json",
                async: false,
                success: function (response) {
                    DetailPreventivePlan.tabStrip.append({
                        text: response.title,
                        content: kendo.format("<textarea id='{0}_editor' rows='10' cols='30' style='height: 43vh; width: 100%'></textarea>", DetailPreventivePlan.listTemplates[i])
                    });
                    DetailPreventivePlan.createEditor(DetailPreventivePlan.listTemplates[i], response.text);
                    DetailPreventivePlan.tabStrip.select(DetailPreventivePlan.tabStrip.items().length - 1);
                },
                error: function (xhr, status, error) {
                    GeneralData.showNotification(error, "", "error");
                }
            });       
        }
    },

    addTemplate: function () {
        var dropdownlist = $("#" + this.inputTemplatePreventivePlanId).data("kendoDropDownList");

        if (this.listTemplates.indexOf(parseInt(dropdownlist.value())) !== -1) {
            GeneralData.showNotification("Existe esta Plantilla dentro de la Gestión Preventiva de la Empresa", "", "error");
            return;
        }            
        
        $.ajax({
            url: "/PreventivePlan/CanSaveTemplatePreventivePlan",
            data: {
                "preventivePlanId": DetailPreventivePlan.id,
                "templateId": dropdownlist.value()
            },
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.exist === false) {
                    DetailPreventivePlan.tabStrip.append({
                        text: dropdownlist.text(),
                        content: kendo.format("<textarea id='{0}_editor' rows='10' cols='30' style='height: 43vh; width: 100%'>{1}</textarea>", dropdownlist.value(), response.text)
                    });
                    DetailPreventivePlan.createEditor(dropdownlist.value());
                    DetailPreventivePlan.tabStrip.select(DetailPreventivePlan.tabStrip.items().length - 1);

                    DetailPreventivePlan.listTemplates.push(parseInt(dropdownlist.value()));

                    $("#" + DetailPreventivePlan.btnSavePreventivePlanTemplateId).show();
                    $("#" + DetailPreventivePlan.btnDeletePreventivePlanTemplateId).show();
                    $("#" + DetailPreventivePlan.btnZoomPreventivePlanTemplateId).hide();
                } else {
                    GeneralData.showNotification("Existe esta Plantilla dentro de la Gestión Preventiva de la Empresa", "", "error");
                }
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");
            }
        });       
    },

    onChangeTemplate: function() {
        var dropdownlist = $("#" + DetailPreventivePlan.inputTemplatePreventivePlanId).data("kendoDropDownList");
        if (dropdownlist.value() !== "") {
            $("#" + DetailPreventivePlan.btnAddTemplateId).removeAttr("disabled");
            $("#" + DetailPreventivePlan.btnAddTemplateId).prop("disabled", false);
        } else {
            $("#" + DetailPreventivePlan.btnAddTemplateId).removeAttr("disabled");
            $("#" + DetailPreventivePlan.btnAddTemplateId).prop("disabled", true);
        }
    },

    createEditor: function (templateId, text) {
        $.ajax({
            url: "/PreventivePlan/GetEditorSnippets",
            data: JSON.stringify({
                "preventivePlanId": DetailPreventivePlan.id,
                "templateId": templateId
            }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            async: false,
            beforeSend: function () {
                kendo.ui.progress($("#pageDetailPreventivePlan"), true);
            },
            success: function (response) {
                var items = [];
                $.each(response.snippets, function (key, value) {
                    items.push({ text: key, value: value });
                });
                var snippets = {
                    name: "insertHtml",
                    items: items
                };

                $("#" + templateId + "_editor").kendoEditor({
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
                var editor = $("#" + templateId + "_editor").data("kendoEditor");
                editor.value(text);

                kendo.ui.progress($("#pageDetailPreventivePlan"), false);
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");

                kendo.ui.progress($("#pageDetailPreventivePlan"), false);
            }
        });    
    },

    savePreventivePlanTemplate: function () {
        var templateId = this.listTemplates[this.tabStrip.select().index()];

        var editor = $("#" + templateId + "_editor").data("kendoEditor");
        var text = editor.value();

        $.ajax({
            url: "/PreventivePlan/SaveTemplatePreventivePlan",
            data: JSON.stringify({
                "preventivePlanId": DetailPreventivePlan.id,
                "templateId": templateId,
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

    deletePreventivePlanTemplate: function () {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Gestión Preventiva</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Plan de Actuación?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var templateId = DetailPreventivePlan.listTemplates[DetailPreventivePlan.tabStrip.select().index()];

                        $.ajax({
                            url: "/PreventivePlan/DeleteTemplatePreventivePlan",
                            data: JSON.stringify({
                                "preventivePlanId": DetailPreventivePlan.id,
                                "templateId": templateId
                            }),
                            contentType: "application/json; charset=utf-8",
                            type: "post",
                            dataType: "json",
                            success: function (response) {
                                if (response.resultStatus === Constants.resultStatus.Ok) {
                                    var indexSelected = DetailPreventivePlan.tabStrip.select().index();
                                    DetailPreventivePlan.tabStrip.remove(DetailPreventivePlan.tabStrip.select());
                                    if (indexSelected !== 0) {
                                        DetailPreventivePlan.tabStrip.select(indexSelected - 1);
                                    } else {
                                        DetailPreventivePlan.tabStrip.select(0);
                                    }

                                    DetailPreventivePlan.listTemplates = jQuery.grep(DetailPreventivePlan.listTemplates, function (value) {
                                        return value !== templateId;
                                    });

                                    if (DetailPreventivePlan.listTemplates == null || DetailPreventivePlan.listTemplates.length === 0) {
                                        $("#" + DetailPreventivePlan.btnSavePreventivePlanTemplateId).hide();
                                        $("#" + DetailPreventivePlan.btnDeletePreventivePlanTemplateId).hide();
                                        $("#" + DetailPreventivePlan.btnZoomPreventivePlanTemplateId).hide();
                                    } else {
                                        $("#" + DetailPreventivePlan.btnSavePreventivePlanTemplateId).show();
                                        $("#" + DetailPreventivePlan.btnDeletePreventivePlanTemplateId).show();
                                        $("#" + DetailPreventivePlan.btnZoomPreventivePlanTemplateId).show();
                                    }

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
                }
            ]
        });
        dialog.data("kendoDialog").open();
    }
});