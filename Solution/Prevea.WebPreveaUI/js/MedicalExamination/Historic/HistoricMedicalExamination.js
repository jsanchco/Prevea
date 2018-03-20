var HistoricMedicalExamination = kendo.observable({

    gridRequestHistoricMedicalExaminationsId: "gridRequestHistoricMedicalExaminations",
    confirmId: "confirm",

    contactPersonId: null,
    companyId: null,

    historicRequestMedicalExaminationsDataSource: null,
    clinicsDataSorce: null,
    doctorsDataSorce: null,

    init: function (contactPersonId, companyId) {
        kendo.culture("es-ES");

        this.contactPersonId = contactPersonId;
        this.companyId = companyId;

        this.createHistoricRequestMedicalExaminationsDataSource();
        this.createClinicsDataSource();
        this.createDoctorsDataSource();
        this.createHistoricRequestMedicalExaminationsGrid();
    },

    createHistoricRequestMedicalExaminationsDataSource: function () {
        this.historicRequestMedicalExaminationsDataSource = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number", defaultValue: 0 },
                        Date: { type: "date", format: "{0:dd/MM/yyyy}", defaultValue: new Date() },
                        Observations: { type: "string" },
                        RequestMedicalExaminationStateId: { type: "number", defaultValue: Constants.requestMedicalExaminationState.Pending },
                        RequestMedicalExaminationStateDescription: { type: "string", editable: false, defaultValue: "Pendiente" },                        
                        CompanyId: { type: "number", defaultValue: HistoricMedicalExamination.companyId }
                    }
                }
            },
            transport: {
                read: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Read",
                    dataType: "jsonp",
                    data: { companyId: this.companyId }
                },
                update: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Update",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Destroy",
                    dataType: "jsonp"
                },
                create: {
                    url: "/HistoricMedicalExamination/RequestMedicalExaminations_Create",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        return { companyId: options.companyId };
                    }
                    if (operation !== "read" && options) {
                        return { requestMedicalExamination: kendo.stringify(options) };
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

    createHistoricRequestMedicalExaminationsGrid: function () {
        $("#" + this.gridRequestHistoricMedicalExaminationsId).kendoGrid({
            columns: [{
                field: "Date",
                title: "Fecha Posible",
                width: 150,
                template: "#= Templates.getColumnTemplateDateIncrease(data.Date) #"
            }, {
                field: "Observations",
                title: "Observaciones"
            }, {
                field: "RequestMedicalExaminationStateDescription",
                title: "Estado de la Petición",
                width: 200,
                template: "#= HistoricMedicalExamination.getColumnTemplateRequestMedicalState(data) #"
            }, {
                title: "Comandos",
                field: "Commands",
                width: 120,
                groupable: "false",
                filterable: false,
                template: "#= HistoricMedicalExamination.getColumnTemplateCommands(data) #"
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
            dataSource: this.historicRequestMedicalExaminationsDataSource,
            toolbar: this.getTemplateToolBar(),
            editable: {
                mode: "inline",
                confirmation: false
            },
            detailTemplate: this.getTemplateChildren(),
            detailInit: this.childrenEmployees,
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
            },
            dataBound: function() {
                var grid = this;
                grid.tbody.find(">tr").each(function() {
                    var dataItem = grid.dataItem(this);
                    if (dataItem.Id === 0) {
                        $(this).find(".k-hierarchy-cell a").removeClass("k-icon");
                    }
                });
            }
        });
        kendo.bind($("#" + this.gridRequestHistoricMedicalExaminationsId), this);
    },

    getColumnTemplateRequestMedicalState: function (data) {
        var html = kendo.format("<div style='float: left; text-align: left; display: inline;'>{0}</div>",
            data.RequestMedicalExaminationStateDescription);

        if (data.RequestMedicalExaminationStateId === Constants.requestMedicalExaminationState.Pending) {
            html += kendo.format("<div id='circleError' style='float: right; text-align: right;'></div></div>");
        }
        if (data.RequestMedicalExaminationStateId === Constants.requestMedicalExaminationState.Validated) {
            html += kendo.format("<div id='circleSuccess' style='float: right; text-align: right;'></div></div>");
        }

        return html;
    },

    getColumnTemplateCommands: function (data) {
        var html = "<div align='center'>";
        html += kendo.format("<a toggle='tooltip' title='Editar' onclick='HistoricMedicalExamination.goToEditRequestMedicalExamination(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-edit' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);

        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            html += kendo.format("<a toggle='tooltip' title='Borrar' onclick='HistoricMedicalExamination.goToDeleteRequestMedicalExamination(\"{0}\")' target='_blank' style='cursor: pointer;'><i class='glyphicon glyphicon-trash' style='font-size: 18px;'></i></a>&nbsp;&nbsp;", data.Id);
        }
        
        html += kendo.format("</div>");

        return html;
    },

    getTemplateToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<span name='create' class='k-grid-add' id='createRequestHistoricMedicalExamination'>";

        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            html += "<a class='btn btn-prevea k-grid-add' role='button'> Agregar nuevo</a>";
        } else {
            html += "<a class='btn btn-prevea k-grid-add' role='button' disabled> Agregar nuevo</a>";
        }

        html += "</span></div>";

        return html;
    },

    getTemplateChildrenToolBar: function () {
        var html = "<div class='toolbar'>";
        html += "<a class='btn btn-prevea' role='button' onclick='HistoricMedicalExamination.validateRequestHistoricMedicalExaminationEmployees()'> Validar</a>";
        html += "</span></div>";

        return html;
    },

    getTemplateChildren: function () {
        var html = "<div id='templateGridEmployeesMedicalExamination' style='border: 1px solid; border-radius: 10px;'>";
        html += "<H2 style='text-align: center;'>Trabajadores</H2><br />";
        html += "<div id='gridEmployeesMedicalExamination' class='gridEmployeesMedicalExamination' style='margin: 5px;'></div><br /><br />";
        html += "</div>";

        return html;
    },

    getColumnTemplateDate: function(data) {
        if (data.Date === null) {
            return "";
        }

        var html;
        if (data.ChangeDate === true) {
            html = "<div style='text-align: center'>";
            if (data.Included === true) {
                html += "<div style='text-align: left'>";
                html += "<div id='circleFirmValidated' toggle='tooltip' title='Validada' style='float: left; text-align: left;'>";
                html += "</div></div>";
            }
            html += kendo.format("<div style='font-weight: bold;'>{0}</div>", kendo.toString(data.Date, "dd/MM/yyyy HH:mm"));
            html += "</div>";
        } else {
            html = "<div style='text-align: center'>";
            if (data.Included === true) {
                html += "<div style='text-align: left'>";
                html += "<div id='circleFirmPending' toggle='tooltip' title='Pendiente de Validar' style='float: left; text-align: left;'>";
                html += "</div></div>";
            }
            html += kendo.format("<div style='font-weight: bold;'>{0}</div>", kendo.toString(data.Date, "dd/MM/yyyy"));
            html += "</div>";
        }

        return html;
    },

    goToHistoric: function() {
        var params = {
            url: "/HistoricMedicalExamination/HistoricMedicalExamination",
            data: {}
        };
        GeneralData.goToActionController(params);
    },

    goToEditRequestMedicalExamination: function (id) {
        var grid = $("#" + HistoricMedicalExamination.gridRequestHistoricMedicalExaminationsId).data("kendoGrid");
        var item = grid.dataSource.get(id);
        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

        grid.editRow(tr);
    },

    goToDeleteRequestMedicalExamination: function (id) {
        var that = this;

        var dialog = $("#" + this.confirmId);
        dialog.kendoDialog({
            width: "400px",
            title: "<strong>Medicina de la Salud</strong>",
            closable: false,
            modal: true,
            content: "¿Quieres <strong>Borrar</strong> la Petición de Reconocimientos Médicos?",
            actions: [
                {
                    text: "Cancelar", primary: true
                },
                {
                    text: "Borrar", action: function ()
                    {
                        var grid = $("#" + that.gridRequestHistoricMedicalExaminationsId).data("kendoGrid");
                        var item = grid.dataSource.get(id);
                        var tr = $("[data-uid='" + item.uid + "']", grid.tbody);

                        grid.removeRow(tr);
                    }
                }
            ]
        });
        dialog.data("kendoDialog").open();
    },

    childrenEmployees: function (e) {
        var includedEditableValue = true;
        var dateEditableValue = true;
        var clinicEditableValue = true;
        var doctorsEditableValue = true;
        if (GeneralData.userRoleId === Constants.role.ContactPerson) {
            dateEditableValue = false;
            clinicEditableValue = false;
            doctorsEditableValue = false;
        } else {
            includedEditableValue = false;
        }

        var samplerNumberEditableValue = false;
        if (GeneralData.userRoleId === Constants.role.Doctor) {
            samplerNumberEditableValue = true;
        }

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
                        Doctors: { type: "string", editable: doctorsEditableValue }
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
            change: function(e) {
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
                    field: "Doctors",
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

        $("#templateGridEmployeesMedicalExamination").css("border-color","#BFBFBF");
    },

    editorSimple: function (container, options) {
        var grid = $("#gridEmployeesMedicalExamination").data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());
        container.text(selectedItem.get(options.field));
        container.css("text-align", "left");
    },


    editorIncluded: function (container, options) {        
        var $input = $("<input name='Included'/>").appendTo(container);
        $input.kendoMobileSwitch({
            onLabel: "Si",
            offLabel: "No",
            checked: options.model.Included
        });   
    },

    editorDate: function(container, options) {
        var dateString = kendo.toString(options.model.Date, "dd/MM/yyyy hh:mm");
        var $input = $("<input name='Date' value=" + dateString + " />").appendTo(container);
        $input.kendoDateTimePicker({
            dateInput: true
        });
    },

    validateRequestHistoricMedicalExaminationEmployees: function () {
        var grid = $("#gridEmployeesMedicalExamination").data("kendoGrid");
        var changes = $.map(grid.dataSource.data(), function (item) {
            var employee = {
                RequestMedicalExaminationsId: item.RequestMedicalExaminationsId,
                EmployeeId: item.EmployeeId,
                EmployeeName: item.EmployeeName,
                Observations: item.Observations,
                SamplerNumber: item.SamplerNumber,
                Date: item.Date,
                Included: item.Included,
                ChangeDate: item.ChangeDate,
                ClinicId: item.ClinicId,
                Doctors: item.Doctors
            }
            return employee;
        });

        if (GeneralData.userRoleId !== Constants.role.ContactPerson) {
            var error = "<ul>";
            for (var i = 0; i < changes.length; i++) {
                if (changes[i].Included === true) {
                    if (changes[i].ChangeDate === false) {
                        error += kendo.format("<li>{0}: Tienes que elegir un Día (Fecha/Hora)</li>",
                            changes[i].EmployeeName);
                    }
                    if (changes[i].ClinicId === 0) {
                        error += kendo.format("<li>{0}: Tienes que elegir una Clínica</li>", changes[i].EmployeeName);
                    }
                }
            }
            if (error !== "<ul>") {
                error += "</ul>";
                GeneralData.showNotification(error, "", "error");
                return;
            }
        }

        $.ajax({
            url: "/HistoricMedicalExamination/UpdateRequestHistoricMedicalExaminationEmployees",
            data: JSON.stringify({ "listEmployees": changes }),
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.resultStatus === Constants.resultStatus.Ok) {
                    var dataItem = HistoricMedicalExamination.historicRequestMedicalExaminationsDataSource.get(response.requestMedicalExamination.Id);
                    dataItem.RequestMedicalExaminationStateId = response.requestMedicalExamination.RequestMedicalExaminationStateId;
                    dataItem.RequestMedicalExaminationStateDescription = response.requestMedicalExamination.RequestMedicalExaminationStateDescription;

                    var gridRequestHistoricMedicalExaminations =
                        $("#" + HistoricMedicalExamination.gridRequestHistoricMedicalExaminationsId).data("kendoGrid");
                    var expanded = $.map(gridRequestHistoricMedicalExaminations.tbody.children(":has(> .k-hierarchy-cell .k-i-collapse)"), function (row) {
                        return $(row).data("uid");
                    });

                    gridRequestHistoricMedicalExaminations.one("dataBound", function () {
                        gridRequestHistoricMedicalExaminations.expandRow(gridRequestHistoricMedicalExaminations.tbody.children().filter(function (idx, row) {
                            return $.inArray($(row).data("uid"), expanded) >= 0;
                        }));
                    });
                    gridRequestHistoricMedicalExaminations.refresh();

                    GeneralData.showNotification(Constants.ok, "", "success");
                } else {
                    GeneralData.showNotification(Constants.ko, "", "error");
                }
            }
        });
    },

    createClinicsDataSource: function () {
        HistoricMedicalExamination.clinicsDataSorce = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/HistoricMedicalExamination/GetClinics",
                    dataType: "jsonp"
                }
            }
        });

        HistoricMedicalExamination.clinicsDataSorce.read();
    },

    createDoctorsDataSource: function () {
        HistoricMedicalExamination.doctorsDataSorce = new kendo.data.DataSource({
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Id: { type: "number" },
                        Name: { type: "string" }
                    }
                }
            },
            transport: {
                read: {
                    url: "/HistoricMedicalExamination/GetDoctors",
                    dataType: "jsonp"
                }
            }
        });

        HistoricMedicalExamination.doctorsDataSorce.read();
    },

    clinicsDropDownEditor: function (container, options) {
        $("<input required name='" + options.field + "'/>")
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Name",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: HistoricMedicalExamination.clinicsDataSorce,
                dataBound: function (e) {
                    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                }
            });
    },

    doctorsDropDownEditor: function (container, options) {
        //$("<input required name='" + options.field + "'/>")
        $("<input name='" + options.field + "'/>")
            .appendTo(container)
            .kendoMultiSelect({
                dataTextField: "Name",
                optionLabel: "Selecciona ...",
                dataValueField: "Id",
                dataSource: HistoricMedicalExamination.doctorsDataSorce,
                dataBound: function (e) {
                    e.sender.list.width("auto").find("li").css({ "white-space": "nowrap", "padding-right": "25px" });
                },
                change: function (e) {
                    var grid = $("#gridEmployeesMedicalExamination").data("kendoGrid");
                    var selectedItem = grid.dataItem(grid.select());

                    var doctors = "";
                    for (var i = 0; i < this.dataItems().length; i++) {
                        doctors += this.dataItems()[i].Id + ",";
                    }
                    if (doctors.match(",$")) {
                        doctors = doctors.slice(0, -1);
                    }
                    selectedItem.Doctors = doctors;
                    //selectedItem.set("Doctors", doctors);
                }
            });
    },

    getClinicName: function (clinicId) {
        if (HistoricMedicalExamination.clinicsDataSorce.data().length === 0) {
            HistoricMedicalExamination.clinicsDataSorce.read();
        }
        for (var index = 0; index < HistoricMedicalExamination.clinicsDataSorce.data().length; index++) {
            if (HistoricMedicalExamination.clinicsDataSorce.data()[index].Id === clinicId) {
                return HistoricMedicalExamination.clinicsDataSorce.data()[index].Name;
            }
        }
        return "";
    },

    getDoctorsName: function (doctors) {
        if (doctors === null)
            return "";

        var splitDoctors = doctors.split(",");
        if (HistoricMedicalExamination.doctorsDataSorce.data().length === 0) {
            HistoricMedicalExamination.doctorsDataSorce.read();
        }

        var doctorsName = "";
        for (var indexDoctor = 0; indexDoctor < HistoricMedicalExamination.doctorsDataSorce.data().length; indexDoctor++) {
            for (var index = 0; index < splitDoctors.length; index++) {
                if (HistoricMedicalExamination.doctorsDataSorce.data()[indexDoctor].Id === parseInt(splitDoctors[index])) {
                    doctorsName += HistoricMedicalExamination.doctorsDataSorce.data()[indexDoctor].Name + ", ";
                }
            }
        }
        if (doctorsName.match(", $")) {
            doctorsName = doctorsName.slice(0, -2);
        }

        return doctorsName;
    }
});