var Library = kendo.observable({

    gridLibraryId: "gridLibrary",

    init: function() {
        this.createGridLibrary();
    },

    createGridLibrary: function () {
        $("#" + this.gridLibraryId).kendoGrid({
            columns: [{
                field: "LibraryAreaName",
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
                field: "DateInitial",
                title: "Fecha Publicación",
                width: 160,
                template: "#= Library.getColumnTemplateDateInitial(data) #"
            }, {
                field: "Edition",
                title: "Edición",
                width: 90,
                groupable: "false",
                template: "#= Library.getColumnTemplateEdition(data) #"
            }, {
                field: "UserName",
                title: "Control",
                width: 90
            }, {
                field: "Url",
                title: "Documento",
                width: 115,
                groupable: "false",
                filterable: false,
                template: "#= Library.getColumnTemplateUrl(data) #"
            }, {
                title: "Comandos",
                width: 110,
                groupable: "false",
                filterable: false,
                template: "#= Library.getColumnTemplateCommands(data) #"
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
            var grid = $("#" + Library.gridLibraryId).data("kendoGrid");
            var model = grid.dataItem(this);

            //OpenDetailLibrary(model.Id, model.ParentId);

        });
    },

    OpenDetailLibrary: function (id, parentId) {
        var url = "@Url.Action('DetailDocument', 'Library', new {id = '__param__', parentId = '__param1__'})";
        url = url.replace("__param__", encodeURIComponent(id));
        url = url.replace("__param1__", encodeURIComponent(parentId));
        window.location.href = url;
    },

    getColumnTemplateDateInitial: function (data) {
        var html = kendo.format("<div class='one-line'><div>Inicial: </div><div>{0}</div></div><div class='one-line'><div>Modificación: </div><div>{1}</div></div>",
            kendo.toString(data.DateInitial, "dd/MM/yy"),
            kendo.toString(data.DateModication, "dd/MM/yy"));

        return html;
    },

    getColumnTemplateEdition: function (data) {
        var html = kendo.format("<div align='right'>{0}</div>",
            data.Edition);

        return html;
    },
    
    getColumnTemplateUrl: function (data) {
        var html = kendo.format("<div align='center'><a toggle='tooltip' title='Abrir Documento' onclick='Library.goToOpenFile(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'>", data.LibraryArea, data.FileName);
        html += kendo.format("<img src='../../Images/{0}'></a></div>", data.Icon);

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Library.goToEditDocument(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, "Edit");
        html += kendo.format("<a toggle='tooltip' title='Histórico' onclick='Library.goToDetailDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Library.goToEditDocument(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id, "Delete");
        html += kendo.format("</div>");

        return html;
    },

    goToOpenFile: function (libraryArea, fileName) {
        var url = "/Library/DownloadFile?directory=__param__&filename=__param1__";
        url = url.replace("__param__", encodeURIComponent(libraryArea));
        url = url.replace("__param1__", encodeURIComponent(fileName));

        window.location = url;
    },

    goToDetailDocument: function (id) {
        var params = {
            url: "/Library/DetailDocument",
            data: {
                id: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToEditDocument: function (id, typeEdit) {
        var params = {
            url: "/Library/EditDocument",
            data: {
                id: id,
                typeEdit: typeEdit
            }
        };
        GeneralData.goToActionController(params);
    }

});

