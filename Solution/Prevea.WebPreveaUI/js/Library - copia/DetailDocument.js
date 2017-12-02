var DetailDocument = kendo.observable({

    // Fields
    id: null,
    parentId: null,
    gridLibraryId: "gridLibraryDetail",

    // Datasources
    libraryDetailDataSource: null,

    init: function (id, parentId) {
        this.id = id;
        if (parentId) {
            this.parentId = parentId;
        } else {
            this.parentId = null;
        }
           
        this.createDataSources();
        this.createKendoWidgets();
    },

    createDataSources: function () {
        this.createLibraryDataSource();
    },

    createKendoWidgets: function() {
        this.createGridLibraryDetail();
    },

    createLibraryDataSource: function () {
        this.libraryDetailDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Edition: { type: "number" },
                        Observations: { type: "string" },
                        DateModication: { type: "date" },
                        Url: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Library/LibraryByParent_Read",
                    dataType: "jsonp",
                    data: {
                        id: DetailDocument.id,
                        parentId: DetailDocument.parentId
                    }
                }
            },
            pageSize: 10
        });
    },

    createGridLibraryDetail: function () {
        $("#" + this.gridLibraryId).kendoGrid({
            columns: [{
                field: "Edition",
                title: "Edición",
                width: 90,
                groupable: "false",
                template: "#= Library.getColumnTemplateEdition(data) #"
            },
            {
                field: "Observations",
                title: "Observaciones"
            },
            {
                field: "DateModification",
                title: "Fecha",
                width: 160,
                template: "#= DetailDocument.getColumnTemplateDate(data) #"
            },
            {
                field: "Url",
                title: "Documento",
                width: 115,
                groupable: "false",
                filterable: false,
                template: "#= Library.getColumnTemplateUrl(data) #"
            }],
            pageable: {
                buttonCount: 2,
                pageSizes: [10, 20, "all"],
                refresh: true,
                messages: {
                    display: "Elementos mostrados {0} - {1} de {2}",
                    itemsPerPage: "Elementos por página",
                    allPages: 'Todos'
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
            }
        });

        $("#" + this.gridLibraryId).on("dblclick", "tr", function () {
            var grid = $("#" + DetailDocument.gridLibraryId).data("kendoGrid");
            var model = grid.dataItem(this);

            //OpenDetailLibrary(model.Id, model.ParentId);

        });
    },

    goToDetailDocument: function (e) {
        var params = {
            url: "/Library/DetailDocument",
            data: {
                id: this.id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToAddDocumentWithParent: function (e) {
        var params = {
            url: "/Library/AddFileDocumentWithParent",
            data: {
                id: this.id
            }
        };
        GeneralData.goToActionController(params);
    },

    getColumnTemplateDate: function (data) {
        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(data.DateModication, "dd/MM/yy"));

        return html;
    }

});