var CorrectiveActions = kendo.observable({    

    gridCorrectiveActionsId: "gridCorrectiveActions",
    confirmId: "confirm",

    riskEvaluationId: null,
    correctiveActionsDataSource: null,
    priorityCorrectiveActionsDataSource: null,

    init: function (riskEvaluationId) {
        kendo.culture("es-ES");

        this.riskEvaluationId = riskEvaluationId;

        this.createCorrectiveActionsDataSource();
        this.createPriorityCorrectiveActionsDataSource();

        this.createCorrectiveActionsGrid();
    },

    createCorrectiveActionsDataSource: function () {
        this.correctiveActionsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Description: { type: "string" },
                        PriorityCorrectiveActionId: { type: "number" },
                        RiskEvaluationId: { type: "number", defaultValue: CorrectiveActions.riskEvaluationId },
                        PriorityCorrectiveActionDescription: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/CorrectiveActions_Read",
                    dataType: "jsonp",
                    data: {
                        riskEvaluationId: this.riskEvaluationId
                    }
                },
                destroy: {
                    url: "/Tecniques/CorrectiveActions_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Tecniques/CorrectiveActions_Create",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Tecniques/CorrectiveActions_Update",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read" && options) {
                        return {
                            riskEvaluationId: options.riskEvaluationId
                        };
                    }

                    if (operation !== "read" && options) {
                        return { correctiveAction: kendo.stringify(options) };
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

    createPriorityCorrectiveActionsDataSource: function () {
        this.priorityCorrectiveActionsDataSource = new kendo.data.DataSource({
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
                    url: "/Tecniques/PriorityCorrectiveActions_Read",
                    dataType: "jsonp"
                }
            }
        });
    },

    createCorrectiveActionsGrid: function () {
        $("#" + this.gridCorrectiveActionsId).kendoGrid({
            columns: [{
                field: "Description",
                title: "Descripción"
            },{
                field: "PriorityCorrectiveActionId",
                title: "Prioridad",
                width: 250,
                editor: CorrectiveActions.priorityCorrectiveActionsDropDownEditor,
                template: "#= Templates.getColumnTemplateIncrease(data.PriorityCorrectiveActionDescription) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= CorrectiveActions.getColumnTemplateCommands(data) #"
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
            dataSource: this.correctiveActionsDataSource,
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
        html += "<span name='create' id='createCorrectiveAction'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='CorrectiveActions.goToEditCorrectiveAction(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='CorrectiveActions.goToDeleteCorrectiveAction(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToEditCorrectiveAction: function (id) {
        var grid = $("#" + CorrectiveActions.gridCorrectiveActionsId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteCorrectiveAction: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Técnicas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> esta Medida Correctiva?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + that.gridCorrectiveActionsId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    priorityCorrectiveActionsDropDownEditor: function (container, options) {
        CorrectiveActions.priorityCorrectiveActionsDataSource.read();

        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: CorrectiveActions.priorityCorrectiveActionsDataSource,
                dataBound: function (e) {
                    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                }
            });
    }

});