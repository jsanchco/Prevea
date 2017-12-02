var DeleteDocument = kendo.observable({
    btnDeleteId: "btnDelete",
    btnUnsubscribeId: "btnUnsubscribe",
    btnCancelId: "btnCancel",
    gridDocumentDetailId: "gridDocumentDetail",
    iconViewFileId: "iconViewFile",
    confirmId: "confirm",

    // Fields
    id: null,
    documentParentId: null,
    icon: null,

    // Datasources
    documentDetailDataSource: null,

    init: function () {
        this.id = $("#Id").val();
        this.documentParentId = $("#DocumentParentId").val();
        this.icon = $("#Icon").val();

        this.createDataSources();
        this.createKendoWidgets();
        this.createIconViewFile();
    },

    createDataSources: function () {
        this.createDocumentDataSource();
    },

    createKendoWidgets: function () {
        $($("#" + DeleteDocument.btnCancelId)).on("click", function () {
            DeleteDocument.goToLibrary();
        });

        $($("#" + DeleteDocument.btnDeleteId)).on("click", function () {
            DeleteDocument.deleteDocument();
        });

        $($("#" + DeleteDocument.btnUnsubscribeId)).on("click", function () {
            DeleteDocument.unsubscribeDocument();
        });

        this.createGridDocumentDetail();
    },

    createIconViewFile: function () {
        if (this.icon !== "unknown_opt.png") {
            var html = kendo.format("<div align='center'><a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenFile(\"{0}\")' target='_blank' style='cursor: pointer;'>", this.id);
            html += kendo.format("<img src='../../Images/{0}' width='25px'></a></div>", this.icon);

            $("#" + this.iconViewFileId).html(html);
        }
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
                        id: this.id,
                        documentParentId: this.documentParentId
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
                    width: 90,
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
                    field: "Url",
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
            dataSource: this.documentDetailDataSource,
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
        kendo.bind($("#" + this.gridDocumentDetailId), this);

        $("#" + this.gridDocumentDetail).on("dblclick", "tr", function () {
            //var grid = $("#" + DeleteDocument.gridDocumentDetail).data("kendoGrid");
            //var model = grid.dataItem(this);
        });
    },

    goToLibrary: function () {
        var params = {
            url: "/Document/Documents"
        };
        GeneralData.goToActionController(params);
    },

    deleteDocument: function () {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Biblioteca",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> el Documento?",
            actions: [
                { text: "Cancelar", primary: true },
                {
                    text: "Aceptar", action: function () {
                        var params = {
                            url: "/Document/DeleteTotalDocument",
                            data: {
                                id: DeleteDocument.id
                            }
                        };
                        GeneralData.goToActionController(params);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();      
    },

    unsubscribeDocument: function () {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Biblioteca",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Baja</strong> el Documento?",
            actions: [
                { text: "Cancelar", primary: true },
                {
                    text: "Aceptar", action: function () {
                        var params = {
                            url: "/Document/UnsubscribeDocument",
                            data: {
                                id: DeleteDocument.id
                            }
                        };
                        GeneralData.goToActionController(params);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();      
    },

    goToDeleteDocument: function () {
        var params = {
            url: "/Document/DeleteDocument",
            data: {
                id: this.id
            }
        };
        GeneralData.goToActionController(params);
    },


    getUrlFileName: function (libraryArea, fileName, icon) {
        var html = kendo.format("<div align='center'><a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenFile(\"{0}\")' target='_blank' style='cursor: pointer;'>", this.id);
        html += kendo.format("<img src='../../Images/{0}' width='25px'></a></div>", icon);

        return html;
    }

});