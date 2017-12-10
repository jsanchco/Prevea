/// <reference path="~/Scripts/kendo/2016.3.1118/kendo.all-vsdoc.js" />

var Simulations = kendo.observable({

    gridSimulationsId: "gridSimulations",
    confirmId: "confirm",

    simulationsDataSource: null,

    init: function () {
        this.createSimulationsDataSource();
        this.createGridSimulations();
    },

    createSimulationsDataSource: function () {
        this.simulationsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        CompanyName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        NIF: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        NumberEmployees: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Date: { type: "date", editable: false },
                        SimulationStateId: { type: "number", editable: false, defaultValue: 1 },
                        SimulationStateName: { type: "string", editable: false, defaultValue: "ValidationPending" },
                        SimulationStateDescription: { type: "string", editable: false, defaultValue: "Pendiente de Validación" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/CommercialTool/Simulations/Simulations_Read",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/CommercialTool/Simulations/Simulations_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/CommercialTool/Simulations/Simulations_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { simulation: kendo.stringify(options) };
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

    createGridSimulations: function () {
        //var that = this;
        $("#" + this.gridSimulationsId).kendoGrid({
            columns: [
                {
                    field: "CompanyName",
                    title: "Razón Social",
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateIncrease(data.CompanyName) #"
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
                    title: "Estado",
                    field: "SimulationStateDescription",
                    template: "#= Simulations.getTemplateSimulationState(data) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= Simulations.getColumnTemplateCommands(data) #"
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
            dataSource: this.simulationsDataSource,
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
        kendo.bind($("#" + this.gridSimulationsId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createSimulation'>";
        html += "<a class='btn btn-prevea' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getTemplateSimulationState: function (data) {
        var html = kendo.format("<div style='float: left; text-align: left; display: inline;'>{0}</div>", data.SimulationStateDescription);

        if (data.SimulationStateId === 1) {
            html += kendo.format("<div id='circleError' style='float: right; text-align: right;'></div></div>");
        }
        if (data.SimulationStateId === 2) {
            html += kendo.format("<div id='circleWarning' style='float: right; text-align: right;'></div></div>");
        }
        if (data.SimulationStateId === 3) {
            html += kendo.format("<div id='circleSuccess' style='float: right; text-align: right;'></div></div>");
        }

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        if (data.IsBlocked === true) {
            html += kendo.format("<a toggle='tooltip' title='Ir a Empresa' onclick='Simulations.goToCompanyFromSimulation(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='Simulations.goToDetailSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Simulations.goToDeleteSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        html += kendo.format("</div>");

        return html;
    },

    goToSimulations: function () {
        var params = {
            url: "/CommercialTool/Simulations/Simulations",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToDetailSimulation: function (id) {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteSimulation: function (simulationId) {
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
                        var grid = $("#" + Simulations.gridSimulationsId).data("kendoGrid");
                        var item = grid.dataSource.get(simulationId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToCompanyFromSimulation: function (id) {
        var params = {
            url: "/CommercialTool/Simulations/CompanyFromSimulation",
            data: {
                simulatorId: id
            }
        };
        GeneralData.goToActionController(params);
    }
});