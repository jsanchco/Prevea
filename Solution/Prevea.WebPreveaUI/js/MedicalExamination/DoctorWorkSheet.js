var DoctorWorkSheet= kendo.observable({

    gridDoctorWorkSheetId: "gridDoctorWorkSheet",
    confirmId: "confirm",

    doctorWorkSheetDataSource: null,

    init: function () {
        kendo.culture("es-ES");

        this.createDoctorWorkSheetDataSource();
        this.createDoctorWorkSheetGrid();
    },

    createDoctorWorkSheetDataSource: function () {
        this.doctorWorkSheetDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    fields: {
                        Date: { type: "date" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/DoctorWorkSheet/DoctorWorkSheet_Read",
                    dataType: "jsonp"
                }
            },
            pageSize: 10
        });
    },

    createDoctorWorkSheetGrid: function () {
        $("#" + this.gridDoctorWorkSheetId).kendoGrid({
            columns: [{
                field: "Date",
                title: "Fecha",
                template: "#= DoctorWorkSheet.getColumnTemplateDate(data) #"
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
            dataSource: this.doctorWorkSheetDataSource,
            //detailTemplate: this.getTemplateChildren(),
            //detailInit: this.childrenMedicalExamination,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            }
        });
        kendo.bind($("#" + this.gridDoctorWorkSheetId), this);
    },

    getColumnTemplateDate: function (data) {
        var html = kendo.format("<div align='center'>{0}</div>",
            kendo.toString(data.Date, "dd/MM/yyyy"));

        return html;
    },

    getTemplateChildren: function () {
        var html = "<div id='templateGridEmployeesMedicalExamination' style='border: 1px solid; border-radius: 10px;'>";
        html += "<H2 style='text-align: center;'>Trabajadores</H2><br />";
        html += "<div id='gridEmployeesMedicalExamination' class='gridEmployeesMedicalExamination' style='margin: 5px;'></div><br /><br />";
        html += "</div>";

        return html;
    },

    goToDoctorWorkSheet: function () {
        var params = {
            url: "/DoctorWorkSheet/DoctorWorkSheet",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    childrenMedicalExamination: function (e) {
        var dataSourceChildren = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        EmployeeId: { type: "number" },
                        EmployeeName: { type: "string", editable: false },
                        Observations: { type: "string" },
                        SamplerNumber: { type: "string", editable: samplerNumberEditableValue },
                        Date: { type: "date", format: "{0:dd/MM/yyyy hh:mm}", editable: dateEditableValue },
                        ChangeDate: { type: "boolean" },
                        Included: { type: "boolean", defaultValue: false, editable: includedEditableValue },
                        RequestMedicalExaminationsId: { type: "number" },
                        ContactPersonId: { type: "number" },
                        NIF: { type: "string", editable: false },
                        ClinicId: { type: "number", editable: clinicEditableValue },
                        ClinicName: { type: "string", editable: clinicEditableValue },
                        Doctors: { type: "string" },
                        SplitDoctors: { editable: doctorsEditableValue }
                    }
                }
            },
            transport: {
                read: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminationEmployees_Read",
                    dataType: "jsonp",
                    data: {
                        requestMedicalExaminationId: e.data.Id,
                        companyId: e.data.CompanyId
                    }
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return {
                            requestMedicalExaminationId: options.requestMedicalExaminationId,
                            companyId: options.companyId
                        };
                    }

                    return null;
                }
            },
            change: function (e) {
                if (e.action != null && e.action === "itemchange") {
                    if (e.field === "Date") {
                        var dataItem = e.items[0];
                        dataItem.set("ChangeDate", true);
                    }
                }
            },
            batch: true,
            pageSize: 20
        });

        var detailRow = e.detailRow;
        detailRow.find(".gridEmployeesMedicalExamination").kendoGrid({
            columns: [
                {
                    field: "EmployeeName",
                    title: "Nombre",
                    width: 200,
                    editor: HistoricMedicalExamination.editorSimple
                }, {
                    field: "EmployeeDNI",
                    title: "DNI",
                    width: 100,
                    editor: HistoricMedicalExamination.editorSimple
                }, {
                    field: "ClinicId",
                    title: "Clínica",
                    width: 150,
                    editor: HistoricMedicalExamination.clinicsDropDownEditor,
                    template: "#= HistoricMedicalExamination.getClinicName(data.ClinicId) #"
                }, {
                    field: "SplitDoctors",
                    title: "Médicos",
                    width: 200,
                    editor: HistoricMedicalExamination.doctorsDropDownEditor,
                    template: "#= HistoricMedicalExamination.getDoctorsName(data.Doctors) #"
                }, {
                    field: "Observations",
                    title: "Observaciones"
                }, {
                    field: "SamplerNumber",
                    title: "Nº Muestra",
                    width: 150
                }, {
                    field: "Date",
                    title: "Día",
                    width: 200,
                    template: "#= HistoricMedicalExamination.getColumnTemplateDate(data) #",
                    editor: HistoricMedicalExamination.editorDate
                }, {
                    title: "Incluido",
                    field: "Included",
                    width: 100,
                    template: "#= Templates.getColumnTemplateBooleanIncrease(data.Included) #",
                    editor: HistoricMedicalExamination.editorIncluded
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
            toolbar: HistoricMedicalExamination.getTemplateChildrenToolBar(),
            editable: true,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            },
            groupable: false
        });
        var grid = detailRow.find(".gridEmployeesMedicalExamination").data("kendoGrid");
        grid.setDataSource(dataSourceChildren);

        if (GeneralData.userRoleId !== Constants.role.ContactPerson) {
            var filter = {
                field: "Included",
                operator: "eq",
                value: true
            };
            grid.dataSource.filter(filter);
            grid.hideColumn("Included");
        }

        $("#templateGridEmployeesMedicalExamination").css("border-color", "#BFBFBF");
    }
});