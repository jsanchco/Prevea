var WorkStations = kendo.observable({

    cnaeSelected: null,

    gridCnaesId: "gridCnaes",
    confirmId: "confirm",
    cnaesDataSource: null,

    init: function (cnaeSelected) {
        kendo.culture("es-ES");

        if (typeof cnaeSelected !== "undefined") {
            this.cnaeSelected = cnaeSelected;
        } else {
            this.cnaeSelected = null;
        }     

        this.createCnaesDataSource();
        this.createCnaesGrid();
    },

    goToWorkStations: function () {
        var params = {
            url: "/Tecniques/WorkStations",
            data: {
                cnaeSelected: null
            }
        };
        GeneralData.goToActionController(params);
    },

    createCnaesDataSource: function () {
        this.cnaesDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        CustomKey: { type: "string" },
                        Name: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/Cnaes_Read",
                    dataType: "jsonp"
                }
            }
        });
    },

    createCnaesGrid: function () {
        $("#" + this.gridCnaesId).kendoGrid({
            columns: [
                {
                    field: "CustomKey",
                    title: "Código",
                    width: 150,
                    template: "#= Templates.getColumnTemplateIncrease(data.CustomKey) #"
                },
                {
                    field: "Name",
                    title: "Nombre",
                    template: "#= Templates.getColumnTemplateIncrease(data.Name) #"
                }
            ],
            pageable: {
                //buttonCount: 2,
                //pageSizes: [10, 20, "all"],
                refresh: true,
                messages: {
                    display: "Elementos mostrados {2}",
                    //itemsPerPage: "Elementos por página",
                    //allPages: "Todos",
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
            dataSource: this.cnaesDataSource,
            detailTemplate: this.getTemplateChildren(),
            detailInit: this.childrenWorkStations,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            dataBound: function () {
                var grid = this;
                grid.tbody.find(">tr").each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.Id === WorkStations.cnaeSelected) {
                        var select = grid.tbody.find('tr[data-uid="' + dataItem.uid + '"]');
                        grid.expandRow(select);
                        WorkStations.cnaeSelected = null;
                    }
                });
            }
        });
    },

    getTemplateChildren: function () {
        var html = "<div id='templateGridWorkStations' class='templateChildren'>";
        html += "<H2 style='text-align: center;'>Puestos de Trabajo</H2><br />";
        html += "<div id='gridWorkStations' class='gridWorkStations' style='margin: 5px;'></div><br /><br />";
        html += "</div>";

        return html;
    },

    childrenWorkStations: function (e) {
        var dataSourceChildren = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Name: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Description: { type: "string" },
                        CnaeId: { type: "number", defaultValue: e.data.Id }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/WorkStations_Read",
                    dataType: "jsonp",
                    data: {
                        cnaeId: e.data.Id
                    }
                },
                create: {
                    url: "/Tecniques/WorkStations_Create",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Tecniques/WorkStations_Destroy",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Tecniques/WorkStations_Update",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read" && options) {
                        return { cnaeId: kendo.stringify(options.cnaeId) };
                    }

                    if (operation !== "read" && options) {
                        return { workStation: kendo.stringify(options) };
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
            kendo.format("{0}gridWorkStations", e.data.id);
        detailRow.find(".gridWorkStations").addClass(classGridDetail);

        detailRow.find(".gridWorkStations").kendoGrid({
            columns: [
                {
                    field: "Name",
                    title: "Puesto de Trabajo",
                    width: 200,
                    template: "#= Templates.getColumnTemplateBold(data.Name) #"
                },
                {
                    field: "ProfessionalCategory",
                    title: "Categoría Profesional",
                    width: 230
                },
                {
                    field: "Description",
                    title: "Descripción"
                },
                {
                    title: "Comandos",
                    width: 120,
                    groupable: "false",
                    filterable: false,                    
                    template: "#= WorkStations.getColumnTemplateWorkStationsCommands(data) #"
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
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false,
            toolbar: WorkStations.getTemplateWorkStationsToolBar(),
            editable: {
                mode: "inline",
                confirmation: false
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

        var grid = detailRow.find(".gridWorkStations").data("kendoGrid");
        grid.setDataSource(dataSourceChildren);
    },

    getTemplateWorkStationsToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createWorkStation'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateWorkStationsCommands: function (data) {
        var html = "<div align='center'>";

        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='WorkStations.goToEditWorkStation(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, data.CnaeId);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='WorkStations.goToDeleteWorkStation(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, data.CnaeId);
        html += kendo.format("<a toggle='tooltip' title='Evaluación de Riesgos' onclick='WorkStations.goToRiskEvaluation(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='fa fa-flash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, data.CnaeId);

        html += kendo.format("</div>");

        return html;
    },

    goToEditWorkStation: function (id, cnaeId) {
        var grid = $("#gridWorkStations").find(cnaeId + "gridWorkStations").prevObject.data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteWorkStation: function (id, cnaeId) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Técnicas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Puesto de Trabajo?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#gridWorkStations").find(cnaeId + "gridWorkStations").prevObject.data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToRiskEvaluation: function(id, cnaeId) {
        var params = {
            url: "/Tecniques/RiskEvaluation",
            data: {
                cnaeId: cnaeId,
                workStationId: id
            }
        };
        GeneralData.goToActionController(params);
    }
});