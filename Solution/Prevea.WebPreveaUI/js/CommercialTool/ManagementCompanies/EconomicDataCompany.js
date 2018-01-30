var EconomicDataCompany = kendo.observable({

    gridTrainingCoursesId: "gridTrainingCourses",
    switchSPAId: "switchSPA",
    trainingCoursesDataSource: null,

    companyId: null,
    simulationId: null,
    stateSPA: null,
    stateSG: null,
    stateSF: null,

    init: function (companyId, simulationId, stateSPA, stateSG, stateSF) {
        kendo.culture("es-ES");

        this.companyId = companyId;
        this.simulationId = simulationId;
        this.stateSPA = stateSPA;
        this.stateSG = stateSG;
        this.stateSF = stateSF;

        this.createTrainingCoursesDataSource();
        this.createTrainingCoursesGrid();

        this.setKendoUIWidgets();
    },

    createTrainingCoursesDataSource: function () {
        this.trainingCoursesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        AssistantsNumber: { type: "number" },
                        Price: { type: "number" },
                        Total: { type: "number" },
                        TrainingCourseName: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Simulations/Courses_Read",
                    dataType: "jsonp",
                    data: { trainingServiceId: this.simulationId }
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { trainingServiceId: options.trainingServiceId };
                    }
                    return null;
                }
            },
            aggregate: [{ field: "Total", aggregate: "sum" }],
            pageSize: 10
        });
    },

    createTrainingCoursesGrid: function () {
        $("#" + this.gridTrainingCoursesId).kendoGrid({
            columns: [{
                field: "TrainingCourseName",
                title: "Nombre del Curso",
                template: "#= TrainingService.getColumnTemplateNameCourse(data.TrainingCourseName) #"
            }, {
                field: "AssistantsNumber",
                title: "Asistentes",
                width: 130,
                groupable: "false",
                template: "#= Templates.getColumnTemplateRight(data.AssistantsNumber) #"
            }, {
                field: "Price",
                title: "Precio",
                width: 100,
                groupable: "false",
                format: "{0:c}",
                template: "#= Templates.getColumnTemplateCurrencyRight(data.Price, 'c0') #"
            }, {
                field: "Total",
                title: "Total",
                width: 120,
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
            dataSource: this.trainingCoursesDataSource,
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
            groupable: false
        });
        kendo.bind($("#" + this.gridTrainingCoursesId), this);
    },

    setKendoUIWidgets: function () {
        $("#" + this.switchSPAId).kendoMobileSwitch({
            onLabel: "SI",
            offLabel: "NO"
        });
    }
});