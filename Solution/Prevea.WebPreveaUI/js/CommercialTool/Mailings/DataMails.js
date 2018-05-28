var DataMails = kendo.observable({
    confirmId: "confirm",
    gridDataMailsId: "gridDataMails",
    numberDataMailsId: "numberDataMails",
    btnSendMailingId: "btnSendMailing",

    mailingId: null,
    dataMailsDataSource: null,

    init: function (mailingId) {
        this.mailingId = mailingId;

        this.createDataMailsDataSource();
        this.createDataMailsGrid();

        this.updateButtons();

        $("#pageDetailMailing").addEventListener("paste", handlePaste);
    },

    handlePaste: function (e) {
        // Stop data actually being pasted into div
        e.stopPropagation();
        e.preventDefault();

        // Get pasted data via clipboard API
        var clipboardData = e.clipboardData || window.clipboardData;
        var pastedData = clipboardData.getData("Text");

        // Do whatever with pasteddata
        alert(pastedData);
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

    createDataMailsGrid: function () {
        $("#" + this.gridDataMailsId).kendoGrid({
            columns: [{
                field: "EMail",
                title: "EMail",
                width: 200,
                groupable: "false",
                template: "#= DataMails.getColumnTemplateEMail(data) #"
            }, {
                field: "CreatorInitials",
                title: "Creador",
                width: 150,
                template: "#= Templates.getColumnTemplateBold(data.CreatorInitials) #"
            }, {
                field: "Observations",
                title: "Observaciones",
                groupable: "false",
                width: 250
            }, {
                field: "Data",
                title: "Datos",
                groupable: "false",
                width: 300
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= DataMails.getColumnTemplateCommands(data) #"
            }],
            pageable: {
                buttonCount: 2,
                pageSizes: [20, 40, "all"],
                refresh: true,
                messages: {
                    display: "Elementos mostrados {0} - {1} de {2}",
                    itemsPerPage: "Elementos por página",
                    allPages: "Todos",
                    empty: "No existen registros para mostrar"
                }
            },            
            filterable: {
                messages: {
                    info: "Filtrar por: ",
                    and: "Y",
                    or: "O",
                    filter: "Aplicar",
                    clear: "Limpiar"
                },
                operators: {
                    string: {
                        contains: "Contiene",
                        eq: "Igual a",
                        neq: "No igual a",
                        startswith: "Empieza con",
                        endswith: "Termina con",
                        doesnotcontain: "No contiene",
                        isempty: "Está vacio",
                        isnotnull: "No está vacio"
                    },
                    number: {
                        eq: "Igual a",
                        gt: "Más grande que",
                        lt: "Más pequeño que"
                    },
                    date: {
                        eq: "Igual a",
                        gt: "Antes que",
                        lt: "Después que",
                        isnull: "Está vacio"
                    }
                }
            },
            dataSource: this.dataMailsDataSource,
            toolbar: this.getTemplateToolBar(),
            editable: {
                mode: "inline",
                confirmation: false
            },
            groupable: {
                messages: {
                    empty: "Arrastre un encabezado de columna y póngalo aquí para agrupar por ella"
                }
            },
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='DataMails.goToEditDataMail(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);

        if (DetailMailing.mailingState === "false") {
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='DataMails.goToDeleteDataMail(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id);
        }
        
        html += kendo.format("</div>");

        return html;
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' id='pasteDataMails'>";        
        html += "<a id='btnPasteData' class='btn btn-prevea' role='button' onclick='DataMails.goToPasteData()'> Pegar Datos</a>&nbsp;&nbsp;";
        html += "<a id='btnSaveDataMails' class='btn btn-prevea' role='button' onclick='DataMails.goToSaveDataMails()'> Guardar</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateEMail: function (data) {
        var html;

        html = "<div>";
        html += "<div style='text-align: left'>";

        if (data.DataMailStateId === 1) {
            html += kendo.format("<div id='circleDataEmailStateWarning' toggle='tooltip' title='{0}' style='float: left; text-align: left;'>", data.DataMailStateDescription);
        }
        if (data.DataMailStateId === 2) {
            html += kendo.format("<div id='circleDataEmailStateSuccess' toggle='tooltip' title='{0}' style='float: left; text-align: left;'>", data.DataMailStateDescription);
        }
        if (data.DataMailStateId === 3) {
            html += kendo.format("<div id='circleDataEmailStateError' toggle='tooltip' title='{0}' style='float: left; text-align: left;'>", data.DataMailStateDescription);
        }
        
        html += "</div>";
        html += "</div>";
        html += kendo.format("<div style='font-weight: bold;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}", data.EMail);
        html += "</div></div>";

        return html;
    },

    goToEditDataMail: function (id) {
        var grid = $("#" + DataMails.gridDataMailsId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteDataMail: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Herramienta Comercial</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Mail?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + that.gridDataMailsId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToPasteData: function () {
        var data = window.clipboardData.getData("Text");
        if (data == null || data === "") {
            return;
        }

        this.pasteData(data);
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
                                action: function() {
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
                if (j === 0 || j=== 1) {
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

    sendMailing: function () {
        $.ajax({
            url: "/Mailings/SendMailing",
            data: JSON.stringify({
                "mailingId": DetailMailing.id
            }),
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    GeneralData.showNotification(Constants.ok, "", "success");

                } else {
                    GeneralData.showNotification("Se han producido Errores en el envío de algunos EMails", "", "error");
                }

                for (var i = 0; i < response.dataMails.length; i++) {
                    var dataItem = DataMails.dataMailsDataSource.get(response.dataMails[i].Id);
                    if (dataItem == null) {
                        continue;
                    }

                    dataItem.set("DataMailStateId", response.dataMails[i].DataMailStateId);
                    dataItem.set("DataMailStateDescription", response.dataMails[i].DataMailStateDescription);
                }

                if (response.mailingState === true)
                    DetailMailing.mailingState = "true";
                else
                    DetailMailing.mailingState = "false";
                DetailMailing.createIconMailingState();

                DataMails.updateButtons();

                var grid = $("#" + DataMails.gridDataMailsId).data("kendoGrid");
                grid.refresh();
            },
            error: function (xhr, status, error) {
                GeneralData.showNotification(error, "", "error");
            }
        });
    },

    updateButtons: function() {
        if (DetailMailing.mailingState === "true") {
            $("#btnPasteData").removeAttr("disabled");
            $("#btnPasteData").prop("disabled", true);

            $("#btnSaveDataMails").removeAttr("disabled");
            $("#btnSaveDataMails").prop("disabled", true);

            $("#" + this.btnSendMailingId).removeAttr("disabled");
            $("#" + this.btnSendMailingId).prop("disabled", true);
        }
    }
});