var ExcelMails = kendo.observable({
    mailingId: null,

    spreadSheetDataMailsId: "spreadSheetDataMails",
    btnSaveBodyMailId: "btnSaveBodyMail",

    dataMailsDataSource: null,

    init: function (mailingId) {
        this.mailingId = mailingId;

        this.createKendoWidgets();

        //$.ajax({
        //    url: "/Mailings/GetBodyMail",
        //    data: JSON.stringify({ "mailingId": Mail.mailingId }),
        //    contentType: "application/json; charset=utf-8",
        //    type: "post",
        //    dataType: "json",
        //    beforeSend: function () {
        //        kendo.ui.progress($("#tabStripDetailMailing"), true);
        //    },
        //    success: function (response) {
        //        if (response.resultStatus === Constants.resultStatus.Ok) {
        //            if (response.bodyMail == null) {
        //                Mail.bodyMail = "";
        //            } else {
        //                Mail.bodyMail = response.bodyMail;
        //            }
        //            Mail.createKendoWidgets();

        //            kendo.ui.progress($("#tabStripDetailMailing"), false);
        //        } else {
        //            GeneralData.showNotification(error, "", "error");

        //            kendo.ui.progress($("#tabStripDetailMailing"), false);
        //        }
        //    },
        //    error: function (xhr, status, error) {
        //        GeneralData.showNotification(error, "", "error");

        //        kendo.ui.progress($("#tabStripDetailMailing"), false);
        //    }
        //});

        //this.updateButtons();
    },

    createDataMailsDataSource: function () {
        this.dataMailsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        MailingId: { type: "number", defaultValue: DataMails.mailingId },
                        EMail: { type: "string", editable: false },
                        CreatorId: { type: "int", editable: false },
                        CreatorInitials: { type: "string", editable: false },
                        Observations: { type: "string" },
                        Data: { type: "string" },
                        DataMailStateId: { type: "number", defaultValue: 1 },
                        DataMailStateDescription: { type: "string", defaultValue: "Pendiente" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Mailings/DataMails_Read",
                    dataType: "jsonp",
                    data: {
                        id: this.mailingId
                    }
                },
                update: {
                    url: "/Mailings/DataMails_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Mailings/DataMails_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Mailings/DataMails_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read" && options) {
                        return { mailingId: options.id };
                    }

                    if (operation !== "read" && options) {
                        return { dataMail: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "update" || e.type === "destroy" || e.type === "create") &&
                    e.response !== null) {
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(Constants.ko, "", "error");
                        if (e.type === "create") {
                            this.data().remove(this.data().at(0));
                        } else {
                            this.cancelChanges();
                        }
                    } else {
                        GeneralData.showNotification(Constants.ok, "", "success");
                    }
                }
            },
            pageSize: 20
        });
    },

    createKendoWidgets: function () {
        $("#" + this.spreadSheetDataMailsId).kendoSpreadsheet({
            columns: 20,
            rows: 100,
            toolbar: false,
            sheetsbar: false,
            sheets: [
                {
                    name: "Datos Mails",
                    dataSource: ExcelMails.dataMailsDataSource,
                    mergedCells: [
                        "A1:B1"
                    ],
                    rows: [
                        {
                            height: 70,
                            cells: [
                                {
                                    index: 0, value: "Datos Mails", fontSize: 32, background: "rgb(96,181,255)",
                                    textAlign: "center", color: "white"
                                }
                            ]
                        },
                        {
                            height: 40,
                            cells: [
                                {
                                    value: "EMail", background: "rgb(167,214,255)", fontSize: 20, textAlign: "center", color: "rgb(0,62,117)"
                                },
                                {
                                    value: "Creador", background: "rgb(167,214,255)", fontSize: 20, textAlign: "center", color: "rgb(0,62,117)"
                                }      
                            ]
                        }
                    ],
                    columns: [
                        { width: 400 },
                        { width: 300 }
                    ]
                }
            ]
        });

        var spreadsheet = $("#" + this.spreadSheetDataMailsId).data("kendoSpreadsheet");

        var range = spreadsheet.activeSheet().range("A1:B1");
        range.enable(false);
        range = spreadsheet.activeSheet().range("A2:B2");
        range.enable(false);

    },

    saveBodyMail: function () {
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

    goToSaveDataMails: function () {
        $.ajax({
            url: "/Mailings/HasDataMailsInDataBase",
            data: JSON.stringify({
                "mailingId": DataMails.mailingId
            }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            async: false,
            success: function (response) {
                if (response.resultStatus === true) {
                    var dialog = $("#" + DataMails.confirmId);
                    dialog.kendoDialog({
                        width: "400px",
                        title: "<strong>Herramienta Comercial</strong>",
                        closable: false,
                        modal: true,
                        content:
                            "Se borrarán todos los datos existente y se copiarán estos nuevos, ¿Quieres <strong>Borrar & Guardar</strong> estos Datos?",
                        actions: [
                            {
                                text: "Cancelar",
                                primary: true
                            },
                            {
                                text: "Borrar & Guardar",
                                action: function () {
                                    DataMails.saveDataMails();
                                }
                            }
                        ]
                    });
                    dialog.data("kendoDialog").open();
                } else {
                    DataMails.saveDataMails();
                }
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");
            }
        });
    },

    pasteData: function (text) {
        this.dataMailsDataSource.data([]);

        var lines = text.split("\r\n");
        for (var i = 0; i < lines.length; i++) {
            var fields = lines[i].split("\t");
            if (fields.length > 0 && (fields[0] == null || fields[0] === "")) {
                continue;
            }

            var data = "";
            for (var j = 0; j < fields.length; j++) {
                if (j === 0 || j === 1) {
                    continue;
                } else {
                    var nameColumn = kendo.format("[Columna{0}]", j - 1);
                    data += nameColumn + fields[j];
                }
            }
            this.dataMailsDataSource.add({
                "EMail": fields[0],
                "CreatorId": parseInt(fields[1]),
                "Data": data,
                "MailingId": this.mailingId,
                "DataMailStateId": 1
            });
        }

        var grid = $("#" + this.gridDataMailsId).data("kendoGrid");
        grid.setDataSource(this.dataMailsDataSource);

        var textnumberDataMails = kendo.format("Mailing preparado para {0} personas", this.dataMailsDataSource.data().length);
        $("#" + this.numberDataMailsId).text(textnumberDataMails);
    },

    saveDataMails: function () {
        var changes = $.map(DataMails.dataMailsDataSource.data(), function (item) {
            var dataMail = {
                MailingId: item.MailingId,
                EMail: item.EMail,
                CreatorId: item.CreatorId,
                Data: item.Data,
                Observations: item.Observations,
                DataMailStateId: item.DataMailStateId
            }
            return dataMail;
        });
        $.ajax({
            url: "/Mailings/SaveDataMails",
            data: JSON.stringify({
                "mailingId": DataMails.mailingId,
                "dataMails": changes
            }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    DataMails.dataMailsDataSource.read();
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
    updateButtons: function () {
        if (DetailMailing.mailingState === "true") {
            $("#" + this.btnSaveBodyMailId).removeAttr("disabled");
            $("#" + this.btnSaveBodyMailId).prop("disabled", true);
        }
    }
});