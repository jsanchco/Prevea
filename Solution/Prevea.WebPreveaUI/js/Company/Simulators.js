var Simulators = kendo.observable({

    gridSimulatorsId: "gridSimulators",
    confirmId: "confirm",

    simulatorsDataSource: null,

    init: function () {
        this.createSimulatorsDataSource();
        this.createGridSimulators();
    },

    createSimulatorsDataSource: function () {
        this.simulatorsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Name: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        NIF: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        NumberEmployees: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Date: { type: "date", editable: false },
                        IsBlocked: { type: "boolean" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Company/Simulators_Read",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Company/Simulators_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Company/Simulators_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { simulator: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "update" || e.type === "destroy" || e.type === "create") &&
                    e.response !== null) {
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(Constants.ko, "", "error");
                        this.cancelChanges();
                    } else {
                        GeneralData.showNotification(Constants.ok, "", "success");
                    }
                }
            },
            pageSize: 10
        });
    },

    createGridSimulators: function () {
        //var that = this;
        $("#" + this.gridSimulatorsId).kendoGrid({
            columns: [
                {
                    field: "Name",
                    title: "Razón Social",
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateIncrease(data.Name) #"
                }, {
                    field: "NIF",
                    title: "NIF",
                    width: 150,
                    groupable: "false"
                }, {
                    field: "NumberEmployees",
                    title: "Número de Empleados",
                    width: 200,
                    template: "#= Templates.getColumnTemplateIncreaseRight(data.NumberEmployees) #"
                }, {
                    field: "Date",
                    title: "Fecha",
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateDate(data.Date) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= Simulators.getColumnTemplateCommands(data) #"
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
            dataSource: this.simulatorsDataSource,
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
            groupable: false,
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }

        });
        kendo.bind($("#" + this.gridSimulatorsId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createUser'>";
        html += "<a class='btn btn-prevea' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        if (data.IsBlocked === true) {
            html += kendo.format("<a toggle='tooltip' title='Ir a Empresa' onclick='Simulators.goToCompanyFromSimulator(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='Simulators.goToEditSimulator(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Simulators.goToDeleteSimulator(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        html += kendo.format("</div>");

        return html;
    },

    goToSimulators: function () {
        var params = {
            url: "/Company/Simulators",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToEditSimulator: function (id) {
        var params = {
            url: "/Company/EditSimulator",
            data: {
                simulatorId: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteSimulator: function (simulatorId) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Simulaciones</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> esta Simulación?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + Simulators.gridSimulatorsId).data("kendoGrid");
                        var item = grid.dataSource.get(simulatorId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToCompanyFromSimulator: function (id) {
        var params = {
            url: "/Company/CompanyFromSimulator",
            data: {
                simulatorId: id
            }
        };
        GeneralData.goToActionController(params);
    }
});