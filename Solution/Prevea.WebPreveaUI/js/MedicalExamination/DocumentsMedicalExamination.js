var DocumentsMedicalExamination = kendo.observable({

    id: null,

    confirmId: "confirm",
    gridDocumentsMedicalExaminationId: "gridDocumentsMedicalExamination",

    documentsMedicalExaminationDataSource: null,
    documentMedicalExaminationTypesDataSource: null,

    addDocumentWindow: null,
    addDocumentId: "addDocument",

    init: function (id) {
        kendo.culture("es-ES");

        this.id = id;

        this.setUpPage();
    },

    setUpPage: function () {
        this.createDocumentsMedicalExaminationDataSource();
        this.createDocumentMedicalExaminationTypesDataSource();
        this.createDocumentsMedicalExaminationGrid();
    },

    createDocumentsMedicalExaminationDataSource: function () {
        this.documentsMedicalExaminationDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Enrollment: { type: "string", editable: false },
                        Url: { type: "string" },
                        MedicalExaminationDocumentTypeId: { type: "number" },
                        MedicalExaminationDocumentTypeDescription: { type: "string" },
                        RequestMedicalExaminationEmployeeId: { type: "number", defaultValue: this.id }
                    }
                }
            },
            transport: {
                read: {
                    url: "/MedicalExamination/DocumentsMedicalExamination_Read",
                    dataType: "jsonp",
                    data: { requestMedicalExaminationEmployeeId: this.id }
                },
                destroy: {
                    url: "/MedicalExamination/DocumentsMedicalExamination_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/MedicalExamination/DocumentsMedicalExamination_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { requestMedicalExaminationEmployeeId: options.requestMedicalExaminationEmployeeId };
                    }
                    if (operation !== "read" && options) {
                        return { medicalExaminationDocument: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "destroy" || e.type === "create") && e.response !== null) {
                    var grid = $("#" + DocumentsMedicalExamination.gridDocumentsMedicalExaminationId).data("kendoGrid");
                    if (typeof e.response.Errors !== "undefined") {
                        GeneralData.showNotification(e.response.Errors, "", "error");
                        if (e.type === "create") {
                            this.data().remove(this.data().at(0));

                            kendo.ui.progress(grid.element, false);
                        } else {
                            if (e.type === "destroy") {
                                DocumentsMedicalExamination.documentsMedicalExaminationDataSource.read();
                            }
                            this.cancelChanges();
                        }
                    } else {
                        if (e.type === "create") {
                            kendo.ui.progress(grid.element, false);

                            GeneralData.showNotification(Constants.ok, "", "success");
                            DocumentsMedicalExamination.goToAddDocument(e.response.Id);
                        }
                    }
                }
            },
            pageSize: 10
        });
    },

    createDocumentMedicalExaminationTypesDataSource: function () {
        DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource = new kendo.data.DataSource({
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
                    url: "/MedicalExamination/DocumentMedicalExaminationTypes_Read",
                    dataType: "jsonp"
                }
            }
        });

        DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource.read();
    },

    createDocumentsMedicalExaminationGrid: function () {
        $("#" + this.gridDocumentsMedicalExaminationId).kendoGrid({
            columns: [{
                field: "Enrollment",
                title: "Nombre",
                template: "#= Templates.getColumnTemplateIncrease(data.Enrollment) #"
            }, {
                field: "MedicalExaminationDocumentTypeId",
                title: "Tipo de Documento",
                width: 300,
                editor: DocumentsMedicalExamination.medicalExaminationDocumentTypesDropDownEditor,
                template: "#= DocumentsMedicalExamination.getMedicalExaminationDocumentTypeDescription(data.MedicalExaminationDocumentTypeId) #",
                groupHeaderTemplate: "Agrupado : #= DocumentsMedicalExamination.getMedicalExaminationDocumentTypeDescription(value) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= DocumentsMedicalExamination.getColumnTemplateCommands(data) #"
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
            dataSource: this.documentsMedicalExaminationDataSource,
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

    goToDetailCompany: function () {
        var params = {
            url: "/Companies/DetailCompany",
            data: {
                id: this.id,
                selectTabId: 0
            }
        };
        GeneralData.goToActionController(params);
    },

    medicalExaminationDocumentTypesDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Description",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource
            });
    },

    getMedicalExaminationDocumentTypeDescription: function (medicalExaminationDocumentTypeId) {
        if (DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource.data().length === 0) {
            DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource.read();
        }
        for (var index = 0; index < DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource.data().length; index++) {
            if (DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource.data()[index].Id === medicalExaminationDocumentTypeId) {
                return DocumentsMedicalExamination.documentMedicalExaminationTypesDataSource.data()[index].Description;
            }
        }
        return null;
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createMedicalExaminationDocument'>";
        html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        html += "</span></div>";

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div style='display: inline-block; margin-left: 37px;'>";

        if (data.Url) {
            html += kendo.format("<a toggle='tooltip' title='Abrir Documento' onclick='GeneralData.goToOpenDocumentByUrl(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/pdf_opt.png'></a></div></a>&nbsp;&nbsp;", data.Url);
        } else {
            html += kendo.format("<a toggle='tooltip' title='Agregar Otro Documento' onclick='DocumentsMedicalExamination.goToAddDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><img style='margin-top: -9px;' src='../../Images/unknown_opt.png'></a></div></a>&nbsp;&nbsp;&nbsp;", data.Id);
        }

        html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='DocumentsMedicalExamination.goToDeleteMedicalExaminationDocument(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToDeleteMedicalExaminationDocument: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Reconocimientos Médicos</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> este Documento?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function ()
                    {
                        var grid = $("#" + that.gridDocumentsMedicalExaminationId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    goToAddDocument: function (contractualDocumentId) {
        this.setUpAddDocumentWindow(contractualDocumentId);
        this.addDocumentWindow.data("kendoWindow").center().open();
    },

    setUpAddDocumentWindow: function (medicalExaminationDocumentId) {
        var url = "/MedicalExamination/AddDocument?medicalExaminationDocumentId=" + medicalExaminationDocumentId;
        this.addDocumentWindow = $("#" + this.addDocumentId);
        this.addDocumentWindow.kendoWindow({
            width: "330px",
            title: "Agregar Documento",
            visible: false,
            modal: true,
            actions: [
                "Close"
            ],
            content: url
        });
    },

    updateRow: function (data) {
        var grid = $("#" + this.gridDocumentsMedicalExaminationId).data("kendoGrid");    
        var dataItem = grid.dataSource.get(data.Id);
        dataItem.Url = data.Url;

        grid.refresh();
    }
});