var WorkCentersCompany = kendo.observable({

    gridWorkCentersCompanyId: "gridWorkCentersCompany",
    confirmId: "confirm",

    companyId: null,

    workCentersCompanyDataSource: null,
    establishmentTypeDataSorce: null,

    init: function (companyId) {
        this.companyId = companyId;

        this.createWorkCentersCompanyDataSource();
        this.createEstablishmentTypeDataSource();
        this.createWorkCentersCompanyGrid();
    },

    createWorkCentersCompanyDataSource: function () {
        this.workCentersCompanyDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Address: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Province: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Description: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Location: { type: "string" },
                        PostalCode: { type: "string" },
                        CompanyId: { type: "number", defaultValue: WorkCentersCompany.companyId },
                        EstablishmentTypeId: { type: "number", defaultValue: 1 },
                        EstablishmentTypeName: { type: "string" },
                        WorkCenterStateId: { type: "number", defaultValue: 1 },
                        WorkCenterStateName: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/WorkCentersCompany_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                update: {
                    url: "/Companies/WorkCentersCompany_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Companies/WorkCentersCompany_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Companies/WorkCentersCompany_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
                    if (operation !== "read" && options) {
                        return { workCenter: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "update" || e.type === "destroy" || e.type === "create") &&
                    e.response !== null) {
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(Constants.ko, "", "error");
                        if (e.type === "create") {
                            this.data().remove(this.data().at(0));
                        } else {
                            this.cancelChanges();
                        } 
                    } else {
                        GeneralData.showNotification(Constants.ok, "", "success");
                    }
                }
            },
            pageSize: 10
        });
    },

    createEstablishmentTypeDataSource: function () {
        WorkCentersCompany.establishmentTypeDataSorce = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" },
                        Description: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Companies/GetEstablishmentTypes",
                    dataType: "jsonp"
                }
            }
        });

        WorkCentersCompany.establishmentTypeDataSorce.read();
    },

    createWorkCentersCompanyGrid: function () {
        var that = this;
        $("#" + this.gridWorkCentersCompanyId).kendoGrid({
            columns: [{
                field: "Address",
                title: "Dirección",
                width: 200
            }, {
                field: "Province",
                title: "Provincia",
                width: 120
            }, {
                field: "Location",
                title: "Localidad",
                width: 120
            }, {
                field: "PostalCode",
                title: "Código Postal",
                width: 150
            },{
                field: "Description",
                title: "Descripción",
                width: 200,
                groupable: "false"
            }, {
                field: "EstablishmentTypeId",
                title: "Establecimiento",
                width: 160,
                editor: WorkCentersCompany.establishmentTypesDropDownEditor,
                template: "#= WorkCentersCompany.getEstablishmentTypeDescription(data.EstablishmentTypeId) #",
                groupHeaderTemplate: "Agrupado : #= WorkCentersCompany.getEstablishmentTypeDescription(value) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,                
                template: "#= WorkCentersCompany.getColumnTemplateCommands(data) #"
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
            dataSource: this.workCentersCompanyDataSource,
            toolbar: this.getTemplateToolBar(),
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
            groupable: {
                messages: {
                    empty: "Arrastre un encabezado de columna y póngalo aquí para agrupar por ella"
                }
            },
            dataBound: function (e) {
                var grid = $("#" + that.gridWorkCentersCompanyId).data("kendoGrid");
                var items = e.sender.items();
                items.each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.WorkCenterStateId === Constants.workCenterState.Baja) {
                        this.className = "unSubscribe";
                    }
                });
            },
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });

        if (GeneralData.userRoleId === Constants.role.Super ||
            GeneralData.userRoleId === Constants.role.ContactPerson) {
            $("#addWorkCenter").removeAttr("disabled");
            $("#addWorkCenter").prop("disabled", true);

        }
    },

    establishmentTypesDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Description",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: WorkCentersCompany.establishmentTypeDataSorce
            });
    },

    getEstablishmentTypeDescription: function (establishmentTypeId) {
        if (WorkCentersCompany.establishmentTypeDataSorce.data().length === 0) {
            WorkCentersCompany.establishmentTypeDataSorce.read();
        }
        for (var index = 0; index < WorkCentersCompany.establishmentTypeDataSorce.data().length; index++) {
            if (WorkCentersCompany.establishmentTypeDataSorce.data()[index].Id === establishmentTypeId) {
                return WorkCentersCompany.establishmentTypeDataSorce.data()[index].Description;
            }
        }
        return null;
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' id='createWorkCenter'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button' id='addWorkCenter'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";        
        if (data.WorkCenterStateId === Constants.workCenterState.Baja) {
            html += kendo.format("<a toggle='tooltip' title='Dar de Alta' onclick='WorkCentersCompany.goToSubscribeWorkCentersCompany(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-thumbs-up' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Editar' onclick='WorkCentersCompany.goToEditWorkCentersCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='WorkCentersCompany.goToDeleteWorkCentersCompany(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }        
        html += kendo.format("</div>");

        return html;
    },

    goToEditWorkCentersCompany: function (id) {
        var grid = $("#" + WorkCentersCompany.gridWorkCentersCompanyId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteWorkCentersCompany: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Empresas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Dar de Baja/Borrar</strong> al Centro de Trabajo?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Dar de Baja", action: function () {
                        that.goToSubscribeWorkCentersCompany(id, false);
                    }
                },
                {
                    text: "Borrar", action: function ()
                    {
                        var grid = $("#" + that.gridWorkCentersCompanyId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToSubscribeWorkCentersCompany: function (workCentersId, subscribe) {
        var that = this;

        $.ajax({
            url: "/Companies/WorkCentersCompany_Subscribe",
            type: "post",
            cache: false,
            datatype: "json",
            data: {
                workCenterId: workCentersId,
                subscribe: subscribe
            },
            success: function (result) {
                if (result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                    return;
                }

                var grid = $("#" + that.gridWorkCentersCompanyId).data("kendoGrid");
                var item = grid.dataSource.get(workCentersId);
                var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                if (subscribe) {
                    item.set("WorkCenterStateId", Constants.workCenterState.Alta);
                } else {
                    item.set("WorkCenterStateId", Constants.workCenterState.Baja);
                }
                
                var cellName = "Commands";
                var cellIndex = grid.element.find("th[data-field = '" + cellName + "']").index();
                var cell = tr.find("td:eq(" + cellIndex + ")");
                cell.html(that.getColumnTemplateCommands(item));

                GeneralData.showNotification(Constants.ok, "", "success");

                console.log(result.Message);
            },
            error: function (result) {
                GeneralData.showNotification(Constants.ko, "", "error");

                console.log(result);
            }
        });
    }
});