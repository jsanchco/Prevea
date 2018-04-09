var DocumentsMedicalExamination = kendo.observable({

    id: null,

    gridDocumentsMedicalExaminationId: "gridDocumentsMedicalExamination",

    documentsMedicalExaminationDataSource: null,

    init: function (id) {
        kendo.culture("es-ES");

        this.id = id;

        this.setUpPage();
    },

    setUpPage: function () {
        this.createDocumentsMedicalExaminationDataSource();
        this.createDocumentsMedicalExaminationGrid();
    },

    createDocumentsMedicalExaminationDataSource: function () {
        this.documentsMedicalExaminationDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Enrollment: { type: "string" },
                        Url: { type: "string" },
                        MedicalExaminationDocumentTypeId: { type: "number", defaultValue: 1 },
                        MedicalExaminationDocumentTypeDescription: { type: "string" },
                        RequestMedicalExaminationEmployeeId: { type: "number" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/MedicalExamination/DocumentsMedicalExamination_Read",
                    dataType: "jsonp"
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
                    if (operation !== "read" && options) {
                        return { medicalExaminationDocument: kendo.stringify(options) };
                    }

                    return null;
                }
            },
            requestEnd: function (e) {
                if ((e.type === "destroy" || e.type === "create") &&
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

    createDocumentsMedicalExaminationGrid: function () {
        $("#" + this.gridDocumentsMedicalExaminationId).kendoGrid({
            columns: [{
                field: "Enrollment",
                title: "Nombre",
                template: "#= Templates.getColumnTemplateIncrease(data.Enrollment) #"
            }, {
                field: "MedicalExaminationDocumentTypeId",
                title: "Tipo de Documento",
                width: 90,
                editor: DocumentsMedicalExamination.establishmentTypesDropDownEditor,
                template: "#= DocumentsMedicalExamination.getMedicalExaminationDocumentTypeDescription(data.MedicalExaminationDocumentTypeId) #",
                groupHeaderTemplate: "Agrupado : #= DocumentsMedicalExamination.getMedicalExaminationDocumentTypeDescription(value) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= Doctors.getColumnTemplateCommands(data) #"
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
    }
});