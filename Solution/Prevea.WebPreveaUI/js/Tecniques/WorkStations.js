var WorkStations = kendo.observable({

    sectorSelected: null,

    gridSectorsId: "gridSectors",
    confirmId: "confirm",
    sectorsDataSource: null,

    init: function (sectorSelected) {
        kendo.culture("es-ES");

        if (typeof sectorSelected !== "undefined") {
            this.sectorSelected = sectorSelected;
        } else {
            this.sectorSelected = null;
        }     

        this.createSectorsDataSource();
        this.createSectorsGrid();
    },

    goToWorkStations: function () {
        var params = {
            url: "/Tecniques/WorkStations",
            data: {
                sectorSelected: null
            }
        };
        GeneralData.goToActionController(params);
    },

    createSectorsDataSource: function () {
        this.sectorsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Name: { type: "string", validation: { required: { message: " Campo Obligatorio " } } },
                        Description: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/Sectors_Read",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Tecniques/Sectors_Create",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Tecniques/Sectors_Destroy",
                    dataType: "jsonp"
                },
                update: {
                    url: "/Tecniques/Sectors_Update",
                    dataType: "jsonp"
                },  
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options) {
                        return { sector: kendo.stringify(options) };
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
                //if (e.type === "read" && e.response != null && WorkStations.sectorSelected != null) {
                //    var grid = $("#" + WorkStations.gridSectorsId).data("kendoGrid");
                //    var data = grid.dataSource.data();
                //    for (var i = 0; i < data.length; i++) {
                //        if (data[i].Id === WorkStations.sectorSelected) {
                //            var select = grid.tbody.find('tr[data-uid="' + data[i].uid + '"]');
                //            grid.expandRow(select);
                //        }
                //    }
                //}
            },
            pageSize: 10
        });
    },

    createSectorsGrid: function () {
        $("#" + this.gridSectorsId).kendoGrid({
            columns: [
                {
                    field: "Name",
                    title: "Nombre",
                    template: "#= Templates.getColumnTemplateIncrease(data.Name) #"
                },
                {
                    field: "Description",
                    title: "Descripcion"
                },
                {
                    title: "Comandos",
                    width: 120,
                    groupable: "false",
                    filterable: false,
                    template: "#= WorkStations.getColumnTemplateSectorsCommands(data) #"
                }
            ],
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
            dataSource: this.sectorsDataSource,
            toolbar: this.getTemplateSectorsToolBar(),
            editable: {
                mode: "inline",
                confirmation: false
            },
            detailTemplate: this.getTemplateChildren(),
            detailInit: this.childrenWorkStations,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            },
            dataBound: function () {
                var grid = this;
                grid.tbody.find(">tr").each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.Id === WorkStations.sectorSelected) {
                        var select = grid.tbody.find('tr[data-uid="' + dataItem.uid + '"]');
                        grid.expandRow(select);
                        WorkStations.sectorSelected = null;
                    }
                });
            }
        });
    },

    getTemplateSectorsToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createSector'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateSectorsCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='WorkStations.goToEditSector(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='WorkStations.goToDeleteSector(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToEditSector: function (id) {
        var grid = $("#" + WorkStations.gridSectorsId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteSector: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Técnicas</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Sector?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + that.gridSectorsId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
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
                        SectorId: { type: "number", defaultValue: e.data.Id }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Tecniques/WorkStations_Read",
                    dataType: "jsonp",
                    data: {
                        sectorId: e.data.Id
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
                        return { sectorId: kendo.stringify(options.sectorId) };
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
                    title: "Nombre",
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

        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='WorkStations.goToEditWorkStation(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, data.SectorId);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='WorkStations.goToDeleteWorkStation(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, data.SectorId);
        html += kendo.format("<a toggle='tooltip' title='Evaluación de Riesgos' onclick='WorkStations.goToRiskEvaluation(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='fa fa-flash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, data.SectorId);

        html += kendo.format("</div>");

        return html;
    },

    goToEditWorkStation: function (id, sectorId) {
        var grid = $("#gridWorkStations").find(sectorId + "gridWorkStations").prevObject.data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteWorkStation: function (id, sectorId) {
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
                        var grid = $("#gridWorkStations").find(sectorId + "gridWorkStations").prevObject.data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToRiskEvaluation: function(id, sectorId) {
        var params = {
            url: "/Tecniques/RiskEvaluation",
            data: {
                sectorId: sectorId,
                workStationId: id
            }
        };
        GeneralData.goToActionController(params);
    }
});