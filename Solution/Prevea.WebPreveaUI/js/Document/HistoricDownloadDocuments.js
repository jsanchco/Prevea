var HistoricDownloadDocuments = kendo.observable({

    gridHistoricDownloadDocumentsId: "gridHistoricDownloadDocuments",
    historicDownloadDocumentDataSource: null,

    init: function () {
        this.createHistoricDownloadDocumentDataSource();
        this.createGridHistoricDownloadDocument();
    },

    createHistoricDownloadDocumentDataSource: function () {
        this.historicDownloadDocumentDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        DocumentId: { type: "int" },
                        DocumentName: { type: "string" },
                        UserName: { type: "string" },
                        Date: { type: "date" },
                        UrlRelative: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Document/HistoricDownloadDocument_Read",
                    dataType: "jsonp",
                    data: {}
                }
            },
            pageSize: 10
        });
    },

    createGridHistoricDownloadDocument: function () {
        $("#" + this.gridHistoricDownloadDocumentsId).kendoGrid({
            columns: [{
                    field: "DocumentName",
                    title: "Documento",
                    width: 90,
                    groupable: "true"
                },
                {
                    field: "UserName",
                    title: "Usuario",
                    width: 90,
                    groupable: "true"
                },
                {
                    field: "Date",
                    title: "Fecha",
                    width: 160,
                    template: "#= Templates.getColumnTemplateDateWithHour(data.Date) #"
                },
                {
                    field: "Url",
                    title: "Documento",
                    width: 115,
                    groupable: "false",
                    filterable: false,
                    template: "#= HistoricDownloadDocuments.getColumnTemplateUrl(data) #"
                }],
            pageable: {
                buttonCount: 2,
                pageSizes: [10, 20, "all"],
                refresh: true,
                messages: {
                    display: "Elementos mostrados {0} - {1} de {2}",
                    itemsPerPage: "Elementos por página",
                    allPages: "Todos"
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
            dataSource: this.historicDownloadDocumentDataSource,
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
        kendo.bind($("#" + this.gridHistoricDownloadDocumentsId), this);
    },

    getColumnTemplateUrl: function (data) {
        var html;
        if (data.Icon === "unknown_opt.png") {
            html = "<div align='center'><a toggle='tooltip' title='No se encuentra el Documento'>";
        } else {
            html = kendo.format(
                "<div align='center'><a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenFile(\"{0}\")' target='_blank' style='cursor: pointer;'>",
                data.DocumentId);
        }

        html += kendo.format("<img src='../../Images/{0}'></a></div>", data.Icon);

        return html;
    }

});