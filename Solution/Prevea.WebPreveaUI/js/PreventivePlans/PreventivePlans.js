var PreventivePlans = kendo.observable({

    gridPreventivePlansId: "gridPreventivePlans",
    confirmId: "confirm",

    preventivePlansDataSource: null,
    companiesDataSource: null,

    init: function () {
        kendo.culture("es-ES");

        this.createPreventivePlansDataSource();
        this.createCompaniesDataSource();

        this.createPreventivePlansGrid();
    },

    createPreventivePlansDataSource: function () {
        this.preventivePlansDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        CompanyId: { type: "number" },
                        CompanyName: { type: "string" },
                        DocumentId: { type: "number" },
                        DocumentBeginDate: { type: "date", defaultValue: new Date() },
                        DocumentEndDate: { type: "date", defaultValue: new Date() }
                    }
                }
            },
            transport: {
                read: {
                    url: "/PreventivePlan/PreventivePlans_Read",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/PreventivePlan/PreventivePlans_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/PreventivePlan/PreventivePlans_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { preventivePlan: kendo.stringify(options) };
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
            change: function(e) {                
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

    createCompaniesDataSource: function () {
        this.companiesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Name: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/PreventivePlan/Companies_Read",
                    dataType: "jsonp"
                }
            }
        });
    },

    createPreventivePlansGrid: function () {
        $("#" + this.gridPreventivePlansId).kendoGrid({
            columns: [{
                field: "CompanyId",
                title: "Empresa",
                width: 250,
                editor: PreventivePlans.companiesDropDownEditor,               
                template: "#= Templates.getColumnTemplateIncrease(data.CompanyName) #"
            }, {
                field: "DocumentBeginDate",
                title: "Fecha de Inicio",
                width: 130,
                groupable: "false",
                template: "#= Templates.getColumnTemplateDate(data.DocumentBeginDate) #"
            }, {
                field: "DocumentEndDate",
                title: "Fecha Final",
                width: 130,
                groupable: "false",
                template: "#= Templates.getColumnTemplateDate(data.DocumentEndDate) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= PreventivePlans.getColumnTemplateCommands(data) #"
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
            dataSource: this.preventivePlansDataSource,
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
        kendo.bind($("#" + this.gridPreventivePlansId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createPreventivePlan'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='PreventivePlans.goToDetailPreventivePlan(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='PreventivePlans.goToDeletePreventivePlan(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToDetailPreventivePlan: function (id) {
        var params = {
            url: "/PreventivePlan/DetailPreventivePlan",
            data: {
                id: id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeletePreventivePlan: function (id) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Gestión de Empresas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Plan Preventivo?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + PreventivePlans.gridPreventivePlansId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToPreventivePlans: function () {
        var params = {
            url: "/PreventivePlan/PreventivePlans",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    companiesDropDownEditor: function (container, options) {
        PreventivePlans.companiesDataSource.read();

        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: PreventivePlans.companiesDataSource,
                dataBound: function (e) {
                    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                }
            });
    }
});