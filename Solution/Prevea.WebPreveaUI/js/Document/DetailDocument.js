var DetailDocument = kendo.observable({

    // Fields
    id: null,
    documentParentId: null,
    gridDocumentDetailId: "gridDocumentDetail",

    // Datasources
    documentDetailDataSource: null,

    init: function (id, documentParentId) {
        this.id = id;

        if (documentParentId) {
            this.documentParentId = documentParentId;
        } else {
            this.documentParentId = null;
        }
           
        this.createDataSources();
        this.createKendoWidgets();
    },

    createDataSources: function () {
        this.createDocumentDataSource();
    },

    createKendoWidgets: function() {
        this.createGridDocumentDetail();
    },

    createDocumentDataSource: function () {
        this.documentDetailDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Edition: { type: "number" },
                        Observations: { type: "string" },
                        Date: { type: "date" },
                        UrlRelative: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Document/DocumentsByParent_Read",
                    dataType: "jsonp",
                    data: {
                        id: DetailDocument.id,
                        documentParentId: DetailDocument.documentParentId
                    }
                }
            },
            pageSize: 10
        });
    },

    createGridDocumentDetail: function () {
        $("#" + this.gridDocumentDetailId).kendoGrid({
            columns: [{
                field: "Edition",
                title: "Edición",
                width: 100,
                groupable: "false",
                template: "#= Templates.getColumnTemplateEdition(data) #"
            },
            {
                field: "Observations",
                title: "Observaciones"
            },
            {
                field: "Date",
                title: "Fecha",
                width: 160,
                template: "#= Templates.getColumnTemplateDate(data.Date) #"
            },
            {
                field: "UrlRelative",
                title: "Documento",
                width: 115,
                groupable: "false",
                filterable: false,
                template: "#= Templates.getColumnTemplateUrl(data) #"
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
            dataSource: this.documentDetailDataSource,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false

        });
        kendo.bind($("#" + this.gridDocumentDetailId), this);

        $("#" + this.gridDocumentDetailId).on("dblclick", "tr", function () {
        });
    },

    goToDetailDocument: function (e) {
        var params = {
            url: "/Document/DetailDocument",
            data: {
                id: this.id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToAddDocumentWithParent: function (e) {
        var params = {
            url: "/Document/AddDocumentWithParent",
            data: {
                id: this.id
            }
        };
        GeneralData.goToActionController(params);
    }

});