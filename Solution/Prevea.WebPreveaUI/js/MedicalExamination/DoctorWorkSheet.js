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
            detailTemplate: this.getTemplateChildren(),
            detailInit: this.childrenMedicalExamination,
            resizable: true,
            autoScroll: true,
            selectable: true,
            sortable: {
                mode: "single",
                allowUnsort: false
            }
        });
        kendo.bind($("#" + this.gridDoctorWorkSheetId), this);

        var grid = $("#" + this.gridDoctorWorkSheetId).data("kendoGrid");
        var options = localStorage["kendo-grid-options"];
        if (options) {
            grid.setOptions(JSON.parse(options));
        }
    },

    getColumnTemplateDate: function (data) {
        var total = data.MedicalExaminationPending + data.MedicalExaminationInProcess + data.MedicalExaminationFinished;
        var html = "<div><div class='row'><div class='col-sm-2'>";
        html += kendo.format("<div class='row' style='font-weight: bold; margin: 10px;'>{0}</div>", data.Date.getFullYear());
        html += "<div class='row' style=''>&nbsp;</div><div class='row' style=''>&nbsp;</div></div><div class='col-sm-6'>";
        html += kendo.format("<div class='row' style='color: red; font-size: 10px'>{0}</div>", GeneralData.getDayOfTheWeek(data.Date.getDay()));
        html += kendo.format("<div class='row' style='font-size: 20px; font-weight: bold;'>{0}</div>", data.Date.getDate());
        html += kendo.format("<div class='row' style='font-size: 10px'>{0}</div>", GeneralData.getMonth(data.Date.getMonth()));
        html += "</div><div class='col-sm-1'><div class='row' style=''>Pendientes:</div><div class='row' style=''>En Proceso:</div><div class='row' style=''>Finalizados:</div><div class='row' style='font-weight: bold; color: blue;'>Totales:</div></div><div class='col-sm-1'>";
        html += kendo.format("<div class='row' style=''>{0}</div>", data.MedicalExaminationPending);
        html += kendo.format("<div class='row' style=''>{0}</div>", data.MedicalExaminationInProcess);
        html += kendo.format("<div class='row' style=''>{0}</div>", data.MedicalExaminationFinished);
        html += kendo.format("<div class='row' style='font-weight: bold; color: blue;'>{0}</div>", total );

        return html;
    },

    getTemplateChildren: function () {        
        var html = "<div id='templateGridEmployeesMedicalExamination' class='templateChildren'>";
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
                        Id: { type: "number" },
                        EmployeeId: { type: "number" },
                        EmployeeName: { type: "string", editable: false },
                        Observations: { type: "string" },
                        SamplerNumber: { type: "string" },
                        Date: { type: "date", format: "{hh:mm}", editable: false },
                        RequestMedicalExaminationsId: { type: "number" },
                        MedicalExaminationStateId: { type: "number" },
                        MedicalExaminationStateDescription: { type: "string" },
                        EmployeeDNI: { type: "string", editable: false }
                    }
                }
            },
            transport: {
                read: {
                    url: "/DoctorWorkSheet/RequestMedicalExaminationEmployees_Read",
                    dataType: "jsonp",
                    data: {
                        date: e.data.Date
                    }
                },
                update: {
                    url: "/DoctorWorkSheet/RequestMedicalExaminationEmployees_Update",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return {
                            dateString: kendo.toString(options.date, "yyyy/M/d")
                        };
                    }
                    if (operation !== "read" && options) {
                        return { medicalExamination: kendo.stringify(options.models[0]) };
                    }

                    return null;
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
                }
            },
            batch: true,
            pageSize: 20
        });

        var detailRow = e.detailRow; 
        var classGridDetail = kendo.format("{0}gridEmployeesMedicalExamination", kendo.toString(e.data.Date, "yyyyMMdd"));
        detailRow.find(".gridEmployeesMedicalExamination").addClass(classGridDetail);
               
        detailRow.find(".gridEmployeesMedicalExamination").kendoGrid({
            columns: [
                {
                    field: "EmployeeName",
                    title: "Nombre",
                    width: 200
                }, {
                    field: "EmployeeDNI",
                    title: "DNI",
                    width: 100
                }, {
                    field: "Observations",
                    title: "Observaciones"
                }, {
                    field: "SamplerNumber",
                    title: "Nº Muestra",
                    width: 150,
                    template: "#= Templates.getColumnTemplateIncreaseRight(data.SamplerNumber) #"
                }, {
                    field: "Date",
                    title: "Hora",
                    width: 200,
                    template: "#= Templates.getColumnTemplateDateOnlyHourBold(data.Date) #"
                }, {
                    field: "MedicalExaminationStateId",
                    title: "Estado",
                    width: 150,
                    template: "#= DoctorWorkSheet.getColumnTemplateMedicalExaminationState(data) #"
                }, {
                    title: "Comandos",
                    field: "Commands",
                    width: 120,
                    groupable: "false",
                    filterable: false,
                    template: "#= DoctorWorkSheet.getColumnTemplateCommands(data) #"
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
            edit: function (e) {
                var commandCell = e.container.find("td:last");
                var html = "<div align='center'>";
                html += "<a class='k-grid-update' toggle='tooltip' title='Guardar' style='cursor: pointer;'><i class='glyphicon glyphicon-saved' style='font-size: 18px;'></i></a>&nbsp;&nbsp;";
                html += "<a class='k-grid-cancel' toggle='tooltip' title='Cancelar' style='cursor: pointer;'><i class='glyphicon glyphicon-ban-circle' style='font-size: 18px;'></i></a>";
                html += "</div>";

                commandCell.html(html);
            },
            groupable: false
        });
        
        var grid = detailRow.find(".gridEmployeesMedicalExamination").data("kendoGrid");
        grid.setDataSource(dataSourceChildren);                
    },

    getColumnTemplateMedicalExaminationState: function (data) {
        var html = kendo.format("<div style='float: left; text-align: left; display: inline;'>{0}</div>",
            data.MedicalExaminationStateDescription);

        if (data.MedicalExaminationStateId === Constants.medicalExaminationState.Pending) {
            html += kendo.format("<div id='circleError' style='float: right; text-align: right;'></div></div>");
        }
        if (data.MedicalExaminationStateId === Constants.medicalExaminationState.InProcess) {
            html += kendo.format("<div id='circleWarning' style='float: right; text-align: right;'></div></div>");
        }
        if (data.MedicalExaminationStateId === Constants.medicalExaminationState.Finished) {
            html += kendo.format("<div id='circleSuccess' style='float: right; text-align: right;'></div></div>");
        }

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='DoctorWorkSheet.goToEditMedicalExamination(\"{0}\", \"{1}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id, kendo.toString(data.Date, "yyyyMMdd"));
        html += kendo.format("<a toggle='tooltip' title='Detalle' onclick='DoctorWorkSheet.goToDetailMedicalExamination(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-list' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        html += kendo.format("</div>");

        return html;
    },

    goToEditMedicalExamination: function (id, date) {
        var findClass = kendo.format(".{0}gridEmployeesMedicalExamination", date);
        var grid = $(findClass).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDetailMedicalExamination: function (id) {
        var grid = $("#" + this.gridDoctorWorkSheetId).data("kendoGrid");
        localStorage["kendo-grid-options"] = kendo.stringify(grid.getOptions());

        var params = {
            url: "/MedicalExamination/DetailMedicalExamination",
            data: {
                medicalExaminationId: id
            }
        };
        GeneralData.goToActionController(params);
    }
});