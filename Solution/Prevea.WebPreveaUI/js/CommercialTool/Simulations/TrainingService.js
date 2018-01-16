var TrainingService = kendo.observable({

    textAreaObservationsId: "textAreaObservationsTrainingService",
    btnValidateId: "btnValidateTrainingService",
    btnSelectCourseId: "btnSelectCourse",
    btnCreateCourseId: "btnCreateCourse",
    confirmId: "confirm",
    gridTrainingCourseTrainingServiceId: "gridTrainingCourseTrainingService",
    chooseCourseId: "chooseCourse",
    chooseCourseWindow: null,

    trainingCourseTrainingServiceDataSource: null,

    selectedCourse: null,

    simulationId: null,
    numberEmployees: null,

    init: function (id, numberEmployees) {
        this.simulationId = id;
        this.numberEmployees = numberEmployees;
        this.selectedCourse = null;        

        this.createTrainingCourseTrainingServiceDataSource();
        this.createTrainingCourseTrainingServiceGrid();

        this.setUpWidgets();

        this.blockFields();
    },

    createTrainingCourseTrainingServiceDataSource: function () {
        var that = this;
        this.trainingCourseTrainingServiceDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        AssistantsNumber: { type: "number", defaultValue: 1, validation: { required: { message: " Campo Obligatorio " } } },
                        Price: { type: "number", validation: { required: { message: " Campo Obligatorio " } } },
                        Total: { type: "number", validation: { required: { message: " Campo Obligatorio " } } },
                        TrainingCourseId: { type: "number" },
                        TrainingCourseName: { type: "string", editable: false },
                        TrainingServiceId: { type: "int", defaultValue: that.simulationId }
                    }
                }
            },
            transport: {
                read: {
                    url: "/Simulations/Courses_Read",
                    dataType: "jsonp",
                    data: { trainingServiceId: this.simulationId }
                },
                update: {
                    url: "/Simulations/Courses_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/Simulations/Courses_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/Simulations/Courses_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { trainingServiceId: options.trainingServiceId };
                    }
                    if (operation !== "read" && options) {
                        return { course: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "update" || e.type === "destroy" || e.type === "create") &&
                    e.response !== null) {
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(Constants.ko, "", "error");
                        this.cancelChanges();
                    } else {
                        $("#" + TrainingService.btnValidateId).removeAttr("disabled");
                        $("#" + TrainingService.btnValidateId).prop("disabled", false);
                        GeneralData.showNotification(Constants.ok, "", "success");

                        TrainingService.updateButtons();
                    }

                    $("#" + TrainingService.btnCreateCourseId).removeAttr("disabled");
                    $("#" + TrainingService.btnCreateCourseId).prop("disabled", false);
                }
            },
            change: function (e) {
                if (e.action != null && e.action === "itemchange") {
                    var dataItem;
                    var value;
                    if (e.field === "AssistantsNumber") {
                        dataItem = e.items[0];
                        value = dataItem.AssistantsNumber * dataItem.Price;
                        dataItem.set("Total", value);
                    }
                    if (e.field === "Total") {
                        dataItem = e.items[0];
                        value = dataItem.Total / dataItem.AssistantsNumber;
                        dataItem.set("Price", value);
                    }

                }
            },
            aggregate: [ { field: "Total", aggregate: "sum" }],
            pageSize: 10
        });
    },

    createTrainingCourseTrainingServiceGrid: function () {
        $("#" + this.gridTrainingCourseTrainingServiceId).kendoGrid({
            columns: [{
                field: "TrainingCourseName",
                title: "Nombre del Curso",
                template: "#= TrainingService.getColumnTemplateNameCourse(data.TrainingCourseName) #"
            }, {
                field: "AssistantsNumber",
                title: "Asistentes",
                width: 130,
                groupable: "false",
                template: "#= Templates.getColumnTemplateRight(data.AssistantsNumber) #"
            }, {
                field: "Price",
                title: "Precio",
                width: 100,
                groupable: "false",
                format: "{0:c}",
                template: "#= Templates.getColumnTemplateCurrencyRight(data.Price, 'c0') #"
            }, {
                field: "Total",
                title: "Total",
                width: 120,
                groupable: "false",
                format: "{0:c}",
                template: "#= Templates.getColumnTemplateCurrencyRight(data.Total, 'c0') #",
                aggregates: ["sum"],
                footerTemplate: "Suma: #= kendo.toString(sum, 'c0') #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= TrainingService.getColumnTemplateCommands(data) #"
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
            dataSource: this.trainingCourseTrainingServiceDataSource,
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
            groupable: false,
            edit: function (e) {
                $("#" + TrainingService.btnCreateCourseId).removeAttr("disabled");
                $("#" + TrainingService.btnCreateCourseId).prop("disabled", true);

                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;' onclick='TrainingService.cancelEdit()'></i></a>";
                html += "</div>";

                commandCell.html(html);

                if (e.model.isNew() && !e.model.dirty) {
                    e.model.set("TrainingCourseId", TrainingService.selectedCourse.Id);
                    e.model.set("Price", TrainingService.selectedCourse.Price);
                    e.model.set("Total", TrainingService.selectedCourse.Price);

                    html = TrainingService.getColumnTemplateNameCourse(TrainingService.selectedCourse.Name);
                    commandCell = e.container.find("td:first");
                    commandCell.html(html);
                }
            }            
        });
        kendo.bind($("#" + this.gridTrainingCourseTrainingServiceId), this);
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' id='createCourse'>";
        html += "<a id='btnCreateCourse' class='btn btn-prevea' role='button' onclick='TrainingService.addCourseFromWindow()'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='TrainingService.goToEditCourse(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='TrainingService.goToDeleteCourse(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    getColumnTemplateNameCourse: function (name) {
        var html = kendo.format("<div style='text-align: left; font-size: 14px; font-weight: bold;'>{0}</div>", name);

        return html;
    },

    setUpWidgets: function() {
        $("#" + this.textAreaObservationsId).change(function () {
            TrainingService.updateButtons();
        });

        this.chooseCourseWindow = $("#" + this.chooseCourseId);
        this.chooseCourseWindow.kendoWindow({
            width: "600px",
            title: "Elegir Curso",
            visible: false,
            modal: true,
            actions: [
                "Close"
            ],
            content: "/Simulations/ChooseCourse",
            close: ChooseCourse.onCloseChooseCourseWindow,
            open: ChooseCourse.onOpenChooseCourse
        });
    },

    goToTrainingService: function () {
        var params = {
            url: "/CommercialTool/Simulations/DetailSimulation",
            data: {
                simulationId: this.simulationId,
                selectTabId: 2
            }
        };
        GeneralData.goToActionController(params);
    },

    goToEditCourse: function (courseId) {
        var grid = $("#" + TrainingService.gridTrainingCourseTrainingServiceId).data("kendoGrid");
        var item = grid.dataSource.get(courseId);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteCourse: function (trainingCourseTrainingServiceId) {
        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Simulaciones</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Curso?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function () {
                        var grid = $("#" + TrainingService.gridTrainingCourseTrainingServiceId).data("kendoGrid");
                        var item = grid.dataSource.get(trainingCourseTrainingServiceId);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    onSuccessUpdate: function (data) {
        if (data.Status === 0) {
            GeneralData.showNotification(Constants.ok, "", "success");
        }
        if (data.Status === 1) {
            GeneralData.showNotification(Constants.ko, "", "error");
        }

        TrainingService.goToTrainingService();
    },

    addCourseFromWindow: function () {
        this.selectedCourse = null;
        this.chooseCourseWindow.data("kendoWindow").center().open();
    },

    selectCourse: function(nodeSelected) {
        this.selectedCourse = nodeSelected;

        var grid = $("#" + TrainingService.gridTrainingCourseTrainingServiceId).data("kendoGrid");
        grid.addRow();
    },

    cancelEdit: function() {
        if (DetailSimulation.simulationStateId !== Constants.simulationState.SendToCompany) {
            $("#" + TrainingService.btnCreateCourseId).removeAttr("disabled");
            $("#" + TrainingService.btnCreateCourseId).prop("disabled", false);
        }
    },

    updateButtons: function() {
        $("#" + DetailSimulation.btnSendToCompaniesId).removeAttr("disabled");
        $("#" + DetailSimulation.btnSendToCompaniesId).prop("disabled", true);

        DetailSimulation.simulationStateId = Constants.simulationState.ValidationPending;
        DetailSimulation.createIconSimulationState();
    },

    blockFields: function () {
        if (DetailSimulation.simulationStateId === Constants.simulationState.SendToCompany) {
            var grid = $("#" + this.gridTrainingCourseTrainingServiceId).data("kendoGrid");
            grid.hideColumn("Commands");

            $("#" + TrainingService.btnCreateCourseId).removeAttr("disabled");
            $("#" + TrainingService.btnCreateCourseId).prop("disabled", true);

            $("#" + TrainingService.textAreaObservationsId).removeAttr("disabled");
            $("#" + TrainingService.textAreaObservationsId).prop("disabled", true);

            $("#" + TrainingService.btnValidateId).removeAttr("disabled");
            $("#" + TrainingService.btnValidateId).prop("disabled", true);
        }
    }
});