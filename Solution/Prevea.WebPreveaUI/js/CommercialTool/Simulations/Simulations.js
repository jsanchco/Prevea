var Simulations = kendo.observable({
    gridSimulationsId: "gridSimulations",
    confirmId: "confirm",
    btnCreateSimulationId: "btnCreateSimulation",
    breadcrumbSimulationsId: "breadcrumbSimulations",

    simulationsDataSource: null,

    init: function() {
        this.createSimulationsDataSource();
        this.createGridSimulations();
    },

    createSimulationsDataSource: function() {
        this.simulationsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        UserId: { type: "number" },
                        Active: { type: "boolean", editable: false, defaultValue: false },
                        UserInitials: { type: "string", editable: false },
                        UserAssignedId: { type: "number", defaultValue: null },
                        UserAssignedInitials: { type: "string", editable: false },
                        Name: { type: "string", defaultValue: "Original" },
                        Original: { type: "boolean", defaultValue: true },
                        SimulationParentId: { type: "number", defaultValue: null },
                        CompanyName: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        CompanyId: { type: "number", editable: false, defaultValue: null },
                        NIF: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        NumberEmployees: {
                            type: "string",
                            validation: { required: { message: " Campo Obligatorio " } }
                        },
                        Date: { type: "date", editable: false },
                        SimulationStateId: {
                            type: "number",
                            editable: false,
                            defaultValue: Constants.simulationState.ValidationPending
                        },
                        SimulationStateName: { type: "string", editable: false, defaultValue: "ValidationPending" },
                        SimulationStateDescription: {
                            type: "string",
                            editable: false,
                            defaultValue: "Pendiente de Validación"
                        }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Simulations/Simulations_Read",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Simulations/Simulations_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Simulations/Simulations_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Simulations/Simulations_Create",
                    dataType: "jsonp"
                },
                parameterMap: function(options, operation) {
                    if (operation !== "read" && options) {
                        return { simulation: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function(e) {
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
            pageSize: 20
        });
    },

    createGridSimulations: function() {
        $("#" + this.gridSimulationsId).kendoGrid({
            columns: [
                {
                    field: "Active",
                    title: "Activa",
                    width: 100,
                    groupable: "true",
                    template: "#= Templates.getColumnTemplateBooleanIncrease(data.Active) #"
                },
                {
                    field: "CompanyName",
                    title: "Razón Social",
                    width: 200,
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateIncrease(data.CompanyName) #"
                }, {
                    field: "NIF",
                    title: "NIF",
                    width: 120,
                    groupable: "false"
                }, {
                    field: "NumberEmployees",
                    title: "Nº de Empleados",
                    width: 160,
                    template: "#= Templates.getColumnTemplateIncreaseRight(data.NumberEmployees) #"
                }, {
                    field: "Date",
                    title: "Fecha",
                    width: 150,
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateDate(data.Date) #"
                }, {
                    title: "Creado por",
                    field: "UserInitials",
                    width: 130
                }, {
                    title: "Asignado a",
                    field: "UserAssignedInitials",
                    width: 130
                }, {
                    title: "Estado",
                    field: "SimulationStateDescription",
                    width: 150,
                    template: "#= Simulations.getTemplateSimulationState(data) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= Simulations.getColumnTemplateCommands('gridSimulations', data) #"
                }
            ],
            pageable: {
                buttonCount: 2,
                pageSizes: [20, 40, "all"],
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
            dataSource: this.simulationsDataSource,
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
            detailTemplate: Simulations.getTemplateChildren(),
            detailInit: Simulations.childrenSimulations,
            groupable: false,
            dataBound: function() {
                var grid = this;
                grid.tbody.find(">tr").each(function() {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.SimulationStateId === Constants.simulationState.ValidationPending ||
                        dataItem.SimulationStateId === Constants.simulationState.Modified ||
                        dataItem.SimulationStateId === Constants.simulationState.Validated) {
                        $(this).find(".k-hierarchy-cell a").removeClass("k-icon");
                    }
                });
            },
            edit: function(e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html +=
                    "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html +=
                    "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }

        });

        var grid = $("#" + this.gridSimulationsId).data("kendoGrid");
        if (GeneralData.userRoleId === Constants.role.PreveaCommercial) {            
            grid.hideColumn("UserInitials");
        }

        if (GeneralData.userRoleId === Constants.role.PreveaPersonal) {
            grid.hideColumn("UserAssignedInitials");
        }

        if (GeneralData.userRoleId === Constants.role.PreveaCommercial) {
            $("#" + Simulations.btnCreateSimulationId).removeAttr("disabled");
            $("#" + Simulations.btnCreateSimulationId).prop("disabled", false);
        } else {
            $("#" + Simulations.btnCreateSimulationId).removeAttr("disabled");
            $("#" + Simulations.btnCreateSimulationId).prop("disabled", true);
        }
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<a id='btnCreateSimulation' class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</div>";

        return html;
    },

    getTemplateSimulationState: function(data) {
        var html = kendo.format("<div style='float: left; text-align: left; display: inline;'>{0}</div>",
            data.SimulationStateDescription);

        if (data.SimulationStateId === Constants.simulationState.ValidationPending) {
            html += kendo.format("<div id='circleError' style='float: right; text-align: right;'></div></div>");
        }
        if (data.SimulationStateId === Constants.simulationState.Modified) {
            html += kendo.format("<div id='circleWarning' style='float: right; text-align: right;'></div></div>");
        }
        if (data.SimulationStateId === Constants.simulationState.Validated) {
            html += kendo.format("<div id='circleSuccess' style='float: right; text-align: right;'></div></div>");
        }
        if (data.SimulationStateId === Constants.simulationState.SendToCompany) {
            html += kendo.format("<div id='circleSuccess' style='float: right; text-align: right;'></div></div>");
        }
        if (data.SimulationStateId === Constants.simulationState.Deleted) {
            html += kendo.format("<div id='circleDeleted' style='float: right; text-align: right;'></div></div>");
        }

        return html;
    },

    getColumnTemplateCommands: function(gridId, data) {
        var html = "<div align='center'>";
        if (data.SimulationStateId === Constants.simulationState.SendToCompany) {
            html += kendo.format(
                "<a toggle='tooltip' title='Ir a Empresa' onclick='Simulations.goToCompanyFromSimulation(\"{0}\", true)' target='_blank' style='cursor: pointer;'><i class='fa fa-share-square' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                data.CompanyId);
            html += kendo.format(
                "<a toggle='tooltip' title='Detalle' onclick='Simulations.goToDetailSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                data.Id);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Simulations.goToEditSimulation(\"{0}\", \"{1}\", \"{2}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                gridId,
                data.Id,
                data.SimulationParentId);

            if (GeneralData.userRoleId === Constants.role.PreveaCommercial) {
                html += kendo.format(
                    "<a toggle='tooltip' title='Detalle' onclick='Simulations.goToDetailSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                    data.Id);
                html += kendo.format(
                    "<a toggle='tooltip' title='Borrar' onclick='Simulations.goToDeleteSimulation(\"{0}\", \"{1}\", \"{2}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                    gridId,
                    data.Id,
                    data.SimulationParentId);
            }
            if (GeneralData.userRoleId === Constants.role.Super) {
                //if (GeneralData.userId !== data.UserAssignedId &&
                //    data.SimulationStateId !== Constants.simulationState.Deleted) {
                //    html += kendo.format(
                //        "<a toggle='tooltip' title='Asignar' onclick='Simulations.goToAssignSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='fa fa-hand-o-left' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                //        data.Id);
                //}
                html += kendo.format(
                    "<a toggle='tooltip' title='Detalle' onclick='Simulations.goToDetailSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                    data.Id);
                if (data.SimulationStateId !== Constants.simulationState.Deleted) {
                    html += kendo.format(
                        "<a toggle='tooltip' title='Borrar' onclick='Simulations.goToDeleteSimulation(\"{0}\", \"{1}\", \"{2}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                        gridId,
                        data.Id,
                        data.SimulationParentId);
                }
            }
            if (GeneralData.userRoleId === Constants.role.PreveaPersonal) {
                if (GeneralData.userId === data.UserAssignedId) {
                    html += kendo.format(
                        "<a toggle='tooltip' title='Detalle' onclick='Simulations.goToDetailSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                        data.Id);
                } else {
                    html += kendo.format(
                        "<a toggle='tooltip' title='Asignar' onclick='Simulations.goToAssignSimulation(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='fa fa-hand-o-left' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                        data.Id);
                }
            }
        }
        html += kendo.format("</div>");

        return html;
    },

    goToSimulations: function() {
        var params = {
            url: "/Simulations/Simulations",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToEditSimulation: function (gridId, simulationId, simulationParent) {
        var grid;
        if (gridId === Simulations.gridSimulationsId) {
            grid = $("#" + Simulations.gridSimulationsId).data("kendoGrid");
        } else {
            grid = $("#gridSimulationsChildren").find(simulationParent + "gridSimulationsChildren").prevObject.data("kendoGrid");
        }

        var item = grid.dataSource.get(simulationId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDetailSimulation: function(id) {
        var params = {
            url: "/Simulations/DetailSimulation",
            data: {
                simulationId: id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    goToDeleteSimulation: function (gridId, simulationId, simulationParent) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Simulaciones</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> esta Simulación?",
            actions: [
                {
                    text: "Cancelar",
                    primary: true
                },
                {
                    text: "Borrar",
                    action: function () {
                        var grid;
                        if (gridId === Simulations.gridSimulationsId) {
                            grid = $("#" + Simulations.gridSimulationsId).data("kendoGrid");
                        } else {
                            grid = $("#gridSimulationsChildren").find(simulationParent + "gridSimulationsChildren").prevObject.data("kendoGrid");
                        }

                        var item = grid.dataSource.get(simulationId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToCompanyFromSimulation: function(id) {
        var params = {
            url: "/Companies/DetailCompany",
            data: {
                id: id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    goToAssignSimulation: function(simulationId) {
        $.ajax({
            url: "/Simulations/AssignSimulation",
            data: {
                simulationId: simulationId
            },
            type: "post",
            dataType: "json",
            success: function(data) {
                if (data.result.Status === Constants.resultStatus.Ok) {
                    Simulations.simulationsDataSource.read();
                    GeneralData.showNotification(Constants.ok, "", "success");
                }
                if (data.result.Status === Constants.resultStatus.Error) {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            },
            error: function() {
                GeneralData.showNotification(Constants.ko, "", "error");
            }
        });
    },

    childrenSimulations: function(e) {
        var dataSourceChildren = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        UserId: { type: "number" },
                        Active: { type: "boolean", editable: false, defaultValue: false },
                        UserAssignedId: { type: "number", defaultValue: e.data.UserAssignedId },
                        CompanyId: { type: "number", defaultValue: e.data.CompanyId },
                        Original: { type: "boolean", defaultValue: false },
                        SimulationParentId: { type: "number", defaultValue: e.data.Id },
                        NumberEmployees: { type: "string", defaultValue: e.data.NumberEmployees, validation: { required: { message: " Campo Obligatorio " } } },
                        Date: { type: "date", editable: false },
                        SimulationStateId: { type: "number", editable: false, defaultValue: Constants.simulationState.ValidationPending },
                        SimulationStateName: { type: "string", editable: false, defaultValue: "ValidationPending" },
                        SimulationStateDescription: { type: "string", editable: false, defaultValue: "Pendiente de Validación" },
                        CompanyName: { type: "string", defaultValue: e.data.CompanyName },
                        NIF: { type: "string", defaultValue: e.data.NIF }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Simulations/SimulationsChildren_Read",
                    dataType: "jsonp",
                    data: { simulationParentId: e.data.Id }
                },
                destroy: {
                    url: "/Simulations/Simulations_Destroy",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Simulations/Simulations_Update",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Simulations/Simulations_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { simulationParentId: options.simulationParentId };
                    }
                    if (operation !== "read" && options) {
                        return { simulation: kendo.stringify(options) };
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
            pageSize: 20
        });

        var detailRow = e.detailRow;
        var classGridDetail =
            kendo.format("{0}gridSimulationsChildren", e.data.id);
        detailRow.find(".gridSimulationsChildren").addClass(classGridDetail);

        detailRow.find(".gridSimulationsChildren").kendoGrid({
            columns: [
                {
                    field: "Active",
                    title: "Activa",
                    width: 100,
                    groupable: "true",
                    template: "#= Templates.getColumnTemplateBooleanIncrease(data.Active) #"
                },
                {
                    field: "NumberEmployees",
                    title: "Número de Empleados",
                    width: 200,
                    template: "#= Templates.getColumnTemplateIncreaseRight(data.NumberEmployees) #"
                }, {
                    field: "Date",
                    title: "Fecha",
                    groupable: "false",
                    template: "#= Templates.getColumnTemplateDate(data.Date) #"
                }, {
                    title: "Estado",
                    field: "SimulationStateDescription",
                    template: "#= Simulations.getTemplateSimulationState(data) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 130,
                    groupable: "false",
                    filterable: false,
                    template: "#= Simulations.getColumnTemplateCommands('gridSimulationsChildren', data) #"
                }],
            pageable: {
                buttonCount: 2,
                pageSizes: [20, 40, "all"],
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
            toolbar: Simulations.getTemplateChildrenToolBar(),
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
            groupable: false,
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            }
        });        
        var grid = detailRow.find(".gridSimulationsChildren").data("kendoGrid");
        grid.setDataSource(dataSourceChildren);

        if (GeneralData.userRoleId === Constants.role.PreveaCommercial) {
            $("#btnCreateChildrenSimulation").removeAttr("disabled");
            $("#btnCreateChildrenSimulation").prop("disabled", false);
        } else {
            $("#btnCreateChildrenSimulation").removeAttr("disabled");
            $("#btnCreateChildrenSimulation").prop("disabled", true);
        }

        $("*[id*=templateGridSimulationsChildren]").each(function () {
            $(this).css("border-color", "#BFBFBF");
        });

        //$("#templateGridSimulationsChildren").css("border-color", "#BFBFBF");
    },

    getTemplateChildren: function () {
        var html = "<div id='templateGridSimulationsChildren' style='border: 1px solid; border-radius: 10px;'>";
        html += "<H2 style='text-align: center;'>Simulaciones Dependientes</H2><br />";
        html += "<div id='gridSimulationsChildren' class='gridSimulationsChildren' style='margin: 5px;'></div><br /><br />";
        html += "</div>";

        return html;
    },

    getTemplateChildrenToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' id='createChildrenSimulation'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button' id='btnCreateChildrenSimulation'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    }
});