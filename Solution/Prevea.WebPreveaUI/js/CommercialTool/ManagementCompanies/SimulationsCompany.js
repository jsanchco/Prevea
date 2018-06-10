var SimulationsCompany = kendo.observable({

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
                        SimulationStateDescription: { type: "string", editable: false, defaultValue: "Pendiente de Validación" },
                        Active: { type: "boolean" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Simulations/SimulationsAll_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
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
                    template: "#= SimulationsCompany.getTemplateSimulationState(data) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= SimulationsCompany.getColumnTemplateCommands(data) #"
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
            detailTemplate: SimulationsCompany.getTemplateChildren(),
            detailInit: SimulationsCompany.childrenSimulationsCompany,
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

        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            var grid = $("#" + this.gridSimulationsCompanyId).data("kendoGrid");
            grid.hideColumn("Commands");
        }
    },

    getTemplateSimulationState: function (data) {
        var active = "Activa";
        if (data.Active === false)
            active = "No Activa";

        var html;
        if (data.Active) {
            html = kendo.format("<div align='center'><strong>{0}</strong></div>",
                active);
        } else {
            html = kendo.format("<div align='center'>{0}</div>",
                data.SimulationStateDescription);
        }

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format(
            "<a toggle='tooltip' title='Ir a Simulación' onclick='SimulationsCompany.goToSimulationFromSimulationsCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
            data.Id);
        html += kendo.format("</div>");

        return html;
    },

    getTemplateChildren: function () {
        var html = "<div id='templateEconomicDataSimulation' style='border: 1px solid; border-radius: 10px;'>";
        html += "<H2 style='text-align: center;'><strong>Datos Económicos</strong></H2><br />";
        html += "<div class='economicDataView'></div><br /><br />";
        html += "</div>";

        return html;
    },

    goToSimulationFromSimulationsCompany: function (simulationId) {
        var params = {
            url: "/Simulations/DetailSimulation",
            data: {
                simulationId: simulationId,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    childrenSimulationsCompany: function(e) {
        var params = {
            url: "/Companies/EconomicDataSimulation",
            data: {
                simulationId: e.data.Id
            }
        };
        $.ajax({
            url: params.url,
            type: "GET",
            cache: false,
            datatype: "html",
            data: params.data,
            success: function (result) {
                var detailRow = e.detailRow;
                var economicDataView = detailRow.find(".economicDataView");
                economicDataView.html(result);

                $("*[id*=templateEconomicDataSimulation]").each(function () {
                    $(this).css("border-color", "#BFBFBF");
                });

                //$("#templateEconomicDataSimulation").css("border-color", "#BFBFBF");
            }
        });
    }
});