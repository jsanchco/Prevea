var TemplatePreventivePlans = kendo.observable({

    gridTemplatePreventivePlansId: "gridTemplatePreventivePlans",
    confirmId: "confirm",

    templatePreventivePlansDataSource: null,

    init: function () {
        kendo.culture("es-ES");

        this.createTemplatePreventivePlansDataSource();
        this.createTemplatePreventivePlansGrid();
    },

    createTemplatePreventivePlansDataSource: function () {
        this.templatePreventivePlansDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Name: { type: "string" },
                        CreateDate: { type: "date", defaultValue: new Date() },
                        ModifyDate: { type: "date", defaultValue: new Date() }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/TemplatePreventivePlans_Read",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Tecniques/TemplatePreventivePlans_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Tecniques/TemplatePreventivePlans_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Tecniques/TemplatePreventivePlans_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        options.Template = "";
                        return { templatePreventivePlan: kendo.stringify(options) };
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
            change: function (e) {
                if (e.field === "CompanyId") {

                    $.ajax({
                        url: "/PreventivePlan/GetContract",
                        data: { companyId: e.items[0].CompanyId },
                        type: "post",
                        dataType: "json",
                        success: function (response) {
                            if (response.resultStatus === Constants.resultStatus.Ok) {
                                e.items[0].set("DocumentBeginDate", response.contract.BeginDate);
                                e.items[0].set("DocumentEndDate", response.contract.EndDate);

                                e.items[0].set("DocumentId", response.contract.Id);
                            } else {
                                GeneralData.showNotification(Constants.ko, "", "error");
                            }
                        }
                    });
                }
            },
            pageSize: 10
        });
    },

    createTemplatePreventivePlansGrid: function () {
        $("#" + this.gridTemplatePreventivePlansId).kendoGrid({
            columns: [{
                field: "Name",
                title: "Nombre",
                width: 250,
                template: "#= Templates.getColumnTemplateIncrease(data.Name) #"
            }, {
                field: "CreateDate",
                title: "Fecha de Creación",
                width: 130,
                groupable: "false",
                template: "#= Templates.getColumnTemplateDateWithHour(data.CreateDate) #"
            }, {
                field: "ModifyDate",
                title: "Fecha de Modificación",
                width: 130,
                groupable: "false",
                template: "#= Templates.getColumnTemplateDateWithHour(data.ModifyDate) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= TemplatePreventivePlans.getColumnTemplateCommands(data) #"
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
            dataSource: this.templatePreventivePlansDataSource,
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
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createTemplatePreventivePlan'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='TemplatePreventivePlans.goToDetailTemplatePreventivePlan(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='TemplatePreventivePlans.goToEditTemplatePreventivePlan(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='TemplatePreventivePlans.goToDeleteTemplatePreventivePlan(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToDetailTemplatePreventivePlan: function (id) {
        var params = {
            url: "/Tecniques/DetailTemplatePreventivePlan",
            data: {
                id: id
            }
        };
        GeneralData.goToActionController(params);
    },

    goToEditTemplatePreventivePlan: function (id) {
        var grid = $("#" + TemplatePreventivePlans.gridTemplatePreventivePlansId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteTemplatePreventivePlan: function (id) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Técnicas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> esta Plantilla?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + TemplatePreventivePlans.gridTemplatePreventivePlansId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToTemplatePreventivePlans: function () {
        var params = {
            url: "/Tecniques/TemplatePreventivePlans",
            data: {}
        };
        GeneralData.goToActionController(params);
    }
});