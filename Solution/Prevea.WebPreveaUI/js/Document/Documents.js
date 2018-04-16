var Documents = kendo.observable({
    isRoleLibrary: false,

    gridDocumentsId: "gridDocuments",
    btnAddDocumentId: "btnAddDocument",
    btnShowAllDocumentsId: "btnShowAllDocuments",
    confirmId: "confirm",
    
    documentsDataSource: null,

    init: function (isRoleLibrary) {
        if (isRoleLibrary) {
            this.isRoleLibrary = (isRoleLibrary.toLowerCase() === "true");
        }

        this.createDocumentsDataSource();
        this.createGridDocuments();
        this.setUpWidgets();
    },

    createGridDocuments: function () {
        $("#" + this.gridDocumentsId).kendoGrid({
            columns: [{
                field: "AreaName",
                title: "Area",
                width: 80
            }, {
                field: "Name",
                title: "Matrícula",
                width: 130,
                groupable: "false"
            }, {
                field: "Description",
                title: "Descripción",
                groupable: "false"
            }, {
                field: "Date",
                title: "Fecha Publicación",
                width: 180,
                template: "#= Documents.getColumnTemplateDateInitial(data) #"
            }, {
                field: "Edition",
                title: "Edición",
                width: 100,
                groupable: "false",
                template: "#= Templates.getColumnTemplateEdition(data) #"
            }, {
                field: "DocumentUserCreatorName",
                title: "Control",
                width: 100
            }, {
                field: "UrlRelative",
                title: "Documento",
                width: 115,
                groupable: "false",
                filterable: false,
                template: "#= Templates.getColumnTemplateUrl(data) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= Documents.getColumnTemplateCommands(data) #"
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
            dataSource: this.documentsDataSource,
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
        kendo.bind($("#" + this.gridDocumentsId), this);
    },

    createDocumentsDataSource: function () {
        this.documentsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        AreaName: { type: "string" },
                        Name: { type: "string" },
                        Description: { type: "string" },
                        Date: { type: "date" },
                        DateModification: { type: "date" },
                        Edition: { type: "number" },
                        DocumentUserCreatorName: { type: "string" },
                        UrlRelative: { type: "string" },
                        DocumentStateId: { type: "bool" }
                    }
                }
            },
            transport: {
                read: {                    
                    url: "/Document/Documents_Read",
                    dataType: "jsonp",
                    data: { documentStateId: 0 }
                }
            },
            pageSize: 10
        });
    },

    setUpWidgets: function() {
        var grid = $("#" + this.gridDocumentsId).data("kendoGrid");
        if (!this.isRoleLibrary) {
            grid.hideColumn("Commands");
        } else {
            grid.showColumn("Commands");
        }
        if (!this.isRoleLibrary) {
            $("#" + this.btnAddDocumentId).hide();
            $("#" + this.btnShowAllDocumentsId).hide();
        }
    },

    getColumnTemplateDateInitial: function (data) {
        var html = kendo.format("<div class='one-line'><div>Inicial: </div><div>{0}</div></div><div class='one-line'><div>Modificación: </div><div>{1}</div></div>",
            kendo.toString(data.Date, "dd/MM/yy"),
            kendo.toString(data.DateModification, "dd/MM/yy"));

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='right'>";
        if (data.DocumentStateId === 3) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='Documents.goToSubscribeDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }        
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Documents.goToEditDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Histórico' onclick='Documents.goToDetailDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Documents.goToDeleteDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToDetailDocument: function (id) {
        var params = {
            url: "/Document/DetailDocument",
            data: {
                id: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToSubscribeDocument: function (id) {        
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "Biblioteca",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Alta</strong> el Documento?",
            actions: [
                { text: "Cancelar", primary: true },
                { text: "Aceptar", action: function() {
                    var params = {
                        url: "/Document/SubscribeDocument",
                        data: {
                            id: id
                        }
                    };
                    GeneralData.goToActionController(params);                    
                } }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    confirmSubscribeDocument: function (content){
        return $("<div></div>").kendoConfirm({
            title: "Biblioteca",
            content: content
        }).data("kendoConfirm").open().result;
    },

    goToEditDocument: function (id) {
        var params = {
            url: "/Document/EditDocument",
            data: {
                id: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteDocument: function (id) {
        var params = {
            url: "/Document/DeleteDocument",
            data: {
                id: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToAddDocument: function() {
        var params = {
            url: "/Document/AddDocument",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    showAllDocuments: function () {
        var data = { documentStateId: 3 };
        this.documentsDataSource.read(data);
    }

});
