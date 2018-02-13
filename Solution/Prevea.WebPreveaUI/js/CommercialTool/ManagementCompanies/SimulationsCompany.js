﻿var SimulationsCompany = kendo.observable({

    companyId: null,

    gridSimulationsCompanyId: "gridSimulationsCompany",
    simulationsCompanyDataSource: null,

    init: function (companyId) {
        this.companyId = companyId;

        this.createSimulationsCompanyDataSource();
        this.createGridSimulationsCompany();
    },

    createSimulationsCompanyDataSource: function () {
        this.simulationsCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        UserId: { type: "number" },
                        UserInitials: { type: "string", editable: false },
                        UserAssignedId: { type: "number", defaultValue: null },
                        UserAssignedInitials: { type: "string", editable: false },
                        CompanyName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        CompanyId: { type: "number", editable: false },
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
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { simulation: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            pageSize: 20
        });
    },

    createGridSimulationsCompany: function () {
        $("#" + this.gridSimulationsCompanyId).kendoGrid({
            columns: [
                {
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
            dataSource: this.simulationsCompanyDataSource,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false
        });
        kendo.bind($("#" + this.gridSimulationsCompanyId), this);
    }
});