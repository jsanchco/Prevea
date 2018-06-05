var Incidences = kendo.observable({

    gridIncidencesId: "gridIncidences",
    confirmId: "confirm",

    incidencesDataSource: null,
    incidenceStatesDataSorce: null,
    criticalNivelsDataSorce: null,

    init: function () {
        this.createIncidencesDataSource();
        this.createIncidenceStatesDataSorceDataSource();
        this.createCriticalNivelsDataSource();
        this.createIncidencesGrid();
    },

    createIncidencesDataSource: function () {
        this.incidencesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Screen: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Description: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        BeginDate: { type: "date", format: "{0:dd/MM/yyyy}", defaultValue: new Date(), editable: false },
                        EndDate: { type: "date", format: "{0:dd/MM/yyyy}", editable: false, defaultValue: null },
                        UserId: { type: "number", defaultValue: GeneralData.userId },
                        UserInitials: { type: "string", editable: false, defaultValue: GeneralData.userInitials },
                        IncidenceStateId: { type: "number", defaultValue: 1 },
                        IncidenceStateDescription: { type: "string" },
                        CriticalNivelId: { type: "number" },
                        CriticalNivelDescription: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Incidences/Incidences_Read",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Incidences/Incidences_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Incidences/Incidences_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Incidences/Incidences_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { incidence: kendo.stringify(options) };
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

    createIncidenceStatesDataSorceDataSource: function () {
        Incidences.incidenceStatesDataSorce = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Description: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Incidences/GetIncidenceStates",
                    dataType: "jsonp"
                }
            }
        });

        Incidences.incidenceStatesDataSorce.read();
    },

    createCriticalNivelsDataSource: function () {
        Incidences.criticalNivelsDataSorce = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Description: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Incidences/GetCriticalNivels",
                    dataType: "jsonp"
                }
            }
        });

        Incidences.criticalNivelsDataSorce.read();
    },

    createIncidencesGrid: function () {
        $("#" + this.gridIncidencesId).kendoGrid({
            columns: [{
                field: "Screen",
                title: "Pantalla",
                width: 200
            }, {
                field: "Description",
                title: "Descripción",
                width: 300
            }, {
                field: "UserInitials",
                title: "Usuario",
                width: 100
            }, {
                field: "BeginDate",
                title: "Fecha de Creación",
                width: 150,
                template: "#= Templates.getColumnTemplateDateWithHour(data.BeginDate) #"
            }, {
                field: "EndDate",
                title: "Fecha de Cierre",
                width: 150,
                template: "#= Templates.getColumnTemplateDateWithHour(data.EndDate) #"
            }, {
                field: "IncidenceStateId",
                title: "Estado",
                width: 100,
                editor: Incidences.incidenceStateDropDownEditor,
                template: "#= IncidenceStateDescription #",
                groupHeaderTemplate: "Agrupado : #= Incidences.getIncidenceStateDescription(value) #"
            }, {
                field: "CriticalNivelId",
                title: "Criticidad",
                width: 100,
                editor: Incidences.criticalNivelDropDownEditor,
                template: "#= CriticalNivelDescription #",
                groupHeaderTemplate: "Agrupado : #= Incidences.getCriticalNivelDescription(value) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= Incidences.getColumnTemplateCommands(data) #"
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
            dataSource: this.incidencesDataSource,
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
        if (GeneralData.userRoleId !== Constants.role.Super) {
            var grid = $("#" + this.gridIncidencesId).data("kendoGrid");
            grid.hideColumn("UserInitials");
        }
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createIncidence'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";

        switch (data.IncidenceStateId) {
            case 1:
                html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Incidences.goToEditIncidence(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
                html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='Incidences.goToDeleteIncidence(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
                break;

            case 3:
                if (GeneralData.userRoleId === Constants.role.Super) {
                    html += kendo.format("<a toggle='tooltip' title='Editar' onclick='Incidences.goToEditIncidence(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
                }
                break;

            case 2:
            case 4:
                if (GeneralData.userRoleId === Constants.role.Super) {
                    html += kendo.format(
                        "<a toggle='tooltip' title='Editar' onclick='Incidences.goToEditIncidence(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                        data.Id);
                    html += kendo.format(
                        "<a toggle='tooltip' title='Borrar' onclick='Incidences.goToDeleteIncidence(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;",
                        data.Id);
                } 
                break;

        }
        
        html += kendo.format("</div>");

        return html;
    },

    getIncidenceStateDescription: function (incidenceStateId) {
        if (Incidences.incidenceStatesDataSorce.data().length === 0) {
            Incidences.incidenceStatesDataSorce.read();
        }
        for (var index = 0; index < Incidences.incidenceStatesDataSorce.data().length; index++) {
            if (Incidences.incidenceStatesDataSorce.data()[index].Id === incidenceStateId) {
                return Incidences.incidenceStatesDataSorce.data()[index].Name;
            }
        }
        return "";
    },

    getCriticalNivelDescription: function (criticalNivelId) {
        if (Incidences.criticalNivelsDataSorce.data().length === 0) {
            Incidences.criticalNivelsDataSorce.read();
        }
        for (var index = 0; index < Incidences.criticalNivelsDataSorce.data().length; index++) {
            if (Incidences.criticalNivelsDataSorce.data()[index].Id === criticalNivelId) {
                return Incidences.criticalNivelsDataSorce.data()[index].Name;
            }
        }
        return "";
    },

    goToEditIncidence: function (id) {
        var grid = $("#" + Incidences.gridIncidencesId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteIncidence: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Control de Errores</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> la Incidencia?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + that.gridIncidencesId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToIncidences: function () {
        var params = {
            url: "/Incidences/Incidences",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    incidenceStateDropDownEditor: function (container, options) {
        if (GeneralData.userRoleId !== Constants.role.Super) {
            $("<div>" + options.model.IncidenceStateDescription + "</div>").appendTo(container);
            return;
        }

        Incidences.incidenceStatesDataSorce.read();

        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Id",
                dataSource: Incidences.incidenceStatesDataSorce
                //dataBound: function (e) {
                //    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                //}
            });
    },

    criticalNivelDropDownEditor: function (container, options) {
        Incidences.criticalNivelsDataSorce.read();

        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Id",
                optionLabel: "Selecciona ...",
                dataSource: Incidences.criticalNivelsDataSorce
                //dataBound: function (e) {
                //    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                //}
            });
    }
});