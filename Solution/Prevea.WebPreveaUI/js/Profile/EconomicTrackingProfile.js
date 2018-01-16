var EconomicTrackingProfile = kendo.observable({

    gridEconomicTrackingId: "gridEconomicTracking",

    economicTrackingDataSource: null,
    userId: null,

    init: function (userId) {
        this.userId = userId;

        this.setUpWidgets();
    },

    setUpWidgets: function () {
        this.createEconomicTrackingDataSource();
        this.createGridEconomicTracking();
    },

    createEconomicTrackingDataSource: function () {
        this.economicTrackingDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        CompanyEnrollment: { type: "string" },
                        CompanyName: { type: "string" },
                        TotalForeignPreventionService: { type: "number" },
                        TotalAgencyService: { type: "number" },
                        TotalTrainingService: { type: "number" },
                        Total: { type: "number" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Profile/Simulations_Read",
                    dataType: "jsonp",
                    data: { userId: this.userId }
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { userId: options.userId };
                    }

                    return null;
                }
            },
            aggregate: [
                { field: "TotalForeignPreventionService", aggregate: "sum" },
                { field: "TotalAgencyService", aggregate: "sum" },
                { field: "TotalTrainingService", aggregate: "sum" },
                { field: "Total", aggregate: "sum" }],
            pageSize: 10
        });
    },

    createGridEconomicTracking: function () {
        $("#" + this.gridEconomicTrackingId).kendoGrid({
            columns: [
                {
                    field: "CompanyEnrollment",
                    title: "Matrícula",
                    width: 30,
                    groupable: "false",
                    template: "#= EconomicTrackingProfile.getColumnTemplateEnrollment(data.CompanyEnrollment) #"
                }, {
                    field: "CompanyName",
                    title: "Nombre",
                    width: 250,
                    groupable: "false"
                }, {
                    field: "TotalForeignPreventionService",
                    title: "SPA",
                    width: 50,
                    groupable: "false",
                    format: "{0:c}",
                    template: "#= Templates.getColumnTemplateCurrencyRight(data.TotalForeignPreventionService, 'c0') #",
                    aggregates: ["sum"],
                    footerTemplate: "Suma: #= kendo.toString(sum, 'c0') #"
                }, {
                    field: "TotalAgencyService",
                    title: "Gestoría",
                    width: 50,
                    groupable: "false",
                    format: "{0:c}",
                    template: "#= Templates.getColumnTemplateCurrencyRight(data.TotalAgencyService, 'c0') #",
                    aggregates: ["sum"],
                    footerTemplate: "Suma: #= kendo.toString(sum, 'c0') #"
                }, {
                    field: "TotalTrainingService",
                    title: "Formación",
                    width: 50,
                    groupable: "false",
                    format: "{0:c}",
                    template: "#= Templates.getColumnTemplateCurrencyRight(data.TotalTrainingService, 'c0') #",
                    aggregates: ["sum"],
                    footerTemplate: "Suma: #= kendo.toString(sum, 'c0') #"
                }, {
                    field: "Total",
                    title: "Total",
                    width: 50,
                    groupable: "false",
                    format: "{0:c}",
                    template: "#= Templates.getColumnTemplateCurrencyRight(data.Total, 'c0') #",
                    aggregates: ["sum"],
                    footerTemplate: "Suma: #= kendo.toString(sum, 'c0') #"
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
            dataSource: this.economicTrackingDataSource,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false
        });
        kendo.bind($("#" + this.gridEconomicTrackingId), this);
    },

    getColumnTemplateEnrollment: function (companyEnrollment) {
        var html = kendo.format("<div style='text-align: center; font-size: 16px; font-weight: bold;'>{0}</div>", companyEnrollment);

        return html;
    }
});