var EditDocument = kendo.observable({
    btnValidateId: "btnValidate",
    btnCancelId: "btnCancel",
    gridLibraryId: "gridLibraryDetail",
    iconViewFileId: "iconViewFile",
    textAreaDescriptionId: "textAreaDescription",
    textAreaObservationsId: "textAreaObservations",
    rowFilesId: "rowFiles",
    rowGridLibraryDetailId: "rowGridLibraryDetail",
    spanNotificationId: "spanNotification",

    // Fields
    id: null,
    parentId: null,    
    typeEdit: null,
    libraryArea: null,
    routeFileDelete: null,
    notification: null,

    // Datasources
    libraryDetailDataSource: null,

    init: function (id, parentId, typeEdit, libraryArea, routeFileDelete, notification) {
        this.id = id;
        if (parentId) {
            this.parentId = parentId;
        } else {
            this.parentId = null;
        }

        if (typeEdit) {
            this.typeEdit = typeEdit;
        }

        if (libraryArea) {
            this.libraryArea = libraryArea;
        }

        if (routeFileDelete) {
            this.routeFileDelete = routeFileDelete;
        } else {
            this.routeFileDelete = null;
        }

        if (notification) {
            this.notification = notification;
        } else {
            this.notification = null;
        }
           
        this.createDataSources();
        this.createKendoWidgets();
        this.createIconViewFile();
    },

    createDataSources: function () {
        this.createLibraryDataSource();
    },

    createKendoWidgets: function () {
        switch (this.typeEdit) {
            case "Edit":
                $("#" + this.btnValidateId).val("Actualizar");
                $("#" + this.textAreaDescriptionId).prop("disabled", false);
                $("#" + this.textAreaObservationsId).prop("disabled", false);
                $("#" + this.rowFilesId).show();
                $("#" + this.rowGridLibraryDetailId).hide();
                
                $("#LibraryAreaId").prop("disabled", false);
                $("#UserId").prop("disabled", false);

                break;
            case "Delete":
                $("#" + this.btnValidateId).val("Borrar");
                $("#" + this.textAreaDescriptionId).prop("disabled", true);
                $("#" + this.textAreaObservationsId).prop("disabled", true);
                $("#" + this.rowFilesId).hide();
                $("#" + this.rowGridLibraryDetailId).show();

                $("#LibraryAreaId").prop("disabled", true);
                $("#UserId").prop("disabled", true);

                break;
        };

        $($("#" + this.btnCancelId)).on("click", function () {
            EditDocument.goToLibrary();
        });

        if (this.notification) {
            $("#" + this.spanNotificationId).kendoNotification().data("kendoNotification").show(this.notification);
        } else {
            $("#" + this.spanNotificationId).hide();
        }        

        this.createGridLibraryDetail();
    },

    createIconViewFile: function () {
        var fileName = $("#FileName").val();
        var icon = $("#Icon").val();

        var html = this.getUrlFileName(this.libraryArea, fileName, icon);
        $("#" + this.iconViewFileId).html(html);
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
                        id: EditDocument.id,
                        parentId: EditDocument.parentId
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
            var grid = $("#" + EditDocument.gridLibraryId).data("kendoGrid");
            var model = grid.dataItem(this);

            //OpenDetailLibrary(model.Id, model.ParentId);

        });
    },

    goToEditDocument: function (e) {
        var params = {
            url: "/Library/EditDocument",
            data: {
                    id: this.id,
                    typeEdit: this.typeEdit,
                    routeFileDelete: this.routeFileDelete
            }
        };
        GeneralData.goToActionController(params);
    },

    goToLibrary: function (e) {
        var params = {
            url: "/Library/Index"
        };
        GeneralData.goToActionController(params);
    },

    getColumnTemplateDate: function (data) {
        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(data.DateModication, "dd/MM/yy"));

        return html;
    },

    getUrlFileName: function (libraryArea, fileName, icon) {
        var html = kendo.format("<div align='center'><a toggle='tooltip' title='Abrir Documento' onclick='EditDocument.goToOpenFile(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'>", libraryArea, fileName);
        html += kendo.format("<img src='../../Images/{0}' width='25px'></a></div>", icon);

        return html;
    },

    goToOpenFile: function (libraryArea, fileName) {
        var url = "/Library/DownloadFile?directory=__param__&filename=__param1__";
        url = url.replace("__param__", encodeURIComponent(libraryArea));
        url = url.replace("__param1__", encodeURIComponent(fileName));

        window.location = url;
    },

    onUpload: function (e) {
        e.data = {
        };
    },

    onSuccess: function (e) {
        if (e.response.status === "Ok") {
            switch (e.response.from) {
            case "SaveFileTmp":

                break;

            case "RemoveFileTmp":

                break;

            default:
                break;
            }

        }
        if (e.response.status === "Error") {
            AddFileDocument.showErrors(new Array(e.response.message));
        }
    },

    onRemove: function (e) {
        e.data = {
        };
    }

});