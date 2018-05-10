var DeltaCodes = kendo.observable({

    gridDeltaCodesId: "gridDeltaCodes",
    confirmId: "confirm",
    deltaCodesDataSource: null,

    init: function () {
        kendo.culture("es-ES");

        this.createDeltaCodesDataSource();
        this.createDeltaCodesGrid();
    },

    goToDeltaCodes: function () {
        var params = {
            url: "/Tecniques/DeltaCodes",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    createDeltaCodesDataSource: function () {
        this.deltaCodesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", editable: false },
                        Name: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Description: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/DeltaCodes_Read",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Tecniques/DeltaCodes_Create",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Tecniques/DeltaCodes_Destroy",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Tecniques/DeltaCodes_Update",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { deltaCode: kendo.stringify(options) };
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
            //pageSize: 10
        });
    },

    createDeltaCodesGrid: function () {
        $("#" + this.gridDeltaCodesId).kendoGrid({
            columns: [
                {
                    field: "Id",
                    title: "Código",
                    width: 120,
                    template: "#= DeltaCodes.getColumnTemplateCode(data.Id) #"
                },
                {
                    field: "Name",
                    title: "Nombre",
                    template: "#= Templates.getColumnTemplateBold(data.Name) #"
                },
                {
                    field: "Description",
                    title: "Descripcion"
                },
                {
                    title: "Comandos",
                    width: 120,
                    groupable: "false",
                    filterable: false,
                    template: "#= DeltaCodes.getColumnTemplateCommands(data) #"
                }
            ],
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
            dataSource: this.deltaCodesDataSource,
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

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createDeltaCode'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "";
        if (data.Id > 32) {
            html = "<div align='center'>";
            html += kendo.format("<a toggle='tooltip' title='Editar' onclick='DeltaCodes.goToEditDeltaCode(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='DeltaCodes.goToDeleteDeltaCode(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("</div>");
        }

        return html;
    },

    getColumnTemplateCode: function (code) {
        if (code === null || code === 0) {
            return "";
        }

        var html = kendo.format("<div style='font-size: 15px; font-weight: bold'>{0}</div>",
            code);

        return html;
    },

    goToEditDeltaCode: function (id) {
        var grid = $("#" + DeltaCodes.gridDeltaCodesId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteDeltaCode: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Técnicas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Código Delta?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + that.gridDeltaCodesId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    }
});