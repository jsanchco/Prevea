var Mailings = kendo.observable({

    gridMailingsId: "gridMailings",
    confirmId: "confirm",

    mailingsDataSource: null,

    init: function () {
        this.createMailingsDataSource();
        this.createMailingsGrid();
    },

    createMailingsDataSource: function () {
        this.mailingsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Name: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        CreateDate: { type: "date", editable: false },
                        SendDate: { type: "date", editable: false, defaultValue: null },
                        Sent: { type: "boolean", editable: false }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Mailings/Mailings_Read",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Mailings/Mailings_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Mailings/Mailings_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Mailings/Mailings_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { mailing: kendo.stringify(options) };
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
            pageSize: 10
        });
    },

    createMailingsGrid: function () {
        $("#" + this.gridMailingsId).kendoGrid({
            columns: [{
                field: "Name",
                title: "Descripción",
                width: 200
            }, {
                field: "Subject",
                title: "Asunto",
                width: 250
            }, {
                field: "CreateDate",
                title: "Fecha de Creación",
                width: 130,
                groupable: "true",
                template: "#= Templates.getColumnTemplateDateWithHour(data.CreateDate) #"
            }, {
                field: "SendDate",
                title: "Fecha de Envío",
                width: 130,
                groupable: "true",
                template: "#= Templates.getColumnTemplateDateWithHour(data.SendDate) #"
            }, {
                field: "Sent",
                title: "Enviado",
                width: 130,
                groupable: "true",
                template: "#= Mailings.getTemplateSent(data.Sent) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= Mailings.getColumnTemplateCommands(data) #"
            }],
            pageable: {
                buttonCount: 2,
                pageSizes: [10, 20, "all"],
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
            dataSource: this.mailingsDataSource,
            toolbar: this.getTemplateToolBar(),
            editable: {
                mode: "inline",
                confirmation: false
            },
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: {
                messages: {
                    empty: "Arrastre un encabezado de columna y póngalo aquí para agrupar por ella"
                }
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
        kendo.bind($("#" + this.gridMailingsId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createMailing'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getTemplateSent: function (data) {
        var html;

        if (data === true) {
            html = "<div style='float: left; text-align: left; display: inline; font-weight: bold;'>Si</div>";
            html += kendo.format("<div id='circleSuccess' style='float: right; text-align: right;'></div></div>");
        } else {
            html = "<div style='float: left; text-align: left; display: inline; font-weight: bold;'>No</div>";
            html += kendo.format("<div id='circleError' style='float: right; text-align: right;'></div></div>");
        }
  
        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";

        html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='Mailings.goToDetailMailing(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);

        if (data.Sent === false) {
            html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Mailings.goToEditMailing(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);        
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Mailings.goToDeleteMailing(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        
        html += kendo.format("</div>");

        return html;
    },

    goToDetailMailing: function(id) {
        var params = {
            url: "/Mailings/DetailMailing",
            data: {
                id: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToEditMailing: function (id) {
        var grid = $("#" + Mailings.gridMailingsId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteMailing: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Herramienta Comercial</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> Mailing?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + that.gridMailingsId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToMailings: function () {
        var params = {
            url: "/Mailings/Mailings",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});